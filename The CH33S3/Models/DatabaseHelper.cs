using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_CH33S3.ViewModels;

namespace The_CH33S3.Models
{
    public static class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static async Task<object?> FindUser(string username, string password)
        {
            using (SqlConnection connection = new (connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("sp_GetPasswordByUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);

                    var result = await command.ExecuteScalarAsync();

                    if (result is not string dbPassword || string.IsNullOrEmpty(dbPassword))
                        return null;

                    if (dbPassword != password)
                        return null;

                    return result;
                }
            }
        }

        public static async Task<List<string>> CheckIfUsernameExists()
        {
            List<string> list = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT Username FROM dbo.[User]", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return list;
        }

        public static bool IfUsernameExists (List<string> usernameList, string usernameInput)
        {
            foreach (string username in usernameList)
            {
                if (usernameInput.Equals(username, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
