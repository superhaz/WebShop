<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.ascx.cs" Inherits="WebShop.UserControls.UserManager.ManageUsers" %>
<h3 style="padding-bottom: 10px;">
        Manage Users</h3>
    <p>
        <asp:Repeater ID="FilteringUI" runat="server" 
            onitemcommand="FilteringUI_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lnkFilter" 
                                Text='<%# Container.DataItem %>' 
                                CommandName='<%# Container.DataItem %>'></asp:LinkButton>
            </ItemTemplate>
            
            <SeparatorTemplate>|</SeparatorTemplate>
        </asp:Repeater>
    </p>
    <p>
        <asp:GridView ID="UserAccounts" runat="server"
            AutoGenerateColumns="False" onrowdeleting="UserAccounts_RowDeleting" DataKeyNames="UserName" CssClass="Padding" Width="600" >
            <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" OnClientClick="javascript:return confirm('This will delete user. Continue?')"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="UserName" 
                    DataNavigateUrlFormatString="~/Admin/UserInformation.aspx?user={0}" Text="Manage" />
                <asp:BoundField DataField="UserName" HeaderText="UserName" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:CheckBoxField DataField="IsApproved" HeaderText="Approved?" />
                <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Locked Out?" />
                <asp:CheckBoxField DataField="IsOnline" HeaderText="Online?" />
                <asp:BoundField DataField="Comment" HeaderText="Comment" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:LinkButton ID="lnkFirst" runat="server" onclick="lnkFirst_Click">&lt;&lt; First</asp:LinkButton> |
        <asp:LinkButton ID="lnkPrev" runat="server" onclick="lnkPrev_Click">&lt; Prev</asp:LinkButton> |
        <asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click">Next &gt;</asp:LinkButton> |
        <asp:LinkButton ID="lnkLast" runat="server" onclick="lnkLast_Click">Last &gt;&gt;</asp:LinkButton>
    </p>