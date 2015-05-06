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
        #region Fields

        private readonly Dictionary<Type, object> _serviceRegistry = new Dictionary<Type, object>();
        private readonly ElderModuleConfiguration _configuration;
        private readonly string _name;

        #endregion

        #region Constructor

        internal ElderBox(ElderModuleConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Resolve an instance for the given Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve an instance for the given Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (_serviceRegistry.ContainsKey(type))
            {
                return _serviceRegistry[type];
            }
            var instance = ResolveInstanceFor(type);
            _serviceRegistry.Add(type, instance);

            return instance;
        }

        /// <summary>
        /// The name of the context of this container
        /// </summary>
        public string ContextName
        {
            get { return _name; }
        }

        /// <summary>
        /// Auto wire fields / property dependencies which are annotated with <see cref="InjectAttribute"/> 
        /// </summary>
        /// <param name="instance"></param>
        public void Autowire(object instance)
        {
            try
            {
                var targetFields = from f in instance.GetType().GetFields()
                    where f.IsDefined(typeof (InjectAttribute), false)
                    select f;

                foreach (var targetField in targetFields)
                {
                    if (targetField.GetValue(instance) == null)
                    {
                        // Only inject if the field is null
                        var fieldValue = Resolve(targetField.FieldType);
                        targetField.SetValue(instance, fieldValue);
                    }
                }
            }
            catch (Exception e)
            {
                throw new AutowireException("Autowiring of instance " + instance.GetType().Name + " failed!", e);
            }
        }

        #endregion

        #region Private methods

        private object ResolveInstanceFor(Type type)
        {
            if(type == null) throw new ArgumentNullException("type");

            object instance = null;
            // First check if we have a mapping for the given type
            Type implementationType = _configuration.GetImplementaionTypeFor(type);
            var typeForImplementation = implementationType ?? type;


            // Maybe this type has already been created
            if (_serviceRegistry.ContainsKey(typeForImplementation))
            {
                return _serviceRegistry[typeForImplementation];
            }


            if (CanCreateInstance(typeForImplementation))
            {
                instance = CreateInstance(typeForImplementation);
            }
            else
            {
                throw new NotSupportedException("Can not create instance for type '" + type.Name + "' which was resolved to implementation '" + typeForImplementation.Name+"'!");
            }

            return instance;
        }

        /// <summary>
        /// Creates a new instance from the given implementaiton
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        private object CreateInstance(Type implementationType)
        {
            if (implementationType == null) throw new ArgumentNullException("implementationType");


            // Try to create an instance of the given type.

            var constructor = (from c in implementationType.GetConstructors()
                              where c.GetCustomAttributes(typeof (InjectAttribute), false).Any()
                              select c).FirstOrDefault();

            if (constructor == null)
            {
                // There was no Constructor with the Inject Attribute.
                // Just get the first available one
                constructor = (from c in implementationType.GetConstructors()
                               where !c.IsPrivate
                               select c).FirstOrDefault();
            }

            if (constructor != null)
            {
                // We have found a constructor
                var instance = CreateInstanceWithConstructor(implementationType, constructor);
                
                Autowire(instance);
                
                return instance;
            }

            throw new NotSupportedException("Can not create an instance for type " + implementationType.Name + " - no viable (public/protected) constructor found.");
        }

        private object CreateInstanceWithConstructor(Type type, ConstructorInfo constructor)
        {
            // Resolve all parameters 
            var parameters = AutowireParameters(constructor);
            return Activator.CreateInstance(type, parameters);
        }

        /// <summary>
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        private object[] AutowireParameters(ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException("constructor");

            var parameterInfos = constructor.GetParameters();
            var parameters = new List<object>(parameterInfos.Length);

            foreach (var parameter in parameterInfos)
            {
                try
                {
                    var paramInstance = Resolve(parameter.ParameterType);
                    parameters.Add(paramInstance);
                }
                catch (Exception e)
                {
                    throw new AutowireException("Autowiring constructor parameter " + parameter.Name +"("+parameter.ParameterType+")" + " has failed!", e);
                }
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Is it possible to create an instance of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CanCreateInstance(Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }

        #endregion

        public override string ToString()
        {
            return ContextName + " holding " + _serviceRegistry.Count + " entries!";
        }
    }
}
