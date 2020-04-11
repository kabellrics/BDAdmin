using BDAdmin.Navigation;
using Business;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel
{
    public class FichierViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;
        public RelayCommand GoBackPage { get; private set; }
        public string Title { get; set; }
        public FichierViewModel(IFrameNavigationService navigationService, IBusinessFichier businessFichier, IBusinessSerie businessSerie)
        {
            _navigationService = navigationService;
            _businessFichier = businessFichier;
            _businessSerie = businessSerie;
            GoBackPage = new RelayCommand(() => GoBack());
            Title = "Fichier";
        }

        private void GoBack()
        {
            _navigationService.NavigateTo("Home", null);
        }
    }
}
