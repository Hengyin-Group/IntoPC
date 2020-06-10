using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InToApp.VM
{
    public class EquipmentInfoVM
    {
        public string Id { get; set; }
        //public string BlackServerName { get; set; }
        //public string ColorServerName { get; set; }
        //public string BlackPrinterName { get; set; }
        //public string ColorPrinterName { get; set; }
        //public string BlackServerIp { get; set; }
        //public string ColorServerIp { get; set; }
        //public Nullable<System.DateTime> CreatTime { get; set; }
        public string AddressTitle { get; set; }
        public string DetailedAddress { get; set; }
        public string Coordinate { get; set; }
        //public Nullable<bool> IsUsing { get; set; }
        //public Nullable<bool> IsaFour { get; set; }
        //public Nullable<bool> IsaThree { get; set; }
        //public Nullable<bool> IsColours { get; set; }
        //public string AreaCode { get; set; }
        public Nullable<int> TojoId { get; set; }
        public Nullable<int> BlackRemainPaper { get; set; }
        public Nullable<int> ColorRemainPaper { get; set; }
        //public Nullable<int> BlackTotalUsePaper { get; set; }
        //public Nullable<int> ColorTotalUsePaper { get; set; }
        //public Nullable<bool> BlackIsOnline { get; set; }
        //public Nullable<bool> ColorIsOnline { get; set; }
        //public string BlackError { get; set; }
        //public string ColorError { get; set; }
        //public string BlackIncome { get; set; }
        //public string ColorIncome { get; set; }
        //public Nullable<int> BlackTaskCnt { get; set; }
        //public Nullable<int> ColorTaskCnt { get; set; }

        /*新增*/
        public string GrayInfo { get; set; }
        public string ColorInfo { get; set; }
    }
}