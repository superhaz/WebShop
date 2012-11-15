<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_SSN" Codebehind="SSN.ascx.cs" %>
<asp:TextBox ID="SSNTextBox" runat="server" Width="150px">
</asp:TextBox><asp:RequiredFieldValidator ID="SSNTextBox_RequiredFieldValidator"
    runat="server" Display="Dynamic" ControlToValidate="SSNTextBox" ErrorMessage="Personnummer"
    Text="*" />
    <asp:CustomValidator ID="SSN_CustomValidator"  runat="server" Text="*" ErrorMessage="10 siffrigt personnummer t ex: 740609-7845 | 811116-7845 | 010913-7598" 
                      OnServerValidate="SSN_CustomValidator_ServerValidate" />
    <%--<asp:RegularExpressionValidator ID="SSNTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="SSNTextBox" ValidationExpression="[0-9]{2}[0-1][0-9][0-3][0-9][-][0-9]{4}"
        Text="*" ErrorMessage="10 siffrigt personnummer t ex: 740609-7845 | 811116-7845 | 010913-7598" />--%>
