using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IntoApp.View.Content;

using IntoApp.View.Content.LeftMenu;
using IntoApp.View.Content.Mine;
using IntoApp.View.Content.Server;
using IntoApp.View.Content.Server.OrderList;
using IntoApp.View.Content.Server.TrayHistory;
using IntoApp.View.Content.WorkBench;

namespace IntoApp.API
{
     public class ContentPageManager
    {
        #region 声明、注册页面

        public static PageEmpty PageEmpty=new PageEmpty();  //空白页

        public static PageContacts PageContacts=new PageContacts();//联系人

        public static PageMessage PageMessage=new PageMessage();//消息，群聊

        public static PageWorkBench PageWorkBench=new PageWorkBench();//工作台

        public static PageServer PageServer=new PageServer();//服务
        
        public static PageMine PageMine=new PageMine();//我的

        #endregion
    }
    public class WorkBenchPageManager
    {
        #region 声明，注册页面

        public static PageApproval PageApproval = new PageApproval();//审批页

        #endregion

    }

    public class ServerPageManager
    {
        public static PageTray PageTray=new PageTray();//唉优盘
        public static PageOrderList PageOrderList=new PageOrderList();//订单列表
        public static PageTrayHistory PageTrayHistory = new PageTrayHistory();
    }

    public class OrderListPageManager 
    {
        public static PageOrderListAll PageOrderListAll=new PageOrderListAll();//全部
        public static PageOrderListComplete PageOrderListComplete=new PageOrderListComplete();//已完成
        public static PageOrderListUnfinished PageOrderListUnfinished =new PageOrderListUnfinished();//未完成
    }

    public class MinePageManager
    {
        public static PageCollection PageCollection = new PageCollection();
    }

    public class TrayHistoryPageManager
    {
        public static PageTrayUploaded PageTrayUploaded=new PageTrayUploaded();
        public static PageTrayDownload PageTrayDownload=new PageTrayDownload();
        public static PageTrayUnDownload PageTrayUnDownload=new PageTrayUnDownload();
    }
}
