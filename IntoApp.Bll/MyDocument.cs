using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using IntoApp.Bll;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntoApp.Bll
{
    public class MyDocument
    {
        Dal.MyDocument myDocument=new Dal.MyDocument();

        //public List<MyDocumentVM> GetMyWord(string token, string pagesize, string page, string query)
        //{
        //    string tempVal= myDocument.GetMyWord(token, pagesize, page, query);
        //    JObject jo = (JObject)JsonConvert.DeserializeObject(tempVal);
        //    List<MyDocumentVM> VMs= new List<MyDocumentVM>();
        //    foreach (var tem in jo["dataList"])
        //    {
        //        MyDocumentVM i = new MyDocumentVM();
        //        i.ID = tem["ID"].ToString();
        //        i.CreateTime = tem["CreateTime"].ToString();
        //        i.Creater = tem["Creater"].ToString();
        //        i.FileType = tem["FileType"].ToString();
        //        i.PreviewDetail = tem["PreviewDetail"].ToString();
        //        i.FileName = tem["FileName"].ToString();
        //        i.PdfUrl = tem["PdfUrl"].ToString();
        //        i.CreaterID = tem["CreaterID"].ToString();
        //        i.FileMd5 = tem["FileMd5"].ToString();
        //        i.SourceFileUrl = tem["SourceFileUrl"].ToString();
        //        i.TrueFileUrl = tem["TrueFileUrl"].ToString();
        //        i.DelTime =tem["DelTime"].ToString();
        //        i.FilsIsUsing = (bool?)tem["FilsIsUsing"];
        //        VMs.Add(i);
        //    }
        //    return VMs;
        //}

        public string GetMyWord(string token, string pagesize, string page, string query)
        {
            return myDocument.GetMyWord(token, pagesize, page, query);
        }

        public string GetPersonalDocTimeList(string token)
        {
            return myDocument.GetPersonalDocTimeList(token);
        }

        public string DelMyDocs(string token, string wordids)
        {
            return myDocument.DelMyDocs(token, wordids);
        }

        public string ReCreateOrder(string token, string fid, string filename)
        {
            return myDocument.ReCreateOrder(token, fid, filename);
        }

        public string DocTransCoding(string token, string[] files, NameValueCollection data)
        {
            return myDocument.DocTransCoding(token,files, data);
        }

        public string ImgTransCoding(string token, string[] files, NameValueCollection data)
        {
            return myDocument.ImgTransCoding(token,files,data);
        }

        public string GetfileTotal(string token,string pagesize,string query)
        {
            return myDocument.GetFileTotal(token, pagesize, query);
        }

        public string BusinessVirtualPrint(string token, string[] files, NameValueCollection data)
        {
            return myDocument.BusinessVirtualPrint(token, files, data);
        }

        public string BusinessVirtualPrint(string token, string UserId, string FileId, string IsColor,
            string PrinterType, string copies, string startpage, string endpage)
        {
            return myDocument.BusinessVirtualPrint(token, UserId, FileId, IsColor, PrinterType, copies,startpage,endpage);
        }

        public string GetTray(string token, string pagesize, string page)
        {
            return myDocument.GetTray(token, pagesize, page);
        }
    }
}
