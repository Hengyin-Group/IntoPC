using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.AutoUpdate.Model;
using IntoApp.AutoUpdate.utils;
using IntoApp.AutoUpdate.ViewModel.Base;
using Microsoft.Practices.ServiceLocation;
using Skin.WPF.Command;

namespace IntoApp.AutoUpdate.ViewModel
{
    public class Page_DownloadViewModel:DownloadModel
    {
        #region 页面初始化
        public Page_DownloadViewModel()
        {
            //LoadCommand=new MyCommand<object[]>(x=>GetDownload1(x));
            GetDownload1();
        }
        #endregion

        #region 命令

        public MyCommand<object[]> LoadCommand { get; set; }

        #endregion

        #region 方法

        #region 双向绑定

        //void GetDownload()
        //{
        //    DownloadModels.Clear();
        //    DownloadModels.Add(new DownloadModel()
        //    {
        //        AppId = UpdateModel.AppId,
        //        CurrentVersion = UpdateModel.CurrentVersion,
        //        DownUrl = UpdateModel.UpdateFileUrl,
        //        LocalUrl = Path.Combine(UpdateModel.LocalFilePath, Path.GetFileName(UpdateModel.UpdateFileUrl)),
        //        FileName = Path.GetFileName(UpdateModel.UpdateFileUrl)
        //    });
        //    if (!Directory.Exists(Path.GetDirectoryName(DownloadModels[0].LocalUrl)))
        //    {
        //        Directory.CreateDirectory(Path.GetDirectoryName(DownloadModels[0].LocalUrl));
        //    }
        //    DownloadHelper download = new DownloadHelper(DownloadModels[0].DownUrl, DownloadModels[0].LocalUrl);
        //    download.SetProgressBarValue += value => { DownloadModels[0].DownValue = value; };
        //}

        #endregion

        #region

        void GetDownload1()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (_downloadModel == null)
                {
                    _downloadModel = new DownloadModel();
                }
                LocalUrl = UpdateModel.LocalFilePath;
                DownUrl = UpdateModel.UpdateFileUrl;
                FileName = Path.GetFileName(DownUrl);
                if (!Directory.Exists(LocalUrl))
                {
                    Directory.CreateDirectory(LocalUrl);
                }
                //重名文件删除
                if (File.Exists(Path.Combine(LocalUrl,FileName)))
                {
                    File.Delete(Path.Combine(LocalUrl,FileName));
                }
                DownloadHelper download = new DownloadHelper(DownUrl, Path.Combine(LocalUrl, FileName));
                download.SetProgressBarValue += value =>
                {
                    DownValue = value;
                    if (value==100)
                    {
                        //Window win = x[0] as Window;
                        //Frame frame = win.FindName("Frame") as Frame;
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _navigationService = ServiceLocator.Current.GetInstance<INavigate>();
                            _navigationService.NavigateTo("PageDecompression");
                        });
                    }
                };
            });
        }

        #endregion

        #endregion

        private INavigate _navigationService;

        #region 属性

        private DownloadModel _downloadModel;

        public string LocalUrl
        {
            get { return _downloadModel.LocalUrl; }
            set { _downloadModel.LocalUrl = value; }
        }

        public string DownUrl
        {
            get { return _downloadModel.DownUrl; }
            set { _downloadModel.DownUrl = value; }
        }

        public double DownValue
        {
            get
            {
                return _downloadModel.DownValue;
            }
            set
            {
                _downloadModel.DownValue = value;
                DownValueStr = _downloadModel.DownValue + "%";
                RaisePropertyChanged("DownValue");
            }
        }

        public string DownValueStr
        {
            get { return _downloadModel.DownStr; }
            set
            {
                _downloadModel.DownStr = value;
                RaisePropertyChanged("DownValueStr");
            }
        }

        public string FileName
        {
            get { return _downloadModel.FileName; }
            set { _downloadModel.FileName = value; }
        }

        #endregion

    }
}

