using BDAdmin.Modal.Interface;
using BDAdmin.Navigation;
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
using System.Windows.Input;

namespace BDAdmin.ViewModel
{
    public class WorkingViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;
        private readonly IDialogService _dialogservice;
        private readonly IBusinessBackground _businessBackground;
        public RelayCommand LoadedCommand
        {
            get; private set;
        }
        public RelayCommand CreatePage { get; private set; }
        private ICommand _ChangeImageForSerie;
        private ICommand _ChangeParentCommand;
        private ICommand _ScanSingleFile;
        public ICommand ScanSingleFileCommand
        {
            get
            {
                return _ScanSingleFile ?? (_ScanSingleFile = new RelayCommand(ScanSingleFile));
            }
        }
        private ICommand _ScanMultiFile;
        public ICommand ScanMultiFileCommand
        {
            get
            {
                return _ScanMultiFile ?? (_ScanMultiFile = new RelayCommand(ScanMultiFile));
            }
        }
        public ICommand ChangeImageForSerie
        {
            get
            {
                return _ChangeImageForSerie ?? (_ChangeImageForSerie = new RelayCommand<SerieViewModel>(ChangeImage));
            }
        }
        public ICommand ChangeParentCommand
        {
            get
            {
                return _ChangeParentCommand ?? (_ChangeParentCommand = new RelayCommand<SerieViewModel>(ChangeParent));
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
        private ObservableCollection<SerieViewModel> _series;
        public RelayCommand GoBackPage { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public string Title { get; set; }
        public int SelectedIndex{ get; set; }
        public ObservableCollection<SerieViewModel> Series { get => _series; set { _series = value; RaisePropertyChanged(); } }

        public WorkingViewModel(IFrameNavigationService navigationService, IBusinessFichier businessFichier, IBusinessSerie businessSerie, IDialogService dialogservice, IBusinessBackground businessBackground)
        {
            try
            {
                _navigationService = navigationService;
                _businessFichier = businessFichier;
                _businessSerie = businessSerie;
                _dialogservice = dialogservice;
                _businessBackground = businessBackground;
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<NotificationMessage>(this, ReceiveNotificationMessage);
                LoadingBck();
                GoBackPage = new RelayCommand(() => GoBack());
                LoadedCommand = new RelayCommand(async () => await Loaded());
                CreatePage = new RelayCommand(() => CreateSerie());
                SaveCommand = new RelayCommand(async () => await Save());
                Title = "Séries";
                Series = new ObservableCollection<SerieViewModel>();
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private void ScanSingleFile()
        {
            try
            {
                string path = _dialogservice.OpenUniqueFileDialog("Comics files (*.cbz;*.cbr)|*.cbz;*.cbr");
                _dialogservice.ShowValidateFile(path);
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }
        private void ScanMultiFile()
        {
            try
            {
                var files = _dialogservice.OpenMultiFileDialog("Comics files (*.cbz;*.cbr)|*.cbz;*.cbr");
                _dialogservice.ShowValidateMultiFile(files);
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }
        private async void ReceiveNotificationMessage(NotificationMessage obj)
        {
            try
            {
                await this.Loaded();
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private byte[] _bckImg;
        public byte[] BckImg
        {
            get { return _bckImg; }
            set { _bckImg = value; RaisePropertyChanged(); }
        }
        private async void LoadingBck()
        {
            try
            {
                BckImg = await _businessBackground.GetRandomBackground();
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }
        private async Task Save()
        {
            try
            {
                foreach (SerieViewModel Svm in Series.Where(x => x.Haschanged == true))
                {
                    await _businessSerie.Update(Svm.Serie);
                }
                _dialogservice.ShowMessageOk("Sauvegarde finie", "OK");
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private async void ChangeParent(SerieViewModel SelectedItem)
        {
            try
            {
                if (Series.Any(x => x.Haschanged == true))
                {
                    _dialogservice.ShowMessageOk("Veuillez effectuez une sauvegarde avant", "OK");
                }
                else
                {
                    if (Series.Count(x => x.IsChecked) > 1)
                        _dialogservice.ShowMessageOk("", "Veuillez choisir un seul élément");
                    else
                    {
                        var newParent = _dialogservice.ShowChooseParent(SelectedItem);
                        if (newParent != null)
                        {
                            var selectedSerie = Series[SelectedIndex];
                            selectedSerie.Serie.ParentID = newParent.Serie.ID;
                            await Loaded();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private async void ChangeImage(SerieViewModel SelectedItem)
        {
            //if (Series.Count(x => x.IsChecked) > 1)
            //    _dialogservice.ShowMessageOk("", "Veuillez choisir un seul élément");
            //else
            //{
            //    _navigationService.NavigateTo("CreateSerie", SelectedItem.Serie);
            //    await Loaded();
            //}
            try
            {
                _dialogservice.ShowCreateOrUpdateSerieModal(SelectedItem);
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private void CreateSerie()
        {
            try
            {
                _dialogservice.ShowCreateOrUpdateSerieModal(null);
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private async Task Loaded()
        {
            try
            {
                Series.Clear();
                var series = await _businessSerie.GetAll();
                series = series.Where(x => x.ParentID == null);
                foreach (var serie in series.OrderBy(x => x.ID))
                {
                    var dsVM = new SerieViewModel();
                    await dsVM.Initialize(serie);
                    Series.Add(dsVM);
                }
            }
            catch (Exception ex)
            {
                _dialogservice.ShowMessageOk("Erreur", ex.Message);
            }
        }

        private void GoBack()
        {
            _navigationService.NavigateTo("Splash",null);
        }
    }
}
