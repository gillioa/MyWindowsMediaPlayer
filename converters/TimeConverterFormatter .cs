using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfMvvmSample.converters
{
    public class PositionToStringTimeConverter : IValueConverter
    {
        public static PositionToStringTimeConverter Instance
        {
            get { return _instance ?? (_instance = new PositionToStringTimeConverter()); }
        }

        private static PositionToStringTimeConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            String ret = new DateTime(timeSpan.Ticks).ToString("HH:mm:ss");
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Méthode non implémentée");
        }
    }

    public class MaximumToValueConverter : IValueConverter
    {
        public static MaximumToValueConverter Instance
        {
            get { return _instance ?? (_instance = new MaximumToValueConverter()); }
        }

        private static MaximumToValueConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var maximum = (Double)value;
            DateTime myDate = DateTime.FromOADate(maximum);
            return myDate.ToString("HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Méthode non implémentée");
        }
    }

    
   
}
