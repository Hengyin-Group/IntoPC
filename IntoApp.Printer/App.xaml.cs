using Lierda.WPFHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using IntoApp.Printer.ViewModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using IntoApp.Printer.Pipe;
using IntoApp.Printer.Utils;
using GalaSoft.MvvmLight.Threading;

namespace IntoApp.Printer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        LierdaCracker cracker = new LierdaCracker();
        public App()
        {
            this.Startup += new StartupEventHandler(App_Starup);
            this.Exit+=new ExitEventHandler(App_Exit);
            //this.InitializeComponent();
        }

        void App_Starup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new Mutex(true, "IntoApp.Printer", out ret);
            if (!ret)
            {
                //MMessageBox.ShouBox("程序正在运行中", "提示", MMessageBox.ButtonType.Yes, MMessageBox.IconType.Info);
                //MessageBox.Show("程序正在运行中", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
            }
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            NotificationHelper.DisposeNotify();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            cracker.Cracker(10);
            
            base.OnStartup(e);

            CheckAdministrator();
            DispatcherHelper.Initialize();
            //不是管理员退出 以管理员身份登录

            //判断系统是否为64位,向配置文件里写入
            //string OSType = Environment.Is64BitOperatingSystem ? "64" : "86";
            //Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //cfa.AppSettings.Settings["ostype"].Value = OSType;
            //cfa.Save();

            #region 双工管道通讯

            // ThreadPool.QueueUserWorkItem((obj) => { PipeHelp.StartConnection(); });

            #endregion


            //注册Application_Error
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            StartupUri = new Uri("AppPrinterWindow.xaml", UriKind.RelativeOrAbsolute);
            //StartupUri = new Uri("TestWindow.xaml", UriKind.RelativeOrAbsolute);
        } 

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //处理完后，我们需要将Handler=true表示已此异常已处理过
            e.Handled = true;
        }



        // 检查是否是管理员身份
        private void CheckAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                // It is not possible to launch a ClickOnce app as administrator directly,
                // so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Start the new process
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    //var reault= MMessageBox.ShouBox("应用程序奔溃了", "提示", MMessageBox.ButtonType.Yes, MMessageBox.IconType.warring);

                    if (MessageBox.Show("应用程序崩溃了", "提示", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK) == MessageBoxResult.OK)
                    {
                        this.Shutdown();
                    }
                    else
                    {
                        this.Shutdown();
                    }
                }

                // Shut down the current process
                Environment.Exit(0);
            }
        }
    }
}
