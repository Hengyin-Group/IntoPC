using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DMSkin.WPF.Controls;

namespace IntoApp.Controls
{
    public class UseLinkButton:UseSystemButton
    {
        public bool DisplayLine
        {
            get { return (bool)GetValue(DisplayLineProperty); }
            set { SetValue(DisplayLineProperty, value); }
        }
        public static readonly DependencyProperty DisplayLineProperty =
            DependencyProperty.Register("DisplayLine", typeof(bool), typeof(UseLinkButton), new PropertyMetadata(true));
    }
}
