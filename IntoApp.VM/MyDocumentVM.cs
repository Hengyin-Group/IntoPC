using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InToApp.VM
{
    public class MyDocumentVM
    {
        public string ID { get; set; }
        public string PdfUrl { get; set; }
        public string CreaterID { get; set; }
        public string Creater { get; set; }
        public string SourceFileUrl { get; set; }
        public string CreateTime { get; set; }
        public Nullable<bool> DelFlag { get; set; }
        public string FileType { get; set; }
        public string PreviewDetail { get; set; }
        public string FileMd5 { get; set; }
        public Nullable<bool> FilsIsUsing { get; set; }
        public string TrueFileUrl { get; set; }
        public string DelTime { get; set; }
        //添加
        public string FileName { get; set; }

    }
}
