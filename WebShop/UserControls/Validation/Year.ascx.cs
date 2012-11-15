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

public partial class App_UserControls_Validation_Year : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return YearTextBox.Text; }
        set { YearTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { YearTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { YearTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            YearTextBox_RequiredFieldValidator.Text = value;
        }
    }

    public int Width
    {
        set { YearTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { YearTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            YearTextBox.ValidationGroup = value;
            YearTextBox_RegularExpressionValidator.ValidationGroup = value;
            YearTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                YearTextBox_RegularExpressionValidator.Text = "";
                YearTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                YearTextBox_RegularExpressionValidator.Text = "*";
                YearTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
