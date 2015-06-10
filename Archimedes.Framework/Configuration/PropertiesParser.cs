using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Parses a property file in the standard spring java format.
    /// </summary>
    public static class PropertiesParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex KeyValueParser = new Regex("(.*?)=(.*)");

        public static Dictionary<string, string> Parse(string propertiesFile)
        {
            return Parse(File.ReadAllLines(propertiesFile));
        }


        public static Dictionary<string, string> Parse(string[] propertyLines)
        {
            var properties = new Dictionary<string, string>();
            

            foreach (var propertyLine in propertyLines)
            {
                var parsed = ParseLine(propertyLine);

                if (parsed != null)
                {
                    if (!properties.ContainsKey(parsed.Key))
                    {
                        properties.Add(parsed.Key, parsed.Value);
                    }
                    else
                    {
                        properties[parsed.Key] = parsed.Value;
                        Log.Warn("Property key '" + parsed.Key + "' is defined multiple times. Value got overriden.");
                    }
                }
            }
            return properties;
        }

        public static PropertyEntry ParseLine(string propertyLine)
        {
            if (!propertyLine.Trim().StartsWith("#"))
            {
                if (KeyValueParser.IsMatch(propertyLine))
                {
                    var key = KeyValueParser.Match(propertyLine).Groups[1].Value;
                    var value = KeyValueParser.Match(propertyLine).Groups[2].Value;

                    return new PropertyEntry(key, value);
                }
            }
            return null;
        }

        public class PropertyEntry
        {
            private readonly string _key;
            private readonly string _value;

            public PropertyEntry(string key, string value)
            {
                _key = key;
                _value = value;
            }

            public string Key
            {
                get { return _key; }
            }

            public string Value
            {
                get { return _value; }
            }
        }

    }
}
