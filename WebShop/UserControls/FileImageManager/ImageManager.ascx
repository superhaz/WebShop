<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageManager.ascx.cs"
    Inherits="WebShop.UserControls.FileImageManager.ImageManager" %>
<asp:PlaceHolder ID="SwitchToImagesListingPlaceHolder" runat="server">
    <asp:LoginView ID="LoginView2" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="SuperAdmins, Administrators">
                <ContentTemplate>
                    <div class="RightAlignColumn Padding" style="width: 97%">
                        <asp:ImageButton ID="EditImageButton" runat="server" ToolTip="Redigera" ImageUrl="~/UserControls/FileImageManager/Images/edit-32.png"
                            OnClick="EditButton_Click" ImageAlign="AbsMiddle" /><br />
                        <asp:LinkButton ID="EditLinkButton" runat="server" Text="Redigera" OnClick="EditButton_Click" />
                    </div>
                    <div style="clear: left">
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:PlaceHolder>
<asp:UpdatePanel ID="ImageViewUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:PlaceHolder ID="ImageViewingPlaceHolder" runat="server">
            <div class="MiddleDiv SolidDiv Margin" style="text-align: center; width: 99%;">
                <div class="MiddleImage">
                    <asp:ImageButton ID="ItemImageButton" runat="server" OnClick="ForwardImageButton_Click" />
                    <asp:HiddenField ID="PriorityHiddenField" runat="server" />
                </div>
            </div>
            <div class="SolidDiv Padding Margin">
                <div class="LeftAlignColumn" style="width: 32%">
                    <asp:ImageButton ID="BackImageButton" runat="server" ImageUrl="~/UserControls/FileImageManager/images/Back-32.png"
                        OnClick="BackImageButton_Click" />
                </div>
                <div class="CenterAlignColumn" style="width: 35%">
                    Klicka på bilden eller pilarna för att se fler bilder.
                </div>
                <div class="RightAlignColumn" style="width: 32%">
                    <asp:ImageButton ID="ForwardImageButton" runat="server" ImageUrl="~/UserControls/FileImageManager/images/Forward-32.png"
                        OnClick="ForwardImageButton_Click" />
                </div>
                <div style="clear: left;">
                </div>
            </div>
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:PlaceHolder ID="AddNewImagePlaceHolder" runat="server" Visible="false">
    <div class="Margin Padding" style="border: dotted 1px #CCC;">
        <div id="FileUpload" style="border: solid 0px black;">
            <div style="border: solid 0px black;">
                Välj fil att ladda upp:<span style="color: Red;">*</span>
            </div>
            <div class="LeftAlignColumn" style="width: 520px; border: solid 0px black;">
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:CustomValidator ID="FileExtension_CustomValidator"  runat="server" ErrorMessage="<br/>* Det gick inte att ladda upp filen. Bildfilen måste vara av typen *.jpg, *.png eller *.gif" 
                      OnServerValidate="FileExtension_CustomValidator_ServerValidate" Display="Dynamic" />
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="SaveImageButton" runat="server" OnClick="SaveButton_Click" ToolTip="Spara"
                    ImageUrl="~/UserControls/FileImageManager/images/filesave-32.png" ImageAlign="AbsMiddle" /><br />
                <asp:LinkButton ID="SaveLinkButton" runat="server" Text="Spara" OnClick="SaveButton_Click" />
                
            </div>
            <div class="RightAlignColumn Padding" style="border: solid 0px black;">
                <asp:ImageButton ID="CancelImageButton" runat="server" OnClick="CancelButton_Click"
                    CausesValidation="false" ToolTip="Avbryt" ImageUrl="~/UserControls/FileImageManager/images/Cancel-32.png"
                    ImageAlign="AbsMiddle" /><br />
                <asp:LinkButton ID="CancelLinkButton" runat="server" Text="Avbryt" OnClick="CancelButton_Click"
                    CausesValidation="false" />
            </div>
            <div style="clear: left;">
            </div>
            <div style="padding-left: 5px; padding-top: 8px; border: solid 0px black;">
                <asp:RequiredFieldValidator ID="FileUpload1_RequiredFieldValidator" runat="server"
                    Display="Dynamic" ControlToValidate="FileUpload1" ErrorMessage="*Välj en fil att ladda upp"
                    Text="" />
            </div>
        </div>
    </div>
