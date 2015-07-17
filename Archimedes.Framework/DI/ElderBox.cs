using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Archimedes.DI.AOP;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Configuration;
using log4net;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// A very lightweight dependency injection container wich requires virtually no configuration.
    /// Use <see cref="ApplicationContext"/> with its Auto-Scan ability to get started easily.
    /// </summary>
    public class ElderBox
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


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

            UpdateSingletonInstance(typeof(ElderBox), this); // Register the current DI container context
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new instance of the given Type.
        /// Using the provided (optional) arguments as constructor parameters.
        /// If there are Constructor parameters which are not provided, this context is used to auto-wire 
        /// the missing dependencies.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Create<T>(params object[] args)
        {
            return (T)Create(typeof(T), args);
        }

        /// <summary>
        /// Creates a new instance of the given Type.
        /// Using the provided (optional) arguments as constructor parameters.
        /// If there are Constructor parameters which are not provided, this context is used to auto-wire 
        /// the missing dependencies.
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Create(Type t, params object[] args)
        {
            return CreateInstance(t, new HashSet<Type>(), args);
        }


        /// <summary>
        /// Resolve an instance for the given Type. All dependencies will be auto wired (injected).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve an instance for the given Type. All dependencies will be auto wired (injected).
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
        /// Usefull if you have already an instance which could not be created using this DI container.
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

            if (instance == null)
            {
                throw new NotSupportedException("Something went wrong while resolving instance for type " + type.Name);
            }

            return instance;
        }

        /// <summary>
        /// Autowires all fields which have the [Inject] Attribute with the resolved dependency.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="unresolvedDependencies"></param>
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
                    if (targetField.GetValue(instance) == null) // Only autowire if the field is null
                    {

                        if (unresolvedDependencies.Contains(targetField.FieldType))
                        {
                            throw new CircularDependencyException(instance.GetType(), targetField.FieldType);
                        }

                        try
                        {
                            var fieldValue = Resolve(targetField.FieldType, unresolvedDependencies);
                            targetField.SetValue(instance, fieldValue);
                        }
                        catch (Exception e)
                        {
                            if (e is CircularDependencyException)
                            {
                                throw;
                            }
                            throw new AutowireException("Autowiring of Field " + targetField.Name + "("+targetField.FieldType.Name+") has failed!", e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is CircularDependencyException)
                {
                    throw;
                }
                throw new AutowireException("Autowiring of instance " + instance.GetType().Name + " failed!", e);
            }

            AutowireConfiguration(instance, unresolvedDependencies);
        }


        /// <summary>
        /// Autofills all fields of the given instance which have the [Value(...)] atribute.
        /// </summary>
        /// <param name="instance"></param>
        private void AutowireConfiguration(object instance, HashSet<Type> unresolvedDependencies)
        {
            var valueFields = (from f in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                               where f.IsDefined(typeof(ValueAttribute), false)
                               select f).ToList();

            if (valueFields.Any())
            {
                var configurationService = (IConfigurationService)Resolve(typeof(IConfigurationService), unresolvedDependencies);

                var configurator = new ValueConfigurator(configurationService.Configuration);

                foreach (var targetField in valueFields)
                {
                    if (targetField.FieldType.IsValueType ||
                        targetField.GetValue(instance) == null) // Only inject value if field is null
                    {
                        var valueAttrs = targetField.GetCustomAttributes(typeof (ValueAttribute), false);
                        if (valueAttrs.Length > 0)
                        {
                            var valueAttr = (ValueAttribute) valueAttrs[0];
                            configurator.SetValue(targetField, instance, valueAttr.Expression);
                        }
                    }
                }
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
                // We allow only types marked with Component, Service or Controller
                ThrowIfTypeNotComponent(typeForImplementation);

                instance = CreateInstance(typeForImplementation, unresolvedDependencies);

                // Successfully resolved the instance:
                UpdateSingletonInstance(typeForImplementation, instance);
                UpdateSingletonInstance(type, instance);

                // Update the unresolved dependencies
                unresolvedDependencies.Remove(typeForImplementation);
                unresolvedDependencies.Remove(type);
            }
            else
            {
                throw new AutowireException("Can not create instance for type '" + type.Name + "' which was resolved to implementation '" + typeForImplementation.Name + "'!");
            }

            return instance;
        }

        [DebuggerStepThrough]
        private void ThrowIfTypeNotComponent(Type type)
        {
            if (!AOPUitl.IsTypeComponent(type)) throw new AutowireException("The implementation " + type + " is not marked as Component and can therefore not be used." +
                                                                              " Did you forget to add a [Service] or [Controller] annotation?");
        }

        /// <summary>
        /// Creates a new instance from the given implementaiton
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="unresolvedDependencies">Holds all types which depend on this instance</param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private object CreateInstance(Type implementationType, HashSet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            if (implementationType == null) throw new ArgumentNullException("implementationType");
            if (unresolvedDependencies == null) throw new ArgumentNullException("unresolvedDependencies");
            if (providedParameters == null) providedParameters = new object[0];


            // Try to create an instance of the given type.

            var allConstructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            var constructor = (from c in allConstructors
                              where c.GetCustomAttributes(typeof (InjectAttribute), false).Any()
                              select c).FirstOrDefault();

            if (constructor == null)
            {
                // There was no Constructor with the Inject Attribute.
                // Just get the first available one
                constructor = (from c in allConstructors
                    where c.IsPublic || !c.IsPrivate
                               select c).FirstOrDefault() ?? allConstructors.FirstOrDefault();
            }

            if (constructor != null)
            {
                // We have found a constructor

                var rawInstance = FormatterServices.GetUninitializedObject(implementationType);

                Autowire(rawInstance, unresolvedDependencies);

                var parameters = AutowireParameters(implementationType, constructor, unresolvedDependencies, providedParameters);
                var obj = constructor.Invoke(rawInstance, parameters);
                return rawInstance;
            }

            throw new NotSupportedException("Can not create an instance for type " + implementationType.Name + " - no viable (public/protected) constructor in the available " + allConstructors.Count() + " found!");
        }


        /// <summary>
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="implementationType">Just used for better debug messages</param>
        /// <param name="constructor"></param>
        /// <param name="circularRefSet"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private object[] AutowireParameters(Type implementationType, ConstructorInfo constructor, HashSet<Type> circularRefSet, object[] providedParameters)
        {
            if (constructor == null) throw new ArgumentNullException("constructor");

            var parameterInfos = constructor.GetParameters();
            var parameters = new List<object>();

            foreach (var parameter in parameterInfos)
            {
                object paramInstance = FindParamInstance(parameter, providedParameters);
                if (paramInstance == null)
                {
                    paramInstance = ResolveParameterInstance(implementationType, parameter, circularRefSet);
                }
                parameters.Add(paramInstance);
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Returns the instance for a parameter type
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="parameterInfo"></param>
        /// <param name="circularRefSet"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private object ResolveParameterInstance(Type implementationType, ParameterInfo parameterInfo, HashSet<Type> circularRefSet)
        {
            object parameter;
            try
            {
                if (circularRefSet.Contains(parameterInfo.ParameterType))
                {
                    throw new CircularDependencyException(implementationType, parameterInfo.ParameterType);
                }
                var paramInstance = Resolve(parameterInfo.ParameterType, circularRefSet); // Recursive call
                if (paramInstance != null)
                {
                    parameter = paramInstance;
                }
                else
                {
                    throw new NotSupportedException("Could not resolve parameter " + parameterInfo.Name + " of " + implementationType + ", the value was (null)!");
                }
            }
            catch (Exception e)
            {
                if (e is CircularDependencyException)
                {
                    throw;
                }
                throw new AutowireException("Autowiring constructor parameter " + parameterInfo.Name + " (" + parameterInfo.ParameterType.Name + ") of " + implementationType + " has failed!", e);
            }

            return parameter;
        }

        /// <summary>
        /// Find a matching instance for the given parameter
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        private object FindParamInstance(ParameterInfo parameterInfo, object[] providedParameters)
        {
            if (providedParameters.Length == 0) return null;
            return (from p in providedParameters
                    where parameterInfo.ParameterType.IsInstanceOfType(p)
                    select p).FirstOrDefault();
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

        /// <summary>
        /// Register the given instance as implemnetation singleton for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        private void UpdateSingletonInstance(Type type, object instance)
        {
            if (!_serviceRegistry.ContainsKey(type))
            {
                _serviceRegistry.Add(type, instance);
            }
            else
            {
                _serviceRegistry[type] = instance;
            }
        }

        #endregion

        public override string ToString()
        {
            return ContextName + " holding " + _serviceRegistry.Count + " entries!";
        }

        public void RegisterInstance<T>(T serviceInstance)
        {
            UpdateSingletonInstance(typeof(T), serviceInstance);
        }
    }
}
