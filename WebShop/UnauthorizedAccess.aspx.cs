using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace WebShop
{
    public partial class UnauthorizedAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }
    }
}