</asp:PlaceHolder>
<asp:UpdatePanel ID="ImagesListingUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:PlaceHolder ID="ImagesListingPlaceHolder" runat="server" >
        <div class="Padding LeftAlignColumn" style="border: solid 0px black; width: 50%;">
            <asp:PlaceHolder ID="SwitchToImageViewingPlaceHolder" runat="server">
                <asp:ImageButton ID="ReturnImageButton" runat="server" ToolTip="Återgå" ImageUrl="~/UserControls/FileImageManager/Images/return_32.png"
                    OnClick="ReturnButton_Click" />
                <br />
                <asp:LinkButton ID="ReturnLinkButton" runat="server" OnClick="ReturnButton_Click"
                    Text="Återgå" />
            </asp:PlaceHolder>
        </div>
        <div class="RightAlignColumn Padding" style="width: 43%; border: solid 0px black;">
            <asp:PlaceHolder ID="SwitchToAddNewImagePlaceHolder" runat="server">
                <asp:ImageButton ID="AddNewImageButton" CssClass="icontext" runat="server" OnClick="AddNewImageButton_Click"
                    ToolTip="Ny bild" ImageUrl="~/UserControls/FileImageManager/images/Add_Photo-32.png" />
                <br />
                <asp:LinkButton ID="AddNewImageLinkButton" runat="server" OnClick="AddNewImageButton_Click"
                    Text="Ny bild" />
            </asp:PlaceHolder>
        </div>
        <div style="clear: both;">
        </div>
        <asp:PlaceHolder ID="ImagesListingRepeaterPlaceHolder" runat="server">
            <div>
                <asp:Repeater ID="ItemRepeater" runat="server" OnItemCommand="ItemRepeater_ItemCommand"
                    OnItemDataBound="ItemRepeater_ItemDataBound">
                    <ItemTemplate>
                        <div class="ProductInList Margin Padding">
                            <asp:PlaceHolder ID="SortingPlaceHolder" runat="server" >
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
                            </asp:PlaceHolder>
                            <div class="ProductInListImage" style="min-height: 150px;">
                                <asp:Image ID="ItemImage" runat="server" Width="80px" />
                                <asp:HiddenField ID="ItemFileIDHiddenField" runat="server" Value='<%# Eval("ItemFileID") %>' />
                                <asp:HiddenField ID="FileIDHiddenField" runat="server" Value='<%# Eval("FileID") %>' />
                                <asp:HiddenField ID="FileNameHiddenField" runat="server" Value='<%# Eval("FileName") %>' />
                                <asp:HiddenField ID="FileSizeHiddenField" runat="server" Value='<%# Eval("FileSize") %>' />
                                <asp:HiddenField ID="ItemIDHiddenField" runat="server" Value='<%# Eval("ItemID") %>' />
                                <asp:HiddenField ID="PriorityHiddenField" runat="server" Value='<%# Eval("Priority") %>' />
                            </div>
                            <div class="ControlDiv">
                                <div style="border: solid 0px yellow; text-align: right;">
                                    <asp:ImageButton ID="DeleteImageButton" runat="server" ToolTip="Ta bort" CommandName="Delete"
                                        CssClass="icontext" ImageUrl="~/UserControls/FileImageManager/images/Delete-16.png"
                                        CommandArgument='<%# Eval("FileID") %>' OnClientClick="javascript:return confirm('Bilden kommer att tas bort. Vill du fortsätta?')" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div style="clear: left;">
                </div>
            </div>
        </asp:PlaceHolder>
        </asp:PlaceHolder>
    </ContentTemplate>
    <Triggers>
        <%--make a full post back--%>
        <asp:PostBackTrigger ControlID="AddNewImageButton" />
        <asp:PostBackTrigger ControlID="AddNewImageLinkButton" />
        <asp:PostBackTrigger ControlID="ReturnImageButton" />
        <asp:PostBackTrigger ControlID="ReturnLinkButton" />
    </Triggers>
</asp:UpdatePanel>
