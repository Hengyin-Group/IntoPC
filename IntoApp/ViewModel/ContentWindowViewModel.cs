using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Media;
//using DMSkin.Core.MVVM;
using IntoApp.ViewModel.Base;
using IntoApp.API;
using IntoApp.Common.Enum;
using IntoApp.Model;
using System.Windows.Threading;
using IntoApp.utils;
using Skin.WPF.Command;
using MessageBox = MyMessageBox.Controls.MessageBox;
using MenuItem = System.Windows.Forms.MenuItem;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using IntoApp.Common;
using IntoApp.UseControl;
using Color = System.Drawing.Color;
using System.Windows.Input;
using IntoApp.Bll;
using IntoApp.Common.Helper;
using IntoApp.View.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Image = System.Windows.Controls.Image;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using TextBox = System.Windows.Controls.TextBox;

namespace IntoApp.ViewModel
{
    public class ContentWindowViewModel:ViewModelBase
    {
        #region 单例模式

        private static ContentWindowViewModel WinContentVM;
        private static object locker=new object();

        public static ContentWindowViewModel GetInstance()
        {
            if (WinContentVM==null)
            {
                lock (locker)
                {
                    WinContentVM=new ContentWindowViewModel();
                }
            }

            return WinContentVM;
        }

        #endregion

        #region 页面初始化
        private ContentWindowViewModel()
        {
            SubscribeMQtt();
            MonitorPrinter();
            ReStartPrinterCommand=new MyCommand(x=>reStartPrinter());
            AboutmeCommand=new MyCommand(x=>AboutmeClick());
            VersioninfoCommand=new MyCommand(x=>VersioninfoClick());
            CanvasHideCommand=new MyCommand<Object[]>(x=>CanvasHide(x));
            ImageMouseBtnUpCommand=new MyCommand<object[]>(x=>ImageMouseBtnUp());
            HeaderClickCommand=new MyCommand(x=>HeaderClick());
            EditNameCommand=new MyCommand<object[]>(x=>EditNickName(x));
            EditSignatureCommand=new MyCommand<object[]>(x=>EditSignature(x));
        }
        #endregion

        #region 命令

        public MyCommand ReStartPrinterCommand { get; set; }
        public MyCommand AboutmeCommand { get; set; }
        public MyCommand VersioninfoCommand { get; set; }
        public MyCommand<Object[]> CanvasHideCommand { get; set; }
        public MyCommand<Object[]> ImageMouseBtnUpCommand { get; set; }
        public MyCommand HeaderClickCommand { get; set; }
        public MyCommand<Object[]> EditNameCommand { get; set; }
        public MyCommand<Object[]> EditSignatureCommand { get; set; }

        #endregion

