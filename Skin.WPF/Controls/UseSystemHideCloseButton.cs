using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseSystemHideCloseButton:UseSystemButton
    {
        Window targetWindow;
        public UseSystemHideCloseButton()
        {
            SystemButtonHoverColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

            Click += delegate
            {
                if (targetWindow == null)
                {
                    targetWindow = Window.GetWindow(this);
                }
                targetWindow.WindowState = WindowState.Minimized;
                targetWindow.ShowInTaskbar = false;
            };
        }
    }
}
