using System.IO;
using System.Windows;
using IntoApp.Printer.ViewModel.Base;
using Skin.WPF.Command;

namespace IntoApp.Printer.ViewModel
{
    public class ModifyNameViewModel : ViewModelBase
    {

        //public delegate void TransfDelegate(string value);
        public delegate void TransfDelegate(string filePath, string copies, string isColor, string documentName, string pageCount);
        //public event TransfDelegate TransfEvent;
        public event TransfDelegate TransfEvent_New;

        private string oldPath;
        private string newPath;
        private string copiesNum;
        private string color;

        private string absoultePath { get; set; }

        //public ModifyNameViewModel(string str)
        //{
        //    oldPath = str;
        //    GetFileName(str);
        //}

        public ModifyNameViewModel(string filePath, string copies, string isColor, string documentName, string pageCount)
        {
            oldPath = filePath;
            //copiesNum = copies;
            //_isColor = isColor;

            //GetFileName(filePath);
            GetFileName1(documentName, filePath, isColor, copies, pageCount);
        }

        /// <summary>
        /// 获取打印文件信息(虚拟打印驱动传参)
        /// </summary>
        /// <param name="documentName">文档名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="isColor"></param>
        /// <param name="copies"></param>
        /// <param name="pageCount"></param>
        public void GetFileName1(string documentName, string filePath, string isColor, string copies, string pageCount1)
        {
            string pathStr = "";
            string[] fileName = (filePath.Substring(0, filePath.Length - 4)).Split('\\');
            for (int i = 0; i < fileName.Length - 1; i++)
            {
                pathStr += fileName[i] + "\\";
            }
            absoultePath = pathStr;
            //_filename = oldFileName = fileName[fileName.Length - 1];
            _filename = documentName;
            _pageCount = pageCount1;
            _copies = copies;
            _isColor = isColor;
        }

        public void GetFileName(string str)
        {
            string pathStr = "";
            string[] fileName = (str.Substring(0, str.Length - 4)).Split('\\');
            for (int i = 0; i < fileName.Length - 1; i++)
            {
                pathStr += fileName[i] + "\\";
            }
            absoultePath = pathStr;
            //int index = fileName.Length - 1;
            _filename = oldFileName = fileName[fileName.Length - 1];
        }

        private string oldFileName { get; set; }
        private string newFileName { get; set; }

        #region 显示的内容

        private string _filename;
        private string _isColor;
        private string _copies;
        private string _pageCount;

        public string PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                RaisePropertyChanged("PageCounr");
            }
        }

        public string Copies
        {
            get { return _copies; }
            set
            {
                _copies = value;
                RaisePropertyChanged("Copies");
            }
        }

        public string IsColor
        {
            get { return _isColor; }
            set
            {
                _isColor = value;
                RaisePropertyChanged("IsColor");
            }
        }

        public string FileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
                RaisePropertyChanged("FileName");
            }
        }


        #endregion

        #region 事件

        public MyCommand<object[]> UpLoadCommand
        {
            get
            {
                return new MyCommand<object[]>(x => UpLoad(x));
            }
        }

        
        public void UpLoad(object[] obj)
        {
            //MessageBox.Show(obj[0].ToString());
            //MessageBox.Show(absoultePath + "upload" + obj[0].ToString() + ".pdf");
            if (!string.IsNullOrEmpty(obj[0].ToString()))
            {
                newPath = Path.Combine(absoultePath, "upload", obj[0].ToString() + ".pdf");
                if (!Directory.Exists(Path.GetDirectoryName(newPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                if (File.Exists(newPath))
                    File.Delete(newPath);
                File.Move(oldPath, newPath);
                File.Delete(oldPath);
                //TransfEvent(newPath);
                TransfEvent_New(newPath, "", "", "", "");
                Window window = obj[1] as Window;
                window.DialogResult = true;
                window.Close();
            }
        }

        #endregion

    }
}
