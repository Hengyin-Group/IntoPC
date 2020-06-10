using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace IntoApp.Common
{
    public class HttpFile
    {
        /// <summary>
        /// 下载文件方法
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool Download(string localFilePath, string url)
        {
            bool flag = false;
            //打开上次下载的文件
            long SPosition = 0;
            //实例化流对象
            FileStream FStream;
            //判断要下载的文件夹是否存在
            string ParentDirectory= Path.GetDirectoryName(localFilePath);
            if (!Directory.Exists(ParentDirectory))
            {
                Directory.CreateDirectory(ParentDirectory);
            }
            if (File.Exists(localFilePath))
            {
                //打开要下载的文件
                FStream = File.OpenWrite(localFilePath);
                //获取已经下载的长度
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);
            }
            else
            {
                //文件不保存创建一个文件
                FStream = new FileStream(localFilePath, FileMode.Create);
                SPosition = 0;
            }
            try
            {
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                long length = myRequest.GetResponse().ContentLength;
                if (SPosition > 0)
                    myRequest.AddRange((int) SPosition); //设置Range值
                //向服务器请求,获得服务器的回应数据流
                Stream myStream = myRequest.GetResponse().GetResponseStream();
                //定义一个字节数据
                byte[] btContent = new byte[512];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 512);
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 512);
                }
                //关闭流
                FStream.Close();
                myStream.Close();
                flag = true;        //返回true下载成功
            }
            catch (Exception e)
            {
                FStream.Close();
                flag = false;       //返回false下载失败
            }
            return flag;
        }


    }
}
