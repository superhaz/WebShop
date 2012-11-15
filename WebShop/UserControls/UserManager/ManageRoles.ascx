<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRoles.ascx.cs" Inherits="WebShop.UserControls.UserManager.ManageRoles" %>
<h3 style="padding-bottom: 10px;">Manage Roles</h3>
    <p>
        <b>Create a New Role: </b>
        <asp:TextBox ID="RoleName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RoleNameReqField" runat="server" 
            ControlToValidate="RoleName" Display="Dynamic" 
            ErrorMessage="You must enter a role name." ValidationGroup="RoleVG"></asp:RequiredFieldValidator>
        
        
        <asp:Button ID="CreateRoleButton" runat="server" Text="Create Role" 
            onclick="CreateRoleButton_Click" ValidationGroup="RoleVG" />
    </p>
    <p>
        <asp:GridView ID="RoleList" runat="server" AutoGenerateColumns="False" 
            onrowdeleting="RoleList_RowDeleting" Width="200">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" OnClientClick="javascript:return confirm('This will deleted role. Continue?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="RoleNameLabel" Text='<%# Container.DataItem %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
