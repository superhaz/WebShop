using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using WebShop.BLL;
using WebShop.UserControls.FileImageManager.BLL;
using WebShop.DAL;
using System.Data;
using WebShop.UserControls.FileImageManager.DAL;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;
using WebShop.UserControls.FileImageManager.DAL.Factory;
using Ninject.Web;
using Ninject;
using WebShop.DAL.Interfaces;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class ProductView : PageBase
    {
        [Inject]
        public IProductData ProductDataComponent { get; set; }
        [Inject]
        public IOrderData OrderDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        #region field variables & properties

        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        private string cookieName = WebConfigurationManager.AppSettings["CookieName"];
        private string productID;
        private string languageID = "1";
        private string categoryID = "1";

        #endregion

        #region Provider

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
                {
                    // creates a new provider of type ProductProvider
                    provider = ItemDataProviderFactory.CreateProvider(ItemDataProviderFactory.ItemDataProviderType.ProductProvider);
                }

                return provider;
            }
        }
        // end of Provider members

        #endregion

        #region ControlMode

        public enum ImageControlMode { ImageViewingMode = 1, ImagesListingMode, AddNewImageMode, ImagesListModeOnly }
        protected UtilHelper.ControlMode Mode
        {
            get
            {
                if (!String.IsNullOrEmpty((string)ViewState["Mode"]))
                {
                    string str = (string)ViewState["Mode"];
                    return (UtilHelper.ControlMode)Enum.Parse(typeof(UtilHelper.ControlMode), (string)ViewState["Mode"], true); // converts a string to enum
                }
                else
                {
                    string str = (string)ViewState["Mode"];
                    ViewState["Mode"] = UtilHelper.ControlMode.DefaultMode.ToString();
                    return UtilHelper.ControlMode.DefaultMode;
                }
            }
            set
            {
                string str = value.ToString();
                ViewState["Mode"] = value.ToString();
            }
        }

        

        #endregion

        #region PageLoad, Loading & Binding
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
            if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
                categoryID = Request.QueryString["CatID"];

            if (!String.IsNullOrEmpty(Request.QueryString["LangID"]))
                languageID = Request.QueryString["LangID"];

            // If querystring value is missing, send the user to Default.aspx
            // save parameter values
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                productID = Request.QueryString["ID"];
            else
                Response.Redirect("~/Default.aspx");
        }

        private void InitiateMode()
        {
            // check if it is a new product
            bool isANewProduct = ProductDataComponent.IsItANewProduct(productID);
            if (isANewProduct)
                Mode = UtilHelper.ControlMode.NewEditMode;
            else
                Mode = UtilHelper.ControlMode.DefaultMode;

            SwitchMode();
        }

        private void SwitchMode()
        {
            if (Mode == UtilHelper.ControlMode.DefaultMode)
            {
                DefaultPlaceHolder.Visible = true;
                ShowEditPlaceHolder.Visible = true;
                EditPlaceHolder.Visible = false;
                CancelPlaceHolder.Visible = false;
                BindDefaultControls();
            }
            else if (Mode == UtilHelper.ControlMode.EditMode || Mode == UtilHelper.ControlMode.NewEditMode)
            {
                DefaultPlaceHolder.Visible = false;
                ShowEditPlaceHolder.Visible = false;
                EditPlaceHolder.Visible = true;
                BindEditControls();
            }
        }

        protected void BindCheckBoxList(DataTable table, CheckBoxList checkBoxList, List<string> selectedIDs)
        {
            checkBoxList.DataSource = table;
            checkBoxList.DataBind();

            // check selected ids
            foreach (ListItem item in checkBoxList.Items)
            {
                foreach (string selectedID in selectedIDs)
                    if (item.Value == selectedID)
                        item.Selected = true;
            }
        }

        private void BindDropDownList(DataTable table, DropDownList dropDownList, string selectedID, string noSelectedValue)
        {
            if (table.Rows.Count > 0)
            {
                //dropDownList.Visible = true;
                dropDownList.DataSource = table;
                dropDownList.DataBind();

                // add default value - ie no selection
                if (!string.IsNullOrEmpty(noSelectedValue))
                    dropDownList.Items.Insert(0, new ListItem(noSelectedValue, ""));

                // assign selected value
                if (!string.IsNullOrEmpty(selectedID))
                    dropDownList.SelectedValue = selectedID;
            }
            else
            {
                //dropDownList.Visible = false;
            }
        }

        private List<string> SelectedCheckBoxListValues(CheckBoxList checkBoxList)
        {
            List<string> selectedValues = new List<string>();

            foreach (ListItem item in checkBoxList.Items)
            {
                if (item.Selected)
                    selectedValues.Add(item.Value);
            }

            return selectedValues;
        }

        #endregion

        #region Save / Load Item

        private void BindDefaultControls()
        {
            // bind size
            BindDropDownList(ProductDataComponent.GetSizesByProductID(productID), SizeDropDownList, null, "Välj en storlek");
            if (SizeDropDownList.Items.Count > 0)
                SizePlaceHolder.Visible = true;
            else
                SizePlaceHolder.Visible = false;

            // bind color
            BindDropDownList(ProductDataComponent.GetColorsByProductID(productID, languageID), ColorDropDownList, null, "Välj en färg");
            if (ColorDropDownList.Items.Count > 0)
                ColorPlaceHolder.Visible = true;
            else
                ColorPlaceHolder.Visible = false;

            // bind image manager control
            ImageManager1.BindControls(productID);

            DataTable table = ProductDataComponent.GetProductByProductID(productID, languageID);
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                TitleLabel.Text = row["Title"].ToString();
                Title = row["Title"].ToString() + " - " + WebConfigurationManager.AppSettings["CompanyName"];
                ArtNoLabel.Text = row["ArtNo"].ToString();
                ShortInfoLabel.Text = UtilHelperComponent.ParseNewLine(row["ShortInfo"].ToString());

                PriceLabel.Text = UtilHelperComponent.RoundPrice(row["Price"].ToString());
                VATHiddenField.Value = row["VAT"].ToString();
                CurrencyLabel.Text = "SEK";
                FullInfoLabel.Text = UtilHelperComponent.ParseNewLine(row["FullInfo"].ToString());

                AvailabilityLabel.Text = row["AvailabilityName"].ToString();

                // check if to show addtocart buttons
                if (row["Availability"].ToString().ToLower() == "OutOfStock".ToLower())
                    AddToCartPlaceHolder.Visible = false;


                if (Request.IsAuthenticated)
                {
                    Label isPublishedLabel = IsPublishedLoginView.FindControl("IsPublishedLabel") as Label;

                    if (row["IsPublished"].ToString() == "True")
                        isPublishedLabel.Text = "Ja";
                    else if (row["IsPublished"].ToString() == "False")
                        isPublishedLabel.Text = "Nej";
                }

                //
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }

        }

        private void BindEditControls()
        {
            // bind availability
            BindDropDownList(ProductDataComponent.GetAvailabilities(languageID), AvailabilityDropDownList, null, "-Välj tillgänglighet-");
            // bind size
            List<string> selectedSizeIDs = ProductDataComponent.GetSizeIDsByProductID(productID);
            BindCheckBoxList(ProductDataComponent.GetSizes("1"), SizeCheckBoxList, selectedSizeIDs);
            // bind color
            List<string> selectedColorIDs = ProductDataComponent.GetColorIDsByProductID(productID, languageID);
            BindCheckBoxList(ProductDataComponent.GetColors(languageID), ColorCheckBoxList, selectedColorIDs);

            if (Mode == UtilHelper.ControlMode.EditMode && !String.IsNullOrEmpty(productID))
            {
                DataTable table = ProductDataComponent.GetProductByProductID(productID, languageID);
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    TitleTextBox.Text = row["Title"].ToString();
                    ArtNoTextBox.Text = row["ArtNo"].ToString();
                    ShortInfoTextBox.Text = row["ShortInfo"].ToString();
                    PriceTextBox.Text = UtilHelperComponent.RoundPrice(row["Price"].ToString());
                    FullInfoTextBox.Text = row["FullInfo"].ToString();

                    string availablityID = row["AvailabilityID"].ToString();
                    if (!string.IsNullOrEmpty(availablityID))
                        AvailabilityDropDownList.SelectedValue = availablityID;

                    if (row["IsPublished"].ToString() == "True")
                        IsPublishedCheckBox.Checked = true;
                    else if (row["IsPublished"].ToString() == "False")
                        IsPublishedCheckBox.Checked = false;
                }
            }
            else if (Mode == UtilHelper.ControlMode.NewEditMode)
            {
                ResetControls();
            }
        }

        private void SaveItem()
        {
            // get selected sizes values
            List<string> sizeIDs = SelectedCheckBoxListValues(SizeCheckBoxList);
            // get selected colors values
            List<string> colorIDs = SelectedCheckBoxListValues(ColorCheckBoxList);

            // check whether it is an update or an insert of a new item
            if (Mode == UtilHelper.ControlMode.NewEditMode && !String.IsNullOrEmpty(productID))
            {
                ProductDataComponent.SaveNewProduct(productID, categoryID, AvailabilityDropDownList.SelectedValue, colorIDs, sizeIDs, ArtNoTextBox.Text, PriceTextBox.Text, IsPublishedCheckBox.Checked, TitleTextBox.Text, ShortInfoTextBox.Text, FullInfoTextBox.Text, "");
                Response.Redirect("~/ProductView.aspx?LangID=" + languageID + "&CatID=" + categoryID + "&ID=" + productID);
            }
            else if (Mode == UtilHelper.ControlMode.EditMode && !String.IsNullOrEmpty(productID))
            {
                ProductDataComponent.SaveProduct(productID, languageID, AvailabilityDropDownList.SelectedValue, colorIDs, sizeIDs, ArtNoTextBox.Text, PriceTextBox.Text, IsPublishedCheckBox.Checked, TitleTextBox.Text, ShortInfoTextBox.Text, FullInfoTextBox.Text, "");
            }

            // makes some recycling cleanup of half-created products
            ProductDataComponent.CleanUpProducts();
        }

        #endregion

        #region Button-Events & Validation

        protected void EditButton_Click(object sender, EventArgs e)
        {
            Mode = UtilHelper.ControlMode.EditMode;
            SwitchMode();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveItem();
                Mode = UtilHelper.ControlMode.DefaultMode;
                ResetControls();
                SwitchMode();
            }
        }

        protected void CancelItemButton_Click(object sender, EventArgs e)
        {
            //BindRepeater();

            ResetControls();
            Mode = UtilHelper.ControlMode.DefaultMode;
            SwitchMode();
        }

        private void ResetControls()
        {
            ArtNoTextBox.Text = "";
            TitleTextBox.Text = "";
            ShortInfoTextBox.Text = "";
            FullInfoTextBox.Text = "";
            IsPublishedCheckBox.Checked = true;
            PriceTextBox.Text = "";
            VATHiddenField.Value = "";

            if (AddedToCartMessageLabel.Visible)
                AddedToCartMessageLabel.Visible = false;
        }

        protected void AddToCartButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(productID))
            {
                // get orderID from cookie
                string orderID = CookieManager1.GetCookieSubkeyValue(cookieName, "OrderID");
                string quantity = "1";
                // verify that orderID exists in database
                bool orderIsValid = OrderDataComponent.VerifyThatOrderExists(orderID);

                if (!orderIsValid)
                {
                    CookieManager1.DeleteCookie(cookieName);
                    orderID = "";
                }

                string size = "-";
                string color = "-";

                // create a new order if not already exists an order and add product to order-row
                if (SizeDropDownList.Visible)
                    size = SizeDropDownList.SelectedItem.Text;

                if (ColorDropDownList.Visible)
                    color = ColorDropDownList.SelectedItem.Text;

                orderID = OrderDataComponent.AddProductToOrder(orderID, null, productID, PriceLabel.Text, VATHiddenField.Value, quantity, size, color);


                if (!string.IsNullOrEmpty(orderID))
                {
                    // save the orderid for this order
                    CookieManager1.SaveCookieSubkeyValue(cookieName, "OrderID", orderID);
                }

                // update the master pages category repeater
                Master.LoadShoppingCart();

                AddedToCartMessageLabel.Visible = true;
            }
        }

        /// <summary>
        /// Cancels a new item and deletes it and all associated files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // deletes all files connected to the product
            FileHelper.DeleteItemFiles(productID, Provider.GetFilesByItemID(productID));
            ProductDataComponent.DeleteProduct(productID);
            Response.Redirect("~/GroupView.aspx?CatID=" + categoryID + "&LangID=" + languageID);
        }

        protected void SwitchToMeasureTableLinkButton_Click(object sender, EventArgs e)
        {
            if (MeasureTablePlaceHolder.Visible)
            {
                MeasureTablePlaceHolder.Visible = false;
                SwitchToMeasureTableLinkButton.Text = "Visa måttabell";
            }
            else
            {
                MeasureTablePlaceHolder.Visible = true;
                SwitchToMeasureTableLinkButton.Text = "Dölj måttabell";
            }
        }

        protected void ShortInfoTextBox_CustomValidator_Validate(object sender, ServerValidateEventArgs e)
        {
            if (ShortInfoTextBox.Text.Length > 100)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        #endregion
    }
}
