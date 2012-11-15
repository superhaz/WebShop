<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_Email" Codebehind="Email.ascx.cs" %>
<asp:TextBox ID="EmailTextBox" runat="server" Width="150" /><br /><asp:RequiredFieldValidator
    ID="EmailTextBox_RequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="EmailTextBox"
    ErrorMessage="Ange en e-post" Text="*" /><asp:RegularExpressionValidator ID="EmailTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="EmailTextBox" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        Text="*" 
    ErrorMessage="Ange en giltig e-post" />
