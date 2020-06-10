using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IntoApp.Dal;

namespace IntoApp.Bll
{
    public class Account
    {
        Dal.Account account = new Dal.Account();
        public string Account_Login(string username,string pwd)
        {
            
            return account.Account_Login(username, pwd);
        }

        public string GetSmsCode(string phone)
        {
            return account.GetSmsCode(phone);
        }

        public string IsExistPhone(string phone)
        {
            return account.IsExistPhone(phone);
        }

        public string Regist(string phone, string registcode, string pwd)
        {
            return account.Regist(phone, registcode, pwd);
        }

        public string ReviewPwd(string uphone, string pwd)
        {
            return account.ReviewPwd(uphone, pwd);
        }

        public string EditOwnInfo(string token, string signature, string nickname, string gender, string birthday)
        {
            return account.EditOwnInfo(token, signature, nickname, gender, birthday);
        }

        public string EditOwnInfo(string token, string[] files, NameValueCollection data)
        {
            return account.EditOwnInfo(token, files, data);
        }

        public string GetPrintPrice(string token)
        {
            return account.GetPrintPrice(token);
        }
    }
}
