using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Parses a property file in the standard spring java format.
    /// </summary>
    public static class PropertiesParser
    {


        public static Dictionary<string, string> Parse(string propertiesFile)
        {
            return Parse(File.ReadAllLines(propertiesFile));
        }


        public static Dictionary<string, string> Parse(string[] propertyLines)
        {
            var properties = new Dictionary<string, string>();
            var keyValueParser = new Regex("(.*)=(.*)");

            foreach (var propertyLine in propertyLines)
            {
                if (!propertyLine.Trim().StartsWith("#"))
                {
                    if (keyValueParser.IsMatch(propertyLine))
                    {
                       var key = keyValueParser.Match(propertyLine).Groups[1].Value;
                       var value = keyValueParser.Match(propertyLine).Groups[2].Value;
                       properties.Add(key, value);
                    }
                    
                }
            }

            return properties;
        }

    }
}
