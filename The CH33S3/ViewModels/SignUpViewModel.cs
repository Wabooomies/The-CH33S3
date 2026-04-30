using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_CH33S3.Models;

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
            }
            catch (SqlException se)
            {
                MessageBox.Show($"Database error: {se.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
