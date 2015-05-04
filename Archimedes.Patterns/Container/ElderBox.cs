using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// A very lightweight dependency injection container wich requires virtually no configuration.
    /// </summary>
    public class ElderBox
    {

        private readonly Dictionary<Type, object> _serviceRegistry = new Dictionary<Type, object>();
        private readonly ElderModuleConfiguration _configuration;
        private readonly string _name;


        internal ElderBox(ElderModuleConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (_serviceRegistry.ContainsKey(type))
            {
                return _serviceRegistry[type];
            }
            var instance = ResolveInstanceFor(type);
            _serviceRegistry.Add(type, instance);

            return instance;
        }

        public string ContextName
        {
            get { return _name; }
        }

        /// <summary>
        /// Auto wire fields / property dependencies which are annotated with Inject
        /// </summary>
        /// <param name="instance"></param>
        public void Autowire(object instance)
        {

            var targetFields = from f in instance.GetType().GetFields()
                               where f.IsDefined(typeof(InjectAttribute), false)
                               select f;

            foreach (var targetField in targetFields)
            {
                if (targetField.GetValue(instance) == null)
                { // Only inject if the field is null
                    var fieldValue = Resolve(targetField.FieldType);
                    targetField.SetValue(instance, fieldValue);
                }
            }

        }



        private object ResolveInstanceFor(Type t)
        {
            object instance = null;
            // First check if we have a mapping for the given type
            Type implementationType = _configuration.GetImplementaionTypeFor(t);

            // Maybe this type has already been created
            if (_serviceRegistry.ContainsKey(implementationType))
            {
                return _serviceRegistry[implementationType];
            }

            if (implementationType != null)
            {
                instance = CreateInstance(implementationType);
            }
            else
            {
               // No mapping defined. If the requested type is a class itself,
               // we may be able to instance it directly
                if (CanCreateInstance(t))
                {
                    instance = CreateInstance(t);
                }
            }
            return instance;
        }

        private object CreateInstance(Type implementationType)
        {
            // Try to create an instance of the given type.

            var constructor = (from c in implementationType.GetConstructors()
                              where c.GetCustomAttributes(typeof (InjectAttribute), false).Any()
                              select c).FirstOrDefault();

            if (constructor == null)
            {
                constructor = implementationType.GetConstructors().FirstOrDefault();
            }

            if (constructor != null)
            {
                return CreateInstanceWithConstructor(implementationType, constructor);
            }

            throw new NotSupportedException("Can not create an instance of class " + implementationType.Name);
        }

        private object CreateInstanceWithConstructor(Type type, ConstructorInfo constructor)
        {
            // Resolve all parameters 
            var parameters = ResolveParameters(constructor);
            return Activator.CreateInstance(type, parameters);
        }

        /// <summary>
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        private object[] ResolveParameters(ConstructorInfo constructor)
        {
            var parameterInfos =constructor.GetParameters();
            List<object> parameters = new List<object>(parameterInfos.Length);

            foreach (var parameter in parameterInfos)
            {
                var paramInstance = Resolve(parameter.ParameterType);
                parameters.Add(paramInstance);
            }
            return parameters.ToArray();
        }

        private bool CanCreateInstance(Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }

        public override string ToString()
        {
            return ContextName + " holding " + _serviceRegistry.Count + " entries!";
        }
    }
}
