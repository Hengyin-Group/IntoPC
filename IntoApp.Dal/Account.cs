using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntoApp.Dal
{
    public class Account
    {
        private static readonly Encoding DEFAULTENCODE = Encoding.UTF8;

        #region 登录注册忘记密码
        //public static string server = RequestAddress.server;
        public static string server = RequestAddress.HostServer;

        public string Account_Login(string username, string pwd)
        {
            string url = server + "/api/account/login" + "?username=" + username + "&password=" + pwd;
            string tempVal = Rep_Resp(url, "POST");
            return tempVal;
        }

        public string GetSmsCode(string phone)
        {
            string url = server + "/api/account/GetSmsCode" + "?phone=" + phone;
            string tempVal = Rep_Resp(url, "GET");
            return tempVal;
        }

        public string IsExistPhone(string phone)
        {
            string url = server + "/api/account/IsExistPhone" + "?phone=" + phone;
            string tempVal = Rep_Resp(url, "GET");
            return tempVal;

        }

        public string Regist(string phone, string registcode, string pwd)
        {
            string url = server + "/api/SysUser/Regist" + "?phone=" + phone + "&pwd=" + pwd + "&registcode=" + registcode;
            string tempVal = Rep_Resp(url, "POST");
            return tempVal;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="uphone"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string ReviewPwd(string uphone, string pwd)
        {
            string url = server + "/api/SysUser/ReviewPwd" + "?uphone=" + uphone + "&pwd=" + pwd;
            string tempVal = Rep_Resp(url, "POST");
            return tempVal;
        }

        public string Rep_Resp(string url, string method)
        {
            try
            {
                WebRequest Rep = WebRequest.Create(url);
                Rep.Method = method;
                Rep.Timeout = 5000;
                Rep.ContentType = "application/json";
                Rep.ContentLength = 0;
                WebResponse Resp = Rep.GetResponse();
                Stream respStream = Resp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
                var tempVal = respStreamReader.ReadToEnd();
                return tempVal;
            }
            catch (Exception e)
            {
                string str = "{\"code\":\"404\",\"message\":\"网络请求异常，请稍后重试！\"}";
                //MMessageBox.ShouBox("网络请求异常，请稍后重试！", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.error);
                return str;
            }

        }
        #endregion

        public string EditOwnInfo(string token, string signature, string nickname, string gender, string birthday)
        {
            ///此处文件上传到IntoApp服务器
            string url = RequestAddress.HostServer + RequestAddress.EditOwnInfo + "?signature=" + signature + "&nickname=" + nickname + "&gender=" + gender + "&birthday=" + birthday;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        public string EditOwnInfo(string token, string[] files, NameValueCollection data)
        {
            ///此处文件上传到IntoApp服务器
            string url = RequestAddress.HostServer + RequestAddress.EditOwnInfo;
            string temVal = RequestAddress.HttpUploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;
        }

        public string GetPrintPrice(string token)
        {
            ///此处文件上传到IntoApp服务器
            string url = RequestAddress.HostServer + RequestAddress.GetPrintPrice;
            string temVal = RequestAddress.HttpUploadFile(token, url, null, null, DEFAULTENCODE);
            return temVal;
        }

    }
}
