using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using System.IO;

namespace EntryEdit
{
    public class DataHelper
    {
        public DataHelper()
        {
            parameterValueMaps = GetParameterValueMaps();
            commandTemplateMaps = GetCommandTemplateMaps();
        }

        public List<List<List<Command>>> LoadBattleConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.BattleConditional);
        }

        public List<List<List<Command>>> LoadWorldConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.WorldConditional);
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

        private string GetDefaultDataFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return "Data/BattleConditionals.bin";
                case CommandType.WorldConditional: return "Data/WorldConditionals.bin";
                case CommandType.EventCommand: return "Data/Events.bin";
                default: return null;
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
                    foreach (XmlNode parameterNode in node.SelectNodes("Parameter"))
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

        public byte[] ConditionalSetsToByteArray(CommandType type, List<List<List<Command>>> conditionalSets)
        {
            int numSets = conditionalSets.Count;
            int numBlocks = 0;
            foreach (List<List<Command>> blocks in conditionalSets)
                numBlocks += blocks.Count;
            
            List<UInt16> setReferences = new List<UInt16>(numSets);
            List<UInt16> blockReferences = new List<UInt16>();
            List<byte> commandBytes = new List<byte>();

            UInt16 setReference = (UInt16)(numSets * 2);
            UInt16 blockReference = (UInt16)((setReference + numBlocks) * 2);
            foreach (List<List<Command>> blocks in conditionalSets)
            {
                setReferences.Add(setReference);
                foreach (List<Command> commands in blocks)
                {
                    blockReferences.Add(blockReference);
                    byte[] currentCommandBytes = CommandsToByteArray(commands);
                    commandBytes.AddRange(currentCommandBytes);
                    blockReference += (UInt16)(currentCommandBytes.Length);
                    setReference += 2;
                }
                blockReferences.Add(0);
                setReference += 2;
            }

            byte[] setBytes = setReferences.ToBytesLE();
            byte[] blockBytes = blockReferences.ToBytesLE();

            List<byte> bytes = new List<byte>(setBytes.Length + blockBytes.Length + commandBytes.Count);
            bytes.AddRange(setBytes);
            bytes.AddRange(blockBytes);
            bytes.AddRange(commandBytes);
            return bytes.ToArray();
        }

        private List<List<List<Command>>> LoadConditionalSetDefaults(CommandType type)
        {
            return LoadConditionalSetsFromFile(type, GetDefaultDataFilepath(type));
        }

        private List<List<List<Command>>> LoadConditionalSetsFromFile(CommandType type, string filepath)
        {
            return LoadConditionalSetsFromByteArray(type, File.ReadAllBytes(filepath));
        }

        private List<List<List<Command>>> LoadConditionalSetsFromByteArray(CommandType type, IList<byte> bytes)
        {
            int setByteOffset = bytes.ToUInt16LE();
            int numSets = setByteOffset / 2;
            List<List<List<Command>>> result = new List<List<List<Command>>>(numSets);

            UInt16[] setOffsets = bytes.SubLength(0, setByteOffset).ToUInt16ArrayLE();
            int blockByteOffset = bytes.SubLength(setOffsets[0], 2).ToUInt16LE();

            for (int setIndex = 0; setIndex < numSets; setIndex++)
            {
                List<List<Command>> setCommandList = new List<List<Command>>();

                int setStartIndex = setOffsets[setIndex];
                int setEndIndex = ((setIndex < (numSets - 1)) ? setOffsets[setIndex + 1] : blockByteOffset);

                UInt16 prevBlockIndex = 0;
                for (int setByteIndex = setStartIndex; setByteIndex < setEndIndex; setByteIndex += 2)
                {
                    UInt16 blockIndex = bytes.SubLength(setByteIndex, 2).ToUInt16LE();
                    if (prevBlockIndex != 0)
                    {
                        int startIndex = prevBlockIndex;
                        
                        int endIndex = blockIndex;
                        int setIndexAddend = 1;
                        while (endIndex == 0)
                        {
                            int checkSetIndex = setIndex + setIndexAddend;
                            endIndex = (checkSetIndex < numSets) ? bytes.SubLength(setOffsets[checkSetIndex], 2).ToUInt16LE() : bytes.Count;
                            setIndexAddend++;
                        }
                        
                        setCommandList.Add(CommandsFromByteArray(type, bytes.SubLength(startIndex, endIndex - startIndex)));
                    }
                    
                    prevBlockIndex = blockIndex;
                }

                result.Add(setCommandList);
            }

            return result;
        }

        private byte[] CommandsToByteArray(IEnumerable<Command> commands)
        {
            List<byte> result = new List<byte>();

            foreach (Command command in commands)
            {
                result.AddRange(CommandToByteArray(command));
            }

            return result.ToArray();
        }

        private byte[] CommandToByteArray(Command command)
        {
            int commandByteLength = command.Template.ByteLength;
            int byteLength = commandByteLength;
            foreach (CommandParameter parameter in command.Parameters)
            {
                byteLength += parameter.Template.ByteLength;
            }

            List<byte> result = new List<byte>(byteLength);
            result.AddRange(command.Template.ID.ToBytesLE(commandByteLength));

            foreach (CommandParameter parameter in command.Parameters)
            {
                result.AddRange(parameter.Value.ToBytesLE(parameter.Template.ByteLength));
            }

            return result.ToArray();
        }

        private List<Command> CommandsFromByteArray(CommandType type, IList<byte> bytes)
        {
            List<Command> result = new List<Command>();
            int startIndex = 0;

            while (startIndex < bytes.Count)
            {
                Command command = CommandFromByteArray(type, bytes.SubLength(startIndex, bytes.Count - startIndex));
                if (command == null)
                    break;

                startIndex += command.GetTotalByteLength();
                result.Add(command);
            }

            return result;
        }

        private Command CommandFromByteArray(CommandType type, IList<byte> bytes)
        {
            Dictionary<int, CommandTemplate> templateMap = commandTemplateMaps[type];
            int value = 0;
            int shiftAmount = 0;

            Command result = new Command();
            CommandTemplate template = null;

            int index = 0;
            for (; index < 4; index++)
            {
                value |= (bytes[index] << shiftAmount);
                shiftAmount += 8;

                if (templateMap.TryGetValue(value, out template))
                {
                    result.Template = template;
                    break;
                }
            }

            if (result.Template == null)
                return null;

            index = result.Template.ByteLength;
            result.Parameters = new List<CommandParameter>();
            foreach (CommandParameterTemplate parameterTemplate in template.Parameters)
            {
                CommandParameter parameter = new CommandParameter();
                int byteLength = parameterTemplate.ByteLength;

                parameter.Template = parameterTemplate;
                parameter.Value = bytes.SubLength(index, byteLength).ToIntLE();
                index += byteLength;
                result.Parameters.Add(parameter);
            }

            return result;
        }
    }
}
