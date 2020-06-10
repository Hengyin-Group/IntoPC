using System.Collections.Specialized;

namespace IntoApp.Bll
{
    public class OrderList
    {
        Dal.OrderList orderList=new Dal.OrderList();
        public string GetOrderList(string token, string pagesize, string pageindex, string orderstate)
        {
            return orderList.GetOrderList(token, pagesize, pageindex, orderstate);
        }

        public string GetOrderDetails(string token, string orderid)
        {
            return orderList.GetOrderDetails(token, orderid);
        }
        public string PCGetOrderList(string token, string pagesize, string pageindex, string orderstate)
        {
            return orderList.PCGetOrderList(token, pagesize, pageindex, orderstate);
        }

        public string DelOrder(string token, string orderid)
        {
            return orderList.DelOrder(token, orderid);
        }

        public string GetCount(string token,string pagesize,string pageindex,string orderState,string userId)
        {
            return orderList.GetCount(token,pagesize, pageindex, orderState, userId);
        }

        public string Again(string token, string UserId, string orderId, string IsColor, string PrinterType,
            string copies, string startpage, string endpage,string isSingle)
        {
            return orderList.Again(token, UserId, orderId, IsColor, PrinterType, copies, startpage, endpage,isSingle);
        }

        public string OrderPayByBusiness(string token, string startpage, string endpage, string copies, string iscolour, string orderno)
        {
            return orderList.OrderPayByBusiness(token, startpage, endpage, copies, iscolour, orderno);
        }

        public string OrderPay(string token,NameValueCollection data)
        {
            return orderList.OrderPay(token, data);
        }

    }
}
