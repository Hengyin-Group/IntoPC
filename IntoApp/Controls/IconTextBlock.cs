using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntoApp.Controls
{
    public class IconTextBlock:Label
    {

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(IconTextBlock), new PropertyMetadata(null));

        //是否显示图标
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(IconTextBlock), new PropertyMetadata(null));



        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(IconTextBlock), new PropertyMetadata(12.0));



        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(IconTextBlock), new PropertyMetadata(12.0));



        //文字
        public string TextBlackText
        {
            get { return (string)GetValue(TextBlackTextProperty); }
            set { SetValue(TextBlackTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBlackText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlackTextProperty =
            DependencyProperty.Register("TextBlackText", typeof(string), typeof(IconTextBlock), new PropertyMetadata(null));

        //文字颜色
        public SolidColorBrush TextColorBrush
        {
            get { return (SolidColorBrush)GetValue(TextColorBrushProperty); }
            set { SetValue(TextColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextColorBrushProperty =
            DependencyProperty.Register("TextColorBrush", typeof(SolidColorBrush), typeof(IconTextBlock), new PropertyMetadata(null));


    }
}
