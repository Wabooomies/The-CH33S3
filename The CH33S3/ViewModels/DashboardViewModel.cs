using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_CH33S3.Models;

namespace The_CH33S3.ViewModels
{
    internal class DashboardViewModel : BaseViewModel
    {
        private BaseViewModel? _currentView;
        private ProfileViewModel? _profileViewModel;

        public BaseViewModel? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        public ProfileViewModel? ProfileViewModel
        {
            get => _profileViewModel;
            set => SetProperty(ref _profileViewModel, value);
        }



        public DashboardViewModel(UserModel user)
        {
            ProfileViewModel = new ProfileViewModel(user);
        }


    }
}
