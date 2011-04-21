using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AvalonDock
{   
    
    public class UnDockableFloatingWindow : FloatingWindow
    {
        static UnDockableFloatingWindow()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnDockableFloatingWindow), new FrameworkPropertyMetadata(typeof(UnDockableFloatingWindow)));

            ContentControl.ContentProperty.OverrideMetadata(typeof(UnDockableFloatingWindow), 
                new FrameworkPropertyMetadata(
                    new PropertyChangedCallback(OnContentPropertyChanged), 
                    new CoerceValueCallback(OnCoerceValueContentProperty)));
        }

        internal UnDockableFloatingWindow(DockingManager manager)
            : base(manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");
        }

        public override Pane ClonePane() {
            throw new NotImplementedException();
        }



        static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // ...
        }

        static object OnCoerceValueContentProperty(DependencyObject d, object baseValue) {
            UnDockableFloatingWindow fl = ((UnDockableFloatingWindow)d);

            if (fl.Content != null) {
                throw new InvalidOperationException("Content on floating windows can't be set more than one time.");
            }

            if (!(baseValue is DockableContent) &&
                !(baseValue is DockablePane)) {
                throw new InvalidOperationException("Content must be of type DockableContent or DockablePane");
            }

            FloatingDockablePane paneToReturn = null;

            if (baseValue is DockableContent)
                paneToReturn = new FloatingDockablePane(fl, baseValue as DockableContent);
            else if (baseValue is DockablePane)
                paneToReturn = new FloatingDockablePane(fl, baseValue as DockablePane);

            return paneToReturn;
        }
    }
    
}
