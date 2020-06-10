using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.Printer.Pipe
{
    public class PipeHelp
    {
        private static StreamString m_StreamString;

        private static NamedPipeClientStream pipeClient ;
        public const string ServerName = "intopipe";

        /// <summary>
        /// 启动客户端，连接服务器，只允许连接一次
        /// </summary>
        /// <returns></returns>

        public static bool StartConnection()
        {
            try
            {
                if (pipeClient == null)
                {
                    pipeClient =new NamedPipeClientStream(".",ServerName,
                            PipeDirection.InOut, PipeOptions.None,
                            TokenImpersonationLevel.Impersonation);
                    pipeClient.Connect(2000);
                    m_StreamString = new StreamString(pipeClient);
                }
            }
            catch (Exception exception)
            {
                //Environment.Exit(0);
                pipeClient = null;
                throw new Exception("未启动服务器端" + exception.Message);
            }
            return true;
        }


        /// <summary>
        /// 通知服务器客户端即将退出
        /// </summary>
        public static void ClosePipe()
        {
            if (pipeClient != null)
            {
                m_StreamString.WriterString("close");
                m_StreamString = null;
                pipeClient.Close();
                pipeClient = null;
            }
        }
        /// <summary>
        /// 从服务器获取数据
        /// </summary>
        /// <returns></returns>
        public static string GetSystemID()
        {
            if (m_StreamString != null)
            {
                m_StreamString.WriterString("GetBusinessSystemId");
                return m_StreamString.ReadString();
            }
            return null;
        }

        public static string ReturnJo(string jo)
        {
            if (m_StreamString != null)
            {
                m_StreamString.WriterString("GetBusinessSystemId");
                return m_StreamString.ReadString();
            }
            return null;
        }
    }
}
