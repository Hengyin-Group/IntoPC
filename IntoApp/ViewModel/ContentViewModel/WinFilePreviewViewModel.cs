using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using IntoApp.Bll;
using IntoApp.Common;
using IntoApp.Model;
using IntoApp.utils;
using MoonPdfLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skin.WPF.Command;
using MessageBox=MyMessageBox.Controls.MessageBox;
using ViewModelBase = IntoApp.ViewModel.Base.ViewModelBase;

namespace IntoApp.ViewModel.ContentViewModel
{
    public class WinFilePreviewViewModel:ViewModelBase
    {
        #region 页面初始化

        //private string _filePath = "D:/1111111.pdf";

        //public string FilePath
        //{
        //    get { return _filePath; }
        //    set
        //    {
        //        _filePath = value;
        //        //RaisePropertyChanged("FilePath");
        //    }
        //}

        
        public WinFilePreviewViewModel()
        {
            RunState = false;
            EmptyIsShow = false;
            LoadedCommand=new MyCommand<object[]>(x=>WinLoaded(x));
            PrinterCommand=new MyCommand<object[]>(x=>Printer(x));
            HideCommand=new MyCommand<object[]>(x=>SetHide(x));
            ShowCommand=new MyCommand<object[]>(x=>SetHide(x));
            ColorSelectCommand = new MyCommand<object[]>(x => ColorSelect(x));
        }

        #endregion

        #region 命令

        public MyCommand<Object[]> LoadedCommand { get; set; }
        public MyCommand<Object[]> PrinterCommand { get; set; }
        public MyCommand<Object[]> HideCommand { get; set; }
        public MyCommand<Object[]> ShowCommand { get; set; }

        public MyCommand<object[]> ColorSelectCommand { get; set; }

        #endregion

        #region 方法


        void ColorSelect(object[] x)
        {
            bool isColor = BooleanHelper.GetBoolean(x[0]) ?? false;
            if (isColor)
            {
                IsDoubleIsEnable = false;
                IsSingleIsChecked = true;
            }
            else
            {
                IsDoubleIsEnable = true;

            }
        }


        public void WinLoaded(Object[] x)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                ///找不到为什么第二次打开IsSingleIsChecked变为False
                IsSingleIsChecked = true;

