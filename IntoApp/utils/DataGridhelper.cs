using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace IntoApp.utils
{
    public class DataGridHelper
    {

        /// <summary>
        /// dataGrid滚动条是否到底了
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsVerticalScrollBarAtButtom(ScrollChangedEventArgs e,DataGrid d)
        {
            bool isAtButton = false;
            double dVer = e.VerticalOffset;
            double dViewport = e.ViewportHeight;
            Double dExtent = e.ExtentHeight;
            if (dVer != 0)
            {
                if (dVer + dViewport == dExtent)
                {
                    isAtButton = true;
                }
                else
                {
                    isAtButton = false;
                }
            }
            else
            {
                isAtButton = false;
            }

            if (d.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled || d.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
            {
                isAtButton = true;
            }

            return isAtButton;
        }


        /// <summary>
        /// scrollViewer是否滑到底
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsVerticalScrollBarAtButtom(ScrollViewer e)
        {
            bool isAtButton = false;
            double dVer = e.VerticalOffset;
            double dViewport = e.ViewportHeight;
            Double dExtent = e.ExtentHeight;
            if (dVer != 0)
            {
                if (dVer + dViewport == dExtent)
                {
                    isAtButton = true;
                }
                else
                {
                    isAtButton = false;
                }
            }
            else
            {
                isAtButton = false;
            }

            if (e.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled || e.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
            {
                isAtButton = true;
            }

            return isAtButton;
        }
    }
}
