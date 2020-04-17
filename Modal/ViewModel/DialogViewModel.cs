using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAdmin.Modal.ViewModel
{
    public class DialogViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value;RaisePropertyChanged(); }
        }

        private string _contentButtonYes;
        public string ContentButtonYes
        {
            get { return _contentButtonYes; }
            set { _contentButtonYes = value; RaisePropertyChanged(); }
        }

        private string _contentButtonNo;
        public string ContentButtonNo
        {
            get { return _contentButtonNo; }
            set { _contentButtonNo = value; RaisePropertyChanged(); }
        }

        private string _contentButtonCancel;
        public string ContentButtonCancel
        {
            get { return _contentButtonCancel; }
            set { _contentButtonCancel = value; RaisePropertyChanged(); }
        }

        private ICommand _yesCommand = null;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand<object>(OnYesClicked));
            }
        }

        private ICommand _NoCommand = null;
        public ICommand NoCommand
        {
            get
            {
                return _NoCommand ?? (_NoCommand = new RelayCommand<object>(OnNoClicked));
            }
        }

        private ICommand _CancelCommand = null;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand<object>(OnCancelClicked));
            }
        }

        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }

        private void OnYesClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, true);
        }
        private void OnNoClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
        private void OnCancelClicked(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }
    }
}
