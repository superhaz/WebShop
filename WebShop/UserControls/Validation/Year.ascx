<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_Year" Codebehind="Year.ascx.cs" %>
<asp:TextBox ID="YearTextBox" Columns="4" runat="server" /><asp:RequiredFieldValidator
    ID="YearTextBox_RequiredFieldValidator" runat="server" Display="Dynamic"
    ControlToValidate="YearTextBox" ErrorMessage="År" Enabled="false"
    Text="*" /><asp:RegularExpressionValidator ID="YearTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="YearTextBox" ValidationExpression="^\d{4}$"
        Text="*" ErrorMessage="År t ex: 1999" />