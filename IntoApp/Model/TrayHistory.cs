using IntoApp.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoApp.ViewModel.Base;

namespace IntoApp.Model
{
    public class TrayHistory:ViewModelBase
    {
        private string _id;
        private int _index;
        private string _fileName;

        /// <summary>
        /// 文件的绝对路径或网络路径
        /// </summary>
        private string _fileUrl;
        private bool _isChecked = false;
        private DateTime _createDate;
        private int _state;
        
        private bool _canDownload=false;
        private double downValue = 0.0;
        private string _downValueStr = "0%";
        private string _downloadText = "下载";
        private bool _downTextShow=false;
        private bool _progressBarShow = false;

        private bool _openLocalDirectoryShow = false;

        /// <summary>
        /// 文件下载到的本地路径
        /// </summary>
        private string _downloadPath;

        private bool _isShow = true;


        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FileUrl
        {
            get { return _fileUrl; }
            set
            {
                _fileUrl = value;
                RaisePropertyChanged("FileUrl");
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value;}
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 没有后缀的文件名
        /// </summary>
        public string FileNameNoSuffix
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    string filename = FileName.Split('.')[0];
                    return filename;
                }
                else
                {
                    return "[文件名为空]";
                }
            }
        }

        public string FileNameCut
        {
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 30); }
        }

        public string CreateTime
        {
            get { return CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public int State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }

        public string StateText
        {
            get
            {
                if (State == 0)
                {
                    return "已上传";
                }

                if (State==10)
                {
                    return "未下载";
                }

                if (State==200)
                {
                    return "已下载";
                }

                else
                {
                    return "未知";
                }
            }
        }

        public bool CanDownload
        {
            get { return _canDownload; }
            set
            {
                _canDownload = value;
                RaisePropertyChanged("CanDownload");
            }
        }

        public double DownValue
        {
            get { return downValue; }
            set
            {
                downValue = value;
                DownValueStr = downValue + "%";
                RaisePropertyChanged("DownValue");
            }
        }

        public string DownValueStr
        {
            get { return _downValueStr; }
            set
            {
                _downValueStr= value;
                RaisePropertyChanged("DownValueStr");
            }
        }

        public string DownloadText
        {
            get { return _downloadText; }
            set
            {
                _downloadText = value;
                RaisePropertyChanged("DownloadText");
            }
        }

        public bool ProgressBarShow
        {
            get { return _progressBarShow; }
            set
            {
                _progressBarShow = value;
                RaisePropertyChanged("ProgressBarShow");
            }
        }

        public bool DownTextShow
        {
            get { return _downTextShow; }
            set
            {
                _downTextShow = value;
                RaisePropertyChanged("DownTextShow");
            }
        }

        public string DownloadPath
        {
            get { return _downloadPath; }
            set
            {
                _downloadPath = value;
                RaisePropertyChanged("DownloadPath");
            }
        }

        public bool OpenLocalDirectoryShow
        {
            get { return _openLocalDirectoryShow; }
            set
            {
                _openLocalDirectoryShow = value; 
                RaisePropertyChanged("OpenLocalDirectoryShow");
            }
        }

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                _isShow = value;
                RaisePropertyChanged("IsShow");
            }
        }
    }
}
