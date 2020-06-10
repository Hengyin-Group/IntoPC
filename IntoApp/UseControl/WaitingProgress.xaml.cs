using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntoApp.UseControl
{
    /// <summary>
    /// WaitingProgress.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingProgress: UserControl
    {
        private Storyboard story;
        public WaitingProgress()
        {
            InitializeComponent();
            this.story = (base.Resources["waiting"] as Storyboard);
        }


        public bool RunState
        {
            get { return (bool)GetValue(RunStateProperty); }
            set { SetValue(RunStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RunState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunStateProperty =
            DependencyProperty.Register("RunState", typeof(bool), typeof(WaitingProgress), new PropertyMetadata(new PropertyChangedCallback(RunStatePropertyChangedCallbask)));

        static void RunStatePropertyChangedCallbask(DependencyObject sender,DependencyPropertyChangedEventArgs e)
        {
            WaitingProgress wp = (sender as WaitingProgress);
            if (wp!=null)
            {
                if (wp.RunState)
                {
                    wp.Start();
                }
                else
                {
                    wp.Stop();
                }
            }
        }

        public void Start()
        {
            if (Visibility==Visibility.Visible)
            {
                return;
            }
            Visibility = Visibility.Visible;
            story.Begin(this.Loading,true); 
        }

        public void Stop()
        {
            base.Dispatcher.BeginInvoke(new Action(() =>
            {
                story.Pause(this.Loading);
                Visibility = Visibility.Collapsed;
            }));
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WaitingProgress), new PropertyMetadata(new PropertyChangedCallback(TextPropertyChangedCallbask)));

        static void TextPropertyChangedCallbask(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WaitingProgress wp=sender as WaitingProgress;
            wp.TextShow.Text = wp.Text;
        }
    }
}
