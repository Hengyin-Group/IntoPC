using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Model
{
    public class ModelHelper
    {
    }
    public class OrderListHelper
    {
        private static string _selectOrderId;

        public static string SelectOrderId
        {
            get { return _selectOrderId; }
            set { _selectOrderId = value; }
        }
    }

    public class FileHelper
    {

        /// <summary>
        /// 本地文件的路径
        /// </summary>
        private static string _localfilePath;

        public static string LocalFilePath
        {
            get { return _localfilePath; }
            set { _localfilePath = value; }
        }

        /// <summary>
        /// 本地文件所在文件夹
        /// </summary>
        private static string _localfiledirectory;

        public static string LocalFileDirectory
        {
            get { return _localfilePath; }
            set { _localfilePath = value; }
        }

        /// <summary>
        /// 网络文件路径
        /// </summary>
        private static string _fileurl;

        public static string FileUrl
        {
            get { return _fileurl; }
            set { _fileurl = value; }
        }

        private static string _orderId;
        public static string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        private static string _orderNo;
        public static string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        public static string _fileId;

        public static string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
            }
        }

        private static int _fileCount;
        public static int FileCount
        {
            get { return _fileCount; }
            set { _fileCount = value; }
        }

        //public static bool _isPrint = false;  //是否可以打印
        //public static bool IsPrint
        //{
        //    get { return _isPrint; }
        //    set { _isPrint = value; }
        //}

        private static int _orderState;
        public static int OrderState
        {
            get { return _orderState; }
            set { _orderState = value; }
        }
    }
}
