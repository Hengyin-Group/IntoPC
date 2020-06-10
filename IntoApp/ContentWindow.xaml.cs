using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using IntoApp.Controls;
using IntoApp.utils;
using IntoApp.ViewModel;
using MenuItem = System.Windows.Forms.MenuItem;
using MyMessageBox.Controls;
using MessageBox = MyMessageBox.Controls.MessageBox;

namespace IntoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ContentWindow
    {
        #region 单例模式

        private static ContentWindow ContentWin;
        private static readonly object locker = new object();
        public static ContentWindow GetInstance()
        {
            if (ContentWin==null)
            {
                lock (locker)
                {
                    ContentWin=new ContentWindow();
                }
            }
            return ContentWin;
        }

        #endregion
        private ContentWindow()
        {
            InitializeComponent(); //渲染
            this.DataContext = ViewModelLocator.Win_ContentVM;
            var c= ViewModelLocator.Notification;
        }

       
    }
}
