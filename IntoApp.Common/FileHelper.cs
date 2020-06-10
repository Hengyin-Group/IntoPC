using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntoApp.Common
{
    public class FilesHelper
    {
        /// <summary>
        /// 文件夹下的文件名是否包含某个字符
        /// </summary>
        /// <returns></returns>
        public static List<FileInfo> GetFileHasStr(string Dir,List<string> list)
        {
            List<FileInfo> retFileInfo = null;
            if (Directory.Exists(Dir))
            {
                DirectoryInfo myDir = new DirectoryInfo(Dir);
                FileInfo[] _fileInfos = myDir.GetFiles();
                for (int i = 0; i < _fileInfos.Length; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (_fileInfos[i].Name.Contains(list[j]))
                        {
                            retFileInfo.Add(_fileInfos[i]);
                        }
                        j++;
                    }
                    i++;
                }
            }
            return retFileInfo;
        }
    }
}
