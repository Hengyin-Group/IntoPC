using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntoApp.View.Content.Setting
{
    /// <summary>
    /// WinVersionInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WinVersionInfo
    {
        #region 单例
        private static WinVersionInfo _winVersionInfo = null;
        private static object locker = new object();

        public static WinVersionInfo GetInstance
        {
            get
            {
                if (_winVersionInfo == null)
                {
                    lock (locker)
                    {
                        return _winVersionInfo = new WinVersionInfo();
                    }
                }

                return _winVersionInfo;
            }
        }


        #endregion

        private WinVersionInfo()
        {
            InitializeComponent();
        }
    }
}
