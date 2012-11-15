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

public partial class App_UserControls_Validation_SSN : System.Web.UI.UserControl
{
    private string ssnFormated; // formatted ssn as: yyyyMMdd-xxxx

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        get { return SSNTextBox.Text; }
        set { SSNTextBox.Text = value; }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { SSNTextBox_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { SSNTextBox_RequiredFieldValidator.ErrorMessage = value; }
    }

    //public string ErrorMessage_RegularExpressionValidator
    //{
    //    set { SSNTextBox_RegularExpressionValidator.ErrorMessage = value; }
    //}

    public int Width
    {
        set { SSNTextBox.Width = value; }
    }

    public string ToolTip
    {
        set { SSNTextBox.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set 
        { 
            SSNTextBox.ValidationGroup = value;
            //SSNTextBox_RegularExpressionValidator.ValidationGroup = value;
            SSNTextBox_RequiredFieldValidator.ValidationGroup = value;
        }
    }

    #region SSN Validation

    public bool IsInteger(string stringToTest)
    {
        int newVal;
        return int.TryParse(stringToTest, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out newVal);
    }

    protected void SSN_CustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (SSNIsValid(SSNTextBox.Text.Trim()))
        {
            SSNTextBox.Text = ssnFormated;
            e.IsValid = true; // ok
        }
        else
        {
            e.IsValid = false; // not ok
        }
    }

    private bool SSNIsValid(string ssn)
    {
        if (ssn.Length == 12)       // yyyyMMddxxxx
            return SSNWith12TokenIsValid(ssn);
        else if (ssn.Length == 10)  // yyMMddxxxx
            return SSNWith10TokenIsValid(ssn);
        else if (ssn.Length == 13)  // yyyyMMdd-xxxx
            return SSNWith13TokenIsValid(ssn);
        else if (ssn.Length == 11)   // yyMMdd-xxxx
            return SSNWith11TokenIsValid(ssn);

        return false;
    }

    private bool SSNWith12TokenIsValid(string ssn)
    {
        string yearString = ssn.Substring(0, 4);
        string monthString = ssn.Substring(4, 2);
        string dayString = ssn.Substring(6, 2);
        string checkDigitsString = ssn.Substring(8, 4);

        return SSNDigitsIsValid(yearString, monthString, dayString, checkDigitsString);
    }

    private bool SSNWith10TokenIsValid(string ssn)
    {
        string yearString = ssn.Substring(0, 2);
        string monthString = ssn.Substring(2, 2);
        string dayString = ssn.Substring(4, 2);
        string checkDigitsString = ssn.Substring(6, 4);

        return SSNDigitsIsValid(yearString, monthString, dayString, checkDigitsString);
    }

    private bool SSNWith13TokenIsValid(string ssn)
    {
        string yearString = ssn.Substring(0, 4);
        string monthString = ssn.Substring(4, 2);
        string dayString = ssn.Substring(6, 2);
        string checkDigitsString = ssn.Substring(9, 4);
        bool containsMinusToken = ssn.Substring(8, 1).Contains("-");

        return SSNDigitsIsValid(yearString, monthString, dayString, checkDigitsString) && containsMinusToken;
    }

    private bool SSNWith11TokenIsValid(string ssn)
    {
        string yearString = ssn.Substring(0, 2);
        string monthString = ssn.Substring(2, 2);
        string dayString = ssn.Substring(4, 2);
        string checkDigitsString = ssn.Substring(7, 4);
        bool containsMinusToken = ssn.Substring(6, 1).Contains("-") || ssn.Substring(6, 1).Contains("+");

        return SSNDigitsIsValid(yearString, monthString, dayString, checkDigitsString) && containsMinusToken;
    }

    private bool SSNDigitsIsValid(string yearString, string monthString, string dayString, string checkDigitsString)
    {
        if (IsInteger(yearString) && IsInteger(monthString) && IsInteger(dayString) && (IsInteger(checkDigitsString) || checkDigitsString.Contains("xxxx") ) )
        {
            int year = int.Parse(yearString);
            int month = int.Parse(monthString);
            int day = int.Parse(dayString);
            //int checkDigits = int.Parse(checkDigitsString);
            // check if year is in format 'yy' or 'yyyy'
            if (year < 100)
            {
                ssnFormated = yearString + monthString + dayString + "-" + checkDigitsString;

                if (YearWith2DigitsIsValid(year) && MonthIsValid(month) && DayIsValid(day) &&  CheckSumIsValid(ssnFormated))
                    return true;
                else
                    return false;
            }
            else
            {
                ssnFormated = yearString.Substring(2,2) + monthString + dayString + "-" + checkDigitsString;
                if (YearWith4DigitsIsValid(year) && MonthIsValid(month) && DayIsValid(day) &&  CheckSumIsValid(ssnFormated))
                    return true;
                else
                    return false;
            }
            
        }
        else
        {
            return false;
        }
    }

    private bool YearWith2DigitsIsValid(int year)
    {
        // verifies that a born year is between 00 and 99
        if (year >= 0 && year <= 99)
            return true;
        else
            return false;
    }

    private bool YearWith4DigitsIsValid(int year)
    {
        // verifies that a person isn't older than 150 years and that his birth year arent in future
        if (year > (DateTime.Today.Year - 150) && year <= DateTime.Today.Year)
            return true;
        else
            return false;
    }

    private bool MonthIsValid(int month)
    {
        if (month >= 1 && month <= 12)
            return true;
        else
            return false;
    }

    private bool DayIsValid(int day)
    {
        if (day >= 1 && day <= 31)
            return true;
        else
            return false;
    }

    //private bool CheckDigitsIsValid(int checkDigits)
    //{
    //    if (checkDigits >= 0 && checkDigits <= 9999)
    //        return true;
    //    else
    //        return false;
    //}

    /// <summary>
    /// assumes the identitynumber is on the form "yyMMdd-nnnn"
    /// </summary>
    /// <param name="pIdentityNumber"></param>
    /// <returns></returns>
    private bool CheckSumIsValid(string ssn)
    {
        // check if it is temporary ssn
        if (ssn.Contains("xxxx"))
        {
            return true;
        }
        else
        {
            int i;
            int multiplier, digit, sum, total = 0;

            string cleanNumber = ssn.Substring(0, 6) + ssn.Substring(7);

            for (i = 1; i <= cleanNumber.Length; i++)
            {
                string number = cleanNumber.Substring(i - 1, 1);

                multiplier = 1 + (i % 2);
                digit = int.Parse(number);
                sum = digit * multiplier;
                if (sum > 9)
                    sum -= 9;
                total += sum;
            }
            return (total % 10 == 0);
        }
    }

    #endregion

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                SSNTextBox_RequiredFieldValidator.Text = "";
                SSN_CustomValidator.Text = "";
            }
            else
            {
                SSNTextBox_RequiredFieldValidator.Text = "*";
                SSN_CustomValidator.Text = "*";
            }
        }
    }

    #region code for using the control
    // example code for next button
    //protected void NextButton_Click(object sender, EventArgs e)
    //{
    //    if (Page.IsValid)
    //    {
    //          Response.Redirect("http://google.com");
    //    }
    //}
    #endregion
}
