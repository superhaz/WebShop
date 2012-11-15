<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ProductView.aspx.cs" Inherits="WebShop.ProductView" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <asp:LoginView ID="LoginView1" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                <ContentTemplate>
                    <div class="Margin Padding">
                        <h2>
                            Hantering av produkt
                        </h2>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
    <div id="MainProductImage">
        <%--<asp:Button ID="ReturnButton" runat="server" Text="Tillbaka" OnClick="ReturnButton_Click" />--%>
        <asp:PlaceHolder ID="CancelPlaceHolder" runat="server">
            <div class="Margin Padding">
                <div class="RightAlignColumn" style="border: solid 0px black; width: 100%;">
                    <asp:ImageButton ID="CancelImageButton" runat="server" ToolTip="Avbryt" ImageUrl="~/UserControls/FileImageManager/Images/Cancel-Product-32.png"
                        OnClick="CancelButton_Click" ImageAlign="AbsMiddle" /><br />
                    <asp:LinkButton ID="CancelLinkButton" runat="server" Text="Avbryt" OnClick="CancelButton_Click" />
                </div>
                <div style="clear: left;">
                </div>
            </div>
        </asp:PlaceHolder>
        <%--Product image--%>
        <shaz:ImageManager ID="ImageManager1" runat="server" FolderUrl="~/ProductImages"
            ItemDataProviderType="ProductProvider" />
    </div>
    <%--Product Information--%>
    <asp:PlaceHolder ID="ShowEditPlaceHolder" runat="server">
        <asp:LoginView ID="LoginView2" runat="server">
            <RoleGroups>
                <asp:RoleGroup Roles="SuperAdmins, Administrators">
                    <ContentTemplate>
                        <div class="RightAlignColumn Padding" style="width: 97%;">
                            <asp:ImageButton ID="EditImageButton" runat="server" ToolTip="Redigera" ImageUrl="~/UserControls/FileImageManager/Images/edit-32.png"
                                ImageAlign="AbsMiddle" OnClick="EditButton_Click" CausesValidation="false" /><br />
                            <asp:LinkButton ID="EditLinkButton" runat="server" Text="Redigera" OnClick="EditButton_Click" />
                        </div>
                        <div style="clear: left">
                        </div>
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
        </asp:LoginView>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="DefaultPlaceHolder" runat="server">
        <asp:UpdatePanel ID="AddToCartUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="ProductTitleAndPrice" class="SolidDiv Padding Margin">
                    <div id="ProductTitle" class="CenterAlignColumn" style="width: 67%; border: solid black 0px;">
                        <div class="LeftAlignColumn">
                            <div style="margin-bottom: 20px;">
                                <h1>
                                    <asp:Label ID="TitleLabel" runat="server" /></h1>
                            </div>
                            <div class="Padding">
                                <ul>
                                    <li>Tillgänglighet: <b>
                                        <asp:Label ID="AvailabilityLabel" runat="server" /></b></li>
                                    <li>Artno:
                                        <asp:Label ID="ArtNoLabel" runat="server" /></li>
                                    <asp:LoginView ID="IsPublishedLoginView" runat="server">
                                        <RoleGroups>
                                            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                                                <ContentTemplate>
                                                    <li>Är publicerad: <b>
                                                        <asp:Label ID="IsPublishedLabel" runat="server" /></b></li>
                                                </ContentTemplate>
                                            </asp:RoleGroup>
                                        </RoleGroups>
                                    </asp:LoginView>
                                </ul>
                            </div>
                            <div style="padding-right: 10px;">
                                <p>
                                    <asp:Label ID="ShortInfoLabel" runat="server" /></p>
                            </div>
                        </div>
                    </div>
                    <div class="RightAlignColumn" style="width: 32%; border: solid black 0px;">
                        <div>
                            <b>PRIS:</b>
                            <h1>
                                <asp:Label ID="PriceLabel" runat="server" />&nbsp;<asp:Label ID="CurrencyLabel" runat="server" /></h1>
                            <asp:HiddenField ID="VATHiddenField" runat="server" />
                        </div>
                        <asp:PlaceHolder ID="ColorPlaceHolder" runat="server" Visible="false">
                            <div style="margin-bottom: 5px;">
                                <asp:DropDownList ID="ColorDropDownList" runat="server" DataTextField="ColorName"
                                    DataValueField="ColorID" Width="110" />
                                <asp:RequiredFieldValidator ID="ColorDropDownList_RequiredFieldValidator" runat="server"
                                    ControlToValidate="ColorDropDownList" Text="" ErrorMessage="<br />* Välj färg"
                                    InitialValue="" Display="Dynamic" ValidationGroup="SelectionVG" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="SizePlaceHolder" runat="server" Visible="false">
                            <div>
                                <asp:LinkButton ID="SwitchToMeasureTableLinkButton" runat="server" Text="Visa måttabell"
                                    OnClick="SwitchToMeasureTableLinkButton_Click" />&nbsp;
                                <asp:DropDownList ID="SizeDropDownList" runat="server" DataTextField="Size" DataValueField="SizeID"
                                    Width="110" />
                                <asp:RequiredFieldValidator ID="SizeDropDownList_RequiredFieldValidator" runat="server"
                                    ControlToValidate="SizeDropDownList" Text="" ErrorMessage="<br />* Välj storlek"
                                    InitialValue="" Display="Dynamic" ValidationGroup="SelectionVG" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="AddToCartPlaceHolder" runat="server">
                            <div class="RightAlignColumn" style="width: 100%; margin-top: 10px;">
                                <asp:Label ID="AddedToCartMessageLabel" runat="server" CssClass="GreenText" Text="Varan har lagts till i kundvagnen<br /><br />"
                                    Visible="false" />
                                <asp:ImageButton ID="AddToCartImageButton" runat="server" ToolTip="Lägg till" OnClick="AddToCartButton_Click"
                                    ImageUrl="~/UserControls/FileImageManager/images/Add_Button-32.png" ValidationGroup="SelectionVG"
                                    ImageAlign="AbsMiddle" />&nbsp;
                                <asp:LinkButton ID="AddToCartLinkButton" runat="server" Text="Lägg till" OnClick="AddToCartButton_Click"
                                    ValidationGroup="SelectionVG" />
                            </div>
                        </asp:PlaceHolder>
                    </div>
                    <div style="clear: left;">
                    </div>
                </div>
                <asp:PlaceHolder ID="MeasureTablePlaceHolder" runat="server" Visible="false">
                    <div class="SolidDiv Padding Margin">
                        <div style="padding-bottom: 10px;">
                            <h1>
                                Måttabell</h1>
                            <p>
                                <b>Normal storlek för en kvinna 160-172 cm lång = storlek 40</b>
                            </p>
                        </div>
                        <div>
                            <table class="ListingWithBorder" style="width: 100%;">
                                <tr>
                                    <td>
                                        <b>Storlek</b>
                                    </td>
                                    <td>
                                        C34
                                    </td>
                                    <td>
                                        C36
                                    </td>
                                    <td>
                                        C38
                                    </td>
                                    <td>
                                        C40
                                    </td>
                                    <td>
                                        C42
                                    </td>
                                    <td>
                                        C44
                                    </td>
                                    <td>
                                        C50
                                    </td>
                                    <td>
                                        C52
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Vidd runt bröst (A)</b>
                                    </td>
                                    <td>
                                        80 cm
                                    </td>
                                    <td>
                                        84 cm
                                    </td>
                                    <td>
                                        88 cm
                                    </td>
                                    <td>
                                        92 cm
                                    </td>
                                    <td>
                                        96 cm
                                    </td>
                                    <td>
                                        100 cm
                                    </td>
                                    <td>
                                        116 cm
                                    </td>
                                    <td>
                                        122 cm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Vidd runt midja (B)</b>
                                    </td>
                                    <td>
                                        66 cm
                                    </td>
                                    <td>
                                        69 cm
                                    </td>
                                    <td>
                                        72 cm
                                    </td>
                                    <td>
                                        76 cm
                                    </td>
                                    <td>
                                        80 cm
                                    </td>
                                    <td>
                                        84 cm
                                    </td>
                                    <td>
                                        99 cm
                                    </td>
                                    <td>
                                        105 cm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Vidd runt stuss (C)</b>
                                    </td>
                                    <td>
                                        90 cm
                                    </td>
                                    <td>
                                        93 cm
                                    </td>
                                    <td>
                                        96 cm
                                    </td>
                                    <td>
                                        99 cm
                                    </td>
                                    <td>
                                        102 cm
                                    </td>
                                    <td>
                                        106 cm
                                    </td>
                                    <td>
                                        120 cm
                                    </td>
                                    <td>
                                        125 cm
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Storlek</b>
                                    </td>
                                    <td colspan="2" class="CenterAlignTD">
                                        34/36<br />
                                        <br />
                                        S
                                    </td>
                                    <td colspan="2" class="CenterAlignTD">
                                        38/40<br />
                                        <br />
                                        M
                                    </td>
                                    <td colspan="2" class="CenterAlignTD">
                                        42/44<br />
                                        <br />
                                        L
                                    </td>
                                    <td colspan="2" class="CenterAlignTD">
                                        50/52<br />
                                        <br />
                                        XL
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div id="ProductInfo" class="SolidDiv Padding Margin">
                    <h1>
                        Produktinformation</h1>
                    <p>
                        <asp:Label ID="FullInfoLabel" runat="server" />
                    </p>
                </div>
                <shaz:CookieManager ID="CookieManager1" runat="server" NumOfExpiresDays="3" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="EditPlaceHolder" runat="server" Visible="false">
        <div id="Div1" class="SolidDiv Padding Margin">
            <div class="LeftAlignColumn" style="border: solid 0px black; width: 520px">
                <div class="Padding">
                    <h3>
                        Titel: *</h3>
                    <asp:TextBox ID="TitleTextBox" runat="server" Width="300" />
                    <asp:RequiredFieldValidator ID="TitleTextBox_RequiredFieldValidator" runat="server"
                        ControlToValidate="TitleTextBox" Text="&nbsp;* Ange titel" ErrorMessage="Titel"
                        ValidationGroup="ProductInfo" Display="Dynamic" />
                </div>
                <div class="Padding">
                    <h3>
                        Tillgänglighet: *</h3>
                    <asp:DropDownList ID="AvailabilityDropDownList" runat="server" DataTextField="AvailabilityName"
                        DataValueField="AvailabilityID" />
                    <asp:RequiredFieldValidator ID="AvailabilityDropDownList_RequiredFieldValidator"
                        runat="server" ControlToValidate="AvailabilityDropDownList" Text="" ErrorMessage="<br />* Välj tillgänglighet"
                        InitialValue="" Display="Dynamic" ValidationGroup="ProductInfo" />
                </div>
                <div class="Padding">
                    <h3>
                        Artikel nr:</h3>
                    <asp:TextBox ID="ArtNoTextBox" runat="server" />
                </div>
                <div class="Padding">
                    <h3>
                        Pris:</h3>
                    <asp:TextBox ID="PriceTextBox" runat="server" />
                </div>
                <div class="Padding">
                    <h3>
                        Valbara storlekar:
                    </h3>
                    <asp:CheckBoxList ID="SizeCheckBoxList" runat="server" RepeatDirection="Horizontal"
                        RepeatColumns="4" DataTextField="Size" DataValueField="SizeID" />
                </div>
                <div class="Padding">
                    <h3>
                        Valbara färger:
                    </h3>
                    <asp:CheckBoxList ID="ColorCheckBoxList" runat="server" RepeatDirection="Horizontal"
                        RepeatColumns="4" DataTextField="ColorName" DataValueField="ColorID" />
                </div>
                <div class="Padding">
                    <h3>
                        Är publicerad:
                        <asp:CheckBox ID="IsPublishedCheckBox" runat="server" Checked="true" /></h3>
                </div>
                <div class="Padding">
                    <h3>
                        Kort info:</h3>
                    <asp:TextBox ID="ShortInfoTextBox" runat="server" TextMode="MultiLine" Width="400"
                        Height="50" />
                    <asp:CustomValidator ID="ShortInfoTextBox_CustomValidator" runat="server" ControlToValidate="ShortInfoTextBox"
                        ErrorMessage="<br>* Max 100 tecken är tillåtna." Text="" ValidateEmptyText="false"
                        OnServerValidate="ShortInfoTextBox_CustomValidator_Validate" ValidationGroup="ProductInfo" /></div>
                <div class="Padding">
                    <h3>
                        Produktinformation:</h3>
                    <asp:TextBox ID="FullInfoTextBox" runat="server" TextMode="MultiLine" Width="400"
                        Height="150" /></div>
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="SaveImageButton" runat="server" OnClick="SaveButton_Click" ToolTip="Spara"
                    ImageUrl="~/UserControls/FileImageManager/images/filesave-32.png" ValidationGroup="ProductInfo" /><br />
                <asp:LinkButton ID="SaveLinkButton" runat="server" Text="Spara" OnClick="SaveButton_Click"
                    ValidationGroup="ProductInfo" />
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="CancelItemImageButton" runat="server" OnClick="CancelItemButton_Click"
                    CausesValidation="false" ToolTip="Avbryt" ImageUrl="~/UserControls/FileImageManager/images/Cancel-32.png" /><br />
                <asp:LinkButton ID="CancelItemLinkButton" runat="server" Text="Avbryt" OnClick="CancelItemButton_Click"
                    CausesValidation="false" />
            </div>
            <div style="clear: left;">
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
