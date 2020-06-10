using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace IntoApp.ViewModel
{
    public class PageForgetPwdViewModel:LoginViewModelBase
    {

        public PageForgetPwdViewModel()
        {
            RunState = false;

            GetCodeCommand =new MyCommand<Object[]>(x=>getCode_btn(x));
            TextChangeCommand=new MyCommand<string>(x=>TextChange(x));
            ForgetPwdCommand=new MyCommand<object[]>(x=>Forget_Click(x));
            NavigateCommand=new MyCommand<object[]>(x=>Navigate(x));
            PreviewTextInputCommand=new MyCommand<TextCompositionEventArgs>(x=>PreViewTextInput(x));

        }

        #region 命令

        public MyCommand<object[]> ForgetPwdCommand { get; set; }

        #endregion

        #region 方法
        private void Forget_Click(object[] obj)
        {
            string Phone = obj[0].ToString();
            PasswordBox pwd = obj[1] as PasswordBox;
            Page page = obj[2] as Page;
            string Pwd = pwd.Password;
            bool bo = Pwd.Length >= 6 ? true : true;
            if (bo)
            {
                RunState = true;
                string ForgetCallBack = account.ReviewPwd(Phone, Pwd);
                JObject ForgetCallBackJo = (JObject)JsonConvert.DeserializeObject(ForgetCallBack);
                if (JObjectHelper.GetStrNum(ForgetCallBackJo["code"].ToString()) == 200)
                {
                    //Button Btn = new Button
                    //{
                    //    Tag = "Login",
                    //};
                    //LoginHelper.LoginNavigate(Btn, page);
                    RunState = false;
                    MessageBox.Show("密码修改成功");
                }
                else
                {
                    RunState = false;
                    MessageBox.Show(ForgetCallBackJo["message"].ToString());
                }
            }
            else
            {
                MessageBox.Show("密码长度不能小于6位");
            }

        }

        #endregion

    }
}
