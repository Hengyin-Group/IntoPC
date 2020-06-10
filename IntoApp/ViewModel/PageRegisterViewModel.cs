using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.utils;
using IntoApp.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using MessageBox=MyMessageBox.Controls.MessageBox;

namespace IntoApp.ViewModel
{
    public class PageRegisterViewModel:LoginViewModelBase
    {
        
        public PageRegisterViewModel()
        {

            RunState = false;

            GetCodeCommand=new MyCommand<Object[]>(x=>getCode_btn(x));
            TextChangeCommand=new MyCommand<string>(x=>TextChange(x));
            RegisterCommand=new MyCommand<object[]>(x => Reg_Click(x));
            NavigateCommand=new MyCommand<object[]>(x=>Navigate(x));
            PreviewTextInputCommand=new MyCommand<TextCompositionEventArgs>(x=>PreViewTextInput(x));

        }

        #region 命令

        public MyCommand<object[]> RegisterCommand { get; set; }

        #endregion

        #region 方法

        private void Reg_Click(object[] obj)
        {
            ///*****
            
            string Phone = obj[0].ToString();
            PasswordBox pwd = obj[1] as PasswordBox;
            Page page = obj[2] as Page;
            string Pwd = pwd.Password;
            ///****
            bool bo = Pwd.Length >= 6 ? true : false;
            if (bo)
            {
                RunState = true;
                string RegCallBack = account.Regist(Phone, "", Pwd); //中间参数为注册时送的金额的验证码，可空
                JObject RegCallBackJo = (JObject)JsonConvert.DeserializeObject(RegCallBack);
                if (JObjectHelper.GetStrNum(RegCallBackJo["code"].ToString()) == 200) //注册成功
                {
                    RunState = false;
                    MessageBox.Show("注册成功");
                }
                else
                {
                    RunState = true;
                    MessageBox.Show(RegCallBackJo["message"].ToString());
                }
            }
            else
            {
                MessageBox.Show("密码长度不能小于6位");
            }
        }

        #endregion

    }

    public class RegInfo
    {
        public static string Phone { get; set; }
        public static string Pwd { get; set; }
    }
}
