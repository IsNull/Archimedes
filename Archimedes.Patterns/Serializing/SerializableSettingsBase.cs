using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Archimedes.Patterns.Serializing
{
    /// <summary>
    /// Represents an Settingsfile which can be serialized to an xml file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class SerializableSettingsBase<T>
            where T : SerializableSettingsBase<T>
    {
        public event EventHandler SettingsChanged;

        #region Serialisation Helpers

        protected SerializableSettingsBase() { }
        protected SerializableSettingsBase(string path) { SettingsSerialisationPath = path; }

        [NonSerialized()]
        internal string SettingsSerialisationPath;

        /// <summary>
        /// Opens the settings at the given path when the file exists.
        /// Otherwise, creates a new default settings file and loads this.
        /// </summary>
        /// <param name="syncsettingsFilePath"></param>
        /// <returns></returns>
        public static T OpenSettings(string syncsettingsFilePath) {
            T settings;

            if (File.Exists(syncsettingsFilePath)) {
                settings = SerializableSettingsBase<T>.Load(syncsettingsFilePath);
            } else {
                settings = (T)Activator.CreateInstance<T>();
                settings.SettingsSerialisationPath = syncsettingsFilePath;
                settings.Save();
            }
            return settings;
        }

        //static object Create(Type t, params Type[] genericArguments) {
        //    Type generic = t.MakeGenericType(genericArguments);
        //    return Activator.CreateInstance(generic);
        //}


        public void Save() {
            SerializerHelper.SerializeObjectToFile(this, SettingsSerialisationPath);
            OnSettingsChanged();
        }

        public static T Load(string path) {
            var settings = SerializerHelper.DeserializeObjectFromFile<T>(path);
            settings.SettingsSerialisationPath = path;
            return settings;
        }

        protected virtual void OnSettingsChanged() {
            if (SettingsChanged != null)
                SettingsChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
