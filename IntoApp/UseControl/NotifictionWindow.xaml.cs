using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using IntoApp.ViewModel.Other;

namespace IntoApp.UseControl
{
    /// <summary>
    /// NotifictionWindowxaml.xaml 的交互逻辑
    /// </summary>
    public partial class NotifictionWindow
    {
        public NotifictionWindow(string str)
        {
            InitializeComponent();
            this.DataContext = new WinNotifiyViewModel(str);
            LoadWindow();
        }
        DispatcherTimer timer = new DispatcherTimer();
        public void LoadWindow()
        {
            this.Topmost = true;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick+=new EventHandler(dispatcherTimer_Tick);
            this.Width = 250;
            this.Height = 100;
            // 垂直向上弹出

            //this.Left = SystemParameters.WorkArea.Width - this.Width - 5;
            //this.Top = SystemParameters.WorkArea.Height;
            //StopTop = SystemParameters.WorkArea.Height - this.Height + 5;

            /*水平向左弹出*/
            this.Left = SystemParameters.WorkArea.Width;
            this.Top = SystemParameters.WorkArea.Height - this.Height;
            StopLeft = SystemParameters.WorkArea.Width - this.Width;
            timer.Start();
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                //do something
            });
        }

        public double StopTop { get; set; }
        public double StopLeft { get; set; }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                if (this.Left > StopLeft)
                {
                    this.Left -= 0.0573;
                }
                else
                {
                    timer.Stop();
                    StartCloseTimer(true,10);
                    break;
                }
            }
        }
        public void StartCloseTimer(Boolean Topmost, int timeout)
        {
            this.Topmost = Topmost;
            //this.LabelStr.Content = lab;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(timeout)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            //this.DialogResult = false;
            this.Close();
        }
    }
}
