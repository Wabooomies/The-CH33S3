using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using The_CH33S3.Models;

namespace The_CH33S3.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
        private readonly UserModel _user;
        private string? _profilePictureUrl = null;

        public string? ProfilePictureUrl
        {
            set => SetProperty(ref _profilePictureUrl, value);
        }

        public ICommand OpenProfileCommand { get; }

        public ProfileViewModel(UserModel user)
        {
            _user = user;
            OpenProfileCommand = new AsyncRelayCommand(OpenProfile);
        }

        private async Task OpenProfile()
        {

        }
    }
}
