using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using log4net;

namespace Archimedes.Patterns.Container
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


        public void EnableAutoConfiguration()
        {
            var conf = new AutoModuleConfiguration(ScanComponents());
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
        /// Finds all types in the application context which are known components
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> ScanComponents()
        {
            if (_components == null)
            {
                _components = FindComponentTypes().ToList();
            }
            return _components;
        }

        private IEnumerable<Type> FindComponentTypes()               
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Log.Info("=> Scanning assembly " + assembly.FullName);
                var components = FindComponentTypes(assembly);

                foreach (var component in components)
                {
                    Log.Info("---> " + component.Name);
                    yield return component;
                }
            }
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

        
    }
}
