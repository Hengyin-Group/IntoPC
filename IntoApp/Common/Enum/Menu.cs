using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Common.Enum
{
    public enum LoginMenu
    {
        Login,
        Register,
        ForgetPwd,
        LoginLoading
    }

    public enum LeftMenu
    {
        /// <summary>
        /// 工作台
        /// </summary>
        WorkBench,
        /// <summary>
        /// 我的
        /// </summary>
        Mine,
        /// <summary>
        /// 服务
        /// </summary>
        Server,
        /// <summary>
        /// 消息
        /// </summary>
        Message,
        /// <summary>
        /// 通讯录
        /// </summary>
        Contacts,
        /// <summary>
        /// 设置
        /// </summary>
        Setting,
        /// <summary>
        /// 重启虚拟打印机
        /// </summary>
        Refresh
    }

    public enum WorkBench_IntermediateMenu  //工作台菜单
    {

    }

    public enum Mine_IntermediateMenu  //我的菜单
    {

    }

    public enum Server_IntermediateMenu  //服务菜单
    {
        Tray,
        OrderList,
        TrayHistory
    }

    public enum Message_IntermediateMenu  //通知消息菜单
    {

    }

    public enum Contacts_IntermediateMenu  //通讯录菜单
    {

    }

    public enum OrderList_TabItem  //订单状态列表
    {
        OrderListAll,    //全部
        OrderListComplete,   //已完成
        OrderListUnfinished    //未完成
    }

    public enum TrayHistory
    {
        Uploaded,
        Downloaded,
        UnDownload
    } //唉优盘记录
}
