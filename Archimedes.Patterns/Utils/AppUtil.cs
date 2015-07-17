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
        /// <exception cref="NotSupportedException">Thrown when the App Name could not be retrived.</exception>
        public static string AppName
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return versionInfo.ProductName;
                }
                throw new NotSupportedException("Entry Assembly not available! This may occur in specail circumstances, such as when running as Unit Test.");
            }
        }

        /// <summary>
        /// Gets the Application Vendor Name of the running app.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the Vendor Name could not be retrived.</exception>
        public static string Vendor
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return versionInfo.CompanyName;
                }
                throw new NotSupportedException("Entry Assembly not available! This may occur in specail circumstances, such as when running as Unit Test.");

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
        /// <exception cref="NotSupportedException">Thrown when the binary folder could not be retrived.</exception>
        public static string ApplicaitonBinaryFolder
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    return Path.GetDirectoryName(assembly.Location);
                }
                throw new NotSupportedException("Entry Assembly not available! This may occur in specail circumstances, such as when running as Unit Test.");
            }
        }

    }
}
