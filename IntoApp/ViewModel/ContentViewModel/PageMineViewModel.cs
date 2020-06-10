using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using IntoApp.API;
using IntoApp.View.Content;
using IntoApp.ViewModel.Base;

namespace IntoApp.ViewModel.ContentViewModel
{
    public class PageMineViewModel:ViewModelBase
    {
        private Page currentPage = new PageEmpty();
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

    }
}
