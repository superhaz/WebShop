using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.DAL.Interfaces
{
    public interface ISettingsData
    {
        string GetSettingsValue(string settingsName);

        string GetAccountNumber();
    }
}