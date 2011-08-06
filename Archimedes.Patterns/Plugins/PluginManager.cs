using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Archimedes.Patterns;

namespace Archimedes.Patterns.Plugins
{
    /// <summary>
    /// Manages all Plugins of an Application, Loads / unloads Plugins. (Singleton)
    /// </summary>
    public sealed class PluginManager : Singleton<PluginManager>
    {
        Dictionary<Type, IPluginHostProvider> _pluginHosts = new Dictionary<Type, IPluginHostProvider>();
        List<Assembly> _pluginAssemblys = null;

        /// <summary>
        /// Gets all Plugin Assemblys in the Plugin dir
        /// </summary>
        public IEnumerable<Assembly> PluginAssemblys {
            get {
                if (_pluginAssemblys == null) {
                    ReloadPluginAssemblys();
                }
                return _pluginAssemblys;
            }
        }

        /// <summary>
        /// Reloads all Plugins (rescans Plugin Dir, refereshes plugin Instances)
        /// </summary>
        public void ReloadPlugins() {
            ReloadPluginAssemblys();                // reload plugin dlls
            foreach (var host in _pluginHosts) {     // reload classes in the host
                host.Value.Reload();
            }
        }


        /// <summary>
        /// Reloads (rescans Plugin Dir) all Plugin Assemblys
        /// </summary>
        private void ReloadPluginAssemblys() {
            if (_pluginAssemblys != null)
                _pluginAssemblys.Clear();
            _pluginAssemblys = LoadPlugInAssemblies();
        }

        /// <summary>
        /// Get all PluginHosts of a specific Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllHosts<T>() where T : PluginHost
        {
            var convhosts = new List<T>();

            if (_pluginHosts.ContainsKey(typeof(T))) {
                var hosts = _pluginHosts[typeof(T)].PluginHosts;
                foreach (var h in hosts) {
                    convhosts.Add(h as T);
                }
                return convhosts;
            } else
                throw new NotSupportedException("Provided type not found!");
        }

        public IEnumerable<IPlugin> GetAllPlugins() {
            foreach (var hostProvider in _pluginHosts.Values)
            {
                foreach (var h in hostProvider.PluginHosts)
                    yield return h.Plugin;
            }
        }

        /// <summary>
        /// Register a PluginHost Type
        /// </summary>
        /// <typeparam name="T">Type of PluginHost to Register</typeparam>
        public void RegisterHost<T>() where T : PluginHost
        {
            if (!_pluginHosts.ContainsKey(typeof(T))){
#if !DEBUG
                try {
#endif
                    var d1 = typeof(PluginHostProvider<>);
                    Type[] typeArgs = { typeof(T) };
                    var makeme = d1.MakeGenericType(typeArgs);
                    var pluginHostProvider = Activator.CreateInstance(makeme) as IPluginHostProvider;
                    pluginHostProvider.Args = this.Args;
                    _pluginHosts.Add(typeof(T), pluginHostProvider);
#if !DEBUG
                } catch (Exception e) {

                }
#endif
            }
        }


        private static List<Assembly> LoadPlugInAssemblies() {
            var plugInAssemblyList = new List<Assembly>();

            try {
                var dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
                var files = dInfo.GetFiles("*.dll");

                if (null != files) {
                    foreach (FileInfo file in files) {
                        try {
                            plugInAssemblyList.Add(Assembly.LoadFile(file.FullName));
                        } catch(NotSupportedException) {
#if DEBUG
                            throw;
#endif
                        }
                    }
                }
            } catch (DirectoryNotFoundException) {
                // ignore (dont load any plugins)
                Debug.Print("Can't find Plugin Directory!");
            }
            return plugInAssemblyList;
        }

        /// <summary>
        /// Commandline Arguments to submit to the Plugins
        /// </summary>
        public string[] Args {
            get;
            set;
        }
    }
}
