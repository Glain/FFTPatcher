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
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(45, 65, 245), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(75, 125, 245), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(125, 175, 245), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(205, 35, 30), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(245, 95, 90), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(245, 135, 130), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(30, 135, 30), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(70, 155, 70), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(110, 175, 110), ForegroundColor = Color.White }
            },
            new CombinedColor[3] {
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(30, 135, 135), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(70, 165, 165), ForegroundColor = Color.White },
                new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(110, 195, 195), ForegroundColor = Color.White }
            }
        };

        private static string[] _defaultElementNames = new string[8] {
            "Fire", "Lightning", "Ice", "Wind", "Earth", "Water", "Holy", "Dark"
        };

        private static CombinedColor[] _defaultElementColors = new CombinedColor[8] {
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(255, 0, 0), ForegroundColor = Color.White },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(128, 0, 128), ForegroundColor = Color.White },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(224, 255, 255), ForegroundColor = Color.Black },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(255, 255, 0), ForegroundColor = Color.Black },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(0, 128, 0), ForegroundColor = Color.White },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(173, 216, 230), ForegroundColor = Color.Black },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(255, 255, 255), ForegroundColor = Color.Black },
            new CombinedColor() { UseColor = true, BackgroundColor = Color.FromArgb(0, 0, 0), ForegroundColor = Color.White },
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

        private string[] _elementNames;
        public static string[] ElementNames
        {
            get
            {
                return GetSettings()._elementNames;
            }
        }

        private CombinedColor[] _elementColors;
        public static CombinedColor[] ElementColors
        {
            get
            {
                return GetSettings()._elementColors;
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

        private static void LoadSettingsXml()
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

            if (SettingsXml != null)
            {
                instance._modifiedColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//ModifiedColor"), _defaultModifiedColor);
                instance._unreferencedColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//UnreferencedColor"), _defaultUnreferencedColor);
                instance._duplicateColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//DuplicateColor"), _defaultDuplicateColor);
                instance._highlightColor = GetCombinedColorFromNode(settingsXml.SelectSingleNode("//HighlightColor"), _defaultHighlightColor);
                instance._teamColors = GetTeamColorsFromNode(settingsXml.SelectSingleNode("//TeamColors"), _defaultTeamColors);

                XmlNode elementsNode = settingsXml.SelectSingleNode("//Elements");
                instance._elementNames = GetElementNamesFromNode(elementsNode, _defaultElementNames);
                instance._elementColors = GetElementColorsFromNode(elementsNode, _defaultElementColors);
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
                _teamColors = _defaultTeamColors,
                _elementNames = _defaultElementNames,
                _elementColors = _defaultElementColors
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

        private static CombinedColor[] GetElementColorsFromNode(XmlNode xmlNode, CombinedColor[] defaultElementColors)
        {
            if (xmlNode == null)
                return defaultElementColors;

            CombinedColor[] resultElementColors = new CombinedColor[defaultElementColors.Length];

            XmlNodeList innerNodes = xmlNode.ChildNodes;
            int innerNodeCount = innerNodes.Count;
            for (int index = 0; index < defaultElementColors.Length; index++)
            {
                XmlNode innerNode = (index < innerNodeCount) ? xmlNode.ChildNodes[index] : null;
                resultElementColors[index] = GetCombinedColorFromNode(innerNode, defaultElementColors[index]);
            }

            return resultElementColors;
        }

        private static string[] GetElementNamesFromNode(XmlNode node, string[] defaultElementNames)
        {
            if (node == null)
                return defaultElementNames;

            string[] nodeNames = GetNamesFromNode(node);
            string[] resultNames = new string[defaultElementNames.Length];

            Array.Copy(nodeNames, resultNames, nodeNames.Length);
            if (nodeNames.Length < resultNames.Length)
            {
                Array.Copy(defaultElementNames, nodeNames.Length, resultNames, nodeNames.Length, resultNames.Length - nodeNames.Length); 
            }

            return resultNames;
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

        private static string[] GetNamesFromNode(XmlNode node)
        {
            if (node == null)
                return null;
            else
            {
                XmlNodeList innerNodes = node.ChildNodes;
                int innerNodeCount = innerNodes.Count;
                string[] names = new string[innerNodeCount];

                for (int index = 0; index < innerNodeCount; index++)
                {
                    XmlNode innerNode = innerNodes[index];
                    if (innerNode != null)
                    {
                        names[index] = GetValueFromAttribute<string>(innerNodes[index].Attributes["Name"]);
                    }
                }

                return names;
            }
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
