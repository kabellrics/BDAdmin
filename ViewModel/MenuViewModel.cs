using BDAdmin.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private RelayCommand _loadedCommand;
        private IFrameNavigationService _navigationService;

        public MenuViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Home",null);
                    }));
            }
        }
    }
}
