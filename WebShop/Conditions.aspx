<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Conditions.aspx.cs" Inherits="WebShop.Conditions1" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Villkor - <%= System.Web.Configuration.WebConfigurationManager.AppSettings["CompanyName"]%></title>
</head>
<%--<body class="Printpage" onload="window.print()">--%>
<body class="Printpage">
    <form id="form1" runat="server">
   <div class="SolidDiv SolidDivPrint Padding Margin">
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
    </form>
</body>
</html>
