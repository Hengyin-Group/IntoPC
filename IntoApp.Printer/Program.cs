using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using IntoApp.Dal;
using IntoApp.Printer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.Printer
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
                JObject Jo = (JObject)JsonConvert.DeserializeObject(JsonStr);
                AccountInfo.Phone = Jo["dataList"]["phone"].ToString();
                AccountInfo.IconImage = Jo["dataList"]["icon"].ToString();
                AccountInfo.NickName = Jo["dataList"]["nickname"].ToString();
                AccountInfo.DepID = Jo["dataList"]["depid"].ToString();
                AccountInfo.PosID = Jo["dataList"]["posid"].ToString();
                AccountInfo.Expertise = Jo["dataList"]["expertise"].ToString();
                AccountInfo.JobState = Jo["dataList"]["jobstate"].ToString();
                AccountInfo.Token = Jo["dataList"]["token"].ToString();
                AccountInfo.ID = Jo["dataList"]["id"].ToString();
                AccountInfo.Marital = Jo["dataList"]["marital"].ToString();
                RequestAddress.server = Jo["dataList"]["enterpriseIp"].ToString();

                App.Main();//启动WPF项目
            }
            //else
            //{
            //AccountInfo.Token =
            //"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VycGhvbmUiOiIxMzEzNzk5MTc5NSIsInVzZXJwd2QiOiIyNS1GOS1FNy05NC0zMi0zQi00NS0zOC04NS1GNS0xOC0xRi0xQi02Mi00RC0wQiIsInVzZXJpZCI6IjIwMTgxMDA4MTgxNjU3NTE3Nzk2NjAxZDU4NWRjYjkifQ.ixMZ06FNUou7V0wiPzF2n9yV437BqA8QLO71eAgkdZQ";
            //AccountInfo.ID = "20181008181657517796601d585dcb9";
            //AccountInfo.Phone = "13137991795";
            //App.Main();
            //}

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
