using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_CH33S3.ViewModels
{
    public class ContainerViewModel : BaseViewModel
    {
        private BaseViewModel? _currentViewModel;
        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ContainerViewModel(BaseViewModel viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}
