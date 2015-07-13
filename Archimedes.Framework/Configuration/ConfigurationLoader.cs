using System;
using System.IO;
using Archimedes.Patterns.Utils;

namespace Archimedes.Framework.Configuration
{
    public class ConfigurationLoader
    {
        public Properties LoadConfiguration(string[] commandLineArgs = null)
        {
            return LoadConfigurationFromPropertiesFile()
                .Merge(LoadConfigurationFromCommandLine(commandLineArgs));
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
            const string propertiesFileName = "application.properties";

            var levelOnePath = AppUtil.ApplicaitonBinaryFolder + @"\" + propertiesFileName;
            var levelTwoPath = AppUtil.AppDataFolder + @"\" + propertiesFileName;

            return LoadProperties(levelOnePath).Merge(LoadProperties(levelTwoPath));
        }

        private Properties LoadProperties(string path)
        {
            var properties = new Properties();

            if (File.Exists(path))
            {
                var tmp = PropertiesParser.Parse(path);
                properties.Merge(tmp);
            }

            return properties;
        }
    }
}
