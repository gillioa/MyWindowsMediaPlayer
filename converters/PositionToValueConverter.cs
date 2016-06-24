using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfMvvmSample.converters
{
        public class PositionToValueConverter : IValueConverter
        {
            public static PositionToValueConverter Instance
            {
                get { return _instance ?? (_instance = new PositionToValueConverter()); }
            }

            private static PositionToValueConverter _instance;

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var time = (TimeSpan)value;
                var ret = time.TotalSeconds;
                return ret;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var time = System.Convert.ToInt32((double)value);
                var ret = new TimeSpan(0, 0, 0, time, 0);
                return ret;
            }
        }
}
