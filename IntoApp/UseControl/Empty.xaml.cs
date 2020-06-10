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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntoApp.UseControl
{
    /// <summary>
    /// Empty.xaml 的交互逻辑
    /// </summary>
    public partial class Empty : UserControl
    {
        public Empty()
        {
            InitializeComponent();
        }


        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(Empty), new PropertyMetadata(new PropertyChangedCallback(IsShowPropertyChangedCallback)));

        static void IsShowPropertyChangedCallback(DependencyObject sender,DependencyPropertyChangedEventArgs e)
        {
            Empty em=sender as Empty;
            if (em!=null)
            {
                if (em.IsShow)
                {
                    em.Visibility = Visibility.Visible;
                }
                else
                {
                    em.Visibility = Visibility.Hidden;
                }
            }
        }


    }
}
