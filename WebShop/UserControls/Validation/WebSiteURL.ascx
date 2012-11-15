<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_WebSiteURL" Codebehind="WebSiteURL.ascx.cs" %>
<asp:TextBox ID="WebSiteURLTextBox" runat="server" Width="150" /><asp:RequiredFieldValidator
    ID="WebSiteURLTextBox_RequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="WebSiteURLTextBox"
    ErrorMessage="webbadress" Text="*" />
<asp:RegularExpressionValidator ID="WebSiteURLTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="WebSiteURLTextBox" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
        Text="*" 
    ErrorMessage="Giltig webbadress t ex: http://www.google.com" />
