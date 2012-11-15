using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop.temp
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TestButton_Click(object sender, EventArgs e)
        {
            if (MessageLabel.Visible)
            {
                MessageLabel.Visible = false;
                PlaceHolder1.Visible = false;
            }
            else
            {
                PlaceHolder1.Visible = true;
                MessageLabel.Visible = true;
            }

        }

        protected void TestButton2_Click(object sender, EventArgs e)
        {
            if (Label1.Visible)
            {
                Label1.Visible = false;
                
            }
            else
            {
                
                Label1.Visible = true;
            }

        }
    }
}