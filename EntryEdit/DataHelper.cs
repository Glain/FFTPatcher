using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using System.IO;

namespace EntryEdit
{
    public class DataHelper
    {
        private readonly Dictionary<CommandParameterType, Dictionary<int, string>> parameterValueMaps;
        private readonly Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandTemplateMaps;
        private readonly Dictionary<CommandType, Dictionary<int, string>> entryNameMaps;

        public DataHelper()
        {
            parameterValueMaps = GetParameterValueMaps();
            commandTemplateMaps = GetCommandTemplateMaps();
            entryNameMaps = GetEntryNameMaps();
        }

        public List<ConditionalSet> LoadBattleConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.BattleConditional);
        }

        public List<ConditionalSet> LoadWorldConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.WorldConditional);
        }

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
                case CommandType.BattleConditional: return "EntryData/BattleConditionals.bin";
                case CommandType.WorldConditional: return "EntryData/WorldConditionals.bin";
                case CommandType.EventCommand: return "EntryData/Events.bin";
                default: return null;
            }
        }
        
        private string GetCommandFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return "EntryData/BattleConditionalCommands.xml";
                case CommandType.WorldConditional: return "EntryData/WorldConditionalCommands.xml";
                case CommandType.EventCommand: return "EntryData/EventCommands.xml";
                default: return null;
            }
        }

        private string GetEntryNameFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return "EntryData/ConditionalSets.xml";
                case CommandType.WorldConditional: return "EntryData/LocationNames.xml";
                case CommandType.EventCommand: return "EntryData/ScenarioNames.xml";
                default: return null;
            }
        }

        private string GetParameterValueListFilepath(CommandParameterType type)
        {
            switch (type)
            {
                case CommandParameterType.Variable: return "EntryData/VariableNames.xml";
                case CommandParameterType.Unit: return "EntryData/CharacterNames.xml";
                case CommandParameterType.Item: return "EntryData/Items.xml";
                case CommandParameterType.Scenario: return "EntryData/ScenarioNames.xml";
                case CommandParameterType.Map: return "EntryData/MapTitles.xml";
                case CommandParameterType.Location: return "EntryData/LocationNames.xml";
                case CommandParameterType.AbilityEffect: return "EntryData/AbilityEffects.xml";
                case CommandParameterType.Spritesheet: return "EntryData/Spritesheets.xml";
                default: return null;
            }
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

        private Dictionary<CommandParameterType, Dictionary<int, string>> GetParameterValueMaps()
        {
            Dictionary<CommandParameterType, Dictionary<int, string>> result = new Dictionary<CommandParameterType, Dictionary<int, string>>();
            result.Add(CommandParameterType.Variable, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Variable)));
            result.Add(CommandParameterType.Unit, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Unit)));
            result.Add(CommandParameterType.Item, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Item)));
            result.Add(CommandParameterType.Scenario, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Scenario)));
            result.Add(CommandParameterType.Map, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Map)));
            result.Add(CommandParameterType.Location, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Location)));
            result.Add(CommandParameterType.AbilityEffect, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.AbilityEffect)));
            result.Add(CommandParameterType.Spritesheet, GetXMLNameMap(GetParameterValueListFilepath(CommandParameterType.Spritesheet)));
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

        private Dictionary<CommandType, Dictionary<int, string>> GetEntryNameMaps()
        {
            Dictionary<CommandType, Dictionary<int, string>> result = new Dictionary<CommandType, Dictionary<int, string>>();
            result.Add(CommandType.BattleConditional, GetXMLNameMap(GetEntryNameFilepath(CommandType.BattleConditional)));
            result.Add(CommandType.WorldConditional, GetXMLNameMap(GetEntryNameFilepath(CommandType.WorldConditional)));
            result.Add(CommandType.EventCommand, GetXMLNameMap(GetEntryNameFilepath(CommandType.EventCommand)));
            return result;
        }

        private Dictionary<int, string> GetXMLNameMap(string filepath)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filepath);
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

                    int commandTemplateID = nodeValue;
                    string commandTemplateName = (attrName != null) ? attrName.InnerText : CommandTemplate.DefaultName;
                    int commandTemplateByteLength = byteLength;

                    List<CommandParameterTemplate> commandTemplateParameters = new List<CommandParameterTemplate>();
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

                        string parameterTemplateName = (attrParamName != null) ? attrParamName.InnerText : CommandParameterTemplate.DefaultName;
                        int parameterTemplateByteLength = paramByteLength;
                        CommandParameterType parameterTemplateType = (attrParamType != null) ? GetParameterType(attrParamType.InnerText) : CommandParameterType.Number;

                        bool isHex = false;
                        bool isSigned = true;

                        if (attrParamMode != null)
                        {
                            string strMode = attrParamMode.InnerText.ToLower().Trim();
                            if (strMode == "hex")
                            {
                                isHex = true;
                                isSigned = false;
                            }
                            else if (strMode == "unsigned")
                            {
                                isHex = false;
                                isSigned = false;
                            }
                        }
                        else
                        {
                            bool isNumber = (parameterTemplateType == CommandParameterType.Number);
                            isHex = !isNumber;
                            isSigned = isNumber;
                        }

                        commandTemplateParameters.Add(new CommandParameterTemplate(parameterTemplateName, parameterTemplateByteLength, isHex, isSigned, parameterTemplateType));
                    }

                    result.Add(commandTemplateID, new CommandTemplate(commandTemplateID, commandTemplateName, commandTemplateByteLength, type, commandTemplateParameters));
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

        public EntryData LoadDefaultEntryData()
        {
            return new EntryData(LoadBattleConditionalDefaults(), LoadWorldConditionalDefaults(), LoadDefaultEvents());
        }

        public EntryBytes GetEntryBytesFromData(EntryData entryData)
        {
            return new EntryBytes(ConditionalSetsToByteArray(CommandType.BattleConditional, entryData.BattleConditionals), ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals),
                EventsToByteArray(entryData.Events));
        }

        public byte[] EventsToByteArray(IList<Event> events)
        {
            byte[] resultBytes = new byte[events.Count * 0x2000];

            int copyIndex = 0;
            foreach (Event inputEvent in events)
            {
                byte[] eventBytes = EventToByteArray(inputEvent);
                Array.Copy(eventBytes, 0, resultBytes, copyIndex, eventBytes.Length);
                copyIndex += eventBytes.Length;
            }

            return resultBytes;
        }

        public byte[] EventToByteArray(Event inputEvent)
        {
            byte[] textOffsetBytes = inputEvent.TextOffset.ToBytes();
            byte[] commandBytes = CommandsToByteArray(inputEvent.CommandList);
            byte[] betweenBytes = inputEvent.BetweenSection.ToByteArray();
            byte[] endBytes = inputEvent.EndSection.ToByteArray();

            byte[] resultBytes = new byte[4 + commandBytes.Length + betweenBytes.Length + endBytes.Length];
            Array.Copy(textOffsetBytes, resultBytes, 4);
            Array.Copy(commandBytes, 0, resultBytes, 4, commandBytes.Length);
            Array.Copy(betweenBytes, 0, resultBytes, commandBytes.Length + 4, betweenBytes.Length);
            Array.Copy(endBytes, 0, resultBytes, commandBytes.Length + betweenBytes.Length + 4, endBytes.Length);

            return resultBytes;
        }

        public List<Event> LoadDefaultEvents()
        {
            return LoadEventsFromFile(GetDefaultDataFilepath(CommandType.EventCommand));
        }

        public List<Event> LoadEventsFromFile(string filepath)
        {
            return GetEventsFromBytes(File.ReadAllBytes(filepath));
        }

        public List<Event> GetEventsFromBytes(IList<byte> bytes)
        {
            List<Event> result = new List<Event>();

            int index = 0;
            for (int startIndex = 0; startIndex < bytes.Count; startIndex += 0x2000)
            {
                result.Add(GetEventFromBytes(index, bytes.SubLength(startIndex, 0x2000)));
                index++;
            }

            return result;
        }

        public Event GetEventFromBytes(int index, IList<byte> bytes)
        {
            List<Command> commandList = CommandsFromByteArray(CommandType.EventCommand, bytes.Sub(4), new HashSet<int>() { 0xDB, 0xE3 });

            int numCommandBytes = 0;
            foreach (Command command in commandList)
                if (command != null)
                    numCommandBytes += command.GetTotalByteLength();

            int naturalTextOffset = numCommandBytes + 4;
            uint textOffset = bytes.SubLength(0, 4).ToUInt32();

            CustomSection betweenSection;
            CustomSection endSection;

            if (textOffset == 0xF2F2F2F2U)
            {
                betweenSection = new CustomSection();
                endSection = new CustomSection(bytes.Sub(naturalTextOffset));
            }
            else
            {
                betweenSection = (textOffset > naturalTextOffset) ? new CustomSection(bytes.SubLength(naturalTextOffset, ((int)textOffset - naturalTextOffset))) : new CustomSection();
                endSection = new CustomSection(bytes.Sub(textOffset));
            }

            return new Event(index, entryNameMaps[CommandType.EventCommand][index], textOffset, commandList, betweenSection, endSection);
        }

        public byte[] ConditionalSetsToByteArray(CommandType type, List<ConditionalSet> conditionalSets)
        {
            int numSets = conditionalSets.Count;
            int numBlocks = 0;
            foreach (ConditionalSet set in conditionalSets)
                numBlocks += set.ConditionalBlocks.Count;
            
            List<UInt16> setReferences = new List<UInt16>(numSets);
            List<UInt16> blockReferences = new List<UInt16>();
            List<byte> commandBytes = new List<byte>();

            UInt16 setReference = (UInt16)(numSets * 2);
            UInt16 blockReference = (UInt16)((setReference + numBlocks) * 2);
            foreach (ConditionalSet set in conditionalSets)
            {
                setReferences.Add(setReference);
                foreach (ConditionalBlock block in set.ConditionalBlocks)
                {
                    blockReferences.Add(blockReference);
                    byte[] currentCommandBytes = CommandsToByteArray(block.Commands);
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

        private List<ConditionalSet> LoadConditionalSetDefaults(CommandType type)
        {
            return LoadConditionalSetsFromFile(type, GetDefaultDataFilepath(type));
        }

        private List<ConditionalSet> LoadConditionalSetsFromFile(CommandType type, string filepath)
        {
            return LoadConditionalSetsFromByteArray(type, File.ReadAllBytes(filepath));
        }

        private List<ConditionalSet> LoadConditionalSetsFromByteArray(CommandType type, IList<byte> bytes)
        {
            int setByteOffset = bytes.ToUInt16LE();
            int numSets = setByteOffset / 2;
            List<ConditionalSet> result = new List<ConditionalSet>(numSets);

            Dictionary<int, string> setNameMap = entryNameMaps[type];

            UInt16[] setOffsets = bytes.SubLength(0, setByteOffset).ToUInt16ArrayLE();
            int blockByteOffset = bytes.SubLength(setOffsets[0], 2).ToUInt16LE();

            for (int setIndex = 0; setIndex < numSets; setIndex++)
            {
                List<ConditionalBlock> conditionalBlocks = new List<ConditionalBlock>();
                string setName = setNameMap[setIndex];

                int setStartIndex = setOffsets[setIndex];
                int setEndIndex = ((setIndex < (numSets - 1)) ? setOffsets[setIndex + 1] : blockByteOffset);

                int numBlocks = 0;
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

                        conditionalBlocks.Add(new ConditionalBlock(numBlocks, CommandsFromByteArray(type, bytes.SubLength(startIndex, endIndex - startIndex))));
                        numBlocks++;
                    }
                    
                    prevBlockIndex = blockIndex;
                }

                result.Add(new ConditionalSet(setIndex, setName, conditionalBlocks));
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

        private List<Command> CommandsFromByteArray(CommandType type, IList<byte> bytes, ICollection<int> sentinelCommands = null)
        {
            List<Command> result = new List<Command>();
            int startIndex = 0;
            int lastCommandID = -1;

            while ((startIndex < bytes.Count) && ((sentinelCommands == null) || (!sentinelCommands.Contains(lastCommandID))))
            {
                Command command = CommandFromByteArray(type, bytes.SubLength(startIndex, bytes.Count - startIndex));
                if (command == null)
                    break;

                if (command.Template != null)
                    lastCommandID = command.Template.ID;

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

            CommandTemplate resultTemplate = null;
            CommandTemplate template = null;

            int index = 0;
            for (; index < 4; index++)
            {
                value |= (bytes[index] << shiftAmount);
                shiftAmount += 8;

                if (templateMap.TryGetValue(value, out template))
                {
                    resultTemplate = template;
                    break;
                }
            }

            if (resultTemplate == null)
                return null;

            index = resultTemplate.ByteLength;
            List<CommandParameter> parameters = new List<CommandParameter>();
            foreach (CommandParameterTemplate parameterTemplate in template.Parameters)
            {
                int byteLength = parameterTemplate.ByteLength;
                parameters.Add(new CommandParameter(parameterTemplate, bytes.SubLength(index, byteLength).ToIntLE()));
                index += byteLength;
            }

            return new Command(resultTemplate, parameters);
        }
    }
}
