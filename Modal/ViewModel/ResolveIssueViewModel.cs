using BDAdmin.ViewModel;
using Business;
using Common;
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
    public class ResolveIssueViewModel : ViewModelBase
    {

        #region BaseModal
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

        private void ValidateClick(object parameter)
        {
            ChoosenIssue = IssueProposals[SelectedIndex];
            CloseDialogWithResult(parameter as Window, true);
        }
        private void CancelClick(object parameter)
        {
            CloseDialogWithResult(parameter as Window, false);
        }

        public void CloseDialogWithResult(Window dialog, bool result)
        {
            if (dialog != null)
                dialog.DialogResult = result;
        }
        #endregion
        private List<ComicVineResult> _IssueProposals;
        private string _NameToSearch;
        private string _ChooseIssuename;
        private IBusinessComicVine _businessComicVine;
        private int _SelectedIndex;
        private ComicVineResult _ChoosenIssue;

        public ResolveIssueViewModel(String Name)
        {
            NameToSearch = Name;
            _businessComicVine = ViewModelLocator.BusinessComicVine();
        }
        private ICommand _ResolveCommand;
        public ICommand ResolveCommand
        {
            get
            {
                return _ResolveCommand ?? (_ResolveCommand = new RelayCommand(StartResolve));
            }
        }
        public ComicVineResult ChoosenIssue
        {
            get { return _ChoosenIssue; }
            set { _ChoosenIssue = value;RaisePropertyChanged(); }
        }
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); ChangeSelected(); }
        }
        public List<ComicVineResult> IssueProposals
        {
            get { return _IssueProposals; }
            set { _IssueProposals = value;RaisePropertyChanged(); }
        }
        public String NameToSearch
        {
            get { return _NameToSearch; }
            set { _NameToSearch = value; RaisePropertyChanged(); }
        }
        public String ChooseIssuename
        {
            get { return _ChooseIssuename; }
            set { _ChooseIssuename = value; RaisePropertyChanged(); }
        }
        private void ChangeSelected()
        {
            try
            {
                ChoosenIssue = IssueProposals[SelectedIndex];
                ChooseIssuename = ChoosenIssue.Name;
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        private async void StartResolve()
        {
            IssueProposals = await _businessComicVine.GetProposalForFichier(NameToSearch);
        }
    }
}
