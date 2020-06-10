using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.utils;
using IntoApp.ViewModel;
using Lierda.WPFHelper;

namespace IntoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        LierdaCracker cracker = new LierdaCracker();
        public App()
        {
            this.Startup+=new StartupEventHandler(App_Starup);
            this.Exit+=new ExitEventHandler(App_Exit);
           //this.InitializeComponent();
        }

        void App_Starup(object sender,StartupEventArgs e)
        {
            bool ret;
            mutex = new Mutex(true, "IntoApp", out ret);
            if (!ret)
            {
                //MMessageBox.ShouBox("程序正在运行中", "提示", MMessageBox.ButtonType.Yes, MMessageBox.IconType.Info);
                MessageBox.Show("程序正在运行中", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            cracker.Cracker(10);
            base.OnStartup(e);
            //初始化
            DispatcherHelper.Initialize();
            this.Dispatcher.Invoke(new Action(delegate
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    UpdateHelper.CheckUpdateStatus();
                });

                #region 判断系统是否为64位,向配置文件里写入 权限会升级

                //string OSType = Environment.Is64BitOperatingSystem ? "64" : "86";
                //Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //cfa.AppSettings.Settings["ostype"].Value = OSType;
                //cfa.Save();

                #region 双工管道通讯
                /*
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    var cc = ViewModelLocator.Pipe;
                });
                */

                #endregion

                #endregion


                //注册Application_Error
                this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
                StartupUri = new Uri("LoginWindow.xaml", UriKind.RelativeOrAbsolute);

            }));
            
        }

        void App_Exit(object sender, ExitEventArgs e)
        {

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //处理完后，我们需要将Handler=true表示已此异常已处理过
            e.Handled = true;
        }
    }
}
