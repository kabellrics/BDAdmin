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
    public class DialogChooseParentViewModel : ViewModelBase
    {
        private IBusinessSerie _businessSerie;
        private IBusinessFichier _businessFichier;
        private SerieViewModel _selectedParent;
        public SerieViewModel SelectedParent
        {
            get { return _selectedParent; }
            set { _selectedParent = value; RaisePropertyChanged(); }
        }
        public String _selectedParentName;
        public String SelectedParentName
        {
            get {
                return _selectedParentName; }
            set{ _selectedParentName = value; RaisePropertyChanged(); }
        }

        private SerieViewModel _serie;
        public SerieViewModel Serie
        {
            get { return _serie; }
            set { _serie = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<SerieViewModel> _seriesChoiceForFather;
        public ObservableCollection<SerieViewModel> SeriesChoiceForFather
        {
            get { return _seriesChoiceForFather; }
            set { _seriesChoiceForFather = value; RaisePropertyChanged(); }
        }
        public ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get { return _loadedCommand ?? (_loadedCommand = new RelayCommand(Loaded)); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged();
                if (SeriesChoiceForFather.Count > 0)
                    SelectedParentName = SeriesChoiceForFather[SelectedIndex].Name;
                else
                    SelectedParentName = string.Empty;
            }
        }
        public DialogChooseParentViewModel()
        {
            SeriesChoiceForFather = new ObservableCollection<SerieViewModel>();
            SelectedIndex = 0;
            _businessSerie = ViewModelLocator.BusinessSerie();
            _businessFichier = ViewModelLocator.BusinessFichier();
        }
        private async void Loaded()
        {
            
            var ancestorID = await _businessSerie.FirstAncestor(Serie.Serie);
            var descendantId = await _businessSerie.GetAllDescendant(ancestorID);
            var allSerie = await _businessSerie.GetAll();
            var filteredseries = allSerie.Where(x => !descendantId.Contains(x.ID) && x.ID != Serie.Serie.ID && x.ID != ancestorID.ID ).ToList();
            foreach (var serie in filteredseries)
            {
                var dsVM = new SerieViewModel();
                await dsVM.Initialize(serie);
                SeriesChoiceForFather.Add(dsVM);
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
            SelectedParent = SeriesChoiceForFather[SelectedIndex];
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
