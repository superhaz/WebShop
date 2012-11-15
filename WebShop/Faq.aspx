<%@ Page Title="FAQ - " Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Faq.aspx.cs" Inherits="WebShop.Faq1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
 <div class="SolidDiv Padding Margin">
        <asp:PlaceHolder ID="DefaultPlaceHolder" runat="server">
            <asp:LoginView ID="LoginView" runat="server">
                <RoleGroups>
                    <asp:RoleGroup Roles="SuperAdmins, Administrators">
                        <ContentTemplate>
                            <div style="padding: 10px 10px 10px 0;">
                                <asp:ImageButton ID="EditImageButton" runat="server" ToolTip="Redigera" ImageUrl="~/UserControls/FileImageManager/Images/edit-16.png"
                                    OnClick="EditButton_Click" ImageAlign="AbsMiddle" /><br />
                                <asp:LinkButton ID="EditLinkButton" runat="server" Text="Redigera" OnClick="EditButton_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:RoleGroup>
                </RoleGroups>
            </asp:LoginView>
            <div style="clear:left"></div>
            <h2>
                <asp:Label ID="TitleLabel" runat="server" /></h2>
            <p>
                <asp:Label ID="TextBlockLabel" runat="server" />
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="EditPlaceHolder" runat="server" Visible="false">
            <div style="padding-top: 5px;">
                <asp:HiddenField ID="TextBlockIDHiddenField" runat="server" />
                Titel:<br />
                <asp:TextBox ID="TitleTextBox" runat="server" Width="200" Font-Size="X-Small" Font-Names="'Helvetica Neue' ,Verdana, Arial,  Helvetica, sans-serif" /><br /><br />
                <asp:TextBox ID="TextBlockTextBox" runat="server" TextMode="MultiLine" Width="630"
                    Height="450" Font-Size="X-Small" Font-Names="'Helvetica Neue' ,Verdana, Arial,  Helvetica, sans-serif" />
            </div>
            <div class="RightAlignColumn" style="border: solid 0px black; padding: 10px 10px 10px 0;">
                <asp:ImageButton ID="SaveImageButton" runat="server" OnClick="SaveButton_Click" ToolTip="Spara"
                    ImageUrl="~/UserControls/FileImageManager/images/filesave-32.png" /><br />
                <asp:LinkButton ID="SaveLinkButton" runat="server" Text="Spara" OnClick="SaveButton_Click" />
            </div>
            <div class="RightAlignColumn" style="border: solid 0px black; padding: 10px 10px 10px 0;">
                <asp:ImageButton ID="CancelImageButton" runat="server" OnClick="CancelButton_Click"
                    CausesValidation="false" ToolTip="Avbryt" ImageUrl="~/UserControls/FileImageManager/images/Cancel-32.png" /><br />
                <asp:LinkButton ID="CancelLinkButton" runat="server" Text="Avbryt" OnClick="CancelButton_Click"
                    CausesValidation="false" />
            </div>
            <div style="clear: left;">
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
