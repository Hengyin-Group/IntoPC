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

namespace IntoApp.View.Content.Server.TrayHistory
{
    /// <summary>
    /// PageUploaded.xaml 的交互逻辑
    /// </summary>
    public partial class PageTrayUploaded : Page
    {
        public PageTrayUploaded()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.page_TrayhistoryUploaded;
        }
    }
}
