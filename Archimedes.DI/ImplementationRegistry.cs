using System;
using System.Collections.Generic;
using System.Linq;
using Archimedes.DI.AOP;

namespace Archimedes.DI
{
    /// <summary>
    /// Holds all (named) implementations of a type.
    /// </summary>
    internal class ImplementationRegistry
    {
        private readonly Type _iface; // Just used for better exception texts


        private readonly List<Type> _anonymousImpls = new List<Type>();
        private readonly Dictionary<string, Type> _namedImpls = new Dictionary<string, Type>();


        private Type _primary = null;


        public ImplementationRegistry(Type iface)
        {
            _iface = iface;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
                throw new NotSupportedException("Could not find a implemnetation for type " + _iface.Name);
            }
            else if (_anonymousImpls.Count == 1)
            {
                return _anonymousImpls.First();
            }
            else
            {
                string implsStr = "";
                foreach (var impl in _anonymousImpls)
                {
                    implsStr += impl + " ";
                }
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

            if (AOPUitl.IsPrimaryImplementation(impl))
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








    }
}
