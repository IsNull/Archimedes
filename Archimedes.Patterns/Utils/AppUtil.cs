using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Archimedes.Patterns.Utils
{
    public static class AppUtil
    {
        /// <summary>
        /// Gets the Application Name of the running app.
        /// </summary>
        public static string AppName
        {
            get
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                return versionInfo.ProductName;
            }
        }

        /// <summary>
        /// Gets the Application Vendor Name of the running app.
        /// </summary>
        public static string Vendor
        {
            get
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                return versionInfo.CompanyName;
            }
        }

        

        /// <summary>
        /// Gets the App Data folder of this application
        /// </summary>
        public static string AppDataFolder 
        {
            get
            {
                var appData = string.Format(@"{0}\{1}\{2}",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Vendor,
                    AppName);

                return appData;
            }
        }

        /// <summary>
        /// Gets the applications binary folder, i.e. the folder where the exe resides.
        /// </summary>
        public static string ApplicaitonBinaryFolder
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

    }
}
