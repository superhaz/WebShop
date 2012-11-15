using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.DAL;
using System.Web.Configuration;
using WebShop.BLL;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Net.Mail;
using WebShop.BLL.Security;
using Ninject.Web;
using Ninject;
using WebShop.DAL.Interfaces;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class Checkout : PageBase
    {
        [Inject]
        public IOrderData OrderDataComponent { get; set; }
        [Inject]
        public ISettingsData SettingsDataComponent { get; set; }
        [Inject]
        public IMailHelper MailHelperComponent { get; set; }
        [Inject]
        public IOrderHelper OrderHelperComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        private string cookieName = WebConfigurationManager.AppSettings["CookieName"];
        private string orderID;
        string productImageFolderUrl = "~/ProductImages/";  // to be refactorized
        string deliveryAndPaymentUrl = "~/App_Themes/Standard/images/";

        protected void Page_Load(object sender, EventArgs e)
        {
            //SendOrderMail();
            LoadParameters();
            InitiateMode();

            if (!IsPostBack)
            {
                LoadPriceControls();
                BindRepeaters();

                QueryStringHelper qs = new QueryStringHelper();
                qs.Add("LangID", Master.LanguageID);
                qs.Add("OrderID", orderID);
                QueryStringHelper qsEncrypted = EncryptionHelper.EncryptQueryString(qs);

                PrintHyperLinkImage.NavigateUrl = "~/PrintInvoice.aspx" + qsEncrypted.ToString();
                //PrintHyperLinkImage.NavigateUrl = "~/PrintInvoice.aspx?OrderID=" + orderID + "&LangID=" + Master.LanguageID;
            }
        }

        private void InitiateMode()
        {
            // add javascript 
            CompleteOrderImageButton.Attributes.Add("OnClick", "ConfirmOrder()");
            CompleteOrderLinkButton.Attributes.Add("OnClick", "ConfirmOrder()");

            // reset controls
            PaymentRequiredMessageLabel.Visible = false;
            DeliveryRequiredMessageLabel.Visible = false;

            // show empty shopping cart if order doesn't exist
            if (string.IsNullOrEmpty(orderID))
            {
                EmptyShoppingCartPlaceHolder.Visible = true;
                ShoppingCartPlaceHolder.Visible = false;
                OrderCompletedPlaceHolder.Visible = false;
            }
        }

        private void BindRepeaters()
        {
            BindItemRepeater();
            BindDeliveryRepeater();
            BindPaymentRepeater();
            //BindInvoiceRepeater();
        }

        protected override void OnInit(EventArgs e)
        {
            // register script for mutual selection of radio button in repeater
            ScriptManager.RegisterClientScriptInclude(Page, typeof(Page), "SetUniqueRadioButton", Page.ResolveUrl("~/Scripts/UniqueRadioButtons.js"));
            // set page title
            Title += WebConfigurationManager.AppSettings["CompanyName"];
            base.OnInit(e);
        }

        private void LoadParameters()
        {
            orderID = CookieManager1.GetCookieSubkeyValue(cookieName, "OrderID");
            if (!string.IsNullOrEmpty(orderID))
            {
                bool orderIsValid = OrderDataComponent.VerifyThatOrderExists(orderID);

                if (!orderIsValid)
                {
                    CookieManager1.DeleteCookie(cookieName);
                    orderID = "";
                }
            }
        }

        private void LoadPriceControls()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                SumLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetTotalOrderPrice(orderID));
                SumCurrencyLabel.Text = "SEK";
                DeliveryCostLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderDeliveryCost(orderID));
                DeliveryCurrencyLabel.Text = "SEK";
                PaymentCostLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderPaymentCost(orderID));
                PaymentCurrencyLabel.Text = "SEK";
                // calculate totalprice
                double totalPrice = OrderHelperComponent.CalculateTotalPrice(SumLabel.Text, DeliveryCostLabel.Text, PaymentCostLabel.Text);
                TotalPriceLabel.Text = totalPrice.ToString();
                TotalCurrencyLabel.Text = "SEK";

                double totalVAT = OrderHelperComponent.CalculateTotalVAT(DeliveryCostLabel.Text, PaymentCostLabel.Text, OrderDataComponent.GetOrder(orderID, Master.LanguageID));

                VATLiteral.Text = totalVAT.ToString();
                VATCurrencyLabel.Text = "SEK";

                // load price controls for invoice template
                SumInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetTotalOrderPrice(orderID));
                SumInvoiceCurrencyLabel.Text = "SEK";
                DeliveryCostInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderDeliveryCost(orderID));
                DeliveryCurrencyInvoiceLabel.Text = "SEK";
                PaymentCostInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderPaymentCost(orderID));
                PaymentCurrencyInvoiceLabel.Text = "SEK";

                TotalPriceInvoiceLabel.Text = totalPrice.ToString();
                TotalCurrencyInvoiceLabel.Text = "SEK";
                VATInvoiceLiteral.Text = totalVAT.ToString();
                VATCurrencyInvoiceLabel.Text = "SEK";
            }

        }

        private void BindItemRepeater()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                DataTable table = OrderDataComponent.GetOrder(orderID, Master.LanguageID);
                ItemRepeater.DataSource = table;
                ItemRepeater.DataBind();
                // also bind to the hidden invoice repeater
                InvoiceRepeater.DataSource = table;
                InvoiceRepeater.DataBind();
            }
        }

        private void BindDeliveryRepeater()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                DeliveryRepeater.DataSource = OrderDataComponent.GetDeliveries(Master.LanguageID);
                DeliveryRepeater.DataBind();
            }
        }

        private void BindPaymentRepeater()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                PaymentRepeater.DataSource = OrderDataComponent.GetPayments(Master.LanguageID);
                PaymentRepeater.DataBind();
            }
        }

        protected void ItemRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string orderRowID = (e.Item.FindControl("OrderRowIDHiddenField") as HiddenField).Value;
            if (e.CommandName.ToString() == "View")
            {
                string productID = (e.Item.FindControl("ProductIDHiddenField") as HiddenField).Value;
                Response.Redirect("~/ProductView.aspx?LangID=" + "1" + "&CatID=" + "1" + "&ID=" + productID);
            }
            // check which linkbutton was presseed
            else if (e.CommandName.ToString() == "IncreaseQuantity")
            {
                // Increase quantity
                OrderDataComponent.IncreaseOrderRowQuantity(orderRowID);
            }
            else if (e.CommandName.ToString() == "DecreaseQuantity")
            {
                // Decrease quantity
                string numOrderRows = OrderDataComponent.DecreaseOrderRowQuantity(orderRowID);

                // ie whole order and its order-rows were deleted
                if (numOrderRows == "0")
                {
                    CookieManager1.DeleteCookie(cookieName);
                    orderID = "";
                    InitiateMode();
                    Master.LoadShoppingCart();
                    return;
                }
            }

            BindItemRepeater();
            LoadPriceControls();
            Master.LoadShoppingCart();
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton ProductImageButton = e.Item.FindControl("ProductImageButton") as ImageButton;
            Literal priceLiteral = e.Item.FindControl("PriceLiteral") as Literal;
            Literal sumLiteral = e.Item.FindControl("SumLiteral") as Literal;
            priceLiteral.Text = UtilHelperComponent.RoundPrice(priceLiteral.Text);
            sumLiteral.Text = UtilHelperComponent.RoundPrice(sumLiteral.Text);
            BindItemImage(e.Item);
        }

        protected void DeliveryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // fix for setting unique radio buttons
            RadioButton deliveryRadioButton = (RadioButton)e.Item.FindControl("DeliveryRadioButton");
            string script = "SetUniqueRadioButton('DeliveryRepeater.*DeliveryGN',this);";
            deliveryRadioButton.Attributes.Add("onclick", script);

            // set image url
            System.Web.UI.WebControls.Image deliveryImage = e.Item.FindControl("DeliveryImage") as System.Web.UI.WebControls.Image;
            deliveryImage.ImageUrl = deliveryAndPaymentUrl + deliveryImage.ImageUrl;
        }

        protected void PaymentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // set unique radio buttons
            RadioButton paymentRadioButton = (RadioButton)e.Item.FindControl("PaymentRadioButton");

            string script = "";

            if (paymentRadioButton.Text == "Kontoinsättning")
            {
                script = "SetUniqueRadioButton('PaymentRepeater.*PaymentGN',this); alert('OBS! Vid betalning via kontoinsättning skickas varorna först efter att BETALNING REGISTRERATS på vårt konto.');";
            }
            else
            {
                script = "SetUniqueRadioButton('PaymentRepeater.*PaymentGN',this);";
            }
            
            paymentRadioButton.Attributes.Add("onclick", script);

            // set image url
            System.Web.UI.WebControls.Image paymentImage = e.Item.FindControl("PaymentImage") as System.Web.UI.WebControls.Image;
            paymentImage.ImageUrl = deliveryAndPaymentUrl + paymentImage.ImageUrl;
        }

        protected void DeliveryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton deliveryRadioButton = sender as RadioButton;
            OrderDataComponent.SetDeliveryForOrder(orderID, deliveryRadioButton.Text, Master.LanguageID);

            DataRow row = OrderDataComponent.GetDeliveryByDeliveryName("1", deliveryRadioButton.Text);
            if (row != null)
            {
                DeliveryInfoLabel.Text = "<b>" + row["DeliveryName"].ToString() + "</b><br />" + row["DeliveryDescription"].ToString() + "<br />Pris: " + UtilHelperComponent.RoundPrice(row["Price"].ToString()) + " kr";
                DeliveryInfoPlaceHolder.Visible = true;
            }

            // set invoice template value
            DeliveryTypeLabel.Text = deliveryRadioButton.Text;
            LoadPriceControls();
        }

        protected void PaymentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton paymentRadioButton = sender as RadioButton;
            OrderDataComponent.SetPaymentForOrder(orderID, paymentRadioButton.Text, Master.LanguageID);

            DataRow row = OrderDataComponent.GetPaymentByPaymentNameInfo("1", paymentRadioButton.Text);
            if (row != null)
            {
                PaymentInfoLabel.Text = "<b>" + row["PaymentName"].ToString() + "</b><br />" + row["PaymentDescription"].ToString() + "<br />Pris: " + UtilHelperComponent.RoundPrice(row["Price"].ToString()) + " kr";
                PaymentInfoPlaceHolder.Visible = true;
            }

            // set invoice template value
            PaymentTypeLabel.Text = paymentRadioButton.Text;
            LoadPriceControls();
        }

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
            if (string.IsNullOrEmpty(orderID))
                return;

            System.Web.UI.WebControls.Image itemImage = item.FindControl("ItemImage") as System.Web.UI.WebControls.Image;
            string filename = (item.FindControl("FileNameHiddenField") as HiddenField).Value;
            //string filesize = (item.FindControl("FileSizeHiddenField") as HiddenField).Value;
            //string priority = (item.FindControl("PriorityHiddenField") as HiddenField).Value;

            //itemImage.AlternateText = itemImage.ToolTip = "prioritet: " + priority;

            // default image
            SetImageSize(itemImage, filename, 60, 60);
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
            string imageUrl = productImageFolderUrl + filename;
            string imageUrlRoot = Server.MapPath(productImageFolderUrl + filename);
            //-----------NOT USED - ONLY FOR CLARIFICATION----------------------------

            if (File.Exists(Server.MapPath(productImageFolderUrl + filename)))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(productImageFolderUrl + filename));
                imageSize = SizeImage(img, imageWidth, imageHeight);
            }
            else
            {
                filename = "no_image.jpg";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(productImageFolderUrl + filename));
                imageSize = SizeImage(img, imageWidth, imageHeight);
            }

            imageControl.Height = imageSize.Height;
            imageControl.Width = imageSize.Width;
            imageControl.ImageUrl = productImageFolderUrl + filename;
        }

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


        protected void CompleteOrderButton_Click(object sender, EventArgs e)
        {
            bool pageIsValid = true;
            // validate delivery selection
            if (!VerifyRadioButtonSelection(DeliveryRepeater, "DeliveryRadioButton"))
            {
                DeliveryRequiredMessageLabel.Visible = true;
                pageIsValid = false;
            }

            // validate payment selection
            if (!VerifyRadioButtonSelection(PaymentRepeater, "PaymentRadioButton"))
            {
                PaymentRequiredMessageLabel.Visible = true;
                pageIsValid = false;
            }

            if (!AcceptTermsCheckBox.Checked)
            {
                AcceptTermsRequiredMessageLabel.Visible = true;
                pageIsValid = false;
            }

            // complete order if terms are accepted
            if (pageIsValid)
            {
                if (!string.IsNullOrEmpty(orderID))
                {
                    // set order as completed in database
                    OrderDataComponent.CompleteOrder(orderID, FirstName.Text, LastName.Text, StreetAddress.Text, PostalCode.Text, CityName.Text, Email.Text, HomePhoneNumber.Text, MobilePhoneNumber.Text);
                    // delete cookie for orderID
                    CookieManager1.DeleteCookie(cookieName);
                    // send order
                    SendOrderMail();

                    // reset controls
                    Master.LoadShoppingCart();
                    ResetControls();
                    ShoppingCartPlaceHolder.Visible = false;
                    OrderCompletedPlaceHolder.Visible = true;
                }
            }
        }

        private void SendOrderMail()
        {
            // get invoice as string
            string invoice = InvoiceToString();
            // read html mail template
            string rootUrl = HttpContext.Current.Server.MapPath("~/Mail/Order.htm");
            string mailBody = File.ReadAllText(rootUrl);
            // insert invoice, orderID and date into mailBody
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            mailBody = mailBody.Replace("$Invoice", invoice).Replace("$OrderID", orderID).Replace("$OrderDate", currentDate).Replace("$AccountNumber", SettingsDataComponent.GetAccountNumber());

            // prepare and send mail to customer
            string companyMail = WebConfigurationManager.AppSettings["CompanyMailAddress"];
            // send mail to customer
            MailMessage mailMessage = MailHelperComponent.PrepareMail(companyMail, Email.Text, "Tack för din beställning - OrderId: " + orderID, mailBody);
            string error = MailHelperComponent.SendMail(mailMessage, Master.ProductionMode);
            // send mail to company
            mailMessage = MailHelperComponent.PrepareMail(companyMail, companyMail, "Ny beställning inkommen OrderId " + orderID, mailBody);
            error += MailHelperComponent.SendMail(mailMessage, Master.ProductionMode);

            if (!string.IsNullOrEmpty(error))
            {
                // TODO: log error in log table
            }
        }

        private string AccountInfo()
        {
            return @"Kontoinsättningsinformation<br />
                    Kontonummer: " + SettingsDataComponent.GetAccountNumber() + @"<br />
                    <b>Obs!</b>Det är viktigt att ni anger Ert ordernummer som en kommentar vid betalning via kontoinsättning annars kan vi inte spåra betalningen. 
                    När beställningen är betald skickas ordern ut.<br /><br />";
        }

        private string InvoiceToString()
        {
            // prepare customer shipping address
            PrepareShippingAddress();
            // make place holder visible to get it work as output
            InvoicePlaceHolder.Visible = true;
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter objHtml = new HtmlTextWriter(new System.IO.StringWriter(sb));
            InvoicePlaceHolder.RenderControl(objHtml);
            // hide place holder from being shown in gui
            InvoicePlaceHolder.Visible = false;
            return sb.ToString();
        }

        private void PrepareShippingAddress()
        {
            NameLabel.Text = FirstName.Text + " " + LastName.Text;
            AddressLabel.Text = StreetAddress.Text;
            PostalAddressLabel.Text = PostalCode.Text + " " + CityName.Text;
            PhonesLabel.Text = "Telefonnummer: " + MobilePhoneNumber.Text + " " + HomePhoneNumber.Text;
            EmailHyperLink.Text = Email.Text;
            EmailHyperLink.NavigateUrl = "Mailto:" + Email.Text;
        }

        // checks whether user has selected a delivery date
        public bool VerifyRadioButtonSelection(Repeater radioButtonRepeater, string radioButton)
        {

            // if no delivery date was selected return validation = false
            bool userHasSelectedARadioButton = false;
            // loop through rows in repeater
            foreach (RepeaterItem item in radioButtonRepeater.Items)
            {
                RadioButton selectedRadioButton = item.FindControl(radioButton) as RadioButton;

                if (selectedRadioButton.Checked)
                {
                    // set boolean value
                    userHasSelectedARadioButton = true;
                    break;
                }
            }

            return userHasSelectedARadioButton;
        }

        private void ResetControls()
        {
            SumLabel.Text = "";
            SumCurrencyLabel.Text = "";
            DeliveryCostLabel.Text = "";
            DeliveryCurrencyLabel.Text = "";
            PaymentCostLabel.Text = "";
            PaymentCurrencyLabel.Text = "";
            TotalPriceLabel.Text = "";
            TotalCurrencyLabel.Text = "";

            FirstName.Text = "";
            LastName.Text = "";
            StreetAddress.Text = "";
            PostalCode.Text = "";
            CityName.Text = "";
            Email.Text = "";
            HomePhoneNumber.Text = "";
            MobilePhoneNumber.Text = "";

            AcceptTermsRequiredMessageLabel.Visible = false;
        }

    }
}
