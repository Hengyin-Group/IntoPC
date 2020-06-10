using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using IntoApp.Model;
using IntoApp.View.Content.Server.OrderList;
using IntoApp.ViewModel.ContentViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.View.Content.Other
{
    /// <summary>
    /// FilePreView.xaml 的交互逻辑
    /// </summary>
    public partial class FilePreView 
    {
        public FilePreView()
        {
            InitializeComponent();
            Messenger.Default.Register<String>(this, "ordercreatesuccess",OrderCreateSuccess);
        }

        public void OrderCreateSuccess(string obj)
        {
            string orderid = obj;
            OrderListHelper.SelectOrderId = orderid;
            (new WinOrderListDetail()).Show();
            this.Close();
        }
    }
}
