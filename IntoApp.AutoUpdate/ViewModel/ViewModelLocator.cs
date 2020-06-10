/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:IntoApp.AutoUpdate"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using IntoApp.AutoUpdate.utils;
using Microsoft.Practices.ServiceLocation;

namespace IntoApp.AutoUpdate.ViewModel
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
            SimpleIoc.Default.Register<InformUpdateViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<Page_DecompressionViewModel>();
            SimpleIoc.Default.Register<Page_DownloadViewModel>();
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

            SimpleIoc.Default.Register<MainViewModel>();
            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigate>(() => navigationService);
        }

        private INavigate CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("PageDecompression",new Uri("/IntoApp.AutoUpdate;component/View/PageDecompression.xaml",UriKind.Relative));
            navigationService.Configure("PageDownload",new Uri("/IntoApp.AutoUpdate;component/View/PageDownload.xaml",UriKind.Relative));
            //navigationService.Configure("PageTwo4", new Uri("/MvvmLightSample;component/Views/PageFour.xaml", UriKind.Relative));
            return navigationService;
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MainWindowViewModel Win_MainWindowViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); }
        }

        public InformUpdateViewModel Win_InformUpdateViewModel
        {
            get { return ServiceLocator.Current.GetInstance<InformUpdateViewModel>(); }
        }

        public Page_DecompressionViewModel page_DecompressionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<Page_DecompressionViewModel>(); }
        }

        public Page_DownloadViewModel page_DownloadViewModel
        {
            get { return ServiceLocator.Current.GetInstance<Page_DownloadViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}