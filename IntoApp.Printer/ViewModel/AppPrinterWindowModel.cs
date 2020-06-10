using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using IntoApp.Printer.View;
using IntoApp.Printer.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;

namespace IntoApp.Printer.ViewModel
{
    public class AppPrinterWindowModel : ViewModelBase
    {

        #region 调用非托管代码


        //导入DLL函数
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void printCallBackFun(int callbackType, int code, [MarshalAs(UnmanagedType.LPStr)]string msg1, [MarshalAs(UnmanagedType.LPStr)]string msg2);

        /// <summary>
        /// 添加打印机
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <param name="driverName">新增驱动名称，如果需要在系统打印机列表中不折叠，需要传入不同的驱动名称。</param>
        /// <returns>成功返回0</returns>
        [DllImport(@"VPrinter.dll", EntryPoint = "addVPrinter", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        extern static int addVPrinter([MarshalAs(UnmanagedType.LPStr)]string printerName, [MarshalAs(UnmanagedType.LPStr)]string driverName);

        /// <summary>
        /// 删除打印机
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <returns>成功返回0</returns>
        [DllImport(@"VPrinter.dll", EntryPoint = "removeVPrinter", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        extern static int removeVPrinter([MarshalAs(UnmanagedType.LPStr)]string printerName);

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="callback">回调函数，参考C# demo</param>
        /// <param name="printerName">打印机名称</param>
        /// <returns>成功返回0</returns>

        [DllImport(@"VPrinter.dll", EntryPoint = "start", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        extern static int start(printCallBackFun callback, [MarshalAs(UnmanagedType.LPStr)]string printerName);

        /// <summary>
        /// 打印机清单，只显示添加的。
        /// </summary>
        /// <returns></returns>
        [DllImport(@"VPrinter.dll", EntryPoint = "listAllVPrinter", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        extern static System.IntPtr listAllVPrinter();


        #endregion

        #region 添加打印机回调

        //从回调MSG中解析用户设置的打印机参数,返回一个字典对象,字典key为设置名称,value为用户设置的值.
        public Dictionary<string, string> getPrintSettings(string msg)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] tmpArr = msg.Split('\n');
            foreach (string settingStr in tmpArr)
            {
                if (settingStr.Trim() == "") continue;
                string[] tmpArr2 = settingStr.Split(":".ToArray(), 2);
                dic.Add(tmpArr2[0], tmpArr2[1]);
            }
            return dic;
        }

        public void CsharpCall(int callbackType, int code, [MarshalAs(UnmanagedType.LPStr)]string msg1, [MarshalAs(UnmanagedType.LPStr)]string msg2)
        {
            Dictionary<string, string> settings = getPrintSettings(msg2);

            //打印回调返回的参数：
            //1.savedPath：虚拟打印生成的pdf文档路径
            //2.drvierName：虚拟打印机驱动名称
            //3.color: 是否为彩色打印  0：黑白;1:彩色；
            //4.copies： 份数；
            //5.duplex: 单双面； 由于是虚拟打印，单双面的参数是无效的； XXXXXXXXXX
            //6.pageCount:文档总页数
            //7.document:源文件名称

            if (callbackType == 1 && code == 1)
            {
                #region 用户上传文件

                string FilePath = "";
                Task.Factory.StartNew(() =>
                {
                    var fileName = settings["document"];
                    var isColorNum = settings["color"];
                    var copiesNum = settings["copies"];
                    var pageCount1 = settings["pageCount"];

                    ModifyNameWindow modify_Window = new ModifyNameWindow(settings["savedPath"], copiesNum, isColorNum, Path.GetFileNameWithoutExtension(fileName),pageCount1);
                    //modify_Window.TransfEvent += frm_TransfEvent;
                    modify_Window.TransfEvent_New += frm_TransfEvent_New;
                    bool bo = (bool)modify_Window.ShowDialog();
                    //void frm_TransfEvent(string value)
                    //{
                    //    FilePath = value;
                    //}

                    void frm_TransfEvent_New(string filePath, string copies, string isColor, string documentName,string pageCount)
                    {
                        FilePath = filePath;
                    }


                    if (bo)
                    {

                    }
                });

                #endregion
            }
        }

        #endregion

        #region  页面初始化
        public AppPrinterWindowModel()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                MonitorMainAPP();
            });

        }
        #endregion

        #region 方法

        DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 监听主程序
        /// </summary>
        void MonitorMainAPP()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            string ProcName = "IntoApp";
            string tempName = "";
            foreach (Process thispro in Process.GetProcesses())
            {
                tempName = thispro.ProcessName;
                if (tempName == ProcName)
                {
                    return;
                }
            }
            timer.Stop();
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }


        #endregion

        #region 页面显示内容

        private string _lab = "正在添加打印机";

        public string Lab
        {
            get { return _lab; }
            set
            {
                _lab = value;
                RaisePropertyChanged("Lab");
            }
        }

        #endregion

        #region 事件

        public MyCommand LoadedCommand
        {
            get
            {
                return new MyCommand(x => Load());
            }
        }

        public void Load()
        {
            //MessageBox.Show("加载完成");
        }

        #endregion
    }
}
