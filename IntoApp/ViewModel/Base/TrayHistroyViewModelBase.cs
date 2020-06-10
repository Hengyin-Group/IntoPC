using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using IntoApp.Common;
using IntoApp.Model;
using Skin.WPF.Command;

namespace IntoApp.ViewModel.Base
{
    public class TrayHistroyViewModelBase:ViewModelBase
    {
        private ObservableCollection<TrayHistory> _trayHistoryUploaded;
        private ObservableCollection<TrayHistory> _trayHistoryDownloaded;
        private ObservableCollection<TrayHistory> _trayHistoryUnDownload;
        public ObservableCollection<TrayHistory> Uploaded
        {
            get
            {
                if (_trayHistoryUploaded==null)
                {
                    _trayHistoryUploaded=new ObservableCollection<TrayHistory>();
                }
                return _trayHistoryUploaded;
            }
            set
            {
                _trayHistoryUploaded = value;
                RaisePropertyChanged("Uploaded");
            }
        }

        public ObservableCollection<TrayHistory> Downloaded
        {
            get
            {
                if (_trayHistoryDownloaded==null)
                {
                    _trayHistoryDownloaded=new ObservableCollection<TrayHistory>();
                }
                return _trayHistoryDownloaded;
            }
            set
            {
                _trayHistoryDownloaded = value;
                RaisePropertyChanged("Downloaded");
            }
        }

        public ObservableCollection<TrayHistory> UnDownload
        {
            get
            {
                if (_trayHistoryUnDownload==null)
                {
                    _trayHistoryUnDownload=new ObservableCollection<TrayHistory>();
                }
                return _trayHistoryUnDownload;
            }
            set
            {
                _trayHistoryUnDownload = value;
                RaisePropertyChanged("UnDownload");
            }
        }

        #region 命令

        public MyCommand LoadedCommand { get; set; }
        public MyCommand<Object[]> DeleteCommand { get; set; }
        public MyCommand<Object[]> SelectAllCommand { get; set; }
        public MyCommand<Object[]> CheckedCommand { get; set; }

        #endregion

        #region 方法
        /// <summary>
        /// CheckBox选中
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="ch"></param>
        public void Check(ObservableCollection<TrayHistory> collection,object[] x)
        {
            CheckBox ch = x[0] as CheckBox;
            int Index = JObjectHelper.GetStrNum(ch.Tag.ToString());
            collection[Index - 1].IsChecked = (bool)ch.IsChecked;
            SelectChecked(collection);
            SelectCheckedAll(collection);
        }

        /// <summary>
        /// 全部选中或全部取消
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="bo"></param>
        public void SelectAll(ObservableCollection<TrayHistory> collection,object[] x)
        {
            bool bo = (bool) (x[0] as CheckBox).IsChecked;
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].IsChecked != bo)
                {
                    collection[i].IsChecked = bo;
                }
            }
            SelectChecked(collection);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collection"></param>
        public void Delete(ObservableCollection<TrayHistory> collection)
        {
            for (int i = collection.Count; i > 0; i--)
            {
                if (collection[i - 1].IsChecked)
                {
                    collection.RemoveAt(i - 1);
                }
            }
            Sort(collection);
            SelectAllIsChecked = false;
            SelectChecked(collection);
            emptyShow(collection);
        }

        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="collection"></param>
        public void Sort(ObservableCollection<TrayHistory> collection)
        {
            int count = Uploaded.Count;
            for (int i = 0; i < count; i++)
            {
                collection[i].Index = i + 1;
            }
        }

        /// <summary>
        /// 一键删除的可用性
        /// </summary>
        /// <param name="collection"></param>
        public void SelectChecked(ObservableCollection<TrayHistory> collection)
        {
            IsEnabled = collection.Any(p => p.IsChecked);
        }
        /// <summary>
        /// 全选框是否需要选中
        /// </summary>
        /// <param name="collection"></param>
        public void SelectCheckedAll(ObservableCollection<TrayHistory> collection)
        {
            SelectAllIsChecked = collection.All(p => p.IsChecked);
        }
        /// <summary>
        /// 内容为空控件是否显示
        /// </summary>
        /// <param name="collection"></param>
        public void emptyShow(ObservableCollection<TrayHistory> collection)
        {
            EmptyIsShow = collection.Count < 1;
        }

        #endregion

        #region 属性

        private bool _runState = true;
        private bool _isEnabled = false;
        private bool _emptyIsShow = true;
        private bool _selectAllIsChecked = false;
        public bool EmptyIsShow
        {
            get { return _emptyIsShow; }
            set
            {
                _emptyIsShow = value;
                RaisePropertyChanged("EmptyIsShow");
            }
        }

        public bool RunState
        {
            get { return _runState; }
            set
            {
                _runState = value;
                RaisePropertyChanged("RunState");
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        public bool SelectAllIsChecked
        {
            get { return _selectAllIsChecked; }
            set
            {
                _selectAllIsChecked = value;
                RaisePropertyChanged("SelectAllIsChecked");
            }
        }



        #endregion


    }
}
