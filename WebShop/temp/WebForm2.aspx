<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebShop.temp.WebForm2" EnableViewState="true" %>

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
                <asp:Button ID="TestButton" runat="server" Text="Test" OnClick="TestButton_Click" />
                <asp:Label ID="MessageLabel" runat="server" Text="Hello World" Visible="false" />
                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ColorNameID"
                        DataSourceID="SqlDataSource1" EnableModelValidation="True">
                        <Columns>
                            <asp:BoundField DataField="ColorNameID" HeaderText="ColorNameID" InsertVisible="False"
                                ReadOnly="True" SortExpression="ColorNameID" />
                            <asp:BoundField DataField="ColorName" HeaderText="ColorName" SortExpression="ColorName" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WebShopDBConnectionString %>"
                        SelectCommand="SELECT [ColorNameID], [ColorName] FROM [ColorName]"></asp:SqlDataSource>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="TestButton2" runat="server" Text="Test" OnClick="TestButton2_Click" />
                <asp:Label ID="Label1" runat="server" Text="Hello World" Visible="true" />
                </ContentTemplate>
                </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
