using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using The_CH33S3.Models;

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

        public ICommand RedirectToSignUpCommand { get; }

        public LogInViewModel()
        {
            RedirectToSignUpCommand = new RelayCommand(RedirectToSignUp);
        }

        public async Task LogIn(string password)
        {
            try
            {
                UserModel? user = null;

                _password = password;

                if (String.IsNullOrEmpty(Username) || string.IsNullOrEmpty(_password))
                {
                    MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = await DatabaseHelper.FindUserCaseSensitive(Username, _password);
                if (result == null)
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                user = new UserModel(Username);

                var app = Application.Current as App;
                if (app is null)
                {
                    MessageBox.Show("Unexpected error: Application instance is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (app._containerViewModel is null)
                {
                    MessageBox.Show("Unexpected error: ContainerViewModel instance is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                app._containerViewModel.CurrentViewModel = new DashboardViewModel(user);
            }
            catch (SqlException se)
            {
                MessageBox.Show($"Database error: {se.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RedirectToSignUp()
        {
            var app = Application.Current as App;

            if (app == null || app._containerViewModel is not ContainerViewModel)
            {
                MessageBox.Show("Unexpected error: Application instance or ContainerViewModel is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            app._containerViewModel.CurrentViewModel = new SignUpViewModel();
        }
    }
}
