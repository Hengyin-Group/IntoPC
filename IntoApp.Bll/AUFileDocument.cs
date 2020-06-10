using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using IntoApp.Dal;

namespace IntoApp.Bll
{
    public class AUFileDocument
    {
        Dal.AUFileDoucument doucument=new AUFileDoucument();
        public string AUDiskFilesCloud(string token, string[] files, NameValueCollection data)
        {
            return doucument.AUDiskFilesCloud(token, files, data);
        }

        public string GetTransferAUDiskFile(string token, string uid)
        {
            return doucument.GetTransferAUDiskFile(token, uid);
        }

        public string DelNoTransferAUDiskFile(string token, string fileids)
        {
            return doucument.DelNoTransferAUDiskFile(token, fileids);
        }

        public string AUDiskFilePrint(string token,string[] files,NameValueCollection data)
        {
            return doucument.AUDiskFilePrint(token, files, data);
        }
    }
}
