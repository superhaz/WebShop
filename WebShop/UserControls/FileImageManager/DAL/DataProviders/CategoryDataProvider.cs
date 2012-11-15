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
    public class CategoryDataProvider:IItemDataProvider
    {
        private static string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region IItemProvider Members

        #region Get Methods

        public DataTable GetItemFiles(string CategoryNameID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT CategoryNameFile.FileID, [File].FileName
                                    FROM CategoryNameFile 
                                    INNER JOIN [File] ON CategoryNameFile.FileID = [File].FileID
                                    WHERE (CategoryNameFile.CategoryNameID = @CategoryNameID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameID", CategoryNameID);
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
                // NOTE! To adapt to the factory pattern we need to rename the CategoryNameFileID => ItemFileID and CategoryNameID => ItemID etc
                string sqlQuery = @"SELECT     CategoryNameFile.CategoryNameFileID AS ItemFileID, CategoryNameFile.CategoryNameID AS ItemID, CategoryNameFile.Priority, [File].FileID, [File].FileName, [File].FileSize
                                    FROM         CategoryNameFile INNER JOIN
                                    [File] ON CategoryNameFile.FileID = [File].FileID 
                                    WHERE CategoryNameID = @CategoryNameID AND MarkedForDeletion = 0
                                    ORDER BY Priority ASC";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
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
                                        SELECT @MaxPriority = MAX(Priority) FROM CategoryNameFile WHERE CategoryNameID=@CategoryNameID
                                        IF (@Priority < 1)
                                            SET @Priority = @MaxPriority
                                        ELSE IF(@Priority > @MaxPriority)
                                            SET @Priority = 1

                                        SELECT     CategoryNameFile.CategoryNameFileID, CategoryNameFile.CategoryNameID, CategoryNameFile.Priority, [File].FileID, [File].FileName, [File].FileSize
                                            FROM         CategoryNameFile INNER JOIN
                                            [File] ON CategoryNameFile.FileID = [File].FileID 
                                            WHERE CategoryNameID = @CategoryNameID AND Priority = @Priority
                                            ORDER BY Priority";

                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
                    if (string.IsNullOrEmpty(priority))
                        cmd.Parameters.AddWithValue("@Priority", "1");
                    else
                        cmd.Parameters.AddWithValue("@Priority", priority);

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
                //inserts a new file and connects the file to a CategoryName
                string sqlQuery = @"UPDATE [CategoryNameFile] SET Priority = @Priority WHERE CategoryNameFileID = @CategoryNameFileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameFileID", itemFileID);
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
                //inserts a new file and connects the file to a CategoryName
                string sqlQuery = @"SELECT MAX(Priority)+1 AS Priority FROM CategoryNameFile WHERE CategoryNameID = @CategoryNameID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
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
                //inserts a new file and connects the file to a CategoryName
                string sqlQuery = @"SELECT MIN(Priority)-1 AS Priority FROM CategoryNameFile WHERE CategoryNameID = @CategoryNameID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
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
                                    -- assign current priority for this CategoryNamefile
                                    SELECT @CurrentPriority=Priority FROM CategoryNameFile WHERE CategoryNameFileID = @CategoryNameFileID
                                    -- check that it isnt already highest prio
                                    IF(@CurrentPriority<(select max(priority) from CategoryNameFile WHERE CategoryNameID = (SELECT CategoryNameID FROM CategoryNameFile WHERE CategoryNameFileID = @CategoryNameFileID)))
                                    BEGIN
	                                    -- decrease priority for CategoryNamefile having next higher prio then current one
	                                    UPDATE CategoryNameFile SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority+1) AND CategoryNameID = @CategoryNameID
	                                    -- increase priority for the current CategoryNamefile
	                                    UPDATE CategoryNameFile SET Priority = (@CurrentPriority+1) WHERE CategoryNameFileID = @CategoryNameFileID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameFileID", itemFileID);
                cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
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
                                    -- assign current priority for this CategoryNamefile
                                    SELECT @CurrentPriority=Priority FROM CategoryNameFile WHERE CategoryNameFileID = @CategoryNameFileID
                                    -- check that it isnt already lowest prio
                                    IF(@CurrentPriority>1)
                                    BEGIN
                                        -- increase priority for CategoryNamefile having next smaller prio then current one
                                        UPDATE CategoryNameFile SET Priority = @CurrentPriority WHERE Priority = (@CurrentPriority-1) AND CategoryNameID = @CategoryNameID
                                        -- decrease priority for the current CategoryNamefile
                                        UPDATE CategoryNameFile SET Priority = (@CurrentPriority-1) WHERE CategoryNameFileID = @CategoryNameFileID
                                    END";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@CategoryNameFileID", itemFileID);
                cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
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
                //inserts a new file and connects the file to a CategoryName
                string sqlQuery = @"INSERT INTO CategoryNameFile (CategoryNameID, FileID, Priority) VALUES (@CategoryNameID, @FileID, @Priority)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", fileID);

                if (!string.IsNullOrEmpty(itemID))
                    cmd.Parameters.AddWithValue("@CategoryNameID", itemID);
                else
                    cmd.Parameters.AddWithValue("@CategoryNameID", DBNull.Value);
                cmd.Parameters.AddWithValue("@Priority", GetANewHighestPriority(itemID));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteItemFileByFileID(string fileID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM CategoryNameFile WHERE FileID = @FileID";

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
