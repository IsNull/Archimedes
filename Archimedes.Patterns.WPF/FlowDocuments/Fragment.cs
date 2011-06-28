using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Archimedes.Patterns.WPF.FlowDocuments
{
    [ContentProperty("Content")]
    public class Fragment : FrameworkElement
    {
        private static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(FrameworkContentElement), typeof(Fragment));

        public FrameworkContentElement Content {
            get {
                return (FrameworkContentElement)GetValue(ContentProperty);
            }
            set {
                SetValue(ContentProperty, value);
            }
        }
    }
}
