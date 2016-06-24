using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfMvvmSample.converters
{
    public class BooleanToImagePathPlayPause : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool?)
            {
                bool? boolean = value as bool?;
                if (boolean.Value == true)
                   return "Images/UI_pause.png";
                else
                   return "Images/UI_play.png";
            }
            throw new Exception("Must not arrived");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Méthode non implémentée !");
        }
    }

}
