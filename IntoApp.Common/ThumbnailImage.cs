using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace IntoApp.Common
{
    
    public class ThumbnailImage
    {
        /// <summary> 
        /// 按照指定大小缩放图片，但是为了保证图片宽高比自动截取 
        /// </summary> 
        /// <param name="srcImage"></param> 
        /// <param name="iWidth"></param> 
        /// <param name="iHeight"></param> 
        /// <returns></returns> 
        public static Bitmap SizeImageWithOldPercent(Image srcImage, int iWidth, int iHeight)
        {

            try
            {
                int newW = srcImage.Width;
                int newH = srcImage.Height;
                int newX = 0;
                int newY = 0;
                double whPercent = 1;
                whPercent= ((double)iWidth / (double)iHeight) * ((double)srcImage.Height / (double)srcImage.Width);
                if (whPercent>1)
                {
                    newW=int.Parse(Math.Round(srcImage.Width/whPercent).ToString());
                }
                else if (whPercent<1)
                {
                    newH = int.Parse(Math.Round(srcImage.Height * whPercent).ToString());
                }
                if (newW!=srcImage.Width)
                {
                    newX = Math.Abs(int.Parse(Math.Round(((double) srcImage.Width - newW) / 2).ToString()));
                }
                else if (newH==srcImage.Height)
                {
                    newY = Math.Abs(int.Parse(Math.Round(((double) srcImage.Height - (double) newH) / 2).ToString()));
                }
                Bitmap cutedImage = CutImage(srcImage, newX, newY, newW, newH);
                Bitmap b=new Bitmap(iWidth,iHeight);
                Graphics g=Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(cutedImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
                return null;
            }
        }

        /// <summary> 
        /// 剪裁 -- 用GDI+ 
        /// </summary> 
        /// <param name="b">原始Bitmap</param> 
        /// <param name="StartX">开始坐标X</param> 
        /// <param name="StartY">开始坐标Y</param> 
        /// <param name="iWidth">宽度</param> 
        /// <param name="iHeight">高度</param> 
        /// <returns>剪裁后的Bitmap</returns> 
        public static Bitmap CutImage(Image b,int StartX,int StartY,int iWidth,int iHeight)
        {
            if (b==null)
                return null;

            int w = b.Width;
            int h = b.Height;
            if (StartX>=w||StartY>=h)
                //开始坐标超出范围
                return null;

            if (StartX+iWidth>w)
                iWidth = w - StartX;

            if (StartY+iHeight>h)
                iHeight = h - StartY;

            try
            {
                //剪裁--用GDI+
                Bitmap bmpOut=new Bitmap(iWidth,iHeight);
                Graphics g=Graphics.FromImage(bmpOut);
                g.DrawImage(b,new Rectangle(0,0,iWidth,iHeight),new Rectangle(StartX,StartY,iWidth,iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
                return null;
            }

        }

        /// <summary>
        /// 等比例压缩
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bitmap CutImage(Image b,int MaxLength)
        {
            if (b == null)
                return null;
            int w = b.Width;
            int h = b.Height;
            int Long = w > h ? w : h;
            int percent = Long / MaxLength;  //百分比
            int iWidth = w * percent;
            int iHeight = h * percent;
            Bitmap bmpOut = new Bitmap(iWidth, iHeight);
            Graphics g = Graphics.FromImage(bmpOut);
            g.DrawImage(b,new Rectangle(0,0,iWidth,iHeight));
            g.Dispose();
            return bmpOut;
        }

        /// <summary>
        /// 截取正方形,长度以最短为主,以中间为基准
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iWidth">目标宽度</param>
        /// <param name="iHeight">目标长度</param>
        /// <returns></returns>
        public static Bitmap CutImage(Image srcImage,int iWidth,int iHeight)
        {
            int w = srcImage.Width;
            int h = srcImage.Height;
            int MinValue = w > h ? h : w;
            int StartX = (w/2)-(MinValue/2);
            int StartY = (h/2)-(MinValue/2);
            try
            {
                Bitmap cutedImage=new Bitmap(MinValue,MinValue);
                Graphics graphics=Graphics.FromImage(cutedImage);
                graphics.DrawImage(srcImage,new Rectangle(0,0,MinValue,MinValue),new Rectangle(StartX,StartY,MinValue,MinValue), GraphicsUnit.Pixel);
                graphics.Dispose();

                Bitmap bmpOut = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(cutedImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch (Exception e)
            {
                return null;
            }
            //Bitmap cutedImage = CutImage(srcImage, StartX, StartY, StartX+MinValue, StartY+MinValue);
            //Bitmap bmpOut = new Bitmap(iWidth, iHeight);
          
        }

    }
}
