using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Archimedes.Patterns.Plugins
{
    /// <summary>
    /// Provides a List with all Plugins from <paramref name="T"/> Type.
    /// </summary>
    /// <typeparam name="T">Plugin Host Type to load</typeparam>
    public class PluginHostProvider<T> : IPluginHostProvider where T : PluginHost, new()
    {
        private List<T> pluginHosts;
        Converter<T, PluginHost> hostConverter = new Converter<T, PluginHost>(PluginHostConverter);

        public PluginHostProvider() { }

        public List<PluginHost> PluginHosts {
            get {
                return TypedPluginHosts.ConvertAll<PluginHost>(hostConverter);
            }
        }

        /// <summary>
        /// CMD Startup Arguments for the Plugins
        /// </summary>
        public string[] Args { get; set; }

        /// <summary>
        /// Strongly typed PluginHost List of all loaded Plugins from this type.
        /// </summary>
        public List<T> TypedPluginHosts {
            get {
                if (null == pluginHosts)
                    Reload();
                return pluginHosts;
            }
        }

        /// <summary>
        /// Re(Loads) all class/Code from the Plugin assemblies. Make sure that PluginAssemblys also was reloaded before.
        /// </summary>
        public void Reload() {
            if (null == pluginHosts)
                pluginHosts = new List<T>();
            else {
                foreach (var host in pluginHosts)
                    host.Plugin.OnUnLoad();
                pluginHosts.Clear();
            }
            var plugInAssemblies = PluginManager.Instance.PluginAssemblys;
            var plugIns = GetPlugIns(plugInAssemblies);

            foreach (var p in plugIns) {
                ConstructorInfo classConstructor = typeof(T).GetConstructor(new Type[] { p.GetType() });
                T classInstance = (T)classConstructor.Invoke(new object[] { p });
                pluginHosts.Add(classInstance);
                p.OnLoad(Args);
            }
        }

        #region Private Methods

        /// <summary>
        /// Get all Plugin Dlls for which match this Plugin Interface
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        List<IPlugin> GetPlugIns(IEnumerable<Assembly> assemblies) {
            var availableTypes = new List<Type>();
            var pluginHost = new T();

            foreach (Assembly currentAssembly in assemblies)
                availableTypes.AddRange(currentAssembly.GetTypes());

            // get a list of objects that implement the ICalculator interface AND 
            // have the CalculationPlugInAttribute
            List<Type> pluginList = availableTypes.FindAll(delegate(Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                object[] arr = t.GetCustomAttributes(pluginHost.PluginAttribute, true);
                return !(arr == null || arr.Length == 0) && interfaceTypes.Contains(pluginHost.PluginInterface);
            });

            // convert the list of Objects to an instantiated list of IPlugins
            return pluginList.ConvertAll<IPlugin>(delegate(Type t) { return Activator.CreateInstance(t) as IPlugin; });
        }

        protected static PluginHost PluginHostConverter(T input) {
            return input as PluginHost;
        }

        #endregion
    }
}
