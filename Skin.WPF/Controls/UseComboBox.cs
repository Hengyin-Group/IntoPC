using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skin.WPF.Controls
{
    public class UseComboBox:ComboBox
    {

        public int Maxlength
        {
            get { return (int)GetValue(MaxlengthProperty); }
            set { SetValue(MaxlengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxlength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxlengthProperty =
            DependencyProperty.Register("Maxlength", typeof(int), typeof(UseComboBox), new PropertyMetadata(100));



        public Geometry ToggleButtonImage
        {
            get { return (Geometry)GetValue(ToggleButtonImageProperty); }
            set { SetValue(ToggleButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleButtonImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleButtonImageProperty =
            DependencyProperty.Register("ToggleButtonImage", typeof(Geometry), typeof(UseComboBox));



        public Geometry ToggleButtonImageIsChecked
        {
            get { return (Geometry)GetValue(ToggleButtonImageIsCheckedProperty); }
            set { SetValue(ToggleButtonImageIsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleButtonImageIsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleButtonImageIsCheckedProperty =
            DependencyProperty.Register("ToggleButtonImageIsChecked", typeof(Geometry), typeof(UseComboBox));



        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(UseComboBox), new PropertyMetadata(""));



    }
}
