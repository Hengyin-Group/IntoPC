using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseSystemCloseButton : UseSystemButton
    {
        Window targetWindow;
        public UseSystemCloseButton()
        {
            SystemButtonHoverColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

            Click += delegate{
                if (targetWindow==null)
                {
                    targetWindow = Window.GetWindow(this);
                }
                targetWindow.Close();
            };
        }
    }
}
