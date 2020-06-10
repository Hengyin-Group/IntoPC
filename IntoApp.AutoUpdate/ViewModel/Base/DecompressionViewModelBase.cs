using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntoApp.AutoUpdate.Model;

namespace IntoApp.AutoUpdate.ViewModel.Base
{
    public class DecompressionViewModelBase:DecompressionModel
    {
        private ObservableCollection<DecompressionModel> _decompressionModel;

        public ObservableCollection<DecompressionModel> DecompressionModel
        {
            get
            {
                if (_decompressionModel==null)
                {
                    _decompressionModel=new ObservableCollection<DecompressionModel>();
                }
                return _decompressionModel;
            }
            set
            {
                _decompressionModel = value;
                RaisePropertyChanged("DecompressionModel");
            }
        }
    }
}
