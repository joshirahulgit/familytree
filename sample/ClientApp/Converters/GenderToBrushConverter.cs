using DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApp.Converters
{
    public class GenderToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Gender)
            {
                Gender gender = (Gender)value;
                switch (gender)
                {
                    case Gender.Female:
                        return new SolidColorBrush(Colors.Red);
                    case Gender.Male:
                        return new SolidColorBrush(Colors.Blue);
                }
            }
            throw new TypeAccessException("Expexted type is: Gender");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
