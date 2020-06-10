using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.AutoUpdate.ViewModel.Base;
using Skin.WPF.Command;

namespace IntoApp.AutoUpdate.ViewModel
{
    public class InformUpdateViewModel:ViewModelBase
    {
        private string _info;

        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        public MyCommand<object[]> UpdateNowCommand
        {
            get
            {
                return new MyCommand<object[]>(x =>UpdateNow(x) );
            }
        }

        public void UpdateNow(object[] obj)
        {
            //MessageBox.Show("现在更新");
        }

        public MyCommand<object[]> NextCommand
        {
            get
            {
                return new MyCommand<object[]>(x=>Next(x));
            }
        }

        public void Next(object[] obj)
        {
            Application.Current.Shutdown();
        }
    }
}
