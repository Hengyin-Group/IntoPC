using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace IntoApp.ViewModel.ContentViewModel
{
    public class WinImageClippingViewModel:ViewModelBase
    {
        #region 单例模式
        private static WinImageClippingViewModel _winImageClippingVM = null;
        private static Object locker=new object();
        public static WinImageClippingViewModel GetInstance
        {
            get
            {
                if (_winImageClippingVM==null)
                {
                    lock (locker)
                    {
                        _winImageClippingVM=new WinImageClippingViewModel();
                    }
                }

                return _winImageClippingVM;
            }
        }

      

        #endregion
        public WinImageClippingViewModel()
        {

        }

        #region 属性

        

        #endregion

        private BitmapImage _bitmapImageSource=new BitmapImage(new Uri(@"D:\360MoveData\Users\comoco\Pictures\Camera Roll\wallhaven-433358.png"));

        public BitmapImage BitmapImageSource
        {
            get { return _bitmapImageSource; }
            set
            {
                _bitmapImageSource = value;
                RaisePropertyChanged(()=>BitmapImageSource);
            }
        }
    }
}
