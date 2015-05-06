using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Archimedes.DI.AOP;

namespace Archimedes.DI
{
    /// <summary>
    /// A very lightweight dependency injection container wich requires virtually no configuration.
    /// </summary>
    public class ElderBox
    {
        #region Fields

        private readonly Dictionary<Type, object> _serviceRegistry = new Dictionary<Type, object>();
        private readonly IModuleConfiguration _configuration;
        private readonly string _name;

        #endregion

        #region Constructor

        public ElderBox(IModuleConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            configuration.Configure();
            _configuration = configuration;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Resolve an instance for the given Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve an instance for the given Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Resolve(Type type)
        {
            return Resolve(type, new HashSet<Type>());
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
        [DebuggerStepThrough]
        public void Autowire(object instance)
        {
            Autowire(instance, new HashSet<Type>());
        }

        #endregion

        #region Private methods

        [DebuggerStepThrough]
        private object Resolve(Type type, HashSet<Type> unresolvedDependencies)
        {
            if (type == null) throw new ArgumentNullException("type");


            if (_serviceRegistry.ContainsKey(type))
            {
                return _serviceRegistry[type];
            }

            unresolvedDependencies.Add(type); // Mark this type as unresolved
            var instance = ResolveInstanceFor(type, unresolvedDependencies);

            if (instance != null)
            {
                // Successfully resolved the instance:
                unresolvedDependencies.Remove(type);
                _serviceRegistry.Add(type, instance);
            }
            else
            {
                throw new NotSupportedException("Something went wrong while resolving instance for type " + type.Name);
            }

            return instance;
        }


        [DebuggerStepThrough]
        private void Autowire(object instance, HashSet<Type> unresolvedDependencies)
        {
            try
            {
                var targetFields = from f in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                   where f.IsDefined(typeof(InjectAttribute), false)
                                   select f;

                foreach (var targetField in targetFields)
                {
                    if (targetField.GetValue(instance) == null)
                    {

                        if (unresolvedDependencies.Contains(targetField.FieldType))
                        {
                            throw new CircularDependencyException(instance.GetType(), targetField.FieldType);
                        }

                        // Only inject if the field is null
                        var fieldValue = Resolve(targetField.FieldType, unresolvedDependencies);
                        targetField.SetValue(instance, fieldValue);
                    }
                }
            }
            catch (Exception e)
            {
                throw new AutowireException("Autowiring of instance " + instance.GetType().Name + " failed!", e);
            }
        }


        [DebuggerStepThrough]
        private object ResolveInstanceFor(Type type, HashSet<Type> unresolvedDependencies)
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
                instance = CreateInstance(typeForImplementation, unresolvedDependencies);
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
        /// <param name="circularRefSet">Holds all types which depend on this instance</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private object CreateInstance(Type implementationType, HashSet<Type> unresolvedDependencies)
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

                var rawInstance = FormatterServices.GetUninitializedObject(implementationType);

                Autowire(rawInstance, unresolvedDependencies);

                var parameters = AutowireParameters(implementationType, constructor, unresolvedDependencies);
                var obj = constructor.Invoke(rawInstance, parameters);
                return rawInstance;
            }

            throw new NotSupportedException("Can not create an instance for type " + implementationType.Name + " - no viable (public/protected) constructor found.");
        }


        /// <summary>
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="implementationType">Just used for better debug messages</param>
        /// <param name="constructor"></param>
        /// <param name="circularRefSet"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private object[] AutowireParameters(Type implementationType, ConstructorInfo constructor, HashSet<Type> circularRefSet)
        {
            if (constructor == null) throw new ArgumentNullException("constructor");

            var parameterInfos = constructor.GetParameters();
            var parameters = new List<object>();

            foreach (var parameter in parameterInfos)
            {
                try
                {
                    if (circularRefSet.Contains(parameter.ParameterType))
                        throw new CircularDependencyException(implementationType, parameter.ParameterType);

                    var paramInstance = Resolve(parameter.ParameterType, circularRefSet); // Recursive call
                    if (paramInstance != null)
                    {
                        parameters.Add(paramInstance);
                    }
                    else
                    {
                        throw new NotSupportedException("Could not resolve parameter " + parameter.Name + " value was (null)!");
                    } 
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
