using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using The_CH33S3.Models;

namespace The_CH33S3.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
        private BitmapImage? _profilePicture;
        private UserModel? _user;

        public BitmapImage? ProfilePicture
        {
            get => _profilePicture;
            set => SetProperty(ref _profilePicture, value);
        }

        public UserModel? User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }



        public ProfileViewModel(UserModel user)
        {
            User = user;
        }
    }
}
