<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_Name" Codebehind="Name.ascx.cs" %>
<asp:TextBox ID="NameTextBox" runat="server" Width="150" /><br /><asp:RequiredFieldValidator
    ID="NameTextBox_RequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="NameTextBox"
    ErrorMessage="Ange ett namn" Text="*" /><asp:RegularExpressionValidator ID="NameTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="NameTextBox" ValidationExpression="^((?:[A-Ö](?:('|(?:[a-ö]{1,3}))[A-Ö])?[a-ö]+)|(?:[A-Ö]\.))(?:([ -])((?:[A-Ö](?:('|(?:[a-ö]{1,3}))[A-Ö])?[a-ö]+)|(?:[A-Ö]\.)))?$"
        Text="*" ErrorMessage="Ange ett giltigt namn" Enabled="false" />