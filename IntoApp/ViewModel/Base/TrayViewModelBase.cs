using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using IntoApp.Model;
using Skin.WPF.Command;

namespace IntoApp.ViewModel.Base
{
    public class TrayViewModelBase:Tray
    {
        private ObservableCollection<Tray> _tray;
        public ObservableCollection<Tray> Tray
        {
            get
            {
                if (_tray==null)
                {
                    _tray=new ObservableCollection<Tray>();
                }

                return _tray;
            }
            set
            {
                _tray = value;
                RaisePropertyChanged("Tray");
            }
        }
    }

    public class LocalTrayViewModelBase : LocalTray
    {
        private ObservableCollection<LocalTray> _localTray;

        public ObservableCollection<LocalTray> LocalTray
        {
            get
            {
                if (_localTray==null)
                {
                    _localTray=new ObservableCollection<LocalTray>();
                }
                return _localTray;
            }
            set
            {
                _localTray = value;
                RaisePropertyChanged("LocalTray");
            }
        }

        public ObservableCollection<LocalTray> _fileUpload;
        public ObservableCollection<LocalTray> FileUpload
        {
            get
            {
                if (_fileUpload==null)
                {
                    _fileUpload=new ObservableCollection<LocalTray>();
                }
                return _fileUpload;
            }
            set
            {
                _fileUpload = value;
                RaisePropertyChanged("FileUpload");
            }
        }

        public delegate void UploadUrl(List<string> list);

        //private UploadUrl UploadFileEvent;
        public event UploadUrl UploadFile;
        //{
        //    add
        //    {
        //        if (UploadFileEvent==null)
        //        {
        //            UploadFileEvent += value;
        //        }

        //    }
        //    remove { UploadFileEvent -= value; }
        //}

        #region 命令

        public MyCommand LoadCommand { get; set; }
        public MyCommand<ExCommandParameter> DropCommand { get; set; }
        public MyCommand<Object[]> SelectAllCommand { get; set; }
        public MyCommand<Object[]> CheckedCommand { get; set; }
        public MyCommand<Object[]> DeleteCommand { get; set; }
        public MyCommand<DataGrid> SelectionChangedCommand { get; set; }
        public MyCommand UploadCommand { get; set; }

        #endregion

        #region 方法
        public void DropDown(ExCommandParameter x, ObservableCollection<LocalTray> local)
        {
            DragEventArgs e = x.EventArgs as DragEventArgs;
            //拖进来的是文件类型
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                int m = local.Count > 0 ? local[local.Count - 1].Index : 0;
                int _index = 0;
                for (int i = 0; i < paths.Length; i++)
                {
                    string path = paths[i];
                    if (path.Length > 1)
                    {
                        FileInfo file = new FileInfo(path);
                        if (file.Attributes == FileAttributes.Directory)
                            e.Effects = DragDropEffects.Link;
                        else
                        {
                            e.Effects = DragDropEffects.None;

                            local.Add(new LocalTray()
                            {
                                Index = m + _index + 1,
                                FileUrl = path,
                                FileName = System.IO.Path.GetFileName(path),
                                CreateDate = DateTime.Now,
                                IsDirectory = false,
                            });
                            _index++;
                        }
                    }
                    else
                        e.Effects = DragDropEffects.None;
                }
            }
            EmptyIsShow = false;
            SelectCheckedAll(local);
        }

        public void SelectCheckedAll(ObservableCollection<LocalTray> local)
        {
            SelectAllIsChecked = local.All(p => p.IsChecked == true);
        }

        public void SelectAll(Object[] x,ObservableCollection<LocalTray> local)
        {
            bool bo = (bool)(x[0] as CheckBox).IsChecked;

            for (int i = 0; i < LocalTray.Count; i++)
            {
                local[i].IsChecked = bo;
            }
            SelectChecked(local);
        }

        public void Checked(Object[] x,ObservableCollection<LocalTray> local)
        {
            CheckBox ch = x[0] as CheckBox;
            int Index = Common.JObjectHelper.GetStrNum(ch.Tag.ToString());
            local[Index - 1].IsChecked = (bool)ch.IsChecked;
            SelectChecked(local);
            SelectCheckedAll(local);
        }

        public void selectionChanged(DataGrid x)
        {
            var trayInfo = (LocalTray)x.SelectedItems[0];
        }

        public void SelectChecked(ObservableCollection<LocalTray> local)
        {
            IsEnabled = local.Any(p => p.IsChecked == true);
        }

        public void Delete(ObservableCollection<LocalTray> local)
        {
            for (int i = local.Count(); i > 0; i--)
            {
                bool bo = local[i - 1].IsChecked;
                if (bo)
                    local.Remove(local[i-1]);
                    //local.RemoveAt(i - 1);
            }
            Sort(local);
            SelectAllIsChecked = false;
            SelectChecked(local);
            emptyShow(local);
        }
        public void Sort(ObservableCollection<LocalTray> local)
        {
            for (int i = 0; i < local.Count; i++)
            {
                local[i].Index = i + 1;
            }
        }

        public void emptyShow(ObservableCollection<LocalTray> local)
        {
            EmptyIsShow = local.Count < 1;
        }

        public void Upload(ObservableCollection<LocalTray> local)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < local.Count; i++)
            {
                if (local[i].IsChecked)
                {
                    list.Add(local[i].FileUrl);
                }
            }
            UploadFile(list);
        }


        #endregion

        #region 属性

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        private bool _selectAllIsChecked = false;
        public bool SelectAllIsChecked
        {
            get { return _selectAllIsChecked; }
            set
            {
                _selectAllIsChecked = value;
                RaisePropertyChanged("SelectAllIsChecked");
            }
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        private bool _contentShow = true;
        public bool ContentShow
        {
            get { return _contentShow; }
            set
            {
                _contentShow = value;
                RaisePropertyChanged("ContentShow");
            }
        }

        #endregion

    }
}
