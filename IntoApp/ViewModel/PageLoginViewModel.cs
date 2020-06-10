using IntoApp.Common.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using DMSkin.Core.MVVM;
using IntoApp.ViewModel.Base;
using IntoApp.API;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Model;
using IntoApp.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ShowLoading= MyMessageBox.Controls.ShowLoading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Skin.WPF.Command;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Common.Helper;
using Skin.WPF.Controls;
using Path = System.IO.Path;
using XmlHelper = IntoApp.Common.XmlHelper;
using MessageBox= MyMessageBox.Controls.MessageBox;


namespace IntoApp.ViewModel
{
    public class PageLoginViewModel:LoginViewModelBase
    {

        public PageLoginViewModel()
        {
            LoginCommand=new MyCommand<Object[]>(x => Login_Click(x));
            SelectPhoneCommand = new MyCommand<object[]>(x => SelectPhone(x));
            SelectCommand=new MyCommand<Object[]>(x=>SelectCombox(x));
            DelSaveUserCommand=new MyCommand<object[]>(x=>DelSaveUser(x));
            PreviewTextInputCommand=new MyCommand<TextCompositionEventArgs>(x=>PreViewTextInput(x));
            NavigateCommand = new MyCommand<Object[]>(x => Navigate(x));
            LoadlocalUserInfo();
            RunState = false;
        }

        #region 例子

