using System.Collections.Generic;
using System.Linq;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds commandline parameters and arguments in a easy accessible model.
    /// 
    /// There are two types of configuration in this commandline-options model:
    /// 
    /// Arguments and Parameters. Arguements are just values, provided in the same order as
    /// they where provided by the user.
    /// Parameters are key-value pairs, where order does not matter.
    /// 
    /// 
    /// </summary>
    public class CommandLineOptions
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private readonly List<string> _arguments = new List<string>();


        /// <summary>
        /// Adds a flag
        /// </summary>
        /// <param name="parameter"></param>
        public void SetParameterEnabled(string parameter)
        {
            SetParameterValue(parameter, true.ToString());
        }

        /// <summary>
        /// Adds a flag with a value (option)
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public void SetParameterValue(string parameter, string value)
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
        /// Returns the value of the given parameter, or null, if the parameter
        /// is not set.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetParameterValue(string parameter)
        {
            if (_parameters.ContainsKey(parameter.ToLower()))
            {
                return _parameters[parameter.ToLower()];
            }
            return null;
        }

        public void AddArgument(string argument)
        {
            _arguments.Add(argument);
        }


        /// <summary>
        /// 
        /// </summary>
        public bool HasArguments
        {
            get { return _arguments.Any(); }
        }

        /// <summary>
        /// Gets all parameters which where provided in the cmd arguments
        /// </summary>
        public IEnumerable<string> Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Returns a key-value map with all parameters.
        /// Arguments are not part of this map as they are a command-line phenomea
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToParameterMap()
        {
            return new Dictionary<string, string>(_parameters);
        } 
    }

}
