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
    public class CommandData
    {
        public CommandType CommandType { get; private set; }
        public List<string> CommandNames { get; private set; }
        public Dictionary<int, CommandTemplate> CommandMap { get; private set; }
        public int DefaultCommandByteLength { get; private set; }
        public Dictionary<string, Dictionary<int, string>> ParameterValueMaps { get; private set; }
        public int MaxParameters { get; private set; }
        public CommandTemplate DefaultCommandTemplate { get; set; }

        public CommandData(CommandType commandType, List<string> commandNames, Dictionary<int, CommandTemplate> commandMap, int defaultCommandByteLength,
            Dictionary<string, Dictionary<int, string>> parameterValueMaps, int maxParameters, CommandTemplate defaultCommandTemplate)
        {
            this.CommandType = commandType;
            this.CommandNames = commandNames;
            this.CommandMap = commandMap;
            this.DefaultCommandByteLength = defaultCommandByteLength;
            this.ParameterValueMaps = parameterValueMaps;
            this.MaxParameters = maxParameters;
            this.DefaultCommandTemplate = defaultCommandTemplate;
        }
    }

    public class DataHelper
    {
        private const int EventSize = 0x2000;
        private readonly byte[] BlankTextOffsetBytes = new byte[4] { 0xF2, 0xF2, 0xF2, 0xF2 };

        private readonly List<string> parameterTypes;
        private readonly Dictionary<CommandType, int> defaultCommandByteLengthMaps;
        private readonly Dictionary<CommandType, CommandTemplate> defaultCommandTemplateMap;

        private readonly Dictionary<string, Dictionary<int, string>> parameterValueMaps;
        private readonly Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandTemplateMaps;
        private readonly Dictionary<CommandType, Dictionary<int, string>> entryNameMaps;

        public DataHelper()
        {
            parameterTypes = new List<string>();
            defaultCommandByteLengthMaps = new Dictionary<CommandType, int>();
            defaultCommandTemplateMap = new Dictionary<CommandType, CommandTemplate>();

            commandTemplateMaps = GetCommandTemplateMaps();
            parameterValueMaps = GetParameterValueMaps();
            entryNameMaps = GetEntryNameMaps();
        }

        public static List<string> GetParameterEntryNames(CommandParameterTemplate template, Dictionary<int, string> valueMap)
        {
            List<string> result = new List<string>();

            int numValues = 1 << (template.ByteLength * 8);
            string hexFormatString = "X" + (template.ByteLength * 2);
            string entry = "";
            int numEntriesFound = 0;
            int totalEntries = valueMap.Count;
            bool allEntriesFound = false;

            for (int index = 0; index < numValues; index++)
            {
                if (allEntriesFound && (index >= 256))
                    break;

                string strIndex = index.ToString(hexFormatString);
                if (valueMap.TryGetValue(index, out entry))
                {
                    result.Add(strIndex + " " + entry);
                    numEntriesFound++;
                    allEntriesFound = (numEntriesFound >= totalEntries);
                }
                else
                {
                    result.Add(strIndex);
                }
            }

            return result;
        }

        public List<ConditionalSet> LoadBattleConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.BattleConditional);
        }

        public List<ConditionalSet> LoadWorldConditionalDefaults()
        {
            return LoadConditionalSetDefaults(CommandType.WorldConditional);
        }

        public List<string> GetParameterValueList(int numBytes, string type)
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

        public Dictionary<string, Dictionary<int, string>> GetParameterMaps()
        {
            Dictionary<string, Dictionary<int, string>> result = new Dictionary<string, Dictionary<int, string>>();

            foreach (KeyValuePair<string, Dictionary<int, string>> kvp in parameterValueMaps)
            {
                result.Add(kvp.Key, new Dictionary<int, string>(kvp.Value));
            }

            return result;
        }

        public Dictionary<CommandType, Dictionary<int, CommandTemplate>> GetCommandMaps()
        {
            Dictionary<CommandType, Dictionary<int, CommandTemplate>> result = new Dictionary<CommandType, Dictionary<int, CommandTemplate>>();

            foreach (KeyValuePair<CommandType, Dictionary<int, CommandTemplate>> kvp in commandTemplateMaps)
            {
                result.Add(kvp.Key, new Dictionary<int, CommandTemplate>(kvp.Value));
            }

            return result;
        }

        public Dictionary<CommandType, int> GetDefaultCommandByteLengthMaps()
        {
            return new Dictionary<CommandType, int>(defaultCommandByteLengthMaps);
        }

        public Dictionary<CommandType, CommandTemplate> GetDefaultCommandTemplateMap()
        {
            return new Dictionary<CommandType, CommandTemplate>(defaultCommandTemplateMap);
        }

        public int GetMaxParameters(CommandType type)
        {
            int max = 0;
            foreach (CommandTemplate template in commandTemplateMaps[type].Values)
            {
                if (template.Parameters.Count > max)
                    max = template.Parameters.Count;
            }

            return max;
        }

        public Dictionary<CommandType, CommandData> GetCommandDataMap()
        {
            Dictionary<CommandType, List<string>> commandNames = GetCommandNames();
            Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandMaps = GetCommandMaps();
            Dictionary<CommandType, int> defaultCommandByteLengthMaps = GetDefaultCommandByteLengthMaps();
            Dictionary<string, Dictionary<int, string>> parameterValueMaps = GetParameterMaps();
            Dictionary<CommandType, CommandTemplate> commandTemplateMap = GetDefaultCommandTemplateMap();

            Dictionary<CommandType, CommandData> result = new Dictionary<CommandType, CommandData>();

            result.Add(CommandType.BattleConditional, new CommandData(CommandType.BattleConditional, commandNames[CommandType.BattleConditional], commandMaps[CommandType.BattleConditional], 
                defaultCommandByteLengthMaps[CommandType.BattleConditional], parameterValueMaps, GetMaxParameters(CommandType.BattleConditional), commandTemplateMap[CommandType.BattleConditional]));
            result.Add(CommandType.WorldConditional, new CommandData(CommandType.WorldConditional, commandNames[CommandType.WorldConditional], commandMaps[CommandType.WorldConditional],
                defaultCommandByteLengthMaps[CommandType.WorldConditional], parameterValueMaps, GetMaxParameters(CommandType.WorldConditional), commandTemplateMap[CommandType.WorldConditional]));
            result.Add(CommandType.EventCommand, new CommandData(CommandType.EventCommand, commandNames[CommandType.EventCommand], commandMaps[CommandType.EventCommand],
                defaultCommandByteLengthMaps[CommandType.EventCommand], parameterValueMaps, GetMaxParameters(CommandType.EventCommand), commandTemplateMap[CommandType.EventCommand]));

            return result;
        }

        /*
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
        */

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

        private string GetParameterValueListFilepath(string type)
        {
            return "EntryData/" + type + "Names.xml";
        }

        /*
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
        */

        private List<string> GetParameterEntryNames(CommandParameterTemplate template)
        {
            return GetParameterEntryNames(template, parameterValueMaps[template.Type]);
        }

        private Dictionary<string, Dictionary<int, string>> GetParameterValueMaps()
        {
            Dictionary<string, Dictionary<int, string>> result = new Dictionary<string, Dictionary<int, string>>();

            foreach (string type in parameterTypes)
            {
                result.Add(type, GetXMLNameMap(GetParameterValueListFilepath(type)));
            }

            return result;
        }

        /*
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
        */

        private Dictionary<int, string> GetParameterValueMap(string type)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            //if (type != CommandParameterType.Number)
            if (!string.IsNullOrEmpty(type))
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

            if (File.Exists(filepath))
            {
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
            string strTrue = bool.TrueString.ToLower().Trim();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(GetCommandFilepath(type));
            XmlNode commandsNode = xmlDocument.SelectSingleNode("//Commands");
            XmlAttribute attrDefaultBytes = commandsNode.Attributes["bytes"];

            int defaultCommandByteLength = 1;
            if (attrDefaultBytes != null)
            {
                int.TryParse(attrDefaultBytes.InnerText, out defaultCommandByteLength);
            }

            defaultCommandByteLengthMaps.Add(type, defaultCommandByteLength);

            XmlNodeList nodeList = xmlDocument.SelectNodes("//Command");
            foreach (XmlNode node in nodeList)
            {
                int nodeValue = GetNodeValue(node);
                XmlAttribute attrName = node.Attributes["name"];
                XmlAttribute attrBytes = node.Attributes["bytes"];
                XmlAttribute attrDefault = node.Attributes["default"];

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
                    bool isDefault = (attrDefault != null) && (attrDefault.InnerText.ToLower().Trim() == strTrue);

                    List<CommandParameterTemplate> commandTemplateParameters = new List<CommandParameterTemplate>();
                    foreach (XmlNode parameterNode in node.SelectNodes("Parameter"))
                    {
                        XmlAttribute attrParamName = parameterNode.Attributes["name"];
                        XmlAttribute attrParamBytes = parameterNode.Attributes["bytes"];
                        XmlAttribute attrParamType = parameterNode.Attributes["type"];
                        XmlAttribute attrParamMode = parameterNode.Attributes["mode"];
                        XmlAttribute attrParamText = parameterNode.Attributes["text"];
                        XmlAttribute attrParamDefault = parameterNode.Attributes["default"];

                        int paramByteLength = 1;
                        if (attrParamBytes != null)
                        {
                            int.TryParse(attrParamBytes.InnerText, out paramByteLength);
                        }

                        string parameterTemplateName = (attrParamName != null) ? attrParamName.InnerText : CommandParameterTemplate.DefaultName;
                        int parameterTemplateByteLength = paramByteLength;
                        //CommandParameterType parameterTemplateType = (attrParamType != null) ? GetParameterType(attrParamType.InnerText) : CommandParameterType.Number;
                        string parameterTemplateType = (attrParamType != null) ? attrParamType.InnerText.ToLower().Trim() : "";

                        if ((!string.IsNullOrEmpty(parameterTemplateType)) && (!parameterTypes.Contains(parameterTemplateType)))
                            parameterTypes.Add(parameterTemplateType);

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
                            //bool isNumber = (parameterTemplateType == CommandParameterType.Number);
                            bool isNumber = string.IsNullOrEmpty(parameterTemplateType);
                            isHex = !isNumber;
                            isSigned = isNumber;
                        }

                        bool isTextReference = (attrParamText != null) && (attrParamText.InnerText.ToLower().Trim() == strTrue);
                        int defaultValue = 0;
                        if (attrParamDefault != null)
                            int.TryParse(attrParamDefault.InnerText, out defaultValue);

                        commandTemplateParameters.Add(new CommandParameterTemplate(parameterTemplateName, parameterTemplateByteLength, isHex, isSigned, isTextReference, parameterTemplateType, defaultValue));
                    }

                    CommandTemplate commandTemplate = new CommandTemplate(commandTemplateID, commandTemplateName, commandTemplateByteLength, type, commandTemplateParameters);
                    result.Add(commandTemplateID, commandTemplate);
                    if (isDefault)
                    {
                        defaultCommandTemplateMap.Add(type, commandTemplate);
                    }
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

        public Dictionary<CommandType, List<string>> GetCommandNames()
        {
            Dictionary<CommandType, List<string>> result = new Dictionary<CommandType, List<string>>();
            result.Add(CommandType.BattleConditional, GetCommandNamesByType(CommandType.BattleConditional, 256, 2));
            result.Add(CommandType.WorldConditional, GetCommandNamesByType(CommandType.WorldConditional, 256, 2));
            result.Add(CommandType.EventCommand, GetCommandNamesByType(CommandType.EventCommand, 256, 1));
            return result;
        }

        public List<string> GetCommandNamesByType(CommandType type, int numValues, int actualByteLength)
        {
            List<string> result = new List<string>();
            string hexFormatString = "X" + (actualByteLength * 2);

            for (int index = 0; index < numValues; index++)
            {
                CommandTemplate template = null;
                if (commandTemplateMaps[type].TryGetValue(index, out template))
                {
                    result.Add(index.ToString("X" + (template.ByteLength * 2)) + " " + template.Name);
                }
                else
                {
                    result.Add(index.ToString(hexFormatString));
                }
            }

            return result;
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
            byte[] resultBytes = new byte[events.Count * EventSize];

            int copyIndex = 0;
            foreach (Event inputEvent in events)
            {
                byte[] eventBytes = EventToByteArray(inputEvent);
                Array.Copy(eventBytes, 0, resultBytes, copyIndex, EventSize);
                copyIndex += eventBytes.Length;
            }

            return resultBytes;
        }

        public byte[] EventToByteArray(Event inputEvent, bool forceOriginalSize = true)
        {
            //byte[] textOffsetBytes = inputEvent.TextOffset.ToBytes();
            byte[] commandBytes = CommandsToByteArray(inputEvent.CommandList);
            byte[] dataBytes = inputEvent.DataSection.ToByteArray();
            byte[] textBytes = inputEvent.TextSection.ToByteArray();
            byte[] textOffsetBytes = (textBytes.Length > 0) ? (commandBytes.Length + dataBytes.Length + 4).ToBytesLE() : BlankTextOffsetBytes;

            /*
            byte[] resultBytes = new byte[4 + commandBytes.Length + dataBytes.Length + textBytes.Length];
            Array.Copy(textOffsetBytes, resultBytes, 4);
            Array.Copy(commandBytes, 0, resultBytes, 4, commandBytes.Length);
            Array.Copy(dataBytes, 0, resultBytes, commandBytes.Length + 4, dataBytes.Length);
            Array.Copy(endBytes, 0, resultBytes, commandBytes.Length + dataBytes.Length + 4, textBytes.Length);
            */

            List<byte> resultByteList = new List<byte>(EventSize);
            resultByteList.AddRange(textOffsetBytes);
            resultByteList.AddRange(commandBytes);
            resultByteList.AddRange(dataBytes);
            resultByteList.AddRange(textBytes);

            if (forceOriginalSize)
            {
                //byte[] resultBytes = new byte[EventSize];
                byte[] resultBytes = new List<byte>(inputEvent.OriginalBytes).ToArray();
                Array.Copy(resultByteList.ToArray(), resultBytes, Math.Min(inputEvent.OriginalBytes.Count, resultByteList.Count));
                return resultBytes;
            }
            else
            {
                return resultByteList.ToArray();
            }
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

            CustomSection dataSection, textSection;

            CustomSection originalTextSection = null;
            if (textOffset == 0xF2F2F2F2U)
            {
                dataSection = new CustomSection();
                textSection = new CustomSection();
                originalTextSection = new CustomSection();
            }
            else
            {
                dataSection = (textOffset > naturalTextOffset) ? new CustomSection(bytes.SubLength(naturalTextOffset, ((int)textOffset - naturalTextOffset))) : new CustomSection();
                int numTextEntries = Event.FindNumTextEntries(commandList);
                numTextEntries = (numTextEntries > 0) ? numTextEntries : 1;
                IList<byte> textBytes = bytes.Sub(textOffset);
                IList<IList<byte>> textByteLists = textBytes.Split((byte)0xFE);
                IList<string> textList = TextUtility.DecodeList(textBytes);
                textSection = new CustomSection(textByteLists, textList, numTextEntries);
                originalTextSection = new CustomSection(textByteLists, textList, textByteLists.Count);
            }

            return new Event(index, entryNameMaps[CommandType.EventCommand][index], commandList, dataSection, textSection, originalTextSection, new List<byte>(bytes));
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

                        ConditionalBlock newBlock = new ConditionalBlock(numBlocks, CommandsFromByteArray(type, bytes.SubLength(startIndex, endIndex - startIndex)));
                        newBlock.FindName(parameterValueMaps);
                        conditionalBlocks.Add(newBlock);
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
            List<CommandParameter> parameters = new List<CommandParameter>(template.Parameters.Capacity);
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
