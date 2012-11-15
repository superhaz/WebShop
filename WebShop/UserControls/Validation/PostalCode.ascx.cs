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

public partial class App_UserControls_Validation_PostalCode : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return PostalCodeTextBox.Text; }
        set { PostalCodeTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { PostalCodeTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { PostalCodeTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RegularExpressionValidator
    {
        set { PostalCodeTextBox_RegularExpressionValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            PostalCodeTextBox_RequiredFieldValidator.Text = value;
            PostalCodeTextBox_RegularExpressionValidator.Text = value;
        }
    }


    public int Width
    {
        set { PostalCodeTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { PostalCodeTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            PostalCodeTextBox.ValidationGroup = value;
            PostalCodeTextBox_RegularExpressionValidator.ValidationGroup = value;
            PostalCodeTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                PostalCodeTextBox_RegularExpressionValidator.Text = "";
                PostalCodeTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                PostalCodeTextBox_RegularExpressionValidator.Text = "*";
                PostalCodeTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
