using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Dal
{
    public class AppVersion
    {
        public string GetAppVersion()
        {
            //string url =RequestAddress.server+RequestAddress.GetAppVersion+ "?userclient=pc";
            //string tempVal = RequestAddress.Rep_Resp(url,"POST");

            string url =RequestAddress.HostServer + RequestAddress.GetAppVersion+ "?userclient=pc";
            string tempVal = RequestAddress.Rep_Resp(url,"POST");
            return tempVal;
        }
    }
}
