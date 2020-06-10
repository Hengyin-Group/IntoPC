using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Dal
{
    public class PersonalInfo
    {

        public string GetPersonalInfo(string token)
        {
            //string url = RequestAddress.server + RequestAddress.GetPersonalInfo;
            string url = RequestAddress.HostServer + RequestAddress.GetPersonalInfo;
            string tempVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return tempVal;
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signature">签名</param>
        /// <param name="nickname">昵称</param>
        /// <param name="gender">性别</param>
        /// <param name="birthday">生日</param>
        /// <returns></returns>
        public string EditOwnInfo(string token,string signature,string nickname,string gender,string birthday)
        {
            //string url = RequestAddress.server + RequestAddress.EditOwnInfo + "?signature=" + signature + "&nickname=" +
            //             nickname + "&gender=" + gender + "&birthday=" + birthday;
            string url = RequestAddress.HostServer + RequestAddress.EditOwnInfo + "?signature=" + signature + "&nickname=" +
                         nickname + "&gender=" + gender + "&birthday=" + birthday;
            string tempVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return tempVal;
        }
    }
}
