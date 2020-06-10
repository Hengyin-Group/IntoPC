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
    /// AboutMe.xaml 的交互逻辑
    /// </summary>
    public partial class WinAboutMe
    {
        #region 创建单例模式

        private static WinAboutMe _winAboutMe = null;
        private static object locker=new object();

        public static WinAboutMe GetInstance
        {
            get
            {
                if (_winAboutMe==null)
                {
                    lock (locker)
                    {
                        _winAboutMe=new WinAboutMe();
                    }
                }

                return _winAboutMe;
            }
        }

        #endregion
        private WinAboutMe()
        {
            InitializeComponent();
        }
    }
}
