using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using IntoApp.API;
using IntoApp.Common.Enum;
using IntoApp.ViewModel;
using Newtonsoft.Json.Linq;
using IntoApp.Model;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.IO;
using IntoApp.Common;
using IntoApp.Dal;

namespace IntoApp.utils
{
    public class LoginHelper
    {
        /// <summary>
        /// 子页控制父页才使用
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="obj"></param>
        public static void LoginNavigate(object btn,object obj)
        {
            Button BTN=btn as Button;
            Page page=obj as Page;
            string pageStr = BTN.Tag.ToString();
            if (!string.IsNullOrEmpty(pageStr)&&Enum.IsDefined(typeof(LoginMenu),pageStr))
            {
                var value= (LoginMenu) Enum.Parse(typeof(LoginMenu), pageStr);
                Window window=Window.GetWindow(page);
                LoginWindowViewModel loginVM = new LoginWindowViewModel();
                switch (value)
                {
                    case LoginMenu.Register:
                        loginVM.CurrentPage = LoginPageManager.PageRegister;
                        break;
                    case LoginMenu.Login:
                        loginVM.CurrentPage = LoginPageManager.PageLogin;
                        break;
                    case LoginMenu.ForgetPwd:
                        loginVM.CurrentPage = LoginPageManager.PageForgetPwd;
                        break;
                    case LoginMenu.LoginLoading:
                        loginVM.CurrentPage = LoginPageManager.PageLoginLoading;
                        break;
                }
                window.DataContext = loginVM;
            }
        }

        /// <summary>
        /// 登录返回值给AccountInfo赋值 
        /// </summary>
        /// <param name="Jo"></param>
        /// <returns></returns>
        public static bool LoginCallBack(JObject Jo)
        {
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

            UserInfoSave save=new UserInfoSave();
            save.SaveIcon();
            //IntoApp.Common.FileHelper.GetFileHasStr(Path.GetDirectoryName())

            //删除空格，将\"转为*，避免传值丢失\"的情况
            Info = Jo.ToString().Replace(" ","").Replace("\"","*");
            return true;
        }

        /// <summary>
        /// 储存用户信息
        /// </summary>
        private static string Info;

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);

        public static void AddPrinter()
        {
            #region  低权限调用高权限外部程序

            Process ps = new Process();
            ProcessStartInfo psi=new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.FileName = "IntoApp.Printer.exe";
            psi.Arguments = Info;
            psi.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "Printer";
            psi.RedirectStandardError = true;
            #region 不能成功

            //Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Printer\\IntoApp.Printer.exe", Info);
            //p.StartInfo.FileName = "IntoApp.Printer.exe";
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "Printer";
            ////@必须加上，不然特殊字符会被自动过滤掉 
            //p.StartInfo.Arguments = Info;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.Verb = "RunAs";

            #endregion

            //不适合WindowsXP
            //ProcessHelper.OpenAdminProcess(psi,ps,"印兔打印机未打开");

            ShellExecute(IntPtr.Zero, "runas", @Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Printer", "IntoApp.Printer.exe"), Info, "", 5);

            #endregion
        }


    }
}
