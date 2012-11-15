using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;
using System.Data;

namespace WebShop.temp
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductDataProvider pdp = new ProductDataProvider();
            DataTable table = pdp.GetFileByItemIDANDPriority("96", null);
        }
    }
}