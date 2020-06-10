using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoApp.ViewModel.Base;

namespace IntoApp.Model
{
    public class AccountInfo
    {
        public static string NickName { get; set; }
        public static string IconImage { get; set; }
        public static string Marital { get; set; }   //金额
        public static string Token { get; set; }
        public static string Phone { get; set; }
        public static string ID { get; set; }
        public static string Expertise { get; set; }  //签名
        public static string JobState { get; set; }
        public static string DepID { get; set; }
        public static string PosID { get; set; }
    }

    public class LocalUserInfo:ViewModelBase
    {
        private int _index;
        private string _phone;
        private string _pwd;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value; 
                RaisePropertyChanged("Index");
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public string Pwd
        {
            get { return _pwd; }
            set
            {
                _pwd = value;
                RaisePropertyChanged("Pwd");
            }
        }
    }

}
