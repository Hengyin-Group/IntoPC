using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace IntoApp.utils
{
    public class ProcessHelper
    {
        #region 打开应用程序，如已打开获取窗口句柄

        /// <summary>
        /// 外部程序信息
        /// </summary>
        [Serializable]
        public class ProcessItem
        {
            //程序名
            public string Name { get; set; }
            //程序的绝对路径，若程序已在注册表中注册，则可省略路径，如：FullPath=notepad 可打开记事本。
            public string FullPath { get; set; }
            //任务管理器中的进程名
            public string ProcessName { get; set; }

        }

        public Process CheckProcess(ProcessItem pItem)
        {
            var processName = System.IO.Path.GetFileName(pItem?.FullPath ?? "").ToLower();
            var psList = Process.GetProcesses();
            var nameNoExt = System.IO.Path.GetFileNameWithoutExtension(processName).ToLower();
            var cpn = pItem.ProcessName?.ToLower();
            var cpnNoExt = System.IO.Path.GetFileNameWithoutExtension(cpn ?? "");
            foreach (var process in psList)
            {
                var pn = process.ProcessName.ToLower();
                if (pn == processName || pn == nameNoExt || pn == cpn || pn == cpnNoExt)
                {
                    return process;
                }
            }
            return null;
        }

        public void StartProcess(ProcessItem pItem)
        {
            try
            {
                var processName = System.IO.Path.GetFileName(pItem?.FullPath ?? "").ToLower();
                if (string.IsNullOrEmpty(processName))
                {
                    //指定的程序路径配置无效;
                }
                else
                {
                    Process ps = CheckProcess(pItem);

                    if (ps == null)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo();
                        //打开相应的软件
                        psi.UseShellExecute = false;
                        psi.FileName = pItem.FullPath;
                        psi.CreateNoWindow = true;
                        psi.RedirectStandardError = true;
                        psi.WorkingDirectory = System.IO.Path.GetDirectoryName(pItem.FullPath);

                        bool tryWithAdmin = false;
                        try
                        {
                            ps = Process.Start(psi);
                        }
                        catch (Exception ex)
                        {
                            //打开失败,尝试用管理员权限打开
                            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                            if (!isAdmin)
                                tryWithAdmin = true;
                        }
                        if (tryWithAdmin)
                        {
                            //必须将UseShellExecute设置为true,将Verb设置为"runas"来用管理员权限启动.
                            //UseShellExecute时不能重定向IO流,所以将RedirectStandardError设置为false
                            psi.UseShellExecute = true;
                            psi.RedirectStandardError = false;
                            psi.Verb = "runas";
                            ps = Process.Start(psi);
                        }
                    }
                    else
                    {
                        ////将指定ps移至前台
                        //ShowWindow(ps.MainWindowHandle, SW_RESTORE); //将窗口还原，如果不用此方法，缩小的窗口不能激活
                        //bool res = SetForegroundWindow(ps.MainWindowHandle);
                        //if (!res)
                        //{
                        //    //无法切换到;
                        //}

                        MessageBox.Show("程序已打开");
                    }
                }
            }
            catch (Exception ex)
            {
                //启动失败
            }
        }

        #region 窗口操作

        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //根据任务栏应用程序显示的名称找相应窗口的句柄
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        private const int SW_RESTORE = 9;

        #endregion

        #endregion

        #region 低权限调用高权限外部应用程序
        /// <summary>
        /// 需要提前定义好ProcessStartInfo，Process，errorStr
        /// </summary>
        /// <param name="psi">启动信息 </param>
        /// <param name="ps"></param>
        /// <param name="errorStr"></param>
        public static void OpenAdminProcess(ProcessStartInfo psi,Process ps,string errorStr)
        {
            bool tryWithAdmin = false;
            try
            {
                ps = Process.Start(psi);
            }
            catch (Exception e)
            {
                bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                if (!isAdmin)
                    tryWithAdmin = true;
            }

            if (tryWithAdmin)
            {
                psi.UseShellExecute = true;
                psi.RedirectStandardError = false;
                psi.Verb = "runas";
                try
                {
                    ps = Process.Start(psi);
                }
                catch (Exception e)
                {
                    MessageBox.Show(errorStr);
                }
            }
        }

        #endregion
    }
}
