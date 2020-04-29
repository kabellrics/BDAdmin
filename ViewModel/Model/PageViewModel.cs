using BDAdmin.Modal.Interface;
using Business;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAdmin.ViewModel.Model
{
    public class PageViewModel : ViewModelBase
    {

        private readonly IDialogService _dialogservice;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessPage _businessPage;
        private Page _page;
        public Page Page
        {
            get { return _page; }
            set { _page = value; RaisePropertyChanged(); }
        }
        public int Ordre
        {
            get { return Page.Ordre; }
            set { Page.Ordre = value;RaisePropertyChanged(); }
        }
        public byte[] Image
        {
            get { return Page.Element; }
            set { Page.Element = value;RaisePropertyChanged(); }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value;RaisePropertyChanged(); }
        }
        public PageViewModel(Page page)
        {
            _dialogservice = ViewModelLocator.DialogService();
            _businessFichier = ViewModelLocator.BusinessFichier();
            _businessPage = ViewModelLocator.BusinessPage();
            Page = page;
            IsChecked = false;
        }
        private ICommand _showImgCommand;
        public ICommand ShowImgCommand
        {
            get
            {
                return _showImgCommand ?? (_showImgCommand = new RelayCommand<object>(ZoomImg));
            }
        }

        private void ZoomImg(object obj)
        {
            _dialogservice.ShowImage(obj as byte[]);
        }
    }
}
