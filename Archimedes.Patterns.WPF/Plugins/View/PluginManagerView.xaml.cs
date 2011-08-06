using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Archimedes.Patterns.WPF.Plugins.View
{
    /// <summary>
    /// Interaction logic for PluginManagerView.xaml
    /// </summary>
    public partial class PluginManagerView : Window
    {
        public PluginManagerView() {
            InitializeComponent();

            this.button1.Command = new CloseCommand(this);
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }

    class CloseCommand : ICommand
    {
        readonly Window _window;

        public CloseCommand(Window w) {
            _window = w;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            _window.Close();
        }
    }
}
