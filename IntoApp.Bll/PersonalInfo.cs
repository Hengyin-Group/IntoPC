using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntoApp.Bll;

namespace IntoApp.Bll
{
    public class PersonalInfo
    {
        Dal.PersonalInfo personalInfo = new Dal.PersonalInfo();
        public string GetPersonalInfo(string token)
        {
            return personalInfo.GetPersonalInfo(token);
        }

        public string EditOwnInfo(string token, string signature, string nickname, string gender, string birthday)
        {
            return personalInfo.EditOwnInfo(token, signature, nickname, gender, birthday);
        }
    }
}
