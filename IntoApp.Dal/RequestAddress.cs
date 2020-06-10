using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Threading;

namespace IntoApp.Dal
{
    public class RequestAddress
    {
        public static string HostServer = "http://into.wainwar.com";

        public static string _server;
        public static string server
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_server))
                {
                    _server = HostServer;
                }
                return _server;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _server = value;
                }
            }
        }

        public static readonly Encoding DEFAULTENCODE = Encoding.UTF8;

        /// <summary>
        /// 唉优盘文件同步
        /// </summary>
        public static string AUDiskFilesCloud = "/api/FileOrder/AUDiskFilesCloud";
        public static string GetTransferAUDiskFile = "/api/FileOrder/GetTransferAUDiskFile";
        public static string DelNoTransferAUDiskFile = "/api/FileOrder/DelNoTransferAUDiskFile";
        public static string AUDiskFilePrint = "/api/FileOrder/AUDiskFilePrint"; //爱优盘打印

        /// <summary>
        /// 我的文档
        /// </summary>
        public static string GetMyWord = "/api/MyFile/GetMyWord";
        public static string GetPersonalDocTimeList = "/api/MyFile/GetPersonalDocTimeList";//获取文档时间列表
        public static string DelMyDocs = "/api/MyFile/DelMyDocs";//删除文档
        public static string ReCreateOrder = "/api/MyFile/ReCreateOrder";//文档打印
        public static string DocTransCoding = "/api/FileOrder/DocTransCoding";//文档转码
        public static string ImgTransCoding = "/api/FileOrder/ImgTransCoding";//图片转码
        public static string GetFileTotal = "/api/MyFile/GetFileTotal";//获取文档总页数

        public static string GetTray = "/api/MyFile/GetAUDiskFileByUser";//获取唉优盘数据

        /// <summary>
        /// 个人中心
        /// </summary>
        public static string GetPersonalInfo = "/api/SysUser/GetPersonalInfo";//获取个人信息
        public static string EditOwnInfo = "/api/SysUser/EditOwnInfo";//修改个人信息
        public static string GetPrintPrice = "/api/SysUser/GetPrintPrice";//获取个人单价信息

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public static string GetDevice = "/api/Equipment/GetEquipMentList";//获取设备列表
        public static string GetEquipIP = "/api/Device/GetEquipIP";//获取墨粉

        /// <summary>
        /// PC端
        /// </summary>
        public static string BusinessVirtualPrint = "/api/FileOrder/BusinessVirtualPrint";
        public static string OrderPayByBusiness = "/api/MyOrder/OrderPayByBusiness";


        /// <summary>
        /// 我的订单
        /// </summary>
        public static string GetOrderList = "/api/MyOrder/GetOrderList";
        public static string GetCount = "/api/MyOrder/GetOrderTotal"; //获取Order条数做分页使用
        public static string GetOrderDetails = "/api/MyOrder/GetOrderDetails"; //获取订单详细信息
        public static string PCGetOrderList = "/api/MyOrder/PCGetOrderList";
        public static string DelOrder = "/api/MyOrder/DelOrder"; //删除订单
        public static string OrderPay = "/api/MyOrder/IntoPay"; //订单支付

        /// <summary>
        /// 获取App版本信息
        /// </summary>
        public static string GetAppVersion = "/api/AppVersion/GetNewestVersion";



        /// <summary>
        /// 请求公用方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string Rep_Resp(string url, string method)
        {
            try
            {
                WebRequest Rep = WebRequest.Create(url);
                Rep.Method = method;
                Rep.Timeout = 5000;
                Rep.ContentLength = 0;
                Rep.ContentType = "application/x-www-form-urlencoded";
                WebResponse Resp = Rep.GetResponse();
                Stream respStream = Resp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream ?? throw new InvalidOperationException(), Encoding.UTF8);
                var tempVal = respStreamReader.ReadToEnd();
                return tempVal;
            }
            catch (Exception e)
            {
                string str = "{\"code\":\"404\",\"message\":\"网络请求异常，请稍后重试！\"}";
                //MMessageBox.ShouBox("网络请求异常，请稍后重试！", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.error);
                return str;
            }
        }

        public static string Rep_Header_Resp(string token, string url, string method)
        {
            try
            {
                WebRequest Rep = WebRequest.Create(url);
                Rep.Method = method;
                Rep.Timeout = 5000;
                Rep.ContentLength = 0;
                Rep.Headers.Add("token", token);
                WebResponse Resp = Rep.GetResponse();
                Stream respStream = Resp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
                var tempVal = respStreamReader.ReadToEnd();
                return tempVal;
            }
            catch (Exception e)
            {
                string str = "{\"code\":\"404\",\"message\":\"网络请求异常，请稍后重试！\"}";
                //MMessageBox.ShouBox("网络请求异常，请稍后重试！", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.error);
                return str;
            }

        }

        #region 上传文件请求
        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string token, string url, string[] files, NameValueCollection data, Encoding encoding)
        {
            try
            {


                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                //1.HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                request.Headers.Add("token", token);
                request.Method = "POST";
                request.KeepAlive = true;
                //request.Credentials = CredentialCache.DefaultCredentials;

                using (Stream stream = request.GetRequestStream())
                {
                    //1.1 key/value
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    if (data != null)
                    {
                        foreach (string key in data.Keys)
                        {
                            stream.Write(boundarybytes, 0, boundarybytes.Length);
                            string formitem = string.Format(formdataTemplate, key, data[key]);
                            byte[] formitembytes = encoding.GetBytes(formitem);
                            stream.Write(formitembytes, 0, formitembytes.Length);
                        }
                    }

                    //1.2 file
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    if (files != null)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            stream.Write(boundarybytes, 0, boundarybytes.Length);
                            string header = string.Format(headerTemplate, "file" + i, Path.GetFileName(files[i]));
                            byte[] headerbytes = encoding.GetBytes(header);
                            stream.Write(headerbytes, 0, headerbytes.Length);
                            //using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                            using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    stream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }

                    //1.3 form end
                    stream.Write(endbytes, 0, endbytes.Length);
                }
                //2.WebResponse
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    return stream.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                string str = "{\"code\":\"404\",\"message\":\"+ e +\"}";
                //MMessageBox.ShouBox("网络请求异常，请稍后重试！", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.error);
                return str;
            }
        }

        #endregion

        #region 文件转PDF上传接口（无token）

        public static string UploadFile(string token, string url, string[] files, NameValueCollection data, Encoding encoding)
        {
            try
            {


                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                //1.HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                //request.Headers.Add("token", token);
                request.Method = "POST";
                request.KeepAlive = true;
                //request.Credentials = CredentialCache.DefaultCredentials;

                using (Stream stream = request.GetRequestStream())
                {
                    //1.1 key/value
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    if (data != null)
                    {
                        foreach (string key in data.Keys)
                        {
                            stream.Write(boundarybytes, 0, boundarybytes.Length);
                            string formitem = string.Format(formdataTemplate, key, data[key]);
                            byte[] formitembytes = encoding.GetBytes(formitem);
                            stream.Write(formitembytes, 0, formitembytes.Length);
                        }
                    }

                    //1.2 file
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    for (int i = 0; i < files.Length; i++)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string header = string.Format(headerTemplate, "file" + i, Path.GetFileName(files[i]));
                        byte[] headerbytes = encoding.GetBytes(header);
                        stream.Write(headerbytes, 0, headerbytes.Length);
                        using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                stream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    //1.3 form end
                    stream.Write(endbytes, 0, endbytes.Length);
                }
                //2.WebResponse
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    return stream.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                string str = "{\"code\":\"404\",\"message\":\"网络请求异常，请稍后重试！\"}";
                //MMessageBox.ShouBox("网络请求异常，请稍后重试！", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.error);
                return str;
            }
        }

        #endregion

        #region HTTP下载文件

        /// <summary>
        /// 下载文件并动态得到文件下载进度
        /// </summary>
        /// <param name="http_file_url"></param>
        /// <param name="save_local_path"></param>
        public void DownloadHttpFile(string http_file_url, string save_local_path)
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
        public bool HttpFileExist(string http_file_url)
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

