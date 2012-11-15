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

public partial class App_UserControls_Validation_Email : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string Text
    {
        get { return EmailTextBox.Text; }
        set { EmailTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { EmailTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { EmailTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RegularExpressionValidator
    {
        set { EmailTextBox_RegularExpressionValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            EmailTextBox_RegularExpressionValidator.Text = value;
            EmailTextBox_RequiredFieldValidator.Text = value;
        }
    }

    public int Width
    {
        set { EmailTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { EmailTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            EmailTextBox.ValidationGroup = value;
            EmailTextBox_RegularExpressionValidator.ValidationGroup = value;
            EmailTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                EmailTextBox_RegularExpressionValidator.Text = "";
                EmailTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                EmailTextBox_RegularExpressionValidator.Text = "*";
                EmailTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
