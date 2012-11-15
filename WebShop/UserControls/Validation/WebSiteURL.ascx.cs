using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class App_UserControls_Validation_WebSiteURL : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string Text
    {
        get { return WebSiteURLTextBox.Text; }
        set { WebSiteURLTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { WebSiteURLTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { WebSiteURLTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RegularExpressionValidator
    {
        set { WebSiteURLTextBox_RegularExpressionValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            WebSiteURLTextBox_RegularExpressionValidator.Text = value;
            WebSiteURLTextBox_RequiredFieldValidator.Text = value;
        }
    }

    public int Width
    {
        set { WebSiteURLTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { WebSiteURLTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            WebSiteURLTextBox.ValidationGroup = value;
            WebSiteURLTextBox_RegularExpressionValidator.ValidationGroup = value;
            WebSiteURLTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                WebSiteURLTextBox_RegularExpressionValidator.Text = "";
                WebSiteURLTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                WebSiteURLTextBox_RegularExpressionValidator.Text = "*";
                WebSiteURLTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
