<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="App_UserControls_Validation_StreetAddress" Codebehind="StreetAddress.ascx.cs" %>
<asp:TextBox ID="StreetAdressTextBox" Width="150" runat="server" /><br /><asp:RequiredFieldValidator
    ID="StreetAdressTextBox_RequiredFieldValidator" runat="server" Display="Dynamic"
    ControlToValidate="StreetAdressTextBox" ErrorMessage="Ange en gatuadress" Text="*" />