using System.Collections.ObjectModel;
using System.Linq;
using Archimedes.Patterns.Plugins;
using Archimedes.Patterns.WPF.ViewModels;

namespace Archimedes.Patterns.WPF.Plugins.ViewModel
{
    public class AllPluginsViewModel : WorkspaceViewModel
    {
        readonly PluginManager _pluginManager;
        public AllPluginsViewModel(PluginManager manager) {
            _pluginManager = manager;
            ImportPlugins();
            Name = "Manfreds Plugin Spass :)";
        }
        void ImportPlugins() {
            var pluginvms = from p in _pluginManager.GetAllPlugins()
                            where p != null
                            select new PluginViewModel(p);

            AllPlugins = new ObservableCollection<PluginViewModel>(pluginvms);
        }
        public ObservableCollection<PluginViewModel> AllPlugins { get; private set; }

        public string Name { get; private set; }
    }
}
