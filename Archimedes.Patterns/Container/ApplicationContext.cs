using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Archimedes.Patterns.Container
{
    public sealed class ApplicationContext
    {
        private const string _defaultContext = "_ELDER_DEFAULT";

        private List<Type> _components = new List<Type>();
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
            var conf = new AutoModuleConfiguration(AutoFindComponents());
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
        public IEnumerable<Type> AutoFindComponents()
        {
            if (_components != null)
            {
                _components = GetTypesWith<ServiceAttribute>(false).ToList();
            }
            return _components;
        }

        private IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
                              where TAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }
    }
}
