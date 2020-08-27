using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EntryEdit
{
    public class DataHelper
    {
        public DataHelper()
        {
            parameterValueMaps = GetParameterValueMaps();
            commandTemplateMaps = GetCommandTemplateMaps();
        }

        private readonly Dictionary<CommandParameterType, Dictionary<int, string>> parameterValueMaps;
        private readonly Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandTemplateMaps;

        public List<string> GetParameterValueList(int numBytes, CommandParameterType type)
        {
            List<string> result = new List<string>();
            int numChoices = (int)Math.Pow(256, numBytes);
            string hexFormatString = "X" + (numBytes * 2);

            for (int index = 0; index < numChoices; index++)
            {
                string name = index.ToString(hexFormatString);
                if (parameterValueMaps[type].ContainsKey(index))
                {
                    name += " " + parameterValueMaps[type][index];
                }
                result.Add(name);
            }

            return result;
        }

        private CommandParameterType GetParameterType(string typeName)
        {
            switch (typeName.ToLower().Trim())
            {
                case "variable": return CommandParameterType.Variable;
                case "unit": return CommandParameterType.Unit;
                case "item": return CommandParameterType.Item;
                case "scenario": return CommandParameterType.Scenario;
                case "map": return CommandParameterType.Map;
                case "location": return CommandParameterType.Location;
                case "abilityeffect": return CommandParameterType.AbilityEffect;
                case "spritesheet": return CommandParameterType.Spritesheet;
                default: return CommandParameterType.Number;
            }
        }

        private string GetCommandFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return "Data/BattleConditionalCommands.xml";
                case CommandType.WorldConditional: return "Data/WorldConditionalCommands.xml";
                case CommandType.EventCommand: return "Data/EventCommands.xml";
                default: return null;
            }
        }

        private string GetParameterValueListFilepath(CommandParameterType type)
        {
            switch (type)
            {
                case CommandParameterType.Variable: return "Data/VariableNames.xml";
                case CommandParameterType.Unit: return "Data/CharacterNames.xml";
                case CommandParameterType.Item: return "Data/Items.xml";
                case CommandParameterType.Scenario: return "Data/ScenarioNames.xml";
                case CommandParameterType.Map: return "Data/MapTitles.xml";
                case CommandParameterType.Location: return "Data/LocationNames.xml";
                case CommandParameterType.AbilityEffect: return "Data/AbilityEffects.xml";
                case CommandParameterType.Spritesheet: return "Data/Spritesheets.xml";
                default: return null;
            }
        }

        private Dictionary<CommandParameterType, Dictionary<int, string>> GetParameterValueMaps()
        {
            Dictionary<CommandParameterType, Dictionary<int, string>> result = new Dictionary<CommandParameterType, Dictionary<int, string>>();
            result.Add(CommandParameterType.Variable, GetParameterValueMap(CommandParameterType.Variable));
            result.Add(CommandParameterType.Unit, GetParameterValueMap(CommandParameterType.Unit));
            result.Add(CommandParameterType.Item, GetParameterValueMap(CommandParameterType.Item));
            result.Add(CommandParameterType.Scenario, GetParameterValueMap(CommandParameterType.Scenario));
            result.Add(CommandParameterType.Map, GetParameterValueMap(CommandParameterType.Map));
            result.Add(CommandParameterType.Location, GetParameterValueMap(CommandParameterType.Location));
            result.Add(CommandParameterType.AbilityEffect, GetParameterValueMap(CommandParameterType.AbilityEffect));
            result.Add(CommandParameterType.Spritesheet, GetParameterValueMap(CommandParameterType.Spritesheet));
            return result;
        }

        private Dictionary<int, string> GetParameterValueMap(CommandParameterType type)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            if (type != CommandParameterType.Number)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(GetParameterValueListFilepath(type));
                XmlNodeList nodeList = xmlDocument.SelectNodes("//Entry");

                foreach (XmlNode node in nodeList)
                {
                    int nodeValue = GetNodeValue(node);
                    XmlAttribute attrName = node.Attributes["name"];

                    if ((nodeValue >= 0) && (attrName != null))
                    {
                        result.Add(nodeValue, attrName.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        private List<string> GetParameterEntryNames(CommandParameterTemplate template)
        {
            List<string> result = new List<string>();

            Dictionary<int, string> valueMap = parameterValueMaps[template.Type];
            int numEntries = 1 << (template.ByteLength * 8);
            string hexFormatString = "X" + (template.ByteLength * 2);
            for (int index = 0; index < numEntries; index++)
            {
                result.Add(index.ToString(hexFormatString) + (valueMap.ContainsKey(index) ? (" " + valueMap[index]) : ""));
            }

            return result;
        }

        private Dictionary<CommandType, Dictionary<int, CommandTemplate>> GetCommandTemplateMaps()
        {
            Dictionary<CommandType, Dictionary<int, CommandTemplate>> result = new Dictionary<CommandType, Dictionary<int, CommandTemplate>>();
            result.Add(CommandType.BattleConditional, GetCommandTemplateMap(CommandType.BattleConditional));
            result.Add(CommandType.WorldConditional, GetCommandTemplateMap(CommandType.WorldConditional));
            result.Add(CommandType.EventCommand, GetCommandTemplateMap(CommandType.EventCommand));
            return result;
        }

        private Dictionary<int, CommandTemplate> GetCommandTemplateMap(CommandType type)
        {
            Dictionary<int, CommandTemplate> result = new Dictionary<int, CommandTemplate>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(GetCommandFilepath(type));
            XmlNode commandsNode = xmlDocument.SelectSingleNode("//Commands");
            XmlAttribute attrDefaultBytes = commandsNode.Attributes["bytes"];

            int defaultCommandByteLength = 1;
            if (attrDefaultBytes != null)
            {
                int.TryParse(attrDefaultBytes.InnerText, out defaultCommandByteLength);
            }

            XmlNodeList nodeList = xmlDocument.SelectNodes("//Command");
            foreach (XmlNode node in nodeList)
            {
                int nodeValue = GetNodeValue(node);
                XmlAttribute attrName = node.Attributes["name"];
                XmlAttribute attrBytes = node.Attributes["bytes"];

                if (nodeValue >= 0)
                {
                    int byteLength = defaultCommandByteLength;
                    if (attrBytes != null)
                    {
                        int.TryParse(attrBytes.InnerText, out byteLength);
                    }

                    CommandTemplate commandTemplate = new CommandTemplate();
                    commandTemplate.ID = nodeValue;
                    commandTemplate.Name = (attrName != null) ? attrName.InnerText : CommandTemplate.DefaultName;
                    commandTemplate.ByteLength = byteLength;

                    commandTemplate.Parameters = new List<CommandParameterTemplate>();
                    foreach (XmlNode parameterNode in node.SelectNodes("//Parameter"))
                    {
                        XmlAttribute attrParamName = parameterNode.Attributes["name"];
                        XmlAttribute attrParamBytes = parameterNode.Attributes["bytes"];
                        XmlAttribute attrParamType = parameterNode.Attributes["type"];
                        XmlAttribute attrParamMode = parameterNode.Attributes["mode"];

                        int paramByteLength = 1;
                        if (attrParamBytes != null)
                        {
                            int.TryParse(attrParamBytes.InnerText, out paramByteLength);
                        }

                        CommandParameterTemplate parameterTemplate = new CommandParameterTemplate();
                        parameterTemplate.Name = (attrParamName != null) ? attrParamName.InnerText : CommandParameterTemplate.DefaultName;
                        parameterTemplate.ByteLength = paramByteLength;
                        parameterTemplate.Type = (attrParamType != null) ? GetParameterType(attrParamType.InnerText) : CommandParameterType.Number;

                        if (attrParamMode != null)
                        {
                            string strMode = attrParamMode.InnerText.ToLower().Trim();
                            if (strMode == "hex")
                            {
                                parameterTemplate.IsHex = true;
                                parameterTemplate.IsSigned = false;
                            }
                            else if (strMode == "unsigned")
                            {
                                parameterTemplate.IsHex = false;
                                parameterTemplate.IsSigned = false;
                            }
                            else
                            {
                                parameterTemplate.IsHex = false;
                                parameterTemplate.IsSigned = true;
                            }
                        }
                        else
                        {
                            bool isNumber = (parameterTemplate.Type == CommandParameterType.Number);
                            parameterTemplate.IsHex = !isNumber;
                            parameterTemplate.IsSigned = isNumber;
                        }

                        commandTemplate.Parameters.Add(parameterTemplate);
                    }

                    result.Add(commandTemplate.ID, commandTemplate);
                }
            }

            return result;
        }

        private int GetNodeValue(XmlNode node)
        {
            int nodeValue = -1;

            XmlAttribute attrValue = node.Attributes["value"];
            if (attrValue != null)
            {
                if (int.TryParse(attrValue.InnerText, out nodeValue))
                    return nodeValue;
            }

            XmlAttribute attrHex = node.Attributes["hex"];
            if (attrHex != null)
            {
                int.TryParse(attrHex.InnerText, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out nodeValue);
            }

            return nodeValue;
        }
    }
}
