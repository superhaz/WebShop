using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebShop.DAL;
using System.Drawing;
using System.IO;
using WebShop.BLL;
using System.Web.Configuration;
using WebShop.DAL.Interfaces;
using Ninject.Web;
using Ninject;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class _Default : PageBase
    {
        [Inject]
        public ICategoryData CategoryDataComponent { get; set; }
        [Inject]
        public IProductData ProductDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        private string languageID = "1";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindControls();
        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }

        private void BindControls()
        {
            BindCategoryItemImage();
            BindProductRepeaterList();
        }

        private void BindProductRepeaterList()
        {
            ItemRepeater.DataSource = ProductDataComponent.GetLatestProducts(languageID, 6);
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
                    Response.Redirect("~/ProductView.aspx?LangID=" + languageID + "&CatID=" + categoryID + "&ID=" + productID);
                }
            }
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string folderUrl = "~/ProductImages/";  // to be refactorized
            Literal priceLiteral = e.Item.FindControl("PriceLiteral") as Literal;
            priceLiteral.Text = UtilHelperComponent.RoundPrice(priceLiteral.Text);
            BindItemImage(e.Item, folderUrl);
        }

        public void BindCategoryItemImage()
        {
            string folderUrl = "~/Admin/CategoryImages/";  // to be refactorized

            DataTable table = CategoryDataComponent.GetCategoryNameFiles(languageID);

            if (table != null && table.Rows.Count > 0)
            {
                int randomRow = UtilHelperComponent.RandomNumber(table.Rows.Count - 1);

                DataRow row = table.Rows[randomRow];
                CategoryIDHiddenField.Value = row["CategoryID"].ToString();
                SetImageSize(MainCategoryImageButton, row["fileName"].ToString(), 630, 450, folderUrl);
            }
            else // no image available
            {
                SetImageSize(MainCategoryImageButton, "", 630, 450, folderUrl);
            }
        }

        protected void MainCategoryImageButton_Click(object sender, EventArgs e)
        {
            // check which linkbutton was presseed
            if (!string.IsNullOrEmpty(CategoryIDHiddenField.Value))
            {
                Response.Redirect("~/GroupView.aspx?CatID=" + CategoryIDHiddenField.Value);
            }

        }

        /// <summary>
        /// Initialized and set the images size for hovering with javascript
        /// </summary>
        private void BindItemImage(RepeaterItem item, string folderUrl)
        {
            System.Web.UI.WebControls.Image itemImage = item.FindControl("ItemImage") as System.Web.UI.WebControls.Image;
            string filename = (item.FindControl("FileNameHiddenField") as HiddenField).Value;
            
            // default image
            SetImageSize(itemImage, filename, 150, 150, folderUrl);
            // large image
            //SetImageSize(LargeItemImage, imageUrl, 250, 250);

            // add javascript attributes
            //itemImage.Attributes.Add("onmouseover", "HighlightImage('ItemImageDiv')");
            //itemImage.Attributes.Add("onmouseout", "RemoveImageHighlight('ItemImageDiv')");
            //itemImage.Attributes.Add("onclick", "ShowLargeImage('LargeImageDiv')");
        }



        private void SetImageSize(System.Web.UI.WebControls.Image imageControl, string filename, int imageWidth, int imageHeight, string folderUrl)
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
    }
}
