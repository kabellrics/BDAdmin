using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Business;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAdmin.Modal.ViewModel
{
    public class ExtractFileViewModel : ViewModelBase
    {
        private IBusinessFile _businessFile;
        public ExtractFileViewModel(string filepath)
        {
            Messenger.Default.Register<ExtractFileNotificationMessage>(this, ReceiveNotificationMessage);
            _businessFile = ViewModelLocator.BusinessFile();
            Filepath = filepath;
            Pages = new List<Page>();
        }

        private void ReceiveNotificationMessage(ExtractFileNotificationMessage obj)
        {
            if (obj.Message == "Quit")
            {
                IsDone = true;
                //ValidateClick(Window);
            }
            else
            {
                Message = obj.Message;
                IMG = obj.IMG;
            }
        }

        private Window _window;
        public Window Window
        {
            get { return _window; }
            set { _window = value;RaisePropertyChanged(); }
        }
        private List<Page> _pages;
        public List<Page> Pages
        {
            get { return _pages; }
            set { _pages = value;RaisePropertyChanged(); }
        }
        private string _filepath;
        public String Filepath
        {
            get { return _filepath; }
            set { _filepath = value; RaisePropertyChanged(); }
        }
        private bool _isDone;
        public bool IsDone
        {
            get { return _isDone; }
            set { _isDone = value;RaisePropertyChanged(); }
        }
        private string _message;
        private byte[] _img;
        public String Message { get { return _message; } set { _message = value;RaisePropertyChanged(); } }
        public byte[] IMG { get { return _img; } set { _img = value;RaisePropertyChanged(); } }


        private ICommand _LoadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _LoadedCommand ?? (_LoadedCommand = new RelayCommand<object>(StartAnalyse));
            }
        }
        private ICommand _yesCommand;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand<object>(ValidateClick));
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
        private async void StartAnalyse(object parameter)
        {
            Window = parameter as Window;
            await Task.Run(() => 
            {
                var pages = _businessFile.ExtractPageInComics(Filepath);
                Pages = pages.ToList();
            });
        }
        private void ValidateClick(object parameter)
        {
            CloseDialogWithResult(parameter as Window, true);
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
