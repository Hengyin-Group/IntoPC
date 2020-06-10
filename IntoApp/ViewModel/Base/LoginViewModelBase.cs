using IntoApp.Bll;
using IntoApp.Common;
using MyMessageBox.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using IntoApp.Model;

namespace IntoApp.ViewModel.Base
{
    public class LoginViewModelBase:ViewModelBase
    {
        public Account account = new Account();
        public Thread thread;
        public string code { get; set; }

        #region 命令

        public MyCommand<Object[]> GetCodeCommand { get; set; }
        public MyCommand<string> TextChangeCommand { get; set; }
        public MyCommand<Object[]> NavigateCommand { get; set; }
        public MyCommand<TextCompositionEventArgs> PreviewTextInputCommand { get; set; }


        #endregion

        #region 方法

        #region 限定输入为数字

        public void PreViewTextInput(TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }

        #endregion

        #region 验证码输入6位

        public void TextChange(string Code_text)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (Code_text.Length == 6)
                {
                    if (Code_text == code)
                    {
                        PwdTextBoxIsEnabled = true;
                        BtnIsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("验证码输入错误");
                    }
                }
            }
            else
            {
                MessageBox.Show("验证码已失效,请重新发送验证码");
            }
        }

        #endregion

        #region 获取验证码

        public void getCode_btn(Object[] obj)
        {
            string Phone = obj[0].ToString();
            string Page_Btn = (obj[1] as Button).Tag.ToString();


            int len = Phone.Length;
            if (len == 11)
            {
                if (Page_Btn.ToLower()=="registercode")
                {
                    reg_IsExistPhone(Phone);
                }
                else
                {
                    forget_IsExistPhone(Phone);
                }
                
            }
            else
            {
                MessageBox.Show("请争取输入手机号");
            }
        }

        public void reg_IsExistPhone(string Phone)  //手机号是否已注册
        {
            string str = account.IsExistPhone(Phone);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
            {
                string GetSmsCodeStr = account.GetSmsCode(Phone);
                JObject CodeJo = (JObject)JsonConvert.DeserializeObject(GetSmsCodeStr);
                if (JObjectHelper.GetStrNum(CodeJo["code"].ToString()) == 200)
                {
                    thread = new Thread(GetCodeBtnText);
                    thread.Start();
                    code = CodeJo["message"].ToString();
                }
                else
                {
                    MessageBox.Show("验证码发送失败");
                }
            }
            else
            {
                MessageBox.Show(jo["message"].ToString());
            }
        }

        public void forget_IsExistPhone(string Phone)  //手机号是否已注册
        {
            string IsExistPhoneStr = account.IsExistPhone(Phone);
            JObject jo = (JObject)JsonConvert.DeserializeObject(IsExistPhoneStr);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) != 200)    //这里和注册时不一样
            {
                string GetSmsCodeStr = account.GetSmsCode(Phone);
                JObject CodeJo = (JObject)JsonConvert.DeserializeObject(GetSmsCodeStr);
                if (JObjectHelper.GetStrNum(CodeJo["code"].ToString()) == 200)
                {
                    thread = new Thread(GetCodeBtnText);
                    thread.Start();
                    code = CodeJo["message"].ToString();
                }
                else
                {
                    MessageBox.Show("验证码发送失败");
                }
            }
            else
            {
                MessageBox.Show("该号码还未注册");
            }
        }
        /// <summary>
        /// 获取验证码按钮文字及删除验证码
        /// </summary>
        void GetCodeBtnText()
        {
            GetCodeIsEnabled = false;
            for (int i = 60 - 1; i >= 0; i--)
            {
                GetCodeText = $"{i}s";
                Thread.Sleep(1000);
            }
            code = String.Empty;
            GetCodeText = "获取验证码";
            GetCodeIsEnabled = true;
        }


        #endregion

        #endregion

        #region 属性

        /// <summary>
        /// 密码框是否可用
        /// </summary>
        private bool _pwdTextBoxIsEnabled = false;
        public bool PwdTextBoxIsEnabled
        {
            get { return _pwdTextBoxIsEnabled; }
            set
            {
                _pwdTextBoxIsEnabled = value;
                RaisePropertyChanged("PwdTextBoxIsEnabled");
            }
        }

        /// <summary>
        /// 验证码按钮的文字
        /// </summary>

        private string _getCodeText = "获取验证码";

        public string GetCodeText
        {
            get { return _getCodeText; }
            set
            {
                _getCodeText = value;
                RaisePropertyChanged("GetCodeText");
            }
        }

        /// <summary>
        /// 验证码能否使用
        /// </summary>
        private bool _getCodeIsEnabled = true;
        public bool GetCodeIsEnabled
        {
            get { return _getCodeIsEnabled; }
            set
            {
                _getCodeIsEnabled = value;
                RaisePropertyChanged("GetCodeIsEnabled");
            }
        }

        /// <summary>
        /// regBtn按钮是否能用
        /// </summary>
        private bool _BtnIsEnabled = false;

        public bool BtnIsEnabled
        {
            get { return _BtnIsEnabled; }
            set
            {
                _BtnIsEnabled = value;
                RaisePropertyChanged("BtnIsEnabled");
            }
        }

        

        #endregion

        private ObservableCollection<LocalUserInfo> _localUserInfo;
        public ObservableCollection<LocalUserInfo> LocalUserInfo
        {
            get
            {
                if (_localUserInfo==null)
                {
                    _localUserInfo=new ObservableCollection<LocalUserInfo>();
                }
                return _localUserInfo;
            }
            set
            {
                _localUserInfo = value;
                RaisePropertyChanged("LocalUserInfo");
            }
        }
    }
}
