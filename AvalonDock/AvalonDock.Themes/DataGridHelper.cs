
// Copyright (c) Trevor Webster
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using MS.Internal;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace AvalonDock.Themes
{
    public static class DataGridHelper
    {



		#region SelectedCells

		public static IList<DataGridCellInfo> GetSelectedCells(DependencyObject obj)
		{
			return (IList<DataGridCellInfo>)obj.GetValue(SelectedCellsProperty);
		}
		public static void SetSelectedCells(DependencyObject obj, IList<DataGridCellInfo> value)
		{
			obj.SetValue(SelectedCellsProperty, value);
		}
		public static readonly DependencyProperty SelectedCellsProperty =
			DependencyProperty.RegisterAttached("SelectedCells", typeof(IList<DataGridCellInfo>), typeof(DataGridHelper), new UIPropertyMetadata(null, OnSelectedCellsChanged));
		static SelectedCellsChangedEventHandler GetSelectionChangedHandler(DependencyObject obj)
		{
			return (SelectedCellsChangedEventHandler)obj.GetValue(SelectionChangedHandlerProperty);
		}
		static void SetSelectionChangedHandler(DependencyObject obj, SelectedCellsChangedEventHandler value)
		{
			obj.SetValue(SelectionChangedHandlerProperty, value);
		}
		static readonly DependencyProperty SelectionChangedHandlerProperty =
			DependencyProperty.RegisterAttached("SelectedCellsChangedEventHandler", typeof(SelectedCellsChangedEventHandler), typeof(DataGridHelper), new UIPropertyMetadata(null));

		//d is MultiSelector (d as ListBox not supported)
		static void OnSelectedCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (GetSelectionChangedHandler(d) != null)
				return;

			if (d is DataGrid)//DataGrid
			{
				DataGrid datagrid = d as DataGrid;
				SelectedCellsChangedEventHandler selectionchanged = null;
				foreach (var selected in GetSelectedCells(d) as IList<DataGridCellInfo>)
					datagrid.SelectedCells.Add(selected);

				selectionchanged = (sender, e) =>
				{
					SetSelectedCells(d, datagrid.SelectedCells);
				};
				SetSelectionChangedHandler(d, selectionchanged);
				datagrid.SelectedCellsChanged += GetSelectionChangedHandler(d);
			}
			//else if (d is ListBox)
			//{
			//    ListBox listbox = d as ListBox;
			//    SelectionChangedEventHandler selectionchanged = null;

			//    selectionchanged = (sender, e) =>
			//    {
			//        SetSelectedCells(d, listbox.SelectedCells);
			//    };
			//    SetSelectionChangedHandler(d, selectionchanged);
			//    listbox.SelectionChanged += GetSelectionChangedHandler(d);
			//}
		}

		#region HorizontalMouseWheelScrollingEnabled
		public static bool GetHorizontalMouseWheelScrollingEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(HorizontalMouseWheelScrollingEnabledProperty);
		}
		public static void SetHorizontalMouseWheelScrollingEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(HorizontalMouseWheelScrollingEnabledProperty, value);
		}
		public static readonly DependencyProperty HorizontalMouseWheelScrollingEnabledProperty =
			DependencyProperty.RegisterAttached("HorizontalMouseWheelScrollingEnabled", typeof(bool), typeof(DataGridHelper), new UIPropertyMetadata(false, OnHorizontalMouseWheelScrollingEnabledChanged));
		static void OnHorizontalMouseWheelScrollingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (d is DataGrid)
			{
				DataGrid datagrid = d as DataGrid;
				datagrid.Loaded += (sender, e) =>
				{
					var obj = datagrid.Template.FindName("DG_ScrollViewer", datagrid);
					if (obj is ScrollViewer)
					{
						ScrollViewer scrollviewer = obj as ScrollViewer;
						var mhelper = new HorizontalMouseScrollHelper(scrollviewer, datagrid);
					}

				};
			}


		}
		#endregion

		public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
		{
			FrameworkElement parent = LogicalTreeHelper.GetParent(element) as FrameworkElement;
			//parent.FindName
			while (parent != null)
			{
				T correctlyTyped = parent as T;
				if (correctlyTyped != null)
					return correctlyTyped;
				else
					return FindParent<T>(parent);
			}

			return null;
		}

		#endregion



        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindTemplatedParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

    }
}
