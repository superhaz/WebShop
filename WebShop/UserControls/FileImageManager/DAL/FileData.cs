using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using WebShop.UserControls.FileImageManager.BLL;
using System.Data;

namespace WebShop.UserControls.FileImageManager.DAL
{
    public class FileData
    {
        private static string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        #region Get & Save 

        public static string GetFileNameByFileID(string fileID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"SELECT FileName FROM [File] WHERE FileID = @FileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", fileID);
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
        }

        /// <summary>
        /// Saves a file's information to the File table
        /// </summary>
        /// <param name="filename">the name of the file</param>
        /// <param name="fileSizeInBytes">the size of the file in byte</param>
        /// <param name="productID">The ID of the item that this file belongs to.</param>
        public static string SaveFile(string filename, int fileSizeInBytes)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"INSERT INTO [File] ([FileName], FileTypeID, FileSize, MarkedForDeletion) VALUES (@Filename, @FileTypeID, @FileSize, 0)
                                   SELECT SCOPE_IDENTITY()";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Filename", filename);
                cmd.Parameters.AddWithValue("@FileTypeID", (int)FileHelper.ConvertFileExtensionToFileType(filename));
                cmd.Parameters.AddWithValue("@FileSize", fileSizeInBytes.ToString());
                
                conn.Open();
                string fileID =  cmd.ExecuteScalar().ToString();
                
                if (string.IsNullOrEmpty(fileID))
                    return "";

                return fileID;
            }
        }

        #endregion


        #region Delete files

        /// <summary>
        /// Deletes file row in database
        /// </summary>
        /// <param name="id">the fileid of a file</param>
        public static void DeleteFile(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"DELETE FROM [File] WHERE FileID = @FileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Removes a file and products relation, and marks the file for deletion
        /// </summary>
        /// <param name="id">the fileid of a file</param>
        public static void MarkFileForDeletion(string fileID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //inserts a new file and connects the file to a product
                string sqlQuery = @"UPDATE [File] SET MarkedForDeletion = 1 WHERE FileID = @FileID";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@FileID", fileID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable GetMarkedFilesForDeletion()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT FileID, FileName FROM [File] WHERE MarkedForDeletion = 1";

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
