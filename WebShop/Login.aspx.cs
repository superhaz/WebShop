using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;

namespace WebShop
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("~/UnauthorizedAccess.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }

        protected void myLogin_LoginError(object sender, EventArgs e)
        {
            // Determine why the user could not login...        
            UserLogin.FailureText = "Your login attempt was not successful. Please try again.";

            // Does there exist a User account for this user?
            MembershipUser usrInfo = Membership.GetUser(UserLogin.UserName);
            if (usrInfo != null)
            {
                // Is this user locked out?
                if (usrInfo.IsLockedOut)
                {
                    UserLogin.FailureText = "Your account has been locked out because of too many invalid login attempts. Please contact the administrator to have your account unlocked.";
                }
                else if (!usrInfo.IsApproved)
                {
                    UserLogin.FailureText = "Your account has not yet been approved. You cannot login until an administrator has approved your account.";
                }
            }
        }
    }
}
