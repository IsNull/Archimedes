using System;
using System.Windows;
using System.Windows.Documents;

namespace Archimedes.Patterns.WPF.FlowDocuments
{
    public class BindableRun : Run
    {
        public static readonly DependencyProperty BoundTextProperty = DependencyProperty.Register(
            "BoundText", typeof(string), typeof(BindableRun), new PropertyMetadata(OnBoundTextChanged));

        public BindableRun() {
            DocHelpers.FixupDataContext(this);
        }

        private static void OnBoundTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Run)d).Text = (string)e.NewValue;
        }

        public String BoundText {
            get { return (string)GetValue(BoundTextProperty); }
            set { SetValue(BoundTextProperty, value); }
        }
    }
}
