using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace Archimedes.Patterns.WPF.Converters
{
    [ValueConversion(typeof(ICommand), typeof(Visibility))]   
    public class CommandToVisibiltyConverter : BaseConverter
    {

        static bool _hideNonExecutableCommands = true;

        /// <summary>
        /// Defines if Commands which are "Non Executable" are hidden from Menus
        /// </summary>
        public static bool HideNonExecutableCommands {
            get {
                return _hideNonExecutableCommands;
            }
            set {
                _hideNonExecutableCommands = value;
            }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (HideNonExecutableCommands) {
                var cmd = value as ICommand;
                if (cmd == null)
                    return Visibility.Collapsed;
                return (cmd.CanExecute(parameter)) ? Visibility.Visible : Visibility.Collapsed;
            } else {
                return Visibility.Visible;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
