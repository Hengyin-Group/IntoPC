using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Dal
{
    public class MyDocument
    {

        private static readonly Encoding DEFAULTENCODE = Encoding.UTF8;
        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pagesize">每页显示数量</param>
        /// <param name="page">当前页</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public string GetMyWord(string token, string pagesize, string page, string query)
        {
            //string url = RequestAddress.server + RequestAddress.GetMyWord + "?pagesize=" +
            //             pagesize + "&page=" + page + "&query=" + query;
            string url = RequestAddress.HostServer + RequestAddress.GetMyWord + "?pagesize=" +
                         pagesize + "&page=" + page + "&query=" + query;
            string tempVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return tempVal;
        }

        public string GetPersonalDocTimeList(string token)
        {
            //string url = RequestAddress.server+ RequestAddress.GetPersonalDocTimeList;
            string url = RequestAddress.HostServer + RequestAddress.GetPersonalDocTimeList;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="token"></param>
        /// <param name="wordids"></param>
        /// <returns></returns>
        public string DelMyDocs(string token, string wordids)
        {
            //string url = RequestAddress.server + RequestAddress.DelMyDocs + "?wordids="+ wordids;
            string url = RequestAddress.HostServer + RequestAddress.DelMyDocs + "?wordids=" + wordids;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }
        /// <summary>
        /// 文档打印
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string ReCreateOrder(string token, string fid, string filename)
        {
            //string url = RequestAddress.server + RequestAddress.ReCreateOrder + "?fid=" + fid + "&filename=" + filename;
            string url = RequestAddress.HostServer + RequestAddress.ReCreateOrder + "?fid=" + fid + "&filename=" + filename;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        //文档转码
        public string DocTransCoding(string token, string[] files, NameValueCollection data)
        {
            string url = RequestAddress.server + RequestAddress.DocTransCoding;
            string temVal = RequestAddress.HttpUploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;
        }

        //图片转码
        public string ImgTransCoding(string token, string[] files, NameValueCollection data)
        {
            string url = RequestAddress.server + RequestAddress.ImgTransCoding;
            string temVal = RequestAddress.HttpUploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;
        }

        //获取文档的总数和页数
        public string GetFileTotal(string token, string pagesize, string query)
        {
            string url = RequestAddress.server + RequestAddress.GetFileTotal + "?pagesize=" + pagesize + "&query=" + query;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        #region PC端文档转PDF上传

        public string BusinessVirtualPrint(string token, string[] files, NameValueCollection data)
        {
            string url = RequestAddress.server + RequestAddress.BusinessVirtualPrint;
            string temVal = RequestAddress.UploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;

        }

        #endregion

        #region 云端订单生成

        public string BusinessVirtualPrint(string token, string UserId, string FileId, string IsColor, string PrinterType, string copies,
            string startpage, string endpage)
        {
            string url = RequestAddress.server + RequestAddress.BusinessVirtualPrint + "?userid=" + UserId +
                         "&fileid=" + FileId + "&printType=" + PrinterType + "&copies=" + copies + "&startpage=" + startpage + "&endpage=" + endpage
                         + "&iscolor=" + IsColor;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;


        }

        #endregion

        #region 获取唉优盘数据

        public string GetTray(string token, string pagesize, string page)
        {
            string url = RequestAddress.server + RequestAddress.GetTray + "?pagesize=" +
                         pagesize + "&page=" + page;
            string tempVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return tempVal;
        }

        #endregion

    }
}
