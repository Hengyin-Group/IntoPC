using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Skin.WPF.Controls
{
    public class CropingImgEx : Control
    {
        static CropingImgEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CropingImgEx), new FrameworkPropertyMetadata(typeof(CropingImgEx)));
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                s_dpiX = graphics.DpiX;
                s_dpiY = graphics.DpiY;
            }
        }


        public CropingImgEx()
        {
            this.Unloaded += CropingImgEx_Unloaded;
        }




        public double DragControlLength
        {
            get { return (double)GetValue(DragControlLengthProperty); }
            set { SetValue(DragControlLengthProperty, value); }
        }

        public static readonly DependencyProperty DragControlLengthProperty =
            DependencyProperty.Register("DragControlLength", typeof(double), typeof(CropingImgEx), new PropertyMetadata(null));



        public double MinControlLength
        {
            get { return (double)GetValue(MinControlLengthProperty); }
            set { SetValue(MinControlLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinControlLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinControlLengthProperty =
            DependencyProperty.Register("MinControlLength", typeof(double), typeof(CropingImgEx), new PropertyMetadata(null));



        public Brush MarkColor
        {
            get { return (Brush)GetValue(MarkColorProperty); }
            set { SetValue(MarkColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkColorProperty =
            DependencyProperty.Register("MarkColor", typeof(Brush), typeof(CropingImgEx), new PropertyMetadata(null));



        public BitmapSource CropingSource
        {
            get { return (BitmapSource)GetValue(CropingSourceProperty); }
            set { SetValue(CropingSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CropingSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CropingSourceProperty =
            DependencyProperty.Register("CropingSource", typeof(BitmapSource), typeof(CropingImgEx), new PropertyMetadata(null));

        public BitmapSource Source
        {
            get { return (BitmapSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }

        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(BitmapSource), typeof(CropingImgEx), new PropertyMetadata(new PropertyChangedCallback(
                OnSourceChanged)));

        public static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CropingImgEx img = sender as CropingImgEx;
            if (e.NewValue != null)
            {
                BitmapSource source = e.NewValue as BitmapSource;

                if (img.imgSource != null)
                {
                    Size targetSize = img.Preview(source.PixelWidth, source.PixelHeight, 300, 300);

                    img.Width = targetSize.Width;
                    img.Height = targetSize.Height;

                    img.DragControlLength = Math.Min(targetSize.Width, targetSize.Height);
                    Canvas.SetTop(img.dragBorder, 0);
                    Canvas.SetLeft(img.dragBorder, 0);
                    img.SetMark(0, 0);
                    img.SetCropingSource();
                }
            }
        }

        Size Preview(double sourceWidht, double sourceHeight, double maxWidth, double maxHeight)
        {

            scaleX = maxWidth / sourceWidht;
            scaleY = maxHeight / sourceHeight;

            Size result;
            if (scaleX < scaleY)
            {
                result = new Size(maxWidth, (int)(sourceHeight * scaleX));
            }
            else if (scaleX > scaleY)
            {
                result = new Size((int)(sourceWidht * scaleY), maxHeight);
            }
            else
            {
                result = new Size(maxWidth, maxHeight);
            }
            return result;
        }




        private Thumb moveThumb;
        private Thumb thumbTopLeft;
        private Thumb thumbTopRight;
        private Thumb thumbBottomLeft;
        private Thumb thumbBottomRight;
        private FrameworkElement dragBorder;

        private Rectangle markTop;
        private Rectangle markLeft;
        private Rectangle markRight;
        private Rectangle markBottom;

        private Image imgSource;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            moveThumb = GetTemplateChild("moveThumb") as Thumb;

            thumbTopLeft = GetTemplateChild("thumbTopLeft") as Thumb;
            thumbTopRight = GetTemplateChild("thumbTopRight") as Thumb;
            thumbBottomLeft = GetTemplateChild("thumbBottomLeft") as Thumb;
            thumbBottomRight = GetTemplateChild("thumbBottomRight") as Thumb;
            dragBorder = GetTemplateChild("drogBorder") as FrameworkElement;

            markTop = GetTemplateChild("topRec") as Rectangle;
            markLeft = GetTemplateChild("leftRec") as Rectangle;
            markRight = GetTemplateChild("rightRec") as Rectangle;
            markBottom = GetTemplateChild("bottomRec") as Rectangle;

            imgSource = GetTemplateChild("imgSource") as Image;
            Canvas.SetTop(dragBorder, (Width - DragControlLength) / 2);
            Canvas.SetLeft(dragBorder, (Height - DragControlLength) / 2);

            moveThumb.DragDelta += moveThumb_DragDelta;

            thumbTopLeft.DragDelta += thumbTopLeft_DragDelta;
            thumbTopRight.DragDelta += thumbTopRight_DragDelta;
            thumbBottomLeft.DragDelta += thumbBottomLeft_DragDelta;
            thumbBottomRight.DragDelta += thumbBottomRight_DragDelta;

            thumbTopLeft.DragCompleted += thumbBottomLeft_DragCompleted;
            thumbTopRight.DragCompleted += thumbBottomLeft_DragCompleted;
            thumbBottomLeft.DragCompleted += thumbBottomLeft_DragCompleted;
            thumbBottomRight.DragCompleted += thumbBottomLeft_DragCompleted;

            SetCropingSource();
            SetMark((Width - DragControlLength) / 2, (Height - DragControlLength) / 2);
        }

        void CropingImgEx_Unloaded(object sender, RoutedEventArgs e)
        {
            moveThumb.DragDelta -= moveThumb_DragDelta;
            thumbTopLeft.DragDelta -= thumbTopLeft_DragDelta;
            thumbTopRight.DragDelta -= thumbTopRight_DragDelta;
            thumbBottomLeft.DragDelta -= thumbBottomLeft_DragDelta;
            thumbBottomRight.DragDelta -= thumbBottomRight_DragDelta;

            thumbBottomLeft.DragCompleted -= thumbBottomLeft_DragCompleted;
            thumbTopRight.DragCompleted -= thumbBottomLeft_DragCompleted;
            thumbBottomLeft.DragCompleted -= thumbBottomLeft_DragCompleted;
            thumbBottomRight.DragCompleted -= thumbBottomLeft_DragCompleted;
        }



        void thumbBottomLeft_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            SetCropingSource();
        }



        void thumbBottomRight_DragDelta(object sender, DragDeltaEventArgs e)
        {

            if (e.HorizontalChange == 0)
            {
                return;
            }
            double width = DragControlLength + e.HorizontalChange;

            double top = Canvas.GetTop(dragBorder);

            double left = Canvas.GetLeft(dragBorder);
            Console.WriteLine("Resize Left：" + Canvas.GetLeft(dragBorder) + "\tResize Top：" + Canvas.GetTop(dragBorder));
            if (left + width > Width || width + top > Height || width <= MinControlLength)
            {
                return;
            }

            DragControlLength = width;
            SetMark(top, left);
            e.Handled = true;
        }

        void thumbBottomLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {

            if (e.HorizontalChange == 0)
            {
                return;
            }

            double width = DragControlLength - e.HorizontalChange;

            double left = Canvas.GetLeft(dragBorder) + e.HorizontalChange;

            double top = Canvas.GetTop(dragBorder);
            Console.WriteLine("Resize Left：" + Canvas.GetLeft(dragBorder) + "\tResize Top：" + Canvas.GetTop(dragBorder));
            if (left < 0 || width <= MinControlLength || top + width > Height)
            {
                return;
            }

            Canvas.SetLeft(dragBorder, left);

            DragControlLength = width;
            SetMark(top, left);
            e.Handled = true;
        }

        void thumbTopRight_DragDelta(object sender, DragDeltaEventArgs e)
        {

            if (e.HorizontalChange == 0)
            {
                return;
            }
            double width = DragControlLength + e.HorizontalChange;

            double top = Canvas.GetTop(dragBorder) - e.HorizontalChange;

            double left = Canvas.GetLeft(dragBorder);
            Console.WriteLine("Resize Left：" + Canvas.GetLeft(dragBorder) + "\tResize Top：" + Canvas.GetTop(dragBorder));
            if (top < 0 || left + width >= Width || width < MinControlLength)
            {
                return;
            }

            Canvas.SetTop(dragBorder, top);

            DragControlLength = width;
            SetMark(top, left);
            e.Handled = true;
        }

        void thumbTopLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.HorizontalChange == 0)
            {
                return;
            }

            double width = DragControlLength - e.HorizontalChange;

            double top = Canvas.GetTop(dragBorder) + e.HorizontalChange;

            double left = Canvas.GetLeft(dragBorder) + e.HorizontalChange;

            Console.WriteLine("Resize Left：" + Canvas.GetLeft(dragBorder) + "\tResize Top：" + Canvas.GetTop(dragBorder));
            if (top < 0 || width <= MinControlLength || left < 0)
            {
                return;
            }

            Canvas.SetTop(dragBorder, top);

            Canvas.SetLeft(dragBorder, left);

            DragControlLength = width;
            SetMark(top, left);
            e.Handled = true;
        }

        void moveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double top = Canvas.GetTop(dragBorder) + e.VerticalChange;
            double left = Canvas.GetLeft(dragBorder) + e.HorizontalChange;
            if (top <= 0)
                top = 0;
            if (top >= (Height - DragControlLength))
                top = Height - DragControlLength;
            if (left <= 0)
                left = 0;
            if (left >= (Width - DragControlLength))
                left = Width - DragControlLength;
            Canvas.SetTop(dragBorder, top);
            Canvas.SetLeft(dragBorder, left);
            SetCropingSource();
            SetMark(top, left);
            Console.WriteLine("Move Left：" + Canvas.GetLeft(dragBorder) + "\tMove Top：" + Canvas.GetTop(dragBorder));
        }

        private static float s_dpiX;
        private static float s_dpiY;
        private double scaleX; // 缩放比例
        private double scaleY; // 


        private Point UnitsToPx(double x, double y)
        {
            scaleX = Source.PixelWidth / this.Width;
            scaleY = Source.PixelHeight / this.Height;
            return new Point((int)(x * s_dpiX / 96), (int)(y * s_dpiY / 96));
        }

        private void SetCropingSource()
        {
            Point point = UnitsToPx(Canvas.GetLeft(dragBorder), Canvas.GetTop(dragBorder));

            int left = (int)(point.X * scaleX);
            int top = (int)(point.Y * scaleY);

            int width = (int)(dragBorder.Width * scaleX);
            int height = (int)(dragBorder.Height * scaleY);
            Int32Rect rect = new Int32Rect(left, top, width, height);

            CroppedBitmap bitImage = new CroppedBitmap(Source, rect);
            CropingSource = bitImage;

        }

        private void SetMark(double top, double left)
        {
            markTop.Height = top;
            double height = Height - top - DragControlLength;
            if (height < 0)
            {
                height = 0;
            }
            markBottom.Height = height;
            markLeft.Width = left;
            markLeft.Height = DragControlLength;
            Canvas.SetTop(markLeft, top);
            markRight.Width = Width - left - DragControlLength;
            markRight.Height = DragControlLength;
            Canvas.SetTop(markRight, top);
        }
    }
}
