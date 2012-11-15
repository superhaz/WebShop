using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop.temp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string test = (string) ViewState["Pelle"];
            BindRepeater();
            //ImageManager1.BindControls("111");
        }

        private void BindRepeater()
        {
            string[] filterOptions = { "All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            FilterRepeater.DataSource = filterOptions;
            FilterRepeater.DataBind();
            
        }
    }
}
