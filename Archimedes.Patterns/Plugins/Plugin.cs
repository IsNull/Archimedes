using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Plugins
{
    /// <summary>
    /// Base Class for a Plugin
    /// </summary>
    public abstract class Plugin : IPlugin
    {
        private string name;
        private string group;
        private string dlllocation;
        private string description;
        private string copyright = "(c) RIEDO Informatics 2011";

        protected Guid id = new Guid("{1FF2DE08-2C86-429B-A9FA-FA029706E5F5}");

        #region Properties

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        public string Group {
            get { return group; }
            set { group = value; }
        }

        public string DllLocation {
            get { return dlllocation; }
            set { dlllocation = value; }
        }

        public Guid GUID {
            get { return id; }
        }

        public string CopyRight {
            get { return copyright; }
            set { copyright = value; }
        }

        #endregion

        public abstract void OnLoad(string[] startupEventArgs);
        public abstract void OnUnLoad();



    }
}
