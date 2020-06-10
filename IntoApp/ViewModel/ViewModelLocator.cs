/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:IntoApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using IntoApp.utils;
using IntoApp.ViewModel.ContentViewModel;
using Microsoft.Practices.ServiceLocation;
using System;
using IntoApp.View.Content.LeftMenu;
using IntoApp.View.Content.Setting;
using IntoApp.ViewModel.ContentViewModel.ServerViewModel;
using IntoApp.ViewModel.ContentViewModel.ServerViewModel.OrderListViewModel;
using IntoApp.ViewModel.Other;
using PageServerViewModel = IntoApp.ViewModel.ContentViewModel.PageServerViewModel;

namespace IntoApp.ViewModel
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
            SimpleIoc.Default.Register<PageLoginViewModel>();
            SimpleIoc.Default.Register<PageForgetPwdViewModel>();
            SimpleIoc.Default.Register<PageRegisterViewModel>();

            SimpleIoc.Default.Register<LoginWindowViewModel>();
            //SimpleIoc.Default.Register<ContentWindowViewModel>();
            SimpleIoc.Default.Register<WinFilePreviewViewModel>();

            SimpleIoc.Default.Register<PageMineViewModel>();
            //SimpleIoc.Default.Register<PageServerViewModel>();
            SimpleIoc.Default.Register<PageWorkBenchViewModel>();
            SimpleIoc.Default.Register<InformUpdateViewModel>();

            SimpleIoc.Default.Register<WinFileUploadViewModel>();
            SimpleIoc.Default.Register<PageTrayViewModel>();
            SimpleIoc.Default.Register<PageTrayHistoryViewModel>();
            //SimpleIoc.Default.Register<PageTrayHistoryUploadedViewModel>();
            //SimpleIoc.Default.Register<PageTrayHistoryDownloadedViewModel>();
            SimpleIoc.Default.Register<PageTrayHistoryUnDownloadViewModel>();
            SimpleIoc.Default.Register<WinImageClippingViewModel>();


            #region  OrderList
            SimpleIoc.Default.Register<PageOrderListViewModel>();
            SimpleIoc.Default.Register<PageOrderListAllViewModel>();
            SimpleIoc.Default.Register<PageOrderListCompleteViewModel>();
            SimpleIoc.Default.Register<PageOrderListUnfinishedViewModel>();

            #endregion

            SimpleIoc.Default.Register<WinNotifiyViewModel>();
            
            #region 

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

            #endregion

            SimpleIoc.Default.Register<MainViewModel>();
            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigate>(() => navigationService);

        }
        /// <summary>
        /// 导航服务器
        /// </summary>
        //public INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
        private INavigate CreateNavigationService()
        {
            var navigationService = new NavigationService();

            #region Login

            navigationService.Configure("PageLogin", new Uri("/IntoApp;component/View/Login/PageLogin.xaml", UriKind.Relative));
            navigationService.Configure("PageRegister", new Uri("/IntoApp;component/View/Login/PageRegister.xaml", UriKind.Relative));
            navigationService.Configure("PageForgetPwd", new Uri("/IntoApp;component/View/Login/PageForgetPwd.xaml", UriKind.Relative));

            #endregion

            #region LeftMenu

            navigationService.Configure("PageContacts",new Uri("/IntoApp;component/View/Content/LeftMenu/PageContacts.xaml",UriKind.Relative));
            navigationService.Configure("PageMessage",new Uri("/IntoApp;component/View/Content/LeftMenu/PageMessage.xaml",UriKind.Relative));
            navigationService.Configure("PageMine",new Uri("/IntoApp;component/View/Content/LeftMenu/PageMine.xaml",UriKind.Relative));
            navigationService.Configure("PageServer",new Uri("/IntoApp;component/View/Content/LeftMenu/PageServer.xaml",UriKind.Relative));
            navigationService.Configure("PageWorkBench",new Uri("/IntoApp;component/View/Content/LeftMenu/PageWorkBench.xaml",UriKind.Relative));

            #endregion

            #region Server

            navigationService.Configure("PageOrderList", new Uri("/IntoApp;component/View/Server/PageOrderList.xaml", UriKind.Relative));
            navigationService.Configure("PageTray", new Uri("/IntoApp;component/View/Server/PageTray.xaml", UriKind.Relative));
            navigationService.Configure("PageTrayHistory", new Uri("/IntoApp;component/View/Server/PageTrayHistory.xaml", UriKind.Relative));

            #region OrderList

            navigationService.Configure("OrderListAll",new Uri("/IntoApp;component/View/Server/OrderList/PageOrderListAll.xaml",UriKind.Relative));
            navigationService.Configure("OrderListComplete",new Uri("/IntoApp;component/View/Server/OrderList/PageOrderListComplete.xaml",UriKind.Relative));
            navigationService.Configure("OrderListUnfinished",new Uri("/IntoApp;component/View/Server/OrderList/PageOrderListUnfinished.xaml",UriKind.Relative));

            #endregion

            #region TrayHistory
            navigationService.Configure("TrayHistory",new Uri("/IntoApp;component/View/Server/PageTrayHistory.xaml",UriKind.Relative));

            navigationService.Configure("TrayUploaded",new Uri("/IntoApp;component/View/Server/TrayHistory/PageTrayUploaded.xaml",UriKind.Relative));
            navigationService.Configure("TrayDownloaded",new Uri("/IntoApp;component/View/Server/TrayHistory/PageTrayDownloaded.xaml",UriKind.Relative));
            navigationService.Configure("TrayUnDownload",new Uri("/IntoApp;component/View/Server/TrayHistory/PageTrayUnDownload.xaml",UriKind.Relative));

            #endregion

            #region other

            

            #endregion


            #endregion

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

        public LoginWindowViewModel Win_login
        {
            get { return ServiceLocator.Current.GetInstance<LoginWindowViewModel>(); }
        }

        //public ContentWindowViewModel Win_content
        //{
        //    get { return ServiceLocator.Current.GetInstance<ContentWindowViewModel>(); }
        //}

        public InformUpdateViewModel Win_InformUpdate
        {
            get { return ServiceLocator.Current.GetInstance<InformUpdateViewModel>(); }
        }

        #region other

        public WinNotifiyViewModel Win_NotifyViewModel
        {
            get { return ServiceLocator.Current.GetInstance<WinNotifiyViewModel>(); }
        }

        #endregion

        #region Login
        public PageLoginViewModel page_Login
        {
            get { return ServiceLocator.Current.GetInstance<PageLoginViewModel>(); }
        }

        public PageForgetPwdViewModel page_ForgetPwd
        {
            get { return ServiceLocator.Current.GetInstance<PageForgetPwdViewModel>(); }
        }

        public PageRegisterViewModel page_Register
        {
            get { return ServiceLocator.Current.GetInstance<PageRegisterViewModel>(); }
        }

        #endregion

        #region Content

        public WinImageClippingViewModel Win_ImageClippingVM
        {
            get { return ServiceLocator.Current.GetInstance<WinImageClippingViewModel>(); }
        }

        #endregion

        #region leftMenu 

        public PageMineViewModel page_Mine
        {
            get { return ServiceLocator.Current.GetInstance<PageMineViewModel>(); }
        }

        //public static PageServerViewModel page_Server
        //{
        //    get { return ServiceLocator.Current.GetInstance<PageServerViewModel>(); }
        //}

        public static PageServerViewModel page_Server
        {
            get { return PageServerViewModel.GetInstance(); }
        }

        public PageWorkBenchViewModel page_WorkBench
        {
            get { return ServiceLocator.Current.GetInstance<PageWorkBenchViewModel>(); }
        }


        #endregion

        #region Server

        #region OrderList

        public PageOrderListViewModel page_OrderList
        {
            get { return ServiceLocator.Current.GetInstance<PageOrderListViewModel>(); }
        }

        public PageOrderListAllViewModel page_OrderListAll
        {
            get { return ServiceLocator.Current.GetInstance<PageOrderListAllViewModel>(); }
        }

        public PageOrderListCompleteViewModel page_OrderListComplete
        {
            get { return ServiceLocator.Current.GetInstance<PageOrderListCompleteViewModel>(); }
        }

        public PageOrderListUnfinishedViewModel page_OrderListUnfinished
        {
            get { return ServiceLocator.Current.GetInstance<PageOrderListUnfinishedViewModel>(); }
        }

        #endregion

        #region Tray

        public PageTrayViewModel page_Tray
        {
            get { return ServiceLocator.Current.GetInstance<PageTrayViewModel>(); }
        }

        public PageTrayHistoryViewModel page_TrayHistory
        {
            get { return ServiceLocator.Current.GetInstance<PageTrayHistoryViewModel>(); }
        }

        

        public PageTrayHistoryUnDownloadViewModel page_TrayHistoryUnDownload
        {
            get { return ServiceLocator.Current.GetInstance<PageTrayHistoryUnDownloadViewModel>(); }
        }

        #endregion

        public WinFileUploadViewModel Win_FileUpload
        {
            get { return ServiceLocator.Current.GetInstance<WinFileUploadViewModel>(); }
        }

        public WinFilePreviewViewModel Win_FilePreview
        {
            get { return ServiceLocator.Current.GetInstance<WinFilePreviewViewModel>(); }
        }

        #endregion

        #region 单例模式
        public static PageTrayHistoryUploadedViewModel page_TrayhistoryUploaded
        {
            //get { return ServiceLocator.Current.GetInstance<PageTrayHistoryUploadedViewModel>(); }
            get { return PageTrayHistoryUploadedViewModel.GetInstance(); }
        }

        public static PageTrayHistoryDownloadedViewModel page_TrayHistoryDownloaded
        {
            //get { return ServiceLocator.Current.GetInstance<PageTrayHistoryDownloadedViewModel>(); }
            get { return PageTrayHistoryDownloadedViewModel.GetInstance(); }
        }
        public static ContentWindow Win_Content
        {
            get { return ContentWindow.GetInstance(); }
        }

        public static ContentWindowViewModel Win_ContentVM
        {
            get { return ContentWindowViewModel.GetInstance(); }
        }

        public static NotificationHelper Notification
        {
            get { return NotificationHelper.GetInstance();}
        }

        public static WinAboutMe Win_AboutMe
        {
            get { return WinAboutMe.GetInstance;}
        }

        public static WinVersionInfo Win_VersionInfo
        {
            get { return WinVersionInfo.GetInstance;}
        }

        //public static WinImageClippingViewModel Win_ImageClippingVM
        //{
        //    get { return WinImageClippingViewModel.GetInstance;}
        //}

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}