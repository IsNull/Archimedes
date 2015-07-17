using System;
using System.IO;
using System.Text;
using Archimedes.Patterns.CommandLine;
using Archimedes.Patterns.Utils;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private const string PropertiesFileName = "application.properties";

        private readonly Properties _configuration = new Properties();


        public Properties Configuration {
            get { return _configuration; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandLineArgs"></param>
        /// <returns></returns>
        public void LoadConfiguration(params string[] commandLineArgs)
        {
            var properties = LoadConfigurationFromPropertiesFile()
                .Merge(LoadConfigurationFromCommandLine(commandLineArgs));

            InterpretProperties(properties);

            Configuration.Merge(properties);
        }

        #region Private methods

        private void InterpretProperties(Properties properties)
        {
            var allProperties = properties.ToKeyValuePairs();
            foreach (var key in allProperties.Keys)
            {
                if (key.Contains(".$hidden"))
                {
                    var hiddenValue = properties.Get(key);
                    properties.Set(key.Replace(".$hidden", ""), Unhide(hiddenValue));
                }
            }

        }

        private string Unhide(string hidden)
        {
            if (!string.IsNullOrEmpty(hidden))
            {
                byte[] decodedBytes = Convert.FromBase64String(hidden);
                string decodedText = Encoding.UTF8.GetString(decodedBytes);
                return decodedText;
            }
            return null;
        }


        private Properties LoadConfigurationFromCommandLine(string[] commandLineArgs)
        {
            var cmdProps = new Properties();

            if (commandLineArgs != null && commandLineArgs.Length != 0)
            {
                var argsCmd = CommandLineParser.ParseCommandLineArgs(commandLineArgs);
                cmdProps.Merge(argsCmd.ToParameterMap());
            }

            return cmdProps;
        }

        private Properties LoadConfigurationFromPropertiesFile()
        {
            try
            {
                var levelOnePath = AppUtil.ApplicaitonBinaryFolder + @"\" + PropertiesFileName;
                var levelTwoPath = AppUtil.AppDataFolder + @"\" + PropertiesFileName;

                return LoadProperties(levelOnePath).Merge(LoadProperties(levelTwoPath));
            }
            catch (NotSupportedException e)
            {
                // Ignore - we cant load any of the property files
            }
            return new Properties();
        }

        private Properties LoadProperties(string path)
        {
            var properties = new Properties();

            if (File.Exists(path))
            {
                var tmp = PropertiesFileParser.Parse(path);
                properties.Merge(tmp);
            }

            return properties;
        }

        #endregion
        
    }
}
