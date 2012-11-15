using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace WebShop.UserControls.UserManager
{
    public partial class CreateUserWithRole : System.Web.UI.UserControl
    {
        public event EventHandler UserCreated;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Reference the SpecifyRolesStep WizardStep
                WizardStep SpecifyRolesStep = RegisterUserWithRoles.FindControl("SpecifyRolesStep") as WizardStep;

                // Reference the RoleList CheckBoxList
                CheckBoxList RoleCheckBoxList = SpecifyRolesStep.FindControl("RoleCheckBoxList") as CheckBoxList;

                // Bind the set of roles to RoleList
                RoleCheckBoxList.DataSource = Roles.GetAllRoles();
                RoleCheckBoxList.DataBind();
            }
        }

        protected void RegisterUserWithRoles_ActiveStepChanged(object sender, EventArgs e)
        {
            // Have we JUST reached the Complete step?
            if (RegisterUserWithRoles.ActiveStep.Title == "Complete")
            {
                // Reference the SpecifyRolesStep WizardStep
                WizardStep SpecifyRolesStep = RegisterUserWithRoles.FindControl("SpecifyRolesStep") as WizardStep;

                // Reference the RoleList CheckBoxList
                CheckBoxList RoleCheckBoxList = SpecifyRolesStep.FindControl("RoleCheckBoxList") as CheckBoxList;

                // Add the checked roles to the just-added user
                foreach (ListItem item in RoleCheckBoxList.Items)
                {
                    if (item.Selected)
                        Roles.AddUserToRole(RegisterUserWithRoles.UserName, item.Text);
                }

                // see if an listener is set, if so raise event
                if (UserCreated != null)
                    UserCreated(this, new EventArgs());
            }
        }
    }
}