using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IntoApp.AutoUpdate.ViewModel.Base;

namespace IntoApp.AutoUpdate.Model
{

    public class UpdateModel
    {
        #region 属性

        /// <summary>
        /// 源程序的完整路径
        /// </summary>
        public static string IntoAppPath { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public static string CurrentVersion { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public static string AppId { get; set; }

        /// <summary>
        /// 新版本
        /// </summary>
        public static string NewVersion { get; set; }

        /// <summary>
        /// 更新日志url
        /// </summary>
        public static string UpdateLogUrl { get; set; }

        /// <summary>
        /// 新版本亮点
        /// </summary>
        public static string NewVersionAdvantages { get; set; }

        /// <summary>
        /// 文件更新地址
        /// </summary>
        public static string UpdateFileUrl { get; set; }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        public static string LocalFilePath { get; set; }

        /// <summary>
        /// 解压路径
        /// </summary>
        public static string UnpackPath { get; set; }
        /// <summary>
        /// 更新包MD5
        /// </summary>
        public static string FileMd5 { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public static string FileName { get; set; }

        /// <summary>
        /// 下载接收的大小
        /// </summary>
        public static long ReceivedBytes { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public static long? TotalBytes { get; set; }

        #endregion
    }
}
