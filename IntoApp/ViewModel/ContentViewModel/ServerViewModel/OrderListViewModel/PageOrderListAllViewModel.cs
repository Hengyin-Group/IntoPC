using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Common;
using IntoApp.Model;
using IntoApp.utils;
using IntoApp.View.Content.Server.OrderList;
using IntoApp.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using MessageBox = MyMessageBox.Controls.MessageBox;
using OrderListHelper = IntoApp.Model.OrderListHelper;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel.OrderListViewModel
{
    public class PageOrderListAllViewModel:OrderListViewModelBase
    {
       

        /// <summary>
        /// 一页多少条
        /// </summary>
        private int _pageSize = 30;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }
        /// <summary>
        /// 当前第几页
        /// </summary>

        private int _pageIndex = 1;  
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;
                RaisePropertyChanged("PageIndex");
            }
        }
        /// <summary>
        /// 要查询订单的状态
        /// </summary>
        private string _orderState = "";

        public string OrderState
        {
            get { return _orderState; }
            set
            {
                _orderState = value;
                RaisePropertyChanged("OrderState");
            }
        }

        #region 页面初始化
        public PageOrderListAllViewModel()
        {
            LoadCommand=new MyCommand(x =>
            {
                OrderListAll.Clear();
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    GetOrderList(PageSize, PageIndex, OrderState);
                });
               
            });
            ScrollChangedCommand=new MyCommand<ExCommandParameter>(x=>ScrollChanged(x));
            SelectionChangedCommand=new MyCommand<DataGrid>(x=>selectionChanged(x));
        }
        #endregion

        #region 命令

        public MyCommand LoadCommand { get; set; }

        public MyCommand<DataGrid> SelectionChangedCommand { get; set; }

        public MyCommand<ExCommandParameter> ScrollChangedCommand { get; set; }


        #endregion

        #region 属性

        #endregion

        #region 方法

        void ScrollChanged(ExCommandParameter x)
        {
            ScrollChangedEventArgs e = x.EventArgs as ScrollChangedEventArgs;
            DataGrid d = x.Parameter as DataGrid;
            //是否滑到底端
            bool bo = DataGridHelper.IsVerticalScrollBarAtButtom(e, d);
            if (bo)
            {
                try
                {
                    int i = OrderListAll.Count - 3;
                    PageIndex++;
                    GetOrderList(PageSize, PageIndex, OrderState);
                    d.UpdateLayout();
                    d.ScrollIntoView(d.Items[i]);
                }
                catch (Exception exception)
                {
                }
            }
        }

        public void selectionChanged(DataGrid x)
        {
            try
            {
                var orderlist = (OrderListAll)x.SelectedItems[0];
                ////MessageBox.Show(orderlist.FileNameCut);
                OrderListHelper.SelectOrderId = orderlist.ID;
                (new WinOrderListDetail()).Show();
            }
            catch (Exception e)
            {

            }
           
        }

        #region 获取订单

        void GetOrderList(int pageSize, int pageIndex, string orderState)
        {
            RunState = true;
            EmptyIsShow = false;
            Bll.OrderList bllOrderList = new Bll.OrderList();
            string Str = bllOrderList.GetOrderList(AccountInfo.Token, pageSize.ToString(), pageIndex.ToString(),
                orderState);
            JObject jo = (JObject)JsonConvert.DeserializeObject(Str);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
            {
                for (int i = 0; i < jo["dataList"].Count(); i++)
                {
                    var order = jo["dataList"][i];
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        OrderListAll.Add(new OrderListAll()
                        {
                            Index = i + 1,
                            CreateTime = DateTimeHelper.GetDateTime(order["CreateTime"].ToString()),
                            FileId = order["FileId"].ToString(),
                            FileName = order["FileName"].ToString(),
                            FileType = order["FileType"].ToString(),
                            ID = order["ID"].ToString(),
                            OrderCateoryId = order["OrderCategoryId"].ToString(),
                            OrderNo = order["OrderNo"].ToString(),
                            OrderState = JObjectHelper.GetStrNum(order["OrderState"].ToString()),  //获取订单状态
                            PayMode = order["PayMode"].ToString()
                        });
                    });
                   
                }
            }
            else
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    MessageBox.Show(jo["message"].ToString());
                });
            }

            RunState = false;
        }

        #endregion

        #endregion

    }
}
