using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Archimedes.DI.AOP;
using log4net;

namespace Archimedes.DI
{
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
        ///   
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.</param>
        public void EnableAutoConfiguration(params string[] assemblyFilters)
        {
            var conf = new AutoModuleConfiguration(ScanComponents(assemblyFilters));
            RegisterContext(_defaultContext, conf);
        }


        public ElderBox GetContext()
        {
            return _contextRegistry[_defaultContext];
        }

        public ElderBox GetContext(string name)
        {
            if (_contextRegistry.ContainsKey(name))
            {
                return _contextRegistry[name];
            }
            
            throw new NotSupportedException("ElderBox Context with name '" + name + "' could not be found. Did you forget to register it?" );
        }

        public void RegisterContext(string name, ElderModuleConfiguration configuration)
        {
            _contextRegistry.Add(name, new ElderBox(configuration));
        }

        /// <summary>
        /// 
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
