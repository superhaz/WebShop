<%@ Page Title="Kontakt - " Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Contact.aspx.cs" Inherits="WebShop.Contact" %>
    <%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <div class="SolidDiv Padding Margin">
    <asp:PlaceHolder ID="ContactFormPlaceHolder" runat="server" >
        <div class="LeftAlignColumn" style="border: solid 0px black; width: 480px">
            <div class="Padding">
            <h2>
                Kontakta</h2>
                För att kontakta oss fyll i formuläret nedan så försöker vi svara er antingen genom e-post eller telefon.
                </div>
            <div class="Padding">
                <h3>
                    E-post: *</h3>
                <shaz:Email ID="Email" runat="server" Width="300" ValidationGroup="ContactVG" ShowErrorMessageInline="true" />
            </div>
            <div class="Padding">
                <h3>
                    Tel nr:</h3>
                <shaz:PhoneNumber ID="PhoneNumber" runat="server" Enable_RequiredFieldValidator="false" ValidationGroup="ContactVG" ShowErrorMessageInline="true"  />
            </div>
            <div class="Padding">
                <h3>
                    Meddelande:</h3>
                <asp:TextBox ID="MessageTextBox" runat="server" TextMode="MultiLine" Width="400"
                    Height="100" /></div>
        </div>
        <div style="clear: left;">
        </div>
        <div class="Padding" style="border: solid 0px black;">
            <asp:ImageButton ID="SendImageButton" runat="server" OnClick="SendButton_Click" ToolTip="Skicka"
                ImageUrl="~/UserControls/FileImageManager/images/Send-Mail-32.png" ValidationGroup="ContactVG" /><br />
            <asp:LinkButton ID="SendLinkButton" runat="server" Text="Skicka" OnClick="SendButton_Click"
                ValidationGroup="ContactVG" />
        </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="MessagePlaceHolder" runat="server" Visible="false" >
            <div class="Padding">
                <h3>
                    Tack!</h3>
                Ert meddelande har skickats, vi kommer att besvara ert mail inom kort.</div>
        </asp:PlaceHolder>
         <asp:PlaceHolder ID="ErrorPlaceHolder" runat="server" Visible="false" >
            <div class="Padding">
                <h3>
                    Fel uppstod!</h3>
                Det gick tyvärr inte att skicka ert meddelande. Försök igen senare eller kontakta oss på order@nordicmuslim.se
                </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
