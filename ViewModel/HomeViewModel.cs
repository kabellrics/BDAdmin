using System;
using BDAdmin.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BDAdmin.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HomeViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            MoveToSeriePage = new RelayCommand(() => MoveToSerie());
            MoveToFichierPage = new RelayCommand(() => MoveToFichier());
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Title = "BD Comics Admin";
                SerieBT = "Serie";
                FichierBT = "Fichier";
            }
            else
            {
                // Code runs "for real"
                Title = "BD Comics Admin";
                SerieBT = "Serie";
                FichierBT = "Fichier";
            }

        }

        private void MoveToFichier()
        {
            _navigationService.NavigateTo("Fichier",null);
        }

        private void MoveToSerie()
        {
            _navigationService.NavigateTo("Serie",null);
        }

        public RelayCommand MoveToSeriePage { get; private set; }
        public RelayCommand MoveToFichierPage { get; private set; }



        public string Title { get; set; }
        public string SerieBT { get; set; }
        public string FichierBT { get; set; }
    }
}