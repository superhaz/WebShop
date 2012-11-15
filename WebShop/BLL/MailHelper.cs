using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;
using WebShop.BLL.Interfaces;

namespace WebShop.BLL
{
    public class MailHelper : IMailHelper
    {
        /// <summary>
        /// Replaces the values/parameters in the xml file with the values from the dictionary. 
        /// </summary>
        /// <param name="body">the text inte the body conating the parameters to replace.</param>
        /// <param name="replaceValues">contains the values that should replace the parameters in body. The keys represent a parameter to replace. </param>
        public string InsertDataInBody(string body, Dictionary<string, string> replaceValues)
        {
            foreach (KeyValuePair<string, string> kvp in replaceValues)
                body = body.Replace("$" + kvp.Key, kvp.Value);

            return body;

        }

        /// <summary>
        /// Sends an MailMessage to a smtp server. Returns a string value (empty if success and containing the error if
        /// an error occurs).
        /// </summary>
        /// <param name="mail">the complete mailmessage to send.</param>
        public string SendMail(MailMessage mail, bool productionMode)
        {
            // remove real mail address receiver and add test receiver
            if (!productionMode)
                mail = PrepareNonProductionMail(mail);

            //send the message
            SmtpClient smtp = new SmtpClient(WebConfigurationManager.AppSettings["SMTPServer"].ToString());
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            try
            {
                smtp.Send(mail);
                return "";
            }
            catch (Exception e) { return e.Message + "   " + e.InnerException; }
        }

        public string SendMail(MailMessage mail, bool useDefaultSettings, bool isAsyncMail, bool productionMode)
        {
            // sends a mail with default settings
            if (useDefaultSettings)
                return SendMail(mail, productionMode);

            // remove real mail address receiver and add test receiver
            if (!productionMode)
                mail = PrepareNonProductionMail(mail);

            // prepare e-mail settings and send mail
            SmtpClient client = new SmtpClient();
            client.Timeout = 3000;
            client.Host = WebConfigurationManager.AppSettings["SMTPServer"].ToString();
            client.Port = Int32.Parse(WebConfigurationManager.AppSettings["Port"]);
            string username = WebConfigurationManager.AppSettings["User"];
            string password = WebConfigurationManager.AppSettings["Pass"];

            // prepare authentication for user
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                NetworkCredential mailAuthentication = new NetworkCredential(username, password);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = mailAuthentication;
            }
            try
            {
                // send mail. NOTE! Don't forget to set Async="true" on the page that will send  your mail
                if (isAsyncMail)
                {
                    // send asynchronious mail
                    client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                    client.SendAsync(mail, null);   // send asynchronious
                }
                else
                {
                    client.Send(mail);
                }

                return "";

            }
            catch (Exception e) { return e.Message + "   " + e.InnerException; }
        }

        public MailMessage PrepareMail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(from, to, subject, body);
            mail.IsBodyHtml = true;
            return mail;
        }

        public MailMessage PrepareMail(string from, string fromName, string to, string toName, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, fromName);
            mail.To.Add(new MailAddress(to, toName));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return mail;
        }

        public MailMessage PrepareNonProductionMail(MailMessage mail)
        {
            mail.Body = "<span style=\"font-family: arial; font-size: 10px; color: #999999;\">[Mail has been sent in test mode. Original recipient(s): " + mail.To.ToString() + "]</span><br/><br/>" + mail.Body;
            mail.To.Clear();
            mail.To.Add(WebConfigurationManager.AppSettings["TestMailReceiver"]);
            mail.CC.Clear();
            mail.Bcc.Clear();

            return mail;
        }

        public bool VerifyMailAddress(string mailAddress)
        {
            if (Regex.IsMatch(mailAddress, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                return true;
            else
                return false;

        }

        #region FUTURE IMPLEMENTATIONS
        /// <summary>
        /// FUTURE IMPLEMENTATIONS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // increase counters
            //_counterOfEmailContacts++;
            //_numOfSuccessEmailDispatches++;
            // notify user if dispatch finnished
            NotifyUserOfCompeletedDispatch();
        }

        /// <summary>
        /// FUTURE IMPLEMENTATIONS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyUserOfCompeletedDispatch()
        {
            //if (_counterOfEmailContacts >= _numOfEmailContacts)
            //{
            //    // looping of whole mailing list completed
            //    new ExceptionBLL("Newsletter result", _numOfSuccessEmailDispatches + " / " + _numOfEmailContacts + "has been sent");
            //}
        }
        #endregion
    }
}
