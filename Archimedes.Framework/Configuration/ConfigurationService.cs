using System.Collections.Generic;
using Archimedes.Patterns;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly Properties _properties = new Properties();


        public void SetParameterValue(string parameter, string value)
        {
            _properties.Set(parameter, value);
        }

        public string Get(string parameter)
        {
            return _properties.Get(parameter);
        }

        public Optional<string> GetOptional(string parameter)
        {
            return _properties.GetOptional(parameter);
        }

        public Optional<string> GetOptional(params string[] parameters)
        {
            return _properties.GetOptional(parameters);
        }

        public bool IsParameterEnabled(string parameter)
        {
            return _properties.IsTrue(parameter);
        }

        public void Merge(Dictionary<string, string> properties)
        {
            _properties.Merge(properties);
        }

        public void Merge(Properties properties)
        {
            _properties.Merge(properties);
        }
    }
}
