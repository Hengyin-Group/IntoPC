namespace IntoApp.Dal
{
    public class Equipment
    {

        //此方法仅限测试使用
        public string GetDevice(string token)
        {
            string url = RequestAddress.server + RequestAddress.GetDevice;
            string tempVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return tempVal;
        }

        public string GetEquipIP(string token, string id, string equipType)
        {
            //string url = RequestAddress.server + RequestAddress.GetEquipIP+"?token="+token+"&id="+id+ "&equipType="+equipType;
            string url = RequestAddress.HostServer + RequestAddress.GetEquipIP + "?token=" + token + "&id=" + id + "&equipType=" + equipType;
            string temVal = RequestAddress.Rep_Header_Resp(token, url, "POST");
            return temVal;
        }
    }
}
