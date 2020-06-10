using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.AutoUpdate.Model;
using IntoApp.AutoUpdate.utils;
using IntoApp.AutoUpdate.ViewModel.Base;
using Skin.WPF.Command;

namespace IntoApp.AutoUpdate.ViewModel
{
    public class Page_DecompressionViewModel:DecompressionModel
    {
        private bool _isFirstLoad = true;

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);

        public bool IsFirstLoad
        {
            get { return _isFirstLoad; }
            set { _isFirstLoad = value; }
        }

        #region 页面初始化
        public Page_DecompressionViewModel()
        {
            //LoadCommand=new MyCommand(x=>UnZip());
            UnZip();
        }

        #endregion

        public MyCommand LoadCommand { get; set; }

        #region 方法

        void UnZip()
        {
            if (_decompression==null)
            {
                _decompression=new DecompressionModel();
            }
            string strPath = UpdateModel.LocalFilePath;
            string strZipPath = UpdateModel.UnpackPath;
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                ZipHelper zipHelper = new ZipHelper();
                zipHelper.GetBarValue += value =>
                {
                    DpValue = value;
                    if (value==100)
                        OpenIntoApp();
                };
                zipHelper.ExportZip(Path.Combine(strPath,UpdateModel.FileName),strZipPath);
            });
        }

        void OpenIntoApp()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                //ProcessStartInfo psi = new ProcessStartInfo();
                ////Process ps=new Process();
                //psi.FileName = UpdateModel.IntoAppPath;
                //psi.UseShellExecute = false;
                //psi.RedirectStandardError = true;
                //Process.Start("runas.exe",$"/trustlevel:0x20000 {UpdateModel.IntoAppPath}");
                Process.Start("explorer.exe", UpdateModel.IntoAppPath);
                //Application.Current.Shutdown();
                //ShellExecute(IntPtr.Zero, "open", "cmd","/c "+UpdateModel.IntoAppPath, "", 0);
                Environment.Exit(0);
            });
            
        }

        #endregion

        private DecompressionModel _decompression;

        #region 属性

        public double DpValue
        {
            get { return _decompression.DpValue; }
            set
            {
                _decompression.DpValue = value;
                DpValueStr = _decompression.DpValue + "%";
                RaisePropertyChanged("DpValue");
            }
        }

        public string DpValueStr
        {
            get { return _decompression.DpValueStr; }
            set
            {
                _decompression.DpValueStr = value;
                RaisePropertyChanged("DpValueStr");
            }
        }


        #endregion

    }
}
