<%@ Page Title="Kassan - " Language="C#" MasterPageFile="~/MasterPage.Master" Async="true"
    AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebShop.Checkout" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <%--<script type="text/javascript" src="scripts/jquery-1.4.2.js"></script>--%>
    <script type="text/javascript">
        function ConfirmOrder() {

            if (!confirm('Beställningen kommer att slutföras. Vill du fortsätta?'))
                return false;
            // http: //forums.asp.net/t/963412.aspx
            // from: http://www.electrictoolbox.com/jquery-scroll-top/
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            return true;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <shaz:CookieManager ID="CookieManager1" runat="server" NumOfExpiresDays="3" />
            <asp:PlaceHolder ID="ShoppingCartPlaceHolder" runat="server">
                <h2 class="Title">
                    Kassan
                </h2>
                <div class="SolidDiv Listing Margin Padding">
                    <table class="Listing" style="width: 100%;">
                        <tr>
                            <th style="width: 70px;">
                                <span style="font-weight: bold;">Beskrivning</span>
                            </th>
                            <th align="left">
                                &nbsp;
                            </th>
                            <th align="left">
                                <span style="font-weight: bold;">Antal</span>
                            </th>
                            <th align="left">
                                <span style="font-weight: bold;">Storlek</span>
                            </th>
                            <th align="left">
                                <span style="font-weight: bold;">Färg</span>
                            </th>
                            <th align="left">
                                <span style="font-weight: bold;">Pris</span>
                            </th>
                            <th align="left">
                                <span style="font-weight: bold;">Moms</span>
                            </th>
                            <th class="RightAlignTH">
                                <span style="font-weight: bold;">Summa</span>
                            </th>
                        </tr>
                        <asp:Repeater ID="ItemRepeater" runat="server" OnItemDataBound="ItemRepeater_ItemDataBound"
                            OnItemCommand="ItemRepeater_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="8">
                                        <div class="DottedLine">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="OrderIDHiddenField" runat="server" Value='<%# Eval("OrderiD") %>' />
                                        <asp:HiddenField ID="OrderRowIDHiddenField" runat="server" Value='<%# Eval("OrderRowID") %>' />
                                        <asp:HiddenField ID="ProductIDHiddenField" runat="server" Value='<%# Eval("ProductID") %>' />
                                        <asp:HiddenField ID="FileNameHiddenField" runat="server" Value='<%# Eval("FileName") %>' />
                                        <asp:ImageButton ID="ItemImage" runat="server" ImageAlign="AbsMiddle" CommandName="View" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="ProductLinkButton" runat="server" CommandName="View" Text='<%# Eval("ProductTitle") %>' />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="DecreaseQuantityImageButton" runat="server" ToolTip="Minska"
                                            CommandName="DecreaseQuantity" ImageUrl="~/UserControls/FileImageManager/Images/Minus-16.png" />
                                        <%# Eval("Quantity") %>
                                        <asp:ImageButton ID="IncreaseQuantityImageButton" runat="server" ToolTip="Öka" CommandName="IncreaseQuantity"
                                            ImageUrl="~/UserControls/FileImageManager/Images/Plus-16.png" />
                                    </td>
                                    <td>
                                        <%# Eval("Size") %>
                                    </td>
                                    <td>
                                        <%# Eval("Color") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="PriceLiteral" runat="server" Text='<%# Eval("Price") %>' />
                                        SEK
                                    </td>
                                    <td>
                                        <asp:Literal ID="VATLiteral" runat="server" Text='<%# Eval("VAT") %>' />
                                        %
                                    </td>
                                    <td class="RightAlignTD">
                                        <asp:Literal ID="SumLiteral" runat="server" Text='<%# Eval("Sum") %>' />
                                        SEK
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <div class="SolidDiv Listing Margin Padding">
                    <table class="Listing" style="width: 100%;">
                        <tr>
                            <td class="RightAlignTD" style="width: 400px;">
                                Summa:
                            </td>
                            <td class="RightAlignTD">
                                <asp:Label ID="SumLabel" runat="server" />&nbsp;<asp:Label ID="SumCurrencyLabel"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="RightAlignTD">
                                Frakt:
                            </td>
                            <td class="RightAlignTD">
                                <asp:Label ID="DeliveryCostLabel" runat="server" />&nbsp;<asp:Label ID="DeliveryCurrencyLabel"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="RightAlignTD">
                                Fakturering:
                            </td>
                            <td class="RightAlignTD">
                                <asp:Label ID="PaymentCostLabel" runat="server" />&nbsp;<asp:Label ID="PaymentCurrencyLabel"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="RightAlignTD">
                                <span style="font-weight: bold; color: Black;">TOTAL:</span>
                            </td>
                            <td class="RightAlignTD">
                                <span style="font-size: large; font-weight: bold; color: Black;">
                                    <asp:Label ID="TotalPriceLabel" runat="server" />&nbsp;<asp:Label ID="TotalCurrencyLabel"
                                        runat="server" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="RightAlignTD">
                                Varav moms:
                            </td>
                            <td class="RightAlignTD">
                                <asp:Literal ID="VATLiteral" runat="server" />&nbsp;<asp:Label ID="VATCurrencyLabel"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h2 class="Title">
                    Leveranstyp
                </h2>
                <div class="SolidDiv LeftAlignColumn Padding Margin" style="width: 635px;">
                    <asp:Repeater ID="DeliveryRepeater" runat="server" OnItemDataBound="DeliveryRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="DottedDiv Margin Padding CenterAlignColumn" style="height: 70px;">
                                <asp:Image ID="DeliveryImage" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' /><br />
                                <asp:RadioButton ID="DeliveryRadioButton" runat="server" GroupName="DeliveryGN" AutoPostBack="true"
                                    Text='<%# Eval("DeliveryName") %>' OnCheckedChanged="DeliveryRadioButton_CheckedChanged" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="DeliveryInfoPlaceHolder" runat="server" Visible="false">
                        <div class="LeftAlignColumn" style="margin: 25px 0 0 25px;">
                            <asp:Label ID="DeliveryInfoLabel" runat="server" />
                        </div>
                    </asp:PlaceHolder>
                    <div style="clear: left;">
                    </div>
                    <asp:Label ID="DeliveryRequiredMessageLabel" runat="server" CssClass="RedText" Text="<br />* Välj leveranstyp."
                        Visible="false" />
                </div>
                <div style="clear: left;">
                </div>
                <h2 class="Title">
                    Faktureringstyp
                </h2>
                <div class="SolidDiv LeftAlignColumn Padding Margin" style="width: 635px;">
                    <asp:Repeater ID="PaymentRepeater" runat="server" OnItemDataBound="PaymentRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="DottedDiv Margin Padding CenterAlignColumn">
                                <asp:Image ID="PaymentImage" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' /><br />
                                <asp:RadioButton ID="PaymentRadioButton" runat="server" GroupName="PaymentGN" AutoPostBack="true"
                                    Text='<%# Eval("PaymentName") %>' OnCheckedChanged="PaymentRadioButton_CheckedChanged" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="PaymentInfoPlaceHolder" runat="server" Visible="false">
                        <div class="LeftAlignColumn" style="margin: 10px 0 0 25px; width: 250px;">
                            <asp:Label ID="PaymentInfoLabel" runat="server" />
                        </div>
                    </asp:PlaceHolder>
                    <div style="clear: left;">
                    </div>
                    <asp:Label ID="PaymentRequiredMessageLabel" runat="server" CssClass="RedText" Text="<br />* Välj faktureringstyp."
                        Visible="false" />
                </div>
                <div style="clear: left;">
                </div>
                <h2 class="Title">
                    Leveransinformation
                </h2>
                <div class="SolidDiv LeftAlignColumn Padding Margin" style="width: 635px;">
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Förnamn: *</h3>
                        <shaz:Name ID="FirstName" runat="server" ValidationGroup="CheckoutVG" ShowErrorMessageInline="true" />
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Efternamn: *</h3>
                        <shaz:Name ID="LastName" runat="server" ValidationGroup="CheckoutVG" ShowErrorMessageInline="true" />
                    </div>
                    <div style="clear: left;">
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Adress: *</h3>
                        <shaz:StreetAddress ID="StreetAddress" runat="server" ValidationGroup="CheckoutVG"
                            ShowErrorMessageInline="true" />
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Postnummer: *</h3>
                        <shaz:PostalCode ID="PostalCode" runat="server" ValidationGroup="CheckoutVG" ShowErrorMessageInline="true" />
                    </div>
                    <div style="clear: left;">
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Ort: *</h3>
                        <shaz:Name ID="CityName" runat="server" ValidationGroup="CheckoutVG" ShowErrorMessageInline="true"
                            ErrorMessage_RegularExpressionValidator="Ange en giltig ort" ErrorMessage_RequiredFieldValidator="Ange en ort" />
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Epost: *</h3>
                        <shaz:Email ID="Email" runat="server" ValidationGroup="CheckoutVG" ShowErrorMessageInline="true" />
                    </div>
                    <div style="clear: left;">
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Telefon:</h3>
                        <shaz:PhoneNumber ID="HomePhoneNumber" runat="server" Enable_RequiredFieldValidator="false"
                            ValidationGroup="CheckoutVG" ShowErrorMessageInline="true" />
                    </div>
                    <div class="LeftAlignColumn Padding">
                        <h3>
                            Mobil: *</h3>
                        <shaz:PhoneNumber ID="MobilePhoneNumber" runat="server" ValidationGroup="CheckoutVG" en
                            ShowErrorMessageInline="true" />
                    </div>
                </div>
                <div style="clear: left;">
                </div>
                <div class="RightAlignColumn Padding Margin" style="width: 635px;">
                    <div style="float: right; border: solid black 0px; vertical-align: text-top; margin-bottom: 10px;">
                        <asp:CheckBox ID="AcceptTermsCheckBox" runat="server" Text="" />
                    </div>
                    <div style="float: right; border: solid black 0px; vertical-align: text-top;">
                        Jag har läst och accepterar butikens
                        <asp:HyperLink runat="server" ID="ConditionsHyperLink" Target="_blank" NavigateUrl="~/Conditions.aspx">
                        ordervillkor
                        </asp:HyperLink></div>
                    <div style="float: right; border: solid black 0px; vertical-align: text-top; margin-right: 10px;">
                        <asp:Label ID="AcceptTermsRequiredMessageLabel" runat="server" CssClass="RedText"
                            Text="* Kryssa för att du accepterar butikens villkor.<br><br>" Visible="false" />
                    </div>
                    <div style="clear: right;">
                    </div>
                    <div class="SolidDiv" style="float: right; vertical-align: text-top; padding: 0px 3px;"
                        onmouseover="this.style.fontWeight='bold'; this.style.border = 'solid 3px #CCC'"
                        onmouseout="this.style.fontWeight='normal';this.style.border = 'solid 1px #CCC'">
                        <asp:ImageButton ID="CompleteOrderImageButton" runat="server" ToolTip="Slutför beställning"
                            ValidationGroup="CheckoutVG" CausesValidation="true" OnClick="CompleteOrderButton_Click"
                            ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Standard/images/Success-48.png" />&nbsp;
                        <asp:LinkButton ID="CompleteOrderLinkButton" runat="server" Text="SLUTFÖR BESTÄLLNINGEN"
                            ValidationGroup="CheckoutVG" ForeColor="Black" CausesValidation="true" OnClick="CompleteOrderButton_Click" />
                    </div>
                    <div style="clear: right;">
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="OrderCompletedPlaceHolder" runat="server" Visible="false">
                <div class="Margin Padding">
                    Din order har slutförts. TACK för din beställning!<br />
                    Ett bekräftelsemail har skickats till din email.<br />
                    För att skriva ut order klicka här.<asp:HyperLink runat="server" ID="PrintHyperLinkImage"
                        Target="_blank">
                        <%--<img alt="Skriv ut" src="../Images/32x32/printer.png" alt="Skriv ut" />--%>
                        <asp:Image ID="PrinterImage" runat="server" ImageUrl="~/App_Themes/Standard/images/agt_print-32.png"
                            ToolTip="Skriv ut" />
                    </asp:HyperLink>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="EmptyShoppingCartPlaceHolder" runat="server" Visible="false">
                <div class="Margin Padding">
                    Din kundvagn är tom.
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="InvoicePlaceHolder" runat="server" Visible="false">
                <table border="1" width="700" cellpadding="0" cellspacing="0" style="margin-bottom: 5px;
                    font-size: 65%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                    <tr>
                        <td>
                            <div style="margin: 10px;">
                                <h2 style="font-size: 140%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                                    Kvitto
                                </h2>
                                Leveransadress:<br />
                                <asp:Label ID="NameLabel" runat="server" /><br />
                                <asp:Label ID="AddressLabel" runat="server" /><br />
                                <asp:Label ID="PostalAddressLabel" runat="server" /><br />
                                <br />
                                <asp:Label ID="PhonesLabel" runat="server" /><br />
                                Email:
                                <asp:HyperLink ID="EmailHyperLink" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
                <table border="1" width="700" cellpadding="0" cellspacing="0" style="margin-bottom: 5px;
                    font-size: 65%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                    <tr>
                        <td>
                            <div style="margin: 10px;">
                                <table width="675" style="font-size: 65%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                                    <tr>
                                        <td style="width: 180px; text-align: left;">
                                            <b>Produktnamn</b>
                                        </td>
                                        <td style="width: 80px; text-align: left;">
                                            <b>Antal</b>
                                        </td>
                                        <td style="width: 80px; text-align: left;">
                                            <b>Storlek</b>
                                        </td>
                                        <td style="width: 80px; text-align: left;">
                                            <b>Färg</b>
                                        </td>
                                        <td style="text-align: left;">
                                            <b>Pris</b>
                                        </td>
                                        <th>
                                            <span style="font-weight: bold;">Moms (%)</span>
                                        </th>
                                        <td style="text-align: right;">
                                            <b>Summa</b>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="InvoiceRepeater" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <%# Eval("ProductTitle") %>
                                                </td>
                                                <td style="text-align: left;">
                                                    <%# Eval("Quantity") %>
                                                </td>
                                                <td style="text-align: left;">
                                                    <%# Eval("Size") %>
                                                </td>
                                                <td style="text-align: left;">
                                                    <%# Eval("Color") %>
                                                </td>
                                                <td style="text-align: left;">
                                                    <%# new WebShop.BLL.UtilHelper().RoundPrice(Eval("Price").ToString())%>
                                                    SEK
                                                </td>
                                                <td>
                                                    <%# Eval("VAT") %>
                                                    %
                                                </td>
                                                <td style="text-align: right;">
                                                    <%# new WebShop.BLL.UtilHelper().RoundPrice(Eval("Sum").ToString())%>
                                                    SEK
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table border="1" width="700" cellpadding="0" cellspacing="0" style="margin-bottom: 5px;
                    font-size: 65%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                    <tr>
                        <td>
                            <div style="margin: 10px;">
                                <table width="675" style="font-size: 65%; font-family: Verdana, Arial,  Helvetica, sans-serif;">
                                    <tr>
                                        <td align="right" width="400">
                                            Summa:
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="SumInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="SumInvoiceCurrencyLabel"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Leveranstyp: <b>
                                                <asp:Label ID="DeliveryTypeLabel" runat="server" /></b>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="DeliveryCostInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="DeliveryCurrencyInvoiceLabel"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Faktureringstyp: <b>
                                                <asp:Label ID="PaymentTypeLabel" runat="server" /></b>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="PaymentCostInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="PaymentCurrencyInvoiceLabel"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <span style="font-weight: bold; color: Black;">TOTAL:</span>
                                        </td>
                                        <td align="right">
                                            <span style="font-size: 130%; font-weight: bold; color: Black;">
                                                <asp:Label ID="TotalPriceInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="TotalCurrencyInvoiceLabel"
                                                    runat="server" />
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Varav moms:
                                        </td>
                                        <td align="right">
                                            <asp:Literal ID="VATInvoiceLiteral" runat="server" />&nbsp;<asp:Label ID="VATCurrencyInvoiceLabel"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
