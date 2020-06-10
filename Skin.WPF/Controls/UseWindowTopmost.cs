using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Skin.WPF.Controls
{
    public class UseWindowTopmost:UseImageCheckBox
    {
        Window targetWindow;

        public UseWindowTopmost()
        {
            Click += delegate
            {
                if (targetWindow == null)
                {
                    targetWindow = Window.GetWindow(this);
                }

                bool bo = (bool)IsChecked;
                if (bo)
                {
                    targetWindow.Topmost = true;
                    this.ToolTip = "取消置顶";
                }
                else
                {
                    targetWindow.Topmost = false;
                    this.ToolTip = "置顶";
                }
            };
        }

    }
}
