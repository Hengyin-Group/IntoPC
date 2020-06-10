using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Threading;
using IntoApp.API;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Common.Helper;
using IntoApp.Dal;
using IntoApp.Model;
using IntoApp.utils;
using IntoApp.View.Content.Other;
using IntoApp.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using MessageBox = MyMessageBox.Controls.MessageBox;
using OrderListHelper = IntoApp.Model.OrderListHelper;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel
{
    public class PageTrayViewModel : LocalTrayViewModelBase
    {
        #region 页面初始化

        private int _pageSize = 30;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }
        /// <summary>
        /// 当前第几页
        /// </summary>

        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged("PageIndex");
            }
        }

        public PageTrayViewModel()
        {
            //GetTray(PageSize.ToString(),PageIndex.ToString());
            RunState = false;
            delegateEvent();
            DropCommand = new MyCommand<ExCommandParameter>(x => DropDown(x, LocalTray));
            UploadCommand = new MyCommand(x => TrayUpload(LocalTray));
            DeleteCommand = new MyCommand<object[]>(x => Delete(LocalTray));
            SelectAllCommand = new MyCommand<object[]>(x => SelectAll(x, LocalTray));
            CheckedCommand = new MyCommand<object[]>(x => Checked(x, LocalTray));
            PrinterCommand = new MyCommand(x => TrayPrint(LocalTray));
            NavToTrayHistoryCommand = new MyCommand(x => Navigate_TrayHistory());
        }



        #endregion

        /// <summary>
        /// 显示DataGrid显示内容的路径
        /// </summary>
        /// 
        //private string _path;

        //public string Path
        //{
        //    get { return _path; }
        //    set { _path = value; }
        //}

        #region 命令

        public MyCommand PrinterCommand { get; set; }

        public MyCommand NavToTrayHistoryCommand { get; set; }



        //public MyCommand<Object[]> DeleteCommand
        //{
        //    get { return new MyCommand<object[]>(x =>
        //    {
        //        int count= LocalTray.Count();
        //        for (int i = count; i>0; i--)
        //        {
        //            bool bo = LocalTray[i - 1].IsChecked;
        //            if (bo)
        //                LocalTray.RemoveAt(i-1);
        //        }
        //        Sort();
        //        SelectAllIsChecked = false;
        //        SelectChecked();
        //        emptyShow();
        //    });}
        //}
        ///// <summary>
        ///// 重新排序
        ///// </summary>
        //void Sort()
        //{
        //    int count= LocalTray.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        LocalTray[i].Index = i + 1;
        //    }
        //}

        //void emptyShow()
        //{
        //     EmptyIsShow= LocalTray.Count < 1;
        //}


        //public MyCommand UploadCommand { get; set; }

        //public MyCommand CreateNewFolderCommand
        //{
        //    get
        //    {
        //        return new MyCommand(x => { MessageBox.Show("正在建设中"); });
        //    }
        //}

        //public MyCommand<Object[]> SelectAllCommand
        //{
        //    get { return new MyCommand<Object[]>(x =>
        //    {
        //        bool bo=(bool)(x[0] as CheckBox).IsChecked;
        //        for (int i = 0; i < LocalTray.Count; i++)
        //        {
        //            if (LocalTray[i].IsChecked!=bo)
        //            {
        //                LocalTray[i].IsChecked = bo;
        //            }
        //        }
        //        SelectChecked();
        //    });}
        //}

        //public MyCommand<Object[]> CheckedCommand
        //{
        //    get
        //    {
        //        return  new MyCommand<Object[]>(x =>
        //        {
        //            CheckBox ch = x[0] as CheckBox;
        //            int Index = JObjectHelper.GetStrNum(ch.Tag.ToString());
        //            LocalTray[Index-1].IsChecked = (bool)ch.IsChecked;
        //            SelectChecked();
        //            SelectCheckedAll();
        //        });
        //    }
        //}

        //public MyCommand<DataGrid> SelectionChangedCommand
        //{
        //    get { return new MyCommand<DataGrid>(x=>selectionChanged(x));}
        //}

        //public void selectionChanged(DataGrid x)
        //{
        //    var trayInfo = (Tray) x.SelectedItems[0];
        //    //MessageBox.Show(trayInfo.FileName);
        //}

        //public MyCommand<ExCommandParameter> DropCommand
        //{
        //    get
        //    {
        //        return new MyCommand<ExCommandParameter>(x =>
        //        {

        //        });
        //    }
        //}

        #region 一键删除的显示恢复

        ///// <summary>
        ///// 一键删除的显示恢复
        ///// </summary>
        //void SelectChecked()
        //{
        //    bool bo = LocalTray.Any(p => p.IsChecked == true);
        //    if (bo)
        //        IsEnabled = true;
        //    else
        //        IsEnabled = false;
        //}

        //void SelectCheckedAll()
        //{
        //    bool bo = LocalTray.All(p => p.IsChecked == true);
        //    if (bo)
        //        SelectAllIsChecked = true;
        //    else
        //        SelectAllIsChecked = false;
        //}

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 注册委托事件
        /// </summary>x
        void delegateEvent()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                UploadFile += UploadFileEvent;
                FilePrint += FilePrintEvent;
            });
        }

        private string History = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trayHistory", AccountInfo.Phone,
            "trayHistoryUpload.xml");

        void TrayUpload(ObservableCollection<LocalTray> local)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                RunState = true;
                Upload(local);
            });
        }

        void TrayPrint(ObservableCollection<LocalTray> local)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                RunState = true;
                #region TrayPrint

                /*
               UploadFile += value =>
               {
                   NameValueCollection data = new NameValueCollection
                   {
                       { "sourceclient","PC"}
                   };
                   Bll.AUFileDocument document = new AUFileDocument();
                   string temVal = document.AUDiskFilePrint(AccountInfo.Token, value.ToArray(), data);
                   JObject jo = (JObject)JsonConvert.DeserializeObject(temVal);
                   if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
                   {
                       FileHelper.FileCount = jo["dataList"]["previewImgs"].Count();
                       FileHelper.FileUrl = "http://into.wainwar.com" + jo["dataList"]["pdfUrl"];
                       FileHelper.OrderId = jo["dataList"]["orderNo"].ToString();
                       FileHelper.LocalFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filetemporary");
                       FileHelper.LocalFilePath = Path.Combine(FileHelper.LocalFileDirectory, jo["dataList"]["fileName"].ToString());
                       DispatcherHelper.CheckBeginInvokeOnUI(() =>
                       {
                           (new FilePreView()).Show();
                       });

                   }
                   else
                   {
                       MessageBox.Show(jo["message"].ToString());
                   }
                   RunState = false;
               };*/

                #endregion
                //Upload(local);
                UploadFilePrint(local);
            });
        }

        void Navigate_TrayHistory()
        {
            var pageserver = ViewModelLocator.page_Server;//获取类的对象
            pageserver.CurrentPage = ServerPageManager.PageTrayHistory;
        }

        void UploadFileEvent(List<string> value)
        {
            NameValueCollection data = new NameValueCollection
            {
                 {"sourceClient", "PC"},
                 {"audiskflag", "audiskpc"}
            };
            Bll.AUFileDocument document = new AUFileDocument();
            string temVal = document.AUDiskFilesCloud(AccountInfo.Token, value.ToArray(), data);
            JObject jo = (JObject)JsonConvert.DeserializeObject(temVal);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    XElement File = new XElement("File");
                    //File.SetElementValue("Id", UnDownload[Index].Id);
                    File.SetElementValue("FileName", Path.GetFileName(value[i].ToString()));
                    File.SetElementValue("FileUrl", value[i].ToString());
                    //File.SetElementValue("DownLoadPath", Path.Combine(localDownload, UnDownload[Index].FileName));
                    File.SetElementValue("CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    FileOperationHelper.WriteXml(History, File);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Delete(LocalTray);
                    var trayUpload = PageTrayHistoryUploadedViewModel.GetInstance();
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        SynchronizationContext.SetSynchronizationContext(
                            new System.Windows.Threading.DispatcherSynchronizationContext(System.Windows
                                .Application
                                .Current.Dispatcher));
                        SynchronizationContext.Current.Post(p =>
                        {
                            trayUpload.GetLocalTrayHistoryUploaded();
                        },
                         null);
                    });
                });
            }
            else
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    MessageBox.Show(jo["message"].ToString());
                });
            }
            RunState = false;
        }

        void FilePrintEvent(List<string> value)
        {
            NameValueCollection data = new NameValueCollection
            {
                { "sourceclient","PC"}
            };
            Bll.AUFileDocument document = new AUFileDocument();
            string temVal = document.AUDiskFilePrint(AccountInfo.Token, value.ToArray(), data);
            JObject jo = (JObject)JsonConvert.DeserializeObject(temVal);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
            {
                //FileHelper.FileCount = jo["dataList"]["previewImgs"].Count();
                FileHelper.FileCount = jo["dataList"]["filePageSection"].ToString().Split('-')[1].GetInt();
                FileHelper.FileUrl = RequestAddress.server + jo["dataList"]["pdfUrl"];
                FileHelper.OrderId = jo["dataList"]["id"].ToString();
                FileHelper.OrderNo = jo["dataList"]["orderNo"].ToString();
                FileHelper.OrderState = JObjectHelper.GetStrNum(jo["dataList"]["orderState"].ToString());
                FileHelper.LocalFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filetemporary");
                FileHelper.LocalFilePath = Path.Combine(FileHelper.LocalFileDirectory, jo["dataList"]["fileName"].ToString());
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    (new FilePreView()).Show();
                });

            }
            else
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    MessageBox.Show(jo["message"].ToString());
                });

            }
            RunState = false;
        }

        public event UploadUrl FilePrint;

        void UploadFilePrint(ObservableCollection<LocalTray> local)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < local.Count; i++)
            {
                if (local[i].IsChecked)
                {
                    list.Add(local[i].FileUrl);
                }
            }
            FilePrint(list);
        }

        #endregion

        #region 命令

        //void Upload()
        //{
        //    RunState = true;
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < LocalTray.Count; i++)
        //    {
        //        if (LocalTray[i].IsChecked)
        //        {
        //            list.Add(LocalTray[i].FileUrl);
        //        }
        //    }
        //    ThreadPool.QueueUserWorkItem((obj) =>
        //    {
        //        NameValueCollection data = new NameValueCollection
        //        {
        //            { "sourceClient","PC"},
        //            { "audiskflag","audiskpc"}
        //        };
        //        Bll.AUFileDocument document = new AUFileDocument();
        //        document.AUDiskFilesCloud(AccountInfo.Token, list.ToArray(), data);
        //        RunState = false;
        //    });
        //}



        #endregion

        #region 获取唉优盘数据

        //public void GetTray(string pageSize,string page)
        //{
        //    Bll.MyDocument bllMyDocument=new MyDocument();
        //    string Str = bllMyDocument.GetTray(AccountInfo.Token,pageSize, page);
        //    JObject jo = (JObject) JsonConvert.DeserializeObject(Str);
        //    if (JObjectHelper.GetStrNum(jo["code"].ToString())==200)
        //    {
        //        for (int i = 0; i < jo["dataList"].Count(); i++)
        //        {
        //            var tray = jo["dataList"][i];
        //            Tray.Add(new Model.Tray()
        //            {
        //                Index = i+1,
        //                Id = tray["Id"].ToString(),
        //                FileName = tray["FileName"].ToString(),
        //                CreateTime = !string.IsNullOrEmpty(tray["CreateTime"].ToString())&&tray["CreateTime"].ToString().ToUpper()!="NULL"?DateTimeHelper.GetDateTime(tray["CreateTime"].ToString()):"暂无",
        //                FileExtension = tray["FileExtension"].ToString(),
        //                FileMd5 = tray["FileMd5"].ToString(),
        //                ParentId = tray["ParentId"].ToString(),
        //                FileUrl = tray["FileUrl"].ToString(),
        //            });
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show(jo["message"].ToString());
        //    }
        //}

        #endregion

        #region 属性




        private bool _trayIsShow = true;
        public bool TrayIsShow
        {
            get { return _trayIsShow; }
            set { _trayIsShow = value; }
        }



        private bool _trayHistoryIsShow = false;
        public bool TrayHistoryIsShow
        {
            get { return _trayHistoryIsShow; }
            set { _trayHistoryIsShow = value; }
        }

        #endregion

    }
}
