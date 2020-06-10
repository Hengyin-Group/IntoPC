using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Skin.WPF.Controls
{
    public class UseLinkButton : UseSystemButton
    {
        public bool DisplayLine
        {
            get { return (bool)GetValue(DMDisplayLineProperty); }
            set { SetValue(DMDisplayLineProperty, value); }
        }
        public static readonly DependencyProperty DMDisplayLineProperty =
            DependencyProperty.Register("DisplayLine", typeof(bool), typeof(UseLinkButton), new PropertyMetadata(true));
    }
}
