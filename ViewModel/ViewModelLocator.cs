/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BDAdmin"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using BDAdmin.Navigation;
using Business;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
//using Microsoft.Practices.ServiceLocation;

namespace BDAdmin.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            SimpleIoc.Default.Register<IBusinessFichier, BusinessFichier>();
            SimpleIoc.Default.Register<IBusinessFile, BusinessFile>();
            SimpleIoc.Default.Register<IBusinessSerie, BusinessSerie>();

            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<DisplayingFichierViewModel>();
            SimpleIoc.Default.Register<DisplayingSerieViewModel>();
            SimpleIoc.Default.Register<FichierViewModel>();
            SimpleIoc.Default.Register<SeriesViewModel>();
            SimpleIoc.Default.Register<CreateSerieViewModel>();
            SetupNavigation();
        }
        private static void SetupNavigation()
        {
            FrameNavigationService bHNavigationService = new FrameNavigationService();
            bHNavigationService.Configure("Fichier", new Uri("../FichierPage.xaml", UriKind.Relative));
            bHNavigationService.Configure("Main", new Uri("../MainWindow.xaml", UriKind.Relative));
            bHNavigationService.Configure("Home", new Uri("../HomePage.xaml", UriKind.Relative));
            bHNavigationService.Configure("Serie", new Uri("../SeriePage.xaml", UriKind.Relative));
            bHNavigationService.Configure("CreateSerie", new Uri("../CreateSeriePage.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => bHNavigationService);
        }
        public CreateSerieViewModel CreateSerie
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateSerieViewModel>();
            }
        }
        public MenuViewModel Menu
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MenuViewModel>();
            }
        }
        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }
        public FichierViewModel Fichiers
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FichierViewModel>();
            }
        }
        public SeriesViewModel Series
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SeriesViewModel>();
            }
        }

        public DisplayingFichierViewModel DisplayingFichier
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DisplayingFichierViewModel>();
            }
        }

        public DisplayingSerieViewModel DisplayingSerie
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DisplayingSerieViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}