using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop.UserControls
{
    public partial class CookieManager : System.Web.UI.UserControl
    {
        public int NumOfExpiresDays  { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveCookieSubkeyValue(string cookieName, string subkeyName, string value)
        {
            // if no cookie exists it will be created - save subkey value
            Response.Cookies[cookieName][subkeyName] = value;
            // set expire time
            Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(NumOfExpiresDays);
        }

        public void SaveCookieSubkeyValues(string cookieName, string[] subkeyNames, string[] values)
        {
            // if no cookie exists it will be created
            for (int i = 0; i < subkeyNames.Length; i++)
            {
                // save subkey value
                Response.Cookies[cookieName][subkeyNames[i]] = values[i];
            }
            // set expire time
            Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(NumOfExpiresDays);
        }

        public HttpCookie GetCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = Request.Cookies[cookieName];
                return cookie;
            }
            else
            {
                return null;
            }
        }

        public string GetCookieSubkeyValue(string cookieName, string subkeyName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                // gets a handle of the cookie's subkey value
                return Request.Cookies[cookieName][subkeyName];
            }
            else
            {
                return "";
            }
        }

        public void DeleteCookieSubkeyValue(string cookieName, string subkeyName)
        {
            // get handle of cookie
            HttpCookie cookie = Request.Cookies[cookieName];
            // remove subkey
            cookie.Values.Remove(subkeyName);
            // set expire time
            cookie.Expires = DateTime.Now.AddDays(NumOfExpiresDays);
            Response.Cookies.Add(cookie);
        }

        // deletes a cookie
        public void DeleteCookie(string cookieName)
        {
            // verify that cookie exists
            if (Request.Cookies[cookieName] != null)
            {
                // remove all subkeys
                // get handle of cookie
                HttpCookie cookie = Request.Cookies[cookieName];
                // remove subkey
                cookie.Values.Clear();
                // set cookie as expired since 1 day
                Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);
            }
        }
    }
}