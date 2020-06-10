using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Linq;

namespace IntoApp.Common
{
    /// <summary>
    /// 对XML文件进行处理的类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 创建Xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateXml(string path, string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            XmlNode root = xmlDoc.CreateElement("Root");
            xmlDoc.AppendChild(root);
            try
            {
                xmlDoc.Save(Path.Combine(path, fileName));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }



        /// <summary>
        /// XML转换为JSON  转换有问题（忽略XML Node Attribute）
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



        #region 获得Xml文档对象的方法
        /// <summary>
        /// 获得Xml文档对象的方法
        /// </summary>
        /// <param name="fullFileName">XML完整文件名</param>
        /// <returns>Xml文档对象</returns>
        public static XmlDocument GetXmlDocument(string fullFileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fullFileName);
                return doc;
            }
            catch
            {
                throw new Exception("XML文件" + fullFileName + "不存在或拒绝访问。");
            }
        }
        #endregion

        #region 根据标记名称获得数据集合的方法
        /// <summary>
        /// 根据标记名称获得数据集合的方法
        /// </summary>
        /// <param name="fileName">XML完整文件名</param>
        /// <param name="tagName">标记名称</param>
        /// <returns>数据集合</returns>
        public static ArrayList GetArrayListByTagName(string fileName, string tagName)
        {
            XmlDocument doc = GetXmlDocument(fileName);
            XmlNodeList nodeList = doc.DocumentElement.GetElementsByTagName(tagName);
            ArrayList al = new ArrayList();
            for (int i = 0; i < nodeList.Count; i++)
            {
                al.Add(nodeList[i].InnerText);
            }
            return al;
        }
        /// <summary>
        /// 将XmlDocument转化为Dataset
        /// </summary>
        /// <param name="doc">xml文档对象</param>
        /// <returns></returns>
        public static DataSet GetDataSetByXmlDocument(XmlDocument doc)
        {
            XmlNodeReader reader = new XmlNodeReader(doc);
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            reader.Close();
            return ((ds != null) && (ds.Tables[0].Rows.Count > 0)) ? ds : null;
        }
        #endregion

        #region  xml操作公共类

        #region 字段定义
        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath;
        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;
        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;
        #endregion

        #region 构造方法
        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        public XmlHelper(string xmlFilePath)
        {
            //获取XML文件的绝对路径
            _filePath = GetPath(xmlFilePath);
        }//SysHelper.
        #endregion

        #region 创建XML的根节点
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {
            /*
            //创建一个XML对象
            _xml = new XmlDocument();

            if (DirFile.IsExistFile(_filePath))
            {
                //加载XML文件
                _xml.Load(this._filePath);
            }

            //为XML的根节点赋值
            _element = _xml.DocumentElement;
          */
        }
        #endregion

        #region 获取指定XPath表达式的节点对象
        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点
            return _element.SelectSingleNode(xPath);
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的值
            return _element.SelectSingleNode(xPath).InnerText;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的属性值
            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #region 新增节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            CreateXMLElement();

            //导入节点
            XmlNode node = _xml.ImportNode(xmlNode, true);

            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>        
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            //创建XmlDataDocument对象
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

            //导入节点
            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

            //将节点插入到根节点下
            AppendNode(node);
        }
        #endregion

        #region 删除节点
        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //获取要删除的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            //删除节点
            _element.RemoveChild(node);
        }
        #endregion //删除节点

        #region 保存XML文件
        /// <summary>
        /// 保存XML文件
        /// </summary>        
        public void Save()
        {
            //创建XML的根节点
            CreateXMLElement();

            //保存XML文件
            _xml.Save(this._filePath);
        }
        #endregion //保存XML文件

        #region 静态方法

        #region 创建根节点对象
        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>        
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = GetPath(xmlFilePath);//SysHelper.

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的值
            return rootElement.SelectSingleNode(xPath).InnerText;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #endregion

        #endregion


        #region  xml操作过程类

        #region 构造函数
        public XmlHelper()
        { }

        //public XmlHelper(string strPath)
        //{
        //    this._XMLPath = strPath;
        //}
        #endregion

        #region 公有属性
        private string _XMLPath;
        public string XMLPath
        {
            get { return this._XMLPath; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 导入XML文件
        /// </summary>
        /// <param name="XMLPath">XML文件路径</param>
        private XmlDocument XMLLoad()
        {
            string XMLFile = XMLPath;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + XMLFile;
                if (File.Exists(filename)) xmldoc.Load(filename);
            }
            catch (Exception e)
            { }
            return xmldoc;
        }

        /// <summary>
        /// 导入XML文件
        /// </summary>
        /// <param name="XMLPath">XML文件路径</param>
        private static XmlDocument XMLLoad(string strPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + strPath;
                if (File.Exists(filename)) xmldoc.Load(filename);
            }
            catch (Exception e)
            { }
            return xmldoc;
        }

        /// <summary>
        /// 返回完整路径
        /// </summary>
        /// <param name="strPath">Xml的路径</param>
        private static string GetXmlFullPath(string strPath)
        {
            if (strPath.IndexOf(":") > 0)
            {
                return strPath;
            }
            else
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
        }
        #endregion

        #region 读取数据
        /// <summary>
        /// 读取指定节点的数据
        /// </summary>
        /// <param name="node">节点</param>
        /// 使用示列:
        /// XMLProsess.Read("/Node", "")
        /// XMLProsess.Read("/Node/Element[@Attribute='Name']")
        public string Read(string node)
        {
            string value = "";
            try
            {
                XmlDocument doc = XMLLoad();
                XmlNode xn = doc.SelectSingleNode(node);
                value = xn.InnerText;
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 读取指定路径和节点的串联值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// 使用示列:
        /// XMLProsess.Read(path, "/Node", "")
        /// XMLProsess.Read(path, "/Node/Element[@Attribute='Name']")
        public static string Read(string path, string node)
        {
            string value = "";
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = xn.InnerText;
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 读取指定路径和节点的属性值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// 使用示列:
        /// XMLProsess.Read(path, "/Node", "")
        /// XMLProsess.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 获取某一节点的所有孩子节点的值
        /// </summary>
        /// <param name="node">要查询的节点</param>
        public string[] ReadAllChildallValue(string node)
        {
            int i = 0;
            string[] str = { };
            XmlDocument doc = XMLLoad();
            XmlNode xn = doc.SelectSingleNode(node);
            XmlNodeList nodelist = xn.ChildNodes;  //得到该节点的子节点
            if (nodelist.Count > 0)
            {
                str = new string[nodelist.Count];
                foreach (XmlElement el in nodelist)//读元素值
                {
                    str[i] = el.Value;
                    i++;
                }
            }
            return str;
        }

        /// <summary>
        /// 获取某一节点的所有孩子节点的值
        /// </summary>
        /// <param name="node">要查询的节点</param>
        public XmlNodeList ReadAllChild(string node)
        {
            XmlDocument doc = XMLLoad();
            XmlNode xn = doc.SelectSingleNode(node);
            XmlNodeList nodelist = xn.ChildNodes;  //得到该节点的子节点
            return nodelist;
        }

        /// <summary> 
        /// 读取XML返回经排序或筛选后的DataView
        /// </summary>
        /// <param name="strWhere">筛选条件，如:"name='kgdiwss'"</param>
        /// <param name="strSort"> 排序条件，如:"Id desc"</param>
        public DataView GetDataViewByXml(string strWhere, string strSort)
        {
            try
            {
                string XMLFile = this.XMLPath;
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + XMLFile;
                DataSet ds = new DataSet();
                ds.ReadXml(filename);
                DataView dv = new DataView(ds.Tables[0]); //创建DataView来完成排序或筛选操作	
                if (strSort != null)
                {
                    dv.Sort = strSort; //对DataView中的记录进行排序
                }
                if (strWhere != null)
                {
                    dv.RowFilter = strWhere; //对DataView中的记录进行筛选，找到我们想要的记录
                }
                return dv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 读取XML返回DataSet
        /// </summary>
        /// <param name="strXmlPath">XML文件相对路径</param>
        public DataSet GetDataSetByXml(string strXmlPath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 插入数据
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Insert(path, "/Node", "Element", "", "Value")
        /// XMLProsess.Insert(path, "/Node", "Element", "Attribute", "Value")
        /// XMLProsess.Insert(path, "/Node", "", "Attribute", "Value")
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="strList">由XML属性名和值组成的二维数组</param>
        public static void Insert(string path, string node, string element, string[][] strList)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = doc.CreateElement(element);
                string strAttribute = "";
                string strValue = "";
                for (int i = 0; i < strList.Length; i++)
                {
                    for (int j = 0; j < strList[i].Length; j++)
                    {
                        if (j == 0)
                            strAttribute = strList[i][j];
                        else
                            strValue = strList[i][j];
                    }
                    if (strAttribute.Equals(""))
                        xe.InnerText = strValue;
                    else
                        xe.SetAttribute(strAttribute, strValue);
                }
                xn.AppendChild(xe);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="strXmlPath">XML文件相对路径</param>
        /// <param name="Columns">要插入行的列名数组，如：string[] Columns = {"name","IsMarried"};</param>
        /// <param name="ColumnValue">要插入行每列的值数组，如：string[] ColumnValue={"XML大全","false"};</param>
        /// <returns>成功返回true,否则返回false</returns>
        public static bool WriteXmlByDataSet(string strXmlPath, string[] Columns, string[] ColumnValue)
        {
            try
            {
                //根据传入的XML路径得到.XSD的路径，两个文件放在同一个目录下
                string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(GetXmlFullPath(strXsdPath)); //读XML架构，关系到列的数据类型
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                DataTable dt = ds.Tables[0];
                DataRow newRow = dt.NewRow();                 //在原来的表格基础上创建新行
                for (int i = 0; i < Columns.Length; i++)      //循环给一行中的各个列赋值
                {
                    newRow[Columns[i]] = ColumnValue[i];
                }
                dt.Rows.Add(newRow);
                dt.AcceptChanges();
                ds.AcceptChanges();
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 修改数据
        /// <summary>
        /// 修改指定节点的数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="value">值</param>
        public void Update(string node, string value)
        {
            try
            {
                XmlDocument doc = XMLLoad();
                XmlNode xn = doc.SelectSingleNode(node);
                xn.InnerText = value;
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + XMLPath);
            }
            catch { }
        }

        /// <summary>
        /// 修改指定节点的数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Insert(path, "/Node","Value")
        /// XMLProsess.Insert(path, "/Node","Value")
        public static void Update(string path, string node, string value)
        {
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.InnerText = value;
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 修改指定节点的属性值(静态)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Insert(path, "/Node", "", "Value")
        /// XMLProsess.Insert(path, "/Node", "Attribute", "Value")
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 更改符合条件的一条记录
        /// </summary>
        /// <param name="strXmlPath">XML文件路径</param>
        /// <param name="Columns">列名数组</param>
        /// <param name="ColumnValue">列值数组</param>
        /// <param name="strWhereColumnName">条件列名</param>
        /// <param name="strWhereColumnValue">条件列值</param>
        public static bool UpdateXmlRow(string strXmlPath, string[] Columns, string[] ColumnValue, string strWhereColumnName, string strWhereColumnValue)
        {
            try
            {
                string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(GetXmlFullPath(strXsdPath));//读XML架构，关系到列的数据类型
                ds.ReadXml(GetXmlFullPath(strXmlPath));

                //先判断行数
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //如果当前记录为符合Where条件的记录
                        if (ds.Tables[0].Rows[i][strWhereColumnName].ToString().Trim().Equals(strWhereColumnValue))
                        {
                            //循环给找到行的各列赋新值
                            for (int j = 0; j < Columns.Length; j++)
                            {
                                ds.Tables[0].Rows[i][Columns[j]] = ColumnValue[j];
                            }
                            ds.AcceptChanges();                     //更新DataSet
                            ds.WriteXml(GetXmlFullPath(strXmlPath));//重新写入XML文件
                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除节点值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Delete(path, "/Node", "")
        /// XMLProsess.Delete(path, "/Node", "Attribute")
        public static void Delete(string path, string node)
        {
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.ParentNode.RemoveChild(xn);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Delete(path, "/Node", "")
        /// XMLProsess.Delete(path, "/Node", "Attribute")
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = XMLLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 删除所有行
        /// </summary>
        /// <param name="strXmlPath">XML路径</param>
        public static bool DeleteXmlAllRows(string strXmlPath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                }
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 通过删除DataSet中指定索引行，重写XML以实现删除指定行
        /// </summary>
        /// <param name="iDeleteRow">要删除的行在DataSet中的Index值</param>
        public static bool DeleteXmlRowByIndex(string strXmlPath, int iDeleteRow)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows[iDeleteRow].Delete();
                }
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除指定列中指定值的行
        /// </summary>
        /// <param name="strXmlPath">XML相对路径</param>
        /// <param name="strColumn">列名</param>
        /// <param name="ColumnValue">指定值</param>
        public static bool DeleteXmlRows(string strXmlPath, string strColumn, string[] ColumnValue)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //判断行多还是删除的值多，多的for循环放在里面
                    if (ColumnValue.Length > ds.Tables[0].Rows.Count)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ColumnValue.Length; j++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < ColumnValue.Length; j++)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    ds.WriteXml(GetXmlFullPath(strXmlPath));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region  辅助项

        #region 获取文件相对路径映射的物理路径
        /// <summary>
        /// 获取文件相对路径映射的物理路径
        /// </summary>
        /// <param name="virtualPath">文件的相对路径</param>        
        public static string GetPath(string virtualPath)
        {

            return HttpContext.Current.Server.MapPath(virtualPath);

        }
        #endregion

        #endregion

        #region 新添加有待更新

        #region 公共变量

        XmlDocument xmldoc;

        XmlNode xmlnode;

        XmlElement xmlelem;

        #endregion

        #region 创建Xml文档

        /// <summary>

        /// 创建一个带有根节点的Xml文件

        /// </summary>

        /// <paramname="FileName">Xml文件名称</param>

        /// <paramname="rootName">根节点名称</param>

        /// <paramname="Encode">编码方式:gb2312，UTF-8等常见的</param>

        /// <paramname="DirPath">保存的目录路径</param>

        ///<returns></returns>

        public bool CreateXmlDocument(string FileName, string RootName, string Encode)

        {

            try

            {

                xmldoc = new XmlDocument();

                XmlDeclaration xmldecl;

                xmldecl = xmldoc.CreateXmlDeclaration("1.0", Encode, null);

                xmldoc.AppendChild(xmldecl);

                xmlelem = xmldoc.CreateElement("", RootName, "");

                xmldoc.AppendChild(xmlelem);

                xmldoc.Save(FileName);

                return true;

            }

            catch (Exception e)

            {

                return false;

                throw new Exception(e.Message);

            }

        }

        #endregion

        #region 常用操作方法(增删改)

        /// <summary>

        /// 插入一个节点和它的若干子节点

        /// </summary> 

        /// <paramname="XmlFile">Xml文件路径</param>

        /// <paramname="NewNodeName">插入的节点名称</param>

        /// <paramname="HasAttributes">此节点是否具有属性，True为有，False为无</param>

        /// <paramname="fatherNode">此插入节点的父节点,要匹配的XPath表达式(例如:"//节点名//子节点名)</param>

        /// <paramname="htAtt">此节点的属性，Key为属性名，Value为属性值</param>

        /// <paramname="htSubNode">子节点的属性，Key为Name,Value为InnerText</param>

        /// <returns>返回真为更新成功，否则失败</returns>

        public bool InsertNode(string XmlFile, string NewNodeName, bool HasAttributes, string fatherNode, Hashtable htAtt, Hashtable htSubNode)

        {

            try

            {

                xmldoc = new XmlDocument();

                xmldoc.Load(XmlFile);

                XmlNode root = xmldoc.SelectSingleNode(fatherNode);

                xmlelem = xmldoc.CreateElement(NewNodeName);

                if (htAtt != null && HasAttributes)//若此节点有属性，则先添加属性

                {

                    SetAttributes(xmlelem, htAtt);

                    SetNodes(xmlelem.Name, xmldoc, xmlelem, htSubNode);//添加完此节点属性后，再添加它的子节点和它们的InnerText

                }

                else

                {

                    SetNodes(xmlelem.Name, xmldoc, xmlelem, htSubNode);//若此节点无属性，那么直接添加它的子节点

                }

                root.AppendChild(xmlelem);

                xmldoc.Save(XmlFile);

                return true;

            }

            catch (Exception e)

            {

                throw new Exception(e.Message);

            }

        }

        /// <summary>

        /// 更新节点

        /// </summary>

        /// <paramname="XmlFile">Xml文件路径</param>

        /// <paramname="fatherNode">需要更新节点的上级节点,要匹配的XPath表达式(例如:"//节点名//子节点名)</param>

        /// <paramname="htAtt">需要更新的属性表，Key代表需要更新的属性，Value代表更新后的值</param>

        /// <param name="htSubNode">需要更新的子节点的属性表，Key代表需要更新的子节点名字Name,Value代表更新后的值InnerText</param>

        /// <returns>返回真为更新成功，否则失败</returns>

        public bool UpdateNode(string XmlFile, string fatherNode, Hashtable htAtt, Hashtable htSubNode)

        {

            try

            {

                xmldoc = new XmlDocument();

                xmldoc.Load(XmlFile);

                XmlNodeList root = xmldoc.SelectSingleNode(fatherNode).ChildNodes;

                UpdateNodes(root, htAtt, htSubNode);

                xmldoc.Save(XmlFile);

                return true;

            }

            catch (Exception e)

            {

                throw new Exception(e.Message);

            }

        }



        /// <summary>

        /// 删除指定节点下的子节点

        /// </summary>

        /// <paramname="XmlFile">Xml文件路径</param>

        /// <paramname="fatherNode">制定节点,要匹配的XPath表达式(例如:"//节点名//子节点名)</param>

        /// <returns>返回真为更新成功，否则失败</returns>

        public bool DeleteNodes(string XmlFile, string fatherNode)

        {

            try

            {

                xmldoc = new XmlDocument();

                xmldoc.Load(XmlFile);

                xmlnode = xmldoc.SelectSingleNode(fatherNode);

                xmlnode.RemoveAll();

                xmldoc.Save(XmlFile);

                return true;

            }

            catch (XmlException xe)

            {

                throw new XmlException(xe.Message);

            }

        }

        /*ｋｅｌｅｙｉ*/

        /// <summary>

        /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)

        /// </summary>

        /// <paramname="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <paramname="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>

        /// <returns>成功返回true,失败返回false</returns>

        public bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)

        {

            bool isSuccess = false;

            xmldoc = new XmlDocument();

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);

                if (xmlNode != null)

                {

                    //删除节点

                    xmldoc.ParentNode.RemoveChild(xmlNode);

                }

                xmldoc.Save(xmlFileName); //保存到XML文档

                isSuccess = true;

            }

            catch (Exception ex)

            {

                throw ex; //这里可以定义你自己的异常处理

            }

            return isSuccess;

        }



        /// <summary>

        /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性

        /// </summary>

        /// <paramname="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <paramname="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>

        /// <paramname="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>

        /// <returns>成功返回true,失败返回false</returns>

        public bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)

        {

            bool isSuccess = false;

            bool isExistsAttribute = false;

            xmldoc = new XmlDocument();

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);

                XmlAttribute xmlAttribute = null;

                if (xmlNode != null)

                {

                    //遍历xpath节点中的所有属性

                    foreach (XmlAttribute attribute in xmlNode.Attributes)

                    {

                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())

                        {

                            //节点中存在此属性

                            xmlAttribute = attribute;

                            isExistsAttribute = true;

                            break;

                        }

                    }

                    if (isExistsAttribute)

                    {

                        //删除节点中的属性

                        xmlNode.Attributes.Remove(xmlAttribute);

                    }

                }

                xmldoc.Save(xmlFileName); //保存到XML文档

                isSuccess = true;

            }

            catch (Exception ex)

            {

                throw ex; //这里可以定义你自己的异常处理

            }

            return isSuccess;

        }



        /// <summary>

        /// 删除匹配XPath表达式的第一个节点中的所有属性

        /// </summary>

        /// <paramname="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>

        /// <returns>成功返回true,失败返回false</returns>

        public bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)

        {

            bool isSuccess = false;

            xmldoc = new XmlDocument();

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);

                if (xmlNode != null)

                {

                    //遍历xpath节点中的所有属性

                    xmlNode.Attributes.RemoveAll();

                }

                xmldoc.Save(xmlFileName); //保存到XML文档

                isSuccess = true;

            }

            catch (Exception ex)

            {

                throw ex; //这里可以定义你自己的异常处理

            }

            return isSuccess;

        }

        #endregion

        #region 私有方法

        /// <summary>

        /// 设置节点属性

        /// </summary>

        /// <paramname="xe">节点所处的Element</param>

        /// <paramname="htAttribute">节点属性，Key代表属性名称，Value代表属性值</param>

        private void SetAttributes(XmlElement xe, Hashtable htAttribute)

        {

            foreach (DictionaryEntry de in htAttribute)

            {

                xe.SetAttribute(de.Key.ToString(), de.Value.ToString());

            }

        }

        /// <summary>

        /// 增加子节点到根节点下

        /// </summary>

        /// <paramname="rootNode">上级节点名称</param>

        /// <paramname="XmlDoc">Xml文档</param>

        /// <paramname="rootXe">父根节点所属的Element</param>

        /// <paramname="SubNodes">子节点属性，Key为Name值，Value为InnerText值</param>

        private void SetNodes(string rootNode, XmlDocument XmlDoc, XmlElement rootXe, Hashtable SubNodes)

        {

            if (SubNodes == null)

                return;

            foreach (DictionaryEntry de in SubNodes)

            {

                xmlnode = XmlDoc.SelectSingleNode(rootNode);

                XmlElement subNode = XmlDoc.CreateElement(de.Key.ToString());

                subNode.InnerText = de.Value.ToString();

                rootXe.AppendChild(subNode);

            }

        }

        /// <summary>

        /// 更新节点属性和子节点InnerText值。

        /// </summary>

        /// <paramname="root">根节点名字</param>

        /// <paramname="htAtt">需要更改的属性名称和值</param>

        /// <paramname="htSubNode">需要更改InnerText的子节点名字和值</param>

        private void UpdateNodes(XmlNodeList root, Hashtable htAtt, Hashtable htSubNode)

        {

            foreach (XmlNode xn in root)

            {

                xmlelem = (XmlElement)xn;

                if (xmlelem.HasAttributes)//如果节点如属性，则先更改它的属性

                {

                    foreach (DictionaryEntry de in htAtt)//遍历属性哈希表

                    {

                        if (xmlelem.HasAttribute(de.Key.ToString()))//如果节点有需要更改的属性

                        {

                            xmlelem.SetAttribute(de.Key.ToString(), de.Value.ToString());//则把哈希表中相应的值Value赋给此属性Key

                        }

                    }

                }

                if (xmlelem.HasChildNodes)//如果有子节点，则修改其子节点的InnerText

                {

                    XmlNodeList xnl = xmlelem.ChildNodes;

                    foreach (XmlNode xn1 in xnl)

                    {

                        XmlElement xe = (XmlElement)xn1;

                        foreach (DictionaryEntry de in htSubNode)

                        {

                            if (xe.Name == de.Key.ToString())//htSubNode中的key存储了需要更改的节点名称，

                            {

                                xe.InnerText = de.Value.ToString();//htSubNode中的Value存储了Key节点更新后的数据

                            }

                        }

                    }

                }

            }

        }

        #endregion

        #region XML文档节点查询和读取(抛出错误)

        /**/

        /// <summary>

        /// 选择匹配XPath表达式的第一个节点XmlNode.

        /// </summary>

        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <paramname="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>

        /// <returns>返回XmlNode</returns>

        public XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)

        {

            xmldoc = new XmlDocument();

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);

                return xmlNode;

            }

            catch (Exception ex)

            {

                return null;

                //throw ex; //这里可以定义你自己的异常处理

            }

        }



        /**/

        /// <summary>

        /// 选择匹配XPath表达式的节点列表XmlNodeList.

        /// </summary>

        /// <paramname="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <paramname="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>

        /// <returns>返回XmlNodeList</returns>

        public XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)

        {

            xmldoc = new XmlDocument();

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNodeList xmlNodeList = xmldoc.SelectNodes(xpath);

                return xmlNodeList;

            }

            catch (Exception ex)

            {

                return null;

                //throw ex; //这里可以定义你自己的异常处理

            }

        }



        /**/

        /// <summary>

        /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute. /// </summary>

        /// <paramname="xmlFileName">XML文档完全文件名(包含物理路径)</param>

        /// <paramname="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>

        /// <paramname="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>

        /// <returns>返回xmlAttributeName</returns>

        public XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)

        {

            string content = string.Empty;

            xmldoc = new XmlDocument();

            XmlAttribute xmlAttribute = null;

            try

            {

                xmldoc.Load(xmlFileName); //加载XML文档

                XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);

                if (xmlNode != null)

                {

                    if (xmlNode.Attributes.Count > 0)

                    {

                        xmlAttribute = xmlNode.Attributes[xmlAttributeName];

                    }

                }

            }

            catch (Exception ex)

            {

                throw ex; //这里可以定义你自己的异常处理

            }

            return xmlAttribute;

        }

        #endregion

        #endregion
    }
}
