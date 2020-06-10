using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseImageCheckBox:CheckBox
    {


        public Geometry Image
        {
            get { return (Geometry)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Geometry), typeof(UseImageCheckBox));



        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(UseImageCheckBox), new PropertyMetadata(15.0));



        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(UseImageCheckBox), new PropertyMetadata(15.0));



        public SolidColorBrush ImageBrush
        {
            get { return (SolidColorBrush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageBrushProperty =
            DependencyProperty.Register("ImageBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,255,255))));





        public Geometry ImageMouseOver
        {
            get { return (Geometry)GetValue(ImageMouseOverProperty); }
            set { SetValue(ImageMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageMouseOverProperty =
            DependencyProperty.Register("ImageMouseOver", typeof(Geometry), typeof(UseImageCheckBox));



        public SolidColorBrush ImageHoverBrush
        {
            get { return (SolidColorBrush)GetValue(ImageHoverBrushProperty); }
            set { SetValue(ImageHoverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHoverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHoverBrushProperty =
            DependencyProperty.Register("ImageHoverBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,255,255))));




        public Geometry ImageIsChecked
        {
            get { return (Geometry)GetValue(ImageIsCheckedProperty); }
            set { SetValue(ImageIsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageIsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageIsCheckedProperty =
            DependencyProperty.Register("ImageIsChecked", typeof(Geometry), typeof(UseImageCheckBox));



        public SolidColorBrush ImageIsCheckedBrush
        {
            get { return (SolidColorBrush)GetValue(ImageIsCheckedBrushProperty); }
            set { SetValue(ImageIsCheckedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageIsCheckedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageIsCheckedBrushProperty =
            DependencyProperty.Register("ImageIsCheckedBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,255,255))));



        public SolidColorBrush TextBrush
        {
            get { return (SolidColorBrush)GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBrushProperty =
            DependencyProperty.Register("TextBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,0,0,0))));



        public SolidColorBrush HoverTextBrush
        {
            get { return (SolidColorBrush)GetValue(HoverTextBrushProperty); }
            set { SetValue(HoverTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverTextBrushProperty =
            DependencyProperty.Register("HoverTextBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))));



        public SolidColorBrush IsCheckedTextBrush
        {
            get { return (SolidColorBrush)GetValue(IsCheckedTextBrushProperty); }
            set { SetValue(IsCheckedTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckedTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedTextBrushProperty =
            DependencyProperty.Register("IsCheckedTextBrush", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))));




        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public SolidColorBrush HoverBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(HoverBackgroundColorProperty); }
            set { SetValue(HoverBackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBackgroundColorProperty =
            DependencyProperty.Register("HoverBackgroundColor", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public SolidColorBrush IsCheckedBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(IsCheckedBackgroundColorProperty); }
            set { SetValue(IsCheckedBackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckedBackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedBackgroundColorProperty =
            DependencyProperty.Register("IsCheckedBackgroundColor", typeof(SolidColorBrush), typeof(UseImageCheckBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

    }
}
