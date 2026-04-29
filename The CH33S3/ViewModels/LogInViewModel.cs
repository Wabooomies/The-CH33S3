using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace The_CH33S3.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        private string? _username;

        public string? Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string? _password;

        public LogInViewModel()
        {

        }

        public async Task LogIn(string password)
        {

            try
            {
                _password = password;

                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_GetPasswordByUser", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", Username);

                        var result = await command.ExecuteScalarAsync();

                        if (result is not string dbPassword || string.IsNullOrEmpty(dbPassword))
                        {
                            MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (dbPassword == password)
                        {
                            MessageBox.Show($"Login successful! Welcome, {Username}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            var app = (Application.Current as App);
                            app._containerViewModel.CurrentViewModel = new DashboardViewModel();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                MessageBox.Show($"Database error: {se.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
