using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Deployment.Application;
using System.Reflection;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public class AboutViewModel : WorkspaceViewModel
    {

        static string _version;
        static string _name;
        static string _copyright = "(c) 2011 RIEDO Informatics";

        static AboutViewModel() {
            // Collect Assembly informations
            var asm = Assembly.GetEntryAssembly();
            _name = asm.GetName().Name;

            if (ApplicationDeployment.IsNetworkDeployed) {
                // this app was ClickOnce  deployed:
                _version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            } else {
                // readout the main asm version
                _version = asm.GetName().Version.ToString();
            }

            // Try update copyright information
            var attribs = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attribs.Length > 0) {
                var cprightAttr = attribs[0] as AssemblyCopyrightAttribute;
                if (cprightAttr != null)
                    _copyright = cprightAttr.Copyright;
            }
        }

        public AboutViewModel() { }

        #region VM Properties

        public string Version {
            get { return _version; }
            set { _version = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string CopyRight {
            get { return _copyright; }
            set { _copyright = value; }
        }

        #endregion
    }
}
