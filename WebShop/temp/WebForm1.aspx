<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebShop.temp.WebForm1" Theme="NordicMuslim" %>

<%@ Register src="../UserControls/FileImageManager/ImageManager.ascx" tagname="Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Repeater ID="FilterRepeater" runat="server" >
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lnkFilter" 
                                Text='<%# Container.DataItem %>' 
                                CommandName='<%# Container.DataItem %>'></asp:LinkButton>
            </ItemTemplate>
            
            <SeparatorTemplate>|</SeparatorTemplate>
        </asp:Repeater>
       
    </div>
    
    
    <div id="MainProductImage">
    <shaz:ImageManager ID="ImageManager1" runat="server" InitImageMode="ImagesListingMode" ItemDataProviderType="ProductProvider" FolderUrl="~/ProductImages" />
   </div>
    
    
    
    </form>
</body>
</html>
