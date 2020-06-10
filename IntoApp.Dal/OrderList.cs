using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Dal
{
    public class OrderList
    {
        
        public string GetOrderList(string token,string pagesize,string pageindex,string orderstate)
        {
            //string url = RequestAddress.server + RequestAddress.GetOrderList+ "?pagesize="+pagesize+ "&pageindex="+pageindex+ "&orderstate="+orderstate;
            string url = RequestAddress.HostServer + RequestAddress.GetOrderList+ "?pagesize="+pagesize+ "&pageindex="+pageindex+ "&orderstate="+orderstate;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }
        public string GetOrderDetails(string token,string orderid)
        {
            //string url = RequestAddress.server + RequestAddress.GetOrderDetails + "?orderid=" + orderid;
            string url = RequestAddress.HostServer + RequestAddress.GetOrderDetails + "?orderid=" + orderid;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }
        public string PCGetOrderList(string token, string pagesize, string pageindex, string orderstate)
        {
            //string url = RequestAddress.server + RequestAddress.PCGetOrderList + "?pagesize=" + pagesize + "&pageindex=" + pageindex + "&orderstate=" + orderstate;
            string url = RequestAddress.HostServer + RequestAddress.PCGetOrderList + "?pagesize=" + pagesize + "&pageindex=" + pageindex + "&orderstate=" + orderstate;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        public string DelOrder(string token,string orderid)
        {
            //string url = RequestAddress.server + RequestAddress.DelOrder + "?orderid=" + orderid;
            string url = RequestAddress.HostServer + RequestAddress.DelOrder + "?orderid=" + orderid;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        public string GetCount(string token,string pagesize,string pageindex,string orderstate,string userId)
        {
            //string url = RequestAddress.server + RequestAddress.GetCount + "?orderState=" + orderstate + "&pagesize=" +
            //             pagesize + "&pageindex=" + pageindex + "&userId=" + userId;
            string url = RequestAddress.HostServer + RequestAddress.GetCount + "?orderState=" + orderstate + "&pagesize=" +
                         pagesize + "&pageindex=" + pageindex + "&userId=" + userId;
            string temVal = RequestAddress.Rep_Header_Resp(token,url, "POST");
            return temVal;
        }

        /// <summary>
        /// 再来一单
        /// </summary>
        /// <param name="token"></param>
        /// <param name="UserId"></param>
        /// <param name="orderId"></param>
        /// <param name="IsColor"></param>
        /// <param name="PrinterType"></param>
        /// <param name="copies"></param>
        /// <param name="startpage"></param>
        /// <param name="endpage"></param>
        /// <returns></returns>
        public string Again(string token, string UserId, string orderId, string IsColor, string PrinterType,
            string copies,string startpage, string endpage,string isSingle)
        {
            //string url = RequestAddress.server + RequestAddress.BusinessVirtualPrint + "?userid=" + UserId +
            //             "&orderid=" + orderId + "&printType=" + PrinterType + "&copies=" + copies + "&iscolor=" +
            //             IsColor+ "&startpage=" + startpage+"&endpage="+endpage;
            string url = RequestAddress.HostServer + RequestAddress.BusinessVirtualPrint + "?userid=" + UserId +
                         "&orderid=" + orderId + "&printType=" + PrinterType + "&copies=" + copies + "&iscolor=" +
                         IsColor + "&startpage=" + startpage + "&endpage=" + endpage+"&issingle="+isSingle;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="token"></param>
        /// <param name="startpage"></param>
        /// <param name="endpage"></param>
        /// <param name="copies"></param>
        /// <param name="iscolour"></param>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string OrderPayByBusiness(string token,string startpage,string endpage,string copies,string iscolour,string orderno)
        {
            //string url = RequestAddress.server + RequestAddress.OrderPayByBusiness + "?startpage=" + startpage +
            //             "&endpage=" + endpage + "&copies=" + copies +
            //             "&iscolour=" + iscolour + "&orderno=" + orderno;
            string url = RequestAddress.HostServer + RequestAddress.OrderPayByBusiness + "?startpage=" + startpage +
                         "&endpage=" + endpage + "&copies=" + copies +
                         "&iscolour=" + iscolour + "&orderno=" + orderno;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        public string OrderPay(string token,NameValueCollection data)
        {
            //string url = RequestAddress.server + RequestAddress.OrderPay;
            string url = RequestAddress.HostServer + RequestAddress.OrderPay;
            string temVal = RequestAddress.HttpUploadFile(token, url, null, data, RequestAddress.DEFAULTENCODE);
            return temVal;
        }
    }
}
