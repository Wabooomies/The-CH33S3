using System.Configuration;
using System.Data;
using System.Windows;
using The_CH33S3.ViewModels;
using The_CH33S3.Views;

namespace The_CH33S3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ContainerViewModel? _containerViewModel { get; set; }

        public App()
        {
            _containerViewModel = new ContainerViewModel(new LogInViewModel());
        }
    }
}
