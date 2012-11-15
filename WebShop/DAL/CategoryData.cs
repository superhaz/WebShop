using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebShop.DAL.Interfaces;

namespace WebShop.DAL
{
    public class CategoryData :ICategoryData
    {
        [Ninject.Inject]
        public IProductData ProductDataComponent { get; set; }

        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region CRUD - select, save, delete

        public DataTable GetCategories(string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Category.CategoryID, Category.VAT, CategoryName.CategoryNameID, CategoryName.LanguageID, 
                                    CategoryName.CategoryName, CategoryName.ShortInfo, Category.Priority,
                                        (select COUNT(ProductID)  FROM ProductCategory WHERE CategoryID=Category.CategoryID) AS NumProductsInCategory
                                    FROM Category INNER JOIN CategoryName ON Category.CategoryID = CategoryName.CategoryID
                                    WHERE CategoryName.LanguageID = @LanguageID
                                    ORDER BY Category.Priority ASC";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }


        public DataTable GetCategoryNameFiles(string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT CategoryName.CategoryID, CategoryName.CategoryName, CategoryName.ShortInfo, 
                                    [File].FileName, CategoryName.LanguageID FROM CategoryNameFile 
                                    INNER JOIN [File] ON CategoryNameFile.FileID = [File].FileID 
                                    INNER JOIN CategoryName ON CategoryNameFile.CategoryNameID = CategoryName.CategoryNameID
                                    WHERE (CategoryName.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }

        }

        public DataRow GetCategoryInfo(string categoryID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT CategoryName.CategoryName, CategoryName.ShortInfo 
                                    FROM CategoryName
                                    WHERE (CategoryName.CategoryID = @CategoryID AND CategoryName.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                if (table.Rows.Count == 1)
                    return table.Rows[0];
                else
                    return null;
            }
        }

        public void DeleteCategory(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM Category WHERE CategoryID = @CategoryID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataRow GetCategoryByCategoryID(string categoryID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Category.CategoryID, Category.VAT, CategoryName.CategoryNameID, CategoryName.LanguageID, 
                                    CategoryName.CategoryName, CategoryName.ShortInfo, Category.Priority
                                    FROM Category INNER JOIN CategoryName ON Category.CategoryID = CategoryName.CategoryID
                                    WHERE Category.CategoryID=@CategoryID AND CategoryName.LanguageID = @LanguageID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count > 0)
                    return table.Rows[0];
                else
                    return null;
            }
        }

        public void SaveCategory(string categoryID, string VAT, string categoryName, string shortInfo, string languageID)
        {
            if (string.IsNullOrEmpty(categoryID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"INSERT INTO Category (VAT, Priority) VALUES (@VAT, @Priority)
                                        SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@VAT", VAT);
                    cmd.Parameters.AddWithValue("@Priority", GetANewHighestPriority());

                    conn.Open();
                    categoryID = cmd.ExecuteScalar().ToString(); ;
                }

                SaveCategoryName(categoryID, categoryName, shortInfo, languageID);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"UPDATE CategoryName SET CategoryName=@CategoryName, ShortInfo=@ShortInfo WHERE CategoryID=@CategoryID AND LanguageID = @LanguageID
                                        UPDATE Category SET VAT=@VAT WHERE CategoryID=@CategoryID";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    cmd.Parameters.AddWithValue("@ShortInfo", shortInfo);
                    cmd.Parameters.AddWithValue("@LanguageID", languageID);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@VAT", VAT);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveCategoryName(string categoryID, string categoryName, string shortInfo, string languageID)
        {
            // Add category name in multiple language
            DataTable table = ProductDataComponent.GetLanguages();
            // add a row for every language 
            foreach (DataRow row in table.Rows)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"INSERT INTO CategoryName (CategoryID, LanguageID, CategoryName, ShortInfo) VALUES (@CategoryID, @LanguageID, @CategoryName, @ShortInfo)";

                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@LanguageID", row["LanguageID"].ToString());

                    if (languageID == row["LanguageID"].ToString())
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        cmd.Parameters.AddWithValue("@ShortInfo", shortInfo);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShortInfo", DBNull.Value);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Priority Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID">The ID of the item that this file belongs to.</param>
        /// <returns>The next available priority for an item.</returns>
        public string GetANewHighestPriority()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT MAX(Priority)+1 AS Priority FROM Category";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();

                // if null is the result then there aren't any categories yet, therefore the first image should have prio 1
                if (string.IsNullOrEmpty(result))
                    return "1";
                else
                    return result;
            }
        }

        public void SetPriorityByCategoryID(string categoryID, string priority)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"UPDATE [Category] SET Priority = @Priority WHERE CategoryID = @CategoryID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@Priority", priority);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DecreasePriority(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this category
                                    SELECT @CurrentPriority=Priority FROM Category WHERE CategoryID = @CategoryID
                                    -- check that it isnt already lowest prio
                                    IF(@CurrentPriority>1)
                                    BEGIN
                                        -- increase priority for category having next lower prio then current one
                                        UPDATE Category SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority-1)
                                        -- decrease priority for the current category
                                        UPDATE Category SET Priority = (@CurrentPriority-1) WHERE CategoryID = @CategoryID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public void IncreasePriority(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this category
                                    SELECT @CurrentPriority=Priority FROM Category WHERE CategoryID = @CategoryID
                                    -- check that it isnt already highest prio
                                    IF(@CurrentPriority<(select max(priority) from Category))
                                    BEGIN
	                                    -- decrease priority for category having next higher prio then current one
	                                    UPDATE Category SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority+1)
	                                    -- increase priority for the current productfile
	                                    UPDATE Category SET Priority = (@CurrentPriority+1) WHERE categoryID = @categoryID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
