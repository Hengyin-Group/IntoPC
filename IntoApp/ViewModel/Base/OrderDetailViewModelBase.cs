using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntoApp.Model;

namespace IntoApp.ViewModel.Base
{
    public class OrderDetailViewModelBase:ViewModelBase
    {
        private ObservableCollection<OrderDetail> _orderDetail;

        public ObservableCollection<OrderDetail> OrderDetail
        {
            get
            {
                if (_orderDetail==null)
                {
                    _orderDetail=new ObservableCollection<OrderDetail>();
                }
                return _orderDetail;
            }
            set
            {
                _orderDetail = value;
                RaisePropertyChanged("OrderDetail");
            }
        }

    }
}
