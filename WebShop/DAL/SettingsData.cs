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
    public class SettingsData :ISettingsData
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["WebShopConnectionString"].ConnectionString;

        public string GetSettingsValue(string settingsName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT SettingsValue
                                    FROM Settings
                                    WHERE SettingsName = @SettingsName";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@SettingsName", settingsName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count > 0)
                    return table.Rows[0]["SettingsValue"].ToString();
                else
                    return "";
            }
        }

        public string GetAccountNumber()
        {
            return GetSettingsValue("AccountNumber");
        }
    }
}
