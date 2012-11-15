<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebShop._Default" Trace="false" MasterPageFile="~/MasterPage.master" Title="Välkommen - " %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" runat="Server">
    <div class="MiddleDiv SolidDiv Margin" style="text-align: center; width: 99%;">
        <div class="MiddleImage">
            <asp:ImageButton ID="MainCategoryImageButton" runat="server" OnClick="MainCategoryImageButton_Click" EnableViewState="true" />
            <asp:HiddenField ID="CategoryIDHiddenField" runat="server" />
        </div>
    </div>
    <div class="MainLatestProducts" style="width: 100%;">
        <div style="border: solid 0px black;">
            <asp:Repeater ID="ItemRepeater" runat="server" OnItemCommand="ItemRepeater_ItemCommand" OnItemDataBound="ItemRepeater_ItemDataBound" EnableViewState="true" >
                <ItemTemplate>
                    <div class="ProductInList Margin Padding">
                        <div class="ProductInListImage" style="min-height: 150px;">
                            <asp:ImageButton ID="ItemImage" runat="server" Width="80px" CommandName="View" CommandArgument='<%# Eval("ProductID") %>' EnableViewState="true" />
                            <asp:HiddenField ID="ProductCategoryIDHiddenField" runat="server" Value='<%# Eval("ProductCategoryID") %>' />
                            <asp:HiddenField ID="CategoryIDHiddenField" runat="server" Value='<%# Eval("CategoryID") %>' />
                            <asp:HiddenField ID="FileNameHiddenField" runat="server" Value='<%# Eval("FileName") %>' />
                            <asp:HiddenField ID="ProductIDHiddenField" runat="server" Value='<%# Eval("ProductID") %>' />
                            <asp:HiddenField ID="PriorityHiddenField" runat="server" Value='<%# Eval("Priority") %>' />
                        </div>
                        <div class="ProductInListText">
                            <asp:LinkButton ID="ShowItemLinkButton" runat="server" Text='<%# Eval("Title") %>'
                                CommandName="View" />
                            <div style="text-align: left; height: 45px; border: solid 0px black;">
                                <asp:Label ID="ShortInfoLabel" runat="server" Text='<%# Eval("ShortInfo") %>' /><br />
                            </div>
                            <div style="text-align: left; height: 20px; vertical-align: text-bottom; border: solid 0px black;">
                                <b>
                                    <asp:Literal ID="PriceLiteral" runat="server" Text='<%# Eval("Price") %>' />&nbsp;
                                    SEK</b>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div style="clear: both;">
            </div>
        </div>
        <%--<div class="ProductInList">
            <div class="ProductInListImage">
                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/App_Themes/Standard/images/product6.png"
                    ToolTip="Product" />
            </div>
            <div class="ProductInListText">
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Abaya!" /><br />
                Elegant abaya med mycket hög kvalite...
            </div>
        </div>--%>
    </div>
</asp:Content>
