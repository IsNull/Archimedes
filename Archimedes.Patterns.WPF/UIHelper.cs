using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Archimedes.Patterns.WPF
{
        /// <summary>
        /// Helper methods for UI-related tasks.
        /// </summary>
        public static class UIHelper
        {
            /// <summary>
            /// Finds a parent of a given item on the visual tree.
            /// </summary>
            /// <typeparam name="T">The type of the queried item.</typeparam>
            /// <param name="child">A direct or indirect child of the
            /// queried item.</param>
            /// <returns>The first parent item that matches the submitted
            /// type parameter. If not matching item can be found, a null
            /// reference is being returned.</returns>
            public static T TryFindParent<T>(DependencyObject child)
              where T : DependencyObject {
                //get parent item
                DependencyObject parentObject = GetParentObject(child);

                //we've reached the end of the tree
                if (parentObject == null) return null;

                //check if the parent matches the type we're looking for
                T parent = parentObject as T;
                if (parent != null) {
                    return parent;
                } else {
                    //use recursion to proceed with next level
                    return TryFindParent<T>(parentObject);
                }
            }

            /// <summary>
            /// This method is an alternative to WPF's
            /// <see cref="VisualTreeHelper.GetParent"/> method, which also
            /// supports content elements. Do note, that for content element,
            /// this method falls back to the logical tree of the element!
            /// </summary>
            /// <param name="child">The item to be processed.</param>
            /// <returns>The submitted item's parent, if available. Otherwise
            /// null.</returns>
            public static DependencyObject GetParentObject(DependencyObject child) {
                if (child == null) return null;
                ContentElement contentElement = child as ContentElement;

                if (contentElement != null) {
                    DependencyObject parent = ContentOperations.GetParent(contentElement);
                    if (parent != null) return parent;

                    FrameworkContentElement fce = contentElement as FrameworkContentElement;
                    return fce != null ? fce.Parent : null;
                }

                //if it's not a ContentElement, rely on VisualTreeHelper
                return VisualTreeHelper.GetParent(child);
            }


            /// <summary>
            /// Remove the FrameworkElement from its Parent
            /// </summary>
            /// <param name="frameworkelement"></param>
            public static void RemoveElementFromParent(FrameworkElement frameworkelement) {
                if (frameworkelement.Parent is ItemsControl) {
                    var itemsParent = frameworkelement.Parent as ItemsControl;
                    itemsParent.Items.Remove(frameworkelement);
                } else if (frameworkelement.Parent is Panel) {
                    var itemsParent = frameworkelement.Parent as Panel;
                    itemsParent.Children.Remove(frameworkelement);
                } else {
                    throw new NotSupportedException("Cant find Parent Items/Chlidren List!");
                }
            }

            public static void RemoveFromParent(this FrameworkElement frameworkelement) {
                UIHelper.RemoveElementFromParent(frameworkelement);
            }
        }
    
}
