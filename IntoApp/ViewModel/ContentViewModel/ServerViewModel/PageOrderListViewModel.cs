using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using IntoApp.API;
using IntoApp.Bll;
using IntoApp.Command;
using IntoApp.Common;
using IntoApp.Common.Enum;
using IntoApp.Controls;
using IntoApp.Model;
using IntoApp.Properties;
using IntoApp.utils;
using IntoApp.View.Content.Server.OrderList;
using IntoApp.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MessageBox=MyMessageBox.Controls.MessageBox;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel
{
    public class PageOrderListViewModel :OrderListViewModelBase
    {

        #region 页面切换
        private Page currentPage = OrderListPageManager.PageOrderListAll;
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

        private OrderList_TabItem selectMenu;

        public OrderList_TabItem SelectMenu
        {
            get { return selectMenu; }
            set
            {
                selectMenu = value;
                RaisePropertyChanged("SelectMenu");
                switch (selectMenu)
                {
                    case OrderList_TabItem.OrderListAll:
                        CurrentPage = OrderListPageManager.PageOrderListAll;
                        break;
                    case OrderList_TabItem.OrderListComplete:
                        CurrentPage = OrderListPageManager.PageOrderListComplete;
                        break;
                    case OrderList_TabItem.OrderListUnfinished:
                        CurrentPage = OrderListPageManager.PageOrderListUnfinished;
                        break;
                }
            }
        }

        #endregion

    }
}
