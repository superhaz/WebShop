using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop.temp
{
    public partial class WebForm31 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TestButton_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(MessageLabelVisibility.Value))
                MessageLabel.Visible = Convert.ToBoolean(MessageLabelVisibility.Value);

            if (MessageLabel.Visible)
            {
                MessageLabel.Visible = false;
                MessageLabelVisibility.Value = MessageLabel.Visible.ToString();
                
            }
            else
            {
                
                MessageLabel.Visible = true;
                MessageLabelVisibility.Value = MessageLabel.Visible.ToString();
            }

        }
    }
}