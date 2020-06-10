using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IntoApp.Model;

namespace IntoApp.Common
{
    public class SavePath
    {
        //用户基本信息存储位置
        public static string UserInfo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserInfo",AccountInfo.Phone);
        public static string UserIconSource_Local = Path.Combine(UserInfo,Path.GetFileNameWithoutExtension(AccountInfo.IconImage), Path.GetFileName(AccountInfo.IconImage));
        public static string UserIcon_30x30 = Path.Combine(UserInfo,Path.GetFileNameWithoutExtension(AccountInfo.IconImage),"_30x30.bmp");
        public static string UserIcon_60x60 = Path.Combine(UserInfo, Path.GetFileNameWithoutExtension(AccountInfo.IconImage),"_60x60.bmp");
        public static string UserIconEdit = Path.Combine(UserInfo, "HeaderEdit.png");
    }

}
