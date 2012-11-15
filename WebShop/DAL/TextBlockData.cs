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
    public class TextBlockData : ITextBlockData
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;
        public enum TextBlockNames { News = 1, Contact, FAQ, About, Other, Conditions }

        public DataTable GetTextBlock(string textBlockName, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT TextBlock.TextBlockID, TextBlockLanguage.Title,TextBlockLanguage.TextBlock
                                    FROM TextBlock INNER JOIN TextBlockLanguage ON TextBlock.TextBlockID = TextBlockLanguage.TextBlockID
                                    WHERE (TextBlock.TextBlockName = @TextBlockName) AND (TextBlockLanguage.LanguageID = @LanguageID)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@TextBlockName", textBlockName);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                return table;
            }

        }

        public void SaveTextBlock(string title, string textBlock, string textBlockID, string languageID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"UPDATE TextBlockLanguage SET Title=@Title, TextBlock=@TextBlock WHERE TextBlockID=@TextBlockID AND LanguageID = @LanguageID";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@TextBlock", textBlock);
                cmd.Parameters.AddWithValue("@TextBlockID", textBlockID);
                cmd.Parameters.AddWithValue("@LanguageID", languageID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
