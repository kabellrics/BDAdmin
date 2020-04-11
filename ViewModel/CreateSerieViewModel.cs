using BDAdmin.Navigation;
using Business;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel
{
    public class CreateSerieViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessSerie _businessSerie;
        public RelayCommand GoBackPage { get; private set; }
        public RelayCommand SavePage { get; private set; }
        public RelayCommand CancelPage { get; private set; }
        public RelayCommand TexChangedEvent { get; private set; }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public String ImgPath
        {
            get
            {
                return _imgPath;
            }
            set
            {
                _imgPath = value;
                RaisePropertyChanged();
                LoadingImg();
            }
        }
        public byte[] ImgByte
        {
            get
            {
                if (string.IsNullOrEmpty(ImgPath) || string.IsNullOrWhiteSpace(ImgPath))
                    return null;
                else
                    return _imgByte;
            }
            set
            {
                _imgByte = value;
                RaisePropertyChanged();
            }
        }

        private String _name;
        private String _imgPath;
        private byte[] _imgByte;
        public CreateSerieViewModel(IFrameNavigationService navigationService, IBusinessSerie businessSerie)
        {
            _navigationService = navigationService;
            _businessSerie = businessSerie;
            GoBackPage = new RelayCommand(() => GoBack());
            CancelPage = new RelayCommand(() => GoBack());
            SavePage = new RelayCommand(async () => await CreateSerie());
            TexChangedEvent = new RelayCommand(() => LoadingImg());
        }

        private void LoadingImg()
        {
            ImgByte = File.ReadAllBytes(ImgPath);
        }

        private void GoBack()
        {
            _navigationService.GoBack();
        }
        private async Task CreateSerie()
        {
            DisplayedSerie disSerie = new DisplayedSerie();
            disSerie.Name = Name;
            disSerie.Image = ImgByte;
            await _businessSerie.Create(disSerie);
            GoBack();
        }
    }
}
