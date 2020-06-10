using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using MyMessageBox.Common;
using Skin.WPF;

namespace MyMessageBox.Controls
{
    internal sealed class NotifictionModule:SkinSimpleWindow
    {
        static NotifictionModule()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotifictionModule),new FrameworkPropertyMetadata(typeof(NotifictionModule)));
        }

        public NotifictionModule()
        {
            try
            {
                this.AllowsTransparency = true;
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.Topmost = true;
                this.MouseDoubleClick += NotifictionModule_MouseDoubleClick;
                Resources.Source=new Uri(@"MyMessageBox;component/Styles/Notifiction.xaml",UriKind.Relative);
            }
            catch (Exception e)
            {

            }
        }

        private void NotifictionModule_MouseDoubleClick(object sender,MouseButtonEventArgs e)
        {
            DoubleClick(sender,e);
        }

        public delegate object[] NotifyValue(object sender,MouseButtonEventArgs e);

        public event NotifyValue DoubleClick;

        DispatcherTimer timer = new DispatcherTimer();
        public void LoadWindow()
        {
            this.Topmost = Topmost;
            timer.Interval=TimeSpan.FromSeconds(400);
            //timer.Tick+=new EventHandler(dis);
            this.Width = 100;
            this.Height = 100;
            /* 垂直向上弹出
             *
            this.Left = SystemParameters.WorkArea.Width - this.Width - 5;
            this.Top = SystemParameters.WorkArea.Height;
            StopTop = SystemParameters.WorkArea.Height - this.Height + 5;
             */
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


        public new string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }


        public Brush TitleForeground
        {
            get { return (Brush)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }


        public Geometry Image
        {
            get { return (Geometry)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Geometry), typeof(NotifictionModule));

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NotifictionModule), new PropertyMetadata("标题"));

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(NotifictionModule), new PropertyMetadata(""));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(NotifictionModule), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255,255,255))));




        public Brush ImageBrush
        {
            get { return (Brush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ImageBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageBrushProperty =
            DependencyProperty.Register("ImageBrush", typeof(Brush), typeof(NotifictionModule));



        public static void Show(string MessageText)
        {

        }

        public static void Show(string title,string messageText, EnumHelper.NotifictionState state,Geometry geometry,Brush iconBrush)
        {
            var notify=new NotifictionModule();
            notify.Message = messageText;
            notify.Title = title;
            switch (state)
            {
                case EnumHelper.NotifictionState.Success:
                    notify.Image = geometry ?? IconSuccess;
                    notify.ImageBrush=iconBrush?? new SolidColorBrush(Color.FromRgb(43, 162, 69));
                    break;
                case EnumHelper.NotifictionState.Error:
                    notify.Image = geometry ?? IconError;
                    notify.ImageBrush = iconBrush ?? new SolidColorBrush(Color.FromRgb(226, 25, 24));
                    break;
                case EnumHelper.NotifictionState.Info:
                    notify.Image = geometry ?? IconInfo;
                    notify.ImageBrush = iconBrush ?? new SolidColorBrush(Color.FromRgb(194, 195, 201));
                    break;
                default:
                    break;
            }

            var result = notify.ShowDialog();
            
        }

        private static Geometry _iconSuccess = Geometry.Parse("");

        public static Geometry IconSuccess
        {
            get { return _iconSuccess; }
            set { _iconSuccess = value; }
        }

        private static Geometry _iconError = Geometry.Parse("");

        public static Geometry IconError
        {
            get { return _iconError; }
            set { _iconError = value; }
        }

        private static Geometry _iconInfo = Geometry.Parse("");

        public static Geometry IconInfo
        {
            get { return _iconInfo; }
            set { _iconInfo = value; }
        }
    }
}
