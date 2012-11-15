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

public partial class App_UserControls_Validation_Name : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return NameTextBox.Text; }
        set { NameTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { NameTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public bool Enable_RegularExpressionValidator
    {
        set { NameTextBox_RegularExpressionValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { NameTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RegularExpressionValidator
    {
        set { NameTextBox_RegularExpressionValidator.ErrorMessage = value; }
    }
	public string ErrorMessageText
    {
        set
        {
            NameTextBox_RegularExpressionValidator.Text = value;
            NameTextBox_RequiredFieldValidator.Text = value;
        }
    }
    public int Width
    {
        set { NameTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { NameTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            NameTextBox.ValidationGroup = value;
            NameTextBox_RegularExpressionValidator.ValidationGroup = value;
            NameTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                NameTextBox_RegularExpressionValidator.Text = "";
                NameTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                NameTextBox_RegularExpressionValidator.Text = "*";
                NameTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
