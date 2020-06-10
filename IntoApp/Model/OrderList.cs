using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntoApp.Common.Helper;
using GHelper=IntoApp.Common.Helper.GeometryHelper;
using IntoApp.ViewModel.Base;

namespace IntoApp.Model
{
    public class OrderListAll:ViewModelBase
    {
        GHelper g = new GHelper();//GeometryHelper类对象的建立

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }

        private string _id;
        public string ID {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }
        }

        private string _createTime;
        public string CreateTime {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }

        private string _fileId;
        public string FileId
        {
            get { return _fileId;}
            set
            {
                _fileId = value;
                RaisePropertyChanged("FileId");
            }
        }

        private string _orderNo;

        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }

        private int _orderState;

        public int OrderState
        {
            get { return _orderState; }
            set
            {
                _orderState = value;
                RaisePropertyChanged("OrderState");
            }
        }
        /// <summary>
        /// 订单状态的文字叙述
        /// </summary>
        public string OrderStateText
        {
            get
            {
                if (OrderState==200)
                {
                    return "已完成";
                }

                if (OrderState==10)
                {
                    return "未打印";
                }

                if (OrderState==0)
                {
                    return "未支付";
                }
                else
                {
                    return "未知";
                }
            }
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        private string _payMode;

        public string PayMode
        {
            get { return _payMode; }
            set
            {
                _payMode = value;
                RaisePropertyChanged("PayMode");
            }
        }

        private string _orderCateoryId;

        public string OrderCateoryId
        {
            get { return _orderCateoryId; }
            set
            {
                _orderCateoryId = value;
                RaisePropertyChanged("OrderCateoryId");
            }
        }

        private string _fileType;

        public string FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                RaisePropertyChanged("FileType");
            }
        }
        /// <summary>
        /// 文件名称带后缀名
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 没有后缀的文件名
        /// </summary>
        public string FileNameNoSuffix
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    string filename = FileName.Split('.')[0];
                    return filename;
                }
                else
                {
                    return "[文件名为空]";
                }
            }
        } 

        public string FileNameCut
        {
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 20); }
        }

        /// <summary>
        /// 文件前的图片
        /// </summary>
        //private Geometry _image;
        public Geometry Image
        {
            //get { return _image; }
            //set
            //{
            //    _image = value;
            //    RaisePropertyChanged("Image");
            //}
            get
            {
                
                if (FileType.ToUpper().Contains("IMG"))
                {
                    return g.Icon_IMG;
                }
                if (FileType.ToUpper().Contains("PPT"))
                {
                    return g.Icon_PPT;
                }
                if (FileType.ToUpper().Contains("DOC"))
                {
                    return g.Icon_DOC;
                }
                if (FileType.ToUpper().Contains("XLS"))
                {
                    return g.Icon_XLS;
                }

                if (FileType.ToUpper().Contains("TXT"))
                {
                    return g.Icon_TXT;
                }

                if (FileType.ToUpper().Contains("PDF"))
                {
                    return g.Icon_PDF;
                }
                else
                {
                    return g.Icon_WARNING;
                }
            }
        }

    }

    public class OrderListComplete : ViewModelBase
    {
        GHelper g = new GHelper();//GeometryHelper类对象的建立

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }
        }

        private string _createTime;
        public string CreateTime
        {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }

        private string _fileId;
        public string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
                RaisePropertyChanged("FileId");
            }
        }

        private string _orderNo;

        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }

        private int _orderState;

        public int OrderState
        {
            get { return _orderState; }
            set
            {
                _orderState = value;
                RaisePropertyChanged("OrderState");
            }
        }
        /// <summary>
        /// 订单状态的文字叙述
        /// </summary>
        public string OrderStateText
        {
            get
            {
                if (OrderState == 200)
                {
                    return "已完成";
                }

                if (OrderState == 10)
                {
                    return "未打印";
                }

                if (OrderState == 0)
                {
                    return "未支付";
                }
                else
                {
                    return "未知";
                }
            }
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        private string _payMode;

        public string PayMode
        {
            get { return _payMode; }
            set
            {
                _payMode = value;
                RaisePropertyChanged("PayMode");
            }
        }

        private string _orderCateoryId;

        public string OrderCateoryId
        {
            get { return _orderCateoryId; }
            set
            {
                _orderCateoryId = value;
                RaisePropertyChanged("OrderCateoryId");
            }
        }

        private string _fileType;

        public string FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                RaisePropertyChanged("FileType");
            }
        }
        /// <summary>
        /// 文件名称带后缀名
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 没有后缀的文件名
        /// </summary>
        public string FileNameNoSuffix
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    string filename = FileName.Split('.')[0];
                    return filename;
                }
                else
                {
                    return "[文件名为空]";
                }
            }
        }

        public string FileNameCut
        {
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 20); }
        }

        /// <summary>
        /// 文件前的图片
        /// </summary>
        //private Geometry _image;
        public Geometry Image
        {
            //get { return _image; }
            //set
            //{
            //    _image = value;
            //    RaisePropertyChanged("Image");
            //}
            get
            {

                if (FileType.ToUpper().Contains("IMG"))
                {
                    return g.Icon_IMG;
                }
                if (FileType.ToUpper().Contains("PPT"))
                {
                    return g.Icon_PPT;
                }
                if (FileType.ToUpper().Contains("DOC"))
                {
                    return g.Icon_DOC;
                }
                if (FileType.ToUpper().Contains("XLS"))
                {
                    return g.Icon_XLS;
                }

                if (FileType.ToUpper().Contains("TXT"))
                {
                    return g.Icon_TXT;
                }

                if (FileType.ToUpper().Contains("PDF"))
                {
                    return g.Icon_PDF;
                }
                else
                {
                    return g.Icon_WARNING;
                }
            }
        }

    }

    public class OrderListUnfinished : ViewModelBase
    {
        GHelper g = new GHelper();//GeometryHelper类对象的建立

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }
        }

        private string _createTime;
        public string CreateTime
        {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }

        private string _fileId;
        public string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
                RaisePropertyChanged("FileId");
            }
        }

        private string _orderNo;

        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }

        private int _orderState;

        public int OrderState
        {
            get { return _orderState; }
            set
            {
                _orderState = value;
                RaisePropertyChanged("OrderState");
            }
        }
        /// <summary>
        /// 订单状态的文字叙述
        /// </summary>
        public string OrderStateText
        {
            get
            {
                if (OrderState == 200)
                {
                    return "已完成";
                }

                if (OrderState == 10)
                {
                    return "未打印";
                }

                if (OrderState == 0)
                {
                    return "未支付";
                }
                else
                {
                    return "未知";
                }
            }
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        private string _payMode;

        public string PayMode
        {
            get { return _payMode; }
            set
            {
                _payMode = value;
                RaisePropertyChanged("PayMode");
            }
        }

        private string _orderCateoryId;

        public string OrderCateoryId
        {
            get { return _orderCateoryId; }
            set
            {
                _orderCateoryId = value;
                RaisePropertyChanged("OrderCateoryId");
            }
        }

        private string _fileType;

        public string FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                RaisePropertyChanged("FileType");
            }
        }
        /// <summary>
        /// 文件名称带后缀名
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 没有后缀的文件名
        /// </summary>
        public string FileNameNoSuffix
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    string filename = FileName.Split('.')[0];
                    return filename;
                }
                else
                {
                    return "[文件名为空]";
                }
            }
        }

        public string FileNameCut
        {
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 20); }
        }

        /// <summary>
        /// 文件前的图片
        /// </summary>
        //private Geometry _image;
        public Geometry Image
        {
            //get { return _image; }
            //set
            //{
            //    _image = value;
            //    RaisePropertyChanged("Image");
            //}
            get
            {

                if (FileType.ToUpper().Contains("IMG"))
                {
                    return g.Icon_IMG;
                }
                if (FileType.ToUpper().Contains("PPT"))
                {
                    return g.Icon_PPT;
                }
                if (FileType.ToUpper().Contains("DOC"))
                {
                    return g.Icon_DOC;
                }
                if (FileType.ToUpper().Contains("XLS"))
                {
                    return g.Icon_XLS;
                }

                if (FileType.ToUpper().Contains("TXT"))
                {
                    return g.Icon_TXT;
                }

                if (FileType.ToUpper().Contains("PDF"))
                {
                    return g.Icon_PDF;
                }
                else
                {
                    return g.Icon_WARNING;
                }
            }
        }

    }

}
