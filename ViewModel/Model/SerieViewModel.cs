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
    public class SerieViewModel : ViewModelBase, IHirarchicalItemViewModel
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
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new NotificationMessage("Update"));
            }
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
        private void ChangeImage()
        {
            if(_dialogservice.ShowCreateOrUpdateSerieModal(this))
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new NotificationMessage("Update"));
            //_navigationService.NavigateTo("CreateSerie", Serie);
        }
        public SerieViewModel()
        {
            _navigationService = ViewModelLocator.NavigationService();
            _businessFichier = ViewModelLocator.BusinessFichier();
            _dialogservice = ViewModelLocator.DialogService();
            _businessSerie = ViewModelLocator.BusinessSerie();
            Childs = new ObservableCollection<IHirarchicalItemViewModel>();
            this.NumberOfChildSerie = string.Format("{0} élément(s)", Childs.Count());
            Haschanged = false;
        }

        public async Task Initialize(Serie serie)
        {
            this.Serie = serie;
            var enfantSerie = await _businessSerie.GetAllByIdParent(Serie.ID);
            var enfantFichier = await _businessFichier.GetAllByIdParent(Serie.ID);
            this.NumberOfChildSerie = string.Format("{0} élément(s)", enfantSerie.Count());
            this.NumberOfChild = string.Format("{0} élément(s)",enfantFichier.Count());
            Haschanged = false;
        }
        public async void GetChild()
        {
            try
            {
                Childs.Clear();
                var seriechild = await _businessSerie.GetAllByIdParent(Serie.ID);
                foreach (var enfant in seriechild.OrderBy(x => x.Name))
                {
                    var enf = new SerieViewModel();
                    await enf.Initialize(enfant);
                    Childs.Add(enf);
                }
                this.NumberOfChildSerie = string.Format("{0} élément(s)", seriechild.Count());
                if (Childs.Count == 0)
                {
                    var filechild = await _businessFichier.GetAllByIdParent(Serie.ID);
                    foreach (var enfant in filechild.OrderBy(x => x.Order))
                    {
                        var enf = new FichierViewModel();
                        await enf.Initialize(enfant);
                        Childs.Add(enf);
                    }
                    this.NumberOfChild = string.Format("{0} élément(s)", filechild.Count());
                }
                if (Childs.Count == 0)
                {
                    var defaultchild = new SerieViewModel();
                    defaultchild.Name = "Pas de sous éléments";
                    Childs.Add(defaultchild);
                }
                Haschanged = false;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        private string _numberOfChildSerie;
        private string _numberOfChild;
        private Serie _serie;
        private bool _isSelected;
        private bool _haschanged;
        private bool _IsExpanded;
        private bool _IsChecked;
        private ObservableCollection<IHirarchicalItemViewModel> _childs;
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
        public ObservableCollection<IHirarchicalItemViewModel> Childs
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
        public string NumberOfChildSerie
        {
            get
            {
                return _numberOfChildSerie;
            }
            set
            {
                _numberOfChildSerie = value;
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
