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

        public string? Username
        {
            get => _username;
            set => _username = value;
        }

        public UserModel(string username)
        {
            Username = username;
        }
    }
}
