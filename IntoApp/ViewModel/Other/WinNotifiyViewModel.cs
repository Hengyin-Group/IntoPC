using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.ViewModel.Base;
using Skin.WPF.Command;
using MessageBox = MyMessageBox.Controls.MessageBox;

namespace IntoApp.ViewModel.Other
{
    public class WinNotifiyViewModel : ViewModelBase
    {
        public delegate void SetText(string text);

        public event SetText SetTextValue;

        public WinNotifiyViewModel(string str)
        {
            //LoadCommand=new MyCommand(str=>load(str));
            load(str);
            SelectCommand=new MyCommand<object[]>(x=>select(x));
        }

        #region 命令

        public MyCommand LoadCommand { get; set; }
        public MyCommand<Object[]> SelectCommand { get; set; }

        #endregion

        #region 方法

        void load(string str)
        {
            Text = str;
        }

        void select(Object[] x)
        {
            //MessageBox.Show("我被点击了");
            //Window win=x[0] as Window;
            //win.DialogResult = true;
        }

        #endregion

        #region 属性

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        #endregion


    }
}
