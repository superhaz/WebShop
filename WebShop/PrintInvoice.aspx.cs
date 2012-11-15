using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.BLL;
using WebShop.DAL;
using System.Data;
using WebShop.BLL.Security;
using System.Web.Configuration;
using Ninject.Web;
using WebShop.DAL.Interfaces;
using Ninject;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class PrintInvoice : PageBase
    {
        [Inject]
        public IOrderData OrderDataComponent { get; set; }
        [Inject]
        public IOrderHelper OrderHelperComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        private string orderID;
        private string languageID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //SendOrderMail();
            LoadParameters();

            // extra validation since Encryption/Decryption has been used on ids
            if (!UtilHelperComponent.IsInteger(orderID) || !UtilHelperComponent.IsInteger(languageID))
                return;

            if (!IsPostBack)
            {
                LoadShippingInfo();
                LoadPriceControls();
                BindRepeater();

                OrderIDLiteral.Text = orderID;
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                DateLiteral.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }

        private void LoadParameters()
        {
            try
            {
                orderID = EncryptionHelper.DecryptQueryString("OrderID");
                languageID = EncryptionHelper.DecryptQueryString("LangID");
            }
            catch (Exception) { }

            // unencrypted code
            //if (!String.IsNullOrEmpty(Request.QueryString["OrderID"]))
            //    orderID = Request.QueryString["OrderID"];

            //if (!String.IsNullOrEmpty(Request.QueryString["LangID"]))
            //    languageID = Request.QueryString["LangID"];
        }

        private void BindRepeater()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                DataTable table = OrderDataComponent.GetOrder(orderID, languageID);
                // also bind to the hidden invoice repeater
                InvoiceRepeater.DataSource = table;
                InvoiceRepeater.DataBind();
            }
        }

        private void LoadShippingInfo()
        {
            DataRow row = OrderDataComponent.GetOrderShippingInfo(orderID, languageID);
            if (row != null)
            {
                NameLabel.Text = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                AddressLabel.Text = row["StreetAddress"].ToString();
                PostalAddressLabel.Text = row["PostalCode"].ToString() + " " + row["City"].ToString();
                PhonesLabel.Text = "Telefonnummer: " + row["HomePhone"].ToString() + " " + row["MobilePhone"].ToString();
                EmailHyperLink.Text = row["Email"].ToString();
                EmailHyperLink.NavigateUrl = "Mailto:" + row["Email"].ToString();
                DeliveryTypeLabel.Text = row["DeliveryName"].ToString();
                PaymentTypeLabel.Text = row["PaymentName"].ToString();
            }
        }


        private void LoadPriceControls()
        {
            if (!string.IsNullOrEmpty(orderID))
            {
                // load price controls for invoice template
                SumInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetTotalOrderPrice(orderID));
                SumInvoiceCurrencyLabel.Text = "SEK";
                DeliveryCostInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderDeliveryCost(orderID));
                DeliveryCurrencyInvoiceLabel.Text = "SEK";
                PaymentCostInvoiceLabel.Text = UtilHelperComponent.RoundPrice(OrderDataComponent.GetOrderPaymentCost(orderID));
                PaymentCurrencyInvoiceLabel.Text = "SEK";

                // calculate totalprice
                double totalPrice = OrderHelperComponent.CalculateTotalPrice(SumInvoiceLabel.Text, DeliveryCostInvoiceLabel.Text, PaymentCostInvoiceLabel.Text);
                double totalVAT = OrderHelperComponent.CalculateTotalVAT(DeliveryCostInvoiceLabel.Text, PaymentCostInvoiceLabel.Text, OrderDataComponent.GetOrder(orderID, languageID));

                TotalPriceInvoiceLabel.Text = totalPrice.ToString();
                TotalCurrencyInvoiceLabel.Text = "SEK";
                VATInvoiceLiteral.Text = totalVAT.ToString();
                VATCurrencyInvoiceLabel.Text = "SEK";
            }

        }
    }
}
