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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntoApp.View.Content.Server.OrderList
{
    /// <summary>
    /// PageOrderListAll.xaml 的交互逻辑
    /// </summary>
    public partial class PageOrderListAll : Page
    {
        public PageOrderListAll()
        {
            InitializeComponent();
        }

        private void DataGrid_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool bo= IsVerticalScrollBarAtButtom(e);
            if (bo)
            {
                MessageBox.Show("到底了");
            }
        }

        public bool IsVerticalScrollBarAtButtom(ScrollChangedEventArgs e)
        {
            bool isAtButton = false;
            double dVer = e.VerticalOffset;
            double dViewport = e.ViewportHeight;
            Double dExtent = e.ExtentHeight;
            if (dVer!=0)
            {
                if (dVer+dViewport==dExtent)
                {
                    isAtButton = true;
                }
                else
                {
                    isAtButton = false;
                }
            }
            else
            {
                isAtButton = false;
            }

            if (this.DataGrid.VerticalScrollBarVisibility==ScrollBarVisibility.Disabled||this.DataGrid.VerticalScrollBarVisibility==ScrollBarVisibility.Hidden)
            {
                isAtButton = true;
            }

            return isAtButton;
}
    }
}
