using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Common.Enum;
using IntoApp.Common.Helper;
using IntoApp.View;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.utils
{
    public class UpdateHelper
    {

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);

        /// <summary>
        /// 检查更新
        /// </summary>
        public static void CheckUpdateStatus()
        {
            string CurrentVersion = ConfigurationManager.AppSettings["CurrentVersion"];//当前程序的version版本
            string AppId = ConfigurationManager.AppSettings["AppId"];//当前程序的AppId，唯一标示
            Bll.AppVersion appVersion=new AppVersion();
            string tempVal= appVersion.GetAppVersion();
            //MessageBox.Show(str);
            JObject jo = (JObject) JsonConvert.DeserializeObject(tempVal);
            if (JObjectHelper.GetStrNum(jo["code"].ToString())==200)  //请求成功
            {
                int[] _currentVersion = Array.ConvertAll<string,int>(CurrentVersion.Split('.'), int.Parse);
                int[] _updateVersion =Array.ConvertAll<string,int>(jo["dataList"]["VersionNo"].ToString().Split('.'),int.Parse);
                int len = _currentVersion.Length >= _updateVersion.Length
                    ? _updateVersion.Length
                    : _currentVersion.Length;
                //判断是否需要更新
                bool bo = false;
                for (int i = 0; i < len; i++)
                {
                    if (!bo)
                        bo = _currentVersion[i] < _updateVersion[i];
                    else
                        break;
                }
                //bo=true可以更新，false不用更新
                if (bo)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        AutoUpdateInfo autoUpdate = new AutoUpdateInfo();
                        bool? update_bo = autoUpdate.ShowDialog();
                        if ( update_bo == null || update_bo == false)
                            return;
                        string appDir = Path.Combine(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar)));
                        string updateFileDir = Path.Combine(Path.Combine(appDir.Substring(0, appDir.LastIndexOf(Path.DirectorySeparatorChar))), "temporary");
                        if (!Directory.Exists(updateFileDir))
                        {
                            Directory.CreateDirectory(updateFileDir);
                        }

                        string exePath = Path.Combine(updateFileDir, "Update");

                        if (!Directory.Exists(exePath))
                        {
                            Directory.CreateDirectory(exePath);
                        }
                        //File.Copy(Path.Combine(appDir,"Update"),exePath,true);
                        FileOperationHelper.FileCopy(Path.Combine(appDir, "Update"), exePath, true);
                        //string str= "{\"CurrentVersion\":\"" + CurrentVersion + "\",\"AppId\":\"" + AppId + "\"}";
                        ProcessStartInfo psi = new ProcessStartInfo();
                        Process ps = new Process();
                        psi.FileName = Path.Combine(exePath, "IntoApp.AutoUpdate.exe");
                        psi.Arguments = tempVal.Replace(" ", "").Replace("\"", "*")+" "+Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"IntoApp.exe");
                        psi.UseShellExecute = false;
                        psi.RedirectStandardError = true;
                        //Process.Start(psi);
                        //ProcessHelper.OpenAdminProcess(psi, ps, "自动更新失败,稍后请手动更新！");
                        ShellExecute(IntPtr.Zero, "runas", @Path.Combine(exePath, "IntoApp.AutoUpdate.exe"), tempVal.Replace(" ", "").Replace("\"", "*") + " " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntoApp.exe"), "", 5);
                        Application.Current.Shutdown();
                    });
                }

            }
            else    //请求失败
            {
                
            }
        }

    }
}
