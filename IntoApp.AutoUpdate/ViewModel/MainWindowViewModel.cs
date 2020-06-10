using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using IntoApp.AutoUpdate.View;
using IntoApp.AutoUpdate.ViewModel.Base;

namespace IntoApp.AutoUpdate.ViewModel
{
    public class MainWindowViewModel:DownloadViewModelBase
    {
        public MainWindowViewModel()
        {

        }

        private Page _currentPage=new PageDownload(); //导航初始化

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }
    }
}
