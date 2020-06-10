using System.Collections.Specialized;
using System.Text;

namespace IntoApp.Dal
{
    public class AUFileDoucument
    {
        private static readonly Encoding DEFAULTENCODE = Encoding.UTF8;

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="token"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AUDiskFilesCloud(string token,string[] files,NameValueCollection data)
        {
            string url = RequestAddress.server + RequestAddress.AUDiskFilesCloud ;
            string temVal = RequestAddress.HttpUploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;
        }

        /// <summary>
        /// 获取唉优盘传输文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetTransferAUDiskFile(string token,string uid)
        {
            string url = RequestAddress.server + RequestAddress.GetTransferAUDiskFile + "?" +
                         "uid=" + uid + "&audiskFlag=" + "audiskmobile";
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fileids">多文件删除用逗号隔开</param>
        /// <returns></returns>
        public string DelNoTransferAUDiskFile(string token,string fileids)
        {
            string url = RequestAddress.server + RequestAddress.DelNoTransferAUDiskFile + "?" +
                         "fileids=" + fileids;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }

        /// <summary>
        /// 唉优盘打印
        /// </summary>
        /// <param name="token"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AUDiskFilePrint(string token,string[] files,NameValueCollection data)
        {
            string url = RequestAddress.server + RequestAddress.AUDiskFilePrint;
            string temVal = RequestAddress.HttpUploadFile(token, url, files, data, DEFAULTENCODE);
            return temVal;
        }
    }
}
