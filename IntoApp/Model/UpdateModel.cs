using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Model
{
    public class UpdateModel
    {
        /// <summary>
        /// 当前版本
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 新版本
        /// </summary>
        public string NewVersion { get; set; }

        /// <summary>
        /// 更新日志url
        /// </summary>
        public string UpdateLogUrl { get; set; }

        /// <summary>
        /// 新版本亮点
        /// </summary>
        public string NewVersionAdvantages { get; set; }

        /// <summary>
        /// 文件更新地址
        /// </summary>
        public string UpdateFileUrl { get; set; }
        /// <summary>
        /// 解压路径
        /// </summary>
        public string UnpackPath { get; set; }
        /// <summary>
        /// 更新包MD5
        /// </summary>
        public string FileMd5 { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载接收的大小
        /// </summary>
        public long ReceivedBytes { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public long? TotalBytes { get; set; }

    }
}
