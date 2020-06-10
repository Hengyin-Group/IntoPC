using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntoApp.Printer.Pipe
{
    public class PipeReport
    {
        #region 单例模式

        private static PipeReport serverPipe;

        private static readonly object locker = new object();

        private static PipeReport GetInstance()
        {
            if (serverPipe == null)
            {
                lock (locker)
                {
                    serverPipe = new PipeReport();
                }
            }
            return serverPipe;
        }

        #endregion
        private PipeReport()
        {
            PipeHelp.StartConnection();
            PipeHelp.GetSystemID();
            //StartClient();
            //Console.ReadKey();
        }

        public static void StartClient()
        {
            string input;
            try
            {
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("localhost", "intopipe"))//与服务端管道名一致，如果连接C++服务端，名称一致即可，“localhost”可以更换为IP地址，也可以进行网络通信
                {
                    pipeStream.Connect();//连接服务端
                    if (!pipeStream.IsConnected)
                    {
                        Console.WriteLine("Failed to connect ....");
                        return;
                    }
                    StreamWriter sw = new StreamWriter(pipeStream);
                    StreamReader sr = new StreamReader(pipeStream);
                    while (true)//循环输入
                    {
                        input = Console.ReadLine();
                        Console.WriteLine("SendMessage:" + input);
                        sw.WriteLine(input);//传递消息到服务端
                        sw.Flush();//注意一定要有，同服务端一样
                        string temp = "";
                        temp = sr.ReadLine();//获取服务端返回信息
                        Console.WriteLine("replyContent:" + temp);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
