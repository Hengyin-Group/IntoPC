using MyMessageBox.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net.Security;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Model;
using IntoApp.UseControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MessageBox = MyMessageBox.Controls.MessageBox;

namespace IntoApp.utils
{
    #region MyRegion

    public class M2MqttHelper
    {
        public delegate void NotifyValue(int value);

        public event NotifyValue SetNotifyValue;

        private static volatile M2MqttHelper _instance = null;
        private static readonly object LockHelper = new object();

        #region Mqtt信息

        private string mqtt_baidu_server = "into_tojo.mqtt.iot.bj.baidubce.com";//百度物联网MQTT 
        private int iMqttPort = 1883; //端口号
        private string strUserName = "into_tojo/tojo"; //创建物接入设备后返回的用户名（实例名+用户名）
        private string strPwd = "F1UaoS4In3BuaexTnpnBAleyyZTVzh0pSzyTb+rXK+k="; //创建身份后返回的密钥
        private string strClientId = Guid.NewGuid().ToString();

        #endregion

        private int CountToConnect=0;

        public M2MqttHelper()
        {
            //ConnectSever();
            //SetNotifyValue(1000);
            CreateInstance(mqtt_baidu_server,"server/"+AccountInfo.ID);
           
        }

        /// <summary>
        /// 创建单例模式
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public M2MqttHelper CreateInstance(string ipAddress, string topic)
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new M2MqttHelper(ipAddress, topic);
                }
            }
            return _instance;
        }

        public static M2MqttHelper getInstance()
        {
            return _instance;
        }


        /// <summary>
        /// 实例化订阅客户端
        /// </summary>
        public MqttClient SubscribeClient;


        public M2MqttHelper(string ipAddress, string topic)
        {

            //int brokerPort = 8883;
            ////string clientId = Guid.NewGuid().ToString();
            //string clientId = "666";
            ////string username = "zzinno";
            //string username = "xxx";//输入用户名
            //string password = "xxxxxxxxx";//输入
            // create client instance 

            bool IsSuccessConnect=false;

            try
            {
                
                SubscribeClient = new MqttClient(ipAddress, iMqttPort, false, MqttSslProtocols.TLSv1_0, null, null); //当主机地址为域名时 
                //SubscribeClient = new MqttClient(ipAddress, brokerPort, true, null, null, MqttSslProtocols.TLSv1_2);
                
                SubscribeClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                //注册链接断开事件
                SubscribeClient.ConnectionClosed += ConnectionClosed;
                // register to message received 
                SubscribeClient.MqttMsgPublishReceived += SubscribeClient_MqttMsgPublishReceived;

                //SubscribeClient.Connect(clientId);
                SubscribeClient.Connect(strClientId, strUserName, strPwd);

                // subscribe to the topic "/home/temperature" with QoS 2 

                //Console.WriteLine("主题订阅成功");
                IsSuccessConnect = true;
                CountToConnect++;
            }
            catch (Exception e)
            {
                if (!IsSuccessConnect&&CountToConnect<10)
                {
                    CreateInstance(mqtt_baidu_server, "server/" + AccountInfo.ID);
                }
                else
                {
                    //MessageBox.Show("通信连接中断,暂时无法进行文件传输");
                    Thread.Sleep(TimeSpan.FromSeconds(CountToConnect));
                }
                //Console.WriteLine(e.Message);
                //Console.WriteLine("连接失败!");
                //Console.ReadKey();
            }
        }

        public void SubscribeClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((obj) => {
            {
                //var receiveMsg = Encoding.UTF8.GetString(e.Message);
                //NotifyIcon notifyIcon = new NotifyIcon();
                //notifyIcon.ShowBalloonTip(1000, "印兔提示", "您有唉优盘同步信息", ToolTipIcon.Info);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    //MessageBox.Show("您有几条唉优盘数据更新");
                    //bool bo= (new NotifictionWindow("hahaha")).ShowDialog()??false;
                    //if (bo)
                    //{
                    //    MessageBox.Show("同意了");
                    //}
                    NotifictionWindow window=new NotifictionWindow("您有几条唉优盘数据更新");
                    window.Show();

                });
            };});
        }



        /// <summary>
        /// 断开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionClosed(object sender, EventArgs e)
        {
            //MessageBox.Show("与服务器连接断开，请检查您的网络");
            CreateInstance(mqtt_baidu_server, "server/" + AccountInfo.ID);
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="publishString"></param>
        public void client_MqttMsgPublish(string publishString)
        {
            SubscribeClient.Publish("server/"+AccountInfo.ID, Encoding.UTF8.GetBytes(publishString), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        //最重要的方法，填入参数为接受到信息后调用的方法，使用添加方法在不同的进程中也能进行调用
        public void AddClient_MqttMsgPublishReceivedAction(Action<object, MqttMsgPublishEventArgs> action)
        {
            SubscribeClient.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(action);
        }
    }


    #endregion




}
