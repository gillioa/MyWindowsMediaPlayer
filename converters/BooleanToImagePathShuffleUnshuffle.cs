using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Data;

namespace WpfMvvmSample.converters
{
    class BooleanToImagePathShuffleUnshuffle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool?)
            {
                bool? boolean = value as bool?;
                if (boolean.Value == true)
                    return "Images/UI_shuffle.png";
                else
                    return "Images/UI_unshuffle.png";
            }
            throw new Exception("Must not arrived");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Méthode non implémentée !");
        }
    }
}
