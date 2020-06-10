using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IntoApp.ViewModel;

namespace IntoApp.View.Content
{
    /// <summary>
    /// WinImageClipping.xaml 的交互逻辑
    /// </summary>
    public partial class WinImageClipping
    {
        private BitmapSource CurBitMap;
        public WinImageClipping()
        {
            InitializeComponent();
        }
        public void OnCutImaging(object source)
        {
            if (source != null && source.GetType() == typeof(RoutedEventArgs))
            {
                // if (((RoutedEventArgs)source).OriginalSource.GetType() == typeof(CroppedBitmap))
                {
                    CurBitMap = (BitmapSource)((RoutedEventArgs)source).OriginalSource;
                }
            }
        }
        public void btnCutPicture_Click()
        {
            string strFilePath = "F:\\1.png";
            //try
            {
                if (File.Exists(strFilePath) == true)
                {
                    File.Delete(strFilePath);
                }
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(this.CurBitMap));

                FileStream fileStream = new FileStream(strFilePath, FileMode.Create, FileAccess.ReadWrite);
                encoder.Save(fileStream);
                fileStream.Close();
            }
            //catch
            {

            }

        }

        public void Load()
        {
            Bitmap bmp=new Bitmap("../../ 20190116150301.jpg");
            System.IntPtr hBitmap = bmp.GetHbitmap();

            //string str = @"D:\360MoveData\Users\comoco\Pictures\微信图片_20190222165430.png";
            //this.ImageDealer.BitSource=new BitmapImage(new Uri("../../20190116150301.jpg",UriKind.RelativeOrAbsolute));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
