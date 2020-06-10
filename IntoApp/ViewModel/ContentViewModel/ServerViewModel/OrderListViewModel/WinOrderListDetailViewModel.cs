using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Common.Helper;
using IntoApp.Dal;
using IntoApp.Model;
using IntoApp.View.Content.Other;
using IntoApp.utils;
using IntoApp.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using MessageBox = MyMessageBox.Controls.MessageBox;
using OrderListHelper = IntoApp.Model.OrderListHelper;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel.OrderListViewModel
{
    public class WinOrderListDetailViewModel : ViewModelBase
    {
        #region 页面初始化

        public WinOrderListDetailViewModel()
        {
            GetOrderDetail(AccountInfo.Token, OrderListHelper.SelectOrderId);
        }

        #endregion


        #region  获取订单实体

        public void GetOrderDetail(string token, string orderid)
        {
            Bll.OrderList bllOrderList = new Bll.OrderList();
            string Str = bllOrderList.GetOrderDetails(token, orderid);
            JObject jo = (JObject)JsonConvert.DeserializeObject(Str);
            if (JObjectHelper.GetStrNum(jo["code"].ToString()) == 200)
            {
                var order = jo["dataList"];
                #region 不知道为毛这样就行

                _orderDetail = new OrderDetail()
                {
                    Index = 1,
                    ID = order["id"].ToString(),
                    PdfUrl = RequestAddress.server + order["pdfUrl"],
                    CreateTime = !string.IsNullOrEmpty(order["createTime"].ToString()) && order["createTime"].ToString().ToUpper() != "NULL" ? DateTimeHelper.GetDateTime(order["createTime"].ToString()) : "暂无",
                    FileType = order["fileType"].ToString(),
                    OrderId = order["orderId"].ToString(),
                    FileId = order["fileId"].ToString(),
                    OrderNo = order["orderNo"].ToString(),
                    OrderState = JObjectHelper.GetStrNum(order["orderState"].ToString()),
                    //QrCodeUrl = RequestAddress.server + order["qrCodeUrl"],
                    QrCodeUrl = RequestAddress.HostServer + order["qrCodeUrl"],
                    FilePageSection = order["filePageSection"].ToString(),
                    TotalPage = JObjectHelper.GetStrNum(order["totalPage"].ToString()),
                    TotalMoney = order["totalMoney"].ToString(),
                    PayMode = order["payMode"].ToString(),
                    PayTime = DateTimeHelper.StringToDateTime(order["payTime"].ToString()),
                    DoneTime = DateTimeHelper.StringToDateTime(order["doneTime"].ToString()),
                    PrintCode = order["printCode"].ToString(),
                    Copies = JObjectHelper.GetStrNum(order["copies"].ToString()),
                    FileName = order["fileName"].ToString(),
                    //PageCount = order["previewImgs"].Count(),
                    PageCount = order["filePageSection"].ToString().Split('-')[1].GetInt(),
                    IsColor = (bool)BooleanHelper.GetBoolean(order["isColor"]),
                    IsSingle = (bool)BooleanHelper.GetBoolean(order["isSingle"])
                };

                switch (_orderDetail.OrderState)
                {
                    case 200:
                        Order200 = true;
                        Order0 = !Order200;
                        Order10 = !Order200;
                        break;
                    case 10:
                        Order10 = true;
                        Order200 = !Order10;
                        Order0 = !Order10;
                        break;
                    case 0:
                        Order0 = true;
                        Order200 = !Order0;
                        Order10 = !Order0;
                        break;
                    default:
                        break;
                }

                #endregion 
            }
            else
            {
                MessageBox.Show(jo["message"].ToString());
            }
        }

        #endregion

        #region 命令

        private FilePreView FP;

        public MyCommand CheckFileCommand
        {
            get
            {
                return new MyCommand(x =>
                {
                    //new WinFilePreviewViewModel().FilePath= "http://into.wainwar.com" + PdfUrl;
                    FileHelper.FileCount = PageCount;
                    FileHelper.FileUrl = PdfUrl;
                    FileHelper.OrderId = ID;
                    FileHelper.OrderNo = OrderNo;
                    FileHelper.OrderState = OrderState;
                    FileHelper.LocalFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filetemporary");
                    FileHelper.LocalFilePath = Path.Combine(FileHelper.LocalFileDirectory, FileName);
                    (new FilePreView()).Show();
                });
            }
        }

        #endregion


        private OrderDetail _orderDetail;

        #region 实体属性

        public int Index
        {
            get { return _orderDetail.Index; }
            set
            {
                _orderDetail.Index = value;
                RaisePropertyChanged("Index");
            }
        }
        public string ID
        {
            get { return _orderDetail.ID; }
            set
            {
                _orderDetail.ID = value;
                RaisePropertyChanged("ID");
            }
        }

        public string PdfUrl
        {
            get { return _orderDetail.PdfUrl; }
            set
            {
                _orderDetail.PdfUrl = value;
                RaisePropertyChanged("PdfUrl");
            }
        }
        public string CreateTime
        {
            get { return _orderDetail.CreateTime; }
            set
            {
                _orderDetail.CreateTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }
        public string FileType
        {
            get { return _orderDetail.FileType; }
            set
            {
                _orderDetail.FileType = value;
                RaisePropertyChanged("FileType");
            }
        }

        public string OrderId
        {
            get { return _orderDetail.OrderId; }
            set
            {
                _orderDetail.OrderId = value;
                RaisePropertyChanged("OrderId");
            }
        }

        public string FileId
        {
            get { return _orderDetail.FileId; }
            set
            {
                _orderDetail.FileId = value;
                RaisePropertyChanged("FileId");
            }
        }

        public string OrderNo
        {
            get { return _orderDetail.OrderNo; }
            set
            {
                _orderDetail.OrderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }

        public int OrderState
        {
            get
            {
                return _orderDetail.OrderState;
            }
            set
            {
                _orderDetail.OrderState = value;
                RaisePropertyChanged("OrderState");
            }
        }

        public string QrCodeUrl
        {
            get { return _orderDetail.QrCodeUrl; }
            set
            {
                _orderDetail.QrCodeUrl = value;
                RaisePropertyChanged("QrCodeUrl");
            }
        }

        public string FilePageSection
        {
            get { return _orderDetail.FilePageSection; }
            set
            {
                _orderDetail.FilePageSection = value;
                RaisePropertyChanged("FilePageSection");
            }
        }

        public int TotalPage
        {
            get { return _orderDetail.TotalPage; }
            set
            {
                _orderDetail.TotalPage = value;
                RaisePropertyChanged("TotalPage");
            }
        }

        public string TotalMoney
        {
            get { return _orderDetail.TotalMoney; }
            set
            {
                _orderDetail.TotalMoney = value;
                RaisePropertyChanged("TotalMoney");
            }
        }

        public string PayMode
        {
            get { return _orderDetail.PayMode; }
            set
            {
                _orderDetail.PayMode = value;
                RaisePropertyChanged("PayMode");
            }
        }

        public DateTime? PayTime
        {
            get { return _orderDetail.PayTime; }
            set
            {
                _orderDetail.PayTime = value;
                RaisePropertyChanged("PayTime");
            }
        }

        public DateTime? DoneTime
        {
            get { return _orderDetail.DoneTime; }
            set
            {
                _orderDetail.DoneTime = value;
                RaisePropertyChanged("DoneTime");
            }
        }

        public string PrintCode
        {
            get { return _orderDetail.PrintCode; }
            set
            {
                _orderDetail.PrintCode = value;
                RaisePropertyChanged("PrintCode");
            }
        }

        public int Copies
        {
            get { return _orderDetail.Copies; }
            set
            {
                _orderDetail.Copies = value;
                RaisePropertyChanged("Copies");
            }
        }

        public string FileName
        {
            get { return _orderDetail.FileName; }
            set
            {
                _orderDetail.FileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public bool IsColor
        {
            get { return _orderDetail.IsColor; }
            set
            {
                _orderDetail.IsColor = value;
                RaisePropertyChanged("IsColor");
            }
        }

        public bool IsSingle
        {
            get { return _orderDetail.IsSingle; }
            set
            {
                _orderDetail.IsSingle = value;
                RaisePropertyChanged("IsSingle");
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
                if (PayTime == null)
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
                if (DoneTime == null)
                {
                    return "暂无";
                }
                return DoneTime.ToString();
            }
        }

        public int PageCount
        {
            get { return _orderDetail.PageCount; }
            set
            {
                _orderDetail.PageCount = value;
                RaisePropertyChanged("PageCount");
            }
        }

        private bool _order200 = false;

        public bool Order200
        {
            get { return _order200; }
            set
            {
                _order200 = value;
                RaisePropertyChanged("Order200");
            }
        }

        private bool _order10 = false;
        public bool Order10
        {
            get { return _order10; }
            set
            {
                _order10 = value;
                RaisePropertyChanged("Order10");
            }
        }

        private bool _order0 = false;
        public bool Order0
        {
            get { return _order0; }
            set
            {
                _order0 = value;
                RaisePropertyChanged("Order0");
            }
        }


        #endregion
    }
}
