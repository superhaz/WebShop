using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.DAL;
using WebShop.BLL;
using System.Web.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using Ninject.Web;
using Ninject;
using WebShop.DAL.Interfaces;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class MasterPage : MasterPageBase
    {
        [Inject]
        public ITextBlockData TextBlockDataComponent { get; set; }
        [Inject]
        public IOrderData OrderDataComponent { get; set; }
        [Inject]
        public ICategoryData CategoryDataComponent { get; set; }
        [Inject]
        public IProductData ProductDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        #region Member fields and Enum

        // the title for the site
        private string SiteTitleTest = WebConfigurationManager.AppSettings["SiteTitleTest"];
        private string SiteTitleProd = WebConfigurationManager.AppSettings["SiteTitleProd"];

        private string _siteTitle;
        private Environment _environment;

        private enum Environment
        {
            Development,
            Test,
            Production
        }

        private string languageID = "1";
        private string orderID;

        public string LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        private string cookieName = WebConfigurationManager.AppSettings["CookieName"];

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            LoadShoppingCart();

            if (!IsPostBack)
                LoadControls();

            // set site environment and version
            SiteTitleLiteral.Text = _siteTitle;
        }

        #region Production mode & Version
        /*
         * NOTE! Setting the production mode needs to be done before all other content and user controls Page_Load is executed
         * Events in ASP.NET Master and Content Pages
         * http://msdn.microsoft.com/en-us/library/dct97kc3.aspx
         * 
         * 1. Content page PreInit
         * 2. Master page Init
         * 3. Content controls Init
         * 4. Master page Init
         * 5. Content page Init
         * 6. Content page Load
         * 7. Master page Load
         * 8. Master page controls Load
         * 9. Content page controls Load
         * */
        protected override void OnInit(EventArgs e)
        {
            bool isProductionMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["ProductionMode"]);
            if (isProductionMode)
            {
                _environment = Environment.Production;
                _siteTitle = SiteTitleProd;
            }
            else
            {
                _environment = Environment.Test;
                _siteTitle = SiteTitleTest;
            }

            base.OnInit(e);
        }

        public bool ProductionMode
        {
            get { return _environment == Environment.Production; }
        }

        #endregion

        #region Bind & Load Controls

        private void LoadControls()
        {
            BindRepeater();
            BindNewProductItemImage();
            BindNewsTextBlock();
            BindOtherTextBlock();

            if (ProductionMode)
                ViewStateButton.Visible = false;
            else
                ViewStateButton.Visible = true;
        }

        private void BindNewsTextBlock()
        {
            // news text
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.News.ToString(), LanguageID);
            if (table != null && table.Rows.Count > 0)
                NewsLabel.Text = UtilHelperComponent.ParseNewLine(table.Rows[0]["TextBlock"].ToString());
        }

        private void BindOtherTextBlock()
        {
            // news text
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.Other.ToString(), LanguageID);
            if (table != null && table.Rows.Count > 0)
                OtherLabel.Text = UtilHelperComponent.ParseNewLine(table.Rows[0]["TextBlock"].ToString());
        }

        private void LoadParameters()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["LangID"]))
                languageID = Request.QueryString["LangID"];


        }

        public void LoadShoppingCart()
        {
            orderID = CookieManager1.GetCookieSubkeyValue(cookieName, "OrderID");

            if (!string.IsNullOrEmpty(orderID))
            {
                NumArticlesLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetNumOrderItemsPerOrder(orderID));
                TotalCartPriceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetTotalOrderPrice(orderID));
            }
            else
            {
                NumArticlesLabel.Text = "0";
                TotalCartPriceLabel.Text = "0";
            }

            ShoppingCartUpdatePanel.Update();
        }

        public void LoadCategories()
        {
            BindRepeater();
        }

        private void BindRepeater()
        {
            CategoryRepeater.DataSource = CategoryDataComponent.GetCategories(languageID);
            CategoryRepeater.DataBind();
        }

        public void BindNewProductItemImage()
        {
            //if (!IsPostBack) // fix for disabling viewstate
            //{
                string folderUrl = "~/ProductImages/";  // to be refactorized

                DataTable table = ProductDataComponent.GetLatestProducts(languageID, 20);

                if (table != null && table.Rows.Count > 0)
                {
                    int randomRow = UtilHelperComponent.RandomNumber(table.Rows.Count - 1);

                    DataRow row = table.Rows[randomRow];
                    ProductIDHiddenField.Value = row["ProductID"].ToString();
                    SetImageSize(ProductImageButton, row["fileName"].ToString(), 185, 2000, folderUrl);
                    // set tooltip for product image
                    ProductImageButton.ToolTip = row["Title"].ToString();
                }
                else // no image available
                {
                    SetImageSize(ProductImageButton, "", 185, 2000, folderUrl);
                }
            //}
        }

        #endregion

        #region News
        protected void SaveNewsButton_Click(object sender, EventArgs e)
        {
            TextBlockDataComponent.SaveTextBlock("", NewsTextBox.Text, TextBlockIDHiddenField.Value, LanguageID);
            BindNewsTextBlock();
            SwitchNewsMode();
        }

        protected void CancelNewsButton_Click(object sender, EventArgs e)
        {
            SwitchNewsMode();
        }

        protected void EditNewsButton_Click(object sender, EventArgs e)
        {
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.News.ToString(), LanguageID);
            if (table != null && table.Rows.Count > 0)
            {
                TextBlockIDHiddenField.Value = table.Rows[0]["TextBlockID"].ToString();
                NewsTextBox.Text = table.Rows[0]["TextBlock"].ToString();
                SwitchNewsMode();
            }
        }

        private void SwitchNewsMode()
        {
            if (NewsDefaultPlaceHolder.Visible)
            {
                NewsDefaultPlaceHolder.Visible = false;
                NewsEditPlaceHolder.Visible = true;
            }
            else
            {
                NewsDefaultPlaceHolder.Visible = true;
                NewsEditPlaceHolder.Visible = false;
            }
        }

        #endregion

        #region Other
        protected void SaveOtherButton_Click(object sender, EventArgs e)
        {
            TextBlockDataComponent.SaveTextBlock("", OtherTextBox.Text, OtherTextBlockIDHiddenField.Value, LanguageID);
            BindOtherTextBlock();
            SwitchOtherMode();
        }

        protected void CancelOtherButton_Click(object sender, EventArgs e)
        {
            SwitchOtherMode();
        }

        protected void EditOtherButton_Click(object sender, EventArgs e)
        {
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.Other.ToString(), LanguageID);
            if (table != null && table.Rows.Count > 0)
            {
                OtherTextBlockIDHiddenField.Value = table.Rows[0]["TextBlockID"].ToString();
                OtherTextBox.Text = table.Rows[0]["TextBlock"].ToString();
                SwitchOtherMode();
            }
        }

        private void SwitchOtherMode()
        {
            if (OtherDefaultPlaceHolder.Visible)
            {
                OtherDefaultPlaceHolder.Visible = false;
                OtherEditPlaceHolder.Visible = true;
            }
            else
            {
                OtherDefaultPlaceHolder.Visible = true;
                OtherEditPlaceHolder.Visible = false;
            }
        }

        #endregion

        #region Click Events

        protected void EditButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Category.aspx?LangID=1");
        }

        protected void CategoryRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            // check which linkbutton was presseed
            if (e.CommandName.ToString() == "View")
            {
                string categoryID = e.CommandArgument.ToString();

                Response.Redirect("~/GroupView.aspx?CatID=" + categoryID);
            }

        }

        protected void UserViewLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/UserView.aspx");
        }

        protected void CheckoutLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Checkout.aspx");
        }

        protected void ProductImageButton_Click(object sender, EventArgs e)
        {
            // check which linkbutton was presseed
            if (!string.IsNullOrEmpty(ProductIDHiddenField.Value))
            {
                Response.Redirect("~/ProductView.aspx?ID=" + ProductIDHiddenField.Value);
            }

        }

        #endregion        

        #region Resizing images

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

        #endregion
    }
}
