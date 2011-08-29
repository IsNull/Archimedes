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
        //CopyIfNewer
    }

    /// <summary>
    /// Represents a single item to install
    /// </summary>
    public class FileInstallTarget
    {
        InstallRanking _ranking = InstallRanking.None;
        FileHandling _handling = FileHandling.CopyIfNotExists;

        public FileInstallTarget() { }

        public string Source {
            get;
            set;
        }

        public string Destionation {
            get;
            set;
        }

        public FileHandling Handling {
            get { return _handling; }
            set { _handling = value; }
        }

        public InstallRanking Ranking {
            get { return _ranking; }
            set { _ranking = value; }
        }

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

                default:
                throw new NotSupportedException(
                    string.Format("The Installer handling for {0} is not Implemented!", _handling));

            }

        }

        public InstallRanking Rank {
            get { return _ranking; }
        }
    }
}
