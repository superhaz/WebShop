using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace WebShop.BLL.Security
{
    /// <summary>
    /// Title: EncryptionHelper 
    /// Description: Business logic for encrypting/decrypting query strings.
    /// Author: http://www.eggheadcafe.com/articles/20060427.asp
    /// Version: 1.0
    /// </summary>
    public class EncryptionHelper
    {
        #region example code

        /// <summary>
        /// Example code
        /// </summary>
        public static void MyFunction()
        {
            /// Encrypting an url's parameter
            string id = "12";
            // encrypt a value
            string href = "http://www.mypage.se/PrintInvoice.aspx" + EncryptQueryString("id", id);

            // encrypting multiple values
            QueryStringHelper qs = new QueryStringHelper();
            qs.Add("langID", "1");
            qs.Add("prodID", "84");
            QueryStringHelper qsEncrypted = EncryptionHelper.EncryptQueryString(qs);
            string link = "http://www.mypage.se/PrintInvoice.aspx" + qsEncrypted.ToString();


            // decryptng the current url's parameter
            string id1 = DecryptQueryString("id");
            string id2 = DecryptQueryString("langID");
            string id3 = DecryptQueryString("prodID");

            // lower level
            QueryStringHelper qs2 = QueryStringHelper.FromCurrent();
            QueryStringHelper qsdec = EncryptionHelper.DecryptQueryString(qs2);
            // check which site to show, either EventRegistration or MemberValidation Login
            if (qsdec["id"] != null )
            {
                id3 = qsdec["prodID"];
            }
        }

        #endregion

        public static string EncryptQueryString(string queryStringName, string queryStringValue)
        {
            // prepare encryption of query string
            QueryStringHelper qs = new QueryStringHelper();
            qs.Add(queryStringName, queryStringValue);
            QueryStringHelper qsEncrypted = EncryptionHelper.EncryptQueryString(qs);

            return qsEncrypted.ToString();
        }

        public static string DecryptQueryString(string queryStringName)
        {
            //// decrypt parameters
            QueryStringHelper qs = QueryStringHelper.FromCurrent();
            QueryStringHelper qsdec = EncryptionHelper.DecryptQueryString(qs);
            //// assign datasource with parameter value
            if (!string.IsNullOrEmpty(qsdec[queryStringName]))
                return qsdec[queryStringName].ToString();
            else
                return null;
        }

        /// <summary>
        /// Encrypts a querystring
        /// </summary>
        /// <param name="queryString">a querystring</param>
        /// <returns>a new encrypted querystring</returns>
        public static QueryStringHelper EncryptQueryString(QueryStringHelper queryString)
        {
            QueryStringHelper newQueryString = new QueryStringHelper();
            string nm = String.Empty;
            string val = String.Empty;
            foreach (string name in queryString)
            {
                nm = name;
                val = queryString[name];
                newQueryString.Add(EncryptionHelper.Hex(nm), EncryptionHelper.Hex(val));
            }
            return newQueryString;
        }

        /// <summary>
        /// Decrypts a querystring
        /// </summary>
        /// <param name="queryString">encrypted querystring</param>
        /// <returns>a decrypted querystring</returns>
        public static QueryStringHelper DecryptQueryString(QueryStringHelper queryString)
        {
            QueryStringHelper newQueryString = new QueryStringHelper();
            string nm;
            string val;
            foreach (string name in queryString)
            {
                nm = EncryptionHelper.DeHex(name);
                val = EncryptionHelper.DeHex(queryString[name]);
                newQueryString.Add(nm, val);
            }
            return newQueryString;
        }

        /// <summary>
        /// decrypts hexstring
        /// </summary>
        /// <param name="hexstring">encrypted hexstring</param>
        /// <returns>decrypted hexstring</returns>
        public static string DeHex(string hexstring)
        {
            string ret = String.Empty;
            StringBuilder sb = new StringBuilder(hexstring.Length / 2);
            for (int i = 0; i <= hexstring.Length - 1; i = i + 2)
            {
                sb.Append((char)int.Parse(hexstring.Substring(i, 2), NumberStyles.HexNumber));
            }
            return sb.ToString();
        }

        /// <summary>
        /// encrypts a token string
        /// </summary>
        /// <param name="sData">a token string</param>
        /// <returns>an encrypted token string</returns>
        public static string Hex(string sData)
        {
            string temp = String.Empty; ;
            string newdata = String.Empty;
            StringBuilder sb = new StringBuilder(sData.Length * 2);
            for (int i = 0; i < sData.Length; i++)
            {
                if ((sData.Length - (i + 1)) > 0)
                {
                    temp = sData.Substring(i, 2);
                    if (temp == @"\n") newdata += "0A";
                    else if (temp == @"\b") newdata += "20";
                    else if (temp == @"\r") newdata += "0D";
                    else if (temp == @"\c") newdata += "2C";
                    else if (temp == @"\\") newdata += "5C";
                    else if (temp == @"\0") newdata += "00";
                    else if (temp == @"\t") newdata += "07";
                    else
                    {
                        sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                        i--;
                    }
                }
                else
                {
                    sb.Append(String.Format("{0:X2}", (int)(sData.ToCharArray())[i]));
                }
                i++;
            }
            return sb.ToString();
        }
    }
}