        #region 方法
        void SubscribeMQtt()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                new M2MqttHelper();
            });
        }

        /// <summary>
        /// 重启打印机
        /// </summary>
        void reStartPrinter()
        {
            string ProcName = "IntoApp.Printer";
            string tempName = "";
            foreach (Process thisProc in Process.GetProcesses())
            {
                tempName = thisProc.ProcessName;
                if (tempName==ProcName)
                {
                    if (!thisProc.CloseMainWindow())
                    {
                        thisProc.Kill();
                    }
                }
            }
            LoginHelper.AddPrinter();
        }

        DispatcherTimer timer=new DispatcherTimer();

        void MonitorPrinter()
        {
            timer.Interval=TimeSpan.FromSeconds(2);
            timer.Tick += (x,e) =>
            {
                string ProcName = "IntoApp.Printer";
                string tempName = "";
                foreach (Process thispro in Process.GetProcesses())
                {
                    tempName = thispro.ProcessName;
                    if (tempName==ProcName)
                    {
                        PrinterIcon= Geometry.Parse("M518.4 928c-51.2-6.4-76.8-12.8-128-38.4-96-44.8-179.2-147.2-211.2-256-25.6-102.4-6.4-217.6 57.6-307.2 51.2-76.8 121.6-134.4 230.4-166.4 25.6-12.8 44.8-19.2 44.8-25.6 0-6.4-12.8-19.2-25.6-32-25.6-25.6-32-38.4-12.8-57.6 19.2-19.2 38.4-12.8 76.8 19.2 19.2 19.2 51.2 44.8 70.4 57.6 38.4 32 51.2 44.8 44.8 64 0 12.8-64 89.6-102.4 134.4-19.2 32-38.4 38.4-51.2 25.6-6.4 0-12.8-12.8-19.2-19.2-6.4-12.8 0-32 32-64 12.8-12.8 19.2-25.6 19.2-32 0-6.4-25.6-12.8-57.6-6.4-57.6 19.2-102.4 44.8-153.6 89.6-19.2 25.6-44.8 57.6-51.2 70.4-51.2 102.4-51.2 224 6.4 320 25.6 44.8 83.2 102.4 128 121.6 102.4 51.2 217.6 44.8 313.6-6.4 115.2-64 179.2-211.2 153.6-332.8-6.4-44.8-38.4-102.4-64-134.4s-32-44.8-25.6-64c6.4-19.2 25.6-25.6 44.8-19.2 32 12.8 89.6 102.4 108.8 166.4 12.8 51.2 12.8 160 0 211.2-12.8 44.8-51.2 115.2-76.8 147.2-25.6 32-83.2 76.8-121.6 102.4-64 25.6-153.6 38.4-230.4 32z m0 0");
                        PrinterBrush=new SolidColorBrush(System.Windows.Media.Color.FromRgb(0,205,0));
                        return;
                    }
                }
                PrinterIcon=Geometry.Parse("M220.8 812.8l22.4 22.4 272-272 272 272 48-44.8-275.2-272 275.2-272-48-48-272 275.2-272-275.2-22.4 25.6-22.4 22.4 272 272-272 272z");
                PrinterBrush=new SolidColorBrush(System.Windows.Media.Color.FromRgb(255 ,0, 0));
            };
            timer.Start();
        }

        void AboutmeClick()
        {
            ViewModelLocator.Win_AboutMe.Show();
        }

        void VersioninfoClick()
        {
            ViewModelLocator.Win_VersionInfo.Show();
        }

        void CanvasHide(object[] obj)
        {
            //Grid grid= obj[0] as Grid;
            //grid.Visibility = Visibility.Hidden;
            CanvasIsShow = false;
            HeaderEditIsShow = false;
        }

        void HeaderClick()
        {
            HeaderEditIsShow = true;
            CanvasIsShow = false;
        }

        /// <summary>
        /// 单方面修改昵称
        /// </summary>
        void EditNickName(object[] x)
        {
            TextBox _nicknameTextBox = x[0] as TextBox;
            string str= _nicknameTextBox.Text;
            if (string.IsNullOrEmpty(str))
            {
                NickName = AccountInfo.NickName;
                MessageBox.Show("昵称不能为空");
                return;
            }
            NameValueCollection data = new NameValueCollection
            {
                { "nickname",str},
            };
            Account account = new Account();
            string msg = account.EditOwnInfo(AccountInfo.Token, null, data);
            JObject Jo = (JObject)JsonConvert.DeserializeObject(msg);
            if (JObjectHelper.GetStrNum(Jo["code"].ToString()) == 200)
                NickName = AccountInfo.NickName = Jo["dataList"]["nickname"].ToString();
        }

        /// <summary>
        /// 单方面修改签名
        /// </summary>
        void EditSignature(object[] x)
        {
            TextBox _nicknameTextBox = x[0] as TextBox;
            string str = _nicknameTextBox.Text;
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Bll.Account account = new Account();
                NameValueCollection data = new NameValueCollection
                {
                    { "signature",str},
                };
                string msg = account.EditOwnInfo(AccountInfo.Token, null, data);
                JObject Jo = (JObject)JsonConvert.DeserializeObject(msg);
                if (JObjectHelper.GetStrNum(Jo["code"].ToString()) == 200)
                    Signature=AccountInfo.Expertise = Jo["dataList"]["expertise"].ToString();
            });
        }
        /// <summary>
        /// 单方面修改头像
        /// </summary>
        void ImageMouseBtnUp()
        {
            //WinImageClipping winImage=new WinImageClipping();
            //winImage.Show();
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*";
            //ofd.InitialDirectory = @"C:\Users\Public\Pictures";
            ofd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonPictures);
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                //此处做你想做的事 ...=ofd.FileName; 
                //MessageBox.Show(ofd.FileName);//显示文件完整路径
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    System.Drawing.Image image = new Bitmap(ofd.FileName);
                    int minValue = image.Height > image.Width ? image.Width : image.Height;
                    Bitmap bmp = ThumbnailImage.CutImage(image, minValue, minValue);
                    Bitmap bmp_60 = new Bitmap(bmp);
                    bmp.Dispose();
                    if (!Directory.Exists(Path.GetDirectoryName(SavePath.UserIconEdit)))
                        Directory.CreateDirectory(Path.GetDirectoryName(SavePath.UserIconEdit));
                    bmp_60.Save(SavePath.UserIconEdit, ImageFormat.Png);
                    bmp_60.Dispose();
                    Bll.Account account = new Account();
                    string msg = account.EditOwnInfo(AccountInfo.Token, new[] { SavePath.UserIconEdit }, null);
                    JObject Jo = (JObject)JsonConvert.DeserializeObject(msg);
                    if (JObjectHelper.GetStrNum(Jo["code"].ToString()) == 200)
                    {
                        AccountInfo.IconImage = Jo["dataList"]["icon"].ToString();
                        string NewFilePath = Path.Combine(SavePath.UserInfo,
                            Path.GetFileNameWithoutExtension(AccountInfo.IconImage));
                        SavePath.UserIcon_30x30 = Path.Combine(NewFilePath, "_30x30.bmp");
                        if (!Directory.Exists(NewFilePath))
                            Directory.CreateDirectory(NewFilePath);
                        File.Copy(SavePath.UserIconEdit, Path.Combine(NewFilePath, Path.GetFileName(AccountInfo.IconImage)));
                        System.Drawing.Image _image = new Bitmap(Path.Combine(NewFilePath, Path.GetFileName(AccountInfo.IconImage)));
                        Bitmap icon_30x30 = ThumbnailImage.SizeImageWithOldPercent(_image, 30, 30);
                        Bitmap bmp30 = new Bitmap(icon_30x30);
                        icon_30x30.Dispose();
                        bmp30.Save(SavePath.UserIcon_30x30, ImageFormat.Bmp);
                        HeaderPath = Path.Combine(NewFilePath, Path.GetFileName(AccountInfo.IconImage));
                        ImagePath = SavePath.UserIcon_30x30;
                    }
                });
            }
        }

        #endregion

        #region 页面切换

        private Page currentPage = ContentPageManager.PageServer;//起始页

        /// <summary>
        /// 当前页
        /// </summary>
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                RaisePropertyChanged("CurrentPage");//当属性发生改变的时候会通知所有监听者
            }
        }

        private LeftMenu selectMenu;

        public LeftMenu SeLeftMenu
        {
            get { return selectMenu; }
            set
            {
                CanvasIsShow = false;
                HeaderEditIsShow = false;
                selectMenu = value;
                RaisePropertyChanged("SeLeftMenu");
                switch (selectMenu)
                {
                    case LeftMenu.Contacts:   //工作台
                        CurrentPage = ContentPageManager.PageContacts;
                        break;
                    case LeftMenu.Message:
                        CurrentPage = ContentPageManager.PageMessage;
                        break;
                    case LeftMenu.WorkBench:
                        CurrentPage = ContentPageManager.PageWorkBench;
                        break;
                    case LeftMenu.Server:
                        CurrentPage = ContentPageManager.PageServer;
                        break;
                    case LeftMenu.Mine:
                        CurrentPage = ContentPageManager.PageMine;
                        break;
                    case LeftMenu.Setting:
                        CanvasIsShow = true;
                        break;
                }
                
            }
        }

        #region 用户信息



        #endregion

        #endregion

        #region 用户个人资料
        private string _headerPath = SavePath.UserIconSource_Local;
        public string HeaderPath
        {
            get
            {
                if (string.IsNullOrEmpty(_headerPath)||!File.Exists(_headerPath))
                {
                    _headerPath = "pack://application:,,,/Contents/image/Header/icon-headsculpture@3x.png";
                }
                return _headerPath;
            }
            set
            {
                _headerPath = value;
                RaisePropertyChanged("HeaderPath");
            }
        }

        private string _signature = SubStringHelper.StrCut(AccountInfo.Expertise,50);
        public string Signature
        {
            get
            {
                if (string.IsNullOrEmpty(_signature))
                {
                    _signature = "这个家伙很懒什么都没留下";
                }
                return _signature;
            }
            set
            {
                _signature = value; 
                RaisePropertyChanged("Signature");
            }
        }


        private string _nickName = AccountInfo.NickName;

        public string NickName
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                RaisePropertyChanged("NickName");
            }
        }

        BitmapImage _ImageSource;
        /// <summary>
        /// 显示的图标
        /// </summary>
        public BitmapImage ImageSource
        {
            get { return _ImageSource; }
            set
            {
                _ImageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        private string _ImagePath = SavePath.UserIcon_30x30;
        /// <summary>
        /// 显示的图标路径
        /// </summary>
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(_ImagePath) || !File.Exists(_ImagePath))
                {
                    _ImagePath = "pack://application:,,,/Contents/image/Header/icon-headsculpture.png";
                }
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;

                RaisePropertyChanged("ImagePath");
            }
        }

        public BitmapImage BitMapImg(string Img)
        {
            Uri uri = new Uri(Img, UriKind.RelativeOrAbsolute);
            BitmapImage image = new BitmapImage(uri);
            return image;
        }

        #endregion

        #region 属性

        private Geometry _printerIcon=Geometry.Parse("M518.4 928c-51.2-6.4-76.8-12.8-128-38.4-96-44.8-179.2-147.2-211.2-256-25.6-102.4-6.4-217.6 57.6-307.2 51.2-76.8 121.6-134.4 230.4-166.4 25.6-12.8 44.8-19.2 44.8-25.6 0-6.4-12.8-19.2-25.6-32-25.6-25.6-32-38.4-12.8-57.6 19.2-19.2 38.4-12.8 76.8 19.2 19.2 19.2 51.2 44.8 70.4 57.6 38.4 32 51.2 44.8 44.8 64 0 12.8-64 89.6-102.4 134.4-19.2 32-38.4 38.4-51.2 25.6-6.4 0-12.8-12.8-19.2-19.2-6.4-12.8 0-32 32-64 12.8-12.8 19.2-25.6 19.2-32 0-6.4-25.6-12.8-57.6-6.4-57.6 19.2-102.4 44.8-153.6 89.6-19.2 25.6-44.8 57.6-51.2 70.4-51.2 102.4-51.2 224 6.4 320 25.6 44.8 83.2 102.4 128 121.6 102.4 51.2 217.6 44.8 313.6-6.4 115.2-64 179.2-211.2 153.6-332.8-6.4-44.8-38.4-102.4-64-134.4s-32-44.8-25.6-64c6.4-19.2 25.6-25.6 44.8-19.2 32 12.8 89.6 102.4 108.8 166.4 12.8 51.2 12.8 160 0 211.2-12.8 44.8-51.2 115.2-76.8 147.2-25.6 32-83.2 76.8-121.6 102.4-64 25.6-153.6 38.4-230.4 32z m0 0");

        public Geometry PrinterIcon
        {
            get { return _printerIcon; }
            set
            {
                _printerIcon = value;
                RaisePropertyChanged("PrinterIcon");
            }
        }

        private SolidColorBrush _printerBrush=new SolidColorBrush(System.Windows.Media.Color.FromRgb(0 ,205, 0));
        public SolidColorBrush PrinterBrush
        {
            get { return _printerBrush; }
            set
            {
                _printerBrush = value;
                RaisePropertyChanged("PrinterBrush");
            }
        }

        private bool _canvasIsShow = false;
        public bool CanvasIsShow
        {
            get { return _canvasIsShow; }
            set
            {
                _canvasIsShow = value; 
                RaisePropertyChanged("CanvasIsShow");
            }
        }

        private bool _headerEditIsShow = false;
        public bool HeaderEditIsShow
        {
            get { return _headerEditIsShow; }
            set
            {
                _headerEditIsShow = value; 
                RaisePropertyChanged("HeaderEditIsShow");
            }
        }

       
        #endregion

    }
}
