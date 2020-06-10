using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntoApp.utils
{
    public class TrayDownload
    {
        public delegate void SetValue(double value);

        public event SetValue SetProgressBarValue;

        public TrayDownload(string http_file_url, string save_local_path)
        {
            DownloadHttpFile(http_file_url,save_local_path);
        }

        public void DownloadHttpFile(string http_file_url, string save_local_path)
        {
            try
            {
                #region 本地存储临时文件，下载完成后修改文件类型·

                string FileType = Path.GetExtension(http_file_url);  //源文件类型
                //临时文件路径
                string new_local_path = Path.Combine(Path.GetDirectoryName(save_local_path), Path.GetFileNameWithoutExtension(save_local_path) + ".tmp");
                #endregion

                WebResponse response = null;
                WebRequest request = WebRequest.Create(http_file_url);
                response = request.GetResponse();
                if (response == null) return;
                //progressBar.Maximum = response.ContentLength; //远程文件的大小
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    Stream netStream = response.GetResponseStream();
                    //文件长度
                    long lenth = response.ContentLength;
                    Stream fileStream = new FileStream(new_local_path, FileMode.Create);
                    byte[] read = new byte[1024];
                    long progressBarValue = 0;
                    int realReadLen = netStream.Read(read, 0, read.Length);
                    while (realReadLen > 0)
                    {
                        fileStream.Write(read, 0, realReadLen);
                        progressBarValue += realReadLen;
                        SetProgressBarValue(Math.Round(progressBarValue * 1.0 / lenth * 100, 2));
                        //Task.Factory.StartNew(() => { });
                        //progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = progressBarValue; }));
                        realReadLen = netStream.Read(read, 0, read.Length);
                    }
                    netStream.Close();
                    fileStream.Close();

                    #region 文件类型恢复

                    File.Move(new_local_path, save_local_path);

                    #endregion

                }, null);
            }
            catch (Exception e)
            {
               
            }
        }
    }
}
