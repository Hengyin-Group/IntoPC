using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Common
{
    public class JObjectHelper
    {
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStrNum(string str)
        {
            int Num = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
            return Num;
        }
        /// <summary>
        /// Newtensoft V4.0获取得到的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStr(string str)
        {
            string newStr = str.Replace("\"", "");
            return newStr;
        }
        /// <summary>
        /// 将JObject转的字符串转为Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double GetDouble(string str)
        {
            double doub = Convert.ToDouble(str.Replace("\"",""));
            return doub;
        }
    }
}
