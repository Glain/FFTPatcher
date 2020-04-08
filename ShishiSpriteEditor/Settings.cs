using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    public class Settings
    {
        public static string PSXFilesFilepath = "PSXFiles.xml";
        public static string PSPFilesFilepath = "PSPFiles.xml";

        private static Settings _instance = null;

        private string _psxFiles = null;
        public string PSXFileString
        {
            get
            {
                if (_psxFiles == null)
                {
                    _psxFiles = File.ReadAllText(PSXFilesFilepath);
                }

                return _psxFiles;
            }
        }

        public static string PSXFiles
        {
            get
            {
                return GetSettings().PSXFileString;
            }
        }

        private string _pspFiles = null;
        public string PSPFileString
        {
            get
            {
                if (_pspFiles == null)
                {
                    _pspFiles = File.ReadAllText(PSPFilesFilepath);
                }

                return _pspFiles;
            }
        }

        public static string PSPFiles
        {
            get
            {
                return GetSettings().PSPFileString;
            }
        }

        public static Settings GetSettings()
        {
            return _instance ?? (_instance = GetInstance());
        }

        private static Settings GetInstance()
        {
            Settings instance = GetDefaultInstance();
            return instance;
        }

        private static Settings GetDefaultInstance()
        {
            return new Settings()
            {
                
            };
        }
    }
}
