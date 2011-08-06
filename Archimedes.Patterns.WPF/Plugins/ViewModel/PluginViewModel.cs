using System;
using Archimedes.Patterns.MVMV;
using Archimedes.Patterns.Plugins;

namespace Archimedes.Patterns.WPF.Plugins.ViewModel
{
    public class PluginViewModel : ViewModelBase
    {
        readonly IPlugin _plugin;

        public PluginViewModel(IPlugin plugin) {
            if (plugin == null)
                throw new ArgumentNullException("plugin");

            _plugin = plugin;
        }

        #region IPlugin PropertyWrapper

        /// <summary>
        /// Human friendly Name of this Plugin
        /// </summary>
        public string Name { 
            get { return _plugin.Name; }
        }

        public string Group { 
            get { return _plugin.Group; }
        }

        public string DllLocation { 
            get { return _plugin.DllLocation; } 
        }

        /// <summary>
        /// Description of the Plugin
        /// </summary>
        public string Description {
            get { return _plugin.Description; } 
        }

        /// <summary>
        /// CopyRight/Manufracturer of the Plugin
        /// </summary>
        public string CopyRight { 
            get { return _plugin.CopyRight; } 
        }

        public Guid GUID { 
            get { return _plugin.GUID; } 
        }

        #endregion
    }
}
