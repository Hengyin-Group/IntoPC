using IntoApp.Common;
using IntoApp.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace IntoApp.utils
{
    public class UserInfoSave
    {
        public bool SaveIcon()
        {
            //头像本地保存
            //List<string> Str = new List<string>();
            //Str.Add(Path.GetFileName(SavePath.UserIcon_30x30));
            //if (!Directory.Exists(SavePath.UserInfo))
            //    Directory.CreateDirectory(SavePath.UserInfo);
            //List<FileInfo> _fileInfos = FilesHelper.GetFileHasStr(Path.GetDirectoryName(SavePath.UserIcon_30x30), Str);
            //if (_fileInfos!=null && _fileInfos.Count > 0)
            //    //图片文件存在
            //    return true;
            if (File.Exists(SavePath.UserIcon_30x30))
            {
                return true;
            }
            else
            {
                //头像文件不存在
               
                bool isSuc= ImageHelper.DownloadImage(AccountInfo.IconImage, SavePath.UserIcon_30x30,ImageFormat.Bmp);
                //bool isSuc_60= ImageHelper.DownloadImage(AccountInfo.IconImage, SavePath.UserIcon_60x60, ImageFormat.Bmp);
                return isSuc;
            }
        }
    }
}

