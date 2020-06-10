using IntoApp.Common;
using IntoApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.ViewModel.Base;

namespace IntoApp.utils
{
    public class OrderListHelper : OrderListViewModelBase
    {
        public void GetOrderList<T>(ObservableCollection<T> cc) where T : new ()
        {
            cc.Add(new T(

                ));
        }

        //public void GetOrderList(int pageSize, int pageIndex, string orderState)
        //{
        //    Bll.OrderList bllOrderList = new Bll.OrderList();
        //    string Str = bllOrderList.GetOrderList(AccountInfo.Token, pageSize.ToString(), pageIndex.ToString(),
        //        orderState);
        //    JObject jo = (JObject)JsonConvert.DeserializeObject(Str);
        //    if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
        //    {
        //        for (int i = 0; i < jo["dataList"].Count(); i++)
        //        {
        //            var order = jo["dataList"][i];
        //            OrderList.Add(new Model.OrderList()
        //            {
        //                Index = i + 1,
        //                CreateTime = GetDateTime(order["CreateTime"].ToString()),
        //                FileId = order["FileId"].ToString(),
        //                FileName = order["FileName"].ToString(),
        //                FileType = order["FileType"].ToString(),
        //                ID = order["ID"].ToString(),
        //                OrderCateoryId = order["OrderCategoryId"].ToString(),
        //                OrderNo = order["OrderNo"].ToString(),
        //                OrderState = JObjectHelper.GetStrNum(order["OrderState"].ToString()),  //获取订单状态
        //                PayMode = order["PayMode"].ToString()
        //            });
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show(jo["message"].ToString());
        //    }
        //}
        ///// <summary>
        ///// 修改时间字符串
        ///// </summary>
        ///// <returns></returns>
        //public string GetDateTime(string Time)
        //{
        //    DateTime dateTime = Convert.ToDateTime(Time.Replace("T", " ").Split('.')[0]);
        //    string time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    return time;
        //}
    }
}
