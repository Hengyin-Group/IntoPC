using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.utils
{
    public class DateTimeHelper
    {
        public static string GetDateTime(string Time)
        {
            DateTime dateTime = Convert.ToDateTime(Time.Replace("T", " ").Split('.')[0]);
            string time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            return time;
        }

        /// <summary>
        /// 可空类型
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static DateTime? StringToDateTime(string Time)
        {
            if (string.IsNullOrEmpty(Time)||Time.ToUpper()=="NUll")
            {
                return null;
            }
            else
            {
                DateTime dateTime = Convert.ToDateTime(Time.Replace("T", " ").Split('.')[0]);
                //string time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                return dateTime;
            }
        }
    }
}
