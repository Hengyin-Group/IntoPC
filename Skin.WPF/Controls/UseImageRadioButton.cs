using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseImageRadioButton : RadioButton
    {
        public Geometry Image
        {
            get { return (Geometry)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Geometry), typeof(UseImageRadioButton));


        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(UseImageRadioButton), new PropertyMetadata(15.0));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(UseImageRadioButton), new PropertyMetadata(15.0));




        /// <summary>
        /// 图标的颜色
        /// </summary>
        public SolidColorBrush ImageBrush
        {
            get { return (SolidColorBrush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageBrushProperty =
            DependencyProperty.Register("ImageBrush", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,255,255))));



        public Geometry ImageMouseOver
        {
            get { return (Geometry)GetValue(ImageMouseOverProperty); }
            set { SetValue(ImageMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageMouseOverProperty =
            DependencyProperty.Register("ImageMouseOver", typeof(Geometry), typeof(UseImageRadioButton));



        public SolidColorBrush ImageHoverBrush
        {   
            get { return (SolidColorBrush)GetValue(ImageHoverBrushProperty); }
            set { SetValue(ImageHoverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHoverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHoverBrushProperty =
            DependencyProperty.Register("ImageHoverBrush", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));



        public Geometry ImageIsChecked
        {
            get { return (Geometry)GetValue(ImageIsCheckedProperty); }
            set { SetValue(ImageIsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageIsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageIsCheckedProperty =
            DependencyProperty.Register("ImageIsChecked", typeof(Geometry), typeof(UseImageRadioButton));



        public SolidColorBrush ImageIsCheckedBrush
        {
            get { return (SolidColorBrush)GetValue(ImageIsCheckedBrushProperty); }
            set { SetValue(ImageIsCheckedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageIsCheckedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageIsCheckedBrushProperty =
            DependencyProperty.Register("ImageIsCheckedBrush", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));



        public SolidColorBrush BackGroundColor
        {
            get { return (SolidColorBrush)GetValue(BackGroundColorProperty); }
            set { SetValue(BackGroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackGroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackGroundColorProperty =
            DependencyProperty.Register("BackGroundColor", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public SolidColorBrush HoverBackGroundColor
        {
            get { return (SolidColorBrush)GetValue(HoverBackGroundColorProperty); }
            set { SetValue(HoverBackGroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBackGroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBackGroundColorProperty =
            DependencyProperty.Register("HoverBackGroundColor", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public SolidColorBrush IsCheckedBackGroundColor
        {
            get { return (SolidColorBrush)GetValue(IsCheckedBackGroundColorProperty); }
            set { SetValue(IsCheckedBackGroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckedBackGroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedBackGroundColorProperty =
            DependencyProperty.Register("IsCheckedBackGroundColor", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));


        public SolidColorBrush HoverTextBrush
        {
            get { return (SolidColorBrush)GetValue(HoverTextBrushProperty); }
            set { SetValue(HoverTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverTextBrushProperty =
            DependencyProperty.Register("HoverTextBrush", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))));




        public SolidColorBrush IsCheckedTextBrush
        {
            get { return (SolidColorBrush)GetValue(IsCheckedTextBrushProperty); }
            set { SetValue(IsCheckedTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckedTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedTextBrushProperty =
            DependencyProperty.Register("IsCheckedTextBrush", typeof(SolidColorBrush), typeof(UseImageRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))));





        public Thickness Border_Thickness
        {
            get { return (Thickness)GetValue(Border_ThicknessProperty); }
            set { SetValue(Border_ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Border_Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Border_ThicknessProperty =
            DependencyProperty.Register("Border_Thickness", typeof(Thickness), typeof(UseImageRadioButton), new PropertyMetadata(new Thickness(0)));





    }
}
