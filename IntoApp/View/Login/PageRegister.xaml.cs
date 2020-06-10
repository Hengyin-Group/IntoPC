using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntoApp.Bll;
using IntoApp.utils;
using Newtonsoft.Json.Linq;

namespace IntoApp.View.Login
{
    /// <summary>
    /// PageRegister.xaml 的交互逻辑
    /// </summary>
    public partial class PageRegister : Page
    {
        Account bll =new Account();
        public PageRegister()
        {
            InitializeComponent();
        }

    }
}
