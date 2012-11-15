<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersAndRoles.ascx.cs" Inherits="WebShop.UserControls.UserManager.UsersAndRoles" %>
    
        
    
    <h3 style="padding-bottom: 10px;">Manage Roles By User</h3>
    <p>
        <b>Select a User:</b>
        <asp:DropDownList ID="UserList" runat="server" AutoPostBack="True" 
            DataTextField="UserName" DataValueField="UserName" 
            onselectedindexchanged="UserList_SelectedIndexChanged">
        </asp:DropDownList>
    </p>
    <p>
        <asp:Repeater ID="UsersRoleList" runat="server">
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="RoleCheckBox" AutoPostBack="true" Text='<%# Container.DataItem %>' OnCheckedChanged="RoleCheckBox_CheckChanged" />
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </p>
    <p>
        <asp:Label ID="ActionStatus" runat="server" CssClass="Important"></asp:Label>
    </p>
    
    <div class="Line" style="margin-top: 20px; padding-bottom: 10px;">
                            </div>
    
    <h3 style="padding-bottom: 10px;">Manage Users By Role</h3>
    <p>
        <b>Select a Role:</b>
        <asp:DropDownList ID="RoleList" runat="server" AutoPostBack="true" 
            onselectedindexchanged="RoleList_SelectedIndexChanged">
        </asp:DropDownList>
    </p>
    <p>
        <asp:GridView ID="RolesUserList" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No users belong to this role." 
            onrowdeleting="RolesUserList_RowDeleting" Width="150">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Remove" OnClientClick="javascript:return confirm('This will remove user from role. Continue?')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Users">
                    <ItemTemplate>
                        <asp:Label runat="server" id="UserNameLabel" Text='<%# Container.DataItem %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <b>Add user (UserName):</b>
        <asp:TextBox ID="UserNameToAddToRole" runat="server"></asp:TextBox>
        
        <asp:Button ID="AddUserToRoleButton" runat="server" Text="Add User to Role" 
            onclick="AddUserToRoleButton_Click" />
    </p>