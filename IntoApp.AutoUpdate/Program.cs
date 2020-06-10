using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using IntoApp.AutoUpdate.Model;
using IntoApp.AutoUpdate.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.AutoUpdate
{
    public class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
        }
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string JsonStr = args[0].Replace("*", "\"");
                UpdateModel.IntoAppPath = args[1];
                UpdateModel.UnpackPath = Path.GetDirectoryName(args[1]);
                JObject jo = (JObject)JsonConvert.DeserializeObject(JsonStr);
                UpdateModel.LocalFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");
                UpdateModel.UpdateFileUrl = jo["dataList"]["DownLoadUrl"].ToString();
                UpdateModel.FileName = Path.GetFileName(UpdateModel.UpdateFileUrl);
                App.Main();//启动WPF项目
            }
            //UpdateModel.IntoAppPath =
            //    Path.Combine("E:\\IntoApp2\\IntoApp\\IntoApp\\IntoApp\\bin\\Debug\\IntoApp.exe");
            //UpdateModel.LocalFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");
            //UpdateModel.UpdateFileUrl = "http://into.wainwar.com/upload/印兔打印.zip";
            //UpdateModel.UnpackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Unpack");
            //UpdateModel.FileName = Path.GetFileName(UpdateModel.UpdateFileUrl);
            //App.Main();
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
