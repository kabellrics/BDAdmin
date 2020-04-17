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
    public class SplashViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessBackground _businessBackground;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 

        public SplashViewModel(IFrameNavigationService navigationService, IBusinessBackground businessBackground)
        {
            _navigationService = navigationService;
            _businessBackground = businessBackground;
            MoveToSeriePage = new RelayCommand(() => MoveToSerie());
            MoveToFichierPage = new RelayCommand(() => MoveToFichier());
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Title = "BD Comics Admin";
                SerieBT = "BD Comics";
                FichierBT = "Fichier";
            }
            else
            {
                // Code runs "for real"
                Title = "BD Comics Admin";
                SerieBT = "BD Comics";
                FichierBT = "Fichier";
            }
            LoadingBck();
        }
        private async void LoadingBck()
        {
            BckImg = await _businessBackground.GetRandomBackground();
        }
        private void MoveToFichier()
        {
            _navigationService.NavigateTo("Fichier", null);
        }

        private void MoveToSerie()
        {
            _navigationService.NavigateTo("Working", null);
        }

        public RelayCommand MoveToSeriePage { get; private set; }
        public RelayCommand MoveToFichierPage { get; private set; }


        private byte[] _bckImg;
        public byte[] BckImg
        {
            get { return _bckImg; }
            set { _bckImg = value; RaisePropertyChanged(); }
        }
        public string Title { get; set; }
        public string SerieBT { get; set; }
        public string FichierBT { get; set; }
    }
}
