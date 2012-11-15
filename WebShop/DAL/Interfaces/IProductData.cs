using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebShop.DAL.Interfaces
{
    public interface IProductData
    {
        #region CRUD - select, save, delete

        DataTable GetProductByProductID(string productID, string languageID);
        DataTable GetLatestProducts(string languageID, int numberOfProducts);
        DataTable GetProductsByCategoryID(string categoryID, string languageID, bool onlyPublishedProducts);
        string SaveNewProduct();
        void SaveNewProduct(string productID, string categoryID, string availabilityID, List<string> colorIDs, List<string> sizeIDs, string artNo, string price, bool isPublished, string title, string shortInfo, string fullInfo, string additionalInfo);
        void SaveProductColors(string productID, List<string> colorIDs);        
        void SaveProductSizes(string productID, List<string> sizeIDs);
        void SaveProduct(string productID, string languageID, string availabilityID, List<string> colorIDs, List<string> sizeIDs, string artNo, string price, bool isPublished, string title, string shortInfo, string fullInfo, string additionalInfo);
        void DeleteProduct(string productID);        
        void CleanUpProducts();
        DataTable GetAvailabilities(string selectedLangID);
        bool IsItANewProduct(string productID);

        #endregion

        #region Order Management

        #endregion

        #region Priority Management ProductCategory
       
        string GetANewHighestPriority(string categoryID);
        string GetANewLowestPriority(string categoryID);
        void SetPriorityByProductCategoryID(string productCategoryID, string priority);
        void DecreasePriority(string productCategoryID, string categoryID);
        void IncreasePriority(string productCategoryID, string categoryID);

        #endregion

        #region Product Colors

        DataTable GetColors(string languageID);
        DataTable GetColorsByProductID(string productID, string languageID);
        List<string> GetColorIDsByProductID(string productID, string languageID);

        #endregion

        #region Product Sizes

        DataTable GetSizes(string sizeTypeID);
        DataTable GetSizesByProductID(string productID);
        List<string> GetSizeIDsByProductID(string productID);

        #endregion

        #region Language Management

        DataTable GetLanguages();

        #endregion
    }
}