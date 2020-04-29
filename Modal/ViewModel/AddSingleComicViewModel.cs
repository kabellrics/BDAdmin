using BDAdmin.Modal.Interface;
using BDAdmin.Navigation;
using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Business;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAdmin.Modal.ViewModel
{
    public class AddSingleComicViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessSerie _businessSerie;
        private readonly IDialogService _dialogservice;
        private readonly IBusinessPage _businessPage;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessFile _businessFile;
        private readonly IBusinessComicVine _businessComicVine;
        private ICommand _DeleteFromPage;
        public ICommand DeleteFromPage
        {
            get
            {
                return _DeleteFromPage ?? (_DeleteFromPage = new RelayCommand(DeleteInTheFile));
            }
        }
        private ICommand _ResolveComicVineCommand;
        public ICommand ResolveComicVineCommand
        {
            get
            {
                return _ResolveComicVineCommand ?? (_ResolveComicVineCommand = new RelayCommand(ResolveComicVine));
            }
        }
        private ICommand _loadingPagesCommand;
        public ICommand LoadingPagesCommand
        {
            get
            {
                return _loadingPagesCommand ?? (_loadingPagesCommand = new RelayCommand(async () => await LoadingPages()));
            }
        }
        private RelayCommand _LoadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                return _LoadedCommand ?? (_LoadedCommand = new RelayCommand(async () => await Loading()));
            }
        }
        private ICommand _LoadCoverCommand;
        public ICommand LoadCoverCommand
        {
            get
            {
                return _LoadCoverCommand ?? (_LoadCoverCommand = new RelayCommand(LoadCoverFromFirstPage));
            }
        }
        private async Task Loading()
        {
            var series = await _businessSerie.GetAll();
            foreach (var serie in series)
            {
                var s = new SerieViewModel();
                await s.Initialize(serie);
                Series.Add(s);
            }
        }
        private void DeleteInTheFile()
        {
            List<PageViewModel> toremove = Pages.Where(x => x.IsChecked == true).ToList();
            foreach (var pagesToDelete in toremove)
                Pages.Remove(pagesToDelete);
        }
        private async void ResolveComicVine()
        {
            //_businessComicVine.GetProposalForFichier(Name);
            var result = await _dialogservice.ShowResolveAttempt(Name);
            if (result != null)
            {
                Name = result.Name;
                Ordre = int.Parse(result.Issue_Number);
                Collection = result.Volume;
                using (var webClient = new WebClient())
                {
                    Image = _businessFile.ResizeFileImageFromWeb(webClient.DownloadData(result.Image), FilePath);
                }
            }
        }
        public async void SaveAll()
        {
            if(SeriesSelected == null)
            {
                _dialogservice.ShowMessageOk("Echec", "Vous devez choisir la série de ce fichier pour pouvoir sauvegarder");
            }
            else
            {
                Fichier newfichier = new Fichier();
                newfichier.Collection = Collection;
                newfichier.Order = Ordre;
                newfichier.Name = Name;
                newfichier.Image = Image;
                newfichier.ParentID = SeriesSelected.Serie.ID;
                await _businessFichier.Create(newfichier);
                var fichier = await _businessFichier.GetFichierAsync(newfichier);
                var pages = Pages.ToList();
                pages.ForEach(x => x.Page.IDFichier = fichier.ID);
                foreach(PageViewModel page in pages)
                {
                    await _businessPage.Create(page.Page);
                }
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new NotificationMessage("Update"));
            }
        }
        private void LoadCoverFromFirstPage()
        {
            if(Pages.Count > 0)
            {
                Image = Pages.First().Image;
            }
        }
        public AddSingleComicViewModel(String filepath)
        {
            _navigationService = ViewModelLocator.NavigationService();
            _businessSerie = ViewModelLocator.BusinessSerie();
            _businessFichier = ViewModelLocator.BusinessFichier();
            _dialogservice = ViewModelLocator.DialogService();
            _businessFile = ViewModelLocator.BusinessFile();
            _businessComicVine = ViewModelLocator.BusinessComicVine();
            _businessPage = ViewModelLocator.BusinessPage();
            FilePath = filepath;
            Name = Path.GetFileNameWithoutExtension(FilePath);
            Fichier = new Fichier();
            Pages = new ObservableCollection<PageViewModel>();
            Series = new List<SerieViewModel>();

            //LoadingPages();
        }
        public async Task LoadingPages()
        {
            var result = await _dialogservice.ShowNotifExtract(FilePath);
            foreach (var Page in result)
            {
                Pages.Add(new PageViewModel(Page));
            }
        }
        private IEnumerable<PageViewModel> LoadPage()
        {
            List<PageViewModel> pages = new List<PageViewModel>();
            foreach (Page page in _businessFile.ExtractPageInComics(FilePath))
            {
                yield return new PageViewModel(page);
            }
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
        private String _filePath;
        public String FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged(); }
        }
        private Fichier _fichier;
        public Fichier Fichier
        {
            get { return _fichier; }
            set { _fichier = value; RaisePropertyChanged(); }
        }
        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }
        private String _collection;
        public String Collection
        {
            get { return _collection; }
            set { _collection = value; RaisePropertyChanged(); }
        }
        private int _ordre;
        public int Ordre
        {
            get { return _ordre; }
            set { _ordre = value; RaisePropertyChanged(); }
        }
        private byte[] _image;
        public byte[] Image
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<PageViewModel> _pages;
        public ObservableCollection<PageViewModel> Pages
        {
            get { return _pages; }
            set { _pages = value; RaisePropertyChanged(); }
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
        #region Command
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
            SaveAll();
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
        #endregion
    }
}
