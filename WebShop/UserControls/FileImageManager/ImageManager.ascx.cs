using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.UserControls.FileImageManager.BLL;
using System.IO;
using WebShop.UserControls.FileImageManager.DAL;
using System.Drawing;
using System.Data;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;
using WebShop.UserControls.FileImageManager.DAL.Factory;
using WebShop.BLL;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WebShop.UserControls.FileImageManager
{
    public partial class ImageManager : System.Web.UI.UserControl
    {
        #region members & properties

        private string filename;
        private string priority;
        private int numImages;

        // the ID used for connecting a file with an item for example ProductID is a type ItemID
        public string ItemID
        {
            get
            {
                string itemID = (string)ViewState["ItemID"];
                if (itemID != null)
                    return itemID;
                else
                    return string.Empty;
            }
            set
            {
                ViewState["ItemID"] = value;
            }
        }

        private string maxNumberOfImages;
        public string MaxNumberOfImages
        {
            get
            {
                return maxNumberOfImages;
            }
            set
            {
                maxNumberOfImages = value;
            }
        }

        // The folder Url where the file exists. Ex "~/ProductImages"
        public string FolderUrl
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("The FolderUrl property tag is missing or has an empty value");

                FileHelper.FolderUrl = value;
                FileHelper.FolderUrlRoot = Server.MapPath(value);
            }
        }

        #endregion

        #region Provider

        // Provider members
        // used for setting type in markup code ex:  <shaz:ImageManager ID="ImageManager1" runat="server" FolderUrl="~/ProductImages"ItemDataProviderType="ProductProvider" />
        public ItemDataProviderFactory.ItemDataProviderType ItemDataProviderType { get; set; }

        // property for provider
        private IItemDataProvider provider;
        public IItemDataProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    // creates a new provider
                    provider = ItemDataProviderFactory.CreateProvider(ItemDataProviderType);
                }

                return provider;
            }
        }
        // end of Provider members

        #endregion

        #region ImageControlMode

        public enum ImageControlMode { ImageViewingMode = 1, ImagesListingMode, AddNewImageMode, ImagesListModeOnly }
        protected ImageControlMode Mode
        {
            get
            {
                if (!String.IsNullOrEmpty((string)ViewState["ImageMode"]))
                {
                    string str = (string)ViewState["ImageMode"];
                    return (ImageControlMode)Enum.Parse(typeof(ImageControlMode), (string)ViewState["ImageMode"], true); // converts a string to enum
                }
                else
                {
                    string str = (string)ViewState["ImageMode"];
                    ViewState["ImageMode"] = ImageControlMode.ImageViewingMode.ToString();
                    return ImageControlMode.ImageViewingMode;
                }
            }
            set
            {
                string str = value.ToString();
                ViewState["ImageMode"] = value.ToString();
            }
        }

        /// <summary>
        /// Userd as property for setting the startup init mode in markup code
        /// </summary>
        public ImageControlMode InitImageMode
        {
            get
            {
                if (!String.IsNullOrEmpty((string)ViewState["InitImageMode"]))
                {
                    string str = (string)ViewState["InitImageMode"];
                    return (ImageControlMode)Enum.Parse(typeof(ImageControlMode), (string)ViewState["InitImageMode"], true); // converts a string to enum
                }
                else
                {
                    string str = (string)ViewState["InitImageMode"];
                    ViewState["InitImageMode"] = ImageControlMode.ImageViewingMode.ToString();
                    return ImageControlMode.ImageViewingMode;
                }
            }
            set
            {
                string str = value.ToString();
                ViewState["InitImageMode"] = value.ToString();
            }
        }

        #endregion

        #region PageLoad & Binding

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // TODO: verify initiated folderURL is set
                string validate = FileHelper.FolderUrl;

                InitiateMode();
                InitiateViewState();

            }
        }

        private void InitiateViewState()
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                    ItemID = Request.QueryString["ID"];
            }
        }

        private void InitiateMode()
        {
            // set the init mode
            Mode = InitImageMode;
            SwitchMode();
        }

        /// <summary>
        /// Binds the ImageManager to an itemID, this method is usually called from another parent control such as ProductView.aspx.cs
        /// </summary>
        /// <param name="itemID"></param>
        public void BindControls(string itemID)
        {
            ItemID = itemID;
            SwitchMode();
        }

        private void SwitchMode()
        {
            if (Mode == ImageControlMode.ImageViewingMode)
            {
                // bind image 
                BindItemImage();

                // set controls visibility
                // ImageViewingMode controls
                SwitchToImagesListingPlaceHolder.Visible = true;
                ImageViewingPlaceHolder.Visible = true;
                // ImageListingMode controls
                SwitchToAddNewImagePlaceHolder.Visible = false;
                SwitchToImageViewingPlaceHolder.Visible = false;
                ImagesListingPlaceHolder.Visible = false;
                ImagesListingRepeaterPlaceHolder.Visible = false;
                // AddNewImageMode controls
                AddNewImagePlaceHolder.Visible = false;

            }
            else if (Mode == ImageControlMode.ImagesListingMode || Mode == ImageControlMode.ImagesListModeOnly)
            {
                // bind repeater with images
                BindRepeater();

                // set controls visibility
                // ImageViewingMode controls
                SwitchToImagesListingPlaceHolder.Visible = false;
                ImageViewingPlaceHolder.Visible = false;
                // ImageListingMode controls
                SwitchToAddNewImagePlaceHolder.Visible = true;
                SwitchToImageViewingPlaceHolder.Visible = true;
                ImagesListingPlaceHolder.Visible = true;
                ImagesListingRepeaterPlaceHolder.Visible = true;
                // AddNewImageMode controls
                AddNewImagePlaceHolder.Visible = false;

                // if ImagesListModeOnly is set then hide SwitchToImageViewing button
                if (InitImageMode == ImageControlMode.ImagesListModeOnly)
                    SwitchToImageViewingPlaceHolder.Visible = false;

                // prevent from uploading more than max number of images
                if (!string.IsNullOrEmpty(MaxNumberOfImages) && new UtilHelper().IsInteger(MaxNumberOfImages))
                {
                    int maxNumber = Convert.ToInt32(MaxNumberOfImages);
                    // hide switchTo button
                    if (ItemRepeater.Items.Count >= maxNumber)
                        SwitchToAddNewImagePlaceHolder.Visible = false;
                    else
                        SwitchToAddNewImagePlaceHolder.Visible = true;
                }
            }
            else if (Mode == ImageControlMode.AddNewImageMode)
            {
                // set controls visibility
                // ImageViewingMode controls
                SwitchToImagesListingPlaceHolder.Visible = false;
                ImageViewingPlaceHolder.Visible = false;
                // ImageListingMode controls
                SwitchToAddNewImagePlaceHolder.Visible = false;
                SwitchToImageViewingPlaceHolder.Visible = false;
                ImagesListingPlaceHolder.Visible = false;
                ImagesListingRepeaterPlaceHolder.Visible = false;
                // AddNewImageMode controls
                AddNewImagePlaceHolder.Visible = true;
            }
        }

        private void BindItemImage()
        {
            DataTable table = Provider.GetFileByItemIDANDPriority(ItemID, priority);
            if (table != null && table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                PriorityHiddenField.Value = row["Priority"].ToString();
                SetImageSize(ItemImageButton, row["fileName"].ToString(), 630, 450);
            }
            else // no image available
            {
                SetImageSize(ItemImageButton, "", 630, 450);
            }
        }

        #endregion

        #region Repeater

        private void BindRepeater()
        {
            if (String.IsNullOrEmpty(ItemID))
                return;


            DataTable table = Provider.GetFilesByItemID(ItemID);
            numImages = table.Rows.Count;
            ItemRepeater.DataSource = table;
            ItemRepeater.DataBind();
        }


        protected void ItemRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string itemID = (e.Item.FindControl("ItemIDHiddenField") as HiddenField).Value;
            string itemFileID = (e.Item.FindControl("ItemFileIDHiddenField") as HiddenField).Value;

            // check which linkbutton was presseed
            if (e.CommandName.ToString() == "Delete")
            {
                // Delete file and update database
                Delete(e);
            }
            else if (e.CommandName.ToString() == "IncreasePriority")
            {
                // Increase priority
                Provider.IncreasePriority(itemFileID, itemID);
            }
            else if (e.CommandName.ToString() == "DecreasePriority")
            {
                // Decrease priority
                Provider.DecreasePriority(itemFileID, itemID);
            }
            else if (e.CommandName.ToString() == "HighestPriority")
            {
                // set highest prio for the current Item
                SetHighestPrio(itemFileID, itemID);
            }
            else if (e.CommandName.ToString() == "LowestPriority")
            {
                // set lowest prio for the current Item
                SetLowestPrio(itemFileID, itemID);
            }

            SwitchMode();
        }

        private void Delete(RepeaterCommandEventArgs e)
        {
            string filename = (e.Item.FindControl("FileNameHiddenField") as HiddenField).Value;
            string fileID = (e.Item.FindControl("FileIDHiddenField") as HiddenField).Value;
            bool result = FileHelper.DeleteFile(filename);

            if (result)
                FileData.DeleteFile(fileID);
            else
            {
                // remove dependency between item and file
                Provider.DeleteItemFileByFileID(fileID);
                // mark the file for deletion
                FileData.MarkFileForDeletion(fileID);
            }

            // Resort priority for files
            BindRepeater(); // rebind repeater without deleted file then make a resort priority
            ResortPriority();
        }



        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            BindItemImage(e.Item);

            // hide sorting functionality if there are only one image
            if (numImages <= 1)
            {
                (e.Item.FindControl("SortingPlaceHolder") as PlaceHolder).Visible = false;
            }
            else
            {
                (e.Item.FindControl("SortingPlaceHolder") as PlaceHolder).Visible = true;
            }
        }


        #endregion

        #region Priority Management

        private void SetHighestPrio(string itemFileID, string itemID)
        {
            // change current Item prio to highest
            string highestPrio = Provider.GetANewHighestPriority(itemID);
            // set the current Item to lowest prio
            Provider.SetPriorityByItemFileID(itemFileID, highestPrio);
            // create a bind to repeater
            BindRepeater();
            // make a resort since current Item has been moved from its priority
            ResortPriority();
            // finally update the repeater to get the final resort
            BindRepeater();
        }

        private void SetLowestPrio(string itemFileID, string itemID)
        {
            // change current Item prio to highest
            string highestPrio = Provider.GetANewLowestPriority(itemID);
            // set the current Item to lowest prio
            Provider.SetPriorityByItemFileID(itemFileID, highestPrio);
            // create a bind to repeater
            BindRepeater();
            // make a resort once again since current Item has been moved from its priority
            ResortPriority();
            // finally update the repeater to get the final resort
            BindRepeater();
        }

        private void ResortPriority()
        {
            ResortPriority(1);  // make a resort without any priority step
        }

        /// <summary>
        /// sort the priority in order
        /// </summary>
        /// <param name="priority">the starting priority</param>
        private void ResortPriority(int priorityStep)
        {
            int priority = priorityStep;
            foreach (RepeaterItem item in ItemRepeater.Items)
            {
                if (item != null)
                {

                    string itemFileID = (item.FindControl("ItemFileIDHiddenField") as HiddenField).Value;
                    Provider.SetPriorityByItemFileID(itemFileID, priority.ToString());

                    priority++;
                }
            }
        }

        #endregion

        #region binding image

        //public void BindItemImageManually()
        //{
        //    ItemRepeaterPlaceHolder.Visible = true;
        //    SavePlaceHolder.Visible = false;
        //    BindItemImage();
        //}

        /// <summary>
        /// Initialized and set the images size for hovering with javascript
        /// </summary>
        private void BindItemImage(RepeaterItem item)
        {
            if (string.IsNullOrEmpty(ItemID))
                return;

            System.Web.UI.WebControls.Image itemImage = item.FindControl("ItemImage") as System.Web.UI.WebControls.Image;
            string filename = (item.FindControl("FileNameHiddenField") as HiddenField).Value;
            string filesize = (item.FindControl("FileSizeHiddenField") as HiddenField).Value;
            string priority = (item.FindControl("PriorityHiddenField") as HiddenField).Value;

            itemImage.AlternateText = itemImage.ToolTip = "namn: " + filename + "\nstorlek: " + FileHelper.ConvertSizeToKBOrMBOrGBOrTB(filesize) + "\nprioritet: " + priority;

            // default image
            SetImageSize(itemImage, filename, 150, 150);
            // large image
            //SetImageSize(LargeItemImage, imageUrl, 250, 250);

            // add javascript attributes
            //itemImage.Attributes.Add("onmouseover", "HighlightImage('ItemImageDiv')");
            //itemImage.Attributes.Add("onmouseout", "RemoveImageHighlight('ItemImageDiv')");
            //itemImage.Attributes.Add("onclick", "ShowLargeImage('LargeImageDiv')");
        }

        private void SetImageSize(System.Web.UI.WebControls.Image imageControl, string filename, int imageWidth, int imageHeight)
        {
            Size imageSize = new Size();
            // verify that file exists

            //-----------NOTE! Make difference between:-------------------------------
            string imageUrl = FileHelper.FolderUrl + filename;
            string imageUrlRoot = Server.MapPath(FileHelper.FolderUrl + filename);
            //-----------NOT USED - ONLY FOR CLARIFICATION----------------------------

            if (File.Exists(Server.MapPath(FileHelper.FolderUrl + filename)))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(FileHelper.FolderUrl + filename));
                imageSize = ScaleImageSize(img, imageWidth, imageHeight);
            }
            else
            {
                filename = "no_image.jpg";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(FileHelper.FolderUrl + filename));
                imageSize = ScaleImageSize(img, imageWidth, imageHeight);
            }

            imageControl.Height = imageSize.Height;
            imageControl.Width = imageSize.Width;
            imageControl.ImageUrl = FileHelper.FolderUrl + filename;
        }

        #region deprecated scale of image size
        public Size SizeImage(System.Drawing.Image img, int MaxWidth, int MaxHeight)
        {
            if (img.Width > MaxWidth || img.Height > MaxHeight)
            {
                double widthRatio = (double)img.Width / (double)MaxWidth;
                double heightRatio = (double)img.Height / (double)MaxHeight;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(img.Width / ratio);
                int newHeight = (int)(img.Height / ratio);
                Size size = new Size(newWidth, newHeight);
                return size;
            }
            else
            {
                Size size = new Size(img.Width, img.Height);
                return size;
            }
        }
        #endregion

        public Size ScaleImageSize(System.Drawing.Image img, int MaxWidth, int MaxHeight)
        {
            double reductionPercentage;
            double width = img.Width;
            double height = img.Height;

            if (width > MaxWidth || height > MaxHeight)
            {
                if (width > MaxWidth)
                {
                    // calculate the reduction scale percent
                    reductionPercentage = ((double)MaxWidth / (double)img.Width);
                    // calculate new width and height from percentage
                    width = img.Width * reductionPercentage;
                    height = img.Height * reductionPercentage;
                }

                if (height > MaxHeight)
                {
                    // calculate the reduction scale percent
                    reductionPercentage = ((double)MaxHeight / height);
                    // calculate new width and height from percentage
                    width = width * reductionPercentage;
                    height = height * reductionPercentage;
                }

                Size size = new Size((int)width, (int)height);
                return size;
            }
            else
            {
                Size size = new Size((int)width, (int)height);
                return size;
            }
        }

        #endregion

        #region button events

        protected void AddNewImageButton_Click(object sender, EventArgs e)
        {
            // set mode and switch
            Mode = ImageControlMode.AddNewImageMode;
            SwitchMode();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            // set mode and switch
            Mode = ImageControlMode.ImagesListingMode;
            SwitchMode();
        }

        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            // set mode and switch
            Mode = ImageControlMode.ImageViewingMode;
            SwitchMode();
        }

        protected void ForwardImageButton_Click(object sender, EventArgs e)
        {
            int priority;

            if (!string.IsNullOrEmpty(PriorityHiddenField.Value) && FileHelper.IsInteger(PriorityHiddenField.Value))
            {
                priority = Convert.ToInt32(PriorityHiddenField.Value);
                priority++;
                this.priority = priority.ToString();
                BindItemImage();
            }

        }

        protected void BackImageButton_Click(object sender, EventArgs e)
        {
            int priority;

            if (!string.IsNullOrEmpty(PriorityHiddenField.Value) && FileHelper.IsInteger(PriorityHiddenField.Value))
            {
                priority = Convert.ToInt32(PriorityHiddenField.Value);
                priority--;
                this.priority = priority.ToString();
                BindItemImage();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveNewFile();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // set mode and switch
            Mode = ImageControlMode.ImagesListingMode;
            SwitchMode();
        }

        public void SaveNewFile()
        {

            if (FileUpload1.HasFile && Page.IsValid)
            {
                try
                {
                    // make some common cleanup procedure
                    FileHelper.CleanupMarkedFilesForDeletion();

                    // get filename
                    filename = Path.GetFileName(FileUpload1.FileName);
                    filename = EncodeFileName(filename);
                     filename = FileHelper.FileNameWithoutExtension(filename) + ".jpg";
                    // rename file if necessary
                    filename = FileHelper.GetUniqueFileName(filename);
                    // upload new file
                    byte[] imageByte = ResizeImageFile(FileUpload1.FileBytes, 630, ImageFormat.Jpeg);
                    int filesize;
                    // saves image to file
                    ByteArrayToFile(FileHelper.FolderUrlRoot + filename, imageByte, out filesize);
                    //FileUpload1.SaveAs(FileHelper.FolderUrlRoot + filename);
                    // save new image-url to database
                    string fileID = FileData.SaveFile(filename, filesize);
                    //string fileID = FileData.SaveFile(filename, FileUpload1.PostedFile.ContentLength);
                    if (!string.IsNullOrEmpty(fileID))
                        Provider.SaveItemFile(fileID, ItemID);

                    // set mode and switch
                    Mode = ImageControlMode.ImagesListingMode;
                    SwitchMode();
                }
                catch (Exception ex) { throw ex; }
            }
        }

        protected void FileExtension_CustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    // get filename
                    filename = Path.GetFileName(FileUpload1.FileName);
                    if (FileHelper.ValidExtension(filename))
                        e.IsValid = true; // ok
                    else
                        e.IsValid = false;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        private string EncodeFileName(string filename)
        {
            // remove whitespace, å ä ö
            return filename.ToLower().Replace(" ", "_").Replace("å", "a").Replace("ä", "a").Replace("ö", "o");
        }

        #region Resize and upload image byte[]

        // see also: http://snippets.dzone.com/posts/show/1485
        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            System.Drawing.Image original = System.Drawing.Image.FromStream(new MemoryStream(imageFile));
            int targetH, targetW;
            if (original.Height < targetSize || original.Width < targetSize) // if image is smaller then defined target size keep original size for image
            {
                targetH = original.Height;
                targetW = original.Width;
            }
            else if (original.Height > original.Width)
            {
                targetH = targetSize;
                targetW = (int)(original.Width * ((float)targetSize / (float)original.Height));
            }
            else
            {
                targetW = targetSize;
                targetH = (int)(original.Height * ((float)targetSize / (float)original.Width));
            }
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(new MemoryStream(imageFile));
            // Create a new blank canvas.  The resized image will be drawn on this canvas.
            Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
            // make image transparent
            bmPhoto.MakeTransparent();
            bmPhoto.SetResolution(72, 72);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //grPhoto.Clear(Color.White); // removes black background
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
            MemoryStream memoryStream = new MemoryStream();
            bmPhoto.Save(memoryStream, imgFormat);
            original.Dispose();
            imgPhoto.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            return memoryStream.GetBuffer();
        }

        /// <summary>  
        /// Function to save byte array to a file   
        /// See also: http://www.digitalcoding.com/Code-Snippets/C-Sharp/C-Code-Snippet-Save-byte-array-to-file.html
        /// </summary>  
        /// <param name="_FileName">File name to save byte array</param>  
        /// <param name="_ByteArray">Byte array to save to external file</param>  
        /// <returns>Return true if byte array save successfully, if not return false</returns>  
        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray, out int fileSize)
        {
            try
            {
                // Open file for reading  
                System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from a byte array.  
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);
                fileSize = Convert.ToInt32(_FileStream.Length);
                // close file stream  
                _FileStream.Close();
                return true;
            }
            catch (Exception _Exception)
            {
                // Error  
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            fileSize = 0;
            // error occured, return false  
            return false;
        }

        #endregion

        #endregion

        #region future code

        //protected void DownloadFile(string fname, bool forceDownload)
        //{
        //    fname = "~/" + fname;
        //    string path = MapPath(fname);
        //    string name = Path.GetFileName(path);
        //    string ext = Path.GetExtension(path);
        //    string type = "";
        //    // set known types based on file extension  
        //    if (ext != null)
        //    {
        //        switch (ext.ToLower())
        //        {
        //            case ".htm":
        //            case ".html":
        //                type = "text/HTML";
        //                break;

        //            case ".txt":
        //                type = "text/plain";
        //                break;

        //            case ".doc":
        //            case ".rtf":
        //                type = "Application/msword";
        //                break;

        //            case ".pdf":
        //                type = "application/pdf";
        //                break;
        //        }
        //    }
        //    if (forceDownload)
        //    {
        //        Response.AppendHeader("content-disposition",
        //            "attachment; filename=" + name);
        //    }
        //    if (type != "")
        //        Response.ContentType = type;
        //    Response.WriteFile(path);
        //    Response.End();
        //} 

        #endregion
    }
}

