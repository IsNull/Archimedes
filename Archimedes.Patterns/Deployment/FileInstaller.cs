using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Archimedes.Patterns.Deployment
{
    public class FileInstaller
    {
        List<FileInstallTarget> _filesToProcess = new List<FileInstallTarget>();

        public virtual bool Install() {
            try {
                foreach (var file in _filesToProcess) {
                    file.Install();
                }
            }catch(Exception e){
                //ToDO: notify the user
                return false;
            }
            return true;
        }

        public void Add(FileInstallTarget t) {
            _filesToProcess.Add(t);
        }

        public void Remove(FileInstallTarget t) {
            _filesToProcess.Remove(t);
        }


    }

}
