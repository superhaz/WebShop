<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="GroupView.aspx.cs" Inherits="WebShop.GroupView" %>

<%--caching site for performance--%>
<%--<%@OutputCache Duration="300" VaryByParam="CatID" %> --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="RepeaterPlaceHolder" runat="server">
                <asp:LoginView ID="LoginView2" runat="server">
                    <RoleGroups>
                        <asp:RoleGroup Roles="SuperAdmins, Administrators">
                            <ContentTemplate>
                                <div class="Margin Padding">
                                    <h2>
                                        Hantering av produktkatalog
                                    </h2>
                                </div>
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                </asp:LoginView>
                <div style="border: solid 0px black;">
                    <asp:LoginView ID="LoginView1" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                                <ContentTemplate>
                                    <div class="Padding" style="text-align: right;">
                                        <asp:ImageButton ID="AddNewImageButton" runat="server" ToolTip="Lägg till ny produkt"
                                            OnClick="AddNewButton_Click" ImageUrl="~/UserControls/FileImageManager/Images/Add-Product-48.png"
                                            ImageAlign="AbsMiddle" /><br />
                                        <asp:LinkButton ID="AddNewLinkButton" runat="server" Text="Ny produkt" OnClick="AddNewButton_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                    </asp:LoginView>
                    <div class="Margin Padding">
                                    <h2>
                                        <asp:Label ID="CategoryTitleLabel" runat="server" />
                                    </h2>
                                    <asp:Label ID="CategoryShortInfoLabel" runat="server" />
                                </div>
                    <asp:Repeater ID="ItemRepeater" runat="server" OnItemCommand="ItemRepeater_ItemCommand"
                        OnItemDataBound="ItemRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="ProductInList Margin Padding">
                                <asp:PlaceHolder ID="AdminPriorityPlaceHolder" runat="server" Visible="true">
                                    <asp:LoginView ID="LoginView1" runat="server">
                                        <RoleGroups>
                                            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                                                <ContentTemplate>
                                                    <div class="Padding Margin">
                                                        <div class="LeftAlignColumn" style="width: 33%">
                                                            <asp:ImageButton ID="DecreaseButton" runat="server" ToolTip="Flytta vänster" CommandName="DecreasePriority"
                                                                ImageUrl="~/UserControls/FileImageManager/images/Back-16.png" />
                                                        </div>
                                                        <div class="CenterAlignColumn" style="width: 33%">
                                                            <asp:ImageButton ID="LowestPriorityImageButton" runat="server" ToolTip="Högst upp"
                                                                CommandName="LowestPriority" ImageUrl="~/UserControls/FileImageManager/images/Up-16.png" />&nbsp;
                                                            <asp:ImageButton ID="HighestPriorityImageButton" runat="server" ToolTip="Längst ned"
                                                                CommandName="HighestPriority" ImageUrl="~/UserControls/FileImageManager/images/Down-16.png" />
                                                        </div>
                                                        <div class="RightAlignColumn" style="width: 33%">
                                                            <asp:ImageButton ID="IncreaseButton" runat="server" ToolTip="Flytta höger" CommandName="IncreasePriority"
                                                                ImageUrl="~/UserControls/FileImageManager/images/Forward-16.png" />
                                                        </div>
                                                        <div style="clear: left;">
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:RoleGroup>
                                        </RoleGroups>
                                    </asp:LoginView>
                                </asp:PlaceHolder>
                                <div class="ProductInListImage" style="min-height: 150px;">
                                    <asp:ImageButton ID="ItemImage" runat="server" Width="80px" CommandName="View" CommandArgument='<%# Eval("ProductID") %>' />
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
                                <asp:PlaceHolder ID="AdminDeletePlaceHolder" runat="server" Visible="true">
                                    <asp:LoginView ID="LoginView3" runat="server">
                                        <RoleGroups>
                                            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                                                <ContentTemplate>
                                                    <div class="ControlDiv">
                                                        <div style="border: solid 0px yellow; text-align: right;">
                                                            <asp:ImageButton ID="DeleteImageButton" runat="server" ToolTip="Ta bort" CommandName="Delete"
                                                                CssClass="icontext" ImageUrl="~/UserControls/FileImageManager/images/Delete-16.png"
                                                                OnClientClick="javascript:return confirm('Produkten kommer att tas bort. Vill du fortsätta?')" />
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:RoleGroup>
                                        </RoleGroups>
                                    </asp:LoginView>
                                </asp:PlaceHolder>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div style="clear: both;">
                    </div>
                </div>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
