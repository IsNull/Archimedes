using System;
namespace Archimedes.Patterns.Plugins
{
    interface IPluginHostProvider
    {
        System.Collections.Generic.List<PluginHost> PluginHosts { get; }

        string[] Args { get; set; }

        void Reload();
    }
}
