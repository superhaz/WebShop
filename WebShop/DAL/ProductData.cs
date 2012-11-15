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
    public class ProductData : IProductData
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region CRUD - select, save, delete

        public DataTable GetProductByProductID(string productID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Product.ProductID, Product.ArtNo, Product.Price, Product.IsPublished, Brand.BrandName, 
                                    ProductTitle.Title, ShortInfo.ShortInfo, FullInfo.FullInfo, AdditionalInfo.AdditionalInfo, 
                                    Language.Language, ProductCategory.CategoryID, Product.AvailabilityID,
                                    (SELECT     AvailabilityName.AvailabilityName
                                    FROM         Product INNER JOIN
                                                          Availability ON Product.AvailabilityID = Availability.AvailabilityID INNER JOIN
                                                          AvailabilityName ON Availability.AvailabilityID = AvailabilityName.AvailabilityID
                                    WHERE     (AvailabilityName.LanguageID = @LanguageID) AND (ProductID = @ProductID)) AS AvailabilityName,
                                    (SELECT     Availability.Availability
                                    FROM         Product INNER JOIN
                                                          Availability ON Product.AvailabilityID = Availability.AvailabilityID
                                    WHERE    (ProductID = @ProductID)) AS Availability,
                                    (SELECT TOP 1 [File].FileName FROM  ProductFile INNER JOIN [File] ON ProductFile.FileID = [File].FileID
                                         WHERE ProductID = Product.ProductID ORDER BY ProductFile.Priority ASC) AS FileName,
                                    (SELECT     Category.VAT FROM Category INNER JOIN ProductCategory ON Category.CategoryID = ProductCategory.CategoryID
                                    WHERE ProductCategory.ProductID = Product.ProductID) AS VAT
                                    FROM FullInfo 
                                    RIGHT OUTER JOIN Brand 
                                    RIGHT OUTER JOIN AdditionalInfo 
                                    RIGHT OUTER JOIN Product 
                                    INNER JOIN ProductTitle ON Product.ProductID = ProductTitle.ProductID 
                                    INNER JOIN Language ON ProductTitle.LanguageID = Language.LanguageID 
                                    INNER JOIN ProductCategory ON Product.ProductID = ProductCategory.ProductID 
                                    ON AdditionalInfo.LanguageID = Language.LanguageID AND 
                                    AdditionalInfo.ProductID = Product.ProductID 
                                    ON Brand.BrandID = Product.BrandID 
                                    ON FullInfo.LanguageID = Language.LanguageID AND 
                                    FullInfo.ProductID = Product.ProductID 
                                    LEFT OUTER JOIN ShortInfo ON Product.ProductID = ShortInfo.ProductID AND 
                                    Language.LanguageID = ShortInfo.LanguageID
                                    WHERE (Language.LanguageID = @LanguageID) AND (Product.ProductID = @ProductID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataTable GetLatestProducts(string languageID, int numberOfProducts)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT TOP @NumOfProducts Product.ProductID, Product.ArtNo, Product.Price, Product.IsPublished, Brand.BrandName, 
                                    ProductTitle.Title, ShortInfo.ShortInfo, FullInfo.FullInfo, AdditionalInfo.AdditionalInfo, 
                                    Language.Language, ProductCategory.ProductCategoryID, ProductCategory.CategoryID, ProductCategory.Priority,
                                        (SELECT TOP 1 [File].FileName FROM  ProductFile INNER JOIN [File] ON ProductFile.FileID = [File].FileID
                                         WHERE ProductID = Product.ProductID ORDER BY ProductFile.Priority ASC) AS FileName
                                    FROM FullInfo 
                                    RIGHT OUTER JOIN Brand 
                                    RIGHT OUTER JOIN AdditionalInfo 
                                    RIGHT OUTER JOIN Product 
                                    INNER JOIN ProductTitle ON Product.ProductID = ProductTitle.ProductID 
                                    INNER JOIN Language ON ProductTitle.LanguageID = Language.LanguageID 
                                    INNER JOIN ProductCategory ON Product.ProductID = ProductCategory.ProductID 
                                    ON AdditionalInfo.LanguageID = Language.LanguageID AND 
                                    AdditionalInfo.ProductID = Product.ProductID 
                                    ON Brand.BrandID = Product.BrandID 
                                    ON FullInfo.LanguageID = Language.LanguageID AND 
                                    FullInfo.ProductID = Product.ProductID 
                                    LEFT OUTER JOIN ShortInfo ON Product.ProductID = ShortInfo.ProductID AND 
                                    Language.LanguageID = ShortInfo.LanguageID
                                    WHERE (Language.LanguageID = @LanguageID AND (Product.IsPublished = 1)) 
                                    ORDER BY RegistrationDate DESC";

                // fix for num of products since it won't work with standard parameter
                sqlQuery = sqlQuery.Replace("@NumOfProducts", numberOfProducts.ToString());

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataTable GetProductsByCategoryID(string categoryID, string languageID, bool onlyPublishedProducts)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Product.ProductID, Product.ArtNo, Product.Price, Product.IsPublished, Brand.BrandName, 
                                    ProductTitle.Title, ShortInfo.ShortInfo, FullInfo.FullInfo, AdditionalInfo.AdditionalInfo, 
                                    Language.Language, ProductCategory.ProductCategoryID, ProductCategory.CategoryID, ProductCategory.Priority,
                                        (SELECT TOP 1 [File].FileName FROM  ProductFile INNER JOIN [File] ON ProductFile.FileID = [File].FileID
                                         WHERE ProductID = Product.ProductID ORDER BY ProductFile.Priority ASC) AS FileName
                                    FROM FullInfo 
                                    RIGHT OUTER JOIN Brand 
                                    RIGHT OUTER JOIN AdditionalInfo 
                                    RIGHT OUTER JOIN Product 
                                    INNER JOIN ProductTitle ON Product.ProductID = ProductTitle.ProductID 
                                    INNER JOIN Language ON ProductTitle.LanguageID = Language.LanguageID 
                                    INNER JOIN ProductCategory ON Product.ProductID = ProductCategory.ProductID 
                                    ON AdditionalInfo.LanguageID = Language.LanguageID AND 
                                    AdditionalInfo.ProductID = Product.ProductID 
                                    ON Brand.BrandID = Product.BrandID 
                                    ON FullInfo.LanguageID = Language.LanguageID AND 
                                    FullInfo.ProductID = Product.ProductID 
                                    LEFT OUTER JOIN ShortInfo ON Product.ProductID = ShortInfo.ProductID AND 
                                    Language.LanguageID = ShortInfo.LanguageID";
                if (onlyPublishedProducts)
                {
                    sqlQuery += " WHERE (Language.LanguageID = @LanguageID) AND (ProductCategory.CategoryID = @CategoryID) AND (Product.IsPublished = 1)";
                }
                else
                {
                    sqlQuery += " WHERE (Language.LanguageID = @LanguageID) AND (ProductCategory.CategoryID = @CategoryID)";
                }

                sqlQuery += " ORDER BY ProductCategory.Priority ASC";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public string SaveNewProduct()
        {
            // add a new empty organization row
            string id = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"INSERT INTO Product (RegistrationDate) VALUES (@RegistrationDate) 
                                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                conn.Open();
                id = cmd.ExecuteScalar().ToString();
            }
            return id;
        }

        public void SaveNewProduct(string productID, string categoryID, string availabilityID, List<string> colorIDs, List<string> sizeIDs, string artNo, string price, bool isPublished, string title, string shortInfo, string fullInfo, string additionalInfo)
        {
            // save size for product
            SaveProductSizes(productID, sizeIDs);
            // save color for product
            SaveProductColors(productID, colorIDs);

            // update product table
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"UPDATE Product SET ArtNo=@ArtNo, AvailabilityID=@AvailabilityID, Price=@Price, IsPublished=@IsPublished WHERE ProductID = @ProductID
                                    -- insert product to Categorty
                                    INSERT INTO ProductCategory (ProductID, CategoryID, Priority) VALUES (@ProductID, @CategoryID, @Priority)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@ArtNo", artNo);
                cmd.Parameters.AddWithValue("@AvailabilityID", availabilityID);
                cmd.Parameters.AddWithValue("@Price", price.Replace(",", "."));
                cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@Priority", GetANewHighestPriority(categoryID));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Add attributes for product in multiple language
            DataTable table = GetLanguages();
            // add a row for every language 
            foreach (DataRow row in table.Rows)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"-- insert product to ProductTitle
                                        INSERT INTO ProductTitle (ProductID, LanguageID, Title) VALUES (@ProductID, @LanguageID, @Title)
                                        -- insert product to ShortInfo
                                        INSERT INTO ShortInfo (ProductID, LanguageID, ShortInfo) VALUES (@ProductID, @LanguageID, @ShortInfo)
                                        -- insert product to FullInfo
                                        INSERT INTO FullInfo (ProductID, LanguageID, FullInfo) VALUES (@ProductID, @LanguageID, @FullInfo)
                                        -- insert product to AdditionalInfo
                                        INSERT INTO AdditionalInfo (ProductID, LanguageID, AdditionalInfo) VALUES (@ProductID, @LanguageID, @AdditionalInfo)";

                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@ShortInfo", shortInfo);
                    cmd.Parameters.AddWithValue("@FullInfo", fullInfo);
                    cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                    cmd.Parameters.AddWithValue("@LanguageID", row["LanguageID"].ToString());
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveProductColors(string productID, List<string> colorIDs)
        {
            // start removing any existing sizes connected to product
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM ProductColor WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // save new colors to product
            foreach (string colorID in colorIDs)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string sqlQuery = @"INSERT INTO ProductColor (ProductID, ColorID) 
                                        VALUES (@ProductID, @ColorID)";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@ColorID", colorID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveProductSizes(string productID, List<string> sizeIDs)
        {
            // start removing any existing sizes connected to product
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM ProductSize WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // save new sizes to product
            foreach (string sizeID in sizeIDs)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    string sqlQuery = @"INSERT INTO ProductSize (ProductID, SizeID) 
                                        VALUES (@ProductID, @SizeID)";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@SizeID", sizeID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveProduct(string productID, string languageID, string availabilityID, List<string> colorIDs, List<string> sizeIDs, string artNo, string price, bool isPublished, string title, string shortInfo, string fullInfo, string additionalInfo)
        {
            // save size for product
            SaveProductSizes(productID, sizeIDs);
            // save size for product
            SaveProductColors(productID, colorIDs);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"UPDATE Product SET ArtNo=@ArtNo, AvailabilityID=@AvailabilityID, Price=@Price, IsPublished=@IsPublished WHERE ProductID=@ProductID
                                    UPDATE ProductTitle SET Title=@Title WHERE LanguageID=@LanguageID AND ProductID=@ProductID
                                    UPDATE ShortInfo SET ShortInfo=@ShortInfo WHERE LanguageID=@LanguageID AND ProductID=@ProductID
                                    UPDATE FullInfo SET FullInfo=@FullInfo WHERE LanguageID=@LanguageID AND ProductID=@ProductID
                                    UPDATE AdditionalInfo SET AdditionalInfo=@AdditionalInfo WHERE LanguageID=@LanguageID AND ProductID=@ProductID";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ArtNo", artNo);
                cmd.Parameters.AddWithValue("@AvailabilityID", availabilityID);
                cmd.Parameters.AddWithValue("@Price", price.Replace(",", "."));
                cmd.Parameters.AddWithValue("@IsPublished", isPublished);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@ShortInfo", shortInfo);
                cmd.Parameters.AddWithValue("@FullInfo", fullInfo);
                cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a product in database
        /// </summary>
        /// <param name="productID">the product's id</param>
        public void DeleteProduct(string productID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM Product WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Cleans up products that does not have a title. Since they don't have title it means that 
        /// they are half-created and should be removed.
        /// </summary>
        public void CleanUpProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM Product WHERE ProductID IN
                                (
	                                SELECT DISTINCT Product.ProductID
                                    FROM Product LEFT OUTER JOIN
                                    ProductTitle ON Product.ProductID = ProductTitle.ProductID
                                    WHERE     (ProductTitle.Title IS NULL)
                                )";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            
        }

        public DataTable GetAvailabilities(string selectedLangID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT     Availability.AvailabilityID, AvailabilityName.AvailabilityName
                                    FROM         Availability INNER JOIN
                                                          AvailabilityName ON Availability.AvailabilityID = AvailabilityName.AvailabilityID
                                    WHERE     (AvailabilityName.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", selectedLangID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public bool IsItANewProduct(string productID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT DISTINCT Product.ProductID
                                    FROM Product LEFT OUTER JOIN
                                    ProductTitle ON Product.ProductID = ProductTitle.ProductID
                                    WHERE     (ProductTitle.Title IS NULL) AND Product.ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Order Management



        #endregion

        #region Priority Management ProductCategory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID">The ID of the item that this file belongs to.</param>
        /// <returns>The next available priority for an item.</returns>
        public string GetANewHighestPriority(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT MAX(Priority)+1 AS Priority FROM ProductCategory WHERE CategoryID = @CategoryID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();

                // if null is the result then there aren't any files for this produt yet, therefore the first image should have prio 1
                if (string.IsNullOrEmpty(result))
                    return "1";
                else
                    return result;
            }
        }

        public string GetANewLowestPriority(string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT MIN(Priority)-1 AS Priority FROM ProductCategory WHERE CategoryID = @CategoryID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();

                // if null is the result then there aren't any files for this produt yet, therefore the first image should have prio 1
                if (string.IsNullOrEmpty(result))
                    return "1";
                else
                    return result;
            }
        }

        public void SetPriorityByProductCategoryID(string productCategoryID, string priority)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"UPDATE [ProductCategory] SET Priority = @Priority WHERE ProductCategoryID = @ProductCategoryID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductCategoryID", productCategoryID);
                cmd.Parameters.AddWithValue("@Priority", priority);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DecreasePriority(string productCategoryID, string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this productfile
                                    SELECT @CurrentPriority=Priority FROM ProductCategory WHERE ProductCategoryID = @ProductCategoryID
                                    -- check that it isnt already lowest prio
                                    IF(@CurrentPriority>1)
                                    BEGIN
                                        -- increase priority for productfile having next lower prio then current one
                                        UPDATE ProductCategory SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority-1) AND CategoryID = @CategoryID
                                        -- decrease priority for the current productfile
                                        UPDATE ProductCategory SET Priority = (@CurrentPriority-1) WHERE ProductCategoryID = @ProductCategoryID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@productCategoryID", productCategoryID);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public void IncreasePriority(string productCategoryID, string categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this productfile
                                    SELECT @CurrentPriority=Priority FROM ProductCategory WHERE ProductCategoryID = @ProductCategoryID
                                    -- check that it isnt already highest prio
                                    IF(@CurrentPriority<(select max(priority) from ProductCategory WHERE CategoryID = (SELECT CategoryID FROM ProductCategory WHERE ProductCategoryID = @ProductCategoryID)))
                                    BEGIN
	                                    -- decrease priority for productfile having next higher prio then current one
	                                    UPDATE ProductCategory SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority+1) AND CategoryID = @CategoryID
	                                    -- increase priority for the current productfile
	                                    UPDATE ProductCategory SET Priority = (@CurrentPriority+1) WHERE ProductCategoryID = @ProductCategoryID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductCategoryID", productCategoryID);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        #endregion

        #region Product Colors

        public DataTable GetColors(string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT     Color.ColorID, ColorName.ColorName
                                    FROM         Color INNER JOIN
                                                          ColorName ON Color.ColorID = ColorName.ColorID
                                    WHERE     (ColorName.LanguageID = @LanguageID)
                                    ORDER BY ColorName";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataTable GetColorsByProductID(string productID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT     Color.ColorID, ColorName.ColorName
                                    FROM         Color INNER JOIN
                                    ColorName ON Color.ColorID = ColorName.ColorID INNER JOIN
                                    ProductColor ON Color.ColorID = ProductColor.ColorID
                                    WHERE     (ColorName.LanguageID = @LanguageID) AND (ProductColor.ProductID = @ProductID)
                                    ORDER BY ColorName.ColorName";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public List<string> GetColorIDsByProductID(string productID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT     Color.ColorID, ColorName.ColorName
                                    FROM         Color INNER JOIN
                                    ColorName ON Color.ColorID = ColorName.ColorID INNER JOIN
                                    ProductColor ON Color.ColorID = ProductColor.ColorID
                                    WHERE     (ColorName.LanguageID = @LanguageID) AND (ProductColor.ProductID = @ProductID)
                                    ORDER BY ColorName.ColorName";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                List<string> selectedValues = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    selectedValues.Add(row["ColorID"].ToString());
                }
                return selectedValues;
            }
        }

        #endregion

        #region Product Sizes

        public DataTable GetSizes(string sizeTypeID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT SizeID, Size FROM Size WHERE SizeTypeID = @SizeTypeID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@SizeTypeID", sizeTypeID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataTable GetSizesByProductID(string productID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Size.SizeID, Size.Size
                                    FROM Size INNER JOIN ProductSize 
                                    ON Size.SizeID = ProductSize.SizeID
                                    WHERE (ProductSize.ProductID = @ProductID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public List<string> GetSizeIDsByProductID(string productID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT Size.SizeID, Size.Size
                                    FROM Size INNER JOIN ProductSize 
                                    ON Size.SizeID = ProductSize.SizeID
                                    WHERE (ProductSize.ProductID = @ProductID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                List<string> selectedValues = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    selectedValues.Add(row["SizeID"].ToString());
                }
                return selectedValues;
            }
        }

        #endregion

        #region Language Management

        public DataTable GetLanguages()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT LanguageID, Language FROM Language ORDER BY Priority";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        #endregion

    }
}
