using System;
using System.Collections.Generic;
using System.Linq;
using Archimedes.Patterns;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Represents properties as a simple key-value store
    /// </summary>
    public class Properties
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();


        /// <summary>
        /// Creates an empty properties object.
        /// </summary>
        public Properties()
        {
            
        }

        /// <summary>
        /// Adds a flag with a value (option)
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public void Set(string parameter, string value)
        {
            var param = parameter.ToLower();
            if (_parameters.ContainsKey(param))
            {
                _parameters[parameter] = value;
            }
            else
            {
                _parameters.Add(param, value);

            }
        }

        /// <summary>
        /// Returns the value of the given parameter.
        /// If the parameter is not present, throws a <see cref="UnknownParameterException"/> exception.
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string Get(string parameter)
        {
            if (_parameters.ContainsKey(parameter.ToLower()))
            {
                return _parameters[parameter.ToLower()];
            }
            throw new UnknownParameterException(parameter);
        }

        /// <summary>
        /// Returns the value of the given parameter, or null, if the parameter
        /// is not defined.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Optional<string> GetOptional(string parameter)
        {
            if (_parameters.ContainsKey(parameter.ToLower()))
            {
                return Optional<string>.OfNullable(_parameters[parameter.ToLower()]);
            }
            return Optional<string>.Empty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Optional<string> GetOptional(params string[] parameters)
        {
            foreach (string param in parameters)
            {
                var opt = GetOptional(param);
                if (opt.IsPresent) return opt; // return the first present parameter value
            }
            return Optional<string>.Empty();
        }


        /// <summary>
        /// A short hand to check if a parameter is set to "true"
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool IsTrue(string parameter)
        {
            var val = Get(parameter);

            return (val != null && true.ToString().ToLower().Equals(val.ToLower()));
        }


        /// <summary>
        /// Merges the given parameter set into this configuration
        /// </summary>
        /// <param name="parameters"></param>
        public Properties Merge(Dictionary<string, string> parameters)
        {
            foreach (var kv in parameters)
            {
                Set(kv.Key, kv.Value);
            }
            return this;
        }

        public Properties Merge(Properties other)
        {
            return Merge(other._parameters);
        }

        public Dictionary<string, string> ToKeyValuePairs()
        {
            return new Dictionary<string, string>(_parameters);
        }

        public override string ToString()
        {
            return _parameters.Aggregate("", (current, kv) =>
                current + (kv.Key + "=" + kv.Value + Environment.NewLine)
                );
        }
    }
}
