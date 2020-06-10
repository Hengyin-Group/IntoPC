using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoApp.Common.Helper;
using IntoApp.ViewModel.Base;

namespace IntoApp.Model
{
    public class OrderDetail:ViewModelBase
    {
        private int _index;

        private string _orderId;
        private string _fileId;
        private string _orderNo;
        private int _orderState;
        private string _qrCodeUrl;
        private string _filePageSection;
        private int _totalPage;
        private string _totalMoney;
        private string _payMode;
        private DateTime? _payTime;
        private DateTime? _doneTime;
        private string _printCode;
        private int _copies;
        private string _fileName;
        private bool _isColor;
        private bool _isSingle;
        private int _pageCount;

        public int Index
        {
            get { return _index;}
            set
            {
                _index = value;
                //RaisePropertyChanged("Index");
            }
        }

        private string _id;
        public string ID
        {
            get { return _id;}
            set
            {
                _id = value; 
                //RaisePropertyChanged("ID");
            }
        }

        private string _pdfUrl;

        public string PdfUrl
        {
            get { return _pdfUrl; }
            set
            {
                _pdfUrl = value;
                //RaisePropertyChanged("PdfUrl");
            }
        }

        private string _createTime;
        public string CreateTime
        {
            get { return _createTime;}
            set
            {
                _createTime = value;
                //RaisePropertyChanged("CreateTime");
            }
        }

        private string _fileType;
        public string FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                //RaisePropertyChanged("FileType");
            }
        }

        public string OrderId
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                //RaisePropertyChanged("OrderId");
            }
        }

        public string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
                //RaisePropertyChanged("FileId");
            }
        }

        public string OrderNo
        {
            get { return _orderNo; }
            set
            {
                _orderNo = value;
               //RaisePropertyChanged("OrderNo");
            }
        }

        public int OrderState
        {
            get { return _orderState; }
            set
            {
                _orderState = value;
                //RaisePropertyChanged("OrderState");
            }
        }

        public string QrCodeUrl
        {
            get { return _qrCodeUrl; }
            set
            {
                _qrCodeUrl =value;
                //RaisePropertyChanged("QrCodeUrl");
            }
        }

        public string FilePageSection
        {
            get { return _filePageSection; }
            set
            {
                _filePageSection = value;
                //RaisePropertyChanged("FilePageSection");
            }
        }

        public int TotalPage
        {
            get { return _totalPage; }
            set
            {
                _totalPage = value; 
                //RaisePropertyChanged("TotalPage");
            }
        }

        public string TotalMoney
        {
            get { return _totalMoney; }
            set
            {
                _totalMoney = value;
                //RaisePropertyChanged("TotalMoney");
            }
        }

        public string PayMode
        {
            get { return _payMode; }
            set
            {
                _payMode = value;
                //RaisePropertyChanged("PayMode");
            }
        }

        public DateTime? PayTime
        {
            get { return _payTime; }
            set
            {
                _payTime = value; 
                //RaisePropertyChanged("PayTime");
            }
        }

        public DateTime? DoneTime
        {
            get { return _doneTime; }
            set
            {
                _doneTime = value;
                //RaisePropertyChanged("DoneTime");
            }
        }

        public string PrintCode
        {
            get { return _printCode; }
            set
            {
                _printCode = value;
                //RaisePropertyChanged("PrintCode");
            }
        }

        public int Copies
        {
            get { return _copies; }
            set
            {
                _copies = value;
                //RaisePropertyChanged("Copies");
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                //RaisePropertyChanged("FileName");
            }
        }

        public bool IsColor
        {
            get { return _isColor; }
            set
            {
                _isColor = value;
                //RaisePropertyChanged("IsColor");
            }
        }

        public bool IsSingle
        {
            get { return _isSingle; }
            set
            {
                _isSingle = value;
                //RaisePropertyChanged("IsSingle");
            }
        }
        //订单状态的文字描述
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

        /// <summary>
        /// 文件名截取
        /// </summary>
        public string FileNameCut
        {
            get { return SubStringHelper.StrCut(FileNameNoSuffix, 30); }
        }

        public string PayTimeStr
        {
            get
            {
                if (PayTime==null)
                {
                    return "暂无";
                }
                return PayTime.ToString();
                
            }
        }

        public string DonTimeStr
        {
            get
            {
                if (DoneTime==null)
                {
                    return "暂无";
                }
                return DoneTime.ToString();
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }
    }
}
