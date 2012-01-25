using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Archimedes.Patterns.Deployment
{
    public enum InstallRanking
    {
        None = 0,
        Critical
    }

    public enum FileHandling
    {
        CopyIfNotExists,
        CopyAlways,
        CopyIfNewer
    }

    /// <summary>
    /// Represents a single item to install
    /// </summary>
    public class FileInstallTarget : Installable
    {
        #region Fields

        InstallRanking _ranking = InstallRanking.None;
        FileHandling _handling = FileHandling.CopyIfNotExists;
        Func<Version> _currentVersionCallback;
        Action<Version> _newVersionInstalledCallback;

        #endregion

        #region Constructors

        public FileInstallTarget() { }

        public FileInstallTarget(Func<Version> getcurrentVersionCallback, Action<Version> setcurrentVersionCallback) { 
            CurrentVersionCallback = getcurrentVersionCallback;
            NewVersionInstalledCallback = setcurrentVersionCallback;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the Source-Path of this file
        /// </summary>
        public string Source {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the version of this item
        /// </summary>
        public Version FileVersion {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Destination path of this file
        /// </summary>
        public string Destionation {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the handling behaviour of this file
        /// </summary>
        public FileHandling Handling {
            get { return _handling; }
            set { _handling = value; }
        }

        /// <summary>
        /// Gets/Sets the ranking of this file
        /// </summary>
        public InstallRanking Ranking {
            get { return _ranking; }
            set { _ranking = value; }
        }

        /// <summary>
        /// Callback to find the current installed version
        /// Can return null to indicate that no version is installed currently.
        /// </summary>
        public Func<Version> CurrentVersionCallback {
            get { return _currentVersionCallback; }
            set { _currentVersionCallback = value; }
        }

        /// <summary>
        /// Callback to save the current fileversion
        /// </summary>
        public Action<Version> NewVersionInstalledCallback {
            get { return _newVersionInstalledCallback; }
            set { _newVersionInstalledCallback = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Installs this File
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public virtual void Install() {
            if (string.IsNullOrEmpty(Source) || string.IsNullOrEmpty(Destionation))
                throw new FileNotFoundException();

            var targetDir = Path.GetDirectoryName(Destionation);

            switch (_handling) {

                case FileHandling.CopyIfNotExists:
                // Install only if the file not exists
                if (!File.Exists(Destionation)) {
                    if (!Directory.Exists(targetDir))
                        Directory.CreateDirectory(targetDir);

                    if (!File.Exists(Source))
                        throw new FileNotFoundException(Source);
                    File.Copy(Source, Destionation);
                }
                break;

                case FileHandling.CopyAlways:

                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                if (!File.Exists(Source))
                    throw new FileNotFoundException(Source);
                File.Copy(Source, Destionation, true);
                break;

                case FileHandling.CopyIfNewer:

                if (CurrentVersionCallback == null)
                    throw new NotSupportedException("CurrentVersionCallback must be set!");

                var current = CurrentVersionCallback();
                if (current == null || this.FileVersion > current) {

                    if (!Directory.Exists(targetDir))
                        Directory.CreateDirectory(targetDir);

                    if (!File.Exists(Source))
                        throw new FileNotFoundException("Sourcefile not found!", Source);
                    File.Copy(Source, Destionation, true);
                    if (NewVersionInstalledCallback != null)
                        NewVersionInstalledCallback(this.FileVersion);
                }

                break;

                default:
                throw new NotSupportedException(
                    string.Format("The Installer handling for {0} is not Implemented!", _handling));

            }
        }

        #endregion

    }
}
