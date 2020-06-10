using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.Bll
{
    public class Equipment
    {
        Dal.Equipment equipment=new Dal.Equipment();
        //此方法仅限测试使用
        public string GetDevice(string token)
        {
            return equipment.GetDevice(token);
        }

        public string GetEquipIP(string token, string id, string equipType)
        {
            return equipment.GetEquipIP(token, id, equipType);
        }
    }
}
