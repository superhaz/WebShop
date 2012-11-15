<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_YesNo" Codebehind="YesNo.ascx.cs" %>
<asp:RadioButtonList ID="YesNoRadioButtonList" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Text="Ja" Value="True" />
    <asp:ListItem Text="Nej" Value="False" />
</asp:RadioButtonList>
<asp:RequiredFieldValidator ID="YesNoRadioButtonList_RequiredFieldValidator" runat="server"
    Display="Dynamic" ControlToValidate="YesNoRadioButtonList" ErrorMessage="Ja/Nej"
    Text="*" />
