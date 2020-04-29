using BDAdmin.Modal.Interface;
using BDAdmin.Navigation;
using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Business;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAdmin.Modal.ViewModel
{
    public class AddMultiComicViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessSerie _businessSerie;
        private readonly IDialogService _dialogservice;
        private readonly IBusinessPage _businessPage;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessFile _businessFile;
        private readonly IBusinessComicVine _businessComicVine;

        #region Command
        private ICommand _LoadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _LoadedCommand ?? (_LoadedCommand = new RelayCommand(Loading));
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
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand<object>(CancelClick));
            }
        }
        private ICommand _AutomaticOrderCommand;
        public ICommand AutomaticOrderCommand
        {
            get
            {
                return _AutomaticOrderCommand ?? (_AutomaticOrderCommand = new RelayCommand(AutomaticOrder));
            }
        }
        #endregion
        #region Property
        private IEnumerable<string> _files;
        public IEnumerable<string> Files
        {
            get { return _files; }
            set { _files = value; RaisePropertyChanged(); }
        }
        private List<SerieViewModel> _series;
        public List<SerieViewModel> Series
        {
            get { return _series; }
            set { _series = value; RaisePropertyChanged(); }
        }
        private SerieViewModel _seriesSelected;
        public SerieViewModel SeriesSelected
        {
            get { return _seriesSelected; }
            set { _seriesSelected = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<AddSingleComicViewModel> _fichiers;
        public ObservableCollection<AddSingleComicViewModel> Fichiers
        {
            get { return _fichiers; }
            set { _fichiers = value;RaisePropertyChanged(); }
        }
        #endregion
        public AddMultiComicViewModel(IEnumerable<string> files)
        {
            _navigationService = ViewModelLocator.NavigationService();
            _businessSerie = ViewModelLocator.BusinessSerie();
            _businessFichier = ViewModelLocator.BusinessFichier();
            _dialogservice = ViewModelLocator.DialogService();
            _businessFile = ViewModelLocator.BusinessFile();
            _businessComicVine = ViewModelLocator.BusinessComicVine();
            _businessPage = ViewModelLocator.BusinessPage();
            Files = files;
            Fichiers = new ObservableCollection<AddSingleComicViewModel>();
            Series = new List<SerieViewModel>();
        }
        #region Methode
        private async void Loading()
        {
            foreach(string filepath in Files)
            {
                Fichiers.Add(new AddSingleComicViewModel(filepath));
            }
            var series = await _businessSerie.GetAll();
            foreach (var serie in series)
            {
                var s = new SerieViewModel();
                await s.Initialize(serie);
                Series.Add(s);
            }
        }
        private void ValidateClick(object parameter)
        {
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
    }
}
