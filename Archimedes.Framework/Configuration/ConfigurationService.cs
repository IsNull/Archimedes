using System;
using System.Collections.Generic;
using System.IO;
using Archimedes.Patterns;
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
        public void LoadConfiguration(string[] commandLineArgs = null)
        {
            var properties = LoadConfigurationFromPropertiesFile()
                .Merge(LoadConfigurationFromCommandLine(commandLineArgs));

            Configuration.Merge(properties);
        }



        #region Private methods

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
