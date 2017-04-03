using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ClientApp.Converters
{
    public class BooleanToCollapseConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                if (bool.Parse(value.ToString()))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            throw new TypeAccessException("Expexted type is: bool");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
