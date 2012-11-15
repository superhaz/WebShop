using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.DAL;
using WebShop.BLL;
using System.Data;
using WebShop.UserControls.FileImageManager.DAL.Factory;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;
using System.Web.Configuration;
using Ninject.Web;
using Ninject;
using WebShop.DAL.Interfaces;
using WebShop.BLL.Interfaces;

namespace WebShop.Admin
{
    public partial class Category : PageBase
    {
        [Inject]
        public ICategoryData CategoryDataComponent { get; set; }
        [Inject]
        public IProductData ProductDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        private string selectedLangID = "1";

        // Provider members
        private IItemDataProvider provider;
        public IItemDataProvider Provider
        {
            get
            {
                if (provider == null)
                    provider = ItemDataProviderFactory.CreateProvider(ItemDataProviderFactory.ItemDataProviderType.CategoryProvider);

                return provider;
            }
        }
        // end of Provider members

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
            else
            {
                ResetMode();
            } 
        }

        protected override void OnInit(EventArgs e)
        {
            Title += WebConfigurationManager.AppSettings["CompanyName"];

            base.OnInit(e);
        }

        private void ResetMode()
        {
            // reset default mode after closing a ViewPointErrand
            if (MessagePlaceHolder.Visible)
            {
                MessagePlaceHolder.Visible = false;
            }
        }

        private void BindRepeater()
        {
            ItemRepeater.DataSource = CategoryDataComponent.GetCategories(selectedLangID);
            ItemRepeater.DataBind();
        }

        protected void ItemRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            // check which linkbutton was presseed
            if (e.CommandName.ToString() == "Delete")
            {
                Delete(e);
            }
            else if (e.CommandName.ToString() == "Edit")
            {
                string categoryID = e.CommandArgument.ToString();
                LoadControls(categoryID);
                SwitchMode();

            }
            else if (e.CommandName.ToString() == "IncreasePriority" && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                // Increase priority
                string categoryID = e.CommandArgument.ToString();
                CategoryDataComponent.IncreasePriority(categoryID);
            }
            else if (e.CommandName.ToString() == "DecreasePriority" && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                // Decrease priority
                string categoryID = e.CommandArgument.ToString();
                CategoryDataComponent.DecreasePriority(categoryID);
            }
            else if (e.CommandName.ToString() == "HighestPriority" && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                // set highest prio for the current product
                string categoryID = e.CommandArgument.ToString();
                SetHighestPrio(categoryID);
            }
            else if (e.CommandName.ToString() == "LowestPriority" && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                // set lowest prio for the current product
                string categoryID = e.CommandArgument.ToString();
                SetLowestPrio(categoryID);
            }

            BindRepeater();
            // update the master pages category repeater
            Master.LoadCategories();
        }

        private void LoadControls(string categoryID)
        {
            DataRow row = CategoryDataComponent.GetCategoryByCategoryID(categoryID, selectedLangID);

            if (row != null)
            {
                CategoryIDHiddenField.Value = row["CategoryID"].ToString();
                ImageManager1.BindControls(row["CategoryNameID"].ToString());
                CategoryNameTextBox.Text = row["CategoryName"].ToString();
                ShortInfoTextBox.Text = row["ShortInfo"].ToString();
                VATTextBox.Text = row["VAT"].ToString();
                
            }
        }

        private void SwitchMode()
        {
            if (ItemPlaceHolder.Visible)
            {
                ItemPlaceHolder.Visible = false;
                RepeaterPlaceHolder.Visible = true;
            }
            else
            {
                ItemPlaceHolder.Visible = true;
                RepeaterPlaceHolder.Visible = false;
            }
        }

        private void Delete(RepeaterCommandEventArgs e)
        {
            string categoryID = e.CommandArgument.ToString();
            string numProductsInCategory = (e.Item.FindControl("NumProductsInCategoryLiteral") as Literal).Text;

            if (UtilHelperComponent.IsInteger(numProductsInCategory))
            {
                // only delete empty categories
                if (Convert.ToInt32(numProductsInCategory) > 0)
                {
                    // show message unable to delete category
                    MessagePlaceHolder.Visible = true;
                }
                else
                {
                    CategoryDataComponent.DeleteCategory(categoryID);
                    BindRepeater();
                }
            }
            else
            {
                // show message unable to delete category
                MessagePlaceHolder.Visible = true;
            }
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal priceLiteral = e.Item.FindControl("NumProductsInCategoryLiteral") as Literal;
        }

        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            SwitchMode();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            CategoryDataComponent.SaveCategory(CategoryIDHiddenField.Value, VATTextBox.Text, CategoryNameTextBox.Text, ShortInfoTextBox.Text, selectedLangID);
            SwitchMode();
            BindRepeater();
            ResetControls();
            // update the master pages category repeater
            Master.LoadCategories();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetControls();
            SwitchMode();
        }

        private void ResetControls()
        {
            CategoryIDHiddenField.Value = "";
            CategoryNameTextBox.Text = "";
            ShortInfoTextBox.Text = "";
        }

        #region Priority Management

        private void SetHighestPrio(string categoryID)
        {
            // change current product prio to lowest
            string highestPrio = CategoryDataComponent.GetANewHighestPriority();
            // set the current product to lowest prio
            CategoryDataComponent.SetPriorityByCategoryID(categoryID, highestPrio);
            // create a bind to repeater
            BindRepeater();
            // make a resort since current product has been moved from its priority
            ResortPriority();
            // finally update the repeater to get the final resort
            BindRepeater();
        }

        private void SetLowestPrio(string categoryID)
        {
            // increase all products in the category with one priority step
            ResortPriority(1);
            // set the product to lowest prio
            CategoryDataComponent.SetPriorityByCategoryID(categoryID, "1");
            // create a bind to repeater
            BindRepeater();
            // make a resort once again since current product has been moved from its priority
            ResortPriority();
            // finally update the repeater to get the final resort
            BindRepeater();
        }

        private void ResortPriority()
        {
            ResortPriority(0);  // make a resort without any priority step
        }

        /// <summary>
        /// sort the priority in order
        /// </summary>
        /// <param name="priority">the starting priority</param>
        private void ResortPriority(int priorityStep)
        {
            int priority = ItemRepeater.Items.Count + priorityStep;
            foreach (RepeaterItem item in ItemRepeater.Items)
            {
                if (item != null)
                {

                    string categoryID = (item.FindControl("CategoryIDHiddenField") as HiddenField).Value;
                    ProductDataComponent.SetPriorityByProductCategoryID(categoryID, priority.ToString());

                    priority--;
                }
            }
        }

        #endregion
    }
}
