using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Archimedes.Patterns.Converters;

namespace Archimedes.Patterns.WPF.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(DateTime))]
    public class TimeSpanToDateTimeConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return TimeSpanConverter.Convert((TimeSpan)value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return TimeSpanConverter.ConvertBack((DateTime)value);
        }
    }
}
