using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IntoApp.Controls.Extensions
{
    /// <summary>
    /// 通用是否可见(附加属性)
    /// </summary>
    public class IsShowExtensions
    {
        public static bool GetIsShowValue(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsShowProperty);
        }

        public static void SetIsShowValue(DependencyObject obj,bool value)
        {
            obj.SetValue(IsShowProperty,value);
        }

        //public static readonly DependencyProperty IsShowProperty =
        //    DependencyProperty.RegisterAttached("IsShow", typeof(bool), typeof(IsShowExtensions),new FrameworkPropertyMetadata(false,IsShowProperChangedCallback));


        public static readonly DependencyProperty IsShowProperty = DependencyProperty.RegisterAttached(
            "IsShow", typeof(bool), typeof(DataGridExtensions),
            new PropertyMetadata(false, IsShowPropertyChangedCallback));
        static void IsShowPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement m = sender as UIElement;
            if (m==null) return;
            if (e.OldValue != e.NewValue)
            {
                m.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
            }
        }


    }
}
