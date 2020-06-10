using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using IntoApp.ViewModel.Base;

namespace IntoApp.ViewModel
{
    public class PageLoginLoadingViewModel:ViewModelBase
    {

        private string _textBlack="正在加载中...";

        public string TextBlack
        {
            get { return _textBlack; }
            set
            {
                _textBlack = value;
                RaisePropertyChanged("TextBlack");
            }
        }
    }
}
