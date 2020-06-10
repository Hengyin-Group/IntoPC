using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace IntoApp.Common
{
    /// <summary>
    /// XML转换帮助类
    /// </summary>
    public class XmlConvertHelper
    {
        /// <summary>
        /// 将一个对象序列化为XML字符串。
        ///
        /// 参数：
        /// obj，object对象；
        /// encoding，编码方式，一般用UTF-8
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ObjectToXMLStr(object obj, Encoding encoding)
        {
            string str = null;

            MemoryStream stream = null;
            StreamReader reader = null;
            XmlWriter writer = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = encoding;
                settings.IndentChars = "\t";

                stream = new MemoryStream();
                writer = XmlWriter.Create(stream, settings);
                serializer.Serialize(writer, obj);
                stream.Position = 0;
                reader = new StreamReader(stream, encoding);
                str = reader.ReadToEnd();
            }
            catch (Exception)
            {
                str = null;
                throw;
            }
            finally
            {
                writer.Close();
                reader.Close();
                stream.Close();
            }

            return str;
        }



        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件（推荐用XML文件）。
        /// 
        /// 参数：
        /// obj，object对象；
        /// path，文件绝对路径；
        /// encoding，编码方式，一般用UTF-8。
        /// 返回值：
        /// bool类型，true表示操作成功，false表示失败
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public static bool ObjectToXMLFile(object obj, string path, Encoding encoding)
        {
            bool b = false;

            if (obj == null)
            {
                throw new ArgumentNullException("参数obj不能为null，请传入有效的对象");
            }
            if (path == null)
            {
                throw new ArgumentNullException("参数path（文件路径）不能为null");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("参数encoding不能为null，请确定有效的字符编码方式。");
            }

            FileStream fs = null;
            XmlWriter writer = null;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);

                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = encoding;
                settings.IndentChars = "\t";

                writer = XmlWriter.Create(fs, settings);

                serializer.Serialize(writer, obj);

                b = true;
            }
            catch (Exception)
            {
                b = false;
                throw;
            }
            finally
            {
                writer.Close();
                fs.Close();
            }

            return b;
        }



        /// <summary>
        /// 读取一个文件（推荐用XML文件），并按XML的方式反序列化对象。
        /// 
        /// 参数：
        /// path，文件绝对路径；
        /// encoding，编码方式，一般用UTF-8。
        /// 返回值：
        /// object对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XMLFlieToObject<T>(string path, Encoding encoding)
        {
            T T_object = default(T);

            if (path == null || !File.Exists(path))
            {
                throw new ArgumentNullException("系统找不到你指定的文件。");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("参数encoding不能为null，请确定有效的字符编码方式。");
            }

            try
            {
                string xml = File.ReadAllText(path, encoding);
                T_object = XMLStrToObject<T>(xml);
            }
            catch (Exception)
            {
                throw;
            }

            return T_object;
        }



        /// <summary>
        /// 从XML字符串中反序列化对象。
        ///
        /// 参数：
        /// str，XML字符串；
        /// encoding，编码方式，一般用UTF-8。
        /// 返回值：
        /// object对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T XMLStrToObject<T>(string str)
        {
            T T_object = default(T);

            if (str == null || str == "")
            {
                throw new ArgumentNullException("参数str不能为空值，请传入有效的XML字符串。");
            }

            TextReader reader = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                reader = new StringReader(str);
                T_object = (T)xmlSerializer.Deserialize(reader);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }

            return T_object;
        }


    }
}

