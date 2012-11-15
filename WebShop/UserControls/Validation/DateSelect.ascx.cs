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

/** DateSelect v0.01
 * Created By: Hassan El-Saabi 2009
 */

public partial class App_UserControls_Validation_DateSelect : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(DateTextBox.Text))
            SelectedDate = Convert.ToDateTime(DateTextBox.Text);

    }

    public string Text
    {
        get { return DateTextBox.Text; }
        set { DateTextBox.Text = value; }
    }

    public bool Enable_MaskedEditExtender
    {
        set { DateTextBox_MaskedEditExtender.Enabled = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { DateTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { DateTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessage_RangeValidator
    {
        set { DateTextBox_RangeValidator.ErrorMessage = value; }
    }

    public int Width
    {
        set { DateTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { DateTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            DateTextBox.ValidationGroup = value;
            DateTextBox_RangeValidator.ValidationGroup = value;
            DateTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    public string CssClass
    {
        set
        {
            DateTextBox_CalendarExtender.CssClass = value;
        }
    }
	
	public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                DateTextBox_RangeValidator.Text = "";
                DateTextBox_RequiredFieldValidator.Text = "";
                EarliestDateFromNowInDays_CustomValidator.Text = "";
            }
            else
            {
                DateTextBox_RangeValidator.Text = "*";
                DateTextBox_RequiredFieldValidator.Text = "*";
                EarliestDateFromNowInDays_CustomValidator.Text = "*";
            }
        }
    }

    public DateTime SelectedDate
    {
        set { DateTextBox_CalendarExtender.SelectedDate = value; }
    }

    private string earliestDateFromNowInDays;
    public string EarliestDateFromNowInDays
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                earliestDateFromNowInDays = value;
            }
        }
    }

    public bool EnableEarliestDateFromNowInDays_CustomValidator
    {
        set
        {
            EarliestDateFromNowInDays_CustomValidator.Enabled = value;
        }
    }

    protected void EarliestDateFromNowInDays_CustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (earliestDateFromNowInDays == null)
            throw new Exception("EarliestDateFromNowInDays property need to be set in aspx file.");

        if (!string.IsNullOrEmpty(DateTextBox.Text))
        {
            TimeSpan timeSpan = Convert.ToDateTime(DateTextBox.Text) - DateTime.Now;
            int days = (int)timeSpan.TotalDays;
            days++; // fixes the 1 day problem
            if (days >= Convert.ToInt32(earliestDateFromNowInDays))
            {
                e.IsValid = true;
            }
            else
            {
                EarliestDateFromNowInDays_CustomValidator.ErrorMessage = "Tidigaste datumet är " + earliestDateFromNowInDays + " dagar från dagens datum.";
                e.IsValid = false;
            }
        }
    }
}
