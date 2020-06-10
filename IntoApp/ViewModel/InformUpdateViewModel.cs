using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.ViewModel.Base;
using Skin.WPF.Command;

namespace IntoApp.ViewModel
{
    public class InformUpdateViewModel:ViewModelBase
    {
        #region 页面初始化
        public InformUpdateViewModel()
        {
            UpdateNowCommand=new MyCommand<object[]>(x=>Update(x));
            NextCommand=new MyCommand<object[]>(x=>Next(x));
        }

        #endregion

        #region 命令

        public MyCommand<object[]> UpdateNowCommand { get; set; }
        public MyCommand<object[]> NextCommand { get; set; }

        #endregion

        #region 方法

        void Update(object[] obj)
        {
            Window window=obj[0] as Window;
            window.DialogResult = true;
            window.Close();
        }

        void Next(object[] obj)
        {
            Window window = obj[0] as Window;
            window.DialogResult = false;
            window.Close();
        }

        #endregion


    }
}
