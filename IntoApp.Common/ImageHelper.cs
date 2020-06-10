using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace IntoApp.Common
{
    public class ImageHelper
    {
        /// <summary>
        /// 图片保存到本地
        /// </summary>
        /// <param name="urlImagePath">网络图片路径</param>
        /// <param name="localImagePath">本地图片路径</param>
        /// <returns>filename_30x30,filename_60x60</returns>
        public static bool DownloadImage(string urlImagePath,string localImagePath,ImageFormat format)
        {
            bool isExist= DownloadHelper.HttpFileExist(urlImagePath);
            if (isExist)
            {
                //DownloadHelper.DownloadHttpFile(urlImagePath,localImagePath);
                //本地保存源文件路径
                string localSourcePath = Path.Combine(Path.GetDirectoryName(localImagePath), Path.GetFileName(urlImagePath));
                bool isSuc= HttpFile.Download(localSourcePath, urlImagePath);
                if (isSuc)
                {
                    //图片压缩
                    string fileDir = Path.GetDirectoryName(localImagePath);
                    string fileName = Path.GetFileNameWithoutExtension(localImagePath);
                    string fileType = Path.GetExtension(localImagePath).Replace(".", "");
                    Image image=new Bitmap(localSourcePath);

                    Bitmap icon_30x30 = ThumbnailImage.SizeImageWithOldPercent(image, 30, 30);
                    //icon_30x30.Save(Path.Combine(fileDir, fileName, "_30x30.bmp"), format);
                    Bitmap bmp30 = new Bitmap(icon_30x30);
                    icon_30x30.Dispose();
                    bmp30.Save(localImagePath, format);

                    //Bitmap icon_60x60 = ThumbnailImage.SizeImageWithOldPercent(image, 60, 60);
                    ////icon_60x60.Save(Path.Combine(fileDir, fileName, "_60x60.bmp"), format);
                    //Bitmap bmp60=new Bitmap(icon_60x60);
                    //icon_60x60.Dispose();
                    //bmp60.Save(localImagePath,format);

                    return true;
                }
            }
            return false;
        }
    }
}

