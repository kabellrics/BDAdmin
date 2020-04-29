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
using BDAdmin.Modal.Implémentation;
using BDAdmin.Modal.Interface;
using Business;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using BDAdmin.ViewModel.Model;
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
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IBusinessFichier, BusinessFichier>();
            SimpleIoc.Default.Register<IBusinessFile, BusinessFile>();
            SimpleIoc.Default.Register<IBusinessSerie, BusinessSerie>();
            SimpleIoc.Default.Register<IBusinessPage, BusinessPage>();
            SimpleIoc.Default.Register<IBusinessBackground, BusinessBackground>();
            SimpleIoc.Default.Register<IBusinessComicVine, BusinessComicVine>();

            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<FichierViewModel>();
            SimpleIoc.Default.Register<SerieViewModel>();
            SimpleIoc.Default.Register<WorkingViewModel>();
            SimpleIoc.Default.Register<CreateSerieViewModel>();
            SimpleIoc.Default.Register<SplashViewModel>();
            SetupNavigation();
        }
        private static void SetupNavigation()
        {
            FrameNavigationService bHNavigationService = new FrameNavigationService();
            bHNavigationService.Configure("Main", new Uri("../MainWindow.xaml", UriKind.Relative));
            bHNavigationService.Configure("Home", new Uri("../HomePage.xaml", UriKind.Relative));
            bHNavigationService.Configure("Working", new Uri("../Views/WorkingView.xaml", UriKind.Relative));
            bHNavigationService.Configure("Splash", new Uri("../Views/SplashView.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => bHNavigationService);
        }
        public static IFrameNavigationService NavigationService()
        {
            return SimpleIoc.Default.GetInstance<IFrameNavigationService>();
        }
        public static IBusinessSerie BusinessSerie()
        {
            return SimpleIoc.Default.GetInstance<IBusinessSerie>();
        }
        public static IBusinessFichier BusinessFichier()
        {
            return SimpleIoc.Default.GetInstance<IBusinessFichier>();
        }
        public static IDialogService DialogService()
        {
            return SimpleIoc.Default.GetInstance<IDialogService>();
        }
        public static IBusinessFile BusinessFile()
        {
            return SimpleIoc.Default.GetInstance<IBusinessFile>();
        }
        public static IBusinessPage BusinessPage()
        {
            return SimpleIoc.Default.GetInstance<IBusinessPage>();
        }
        public static IBusinessComicVine BusinessComicVine()
        {
            return SimpleIoc.Default.GetInstance<IBusinessComicVine>();
        }
        //public static ViewModelLocator Instance
        //{
        //    get
        //    {
        //        if(_instance == null)
        //        {
        //            _instance = new ViewModelLocator();
        //        }
        //        return _instance;
        //    }
        //}
        //public static ViewModelLocator _instance = null;
        public CreateSerieViewModel CreateSerie
        {
            get
            {
                CreateSerieViewModel vm = ServiceLocator.Current.GetInstance<CreateSerieViewModel>();
                SimpleIoc.Default.Unregister<CreateSerieViewModel>();
                SimpleIoc.Default.Register<CreateSerieViewModel>();
                return vm;
            }
        }
        public MenuViewModel Menu
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MenuViewModel>();
            }
        }
        public SplashViewModel Splash
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SplashViewModel>();
            }
        }
        public WorkingViewModel Working
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WorkingViewModel>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}