﻿using DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                object brush = null;
                Gender gender = (Gender)value;
                switch (gender)
                {
                    case Gender.Female:
                        brush = (RadialGradientBrush)Application.Current.Resources["FemaleBG"];
                        break;
                    case Gender.Male:
                        brush = (RadialGradientBrush)Application.Current.Resources["MaleBG"];
                        break;
                }
                if (parameter != null && parameter.ToString().ToLower().Equals("border") && brush != null)
                {
                    brush = new SolidColorBrush(Colors.WhiteSmoke);// Multiply(brush.Color, 10);
                }
                return brush;
            }
            throw new TypeAccessException("Expexted type is: Gender");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
