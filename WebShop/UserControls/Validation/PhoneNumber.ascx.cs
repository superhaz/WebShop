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

public partial class App_UserControls_Validation_PhoneNumber : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return PhoneNumberTextBox.Text; }
        set { PhoneNumberTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { PhoneNumberTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { PhoneNumberTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RegularExpressionValidator
    {
        set { PhoneNumberTextBox_RegularExpressionValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            PhoneNumberTextBox_RequiredFieldValidator.Text = value;
            PhoneNumberTextBox_RegularExpressionValidator.Text = value;
        }
    }

    public int Width
    {
        set { PhoneNumberTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { PhoneNumberTextBox.ToolTip = value; }
    }

    public int Columns
    {
        set { PhoneNumberTextBox.Columns = value; }
    }

    public string ValidationGroup
    {
        set
        {
            PhoneNumberTextBox.ValidationGroup = value;
            PhoneNumberTextBox_RegularExpressionValidator.ValidationGroup = value;
            PhoneNumberTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                PhoneNumberTextBox_RegularExpressionValidator.Text = "";
                PhoneNumberTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                PhoneNumberTextBox_RegularExpressionValidator.Text = "*";
                PhoneNumberTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
