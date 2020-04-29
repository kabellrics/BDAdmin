using BDAdmin.Modal.Interface;
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
    public class FichierViewModel : ViewModelBase, IHirarchicalItemViewModel
    {
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;
        private readonly IBusinessPage _businessPage;
        private readonly IDialogService _dialogservice;
        private ICommand _ChangeImageForSerie;
        private ICommand _ChangeParentCommand;
        private bool _haschanged;
        private bool _IsExpanded;
        private bool _IsChecked;
        private bool _isSelected;
        private string _numberOfChild;
        private ObservableCollection<PageViewModel> _childs;
        public FichierViewModel()
        {
            _businessFichier = ViewModelLocator.BusinessFichier();
            _businessSerie = ViewModelLocator.BusinessSerie();
            _businessPage = ViewModelLocator.BusinessPage();
            _dialogservice = ViewModelLocator.DialogService();
            Childs = new ObservableCollection<PageViewModel>();
        }
        public async Task Initialize(Fichier fichier)
        {
            this.Fichier = fichier;
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
        private Fichier _fichier;
        private Serie _parent;
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
        public async void GetChild()
        {
            try
            {
                Childs.Clear();
                var pagechild = await _businessPage.GetAllByIdParent(Fichier.ID);
                foreach(Page page in pagechild.OrderBy(x => x.Ordre))
                {
                    Childs.Add(new PageViewModel(page));
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        public ObservableCollection<PageViewModel> Childs
        {
            get { return _childs; }
            set { _childs = value; RaisePropertyChanged(); }
        }
        public Fichier Fichier { get => _fichier; set => _fichier = value; }
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

        public ICommand ChangeImageForSerie
        {
            get
            {
                return _ChangeImageForSerie ?? (_ChangeImageForSerie = new RelayCommand(ChangeImage));
            }
        }

        public ICommand ChangeParentCommand {
            get
            {
                return _ChangeParentCommand ?? (_ChangeParentCommand = new RelayCommand(ChangeParent));
            }
        }
        private async void ChangeParent()
        {
            //var newParent = _dialogservice.ShowChooseParent(this);
            //if (newParent != null)
            //{
            //    Fichier.ParentID = newParent.Serie.ID;
            //    await _businessFichier.Update(Fichier);
            //    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new NotificationMessage("Update"));
            //}
        }
        private void ChangeImage()
        {
            //_navigationService.NavigateTo("CreateSerie", Serie);
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
        public byte[] Image
        {
            get
            {
                return _fichier.Image;
            }
            set
            {
                _fichier.Image = value;
                RaisePropertyChanged();
                Haschanged = true;
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
        public string Name
        {
            get
            {
                return _fichier.Name;
            }
            set
            {
                _fichier.Name = value;
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
    }
}
