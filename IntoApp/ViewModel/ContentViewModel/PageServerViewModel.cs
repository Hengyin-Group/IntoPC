using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using IntoApp.API;
using IntoApp.Common.Enum;
using IntoApp.utils;
using IntoApp.View.Content.Server;
using IntoApp.ViewModel.Base;
using Microsoft.Practices.ServiceLocation;
using Skin.WPF.Command;

namespace IntoApp.ViewModel.ContentViewModel
{
    public class PageServerViewModel:ViewModelBase
    {

        #region 单例模式

        private static PageServerViewModel PageServer;

        private static readonly object locker = new object();

        public static PageServerViewModel GetInstance()
        {
            if (PageServer == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (PageServer == null)
                    {
                        PageServer = new PageServerViewModel();
                    }
                }
            }
            return PageServer;
        }

        #endregion

        private Page currentPage = ServerPageManager.PageTray;

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

        private Server_IntermediateMenu seleftMenu;

        public Server_IntermediateMenu SeLeftMenu
        {
            get { return seleftMenu; }
            set
            {
                seleftMenu = value;
                RaisePropertyChanged("SeLeftMenu");
                switch (seleftMenu)
                {
                    case Server_IntermediateMenu.OrderList:
                        CurrentPage = ServerPageManager.PageOrderList;
                        break;
                    case Server_IntermediateMenu.Tray:
                        CurrentPage = ServerPageManager.PageTray;
                        break;
                    case Server_IntermediateMenu.TrayHistory:
                        CurrentPage = ServerPageManager.PageTrayHistory;
                        break;
                }
            }
        }

        private PageServerViewModel()
        {
            RunState = false;
        }

        #region 命令

        public MyCommand<Object[]> NavigateCommand { get; set; }

        #endregion

        /// <summary>
        /// 打开上传窗口
        /// </summary>
        public MyCommand UpLoadCommand
        {
            get
            {
                return new MyCommand(x =>UpLoadWindowShow() );
            }
        }
        public void UpLoadWindowShow()
        {
            new WinFileUpload().Show();
        }

    }
}
