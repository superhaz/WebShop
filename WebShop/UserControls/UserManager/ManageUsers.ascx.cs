using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Permissions;

namespace WebShop.UserControls.UserManager
{
    public partial class ManageUsers : System.Web.UI.UserControl
    {
        public event EventHandler UserDeleted;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUserAccounts();

                BindFilteringUI();
            }
        }

        private void BindFilteringUI()
        {
            string[] filterOptions = { "All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            FilteringUI.DataSource = filterOptions;
            FilteringUI.DataBind();
        }

        public void BindUserAccounts()
        {
            int totalRecords;
            UserAccounts.DataSource = Membership.FindUsersByName(this.UsernameToMatch + "%", this.PageIndex, this.PageSize, out totalRecords);
            UserAccounts.DataBind();

            // Enable/disable the paging interface
            bool visitingFirstPage = (this.PageIndex == 0);
            lnkFirst.Enabled = !visitingFirstPage;
            lnkPrev.Enabled = !visitingFirstPage;

            int lastPageIndex = (totalRecords - 1) / this.PageSize;
            bool visitingLastPage = (this.PageIndex >= lastPageIndex);
            lnkNext.Enabled = !visitingLastPage;
            lnkLast.Enabled = !visitingLastPage;
        }

        protected void FilteringUI_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "All")
                this.UsernameToMatch = string.Empty;
            else
                this.UsernameToMatch = e.CommandName;

            BindUserAccounts();
        }

        #region Paging Interface Click Event Handlers
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 0;
            BindUserAccounts();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex -= 1;
            BindUserAccounts();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            this.PageIndex += 1;
            BindUserAccounts();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            // Determine the total number of records
            int totalRecords;
            Membership.FindUsersByName(this.UsernameToMatch + "%", this.PageIndex, this.PageSize, out totalRecords);

            // Navigate to the last page index
            this.PageIndex = (totalRecords - 1) / this.PageSize;
            BindUserAccounts();
        }
        #endregion

        #region Properties
        private string UsernameToMatch
        {
            get
            {
                object o = ViewState["UsernameToMatch"];
                if (o == null)
                    return string.Empty;
                else
                    return (string)o;
            }
            set
            {
                ViewState["UsernameToMatch"] = value;
            }
        }

        private int PageIndex
        {
            get
            {
                object o = ViewState["PageIndex"];
                if (o == null)
                    return 0;
                else
                    return (int)o;
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        private int PageSize
        {
            get
            {
                return 10;
            }
        }
        #endregion

        #region RowDeleting Event Handler
        [PrincipalPermission(SecurityAction.Demand, Role = "SuperAdmins")]
        protected void UserAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Determine the username of the user we are editing
            string UserName = UserAccounts.DataKeys[e.RowIndex].Value.ToString();

            // Delete the user
            Membership.DeleteUser(UserName);

            // Revert the grid's EditIndex to -1 and rebind the data
            UserAccounts.EditIndex = -1;
            BindUserAccounts();

            // see if an listener is set, if so raise event
            if (UserDeleted != null)
                UserDeleted(this, new EventArgs());
        }
        #endregion
    }
}