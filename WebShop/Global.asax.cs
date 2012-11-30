using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Ninject;
using Ninject.Web;
using Ninject.Web.Common;
using WebShop.DAL.Interfaces;
using WebShop.DAL;
using WebShop.BLL.Security;
using WebShop.BLL;
using WebShop.BLL.Interfaces;


namespace WebShop
{
    public class Global : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<ICategoryData>().To<CategoryData>().InRequestScope();
            kernel.Bind<IOrderData>().To<OrderData>().InRequestScope();
            kernel.Bind<IProductData>().To<ProductData>().InRequestScope();
            kernel.Bind<ISettingsData>().To<SettingsData>().InRequestScope();
            kernel.Bind<ITextBlockData>().To<TextBlockData>().InRequestScope();
            kernel.Bind<IMailHelper>().To<MailHelper>().InRequestScope();
            kernel.Bind<IOrderHelper>().To<OrderHelper>().InRequestScope();
            kernel.Bind<IUtilHelper>().To<UtilHelper>().InRequestScope();
            return kernel;
        }

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}