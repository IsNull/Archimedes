using System;
using System.Collections.Generic;
using Archimedes.Patterns.Serializing;

namespace Archimedes.Patterns.Deployment
{

    /// <summary>
    /// Represents a Version Storage
    /// </summary>
    [Serializable]
    public class XMLVersionStore : SerializableSettingsBase<XMLVersionStore>, IVersionStore
    {
        /// <summary>
        /// This Field is for serialisation only, dont use!
        /// </summary>
        public SerializableDictionary<string, string> InstalledVersions = new SerializableDictionary<string, string>();

        public XMLVersionStore() { }
        public XMLVersionStore(string file) : base(file) { }

        /// <summary>
        /// Returns the current version for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Version GetVersionFor(string id){
            if (InstalledVersions.ContainsKey(id)) {
                return new Version(InstalledVersions[id]);
            } else
                return null;
        }

        /// <summary>
        /// Sets the current version for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        public void SetVersionFor(string id, Version version) {
            if (InstalledVersions.ContainsKey(id)) {
                InstalledVersions[id] = version.ToString();
            } else {
                InstalledVersions.Add(id, version.ToString());
            }
            this.Save();
        }
    }
}
