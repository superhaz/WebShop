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

public partial class App_UserControls_Validation_YesNo : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Properties
    /// <summary>
    /// Gets or sets a boolean value to the radiobuttonlist accepted values true, false, null
    /// </summary>
    public bool? ValueBool
    {
        get { return ParseBoolValueFromYesNoRadioButtonList(YesNoRadioButtonList); }
        set { ParseBoolValueToYesNoRadioButtonList(YesNoRadioButtonList, value); }
    }

    /// <summary>
    /// Gets or sets a boolean value string to the radiobuttonlist accepted strings "true", "false", "" (ie empty)
    /// </summary>
    public string ValueString
    {
        get { return ParseStringValueFromYesNoRadioButtonList(YesNoRadioButtonList); }
        set { ParseStringValueToYesNoRadioButtonList(YesNoRadioButtonList, value); }
    }

    public bool Enable_RequiredFieldValidator
    {
        set { YesNoRadioButtonList_RequiredFieldValidator.Enabled = value; }
    }

    public string ErrorMessage_RequiredFieldValidator
    {
        set { YesNoRadioButtonList_RequiredFieldValidator.ErrorMessage = value; }
    }

    public string ErrorMessageText
    {
        set
        {
            YesNoRadioButtonList_RequiredFieldValidator.Text = value;
        }
    }

    public string ToolTip
    {
        set { YesNoRadioButtonList.ToolTip = value; }
    }

    public string ValidationGroup
    {
        set
        {
            YesNoRadioButtonList.ValidationGroup = value;
            YesNoRadioButtonList_RequiredFieldValidator.ValidationGroup = value;
        }
    }
    #endregion

    #region parse values
    private bool? ParseBoolValueFromYesNoRadioButtonList(RadioButtonList radioButtonList)
    {
        foreach (ListItem item in radioButtonList.Items)
        {
            if (item.Selected)
            {
                if (Boolean.Parse(item.Value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return null;
    }

    private string ParseStringValueFromYesNoRadioButtonList(RadioButtonList radioButtonList)
    {
        foreach (ListItem item in radioButtonList.Items)
        {
            if (item.Selected)
            {
                if (Boolean.Parse(item.Value))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }

        return null;
    }

    private void ParseBoolValueToYesNoRadioButtonList(RadioButtonList radioButtonList, bool? isYesSelected)
    {
        if (isYesSelected == null)
            return;
        bool boolIsYesSelected = (bool)isYesSelected;

        foreach (ListItem item in radioButtonList.Items)
        {
            if (Boolean.Parse(item.Value))
            {
                if (boolIsYesSelected)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
            else
            {
                if (!boolIsYesSelected)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }

    }

    private void ParseStringValueToYesNoRadioButtonList(RadioButtonList radioButtonList, string isYesSelected)
    {
        if (isYesSelected == null || isYesSelected == "")
            return;
        bool boolIsYesSelected = bool.Parse(isYesSelected);

        foreach (ListItem item in radioButtonList.Items)
        {
            if (Boolean.Parse(item.Value))
            {
                if (boolIsYesSelected)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
            else
            {
                if (!boolIsYesSelected)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }

    }
    #endregion

    public bool ShowErrorMessageInline
    {
        set
        {
            if (value)
            {
                YesNoRadioButtonList_RequiredFieldValidator.Text = "";
            }
            else
            {
                YesNoRadioButtonList_RequiredFieldValidator.Text = "*";
            }
        }
    }
}
