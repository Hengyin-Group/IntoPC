using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoApp.View.Login;

namespace IntoApp.API
{
    public class LoginPageManager
    {
        public static PageLogin PageLogin=new PageLogin();

        public static PageRegister PageRegister=new PageRegister();

        public static PageForgetPwd PageForgetPwd=new PageForgetPwd();

        public static PageLoginLoading PageLoginLoading=new PageLoginLoading();

    }
}
