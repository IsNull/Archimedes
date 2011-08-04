﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace WPF.Themes.Helper
{
    public class DataGridRowHeaderCommand : ICommand
    {

        DataGridRow _prevRow;
        /// <summary>
        /// Collapse/Expands the selected DataGridRow.
        /// </summary>
        /// <param name="parameter">The DataGridRowHeader</param>
        public void Execute(object parameter) {
            var rowHeader = parameter as DataGridRowHeader;
            var row = DataGridHelper.FindTemplatedParent<DataGridRow>(rowHeader) as DataGridRow;
            if (_prevRow is DataGridRow
                && _prevRow != row
                && DataGridHelper.FindVisualParent<DataGrid>(_prevRow) == DataGridHelper.FindVisualParent<DataGrid>(rowHeader)) {	//collapse the previously selected row
                _prevRow.DetailsVisibility = Visibility.Collapsed;
            }
            if (row.DetailsVisibility == Visibility.Visible)
                row.DetailsVisibility = Visibility.Collapsed;
            else
                row.DetailsVisibility = Visibility.Visible;

            _prevRow = row;
        }

        public bool CanExecute(object parameter) {
            return true;
        }


        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }
}
