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
using System.Globalization;

public partial class App_UserControls_Validation_Number : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IsFocused)
            {
                // set focus
                //Page.RegisterStartupScript("SetFocus", "<script>document.getElementById('" + NumberTextBox.ClientID + "').focus();</script>");
                ScriptManager.RegisterStartupScript(Page, GetType(), "SetFocus", "<script>document.getElementById('" + NumberTextBox.ClientID + "').focus();</script>", true);
            }
        }
    }

    public bool IsFocused { get; set; }

    public string Text
    {
        get { return NumberTextBox.Text; }
        set { NumberTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { NumberTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { NumberTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_CompareValidator
    {
        set { NumberTextBox_CompareValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            NumberTextBox_RequiredFieldValidator.Text = value;
            NumberTextBox_CompareValidator.Text = value;
        }
    }

    public int Width
    {
        set { NumberTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { NumberTextBox.ToolTip = value; }
    }

    public int Columns
    {
        set { NumberTextBox.Columns = value; }
    }

    public string ValidationGroup
    {
        set
        {
            NumberTextBox.ValidationGroup = value;
            NumberTextBox_CompareValidator.ValidationGroup = value;
            NumberTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public bool AutoPostBack
    {
        set { NumberTextBox.AutoPostBack = value; }
    }

    public bool Enable
    {
        set { NumberTextBox.Enabled = value; }
    }

    public bool EnableNumberLimit { get; set; }

    public int StartNumber { get; set; }

    public int EndNumber { get; set; }

    protected void Number_CustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (EnableNumberLimit)
        {
            if (IsInteger(NumberTextBox.Text))
            {
                int number = int.Parse(NumberTextBox.Text);
                if (number >= StartNumber && number <= EndNumber)
                    e.IsValid = true; // ok
                else
                    e.IsValid = false; // not ok
            }
        }

        Number_CustomValidator.ErrorMessage = "Giltig siffra mellan " + StartNumber.ToString() + "-" + EndNumber.ToString();
    }

    public bool IsInteger(string stringToTest)
    {
        int newVal;
        return int.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal);
    }

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                NumberTextBox_CompareValidator.Text = "";
                NumberTextBox_RequiredFieldValidator.Text = "";
            }
            else
            {
                NumberTextBox_CompareValidator.Text = "*";
                NumberTextBox_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
