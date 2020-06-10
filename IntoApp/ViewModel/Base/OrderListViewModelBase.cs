using IntoApp.Common;
using IntoApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MessageBox=MyMessageBox.Controls.MessageBox;

namespace IntoApp.ViewModel.Base
{
    public class OrderListViewModelBase: ViewModelBase
    {
        #region 

        //public void GetOrderList(int pageSize, int pageIndex, string orderState)
        //{
        //    Bll.OrderList bllOrderList = new Bll.OrderList();
        //    string Str = bllOrderList.GetOrderList(AccountInfo.Token, pageSize.ToString(), pageIndex.ToString(),
        //        orderState);
        //    JObject jo = (JObject)JsonConvert.DeserializeObject(Str);
        //    if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
        //    {
        //        for (int i = 0; i < jo["dataList"].Count(); i++)
        //        {
        //            var order = jo["dataList"][i];
        //            OrderListAll.Add(new Model.OrderListAll()
        //            {
        //                Index = i + 1,
        //                CreateTime = GetDateTime(order["CreateTime"].ToString()),
        //                FileId = order["FileId"].ToString(),
        //                FileName = order["FileName"].ToString(),
        //                FileType = order["FileType"].ToString(),
        //                ID = order["ID"].ToString(),
        //                OrderCateoryId = order["OrderCategoryId"].ToString(),
        //                OrderNo = order["OrderNo"].ToString(),
        //                OrderState = JObjectHelper.GetStrNum(order["OrderState"].ToString()),  //获取订单状态
        //                PayMode = order["PayMode"].ToString()
        //            });
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show(jo["message"].ToString());
        //    }
        //}
        ///// <summary>
        ///// 修改时间字符串
        ///// </summary>
        ///// <returns></returns>
        //public string GetDateTime(string Time)
        //{
        //    DateTime dateTime = Convert.ToDateTime(Time.Replace("T", " ").Split('.')[0]);
        //    string time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    return time;
        //}

        #endregion

        #region 集合

        private ObservableCollection<OrderListAll> _orderListAll;
        public ObservableCollection<OrderListAll> OrderListAll
        {
            get
            {
                if (_orderListAll == null)
                {
                    _orderListAll = new ObservableCollection<OrderListAll>();
                }

                return _orderListAll;
            }
            set
            {
                _orderListAll = value;
                RaisePropertyChanged("OrderListAll");
            }
        }

        private ObservableCollection<OrderListComplete> _orderListComplete;
        public ObservableCollection<OrderListComplete> OrderListComplete
        {
            get
            {
                if (_orderListComplete == null)
                {
                    _orderListComplete = new ObservableCollection<OrderListComplete>();
                }

                return _orderListComplete;
            }
            set
            {
                _orderListComplete = value;
                RaisePropertyChanged("OrderListComplete");
            }
        }

        private ObservableCollection<OrderListUnfinished> _orderListUnfinished;
        public ObservableCollection<OrderListUnfinished> OrderListUnfinished
        {
            get
            {
                if (_orderListUnfinished == null)
                {
                    _orderListUnfinished = new ObservableCollection<OrderListUnfinished>();
                }

                return _orderListUnfinished;
            }
            set
            {
                _orderListUnfinished = value;
                RaisePropertyChanged("OrderListUnfinished");
            }
        }

        #endregion
    }
}
