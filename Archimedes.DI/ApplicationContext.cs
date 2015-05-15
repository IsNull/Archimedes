using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Archimedes.DI.AOP;
using log4net;

namespace Archimedes.DI
{
    /// <summary>
    /// Represents the Application Context, which provides component scanning / auto-configuration.
    /// </summary>
    public sealed class ApplicationContext
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string _defaultContext = "_ELDER_DEFAULT";

        private List<Type> _components = null; // Lazy initialized!
        private readonly Dictionary<string, ElderBox> _contextRegistry = new Dictionary<string, ElderBox>();

        #region Singleton

        private static readonly ApplicationContext _instance = new ApplicationContext();

        public static ApplicationContext Instance
        {
            get { return _instance; }
        }

        private ApplicationContext()
        {
            
        }

        #endregion


        /// <summary>
        ///  Enables Auto-Configuration, which basically scans for Components.
        /// 
        ///  Components must be marked with [Service] or [Controller].
        /// 
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.
        /// If none provided, all assemblies are scanned.</param>
        public void EnableAutoConfiguration(params string[] assemblyFilters)
        {
            var conf = new AutoModuleConfiguration(ScanComponents(assemblyFilters));
            RegisterContext(_defaultContext, conf);
        }

        /// <summary>
        /// Gets the default context which is populated by the auto configuration.
        /// </summary>
        /// <returns></returns>
        public ElderBox GetContext()
        {
            return _contextRegistry[_defaultContext];
        }

        /// <summary>
        /// Get a named DI context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ElderBox GetContext(string name)
        {
            if (_contextRegistry.ContainsKey(name))
            {
                return _contextRegistry[name];
            }
            
            throw new NotSupportedException("ElderBox Context with name '" + name + "' could not be found. Did you forget to register it?" );
        }

        /// <summary>
        /// Register a named DI context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        public void RegisterContext(string name, ElderModuleConfiguration configuration)
        {
            _contextRegistry.Add(name, new ElderBox(configuration));
        }

        /// <summary>
        /// Finds all types in the application context which are known components
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.</param>
        /// <returns></returns>
        public IEnumerable<Type> ScanComponents(params string[] assemblyFilters)
        {
            if (_components == null)
            {
                _components = FindComponentTypes(assemblyFilters).ToList();
            }
            return _components;
        }

        #region Private Implementation

        private IEnumerable<Type> FindComponentTypes(string[] assemblyFilters)
        {
            Log.Info("Assembly Component-Scanning restricted to: " + string.Join(", ", assemblyFilters));
            
            EnsureAssembliesAreLoadedForComponentScan();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (ScanAssembly(assembly, assemblyFilters))
                {
                    Log.Info("==  Scanning assembly " + assembly.GetName().Name + "  ==");
                    var components = FindComponentTypes(assembly);

                    foreach (var component in components)
                    {
                        Log.Info("      * " + component.Name);
                        yield return component;
                    }
                    Log.Info(" == == ");
                }
            }
        }

        private bool ScanAssembly(Assembly assembly, string[] assemblyFilters)
        {
            if (assemblyFilters.Length == 0) return true;

            foreach (var filter in assemblyFilters)
            {
                if (Regex.IsMatch(assembly.GetName().Name, filter, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


        private IEnumerable<Type> FindComponentTypes(Assembly assembly)
        {
            var service = typeof(ServiceAttribute);
            var component = typeof(ComponentAttribute);
            var controller = typeof(ControllerAttribute);

            return from t in assembly.GetTypes()
                   where t.IsDefined(service, false) || t.IsDefined(component, false) || t.IsDefined(controller, false)
                   select t;
        }

        private void EnsureAssembliesAreLoadedForComponentScan()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));
        }

        #endregion


    }
}
