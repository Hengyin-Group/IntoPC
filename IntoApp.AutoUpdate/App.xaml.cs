using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace IntoApp.AutoUpdate
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;

        public App()
        {
            this.Startup+=new StartupEventHandler(App_Starup);
        }

        void App_Starup(object sender,StartupEventArgs e)
        {
            bool ret;
            mutex=new Mutex(true, "IntoApp.AutoUpdate",out ret);
            if (!ret)
            {
                Environment.Exit(0);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CheckAdministrator();
            DispatcherHelper.Initialize();
            //不是管理员退出 以管理员身份登录
            StartupUri = new Uri("MainWindow.xaml",UriKind.RelativeOrAbsolute);
        }
        /// 检查是否是管理员身份
        /// </summary>
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

                    if (MessageBox.Show("应用程序奔溃了", "提示", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK) == MessageBoxResult.OK)
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
