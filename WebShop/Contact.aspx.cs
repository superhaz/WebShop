using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Configuration;
using System.Net.Mail;
using WebShop.BLL;
using Ninject.Web;
using WebShop.BLL.Interfaces;
using Ninject;

namespace WebShop
{
    public partial class Contact : PageBase
    {
        [Inject]
        public IMailHelper MailHelperComponent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            string error = SendContactMail();
            if (string.IsNullOrEmpty(error))
            {
                ContactFormPlaceHolder.Visible = false;
                MessagePlaceHolder.Visible = true;
                ErrorPlaceHolder.Visible = false;
            }
            else
            {
                ContactFormPlaceHolder.Visible = false;
                MessagePlaceHolder.Visible = false;
                ErrorPlaceHolder.Visible = true;
            }

        }

        private string SendContactMail()
        {
            // read html mail template
            string rootUrl = HttpContext.Current.Server.MapPath("~/Mail/Contact.htm");
            string mailBody = File.ReadAllText(rootUrl);
            // insert invoice, orderID and date into mailBody
            mailBody = mailBody.Replace("$Email", Email.Text).Replace("$Phone", PhoneNumber.Text).Replace("$Message", MessageTextBox.Text);
            // prepare and send mail 
            string companyMail = WebConfigurationManager.AppSettings["CompanyMailAddress"];
            MailMessage mailMessage = MailHelperComponent.PrepareMail(Email.Text, companyMail, "Meddelande från kontaktformulär", mailBody);
            return MailHelperComponent.SendMail(mailMessage, Master.ProductionMode);
        }
    }
}
