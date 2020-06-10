﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseCheckBox : CheckBox
    {
        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(UseCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255,255,255))));


        /// <summary>
        /// 是否显示内容
        /// </summary>
        public bool IsShowContent
        {
            get { return (bool)GetValue(IsShowContentProperty); }
            set { SetValue(IsShowContentProperty, value); }
        }
        public static readonly DependencyProperty IsShowContentProperty =
            DependencyProperty.Register("IsShowContent", typeof(bool), typeof(UseCheckBox), new PropertyMetadata(true));


    }
}
