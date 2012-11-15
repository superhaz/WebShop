<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_DateSelect" Codebehind="DateSelect.ascx.cs" %>

<asp:TextBox ID="DateTextBox" runat="server" Width="80px" CssClass="icontext" />
<ajaxToolkit:CalendarExtender ID="DateTextBox_CalendarExtender" runat="server" Enabled="True"
    Format="yyyy-MM-dd" PopupButtonID="imbShowToDateCalendar" TargetControlID="DateTextBox">
</ajaxToolkit:CalendarExtender>
<asp:ImageButton ID="imbShowToDateCalendar" CssClass="icontext" runat="server" CausesValidation="false"
    ImageUrl="~/UserControls/Validation/calendar_button.png" />
<ajaxToolkit:MaskedEditExtender ID="DateTextBox_MaskedEditExtender" runat="server"
    TargetControlID="DateTextBox" Mask="9999/99/99" MaskType="Date" MessageValidatorTip="true"
    OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" />
<asp:RangeValidator ID="DateTextBox_RangeValidator" runat="server" Display="Dynamic"
    ControlToValidate="DateTextBox" ErrorMessage="Välj ett giltigt datum ex: 2000-10-22"
    Type="Date" Text="*" MaximumValue="2200-01-01" MinimumValue="1900-01-01" />
<asp:RequiredFieldValidator ID="DateTextBox_RequiredFieldValidator" runat="server"
    Display="Dynamic" ControlToValidate="DateTextBox" ErrorMessage="Välj ett datum"
    Text="*" />
<asp:CustomValidator ID="EarliestDateFromNowInDays_CustomValidator" runat="server"
    Text="*" CssClass="ErrorText" OnServerValidate="EarliestDateFromNowInDays_CustomValidator_ServerValidate" />
