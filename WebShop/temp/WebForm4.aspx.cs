using System;
using System.Collections.Generic;
using System.IO;
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
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(FileUpload1.FileName);
            filename = Server.MapPath(filename);
            FileUpload1.SaveAs(filename);
        }

        
    }
}