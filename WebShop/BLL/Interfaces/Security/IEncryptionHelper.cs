using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.BLL.Security;

namespace WebShop.BLL.Interfaces.Security
{
    public interface IEncryptionHelper
    {
        string EncryptQueryString(string queryStringName, string queryStringValue);

        string DecryptQueryString(string queryStringName);
        QueryStringHelper EncryptQueryString(QueryStringHelper queryString);
        QueryStringHelper DecryptQueryString(QueryStringHelper queryString);
        string DeHex(string hexstring);
        string Hex(string sData);
    }
}