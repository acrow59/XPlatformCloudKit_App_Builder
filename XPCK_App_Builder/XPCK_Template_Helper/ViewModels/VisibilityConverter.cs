using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace XPCK_Template_Helper.ViewModels
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if(parameter.Equals("1"))
                    return ((Boolean)value) == true ? Visibility.Collapsed : Visibility.Visible;
                else
                    return ((Boolean)value) == true ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return ((Boolean)value) == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
