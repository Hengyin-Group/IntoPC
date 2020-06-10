//using DMSkin.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using IntoApp.API;
using IntoApp.Common.Enum;
using System.ComponentModel;
using IntoApp.ViewModel.Base;
using IntoApp.utils;
using IntoApp.View.Login;
using Microsoft.Practices.ServiceLocation;

namespace IntoApp.ViewModel
{
    public class LoginWindowViewModel: ViewModelBase
    {
      
        public LoginWindowViewModel()
        {
            RunState = false;
        }

        private Page currentPage = new PageLogin();

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            { 
                currentPage = value;
                //RaisePropertyChanged("CurrentPage");
            }
        }

        //private LoginMenu selectMenu;

        //public LoginMenu SeLoginMenu
        //{
        //    get { return selectMenu;}
        //    set
        //    {
        //        selectMenu = value;
        //        RaisePropertyChanged("SeLoginMenu");
        //        switch (selectMenu)
        //        {
        //            case LoginMenu.Login:
        //                CurrentPage = LoginPageManager.PageLogin;
        //                break;
        //            case LoginMenu.ForgetPwd:
        //                CurrentPage = LoginPageManager.PageForgetPwd;
        //                break;
        //            case LoginMenu.Register:
        //                CurrentPage = LoginPageManager.PageRegister;
        //                break;
        //        }
        //    }
        //}


    }
}
