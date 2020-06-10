using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoApp.AutoUpdate.ViewModel.Base;

namespace IntoApp.AutoUpdate.Model
{
    public class DecompressionModel:ViewModelBase
    {
        private string _downlocalPath;
        private string _decompressionPath;
        private double _dpValue; //解压
        private string _dpValueStr="0%";//解压百分比

        public string DownlocalPath
        {
            get { return _downlocalPath; }
            set { _downlocalPath = value; }
        }

        public string DecompressionPath
        {
            get { return _decompressionPath; }
            set { _decompressionPath = value; }
        }

        public double DpValue
        {
            get { return _dpValue; }
            set
            {
                _dpValue = value;
                RaisePropertyChanged("DpValue");
            }
        }

        public string DpValueStr
        {
            get { return _dpValueStr; }
            set
            {
                _dpValueStr = value;
                RaisePropertyChanged("DpValueStr");
            }
        }
    }
}
