using BDAdmin.Modal.Interface;
using BDAdmin.Navigation;
using Business;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAdmin.ViewModel.Model
{
    public class SerieViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;
        private readonly IDialogService _dialogservice;
        private ICommand _ChangeImageForSerie;
        private ICommand _ChangeParentCommand;
        public ICommand ChangeImageForSerie
        {
            get
            {
                return _ChangeImageForSerie ?? (_ChangeImageForSerie = new RelayCommand(ChangeImage));
            }
        }
        public ICommand ChangeParentCommand
        {
            get
            {
                return _ChangeParentCommand ?? (_ChangeParentCommand = new RelayCommand(ChangeParent));
            }
        }
        private async void ChangeParent()
        {
            var newParent = _dialogservice.ShowChooseParent(this);
            if (newParent != null)
            {
                Serie.ParentID = newParent.Serie.ID;
                await _businessSerie.Update(Serie);
            }
        }
        private void ChangeImage()
        {
            _navigationService.NavigateTo("CreateSerie", Serie);
        }
        public SerieViewModel()
        {
            _navigationService = ViewModelLocator.NavigationService();
            _businessFichier = ViewModelLocator.BusinessFichier();
            _dialogservice = ViewModelLocator.DialogService();
            _businessSerie = ViewModelLocator.BusinessSerie();
            Childs = new ObservableCollection<SerieViewModel>();
            this.NumberOfChild =string.Format("{0} élément(s)",Childs.Count());
            Haschanged = false;
        }
        
        public async Task Initialize(Serie serie)
        {
            this.Serie = serie;
            var enfantSerie = await _businessSerie.GetAllByIdParent(Serie.ID);
            var enfantFichier = await _businessFichier.GetAllByIdParent(Serie.ID);
            this.NumberOfChild = string.Format("{0} élément(s)", enfantSerie.Count()+ enfantFichier.Count());
            Haschanged = false;
        }
        public async void GetChild()
        {
            try
            {
                Childs.Clear();
                foreach (var enfant in await _businessSerie.GetAllByIdParent(Serie.ID))
                {
                    var enf = new SerieViewModel();
                    await enf.Initialize(enfant);
                    Childs.Add(enf);
                }
                if (Childs.Count == 0)
                {
                    foreach (var enfant in await _businessFichier.GetAllByIdParent(Serie.ID))
                    {
                        var enf = new SerieViewModel();
                        await enf.Initialize(enfant);
                        Childs.Add(enf);
                    }
                }
                if (Childs.Count == 0)
                {
                    var defaultchild = new SerieViewModel();
                    defaultchild.Name = "Pas de sous éléments";
                    Childs.Add(defaultchild);
                }
                this.NumberOfChild = string.Format("{0} élément(s)", Childs.Count());
                Haschanged = false;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        private string _numberOfChild;
        private Serie _serie;
        private bool _isSelected;
        private bool _haschanged;
        private bool _IsExpanded;
        private bool _IsChecked;
        private ObservableCollection<SerieViewModel> _childs;
        public String Name
        {
            get
            {
                return _serie.Name;
            }
            set
            {
                _serie.Name = value;
                RaisePropertyChanged();
                Haschanged = true;
            }
        }
        public byte[] Image
        {
            get
            {
                return _serie.Image;
            }
            set
            {
                _serie.Image = value;
                RaisePropertyChanged();
                Haschanged = true;
            }
        }
        public ObservableCollection<SerieViewModel> Childs
        {
            get { return _childs; }
            set { _childs = value; RaisePropertyChanged(); }
        }
        public Serie Serie
        {
            get
            {
                return _serie;
            }
            set
            {
                _serie = value;
                RaisePropertyChanged();
                Haschanged = true;
            }
        }
        public string NumberOfChild
        {
            get
            {
                return _numberOfChild;
            }
            set
            {
                _numberOfChild = value;
                RaisePropertyChanged();
                Haschanged = true;
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
                Haschanged = true;
            }
        }
        public bool Haschanged
        {
            get
            {
                return _haschanged;
            }
            set
            {
                _haschanged = value;
                RaisePropertyChanged();
            }
        }
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                RaisePropertyChanged();
                if (IsExpanded)
                    GetChild();
            }
        }
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                RaisePropertyChanged();
            }
        }
    }
}
