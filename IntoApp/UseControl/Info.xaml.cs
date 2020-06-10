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
using Skin.WPF.Controls;

namespace IntoApp.UseControl
{
    /// <summary>
    /// Info.xaml 的交互逻辑
    /// </summary>
    public partial class Info : UserControl
    {
        public Info()
        {
            InitializeComponent();
        }

        public Geometry ImagePath
        {
            get { return (Geometry)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(Geometry), typeof(Info),new PropertyMetadata(ImagePathPropertyChangedCallbask));

        static void ImagePathPropertyChangedCallbask(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Info info=(sender) as Info;
            if (info!=null)
            {
                info.ImagePathUserControls.Image = info.ImagePath;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Info), new PropertyMetadata(TextPropertyChangedCallBack));

        static void TextPropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Info info=(sender) as Info;
            info.TextShow.Text = info.Text;
        }



        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(bool), typeof(Info), new PropertyMetadata(IsShowPropertyChangedCallBack));

        static void IsShowPropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Info info=sender as Info;
            if (info!=null)
            {
                if (info.IsShow)
                {
                    info.Visibility = Visibility.Visible;
                }
                else
                {
                    info.Visibility = Visibility.Hidden;
                }
            }
        }



    }
}