        private MyCommand<Object> _loadedCommand;
        public MyCommand<Object> LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                    _loadedCommand = new MyCommand<Object>(
                        new Action<object>(
                            o => MessageBox.Show("程序加载完毕！"))
                    );
                return _loadedCommand;
            }
        }

        private MyCommand<MouseEventArgs> _mouseMoveCommand;
        public MyCommand<MouseEventArgs> MouseMoveCommand
        {
            get
            {
                if (_mouseMoveCommand == null)
                    _mouseMoveCommand = new MyCommand<MouseEventArgs>(
                        new Action<MouseEventArgs>(e =>
                        {
                            var point = e.GetPosition(e.Device.Target);
                            var left = "左键放开";
                            var mid = "中键放开";
                            var right = "右键放开";

                            if (e.LeftButton == MouseButtonState.Pressed)
                                left = "左键按下";
                            if (e.MiddleButton == MouseButtonState.Pressed)
                                mid = "中键按下";
                            if (e.RightButton == MouseButtonState.Pressed)
                                right = "右键按下";

                            TipText = $"当前鼠标位置  X:{point.X}  Y:{point.Y}  当前鼠标状态:{left} {mid}  {right}";
                        }),
                        new Func<object, bool>(o => IsReceiveMouseMove));
                return _mouseMoveCommand;
            }
        }

        private MyCommand<RoutedEventArgs> _mouseClickCommand;

        public MyCommand<RoutedEventArgs> MouseClickCommand
        {
            get
            {
                return new MyCommand<RoutedEventArgs>(new Action<RoutedEventArgs>(e =>
                {
                    LoginWindow loginWindow = new LoginWindow();
                    LoginWindowViewModel loginWindowViewModel = new LoginWindowViewModel();
                    loginWindowViewModel.CurrentPage = LoginPageManager.PageRegister;
                    loginWindow.DataContext = loginWindowViewModel;
                    MessageBox.Show("点击了");
                }), (o) => { return true; });
            }

        }



        private bool _IsReceiveMouseMove = true;
        public bool IsReceiveMouseMove
        {
            get { return _IsReceiveMouseMove; }
            set
            {
                _IsReceiveMouseMove = value;
                RaisePropertyChanged("IsReceiveMouseMove");
            }
        }

        private string _tipText;
        public string TipText
        {
            get { return _tipText; }
            set
            {
                _tipText = value;
                RaisePropertyChanged("TipText");
            }
        }

        #endregion

        #region 命令

        public MyCommand<Object[]> LoginCommand { get; set; }
        public MyCommand<Object[]> SelectCommand { get; set; }
        public MyCommand<Object[]> DelSaveUserCommand { get; set; }
        public MyCommand<Object[]> SelectPhoneCommand { get; set; }

        #endregion

        //private MyCommand _clickCommand;

        //public MyCommand<object[]> LoginCommand
        //{
        //    get
        //    {
        //        return  new MyCommand<object[]>(x=> Login_Click(x));
        //    }
        //}
        //窗口登录操作

        #region 方法
        private void Login_Click(object[] obj)
        {
            #region

            RunState = true;
            
            UseComboBox cmb = obj[0] as UseComboBox;
            List<WaterMarkTextBox> TextBox = DependencyObjectHelper.FindVisualChild<WaterMarkTextBox>(cmb);
            string Phone = TextBox[0].Text;
            
            PasswordBox pwd = obj[1] as PasswordBox;
            string Pwd = pwd.Password;
            Page page = obj[2] as Page;
            Window window = Window.GetWindow(page);//获取当前页的母窗体(LoginWindow)
            CheckBox ch = obj[3] as CheckBox;

            Task.Factory.StartNew(new Action(delegate
            {
                string LoginCallBack = account.Account_Login(Phone, Pwd);
                JObject LoginCallBackJo = (JObject)JsonConvert.DeserializeObject(LoginCallBack);
                if (JObjectHelper.GetStrNum(LoginCallBackJo["code"].ToString()) == 200)
                {
                    if (LoginCallBackJo["dataList"]["enterpriseUserFlag"].GetInt() == 0)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            RunState = false;
                            MessageBox.Show("您还未进行企业认证，请至移动端申请企业认证后登陆");
                        });
                        return;
                    }
                    Bo = true;
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if ((bool)ch.IsChecked)
                        {
                            SaveUserLoginInfo(Phone, Pwd);
                        }
                        bool bo = LoginHelper.LoginCallBack(LoginCallBackJo);
                        if (bo)
                        {
                            Bo = true;
                            ViewModelLocator.Win_Content.Show();
                            //new ContentWindow().Show(); //contentwindow打开
                            //var cc= ViewModelLocator.Pipe;
                            Thread thread = new Thread(LoginHelper.AddPrinter);
                            thread.Start();
                            window.Close();
                        }
                    });
                }
                else
                {
                    Bo = false;
                    RunState = false;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { MessageBox.Show(LoginCallBackJo["message"].ToString()); });
                }
            }));
           
            #endregion
        }

        private void SelectCombox(object[] obj)
        {
            ComboBox cmb= obj[0] as ComboBox;
            PasswordBox Psb= obj[1] as PasswordBox;
            List<WaterMarkTextBox> TextBox = DependencyObjectHelper.FindVisualChild<WaterMarkTextBox>(cmb);
            string[] SplitStr = cmb.SelectedValue.ToString().Split('*');
            string _pwd= RsaHelper.RSADecrypt(SplitStr[0],SplitStr[1]);
            Psb.Password = _pwd;
        }

        private void SaveUserLoginInfo(string Phone,string Pwd)
        {
            string PrivateKey = "";
            string PublicKey = "";
            RsaHelper.Generator(out PrivateKey,out PublicKey,1024);
            //string _Phone=
            string aa= RsaHelper.RSAEncrypt(PublicKey, Pwd);
            string _pwd = PrivateKey + "*" + aa;

            #region MyRegion

            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //if (!File.Exists(path))
            //{
            //    XmlHelper.CreateXml(path, "XmlUser.xml");
            //}
            //XElement xele=XElement.Load(Path.Combine(path, "XmlUser.xml"));
            //var item = (from ele in xele.DescendantsAndSelf("User")
            //            select xele).ToList();
            //if (item.Count > 0)
            //{
            //    string str = "<Root>";
            //    for (int i = 0; i < item.Count; i++)
            //    {
            //        str += item[i].ToString();
            //    }
            //    str += "</Root>";
            //    string json = XmlHelper.ParseXmlToJson(str);
            //    JObject jo = (JObject) JsonConvert.DeserializeObject(json);
            //    int index = 0;
            //    for (int i = 0; i < jo["Root"]["User"].Count(); i++)
            //    {
            //        var cc = jo["Root"]["User"][i];
            //        LocalUserInfo.Add(new LocalUserInfo()
            //        {
            //            Index = index+1,
            //            Phone = cc["Phone"].ToString(),
            //            Pwd = cc["Pwd"].ToString(),
            //        });
            //    }
            //}
            //else
            //{

            //}

            #endregion

            XElement xele = XElement.Load(Path.Combine(localUserInfoFilepath, FileName));
            bool IsExist=false;

            for (int i = 0; i < LocalUserInfo.Count; i++)
            {
                if (LocalUserInfo[i].Phone == Phone)
                {
                    IsExist = true;
                    if (LocalUserInfo[i].Pwd != _pwd)
                    {
                        var item = (from ele in xele.Elements("User")
                            where (String)ele.Element("Phone")==Phone
                            select ele).FirstOrDefault();
                        if (item != null)
                        {
                            item.Element("Pwd").ReplaceWith(new XElement("Pwd", _pwd));
                            xele.Save(Path.Combine(localUserInfoFilepath, FileName));
                            //item.ElementsAfterSelf().FirstOrDefault().ReplaceWith(new XElement("Pwd", _pwd));
                        }
                    }
                    break;
                }
            }
            if (!IsExist)
            {
                XElement User=new XElement("User");
                User.SetElementValue("Phone",Phone);
                User.SetElementValue("Pwd",_pwd);
                xele.AddFirst(User);
                xele.Save(Path.Combine(localUserInfoFilepath,FileName));
            }
        }

        #region 获取本地Xml文件

        private string localUserInfoFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login");

        private string FileName = "XmlUser.xml";

        void LoadlocalUserInfo()
        {
            if (!Directory.Exists(localUserInfoFilepath))
            {
                Directory.CreateDirectory(localUserInfoFilepath);
            }
            if (!File.Exists(Path.Combine(localUserInfoFilepath, FileName)))
            {
                XmlHelper.CreateXml(localUserInfoFilepath, FileName);
            }

            GetLocalFilePath();
        }

        public void GetLocalFilePath()
        {
            ///Xml转json
            XElement xele = XElement.Load(Path.Combine(localUserInfoFilepath, FileName));
            var item = (from ele in xele.DescendantsAndSelf("User")
                select ele).ToList();
            if (item.Count>0) //存在用户
            {
                XmlDocument document=new XmlDocument();
                document.LoadXml(xele.ToString());
                string str = JsonConvert.SerializeXmlNode(document);
                JObject json = (JObject) JsonConvert.DeserializeObject(str);
                //判断是否存在“[”“]” 来判断是否为多用户 正则匹配
                Regex reg=new Regex(@"\[[^\[\]]+\]");
                MatchCollection Users= reg.Matches(str);
                bool UsersCount= Users.Count > 0 ;
                //大于0  至少有两个用户 等于0  只存在一个用户
                int index = 0;
                if (UsersCount)
                {
                    for (int i = 0; i < json["Root"]["User"].Count(); i++)
                    {
                        var cc = json["Root"]["User"][i];
                        LocalUserInfo.Add(new LocalUserInfo()
                        {
                            Index = index + 1,
                            Phone = cc["Phone"].ToString(),
                            Pwd =cc["Pwd"].ToString(),
                        });
                        index++;
                    }
                }
                else
                {
                    var cc = json["Root"]["User"];
                    LocalUserInfo.Add(new LocalUserInfo()
                    {
                        Index = index + 1,
                        Phone = cc["Phone"].ToString(),
                        Pwd = cc["Pwd"].ToString(),
                    });
                }
               
            }
        }

        #endregion

        private void DelSaveUser(object[] x)
        {
            Button Btn= x[0] as Button;
            int _index= JObjectHelper.GetStrNum(Btn.Tag.ToString())-1;
            string UserPhone = LocalUserInfo[_index].Phone;
            LocalUserInfo.RemoveAt(_index);
            //删除本地用户
            XElement xele=XElement.Load(Path.Combine(localUserInfoFilepath, FileName));
            //MessageBox.Show(xele.Element("Phone").Value)
            var item = (from ele in xele.Elements("User")
                       where (string)ele.Element("Phone")==UserPhone
                        select ele).FirstOrDefault();
            if (item!=null)
            {
                item.Remove();
            }
            xele.Save(Path.Combine(localUserInfoFilepath, FileName));
            sort();
        }

        void sort()
        {
            int index = 0;
            for (int i = 0; i < LocalUserInfo.Count; i++)
            {
                LocalUserInfo[i].Index = index + 1;
                index++;
            }
        }

        void SelectPhone(object[] x)
        {
            ComboBox cmbBox=x[0] as ComboBox;
            Button phoneBtn=x[1] as Button;
            PasswordBox Psb = x[2] as PasswordBox;

            Page loginPage=x[3] as Page;
            CheckBox isKeepPwd= loginPage.FindName("IsKeepPwd") as CheckBox;
            isKeepPwd.IsChecked = true;

            List<WaterMarkTextBox> TextBox = DependencyObjectHelper.FindVisualChild<WaterMarkTextBox>(cmbBox);
            int index = JObjectHelper.GetStrNum(phoneBtn.Tag.ToString())-1;
            //cmbBox.Text = LocalUserInfo[index].Phone;
            TextBox[0].Text = LocalUserInfo[index].Phone;
            cmbBox.IsDropDownOpen = false;
            string[] SplitStr = LocalUserInfo[index].Pwd.Split('*');
            string _pwd = RsaHelper.RSADecrypt(SplitStr[0], SplitStr[1]);
            Psb.Password = _pwd;
        }

        #endregion

        #region 属性显示

        /// <summary>
        /// 登录是否成功
        /// </summary>
        private bool? _bo;

        public bool? Bo
        {
            get { return _bo; }
            set
            {
                _bo = value;
                RaisePropertyChanged("Bo");
            }
        }


        #endregion

    }

}
    
