using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IntoApp.Common
{
    public partial class ReflectHelper
    {
        /// <summary>
        /// 判断属性是否存在
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool JudgeHasProperty(string PropertyName, object o)
        {
            if (o == null)
            {
                o = new { };
            }

            PropertyInfo[] p1 = o.GetType().GetProperties();
            bool b = false;
            foreach (PropertyInfo pi in p1)
            {
                if (pi.Name.ToLower() == PropertyName.ToLower())
                {
                    b = true;
                }
            }
            return b;
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="o"></param>
        /// <returns></returns>

        public static object GetPropertyValueByName(string PropertyName, object o)
        {
            if (o == null)
            {
                o = new { };
            }
            object returnObject = new object();
            PropertyInfo[] p1 = o.GetType().GetProperties();
            foreach (PropertyInfo pi in p1)
            {
                if (pi.Name.ToLower() == PropertyName.ToLower())
                {
                    returnObject = pi.GetValue(o,null);
                }
            }

            return returnObject;
        }
        /// <summary>
        /// 遍历属性值
        /// </summary>
        /// <param name="o"></param>

        public static ArrayList ForeachPropertyValue(object o)
        {
            if (o == null)
            {
                o = new { };
            }
            ArrayList arrayList = new ArrayList();
            PropertyInfo[] p1 = o.GetType().GetProperties();
            foreach (PropertyInfo pi in p1)
            {
                arrayList.Add(pi.Name + ":" + pi.GetValue(o, null));
                //Console.WriteLine(pi.Name+":"+pi.GetValue(o,null));
            }
            return arrayList;
        }

    }
}
