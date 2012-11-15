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

public partial class App_UserControls_Validation_StreetAddress : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return StreetAdressTextBox.Text; }
        set { StreetAdressTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { StreetAdressTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { StreetAdressTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            StreetAdressTextBox_RequiredFieldValidator.Text = value;
        }
    }

    public int Width
    {
        set { StreetAdressTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { StreetAdressTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            StreetAdressTextBox.ValidationGroup = value;
            StreetAdressTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                StreetAdressTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                StreetAdressTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
