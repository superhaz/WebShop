using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebShop.DAL;
using WebShop.BLL;
using WebShop.DAL.Interfaces;
using Ninject.Web;
using Ninject;
using WebShop.BLL.Interfaces;

namespace WebShop
{
    public partial class Conditions1 : PageBase
    {
        [Inject]
        public ITextBlockData TextBlockDataComponent { get; set; }
        [Inject]
        public IUtilHelper UtilHelperComponent { get; set; }

        private string languageID = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadControls();

            }
        }

        private void LoadControls()
        {
            BindTextBlock();
        }

        private void BindTextBlock()
        {
            //  text
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.Conditions.ToString(), languageID);
            if (table != null && table.Rows.Count > 0)
            {
                TitleLabel.Text = table.Rows[0]["Title"].ToString();
                TextBlockLabel.Text = UtilHelperComponent.ParseNewLine(table.Rows[0]["TextBlock"].ToString());
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            TextBlockDataComponent.SaveTextBlock(TitleTextBox.Text, TextBlockTextBox.Text, TextBlockIDHiddenField.Value, languageID);
            BindTextBlock();
            SwitchMode();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            SwitchMode();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            DataTable table = TextBlockDataComponent.GetTextBlock(TextBlockData.TextBlockNames.Conditions.ToString(), languageID);
            if (table != null && table.Rows.Count > 0)
            {
                TextBlockIDHiddenField.Value = table.Rows[0]["TextBlockID"].ToString();
                TitleTextBox.Text = table.Rows[0]["Title"].ToString();
                TextBlockTextBox.Text = table.Rows[0]["TextBlock"].ToString();
                SwitchMode();
            }
        }

        private void SwitchMode()
        {
            if (DefaultPlaceHolder.Visible)
            {
                DefaultPlaceHolder.Visible = false;
                EditPlaceHolder.Visible = true;
            }
            else
            {
                DefaultPlaceHolder.Visible = true;
                EditPlaceHolder.Visible = false;
            }
        }
    }
}