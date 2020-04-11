using BDAdmin.Navigation;
using Business;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel
{
    public class SeriesViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;
        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand CreatePage { get; private set; }

        private ObservableCollection<DisplayingSerieViewModel> _series;
        public RelayCommand GoBackPage { get; private set; }
        public string Title { get; set; }
        public ObservableCollection<DisplayingSerieViewModel> Series { get => _series; set => _series = value; }

        public SeriesViewModel(IFrameNavigationService navigationService, IBusinessFichier businessFichier, IBusinessSerie businessSerie)
        {
            _navigationService = navigationService;
            _businessFichier = businessFichier;
            _businessSerie = businessSerie;
            GoBackPage = new RelayCommand(() => GoBack());
            LoadedCommand = new RelayCommand(async()=> await Loaded());
            CreatePage = new RelayCommand(() => CreateSerie());
            Title = "Séries";
            Series = new ObservableCollection<DisplayingSerieViewModel>();
        }

        private void CreateSerie()
        {
            _navigationService.NavigateTo("CreateSerie", null);
        }

        private async Task Loaded()
        {
            Series.Clear();
            var series = await _businessSerie.GetAll();
            foreach(var serie in series)
            {
                var dsVM = new DisplayingSerieViewModel(_businessFichier, _businessSerie);
                await dsVM.Initialize(serie);
                Series.Add(dsVM);
            }
        }

        private void GoBack()
        {
            _navigationService.NavigateTo("Home",null);
        }
    }
}
