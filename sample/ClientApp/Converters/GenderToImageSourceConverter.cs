using DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClientApp.Converters
{
    class GenderToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Gender)
            {
                Gender gender = (Gender)value;
                switch (gender)
                {
                    case Gender.Female:
                        return "/ClientApp;Component/Contents/Images/Female-icon.png";
                    case Gender.Male:
                        return "/ClientApp;Component/Contents/Images/Male-icon.png";
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
