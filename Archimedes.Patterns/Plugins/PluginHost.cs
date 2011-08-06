using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Plugins
{
    public abstract class PluginHost
    {
        readonly protected IPlugin mplugin;

        public PluginHost(IPlugin plugin) {
            mplugin = plugin;
        }

        public IPlugin Plugin {
            get {
                return mplugin;
            }
        }

        public abstract Type PluginInterface { get; }
        public abstract Type PluginAttribute { get; }
    }
}
