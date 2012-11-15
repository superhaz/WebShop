using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace WebShop.Admin
{
    public partial class UserView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // add an eventhandler for the MessageSent event, and tell the 
            // handler to invoke the method SendEventUserControl1_MessageSent(string message)
            CreateUserWithRole1.UserCreated += new EventHandler(CreateUserWithRole1_UserCreated);
            ManageUsers1.UserDeleted += new EventHandler(ManageUsers1_UserDeleted);
        }

        void CreateUserWithRole1_UserCreated(object sender, EventArgs e)
        {
            UpdateTab1();
            UpdateTab2();
        }

        void ManageUsers1_UserDeleted(object sender, EventArgs e)
        {
            UpdateTab2();
        }
        private void UpdateTab1()
        {
            // tab 1
            ManageUsers1.BindUserAccounts();
        }

        private void UpdateTab2()
        {
            // tab 2
            UsersAndRoles1.BindUsersToUserList();
            UsersAndRoles1.DisplayUsersBelongingToRole();
            UsersAndRolesUpdatePanel.Update();
        }

        private void UpdateTab3()
        {
            
        }

        #region Properties

        protected enum Tab
        {
            About = 1,
            Config = 2,
            Licensing = 3,
        }

        #endregion

        #region Quick Binding

        private void InitTabButtons()
        {
            // hide some tab buttons
            LicensingTabPlaceHolder.Visible = false;
        }

        internal void Bind()
        {
            //Label1.Text = "Showing About tab";
            //Label2.Text = "Showing Configuration tab";

            //if (LicensingTabPlaceHolder.Visible)
            //{
            //    Label3.Text = "Showing License tab";
            //}
        }


        #endregion
    }
}
