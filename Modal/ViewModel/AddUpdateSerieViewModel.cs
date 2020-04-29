using BDAdmin.Modal.Interface;
using BDAdmin.Navigation;
using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
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
using System.Windows;
using System.Windows.Input;

namespace BDAdmin.Modal.ViewModel
{
    public class AddUpdateSerieViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessSerie _businessSerie;
        private readonly IDialogService _dialogservice;
        private readonly IBusinessFile _businessFile;
        //public RelayCommand GoBackPage { get; private set; }
        //public RelayCommand SavePage { get; private set; }
        //public RelayCommand CancelPage { get; private set; }
        public RelayCommand TexChangedEvent { get; private set; }
        public RelayCommand ChooseFile { get; private set; }
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
            get
            {
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

        private void LoadingImg()
        {
            ImgByte = _businessFile.ResizeFileImage(ImgPath);
        }

        public AddUpdateSerieViewModel()
        {
            Serie = null;
            _imgByte = null;
            _dialogservice = ViewModelLocator.DialogService();
            _navigationService = ViewModelLocator.NavigationService();
            _businessSerie = ViewModelLocator.BusinessSerie();
            _businessFile = ViewModelLocator.BusinessFile();
            //GoBackPage = new RelayCommand(() => GoBack());
            //CancelPage = new RelayCommand(() => GoBack());
            //SavePage = new RelayCommand(async () => await CreateSerie());
            TexChangedEvent = new RelayCommand(() => LoadingImg());
            ChooseFile = new RelayCommand(() => ChooseImgFile());
            Serie = new Serie();
            Serie.ID = -1;
        }

        private void ChooseImgFile()
        {
            string path = _dialogservice.OpenUniqueFileDialog("Img files (*.jpg;*.png)|*.jpg;*.png");
            if(!string.IsNullOrEmpty(path))
            {
                ImgPath = path;
            }
        }

        private async void CreateSerie()
        {
            Serie.Name = Name;
            Serie.Image = ImgByte;
            if (!string.IsNullOrEmpty(ImgPath) || !string.IsNullOrWhiteSpace(ImgPath))
                Serie.ImgExtension = Path.GetExtension(ImgPath);
            if (Serie.ID == -1)
                await _businessSerie.Create(Serie);
            else
                await _businessSerie.Update(Serie);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new NotificationMessage("Update"));
        }
        private ICommand _yesCommand;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand<object>(ValidateClick));
            }
        }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand<object>(CancelClick));
            }
        }
        private ICommand _LeaveCommand;
        public ICommand LeaveCommand
        {
            get
            {
                return _LeaveCommand ?? (_LeaveCommand = new RelayCommand<object>(QuitClick));
            }
        }
        private void ValidateClick(object parameter)
        {
            CreateSerie();
            CloseDialogWithResult(parameter as Window, true);
        }
        private void CancelClick(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
        private void QuitClick(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }
    }
}
