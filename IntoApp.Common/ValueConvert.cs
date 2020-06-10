using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IntoApp.Common
{
    public static partial class ValueConvert
    {
        /// <summary>
        /// 得到对象Int的值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int GetInt(this object Value)
        {
            return GetInt(Value, 0);
        }

        public static int GetInt(this object Value, int defaultValue)
        {
            if (Value == null) return defaultValue;
            if (Value is string && Value.GetString().HasValue() == false) return defaultValue;
            if (Value is DBNull) return defaultValue;
            if ((Value is string) == false && (Value is IConvertible) == true)
                return (Value as IConvertible).ToInt32(CultureInfo.CurrentCulture);
            int retVal = defaultValue;
            if (int.TryParse(Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out retVal))
            {
                return retVal;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 得到对象的String值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetString(this object Value)
        {
            return GetString(Value, string.Empty);
        }

        public static string GetString(this object Value, string defaultValue)
        {
            if (Value == null) return defaultValue;
            string retVal = defaultValue;
            try
            {
                var strValue = Value as string;
                if (strValue != null) return strValue;

                char[] chrs = Value as char[];
                if (chrs != null) return new string(chrs);

                retVal = Value.ToString();
            }
            catch (Exception e)
            {
                return defaultValue;
            }
            return retVal;
        }

        public static bool GetBool(this object Value)
        {
            return GetBool(Value, false);
        }

        public static bool GetBool(this object Value, bool defaultValue)
        {
            if (Value == null) return defaultValue;
            if (Value is string && Value.GetString().HasValue() == false) return defaultValue;
            if ((Value is string) == false && (Value is IConvertible) == true)
            {
                if (Value is DBNull) return defaultValue;
                try
                {
                    return (Value as IConvertible).ToBoolean(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                }
            }

            if (Value is string)
            {
                if (Value.GetString() == "0") return false;
                if (Value.GetInt() >= 1) return true;
                if (Value.GetString().ToLower() == "yes") return true;
                if (Value.GetString().ToLower() == "no") return false;
            }

            bool retValue = defaultValue;
            if (bool.TryParse(Value.GetString(), out retValue))
            {
                return retValue;
            }
            else return defaultValue;
        }


        /// <summary>
        /// 检测Value是否包含有效值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool HasValue(this string Value)
        {
            if (Value != null)
            {
                return !string.IsNullOrEmpty(Value.ToString());
            }
            else return false;
        }
    }
}
