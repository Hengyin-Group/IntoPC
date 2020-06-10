using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Bll
{
    public class AppVersion
    {
        Dal.AppVersion appVersion=new Dal.AppVersion();
        public string GetAppVersion()
        {
            return appVersion.GetAppVersion();
        }
    }
}
