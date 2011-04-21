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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvalonDock.DemoApp
{
    /// <summary>
    /// Interaction logic for SampleDockableContent.xaml
    /// </summary>
    public partial class SampleDockableContent : DockableContent
    {
        public SampleDockableContent()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var dockableContent = new DockableContent()
            {
                Name = "mynewContent",
                Title = "New Dock Content!"
            };
            dockableContent.Content = new Button() { Content = "Hello Wurld:D", Width = 150, Height = 30 };
            dockableContent.DockableStyle = DockableStyle.Floating;
            dockableContent.HideOnClose = false;
            dockableContent.Closed += OnClosed;
            
            dockableContent.ShowAsFloatingWindow(this.Manager, false);
        }

        void OnClosed(object sender, EventArgs e) {
            MessageBox.Show("asf");
        }
    }
}
