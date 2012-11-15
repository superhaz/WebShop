<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebShop.temp.WebForm31" EnableViewState="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:HiddenField ID="MessageLabelVisibility" runat="server" />
                <asp:Button ID="TestButton" runat="server" Text="Test" OnClick="TestButton_Click" />
                <asp:Label ID="MessageLabel" runat="server" Text="Hello World" Visible="false" />

            </ContentTemplate>
        </asp:UpdatePanel>
       
    </div>
    </form>
</body>
</html>
