using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntoApp.Printer.Utils
{
    public class NotificationHelper
    {
        private static NotificationHelper _Notify = null;
        private static object locker=new object();

        public static NotificationHelper GetInstance
        {
            get
            {
                if (_Notify==null)
                {
                    lock (locker)
                    {
                        return _Notify = new NotificationHelper();
                    }
                }
                return _Notify;
            }
        }

        private NotificationHelper()
        {
            Init_NotifyIcon();
        }

        static NotifyIcon notifyIcon;  //托盘图标

        void Init_NotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon(
                System.Windows.Application.GetResourceStream(
                    new Uri("../Image/Printer_Ico.ico", UriKind.Relative)).Stream);//加载图标
            notifyIcon.Text = "印兔打印机";
            notifyIcon.Visible = true;

            //鼠标右击事件
            MenuItem m1 = new MenuItem("退出");
            m1.Click += m1_click;
            MenuItem[] m = new MenuItem[] { m1 };
            notifyIcon.ContextMenu = new ContextMenu(m);

        }

        void m1_click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public static void DisposeNotify()
        {
            notifyIcon.Dispose();
        }


    }
}
