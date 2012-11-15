using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebShop.DAL;
using System.Drawing;
using WebShop.BLL;
using WebShop.UserControls.FileImageManager.DAL;
using WebShop.UserControls.FileImageManager.BLL;
using System.IO;
using System.Data.SqlClient;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;
using WebShop.UserControls.FileImageManager.DAL.Factory;
using System.Web.Security;
using System.Web.Configuration;
using Ninject.Web;
using Ninject;
using WebShop.DAL.Interfaces;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class GroupView : PageBase
    {
        [Inject]
        public ICategoryData CategoryDataComponent { get; set; }
        [Inject]
        public IProductData ProductDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        #region Variables, PageLoad & Init

        private string selectedCatID = "1";
        private string selectedLangID = "1";
        string folderUrl = "~/ProductImages/";  // to be refactorized

        // Provider members

        /// <summary>
        /// The provider when used in for example ProductView or CategoryView is simply
        /// for doing DELETE operation on a product. 
        /// In ProductView: When canceling a New Product, the product and its possible 
        /// images needs to be deleted.
        /// In GroupView: When deleting a product, all images needs to be deleted.
        /// </summary>
        private IItemDataProvider provider;
        public IItemDataProvider Provider
        {
            get
            {
                if (provider == null)
                    provider = ItemDataProviderFactory.CreateProvider(ItemDataProviderFactory.ItemDataProviderType.ProductProvider);

                return provider;
            }
        }
        // end of Provider members

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();

            if (!IsPostBack)
            {
                InitiateMode();
            }
        }

        private void LoadParameters()
        {
            // save parameter values
            if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
                selectedCatID = Request.QueryString["CatID"];
            else
                Response.Redirect("~/Default.aspx");

            if (!String.IsNullOrEmpty(Request.QueryString["LangID"]))
                selectedLangID = Request.QueryString["LangID"];

        }

        private void InitiateMode()
        {
            if (!String.IsNullOrEmpty(selectedCatID))
            {
                SetCategoryInfo();
                BindRepeater();
            }
        }

        private void SetCategoryInfo()
        {
            DataRow row = CategoryDataComponent.GetCategoryInfo(selectedCatID, selectedLangID);
            if (row != null)
            {
                string categoryTitle = row["CategoryName"].ToString();
                string categoryShortInfo = row["ShortInfo"].ToString();
                Title = categoryTitle + " - " + WebConfigurationManager.AppSettings["CompanyName"];
                CategoryTitleLabel.Text = categoryTitle;
                CategoryShortInfoLabel.Text = UtilHelperComponent.ParseNewLine(categoryShortInfo);
            }
        }

        #endregion

        #region Repeater

        private void BindRepeater()
        {
            if (String.IsNullOrEmpty(selectedCatID))
                return;

            if (Roles.IsUserInRole(UtilHelper.Roles.Administrators.ToString()) || Roles.IsUserInRole(UtilHelper.Roles.Administrators.ToString()))
                ItemRepeater.DataSource = ProductDataComponent.GetProductsByCategoryID(selectedCatID, selectedLangID, false);
            else
                ItemRepeater.DataSource = ProductDataComponent.GetProductsByCategoryID(selectedCatID, selectedLangID, true);
            ItemRepeater.DataBind();
        }

        protected void ItemRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string categoryID = (e.Item.FindControl("CategoryIDHiddenField") as HiddenField).Value;
            string productID = (e.Item.FindControl("ProductIDHiddenField") as HiddenField).Value;
            string productCategoryID = (e.Item.FindControl("ProductCategoryIDHiddenField") as HiddenField).Value;

            if (!string.IsNullOrEmpty(categoryID) && !string.IsNullOrEmpty(productID) && !string.IsNullOrEmpty(productCategoryID))
            {
                if (e.CommandName.ToString() == "View")
                {
                    Response.Redirect("~/ProductView.aspx?LangID=" + selectedLangID + "&CatID=" + selectedCatID + "&ID=" + productID);
                }
                // check which linkbutton was presseed
                else if (e.CommandName.ToString() == "Delete")
                {
                    // Delete file and update database
                    Delete(e);
                }
                else if (e.CommandName.ToString() == "IncreasePriority" )
                {
                    // Increase priority
                    ProductDataComponent.IncreasePriority(productCategoryID, categoryID);
                }
                else if (e.CommandName.ToString() == "DecreasePriority")
                {
                    // Decrease priority
                    ProductDataComponent.DecreasePriority(productCategoryID, categoryID);
                }
                else if (e.CommandName.ToString() == "HighestPriority")
                {
                    // set highest prio for the current product
                    SetHighestPrio(productCategoryID, categoryID);
                }
                else if (e.CommandName.ToString() == "LowestPriority")
                {
                    // set lowest prio for the current product
                    SetLowestPrio(productCategoryID, categoryID);
                }

                BindRepeater();
            }
        }

        private void Delete(RepeaterCommandEventArgs e)
        {
            string productID = (e.Item.FindControl("ProductIDHiddenField") as HiddenField).Value;

            // deletes all files connected to the product
            FileHelper.DeleteItemFiles(productID, Provider.GetFilesByItemID(productID));
            ProductDataComponent.DeleteProduct(productID);

            // Resort priority for files
            BindRepeater();
            ResortPriority();
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal priceLiteral = e.Item.FindControl("PriceLiteral") as Literal;
            priceLiteral.Text = UtilHelperComponent.RoundPrice(priceLiteral.Text);
            BindItemImage(e.Item);
        }

        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            string selectedProdID = ProductDataComponent.SaveNewProduct();
            Response.Redirect("~/ProductView.aspx?LangID=" + selectedLangID + "&CatID=" + selectedCatID + "&ID=" + selectedProdID);
        }

        #endregion

        #region Priority Management

        private void SetHighestPrio(string productCategoryID, string categoryID)
        {
            // change current product prio to highest
            string highestPrio = ProductDataComponent.GetANewHighestPriority(categoryID);
            // set the current product to highest prio
            ProductDataComponent.SetPriorityByProductCategoryID(productCategoryID, highestPrio);
            // create a bind to repeater
            BindRepeater();
            // make a resort since current product has been moved from its priority
            ResortPriority();
            // finally update the repeater to get the final resort
            BindRepeater();
        }

        private void SetLowestPrio(string productCategoryID, string categoryID)
        {
            // change current product prio to lowest
            string lowestPrio = ProductDataComponent.GetANewLowestPriority(categoryID);
            // set the product to lowest prio
            ProductDataComponent.SetPriorityByProductCategoryID(productCategoryID, lowestPrio);
            // create a bind to repeater
            BindRepeater();
            // make a resort once again since current product has been moved from its priority
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

                    string productCategoryID = (item.FindControl("ProductCategoryIDHiddenField") as HiddenField).Value;
                    ProductDataComponent.SetPriorityByProductCategoryID(productCategoryID, priority.ToString());

                    priority++;
                }
            }
        }

        #endregion

        #region Binding Image

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
            if (string.IsNullOrEmpty(selectedCatID))
                return;

            System.Web.UI.WebControls.Image itemImage = item.FindControl("ItemImage") as System.Web.UI.WebControls.Image;
            string filename = (item.FindControl("FileNameHiddenField") as HiddenField).Value;
            //string filesize = (item.FindControl("FileSizeHiddenField") as HiddenField).Value;
            string priority = (item.FindControl("PriorityHiddenField") as HiddenField).Value;

            if(Request.IsAuthenticated)
                itemImage.AlternateText = itemImage.ToolTip = "prioritet: " + priority;

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
            string imageUrl = folderUrl + filename;
            string imageUrlRoot = Server.MapPath(folderUrl + filename);
            //-----------NOT USED - ONLY FOR CLARIFICATION----------------------------

            if (File.Exists(Server.MapPath(folderUrl + filename)))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(folderUrl + filename));
                imageSize = ScaleImageSize(img, imageWidth, imageHeight);
            }
            else
            {
                filename = "no_image.jpg";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(folderUrl + filename));
                imageSize = ScaleImageSize(img, imageWidth, imageHeight);
            }

            imageControl.Height = imageSize.Height;
            imageControl.Width = imageSize.Width;
            imageControl.ImageUrl = folderUrl + filename;
        }

        #region deprecated scale of image siz
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
    }
}
