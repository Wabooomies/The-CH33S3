using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_CH33S3.Models
{
    internal class UserModel
    {
        private string? _username;
        private string? _password;

        public string? Username
        {
            get => _username;
            set => _username = value;
        }

        public string? Password
        {
            protected get => _password;
            set => _password = value;
        }

        public UserModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
