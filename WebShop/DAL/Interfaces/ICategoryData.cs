using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebShop.DAL.Interfaces
{
    public interface ICategoryData
    {
        #region CRUD - select, save, delete
        DataTable GetCategories(string languageID);
        void DeleteCategory(string categoryID);
        DataRow GetCategoryByCategoryID(string categoryID, string languageID);
        DataTable GetCategoryNameFiles(string languageID);
        DataRow GetCategoryInfo(string categoryID, string languageID);
        void SaveCategory(string categoryID, string VAT, string categoryName, string shortInfo, string languageID);
        void SaveCategoryName(string categoryID, string categoryName, string shortInfo, string languageID);
        #endregion

        #region Priority Management

        string GetANewHighestPriority();
        void SetPriorityByCategoryID(string categoryID, string priority);
        void DecreasePriority(string categoryID);
        void IncreasePriority(string categoryID);

        #endregion


        

    }
}