<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_PhoneNumber" Codebehind="PhoneNumber.ascx.cs" %>
<asp:TextBox ID="PhoneNumberTextBox" runat="server" Width="150" /><br /><asp:RequiredFieldValidator
    ID="PhoneNumberTextBox_RequiredFieldValidator" runat="server" Display="Dynamic"
    ControlToValidate="PhoneNumberTextBox" ErrorMessage="Ange ett telefonnummer" Text="*" /><asp:RegularExpressionValidator
        ID="PhoneNumberTextBox_RegularExpressionValidator" runat="server" Display="Dynamic"
        ControlToValidate="PhoneNumberTextBox" ValidationExpression="^(([+]\d{2}[1-9]\d{0,2})|([0]\d{1,4}[-]))((\d{2}(\d{2}){2})|(\d{3}(\d{3})*(\d{2})+))$"
        Text="*" ErrorMessage="Ange ett giltigt telefonnummer" Enabled="false" />