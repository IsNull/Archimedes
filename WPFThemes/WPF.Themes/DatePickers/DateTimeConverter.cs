using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;


namespace WPF.Themes.DatePickers
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            DateTime? selectedDate = value as DateTime?;

            if (selectedDate != null) {
                string dateTimeFormat = parameter as string;
                return selectedDate.Value.ToString(dateTimeFormat);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
