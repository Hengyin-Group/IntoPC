using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IntoApp.Common
{
    public class BooleanHelper
    {
        public static Boolean? GetBoolean(object value)
        {
            string Str = value.ToString();
            if (string.IsNullOrEmpty(Str)||Str.ToUpper()=="NULL")
            {
                return null;
            }
            else
            {
                //判断是否全是数字
                Regex Numre=new Regex(@"^\d+$");
                bool Numbo= Numre.IsMatch(Str);
                //判断是否全是字母
                Regex Strre=new Regex(@"^\w+$");
                bool Strbo = Strre.IsMatch(Str);
                if (Numbo)
                {
                    int num = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(Str, @"[^0-9]+", ""));
                    if (num > 0)
                        return true;
                    else
                        return false;
                }
                else if (Strbo)
                {
                    if (Str.ToUpper() == "TRUE")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }
    }
}
