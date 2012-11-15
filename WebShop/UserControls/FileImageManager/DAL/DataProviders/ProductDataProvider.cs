using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebShop.UserControls.FileImageManager.DAL;
using WebShop.UserControls.FileImageManager.BLL;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WebShop.UserControls.FileImageManager.DAL.DataProviders
{
    public class ProductDataProvider : IItemDataProvider
    {
        private static string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region IItemProvider Members

        #region Get Methods

        public DataTable GetItemFiles(string productID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT ProductFile.FileID, [File].FileName
                                    FROM ProductFile 
                                    INNER JOIN [File] ON ProductFile.FileID = [File].FileID
                                    WHERE (ProductFile.ProductID = @ProductID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public DataTable GetFilesByItemID(string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // NOTE! To adapt to the factory pattern we need to rename the ProductFileID => ItemFileID and ProductID => ItemID etc
                string sqlQuery = @"SELECT     ProductFile.ProductFileID AS ItemFileID, ProductFile.ProductID AS ItemID, ProductFile.Priority, [File].FileID, [File].FileName, [File].FileSize
                                    FROM         ProductFile INNER JOIN
                                    [File] ON ProductFile.FileID = [File].FileID 
                                    WHERE ProductID = @ProductID AND MarkedForDeletion = 0
                                    ORDER BY Priority ASC";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", itemID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        #endregion

        #region Priority Management

        public DataTable GetFileByItemIDANDPriority(string itemID, string priority)
        {
            if (!string.IsNullOrEmpty(itemID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"-- chck if priority < 1 or priorty > max
                                        DECLARE @MaxPriority int
                                        SELECT @MaxPriority = MAX(Priority) FROM ProductFile WHERE ProductID=@ProductID
                                        IF (@Priority < 1)
                                            SET @Priority = @MaxPriority
                                        ELSE IF(@Priority > @MaxPriority)
                                            SET @Priority = 1

                                        SELECT     ProductFile.ProductFileID, ProductFile.ProductID, ProductFile.Priority, [File].FileID, [File].FileName, [File].FileSize
                                            FROM         ProductFile INNER JOIN
                                            [File] ON ProductFile.FileID = [File].FileID 
                                            WHERE ProductID = @ProductID AND Priority = @Priority
                                            ORDER BY Priority";

                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductID", itemID);
                    if (string.IsNullOrEmpty(priority))
                        cmd.Parameters.AddWithValue("@Priority", 1);    // if using "0" an arithmetic exception is cast on line: SET @Priority = @MaxPriority
                    else
                        cmd.Parameters.AddWithValue("@Priority", Convert.ToInt32(priority));

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }

            return null;
        }

        public void SetPriorityByItemFileID(string itemFileID, string priority)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"UPDATE [ProductFile] SET Priority = @Priority WHERE ProductFileID = @ProductFileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductFileID", itemFileID);
                cmd.Parameters.AddWithValue("@Priority", priority);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID">The ID of the item that this file belongs to.</param>
        /// <returns>Gets a priority that is the highest for an item.</returns>
        public string GetANewHighestPriority(string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT MAX(Priority)+1 AS Priority FROM ProductFile WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", itemID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();

                // if null is the result then there aren't any files for this produt yet, therefore the first image should have prio 1
                if (string.IsNullOrEmpty(result))
                    return "1";
                else
                    return result;
            }
        }

        public string GetANewLowestPriority(string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT MIN(Priority)-1 AS Priority FROM ProductFile WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", itemID);
                conn.Open();
                string result = cmd.ExecuteScalar().ToString();

                // if null is the result then there aren't any files for this produt yet, therefore the first image should have prio 1
                if (string.IsNullOrEmpty(result))
                    return "1";
                else
                    return result;
            }
        }

        public void IncreasePriority(string itemFileID, string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this productfile
                                    SELECT @CurrentPriority=Priority FROM ProductFile WHERE ProductFileID = @ProductFileID
                                    -- check that it isnt already highest prio
                                    IF(@CurrentPriority<(select max(priority) from ProductFile WHERE ProductID = (SELECT ProductID FROM ProductFile WHERE ProductFileID = @ProductFileID)))
                                    BEGIN
	                                    -- decrease priority for productfile having next higher prio then current one
	                                    UPDATE ProductFile SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority+1) AND ProductID = @ProductID
	                                    -- increase priority for the current productfile
	                                    UPDATE ProductFile SET Priority = (@CurrentPriority+1) WHERE ProductFileID = @ProductFileID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductFileID", itemFileID);
                cmd.Parameters.AddWithValue("@ProductID", itemID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DecreasePriority(string itemFileID, string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // -- NOTE! Lowest priority is 1 everything else is higher....
                string sqlQuery = @"-- declare variable
                                    DECLARE @CurrentPriority int
                                    -- assign current priority for this productfile
                                    SELECT @CurrentPriority=Priority FROM ProductFile WHERE ProductFileID = @ProductFileID
                                    -- check that it isnt already lowest prio
                                    IF(@CurrentPriority>1)
                                    BEGIN
                                        -- increase priority for productfile having next smaller prio then current one
                                        UPDATE ProductFile SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority-1) AND ProductID = @ProductID
                                        -- decrease priority for the current productfile
                                        UPDATE ProductFile SET Priority = (@CurrentPriority-1) WHERE ProductFileID = @ProductFileID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ProductFileID", itemFileID);
                cmd.Parameters.AddWithValue("@ProductID", itemID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Save & Delete 

        public void SaveItemFile(string fileID, string itemID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"INSERT INTO ProductFile (ProductID, FileID, Priority) VALUES (@ProductID, @FileID, @Priority)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", fileID);

                if (!string.IsNullOrEmpty(itemID))
                    cmd.Parameters.AddWithValue("@ProductID", itemID);
                else
                    cmd.Parameters.AddWithValue("@ProductID", DBNull.Value);
                cmd.Parameters.AddWithValue("@Priority", GetANewHighestPriority(itemID));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteItemFileByFileID(string fileID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM ProductFile WHERE FileID = @FileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", fileID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #endregion
    }
}
