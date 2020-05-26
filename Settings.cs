using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace FFTPatcher
{
    public class Settings
    {
        public class CombinedColor
        {
            public bool UseColor { get; set; }
            public Color BackgroundColor { get; set; }
            public Color ForegroundColor { get; set; }
        }

        private static string _settingsFilename = "Settings.xml";

        private static XmlDocument settingsXml = null;
        private static XmlDocument SettingsXml
        {
            get
            {
                if (settingsXml == null)
                    settingsXml = GetSettingsXml(_settingsFilename);

                return settingsXml;
            }
        }

        private static Settings _instance = null;

        private static CombinedColor _defaultModifiedColor = new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(175, 175, 255), ForegroundColor = Color.White };
        private static CombinedColor _defaultUnreferencedColor = new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(125, 205, 125), ForegroundColor = Color.White };
        private static CombinedColor _defaultDuplicateColor = new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(205, 125, 125), ForegroundColor = Color.White };

        private CombinedColor _modifiedColor;
        public static CombinedColor ModifiedColor 
        {
            get
            {
                return GetSettings()._modifiedColor;
            }
        }

        private CombinedColor _unreferencedColor;
        public static CombinedColor UnreferencedColor
        {
            get
            {
                return GetSettings()._unreferencedColor;
            }
        }

        private CombinedColor _duplicateColor;
        public static CombinedColor DuplicateColor
        {
            get
            {
                return GetSettings()._duplicateColor;
            }
        }

        private static XmlDocument GetSettingsXml(string filename = null)
        {
            filename = filename ?? _settingsFilename;

            if (File.Exists(filename))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                return xmlDoc;
            }
            else
            {
                return null;
            }
        }

        public static void LoadSettingsXml()
        {
            settingsXml = GetSettingsXml();
        }

        private static Settings GetSettings()
        {
            return _instance ?? (_instance = GetInstance());
        }

        private static Settings GetInstance()
        {
            Settings instance = GetDefaultInstance();

            if (settingsXml != null)
            {
                instance._modifiedColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//ModifiedColor"), _defaultModifiedColor);
                instance._unreferencedColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//UnreferencedColor"), _defaultUnreferencedColor);
                instance._duplicateColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//DuplicateColor"), _defaultDuplicateColor);
            }

            return instance;
        }

        private static Settings GetDefaultInstance()
        {
            return new Settings() {
                _modifiedColor = _defaultModifiedColor,
                _unreferencedColor = _defaultUnreferencedColor,
                _duplicateColor = _defaultDuplicateColor
            };
        }

        private static CombinedColor GetCombinedColorFromNode(XmlNode xmlNode, CombinedColor defaultColor)
        {
            bool useColor = true;

            if (xmlNode != null)
            {
                XmlNode useColorNode = xmlNode["UseColor"];
                if (useColorNode != null)
                {
                    useColor = GetValueFromAttribute<bool>(useColorNode.Attributes["Value"]);
                }

                if (useColor)
                {
                    XmlNode backgroundColorNode = xmlNode["BackgroundColor"];
                    XmlNode foregroundColorNode = xmlNode["ForegroundColor"];
                    return new CombinedColor() { UseColor = useColor, BackgroundColor = GetColorFromNode(backgroundColorNode), ForegroundColor = GetColorFromNode(foregroundColorNode) };
                }
                else
                {
                    return new CombinedColor() { UseColor = useColor, BackgroundColor = SystemColors.Window, ForegroundColor = SystemColors.WindowText };
                }
            }

            return defaultColor;
        }

        private static Color GetColorFromNode(XmlNode node)
        {
            int redValue = 0, greenValue = 0, blueValue = 0;

            if (node != null)
            {
                redValue = GetValueFromAttribute<int>(node.Attributes["Red"]);
                greenValue = GetValueFromAttribute<int>(node.Attributes["Green"]);
                blueValue = GetValueFromAttribute<int>(node.Attributes["Blue"]);
            }

            return Color.FromArgb(redValue, greenValue, blueValue);
        }

        private static T GetValueFromAttribute<T>(XmlAttribute attr)
        {
            T value = default(T);

            try
            {
                value = (T)Convert.ChangeType(attr.InnerText ?? "", typeof(T));
            }
            catch (Exception) { }

            return value;
        }
    }
}
