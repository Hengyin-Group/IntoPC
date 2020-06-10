using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace IntoApp.Converters
{
    public class BoolToVisiblityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var param = parameter as string;
                //是否反转
                var isInvert = false;
                if (!string.IsNullOrEmpty(param))
                {
                    bool.TryParse(param, out isInvert);
                }
                if ((bool)value)
                {
                    return isInvert ? Visibility.Collapsed: Visibility.Visible;
                }
                else
                {
                    return isInvert ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
