<%@ Page Title="Användarinfo - " Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="True" CodeBehind="UserView.aspx.cs" Inherits="WebShop.Admin.UserView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../App_Themes/Standard/Tabs.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.hotkeys.js" type="text/javascript"></script>
    <script src="../scripts/TabScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <div class="Padding">
        <asp:HiddenField ID="SelectedTabHiddenField" runat="server" Value="#tab-1" />
        <div id="tabz">
            <asp:PlaceHolder ID="AboutTabPlaceHolder" runat="server">
                <div id="tab1" class="tab" style="display: block">
                    <a href="#tab-1">Users</a></div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="ConfigurationTabPlaceHolder" runat="server">
                <div id="tab2" class="tab" style="display: block">
                    <a href="#tab-2">Users and Roles</a></div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="LicensingTabPlaceHolder" runat="server">
                <div id="tab3" class="tab" style="display: block">
                    <a href="#tab-3">Roles</a></div>
            </asp:PlaceHolder>
            <div id="tab4" class="tab" style="display: block; border: solid black 0px; text-align: center;
                margin-top: -5px; height: 11px; width: 50px; margin-left: 5px;">
                <asp:Label ID="AjaxLoadingLabel" runat="server" />
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div id="TabContent">
            <div id="tab-1" style="display: block" class="tab">
                <div class="detailsContainer emptyTab">
                   
                    <asp:UpdatePanel ID="ChildDetailUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <shaz:CreateUserWithRole ID="CreateUserWithRole1" runat="server" />
                            <div class="Line" style="margin-top: 20px; padding-bottom: 10px;">
                            </div>
                            <shaz:ManageUsers ID="ManageUsers1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="tab-2" style="display: block" class="tab">
                <div class="detailsContainer emptyTab">
                    <asp:UpdatePanel ID="UsersAndRolesUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <shaz:UsersAndRoles ID="UsersAndRoles1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="tab-3" style="display: block" class="tab">
                <div class="detailsContainer emptyTab">
                    <asp:UpdatePanel ID="RolesUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <shaz:ManageRoles ID="ManageRoles1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
