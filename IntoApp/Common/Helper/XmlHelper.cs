using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace IntoApp.Common.Helper
{
    public class XmlHelper
    {
        /// <summary>
        /// 创建Xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateXml(string path,string fileName)
        {
            XmlDocument xmlDoc=new XmlDocument();
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            XmlNode root = xmlDoc.CreateElement("Root");
            xmlDoc.AppendChild(root);
            try
            {
                xmlDoc.Save(Path.Combine(path,fileName));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }



        /// <summary>
        /// XML转换为JSON（忽略XML Node Attribute）
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ParseXmlToJson(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return string.Empty;
            Func<XElement, int?, string> _fun = null;
            _fun = (XElement node, int? nodeCount) =>
            {
                int thisNodeCount = nodeCount ?? 1;
                StringBuilder s = new StringBuilder();
                int count = node.Elements().Count();
                if (!node.HasElements)
                {
                    if (thisNodeCount <= 1)
                    {
                        s.AppendFormat("\"{0}\":\"{1}\"", node.Name.LocalName, node.Value);
                    }
                    else
                    {
                        s.AppendFormat("\"{0}\"", node.Value);
                    }
                }
                else
                {

                    if (count == 1)
                    {
                        var childNode = node.Elements().FirstOrDefault();
                        s.AppendFormat("\"{0}\":{1}", node.Name.LocalName, _fun(childNode, childNode == null ? 0 : 1));
                    }
                    else
                    {
                        //检测子元素localName是否全部一致，如果一致则使用[]集合，否则为{}
                        //抽取Localname唯一
                        var LocalNames = node.Elements().Select(t => t.Name.LocalName).Distinct();
                        string NodesJson = string.Empty;
                        NodesJson += string.Format("\"{0}\":", node.Name.LocalName);
                        int ChildNameCount = node.Elements().Select(t => t.Name.LocalName).Distinct().Count();
                        #region 如果存在重复节点，则合并同类项,格式为key:[]
                        if (ChildNameCount != node.Elements().Count())
                        {
                            //子元素有重复,递归节点
                           //NodesJson += "[";
                            foreach (string key in LocalNames)
                            {
                                NodesJson += string.Format("{{\"{0}\":[", key);
                                foreach (var nd in node.Elements(key))
                                {
                                    NodesJson += _fun(nd, node.Elements(key).Count()) + ",";
                                }
                                NodesJson = NodesJson.TrimEnd(new char[] { ',' });
                                NodesJson += "]},";
                            }
                            NodesJson = NodesJson.TrimEnd(new char[] { ',' });
                            //NodesJson += "]";
                            s.AppendFormat("{0}", NodesJson);
                        }
                        #endregion
                        #region 如果无重复节点，则不合并
                        else
                        {
                            //子元素无重复
                            NodesJson = "{";
                            foreach (var nd in node.Elements())
                            {
                                string json = _fun(nd, 1);
                                NodesJson += json + ",";
                            }
                            NodesJson = NodesJson.TrimEnd(new char[] { ',' });
                            NodesJson += "}";
                            //判断父节点，同名项数量
                            if (thisNodeCount <= 1)
                            {
                                s.AppendFormat("\"{0}\":{1}", node.Name.LocalName, NodesJson);
                            }
                            else
                            {
                                s.AppendFormat("{0}", NodesJson);
                            }
                        }
                        #endregion
                    }
                }
                return s.ToString();
            };
            XDocument xd = XDocument.Parse(xml);
            if (xd == null) return string.Empty;
            return string.Format("{{{0}}}", _fun(xd.Root, null));
        }

    }
}
