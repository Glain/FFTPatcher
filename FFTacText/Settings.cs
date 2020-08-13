using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FFTPatcher.TextEditor
{
    public class Settings
    {
        public static string PSXTextFilepath = "PSXText.xml";
        public static string PSPTextFilepath = "PSPText.xml";

        private static Settings _instance = null;

        private XmlNode _psxTextNode = null;
        public XmlNode PSXTextNode
        {
            get
            {
                if (_psxTextNode == null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PSXTextFilepath);
                    _psxTextNode = doc;
                }

                return _psxTextNode;
            }
        }

        public static XmlNode PSXText
        {
            get
            {
                return GetSettings().PSXTextNode;
            }
        }

        private XmlNode _pspTextNode = null;
        public XmlNode PSPTextString
        {
            get
            {
                if (_pspTextNode == null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PSPTextFilepath);
                    _pspTextNode = doc;
                }

                return _pspTextNode;
            }
        }

        public static XmlNode PSPText
        {
            get
            {
                return GetSettings().PSPTextString;
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
