<%@ Page Title="Hantering av kategori - " Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="WebShop.Admin.Category" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <div class="Margin Padding" style="border: solid 0px black;">
        <h2>
            Hantering av kategorier
        </h2>
    </div>
    <asp:PlaceHolder ID="ItemPlaceHolder" runat="server" Visible="false">
        <shaz:ImageManager ID="ImageManager1" runat="server" FolderUrl="~/Admin/CategoryImages"
            ItemDataProviderType="CategoryProvider" InitMode="ImagesListModeOnly" MaxNumberOfImages="1" />
        <div class="SolidDiv Margin Padding">
            <div id="Div1" style="margin-bottom: 10px;">
                <div id="Div2" class="CenterAlignColumn " style="width: 100%;">
                    <div class="LeftAlignColumn" style="border: solid 0px black;">
                        <div>
                            <h3>
                                Kategori: *</h3>
                            <asp:HiddenField ID="CategoryIDHiddenField" runat="server" />
                            <asp:TextBox ID="CategoryNameTextBox" runat="server" Width="300" />
                            <asp:RequiredFieldValidator ID="CategoryNameTextBox_RequiredFieldValidator" runat="server"
                                ControlToValidate="CategoryNameTextBox" Text="" ErrorMessage="&nbsp;* Ange kategori"
                                ValidationGroup="CategoryVG" Display="Dynamic" />
                        </div>
                        <div>
                            <h3>
                                Moms (%):</h3>
                            <asp:TextBox ID="VATTextBox" runat="server" Width="300" />
                            <asp:RequiredFieldValidator ID="VATTextBox_RequiredFieldValidator" runat="server"
                                Display="Dynamic" ControlToValidate="VATTextBox" ErrorMessage="&nbsp;* Ange ett momsvärde"
                                Text="" ValidationGroup="CategoryVG" />
                            <asp:CompareValidator ID="VATTextBox_CompareValidator" ControlToValidate="VATTextBox"
                                Operator="DataTypeCheck" Type="Integer" Text="" runat="server" ErrorMessage="&nbsp;* Ange giltigt momsvärde t ex 20"
                                Display="Dynamic" ValidationGroup="CategoryVG"></asp:CompareValidator>
                        </div>
                        <div>
                            <h3>
                                Kort info:</h3>
                            <asp:TextBox ID="ShortInfoTextBox" runat="server" TextMode="MultiLine" Width="400"
                                Height="50" />
                        </div>
                    </div>
                </div>
                <div style="clear: left;">
                </div>
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="SaveImageButton" runat="server" OnClick="SaveButton_Click" ToolTip="Spara"
                    ImageUrl="~/UserControls/FileImageManager/images/filesave-32.png" ValidationGroup="CategoryVG" /><br />
                <asp:LinkButton ID="SaveLinkButton" runat="server" Text="Spara" OnClick="SaveButton_Click"
                    ValidationGroup="CategoryVG" />
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="CancelImageButton" runat="server" OnClick="CancelButton_Click"
                    CausesValidation="false" ToolTip="Avbryt" ImageUrl="~/UserControls/FileImageManager/images/Cancel-32.png" /><br />
                <asp:LinkButton ID="CancelLinkButton" runat="server" Text="Avbryt" OnClick="CancelButton_Click"
                    CausesValidation="false" />
            </div>
            <div style="clear: left;">
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="RepeaterPlaceHolder" runat="server">
        <div class="SolidDiv Margin Padding">
            <div class="RightAlignColumn" style="width: 100%; border: solid 0px black; margin-bottom: 10px;">
                <asp:ImageButton ID="AddNewImageButton" runat="server" ToolTip="Lägg till ny kategori"
                    OnClick="AddNewButton_Click" ImageUrl="~/UserControls/FileImageManager/Images/Add-Category-32.png"
                    ImageAlign="AbsMiddle" /><br />
                <asp:LinkButton ID="AddNewLinkButton" runat="server" Text="Ny kategori" OnClick="AddNewButton_Click" />
            </div>
            <div style="clear: left;">
            </div>
            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server" Visible="false">
                <div class="Margin Padding">
                    <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="ErrorMessage" Text="Det gick inte att ta bort kategorin eftersom kategorin innehåller produkter. Det går endast att ta bort tomma kategorier!" />
                </div>
            </asp:PlaceHolder>
            <div>
                <table class="Listing" style="width: 100%; padding: 5px; border: dotted 1px #CCC;">
                    <tr>
                        <th align="left">
                            <span style="font-weight: bold;">Kategori</span>
                        </th>
                        <th align="left">
                            <span style="font-weight: bold;">Antal produkter</span>
                        </th>
                        <th align="left">
                            <span style="font-weight: bold;">Moms (%)</span>
                        </th>
                        <th>
                            &nbsp;
                        </th>
                    </tr>
                    <asp:Repeater ID="ItemRepeater" runat="server" OnItemCommand="ItemRepeater_ItemCommand"
                        OnItemDataBound="ItemRepeater_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="4">
                                    <div class="DottedLine">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="CategoryIDHiddenField" runat="server" Value='<%# Eval("CategoryID") %>' />
                                    <%# Eval("CategoryName") %>
                                </td>
                                <td>
                                    <asp:Literal ID="NumProductsInCategoryLiteral" runat="server" Text='<%# Eval("NumProductsInCategory") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="VATLiteral" runat="server" Text='<%# Eval("VAT") %>' />
                                </td>
                                <td style="text-align: right;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="DecreasePriority"
                                        ImageAlign="AbsMiddle" ImageUrl="~/UserControls/FileImageManager/Images/Up-16.png"
                                        ToolTip="Flytta upp" CommandArgument='<%# Eval("CategoryID") %>' />&nbsp;|&nbsp;
                                    <asp:ImageButton ID="ImageButton2" runat="server" CommandName="IncreasePriority"
                                        ImageAlign="AbsMiddle" ImageUrl="~/UserControls/FileImageManager/Images/Down-16.png"
                                        ToolTip="Flytta ned" CommandArgument='<%# Eval("CategoryID") %>' />&nbsp;|&nbsp;
                                    <asp:ImageButton ID="EditImageButton" runat="server" CommandName="Edit" ImageUrl="~/UserControls/FileImageManager/Images/Edit-16.png"
                                        ImageAlign="AbsMiddle" ToolTip="Redigera" CommandArgument='<%# Eval("CategoryID") %>' />&nbsp;|&nbsp;
                                    <asp:ImageButton ID="DeleteImageButton" runat="server" CommandName="Delete" ImageUrl="~/UserControls/FileImageManager/Images/Delete-16.png"
                                        ToolTip="Ta bort" ImageAlign="AbsMiddle" CommandArgument='<%# Eval("CategoryID") %>'
                                        OnClientClick="javascript:return confirm('Kategorin kommer att tas bort. Vill du fortsätta?')" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
