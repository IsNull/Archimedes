using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Archimedes.Patterns.WPF.Behaviours
{
    public static class ControlExtensions
    {
        public static ItemsControl FindItemsConrolParent(this FrameworkElement target) {
            ItemsControl result = null;
            result = target.Parent as ItemsControl;
            if (result != null)
                return result;
            result = ItemsControl.ItemsControlFromItemContainer(target);
            if (result != null)
                return result;
            return FindVisualParent<ItemsControl>(target);
        }
        public static T FindVisualParent<T>(FrameworkElement target) where T : FrameworkElement {
            if (target == null)
                return null;
            var visParent = VisualTreeHelper.GetParent(target);
            var result = visParent as T;
            if (result != null)
                return result;
            return FindVisualParent<T>(visParent as FrameworkElement);
        }
        public static T GetTemplateChild<T>(this Control target, string templtePartName) where T : FrameworkElement {
            if (target == null)
                throw new ArgumentNullException("target", "Cannot get the templated child of a null object");
            var childCount = VisualTreeHelper.GetChildrenCount(target);
            if (childCount == 0)
                return null;
            return (VisualTreeHelper.GetChild(target, 0) as FrameworkElement).FindName(templtePartName) as T;
        }
    }
}
