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
using IntoApp.ViewModel;
using IntoApp.ViewModel.ContentViewModel;

namespace IntoApp.View.Content.LeftMenu
{
    /// <summary>
    /// PageServer.xaml 的交互逻辑
    /// </summary>
    public partial class PageServer : Page
    {
        public PageServer()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.page_Server;
        }
    }
}
