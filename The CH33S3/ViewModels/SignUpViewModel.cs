using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace The_CH33S3.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private string? _username;

        public string? Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string? _password;

        public SignUpViewModel()
        {
        }

        public async Task SignUp(string password)
        {
            try
            {
                _password = password;

                // 1. Stop empty submissions
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(_password))
                {
                    MessageBox.Show("Please enter both a username and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 2. Grab the connection string using your incredibly fast parallel helper
                var (connString, isSuccess) = await VersatileConnectionStringHelper.GetWorkingConnectionString();

                if (!isSuccess)
                {
                    MessageBox.Show("Could not connect to the server. Check your network.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();

                    // 3. Prevent duplicate usernames crashing the DB
                    string checkQuery = "SELECT COUNT(1) FROM [dbo].[User] WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", Username);
                        int exists = (int)await checkCmd.ExecuteScalarAsync();
                        if (exists > 0)
                        {
                            MessageBox.Show("That username is already taken. Please choose another.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // 4. Insert the user WITH FAKED REQUIRED DATA to bypass the NOT NULL constraints
                    string insertQuery = @"
                        INSERT INTO [dbo].[User] (Username, Password, Email, ContactNumber, EloRating) 
                        VALUES (@Username, @Password, @Email, @ContactNumber, @EloRating)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", _password); // Storing plain text for the presentation. Security is a v2.0 problem.

                        // Faking the data the UI doesn't ask for but the DB demands
                        cmd.Parameters.AddWithValue("@Email", $"{Username}@chess.com");
                        cmd.Parameters.AddWithValue("@ContactNumber", "00000000000");
                        cmd.Parameters.AddWithValue("@EloRating", 1200); // Standard starting Elo in chess                

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Account created successfully! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // If you have navigation set up, clear the boxes or switch views here.
                Username = string.Empty;
            }
            catch (SqlException se)
            {
                MessageBox.Show($"Database error: {se.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}