using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Forms;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Printer.Model;
using IntoApp.Printer.View;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Application = System.Windows.Application;
using MessageBox = MyMessageBox.Controls.MessageBox;
using GalaSoft.MvvmLight.Threading;
//using MessageBox = System.Windows.MessageBox;

namespace IntoApp.Printer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AppPrinterWindow
    {

        public static printCallBackFun PrintCallBack;//接收返回值，防止被GC回收
        public AppPrinterWindow()
        {
            InitializeComponent();
            //托盘图标加载
            //Init_NotifyIcon();
            //托盘图标加载单例
            //var cc = ViewModelLocator.notify;
            //监听打印机
            PrintCallBack = CsharpCall;
            start(PrintCallBack, "");
            Label.Content = "正在添加打印机";
            LoadWindow(true);//窗体动画开始
        }

        #region  窗口加载,关闭的方式
        DispatcherTimer timer = new DispatcherTimer();
        public void LoadWindow(Boolean Topmost)
        {
            this.Topmost = Topmost;
            #region 右下角弹窗初始化
            timer.Interval = TimeSpan.FromMilliseconds(400);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);

            /**********右下角垂直向上弹出**********/
            //this.Height是自定义窗口的高度
            //this.Width是自定义窗口的宽度
            //this.Left  获取或设置窗口左边缘相对于桌面的位置。
            //此处左边缘即为显示屏的宽度减去自定义窗口的宽度
            this.Left = SystemParameters.WorkArea.Width - this.Width - 5;
            //this.Top  获取或设置窗口上边缘相对于桌面的位置。
            //此处上边缘即为显示屏的高度，即上边缘位于屏幕底线
            //this.Top=0的时候，自定义窗口位于屏幕顶线
            //this.Top=SystemParameters.WorkArea.Height - this.Height的时候自定义窗口的下边正好与屏幕底线重合
            //this.Top = SystemParameters.WorkArea.Height;//SystemParameters.WorkArea.Height是工作区的高度，不包括任务栏
            this.Top = SystemParameters.PrimaryScreenHeight;//整个屏幕的高度，包括任务栏
            //定义弹出窗口完全弹出时候的上边的位置,若一开始this.Top就等于stopTop，则显示为完全弹出的样子
            //完全弹出的时候下边紧贴这任务栏
            StopTop = SystemParameters.WorkArea.Height - this.Height + 5;
            //StopTop = SystemParameters.PrimaryScreenHeight - this.Height;//完全弹出的时候任务栏会遮挡住一部分
            /**********右下角垂直向上弹出**********/

            /**********右下角水平向左弹出**********/
            //this.Left = SystemParameters.WorkArea.Width;
            //this.Top = SystemParameters.WorkArea.Height - this.Height;
            //StopLeft = SystemParameters.WorkArea.Width - this.Width;
            /**********右下角水平向左弹出**********/
            timer.Start();//启动计时器
            #endregion
            Thread thread = new Thread(AddPrinter);
            thread.Start();

        }


        /**********右下角水平向左弹出**********/

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            /**********右下角垂直向上弹出**********/
            while (true)
            {
                if (this.Top > StopTop)
                {
                    this.Top -= 0.0400;
                }
                else
                {
                    timer.Stop();
                    break;
                }
            }
            /**********右下角垂直向上弹出**********/


            /**********右下角水平向左弹出**********/
            //while (true)
            //{
            //    if (this.Left > StopLeft)
            //    {
            //        this.Left -= 0.00273;
            //    }
            //    else
            //    {
            //        timer.Stop();
            //        break;
            //    }
            //}
            /**********右下角水平向左弹出**********/
        }

        /**********右下角垂直向上弹出**********/
        public double StopTop
        {
            get;
            set;
        }
        /**********右下角垂直向上弹出**********/


        /**********右下角水平向左弹出**********/
        public double StopLeft
        {
            get;
            set;
        }
        /**********右下角水平向左弹出**********/

        public void StartCloseTimer(Boolean Topmost, int timeout)
        {
            this.Topmost = Topmost;
            //this.LabelStr.Content = lab;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(timeout)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            this.Hide();
        }

        #endregion

        #region 调用非托管代码

        //导入DLL函数

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

        #region  添加打印机
        void AddPrinter()
        {
            string name = "印兔打印";
            string driverName = "Into_1"; //+ Guid.NewGuid().ToString();
            int code = addVPrinter(name, driverName);
            if (code == 0)
            {
                PrintCallBack = new printCallBackFun(CsharpCall);
                start(PrintCallBack, name);
                this.Dispatcher.Invoke(new Action(delegate
                {
                    this.Label.Content = "添加打印机成功";
                    StartCloseTimer(true, 3);
                    //PrinterVM.Lab = "添加打印机成功";
                }));

                //成功
            }
            else if (code == 1)
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    this.Label.Content = "添加打印机成功";
                    StartCloseTimer(true, 3);
                    //PrinterVM.Lab = "添加打印机成功";
                }));
            }
            else
            {
                //MMessageBox.ShouBox("打印机未正确添加,请联系维护人员。", "提示", MMessageBox.ButtonType.Yes, MMessageBox.IconType.Info);
                //MessageBox.Show("打印机未正确添加,请联系维护人员。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //PrinterVM.Lab = "打印机未正确添加,请联系维护人员";
                this.Label.Content = "打印机未正确添加,请联系维护人员";
                StartCloseTimer(true, 3);
            }
        }
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
            try
            {
                Dictionary<string, string> settings = getPrintSettings(msg2);
                if (callbackType == 1 && code == 1)
                {
                    #region 用户上传文件
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        string FilePath = "";
                        string copiesNum = "";
                        string color = "";
                        string documentName = "";
                        string pageCount1 = "";
                        //string newFileName = reName(settings["savedPath"]);
                        string newFileName = reName(settings["savedPath"], settings["document"]);
                        if (string.IsNullOrEmpty(newFileName))
                        {
                            return;
                        }
                        /*
                         * 这里用了委托
                         */
                        List<string> filePath = new List<string>();
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            //ModifyNameWindow modify_Window = new ModifyNameWindow(newFileName);
                            ModifyNameWindow modify_Window = new ModifyNameWindow(newFileName,
                                settings["copies"].ToString(), settings["color"]
                                , settings["document"], settings["pageCount"]);
                            //modify_Window.TransfEvent_New += frm_TransfEvent;
                            modify_Window.TransfEvent_New += frm_TransfEvent;
                            bool bo = (bool)modify_Window.ShowDialog();

                            //string filePath, string copies, string isColor, string documentName
                            //void frm_TransfEvent(string value)
                            void frm_TransfEvent(string filePath1, string copies, string isColor, string fileName, string pageCount)
                            {
                                FilePath = filePath1;
                                copiesNum = copies;
                                color = isColor;
                                documentName = fileName;
                                pageCount1 = pageCount;
                            }
                            if (bo)
                            {
                                string iscolor = "0";
                                if (settings["color"] == "1")
                                    iscolor = "true";
                                else if (settings["color"] == "0")
                                    iscolor = "false";
                                #region // Old Pram
                                //NameValueCollection data = new NameValueCollection
                                //{
                                //   { "iscolor", iscolor },
                                //   { "copies", settings["copies"] },
                                //   { "endpage",settings["pageCount"]},
                                //   { "userid", AccountInfo.ID },
                                //   { "printType", "1" }
                                //}; 
                                #endregion
                                NameValueCollection dataUpload = new NameValueCollection
                                {
                                   { "sourceclient","PC"}
                                };
                                var orderList = new OrderList();
                                var account = new Account();
                                var myDocument = new Bll.MyDocument();
                                string msg = myDocument.DocTransCoding(AccountInfo.Token, new[] { FilePath }, dataUpload);
                                //string msg = myDocument.BusinessVirtualPrint(AccountInfo.Token, new[] { FilePath, FilePath }, data);
                                JObject jo = (JObject)JsonConvert.DeserializeObject(msg);
                                if (jo["code"].GetInt() == 200)
                                {
                                    string getPrintPrice = account.GetPrintPrice(AccountInfo.Token);
                                    JObject JoPrice = (JObject)JsonConvert.DeserializeObject(getPrintPrice);
                                    if (JoPrice["code"].GetInt() == 200)
                                    {
                                        /*
                                         * 金额计算
                                         */

                                        double getpayMoney = settings["color"].GetBool()
                                            ? Convert.ToDouble(JoPrice["dataList"]["enterpriseColorPrice"]) : Convert.ToDouble(JoPrice["dataList"]["enterpriseBlackPrice"]);
                                        //double payMoney = jo["dataList"]["copies"].GetInt() * jo["dataList"]["totalPage"].GetInt() * getpayMoney;
                                        double payMoney = settings["copies"].GetInt() * jo["dataList"]["totalPage"].GetInt() * getpayMoney;

                                        //MessageBox.Show(settings["copies"] + payMoney);
                                        #region 双面打订单金额减半

                                        //settings["duplex"]=1是双面，
                                        if (settings["duplex"].GetBool())
                                        {
                                            //重新计算张数
                                            double ReSetTotalpage = Math.Ceiling(Convert.ToDouble(jo["dataList"]["totalPage"]) / 2);
                                            //重新计算金额
                                            payMoney = settings["copies"].GetInt() * ReSetTotalpage * getpayMoney;
                                        }

                                        #endregion

                                        string orderID = jo["dataList"]["orderId"].ToString();
                                        string homePage = jo["dataList"]["filePageSection"].ToString().Split('-')[0];
                                        string endPage = jo["dataList"]["filePageSection"].ToString().Split('-')[1];
                                        

                                        NameValueCollection dataPay = new NameValueCollection
                                    {
                                        { "orderid",orderID},
                                        { "paymode","3"},
                                        { "homepage",homePage},
                                        { "endpage",endPage},
                                        { "iscolor",settings["color"]},
                                        { "printmode",settings["duplex"]},
                                        { "copies",settings["copies"]},
                                        { "amount",payMoney.ToString(CultureInfo.InvariantCulture)}
                                    };
                                        string temVal = orderList.OrderPay(AccountInfo.Token, dataPay);
                                        JObject JoPay = (JObject)JsonConvert.DeserializeObject(temVal);
                                        if (JoPay["code"].GetInt() == 200)
                                            MessageBox.Show("您的订单已生成", "提示");
                                        else
                                            MessageBox.Show(JoPay["msg"].ToString(), "提示");

                                    }
                                    else
                                    {
                                        MessageBox.Show("用户支付金额获取失败，已生成未支付订单", "提示");
                                    }
                                }
                                else
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        MessageBox.Show("订单生成失败2", "提示");
                                    });

                                }

                            }
                        });
                    });
                    #endregion
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        #endregion

        #region 程序在系统托盘显示
        NotifyIcon notifyIcon;  //托盘图标

        void Init_NotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon(
                Application.GetResourceStream(
                    new Uri("Image/Printer_Ico.ico", UriKind.Relative)).Stream);//加载图标
            notifyIcon.Text = "印兔打印机";
            notifyIcon.Visible = true;

            //鼠标右击事件
            MenuItem m1 = new MenuItem("退出");
            m1.Click += m1_click;
            MenuItem[] m = new MenuItem[] { m1 };
            notifyIcon.ContextMenu = new ContextMenu(m);

        }

        void m1_click(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;
            Application.Current.Shutdown();
        }



        #endregion

        #region 将文件命名为"未命名文件"

        public string reName(string sourcePath)
        {

            if (!File.Exists(sourcePath))
            {
                MessageBox.Show("文件生成失败,请稍候重试", "提示");
                return null;
            }
            string dirPath = Path.GetDirectoryName(sourcePath);
            //string fileName = Path.GetFileName(settings["savedPath"]);
            string newFileName = Path.Combine(dirPath, "未命名.pdf");
            int m = 0;
            for (int i = 1; i <= 100; i++)
            {
                if (File.Exists(newFileName))
                {
                    m++;
                    newFileName = Path.Combine(dirPath, "未命名(" + i + ").pdf");
                }
                else
                    break;
            }

            if (m >= 100)
            {
                MessageBox.Show("目录" + dirPath + "文件过大,请及时清理");
                return null;
            }
            File.Move(sourcePath, newFileName);
            return newFileName;

        }

        public string reName(string sourcePath, string pdfName)
        {

            if (!File.Exists(sourcePath))
            {
                MessageBox.Show("文件生成失败,请稍候重试", "提示");
                return null;
            }
            string dirPath = Path.GetDirectoryName(sourcePath);
            //string fileName = Path.GetFileName(settings["savedPath"]);
            string newFileName = Path.Combine(dirPath ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(pdfName) + ".pdf");
            int m = 0;
            for (int i = 1; i <= 100; i++)
            {
                if (File.Exists(newFileName))
                {
                    m++;
                    newFileName = Path.Combine(dirPath, Path.GetFileNameWithoutExtension(pdfName) + "(" + i + ").pdf");
                }
                else
                    break;
            }

            if (m >= 100)
            {
                MessageBox.Show("目录" + dirPath + "文件过大,请及时清理");
                return null;
            }
            File.Move(sourcePath, newFileName);
            return newFileName;

        }
        #endregion


    }
}
