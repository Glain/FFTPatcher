using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    public class Settings
    {
        public static string PSXImagesFilepath = "PSXImages.xml";
        public static string PSPImagesFilepath = "PSPImages.xml";

        private static Settings _instance = null;

        private string _psxImages = null;
        public string PSXImagesString
        {
            get
            {
                if (_psxImages == null)
                {
                    _psxImages = File.ReadAllText(PSXImagesFilepath);
                }

                return _psxImages;
            }
        }

        public static string PSXImages
        {
            get
            {
                return GetSettings().PSXImagesString;
            }
        }

        private string _pspImages = null;
        public string PSPImagesString
        {
            get
            {
                if (_pspImages == null)
                {
                    _pspImages = File.ReadAllText(PSPImagesFilepath);
                }

                return _pspImages;
            }
        }

        public static string PSPImages
        {
            get
            {
                return GetSettings().PSPImagesString;
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
