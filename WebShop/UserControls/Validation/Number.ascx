<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_UserControls_Validation_Number" Codebehind="Number.ascx.cs" %>
<asp:TextBox ID="NumberTextBox" runat="server" /><asp:RequiredFieldValidator ID="NumberTextBox_RequiredFieldValidator" runat="server"
    Display="Dynamic" ControlToValidate="NumberTextBox" ErrorMessage="Ange en siffra" Text="*" /><asp:CompareValidator ID="NumberTextBox_CompareValidator" ControlToValidate="NumberTextBox"
    Operator="DataTypeCheck" Type="Integer" Text="*" runat="server" ErrorMessage="Giltig siffra t ex: 50" Display="Dynamic"></asp:CompareValidator>
<asp:CustomValidator ID="Number_CustomValidator"  runat="server" Text="*" 
    ErrorMessage="Giltig siffra mellan 0-300" OnServerValidate="Number_CustomValidator_ServerValidate" Display="Dynamic" />
	 <%--<asp:RegularExpressionValidator ID="YearTextBox_RegularExpressionValidator"
        runat="server" Display="Dynamic" ControlToValidate="NumberTextBox" ValidationExpression="^([0-9]|[0-9]\d|300)$"
        Text="*" ErrorMessage="Giltig siffra mellan 0-300" />--%>