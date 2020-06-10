using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace IntoApp.Common
{
    public class DownloadHelper
    {
        #region HTTP下载文件

        /// <summary>
        /// 下载文件并动态得到文件下载进度
        /// </summary>
        /// <param name="http_file_url"></param>
        /// <param name="save_local_path"></param>
        public static void DownloadHttpFile(string http_file_url, string save_local_path)
        {
            WebResponse response = null;
            WebRequest request = WebRequest.Create(http_file_url);
            response = request.GetResponse();
            if (response == null) return;
            //  progressBar.Maximum =response.ContentLength; 远程文件的大小
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Stream netStream = response.GetResponseStream();
                Stream fileStream = new FileStream(save_local_path, FileMode.Create);
                byte[] read = new byte[1024];
                long progressBarValue = 0;
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0)
                {
                    fileStream.Write(read, 0, realReadLen);
                    progressBarValue += realReadLen;
                    //progressBar.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                fileStream.Close();
            }, null);
        }
        /// <summary>
        /// 远程文件是否存在
        /// </summary>
        /// <param name="http_file_url"></param>
        /// <returns></returns>
        public static bool HttpFileExist(string http_file_url)
        {
            WebResponse response = null;
            bool result = false;
            try
            {
                response = WebRequest.Create(http_file_url).GetResponse();
                result = response != null;
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (result)
                {
                    response.Close();
                }
            }
            return result;
        }

        #endregion
    }
}
