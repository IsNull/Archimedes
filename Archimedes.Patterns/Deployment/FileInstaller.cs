using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Archimedes.Patterns.Deployment
{

    public class FileInstaller
    {
        List<Installable> _filesToProcess = new List<Installable>();

        /// <summary>
        /// Install all items
        /// </summary>
        /// <returns></returns>
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

        public void Add(Installable t) {
            _filesToProcess.Add(t);
        }

        public void Remove(Installable t) {
            _filesToProcess.Remove(t);
        }


    }

}
