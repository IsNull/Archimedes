using System;
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
    public static class PropertiesFileParser
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
                    if (!properties.ContainsKey(parsed.Item1))
                    {
                        properties.Add(parsed.Item1, parsed.Item2);
                    }
                    else
                    {
                        properties[parsed.Item1] = parsed.Item2;
                        Log.Warn("Property key '" + parsed.Item1 + "' is defined multiple times. Value got overriden.");
                    }
                }
            }
            return properties;
        }

        public static Tuple<string,string> ParseLine(string propertyLine)
        {
            if (!propertyLine.Trim().StartsWith("#"))
            {
                if (KeyValueParser.IsMatch(propertyLine))
                {
                    var key = KeyValueParser.Match(propertyLine).Groups[1].Value;
                    var value = KeyValueParser.Match(propertyLine).Groups[2].Value;

                    return new Tuple<string, string>(key, value);
                }
            }
            return null;
        }


    }
}
