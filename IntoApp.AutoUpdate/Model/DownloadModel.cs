using IntoApp.AutoUpdate.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntoApp.AutoUpdate.Model
{
    public class DownloadModel : ViewModelBase
    {
        private string _appID;
        private string _currentVersion; //当前版本
        private string _version; //要更新的版本
        private string _downUrl; //下载路径
        private string _localUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download"); //本地保存地址
        private double _downValue; //下载内容长度
        private string _downStr; //下载百分比
        private string _fileName; //文件名

        public string AppId
        {
            get { return _appID; }
            set
            {
                _appID = value;
            }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string CurrentVersion
        {
            get { return _currentVersion; }
            set { _currentVersion = value; }
        }

        public string DownUrl
        {
            get { return _downUrl; }
            set { _downUrl = value; }
        }

        public string LocalUrl
        {
            get { return _localUrl; }
            set { _localUrl = value; }
        }

        public double DownValue
        {
            get { return _downValue; }
            set
            {
                _downValue = value;
            }
        }

        public string DownStr
        {
            get { return _downStr; }
            set
            {
                _downStr = value;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
    }
}
