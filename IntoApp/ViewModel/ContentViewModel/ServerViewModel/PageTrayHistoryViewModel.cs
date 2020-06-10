using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Controls;
using IntoApp.API;
using IntoApp.Common.Helper;
using IntoApp.Model;
using IntoApp.ViewModel.Base;
using MessageBox= MyMessageBox.Controls.MessageBox;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrayHistory = IntoApp.Common.Enum.TrayHistory;
using IntoApp.Common;
using XmlHelper = IntoApp.Common.Helper.XmlHelper;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using IntoApp.Bll;
using IntoApp.utils;
using Skin.WPF.Command;
using uPLibrary.Networking.M2Mqtt;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Common.Enum;
using IntoApp.Dal;
using IntoApp.View.Content.Server.TrayHistory;
using Button = System.Windows.Controls.Button;
using FileHelper = IntoApp.Model.FileHelper;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel
{
    public class PageTrayHistoryViewModel:ViewModelBase
    {

        #region 页面初始化

        public PageTrayHistoryViewModel()
        {
            BackToTrayCommand=new MyCommand(x=>Back());
        }

        #endregion
        #region 命令

        public MyCommand BackToTrayCommand { get; set; }

        #endregion

        #region 方法

        void Back()
        {
            var pageserver = ViewModelLocator.page_Server;
            pageserver.CurrentPage = ServerPageManager.PageTray;
        }

        #endregion


        private Page currentPage=TrayHistoryPageManager.PageTrayUploaded;
        private TrayHistory selectMenu;

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value; 
                RaisePropertyChanged("CurrentPage");
            }
        }

        public TrayHistory SelectMenu
        {
            get { return selectMenu; }
            set
            {
                selectMenu = value;
                RaisePropertyChanged("SelectMenu");
                switch (selectMenu)
                {
                    case TrayHistory.Uploaded:
                        CurrentPage = TrayHistoryPageManager.PageTrayUploaded;
                        break;
                    case TrayHistory.Downloaded:
                        CurrentPage = TrayHistoryPageManager.PageTrayDownload;
                        break;
                    case TrayHistory.UnDownload:
                        CurrentPage = TrayHistoryPageManager.PageTrayUnDownload;
                        break;
                }
            }
        }
    }
    public class PageTrayHistoryUploadedViewModel : TrayHistroyViewModelBase
    {

        #region 单例模式

        private static PageTrayHistoryUploadedViewModel PageTrayUpload;

        private static readonly  object locker=new object();

        public static PageTrayHistoryUploadedViewModel GetInstance()
        {
            if (PageTrayUpload==null)
            {
                lock (locker)
                {
                    PageTrayUpload = new PageTrayHistoryUploadedViewModel();
                }

            }
            return PageTrayUpload;
        }

        #endregion

        #region 页面初始化

        private bool _isFirstLoaded = true;
        public bool IsFirstLoaded
        {
            get { return _isFirstLoaded; }
            set { _isFirstLoaded = value;}
        }

        private string path =AppDomain.CurrentDomain.BaseDirectory+"trayHistory" ;

        private PageTrayHistoryUploadedViewModel()
        {
            //GetLocalTrayHistoryUploaded();
            LoadedCommand = new MyCommand(x => GetLocalTrayHistoryUploaded());
            DeleteCommand=new MyCommand<object[]>(x =>Delete(Uploaded));
            SelectAllCommand=new MyCommand<object[]>(x=>SelectAll(Uploaded,x));
            CheckedCommand=new MyCommand<object[]>(x=>Check(Uploaded,x));
        }
        #endregion

        #region 方法

        public void GetLocalTrayHistoryUploaded()
        {
            Uploaded.Clear();
            string FileDir = Path.Combine(path, AccountInfo.Phone);
            string FilePath = Path.Combine(FileDir, "trayHistoryUpload.xml");
            if (!Directory.Exists(FileDir))
            {
                Directory.CreateDirectory(FileDir);
            }
            if (!File.Exists(FilePath))
            {
                XmlHelper.CreateXml(FileDir, "trayHistoryUpload.xml");
            }
            XElement xele = XElement.Load(FilePath);
            var item = (from ele in xele.DescendantsAndSelf("File")
                        select ele).ToList();
            EmptyIsShow = item.Count < 1;
            if (!EmptyIsShow)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xele.ToString());
                string str = JsonConvert.SerializeXmlNode(document);
                JObject json = (JObject)JsonConvert.DeserializeObject(str);
                Regex reg = new Regex(@"\[[^\[\]]+\]");
                MatchCollection Files = reg.Matches(str);
                bool FileCount = Files.Count > 0;
                int index = 0;
                if (FileCount)
                {
                    for (int i = 0; i < json["Root"]["File"].Count(); i++)
                    {
                        var cc = json["Root"]["File"][i];
                        Uploaded.Add(new Model.TrayHistory()
                        {
                            Index = index + 1,
                            CreateDate = Convert.ToDateTime(cc["CreateTime"].ToString()),
                            FileName = cc["FileName"].ToString(),
                            FileUrl = cc["FileUrl"].ToString(),
                            //DownloadPath = cc["DownLoadPath"].ToString(),
                            //Id = cc["Id"].ToString()
                        });
                        index++;
                    }
                }
                else
                {
                    var cc = json["Root"]["File"];
                    Uploaded.Add(new Model.TrayHistory()
                    {
                        Index = index + 1,
                        CreateDate = Convert.ToDateTime(cc["CreateTime"].ToString()),
                        FileName = cc["FileName"].ToString(),
                        //FileUrl = cc["FileUrl"].ToString(),
                        //DownloadPath = cc["DownLoadPath"].ToString(),
                        Id = cc["Id"].ToString()
                    });
                }
            }
            RunState = false;
        }

        #endregion

    }
    public class PageTrayHistoryDownloadedViewModel : TrayHistroyViewModelBase
    {
        #region 单例模式

        private static PageTrayHistoryDownloadedViewModel PageTrayDownloaded;

        private static readonly object locker=new object();

        public static PageTrayHistoryDownloadedViewModel GetInstance()
        {
            if (PageTrayDownloaded==null)
            {
                lock (locker)
                {
                    PageTrayDownloaded=new PageTrayHistoryDownloadedViewModel();
                }
            }

            return PageTrayDownloaded;
        }

        #endregion

        #region 页面初始化

        private bool _isFirstLoaded = true;
        public bool IsFirstLoaded
        {
            get { return _isFirstLoaded; }
            set { _isFirstLoaded = value; }
        }

        private string path = AppDomain.CurrentDomain.BaseDirectory + "trayHistory";
        private PageTrayHistoryDownloadedViewModel()
        {
            //GetTrayHistoryDownload();
            LoadedCommand=new MyCommand(x=>GetTrayHistoryDownload());
            DeleteCommand=new MyCommand<object[]>(x=>Delete(Downloaded));
            OpenFloderCommand = new MyCommand<object[]>(x => OpenFloder(x));
            SelectAllCommand =new MyCommand<object[]>(x=>SelectAll(Downloaded,x));
            CheckedCommand=new MyCommand<object[]>(x=>Check(Downloaded,x));
        }
        #endregion

        #region 命令

        public MyCommand<Object[]> OpenFloderCommand { get; set; }

        #endregion
        private string localDownload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trayFileDownload");

        #region 方法
        public void GetTrayHistoryDownload()
        {
            Downloaded.Clear();
            string FileDir = Path.Combine(path, AccountInfo.Phone);
            string FilePath = Path.Combine(FileDir, "trayHistoryDownload.xml");
            if (!Directory.Exists(FileDir))
            {
                Directory.CreateDirectory(FileDir);
            }
            if (!File.Exists(FilePath))
            {
                XmlHelper.CreateXml(FileDir, "trayHistoryDownload.xml");
            }
            XElement xele = XElement.Load(FilePath);
            var item = (from ele in xele.DescendantsAndSelf("File")
                        select ele).ToList();
            EmptyIsShow = item.Count < 1;
            if (!EmptyIsShow)
            {
                XmlDocument document=new XmlDocument();
                document.LoadXml(xele.ToString());
                string str = JsonConvert.SerializeXmlNode(document);
                JObject json = (JObject) JsonConvert.DeserializeObject(str);
                Regex reg=new Regex(@"\[[^\[\]]+\]");
                MatchCollection Files = reg.Matches(str);
                bool FileCount = Files.Count > 0;
                int index = 0;
                if (FileCount)
                {
                    for (int i = 0; i < json["Root"]["File"].Count(); i++)
                    {
                        var cc = json["Root"]["File"][i];
                        Downloaded.Add(new Model.TrayHistory()
                        {
                            Index = index + 1,
                            CreateDate = Convert.ToDateTime(cc["CreateTime"].ToString()),
                            FileName = cc["FileName"].ToString(),
                            FileUrl = cc["FileUrl"].ToString(),
                            DownloadPath = cc["DownLoadPath"].ToString(),
                            Id = cc["Id"].ToString()
                        });
                        index++;
                    }
                }
                else
                {
                    var cc = json["Root"]["File"];
                    Downloaded.Add(new Model.TrayHistory()
                    {
                        Index = index+1,
                        CreateDate = Convert.ToDateTime(cc["CreateTime"].ToString()),
                        FileName = cc["FileName"].ToString(),
                        FileUrl = cc["FileUrl"].ToString(),
                        DownloadPath = cc["DownLoadPath"].ToString(),
                        Id = cc["Id"].ToString()
                    });
                }

                //string str = "<Root>";
                //for (int i = 0; i < item.Count; i++)
                //{
                //    str += item[i].ToString();
                //}
                //str += "</Root>";
                //string json = XmlHelper.ParseXmlToJson(str);
                //JObject jo = ((JObject)JsonConvert.DeserializeObject(json));
                //int index = 0;
                //MessageBox.Show(jo["Root"]["Item"].ToString());
                
            }
            RunState = false;
        }

        public void OpenFloder(Object[] obj)
        {
            Button Btn = obj[0] as Button;
            int Index = JObjectHelper.GetStrNum(Btn.Tag.ToString()) - 1;
            string FileName = Downloaded[Index].FileName;
            //打开文件资源管理器并选中文件
            System.Diagnostics.Process.Start("explorer.exe", "/select," + Path.Combine(localDownload, FileName));
        }

        #endregion

    }
    public class PageTrayHistoryUnDownloadViewModel : TrayHistroyViewModelBase
    {
        #region 页面初始化

        public PageTrayHistoryUnDownloadViewModel()
        {
            //GetTrayFileList();
            if (IsRefresh)
            {
                LoadedCommand = new MyCommand(x => GetTrayFileList());
                DownloadAllCommand = new MyCommand(x => DownloadAll());
                DownloadCommand = new MyCommand<object[]>(x => DownLoad(x));
                OpenFloderCommand = new MyCommand<object[]>(x => OpenFloder(x));
                DeleteCommand = new MyCommand<object[]>(x => Delete(UnDownload));
                SelectAllCommand = new MyCommand<object[]>(x => SelectAll(UnDownload, x));
                CheckedCommand = new MyCommand<object[]>(x => Check(UnDownload, x));
            }
        }

        #endregion

        #region 命令
        public MyCommand LoadedCommand { get; set; }
        public MyCommand DownloadAllCommand { get; set; }
        public MyCommand<Object[]> DownloadCommand { get; set; }
        public MyCommand<Object[]> OpenFloderCommand { get; set; }


        #endregion

        #region 方法

        private string localDownload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trayFileDownload");

        private string History = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trayHistory", AccountInfo.Phone,
            "trayHistoryDownload.xml");

        void DownloadAll()
        {
            List<int> Index=new List<int>();
            for (int i = 0; i < UnDownload.Count; i++)
            {
                if (UnDownload[i].IsChecked&&UnDownload[i].CanDownload)
                {
                    if (!Directory.Exists(localDownload))
                    {
                        Directory.CreateDirectory(localDownload);
                    }
                    if (DownloadHelper.HttpFileExist(UnDownload[i].FileUrl))
                    {
                        #region 

                        //UnDownload[i].CanDownload = false;
                        //UnDownload[i].ProgressBarShow = true;
                        //TrayDownload _trayDownload = new TrayDownload(UnDownload[i].FileUrl, localDownload);
                        //_trayDownload.SetProgressBarValue += value =>
                        //{
                        //    UnDownload[i].DownValue = value;
                        //};

                        #endregion
                        //DownloadFile(i);
                        Index.Add(i);
                    }
                    else
                    {
                        UnDownload[i].DownloadText = "文件路径错误或已删除";
                    }
                }
            }

            DownloadCount(Index);
        }

        private readonly object locker=new object();
        void DownLoad(object[] obj)
        {
            Button Btn = obj[0] as Button;
            List<int> list=new List<int>(); list.Add(JObjectHelper.GetStrNum(Btn.Tag.ToString()) - 1);
            lock (locker)
            {
                ++DownLink;
                if (DownLink >= MaxDegreeOfParallelism)
                {
                    MessageBox.Show("最多5个下载任务");
                    return;
                }
            }
            DownloadCount(list);
            lock (locker)
            {
                --DownLink;
            }


            //DownloadFile(Index);
            //MessageBox.Show(Index.ToString());
        }

        private int MaxDegreeOfParallelism = 5;
        void DownloadCount(List<int> index)
        {
            IsRefresh = false;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
            Parallel.ForEach(index, parallelOptions, i =>
            {
                DownloadFile(i);
            });
            IsRefresh = true;

        }

        /// <summary>
        /// 根据索引下载文件
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="Index"></param>
        void DownloadFile(int Index)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                UnDownload[Index].DownTextShow = false;
                UnDownload[Index].CanDownload = false;
                UnDownload[Index].ProgressBarShow = true;
                if (!Directory.Exists(localDownload))
                {
                    Directory.CreateDirectory(localDownload);
                }

                bool IsCover = false;
                if (File.Exists(Path.Combine(localDownload, UnDownload[Index].FileName)))
                {
                    //MessageBoxResult result = MessageBox.Show("存在相同文件名是否覆盖?", "提示", MessageBoxButton.YesNo);
                    //IsCover = result == MessageBoxResult.OK;
                    //if (result == MessageBoxResult.OK)
                        File.Delete(Path.Combine(localDownload, UnDownload[Index].FileName));
                    //else
                    //    return;
                }
                TrayDownload _trayDownload = new TrayDownload(UnDownload[Index].FileUrl, Path.Combine(localDownload, UnDownload[Index].FileName));
                _trayDownload.SetProgressBarValue += value =>
                {
                    UnDownload[Index].DownValue = value;
                    if ((int)value == 100)
                    {
                        UnDownload[Index].ProgressBarShow = false;
                        UnDownload[Index].OpenLocalDirectoryShow = true;
                        UnDownload[Index].IsShow = false;
                        XElement File = new XElement("File");
                        File.SetElementValue("Id", UnDownload[Index].Id);
                        File.SetElementValue("FileName", UnDownload[Index].FileName);
                        File.SetElementValue("FileUrl", UnDownload[Index].FileUrl);
                        File.SetElementValue("DownLoadPath", Path.Combine(localDownload,UnDownload[Index].FileName));
                        File.SetElementValue("CreateTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        FileOperationHelper.WriteXml(History, File);
                        var trayDownload= PageTrayHistoryDownloadedViewModel.GetInstance();
                        //修改下载记录表
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            SynchronizationContext.SetSynchronizationContext(
                                new System.Windows.Threading.DispatcherSynchronizationContext(System.Windows
                                    .Application.Current.Dispatcher));
                            SynchronizationContext.Current.Post(p =>
                            {
                                trayDownload.GetTrayHistoryDownload();
                            },null);
                        });
                        DelNoTransferAUDiskFile(UnDownload[Index].Id);
                    }
                };
            });  
        }

        /// <summary>
        /// 获取同步文件列表
        /// </summary>
        void GetTrayFileList()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                RunState = true;
                EmptyIsShow=false;
                Bll.AUFileDocument document = new AUFileDocument();
                string temVal = document.GetTransferAUDiskFile(AccountInfo.Token, AccountInfo.ID);
                JObject jo = (JObject)JsonConvert.DeserializeObject(temVal);
                if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        UnDownload.Clear();
                        int index = 0;
                        for (int i = 0; i < jo["dataList"].Count(); i++)
                        {
                            UnDownload.Add(new Model.TrayHistory()
                            {
                                Index = index + 1,
                                FileUrl = RequestAddress.server + jo["dataList"][i]["fileUrl"],
                                Id = jo["dataList"][i]["ID"].ToString(),
                                FileName = Path.GetFileName(jo["dataList"][i]["fileUrl"].ToString()),
                                CreateDate = DateTime.Now,
                                CanDownload = true,
                                DownTextShow = true
                            });
                            index++;
                            EmptyIsShow = false;
                        }
                    });
                }
                RunState = false;
            });
        }

        /// <summary>
        /// 打开下载目录
        /// </summary>
        void OpenFloder(Object[] obj)
        {
            Button Btn = obj[0] as Button;
            int Index = JObjectHelper.GetStrNum(Btn.Tag.ToString()) - 1;
            string FileName= UnDownload[Index].FileName;
            //打开文件资源管理器并选中文件
            System.Diagnostics.Process.Start("explorer.exe","/select,"+ Path.Combine(localDownload,FileName));
        }

        void DelNoTransferAUDiskFile(string id)
        {
            AUFileDocument document =new AUFileDocument();
            document.DelNoTransferAUDiskFile(AccountInfo.Token, id);
        }


        #endregion


        #region 属性

        private bool _isRefresh=true;

        public bool IsRefresh
        {
            get { return _isRefresh; }
            set { _isRefresh = value; }
        }


        private int _downLink=0;

        public int DownLink
        {
            get { return _downLink; }
            set { _downLink = value; }
        }


        #endregion

    }
    
}
