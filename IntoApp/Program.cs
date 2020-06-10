using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using IntoApp.utils;

namespace IntoApp
{
    public class Program
    {
        static Program()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
        }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            #region 程序启动检查更新

            //string CurrentVersion = ConfigurationManager.AppSettings["CurrentVersion"];//当前程序的version版本
            //string AppId = ConfigurationManager.AppSettings["AppId"];//当前程序的AppId，唯一标示
            //string str= "{\"CurrentVersion\":\"" + CurrentVersion + "\",\"AppId\":\"" + AppId + "\"}";
            //Process ps=new Process();
            //ProcessStartInfo psi=new ProcessStartInfo();
            //psi.Arguments = str.Replace(" ", "").Replace("\"", "*");
            //psi.UseShellExecute = false;
            //psi.FileName = "IntoApp.AutoUpdate.exe";
            //psi.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "Update";
            //psi.RedirectStandardError = true;
            //ProcessHelper.OpenAdminProcess(psi,ps,"自动更新打开失败！您可在应用程序内手动更新");

            #endregion
            App.Main();//启动WPF项目
        }

        //解析程序集失败，会加载对应的程序集
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";
            //判断程序集的区域性
            if (!assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture))
            {
                path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
    }
}
