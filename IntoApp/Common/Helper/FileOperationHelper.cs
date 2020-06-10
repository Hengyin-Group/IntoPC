using IntoApp.Common.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace IntoApp.Common.Helper
{
    public class FileOperationHelper
    {
        /// <summary>
        /// 文件复制操作
        /// </summary>
        /// <param name="sourceFileName">源文件路径</param>
        /// <param name="destFileName">新文件路径</param>
        /// <param name="bo">是否执行覆盖</param>
        public static void FileCopy(string sourceFileName, string destFileName, bool bo)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceFileName);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();//获取目录下的所有内容
                foreach (FileSystemInfo info in fileinfo)
                {
                    if (info is DirectoryInfo)
                    {
                        if (!Directory.Exists(destFileName+"\\"+info.Name))
                        {
                            Directory.CreateDirectory(destFileName + "\\" + info.Name);
                        }
                        FileCopy(info.FullName, destFileName + "\\" + info.Name,bo);
                    }
                    else
                    {
                        File.Copy(info.FullName,destFileName+"\\"+info.Name,bo);
                    }
                }

            }
            catch (Exception e)
            {

            }
        }

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 写入Xml
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void WriteXml(string filePath, XElement data)
        {

            try
            {
                LogWriteLock.EnterWriteLock();
                XElement xele = XElement.Load(filePath);
                xele.AddFirst(data);
                xele.Save(filePath);
            }
            catch (Exception e)
            {
            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }

        }

        public static void DeleteXml(string filePath,XElement data)
        {

        }

    }
}
