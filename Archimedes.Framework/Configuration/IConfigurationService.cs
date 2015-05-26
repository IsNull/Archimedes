using System.Collections.Generic;
using Archimedes.Patterns;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public interface IConfigurationService
    {

        /// <summary>
        /// Adds a flag with a value (option)
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        void SetParameterValue(string parameter, string value);


        /// <summary>
        /// Returns the value of the given parameter.
        /// If the parameter is not present, throws a <see cref="UnknownParameterException"/> exception.
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        string Get(string parameter);

        /// <summary>
        /// Returns the value of the given optionally parameter.
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Optional<string> GetOptional(string parameter);


        /// <summary>
        /// Returns the value of the first parameter which is present.
        /// If no parameter is present, Optional.Empty() is returned.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Optional<string> GetOptional(params string[] parameters);

        /// <summary>
        /// A short hand to check if a parameter is set to "true"
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool IsParameterEnabled(string parameter);



        /// <summary>
        /// Merges the given properties set into this configuration
        /// </summary>
        /// <param name="properties"></param>
        void Merge(Dictionary<string, string> properties);

        /// <summary>
        /// Merges the given properties set into this configuration
        /// </summary>
        /// <param name="properties"></param>
        void Merge(Properties properties);
    }
}
