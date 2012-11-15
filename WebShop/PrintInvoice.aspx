<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintInvoice.aspx.cs" Inherits="WebShop.PrintInvoice" Title="Faktura utskrift - " %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<%--<body class="Printpage" onload="window.print()">--%>
<body class="Printpage">
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder ID="InvoicePlaceHolder" runat="server" Visible="true">
            <div class="Margin">
                <div class="LeftAlignColumn" style="width: 150px">
                    Ordernummer:
                    <asp:Literal ID="OrderIDLiteral" runat="server" /></div>
                <div class="LeftAlignColumn" style="width: 150px">
                    Datum:
                    <asp:Literal ID="DateLiteral" runat="server" /></div>
                <div style="clear: left;">
                </div>
            </div>
            <div class="SolidDivPrint Margin Padding">
                <h2>
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
            <div class="SolidDivPrint Margin Padding">
                <table class="Listing" style="width: 100%;">
                    <tr>
                        <th style="width: 180px;">
                            <span style="font-weight: bold;">Produktnamn</span>
                        </th>
                        <th style="width: 80px;">
                            <span style="font-weight: bold;">Antal</span>
                        </th>
                        <th style="width: 80px;">
                            <span style="font-weight: bold;">Storlek</span>
                        </th>
                        <th style="width: 80px;">
                            <span style="font-weight: bold;">Färg</span>
                        </th>
                        <th>
                            <span style="font-weight: bold;">Pris</span>
                        </th>
                        <th>
                            <span style="font-weight: bold;">Moms (%)</span>
                        </th>
                        <th class="RightAlignTH">
                            <span style="font-weight: bold;">Summa</span>
                        </th>
                    </tr>
                    <asp:Repeater ID="InvoiceRepeater" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td colspan="8">
                                    <div class="DottedLine">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%# Eval("ProductTitle") %>
                                </td>
                                <td>
                                    <%# Eval("Quantity") %>
                                </td>
                                <td>
                                        <%# Eval("Size") %>
                                    </td>
                                    <td>
                                        <%# Eval("Color") %>
                                    </td>
                                <td>
                                    <%# WebShop.BLL.UtilHelper.RoundPrice(Eval("Price").ToString())%>
                                    SEK
                                </td>
                                <td>
                                    <%# Eval("VAT") %>
                                    %
                                </td>
                                <td class="RightAlignTD">
                                    <%# WebShop.BLL.UtilHelper.RoundPrice(Eval("Sum").ToString())%>
                                    SEK
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="SolidDivPrint Margin Padding">
                <table class="Listing" style="width: 100%;">
                    <tr>
                        <td class="RightAlignTD" style="width: 400px;">
                            Summa:
                        </td>
                        <td class="RightAlignTD">
                            <asp:Label ID="SumInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="SumInvoiceCurrencyLabel"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="RightAlignTD">
                            Leveranstyp: <b><asp:Label ID="DeliveryTypeLabel" runat="server" /></b>
                        </td>
                        <td class="RightAlignTD">
                            <asp:Label ID="DeliveryCostInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="DeliveryCurrencyInvoiceLabel"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="RightAlignTD">
                            Faktureringtyp: <b><asp:Label ID="PaymentTypeLabel" runat="server" /></b>
                        </td>
                        <td class="RightAlignTD">
                            <asp:Label ID="PaymentCostInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="PaymentCurrencyInvoiceLabel"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="RightAlignTD">
                            <span style="font-weight: bold; color: Black;">TOTAL:</span>
                        </td>
                        <td class="RightAlignTD">
                            <span style="font-size: large; font-weight: bold; color: Black;">
                                <asp:Label ID="TotalPriceInvoiceLabel" runat="server" />&nbsp;<asp:Label ID="TotalCurrencyInvoiceLabel"
                                    runat="server" />
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="RightAlignTD">
                            Varav moms:
                        </td>
                        <td class="RightAlignTD">
                            <asp:Literal ID="VATInvoiceLiteral" runat="server" />&nbsp;<asp:Label ID="VATCurrencyInvoiceLabel"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="Margin ">
                <a href="http://www.webshop.se">www.webshop.se</a>
            </div>
        </asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
