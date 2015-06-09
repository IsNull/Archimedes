using System;
using System.IO;

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
            var cmdProps = new Properties();

            var path = AppDomain.CurrentDomain.BaseDirectory + @"\application.properties";
            if (File.Exists(path))
            {
                var properties = PropertiesParser.Parse(path);
                cmdProps.Merge(properties);
            }
            return cmdProps;
        }
    }
}
