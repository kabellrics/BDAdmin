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
    public class ShowImageViewModel : ViewModelBase
    {
        private byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; RaisePropertyChanged(); }
        }
        public ShowImageViewModel(byte[] data)
        {
            Data = data;
        }
        private ICommand _yesCommand;
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand<object>(ValidateClick));
            }
        }
        private void ValidateClick(object parameter)
        {
            CloseDialogWithResult(parameter as Window, true);
        }
        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }
    }
}
