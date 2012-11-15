using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.BLL.Security;

namespace WebShop.BLL.Interfaces.Security
{
    interface IQueryStringHelper
    {
        string Document
        {
            get;
        }

        QueryStringHelper FromCurrent();
        

        QueryStringHelper FromUrl(string url);

        void ClearAllExcept(string except);

        void ClearAllExcept(string[] except);

        string ToString(bool includeUrl);
    }
}