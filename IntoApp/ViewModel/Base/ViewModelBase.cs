using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IntoApp.Common.Enum;
using IntoApp.utils;
using Microsoft.Practices.ServiceLocation;
using Skin.WPF.Command;

namespace IntoApp.ViewModel.Base
{
     public class ViewModelBase:INotifyPropertyChanged
     {
        #region  属性监听

         public event PropertyChangedEventHandler PropertyChanged;

         public void RaisePropertyChanged(string propertyName)
         {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }

        #endregion

        #region 方法

        private INavigate _navigationService;
         /// <summary>
         /// 当前页的主窗体Frame跳页
         /// </summary>
         /// <param name="obj"></param>
        public void Navigate(Object[] obj)
         {
             Window win = Window.GetWindow((obj[0] as Page));
             Frame frame = win.FindName("Frame") as Frame;
             string str = (obj[1] as Button).Tag.ToString();
             //MessageBox.Show(str);
             _navigationService = ServiceLocator.Current.GetInstance<INavigate>();
             _navigationService.FrameNavigateTo(str, frame);
        }

         #endregion

        #region 命令

         public MyCommand<Object[]> NavigateBaseCommand { get; set; }

         #endregion

        #region 属性
        /// <summary>
        /// loading控件
        /// </summary>
        private bool _runState = true;
        public bool RunState
        {
            get { return _runState; }
            set
            {
                _runState = value;
                RaisePropertyChanged("RunState");
            }
        }

         /// <summary>
         /// 空显示控件
         /// </summary>

        private bool _emptyIsShow = true;
        public bool EmptyIsShow
         {
             get { return _emptyIsShow; }
             set
             {
                 _emptyIsShow = value;
                 RaisePropertyChanged("EmptyIsShow");
             }
         }

        #endregion

    }
}
