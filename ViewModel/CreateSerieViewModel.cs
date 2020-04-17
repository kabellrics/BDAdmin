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
using System.Windows.Navigation;

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
        public Serie Serie { get; set; }
        public String Name
        {
            get
            {
                    return Serie.Name;
            }
            set
            {
                Serie.Name = value;
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
            get { 
            //{
            //    if (string.IsNullOrEmpty(ImgPath) || string.IsNullOrWhiteSpace(ImgPath))
            //        return null;
            //    else
                    return Serie.Image;
            }
            set
            {
                Serie.Image = value;
                RaisePropertyChanged();
            }
        }

        private String _name;
        private String _imgPath;
        private byte[] _imgByte;


        public CreateSerieViewModel(IFrameNavigationService navigationService, IBusinessSerie businessSerie)
        {
            Serie = null;
            _imgByte = null;
            _navigationService = navigationService;
            _businessSerie = businessSerie;
            GoBackPage = new RelayCommand(() => GoBack());
            CancelPage = new RelayCommand(() => GoBack());
            SavePage = new RelayCommand(async () => await CreateSerie());
            TexChangedEvent = new RelayCommand(() => LoadingImg());
            if (_navigationService.Parameter == null) { 
                Serie = new Serie();
                Serie.ID = -1;
            }
            else
                Serie = _navigationService.Parameter as Serie;
        }

        private void LoadingImg()
        {
            ImgByte = File.ReadAllBytes(ImgPath);
        }

        private void GoBack()
        {
            _navigationService.NavigateTo("Splash", null);
        }
        private async Task CreateSerie()
        {
            Serie.Name = Name;
            Serie.Image = ImgByte;
            if(!string.IsNullOrEmpty(ImgPath) || !string.IsNullOrWhiteSpace(ImgPath))
                Serie.ImgExtension = Path.GetExtension(ImgPath);
            if (Serie.ID == -1)
                await _businessSerie.Create(Serie);
            else
                await _businessSerie.Update(Serie);
            GoBack();
        }
    }
}