                MaxValue = FileHelper.FileCount;
                LocalFilePath = FileHelper.LocalFilePath;
                FileUrl = FileHelper.FileUrl;
                OrderState = FileHelper.OrderState;
                OrderId = FileHelper.OrderId;
                OrderNo = FileHelper.OrderNo;
                switch (OrderState)
                {
                    case 0: //未支付订单
                        BtnText = "支付";
                        break;
                    case 10:
                        BtnText = "重新生成";
                        break;
                    case 200:
                        BtnText = "再来一单";
                        break;
                }
                bool bo = HttpFile.Download(FileHelper.LocalFilePath, FileHelper.FileUrl);
               
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    MoonPdfPanel pdfPanel = x[0] as MoonPdfPanel;
                    pdfPanel.MaxZoomFactor = 2.0;
                    pdfPanel.MinZoomFactor = 0.5;
                    //Thread.Sleep(100);
                    if (bo)
                    {
                        pdfPanel.OpenFile(FileHelper.LocalFilePath);
                        _isLoaded = true;
                    }
                    else
                    {
                        MessageBox.Show("文件路径错误或已删除");
                    }
                });
            });
        }

        public void Printer(Object[] x)
        {
            int num1 = JObjectHelper.GetStrNum(x[0].ToString());
            int num2 = JObjectHelper.GetStrNum(x[1].ToString());
            int begion = num1 <= num2 ? num1 : num2;
            int end = num1 > num2 ? num1 : num2;
            int copies = JObjectHelper.GetStrNum(x[2].ToString());
            bool isColor = BooleanHelper.GetBoolean(x[3]) ?? false;
            bool isSinglePrint = BooleanHelper.GetBoolean(x[4]) ?? false;
            JObject jo = null;
            switch (OrderState)
            {
                case 0:  //去支付
                    jo = GotoPay(begion.ToString(), end.ToString(), copies.ToString(), isColor,isSinglePrint);
                    break;
                case 10: //未打印订单,重新生成一单
                    jo= ReCreateOrder(begion.ToString(), end.ToString(), copies.ToString(), isColor?"1":"0",isSinglePrint?"0":"1");
                    break;
                case 200: //打印完成，重新生成
                    jo= ReCreateOrder(begion.ToString(), end.ToString(), copies.ToString(), isColor ? "1" : "0", isSinglePrint?"0":"1");
                    break;
                default:
                    MessageBox.Show("信息错误");
                    return;
            }

            if (JObjectHelper.GetStrNum(jo["code"].ToString())==200)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send<string>(OrderId, "ordercreatesuccess");
                });
                MessageBox.Show("订单生成成功");
            }
            else
            {
                MessageBox.Show(jo["message"].ToString());
            }
            //myDocument.ReCreateOrder(AccountInfo.Token, FileId, Path.GetFileName(LocalFilePath));

        }

        JObject GotoPay(string begionPage,string endPage,string copies,bool isColor,bool isSingle)
        {
            Bll.OrderList orderList = new OrderList();
            Bll.Account account=new Account();
            string priceVal= account.GetPrintPrice(AccountInfo.Token);
            JObject JoPrice = (JObject) JsonConvert.DeserializeObject(priceVal);
            if (JoPrice["code"].GetInt()==200)
            {
                //double BlackPrice = Convert.ToDouble(JoPrice["dataList"]["enterpriseBlackPrice"]);
                //double ColorPrice = Convert.ToDouble(JoPrice["dataList"]["enterpriseColorPrice"]);
                int PageCount = (JObjectHelper.GetStrNum(endPage) - JObjectHelper.GetStrNum(begionPage) + 1) * JObjectHelper.GetStrNum(copies);
                double payMoney = PageCount * (isColor ? Convert.ToDouble(JoPrice["dataList"]["enterpriseColorPrice"]):Convert.ToDouble(JoPrice["dataList"]["enterpriseBlackPrice"]));
                #region 双面订单金额减半
                if (!isSingle)
                {
                    ///重新计算页数
                    double ReSetTotalPage = Math.Ceiling(Convert.ToDouble((Convert.ToDouble(endPage) - Convert.ToDouble(begionPage) + 1) / 2));
                    payMoney = ReSetTotalPage * copies.GetInt() * (isColor ? Convert.ToDouble(JoPrice["dataList"]["enterpriseColorPrice"]) : Convert.ToDouble(JoPrice["dataList"]["enterpriseBlackPrice"]));
                }

                #endregion
                NameValueCollection data = new NameValueCollection
                {
                    {"orderid",OrderId},
                    {"amount",payMoney.ToString()},
                    { "paymode","3"},
                    { "homepage",begionPage},
                    { "endpage",endPage},
                    { "iscolor",isColor?"1":"0"},
                    { "printmode",isSingle?"0":"1"},
                    { "copies",copies}
                };
                string temVal = orderList.OrderPay(AccountInfo.Token, data);
                //string temVal = orderList.OrderPayByBusiness(AccountInfo.Token, begionPage, endPage, copies, isColor, OrderNo);
                return (JObject)JsonConvert.DeserializeObject(temVal);
            }
            else
            {
                var RetVal =JsonConvert.SerializeObject(new
                {
                    code=404,
                    message="订单生成失败"
                });
                return (JObject)JsonConvert.DeserializeObject(RetVal);
            }
        }

        JObject ReCreateOrder(string begionPage, string endPage, string copies, string isColor,string isSingle)
        {
            Bll.OrderList orderList=new OrderList();
            string temVal = orderList.Again(AccountInfo.Token, AccountInfo.ID, OrderId, isColor,"3", copies,
                begionPage, endPage,isSingle);
            return (JObject) JsonConvert.DeserializeObject(temVal);
        }

        public void SetHide(object[] x)
        {
            CanvsShow = !CanvsShow;
        }


        
        #endregion

        #region 单双页显示

        private bool bo =false;

        public MyCommand<Object[]> SingleCommand
        {
            get
            {
                return new MyCommand<object[]>(x => SingleOrDoublePage(x));
            }
        }

        public void SingleOrDoublePage(Object[] obj)
        {
            MoonPdfPanel pdfPanel=obj[0] as MoonPdfPanel;
            CheckBox checkBox=obj[1] as CheckBox;
            bo = checkBox.IsChecked == true;
            if (bo)
            {
                SingleOrDoubleToolTip = "切换至单页显示";
                if (_isLoaded)
                {
                    pdfPanel.ViewType = MoonPdfLib.ViewType.Facing;
                    pdfPanel.Zoom(0.5);
                }
            }
            else
            {
                SingleOrDoubleToolTip = "切换至双页显示";
                if (_isLoaded)
                {
                    pdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage;
                    pdfPanel.Zoom(1.0);
                }
            }
        }

        private string _sigleOrDoubleToolTip = "切换至双页显示";

        public string SingleOrDoubleToolTip
        {
            get { return _sigleOrDoubleToolTip; }
            set
            {
                _sigleOrDoubleToolTip = value;
                RaisePropertyChanged("SingleOrDoubleToolTip");
            }
        }

        #endregion

        public bool _isLoaded = false;

        #region 放大

        public MyCommand<Object[]> MagnifyCommand
        {
            get
            {
                return new MyCommand<object[]>(x => 
                {
                    MoonPdfPanel pdfPanel = x[0] as MoonPdfPanel;
                    if (_isLoaded)
                    {
                        pdfPanel.ZoomIn();
                    }
                });
            }
        }

        #endregion

        #region 缩小

        public MyCommand<Object[]> ShrinkCommand
        {
            get
            {
                return new MyCommand<object[]>(x =>
                {
                    MoonPdfPanel pdfPanel=x[0] as MoonPdfPanel;
                    if (_isLoaded)
                    {
                        pdfPanel.ZoomOut();
                    }
                    
                });
            }
        }

        #endregion

        #region 适中显示

        public MyCommand<Object[]> ModerationCommand
        {
            get
            {
                return new MyCommand<object[]>(x =>
                {
                    MoonPdfPanel pdfPanel=x[0] as MoonPdfPanel;
                    if (_isLoaded)
                    {
                        double d = bo ? 0.5 : 1.0;
                        pdfPanel.Zoom(d);
                    }
                });
            }
        }

        #endregion

        #region 属性


        private bool _isDoubleIsEnable = true;
        public bool IsDoubleIsEnable
        {
            get { return _isDoubleIsEnable; }
            set
            {
                _isDoubleIsEnable = value;
                RaisePropertyChanged("IsDoubleIsEnable");
            }
        }

        public bool _isSingleIsChecked = true;
        public bool IsSingleIsChecked
        {
            get { return _isSingleIsChecked; }
            set
            {
                _isSingleIsChecked = value;
                RaisePropertyChanged("IsSingleIsChecked");

            }
        }


        private bool _canvsShow = true;
        public bool CanvsShow
        {
            get { return _canvsShow; }
            set
            {
                _canvsShow = value;
                RaisePropertyChanged("CanvsShow");
            }
        }

        private string _btnText="打印";
        public string BtnText
        {
            get { return _btnText; }
            set
            {
                _btnText = value;
                RaisePropertyChanged("BtnText");
            }
        }

        private int _maxValue=1;

        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                RaisePropertyChanged("MaxValue");
            }
        }

        private string _fileId ;

        public string FileId
        {
            get { return _fileId; }
            set { _fileId = value; }
        }

        private string _localFilePath;
        public string LocalFilePath
        {
            get { return _localFilePath; }
            set { _localFilePath = value; }
        }

        private string _fileUrl;
        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
        }

        private string _orderNo;
        public string OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        private string _orderId;

        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }
        
        //private bool _isPrint;
        //public bool IsPrint
        //{
        //    get { return _isPrint; }
        //    set { _isPrint = value; }
        //}

        private int _orderState;
        public int OrderState
        {
            get { return _orderState; }
            set { _orderState = value; }
        }
       

        #endregion

    }
}
