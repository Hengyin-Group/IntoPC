using IntoApp.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using IntoApp.Dal;
using GHelper=IntoApp.Common.Helper.GeometryHelper;
using IntoApp.ViewModel.Base;

namespace IntoApp.Model
{
    /// <summary>
    /// 唉优盘文件类
    /// </summary>
    public class Tray:ViewModelBase
    {

        private string _id;
        private string _fileName;
        private string _createTime;
        private string _fileExtension;
        private string _fileMD5;
        private string _parentId;
        private string _fileUrl;
        private bool _isChecked = false;
        private int _index;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 带后缀名的文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }

        public string FileMd5
        {
            get { return _fileMD5; }
            set { _fileMD5 = value; }
        }

        public string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl =RequestAddress.server+value; }
        }

        public string CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
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

        //private GHelper g;
        private GHelper g=new GeometryHelper() ;

        public Geometry Image
        {
            get { return g.Icon_Folder; }
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

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

    }

    public class LocalTray : ViewModelBase
    {
        private int _index;
        private string _fileName;
        private string _fileUrl;
        private bool _isChecked = false;
        private DateTime _createDate;

        private int _state;
        private bool _isDirectory;

        private bool _canDownload = false;
        private double downValue = 0.0;
        private bool _downTextShow = false;
        private bool _progressBarShow = false;

        /// <summary>
        /// 文件下载到的本地路径
        /// </summary>
        private string _downloadPath;

        private bool _showFileNoExist=false;


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

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value; 
                RaisePropertyChanged("IsChecked");
            }
        }

        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
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

        public bool IsDirectory
        {
            get { return _isDirectory; }
            set { _isDirectory = value; }
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
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 20); }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
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
                //DownValueStr = downValue + "%";
                RaisePropertyChanged("DownValue");
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

        public bool ProgressBarShow
        {
            get { return _progressBarShow; }
            set
            {
                _progressBarShow = value;
                RaisePropertyChanged("ProgressBarShow");
            }
        }

        public bool ShowFileNoExist
        {
            get { return _showFileNoExist; }
            set
            {
                _showFileNoExist = value;
                RaisePropertyChanged("ShowFileNoExist");
            }
        }
    }
}
