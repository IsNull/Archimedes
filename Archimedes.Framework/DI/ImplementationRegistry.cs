using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Archimedes.DI.AOP;
using Archimedes.Framework.AOP;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Holds all (named) implementation types of an interface.
    /// </summary>
    internal class ImplementationRegistry
    {
        #region Fields

        private readonly Type _iface; // Just used for better exception texts
        private readonly List<Type> _anonymousImpls = new List<Type>();
        private readonly Dictionary<string, Type> _namedImpls = new Dictionary<string, Type>();


        private Type _primary = null;

        #endregion

        #region Constructor

        public ImplementationRegistry(Type iface)
        {
            _iface = iface;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Try to get the implementation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Type TryGetImplementation(string name = null)
        {
            // First priority is a named implementation
            if (name != null)
            {
                return FindImplementationByName(name);
            }

            // Second priority is a primary service
            if (_primary != null)
            {
                return _primary;
            }

            // Last priority are anonymous implementations
            if (_anonymousImpls.Count == 0)
            {
                if (_namedImpls.Count == 1)
                {
                    return _namedImpls.First().Value;
                }
                throw new NotSupportedException("Could not find an implemnetation for type " + _iface.Name);
            }
            else if (_anonymousImpls.Count == 1)
            {
                return _anonymousImpls.First();
            }
            else
            {
                string implsStr = string.Join(" ", _anonymousImpls);
                throw new AmbiguousMappingException("There is more than one ("+_anonymousImpls.Count+") implementation available for the same type "+_iface+": " + implsStr);
            }
        }

        /// <summary>
        /// Register a new implementation
        /// </summary>
        /// <param name="impl"></param>
        public void Register(Type impl)
        {
            var attr = AOPUitl.GetComponentAttribute(impl);
            string name = attr.Name;

            if (name != null)
            {
                if (!_namedImpls.ContainsKey(name))
                {
                    _namedImpls.Add(name, impl);
                    _anonymousImpls.Add(impl);
                }
                else
                {
                    var offendingImpl = _namedImpls[name];
                    throw new AmbiguousMappingException("A implementation of "+_iface.Name+" has ambiguous name " + name + " which already exists! Implementation " + offendingImpl.Name + " collides with " + impl.Name + "!");
                }
            }
            else
            {
                _anonymousImpls.Add(impl);
            }

            RegisterPrimary(impl);
        }

        #endregion

        #region Private methods

        private void RegisterPrimary(Type impl)
        {
            var primaryAttr = AOPUitl.GetPrimaryAttribute(impl);

            if (primaryAttr != null)
            {
                // The Implementation has the Primary Attribute
                if (IsPrimaryForThisIface(primaryAttr))
                {
                    if (_primary == null)
                    {
                        _primary = impl;
                    }
                    else
                    {
                        throw new AmbiguousMappingException("The [Primary] annotation is used on more than one available implementations: " + _primary.Name + " and " + impl);
                    }
                }
            }
        }

        /// <summary>
        /// The primary attribute allows to specify specific interface types
        /// for which it overrules.
        /// </summary>
        /// <param name="primaryAttribute"></param>
        /// <returns></returns>
        private bool IsPrimaryForThisIface(PrimaryAttribute primaryAttribute)
        {
            if (primaryAttribute.PrimaryForTypes.Length == 0) return true;

            return primaryAttribute.PrimaryForTypes.Any(primaryApplies => _iface == primaryApplies);
        }


        /// <summary>
        /// Try to find an implementation with the given name.
        /// Will throw an exception if no implementation was found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Type FindImplementationByName(string name)
        {
            if (_namedImpls.ContainsKey(name))
            {
                return _namedImpls[name];
            }
            else
            {
                throw new NotSupportedException("Could not find a named implemnetation '" + name + "' of type " + _iface.Name);
            }
        }

        #endregion


        public override string ToString()
        {
            if (_primary != null) return _primary.Name;

            string allImpl = "";
            foreach (var impl in _anonymousImpls)
            {
                allImpl += impl.Name + " | ";
            }
            return allImpl;
        }
    }
}
