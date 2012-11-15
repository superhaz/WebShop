<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_PostalCode" Codebehind="PostalCode.ascx.cs" %>

<asp:TextBox ID="PostalCodeTextBox" Columns="6" runat="server" /><br /><asp:RegularExpressionValidator
    ID="PostalCodeTextBox_RegularExpressionValidator" runat="server" Display="Dynamic"
    ControlToValidate="PostalCodeTextBox" ValidationExpression="^\d{5}$" Text="*"
    ErrorMessage="Ange ett 5 siffrigt postnummer" /><asp:RequiredFieldValidator ID="PostalCodeTextBox_RequiredFieldValidator"
        runat="server" Display="Dynamic" ControlToValidate="PostalCodeTextBox" ErrorMessage="Ange ett postnummer"
        Text="*" />
            
