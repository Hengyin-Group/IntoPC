using CL.IO.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntoApp.AutoUpdate.utils
{
    public class ZipHelper
    {

        public delegate void GetValue(double value);  ///解压缩

        public event GetValue GetBarValue;

        public delegate void SetValue(double value);  ///压缩

        public event SetValue SetBarValue;


        /// <summary>
        /// 压缩文件夹并复制到制定目录
        /// </summary>
        /// <param name="strPath">待压缩的文件夹路径</param>
        /// <param name="strZipPath">需要复制到的目录路径（该路径需要带压缩文件名）</param>
        public void ImportZip(string strPath, string strZipPath)
        {
            ZipHandler handler = ZipHandler.GetInstance();
            TaskFactory fastory = new TaskFactory();
            Task[] tasks = new Task[]
            {
                fastory.StartNew(() =>
                {
                    handler.PackDirectory(strPath, strZipPath, (num) =>
                    {
                        //pbYSJD.Value = Convert.ToInt32(num);
                        SetBarValue(num);
                    });
                })
            };
            fastory.ContinueWhenAll(tasks, ImportTasksEnded);

        }
        private void ImportTasksEnded(Task[] tasks)
        {
            //MessageBox.Show("压缩完成", "提示");
            //btnImport.Enabled = true;
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="strPath">压缩文件路径</param>
        /// <param name="strZipPath">解压后文件路径</param>
        public void ExportZip(string strPath,string strZipPath)
        {
            ZipHandler handler=ZipHandler.GetInstance();
            TaskFactory fasFactory=new TaskFactory();
            Task[] tasks = new Task[]
            {
                fasFactory.StartNew(() =>
                {
                    handler.UnpackAll(strPath,strZipPath, (num) => { GetBarValue(num); });
                })
            };
            fasFactory.ContinueWhenAll(tasks, ExportTasksEnded);
        }
        private void ExportTasksEnded(Task[] tasks)
        {
            //MessageBox.Show("压缩完成", "提示");
            //btnImport.Enabled = true;
        }

    }
}
