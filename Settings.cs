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
            public Color BackgroundColor { get; set; }
            public Color ForegroundColor { get; set; }
        }

        private static string _filename = "Settings.xml";
        private static Settings _instance = null;
        private static CombinedColor _defaultColor = new CombinedColor() { BackgroundColor = Color.FromArgb(175, 175, 255), ForegroundColor = Color.White };

        private CombinedColor _modifiedColor;
        public static CombinedColor ModifiedColor 
        {
            get
            {
                return GetSettings()._modifiedColor;
            }
        }

        public static Settings GetSettings()
        {
            return _instance ?? (_instance = GetInstance());
        }

        private static Settings GetInstance()
        {
            Settings instance = GetDefaultInstance();

            if (File.Exists(_filename))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_filename);

                bool useModifiedColor = true;
                XmlNode modifiedColorNode = xmlDoc.SelectSingleNode("//ModifiedColor");
                if (modifiedColorNode != null)
                {
                    XmlNode useModifiedColorNode = modifiedColorNode["UseModifiedColor"];
                    if (useModifiedColorNode != null)
                    {
                        useModifiedColor = GetValueFromAttribute<bool>(useModifiedColorNode.Attributes["Value"]);
                    }

                    if (useModifiedColor)
                    {
                        XmlNode backgroundColorNode = modifiedColorNode["BackgroundColor"];
                        XmlNode foregroundColorNode = modifiedColorNode["ForegroundColor"];
                        instance._modifiedColor = new CombinedColor() { BackgroundColor = GetColorFromNode(backgroundColorNode), ForegroundColor = GetColorFromNode(foregroundColorNode) };
                    }
                    else
                    {
                        instance._modifiedColor = new CombinedColor() { BackgroundColor = SystemColors.Window, ForegroundColor = SystemColors.WindowText };
                    }
                }
            }

            return instance;
        }

        private static Settings GetDefaultInstance()
        {
            return new Settings() {
                _modifiedColor = _defaultColor 
            };
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
