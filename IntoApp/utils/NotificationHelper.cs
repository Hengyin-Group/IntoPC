using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using IntoApp.ViewModel;

namespace IntoApp.utils
{
    public class NotificationHelper
    {
        //private static NotifyIcon notifyIcon;
        ///// <summary>
        ///// 通知（静态方法）
        ///// </summary>
        ///// <param name="time"></param>
        ///// <param name="title"></param>
        ///// <param name="msg"></param>
        ///// <param name="icon"></param>
        //public static void NotifyIconMessage(int time,string title,string msg,ToolTipIcon icon)
        //{
        //    notifyIcon=new NotifyIcon();
        //    notifyIcon.ShowBalloonTip(time,title,msg,icon);
        //    //notifyIcon.ShowBalloonTip(1000, "提示", "印兔已打开", ToolTipIcon.Info);
        //}

        #region 单例模式

        private static NotificationHelper Notify;

        private static readonly object locker=new object();

        public static NotificationHelper GetInstance()
        {
            if (Notify == null)
            {
                lock (locker)
                {
                    Notify=new NotificationHelper();
                }
            }

            return Notify;
        }

        #endregion

        #region 页面初始化
        private static NotifyIcon notifyIcon;

        private NotificationHelper()
        {
            
            Init_NotifyIcon();
            //notifyIcon.ShowBalloonTip();
        }
        //计时器
        DispatcherTimer notifybar_timer;
        //鼠标是否在通知栏内变量
        //bool NotifyBar_IsMouseEnter = false;
        private void Init_NotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("Contents/image/IntoApp.ico", UriKind.Relative)).Stream),
                Text = "印兔",
                Visible = true
            };
            //notifyIcon.ShowBalloonTip(1000,"111","2222",ToolTipIcon.Info);
            MenuItem m1=new MenuItem("打开主窗体");
            m1.Click += M1_Click;
            MenuItem m2=new MenuItem("退出");
            m2.Click += M2_Click;
            MenuItem[] m=new MenuItem[]{m1,m2};
            notifyIcon.ContextMenu = new ContextMenu(m);
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                var Win = ViewModelLocator.Win_Content;
                if (Win.WindowState == WindowState.Minimized)
                {
                    Win.WindowState = WindowState.Normal;
                    Win.Show();
                    Win.ShowInTaskbar = true;
                    Win.Activate();
                }
                else
                {
                    Win.Activate();
                }
            }
        }

        private void M1_Click(object sender, EventArgs e)
        {
            var Win = ViewModelLocator.Win_Content;
            if (Win.WindowState == WindowState.Minimized)
            {
                Win.WindowState = WindowState.Normal;
                Win.Show();
                Win.ShowInTaskbar = true;
                Win.Activate();
            }
            else
            {
                Win.Activate();
            }

        }

        private void M2_Click(object sender, EventArgs e)
        {
            var Win = ViewModelLocator.Win_Content;
            notifyIcon.Dispose();
            Environment.Exit(0);
        }

        public void NotifyIconMessage(int time, string title, string msg, ToolTipIcon icon)
        {
            notifyIcon.ShowBalloonTip(time,title,msg,icon);
        }

        #endregion
    }
}
