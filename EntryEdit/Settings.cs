using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PatcherLib.Utilities;
using PatcherLib.Iso;
using System.IO;
using PatcherLib.Datatypes;
using System.Xml;
using PatcherLib.Helpers;

namespace EntryEdit
{
    internal static class Settings
    {
        private static readonly string _entryDirectory = ConfigurationManager.AppSettings["EntryDirectory"];
        public static string EntryDirectory { get { return _entryDirectory; } }

        private static readonly string _settingsFilename = ConfigurationManager.AppSettings["SettingsFilename"];
        public static string SettingsFilename {  get { return _settingsFilename; } }

        private static SettingsData PSXSettings = null;
        private static SettingsData PSPSettings = null;

        public static SettingsData PSX
        {
            get
            {
                if (PSXSettings == null)
                    LoadSettings();

                return PSXSettings;
            }
        }

        public static SettingsData PSP
        {
            get
            {
                if (PSPSettings == null)
                    LoadSettings();

                return PSPSettings;
            }
        }

        public static SettingsData GetSettings(Context context)
        {
            switch (context)
            {
                case Context.US_PSX: return PSX;
                case Context.US_PSP: return PSP;
                default: return null;
            }
        }

        private static void LoadSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(SettingsFilename);
            XmlNode settingsNode = xmlDoc["Settings"];

            PSXSettings = SettingsData.LoadSettings(settingsNode[ISOHelper.TypeStrings.PSX], Context.US_PSX);
        }
    }
}
