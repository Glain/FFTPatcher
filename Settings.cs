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
        public enum TeamColorMap
        {
            Selected = 0,
            Always = 1,
            NotAlways = 2
        }

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
        private static CombinedColor _defaultHighlightColor = new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White };

        private static CombinedColor[][] _defaultTeamColors = new CombinedColor[4][] {
            new CombinedColor[3] { 
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(225, 200, 75), ForegroundColor = Color.White }
            }
        };

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

        private CombinedColor _highlightColor;
        public static CombinedColor HighlightColor
        {
            get
            {
                return GetSettings()._highlightColor;
            }
        }

        private CombinedColor[][] _teamColors;
        public static CombinedColor GetTeamColor(int teamIndex, int colorIndex)
        {
            return GetSettings()._teamColors[teamIndex][colorIndex];
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
                instance._highlightColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//HighlightColor"), _defaultHighlightColor);
                instance._teamColors = GetTeamColorsFromNode(settingsXml.SelectSingleNode("//TeamColors"), _defaultTeamColors);
            }

            return instance;
        }

        private static Settings GetDefaultInstance()
        {
            return new Settings() {
                _modifiedColor = _defaultModifiedColor,
                _unreferencedColor = _defaultUnreferencedColor,
                _duplicateColor = _defaultDuplicateColor,
                _highlightColor = _defaultHighlightColor,
                _teamColors = _defaultTeamColors
            };
        }

        private static CombinedColor[][] GetTeamColorsFromNode(XmlNode xmlNode, CombinedColor[][] defaultTeamColors)
        {
            if (xmlNode == null)
                return defaultTeamColors;

            XmlAttribute attrUse = xmlNode.Attributes["Use"];
            if (attrUse != null)
            {
                bool isUsing = true;
                if (bool.TryParse(attrUse.InnerText, out isUsing))
                {
                    if (!isUsing)
                    {
                        CombinedColor[][] unusedTeamColors = new CombinedColor[4][];
                        for (int teamIndex = 0; teamIndex < 4; teamIndex++)
                        {
                            unusedTeamColors[teamIndex] = new CombinedColor[3];
                            for (int colorIndex = 0; colorIndex < 3; colorIndex++)
                                unusedTeamColors[teamIndex][colorIndex] = new CombinedColor
                                {
                                    UseColor = false,
                                    ForegroundColor = defaultTeamColors[teamIndex][colorIndex].ForegroundColor,
                                    BackgroundColor = defaultTeamColors[teamIndex][colorIndex].BackgroundColor
                                };
                        }

                        return unusedTeamColors;
                    }
                }
            }

            CombinedColor[][] resultTeamColors = new CombinedColor[4][];
            for (int teamIndex = 0; teamIndex < 4; teamIndex++)
            {
                resultTeamColors[teamIndex] = new CombinedColor[3];
                for (int colorIndex = 0; colorIndex < 3; colorIndex++)
                    resultTeamColors[teamIndex][colorIndex] = defaultTeamColors[teamIndex][colorIndex];
            }

            XmlNodeList teamNodes = xmlNode.ChildNodes;
            int teamNodeCount = Math.Min(teamNodes.Count, 4);
            for (int teamNodeIndex = 0; teamNodeIndex < teamNodeCount; teamNodeIndex++)
            {
                XmlNode teamNode = teamNodes[teamNodeIndex];
                XmlNodeList colorNodes = teamNode.ChildNodes;
                int colorNodeCount = Math.Min(colorNodes.Count, 3);
                for (int colorNodeIndex = 0; colorNodeIndex < colorNodeCount; colorNodeIndex++)
                {
                    XmlNode colorNode = colorNodes[colorNodeIndex];
                    resultTeamColors[teamNodeIndex][colorNodeIndex] = GetCombinedColorFromNode(colorNode, defaultTeamColors[teamNodeIndex][colorNodeIndex]);
                }
            }

            return resultTeamColors;
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
