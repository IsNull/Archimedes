using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace Archimedes.Patterns.WPF.Converters
{
    /// <summary>
    /// Base Class for easy to use ValueConverters. 
    /// (Subclassing from MarkupExtension allows easy to use style of the Converter: Text={Binding Time, Converter={x:MyConverter}}) 
    /// This Class is abstract.
    /// </summary>
    public abstract class BaseConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
    }
}
