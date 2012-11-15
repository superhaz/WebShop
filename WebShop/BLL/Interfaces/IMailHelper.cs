using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace WebShop.BLL.Interfaces
{
    public interface IMailHelper
    {
        string InsertDataInBody(string body, Dictionary<string, string> replaceValues);
        string SendMail(MailMessage mail, bool productionMode);
        string SendMail(MailMessage mail, bool useDefaultSettings, bool isAsyncMail, bool productionMode);
        MailMessage PrepareMail(string from, string to, string subject, string body);
        MailMessage PrepareMail(string from, string fromName, string to, string toName, string subject, string body);
        MailMessage PrepareNonProductionMail(MailMessage mail);
        bool VerifyMailAddress(string mailAddress);

        #region FUTURE IMPLEMENTATIONS

        /// <summary>
        /// FUTURE IMPLEMENTATIONS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NotifyUserOfCompeletedDispatch();
        #endregion
    }
}