﻿using System;
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
        public const string EntryNameBattleConditionals = "BattleConditionals.bin";
        public const string EntryNameWorldConditionals = "WorldConditionals.bin";
        public const string EntryNameEvents = "Events.bin";
        public const uint BlankTextOffsetValue = 0xF2F2F2F2U;

        public static readonly byte[] BlankTextOffsetBytes = new byte[4] { 0xF2, 0xF2, 0xF2, 0xF2 };

        public int BattleConditionalsSize { get { return settings.BattleConditionalsSize; } }
        public int WorldConditionalsSize { get { return settings.WorldConditionalsSize; } }
        public int EventSize { get { return settings.EventSize; } }
        public int BattleConditionalSetMaxBlocks { get { return settings.BattleConditionalSetMaxBlocks; } }
        public int BattleConditionalSetMaxCommands { get { return settings.BattleConditionalSetMaxCommands; } }

        private const string StrUnknown = "unknown";
        private const string StrBlank = "blank";

        private readonly List<string> parameterTypes;
        private readonly Dictionary<CommandType, int> defaultCommandByteLengthMaps;
        private readonly Dictionary<CommandType, CommandTemplate> defaultCommandTemplateMap;

        private readonly Dictionary<string, Dictionary<int, string>> parameterValueMaps;
        private readonly Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandTemplateMaps;
        private readonly Dictionary<CommandType, Dictionary<int, string>> entryNameMaps;
        private readonly Dictionary<CommandType, HashSet<int>> sentinelCommandIDMap;

        private Context context;
        private SettingsData settings;

        public DataHelper(Context context)
        {
            this.context = context;
            settings = Settings.GetSettings(context);

            parameterTypes = new List<string>();
            defaultCommandByteLengthMaps = new Dictionary<CommandType, int>();
            defaultCommandTemplateMap = new Dictionary<CommandType, CommandTemplate>();

            commandTemplateMaps = GetCommandTemplateMaps();
            parameterValueMaps = GetParameterValueMaps();
            entryNameMaps = GetEntryNameMaps();
            sentinelCommandIDMap = GetSentinelCommandIDMap();
        }

        public static string GetCommandListScript(IEnumerable<Command> commands)
        {
            StringBuilder sb = new StringBuilder();

            if (commands != null)
            {
                foreach (Command command in commands)
                {
                    sb.AppendLine(command.GetScriptString());
                }
            }

            return sb.ToString();
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

        public static int GetMaxBlocks(IEnumerable<ConditionalSet> conditionalSets)
        {
            int result = 0;

            if (conditionalSets != null)
            {
                foreach (ConditionalSet conditionalSet in conditionalSets)
                {
                    if (conditionalSet.ConditionalBlocks != null)
                        result = Math.Max(result, conditionalSet.ConditionalBlocks.Count);
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

        public List<ConditionalSet> LoadTrimmedBattleConditionals()
        {
            return LoadConditionalSetsFromFile(CommandType.BattleConditional, settings.FilepathTrimmedBattleConditionals);
        }

        public ConditionalSet LoadActiveConditionalSet(int setIndex, string name, CommandType type, IList<byte> blockOffsetBytes, IList<byte> commandBytes)
        {
            List<UInt16> blockOffsets = new List<UInt16>(blockOffsetBytes.Count / 2);

            for (int index = 0; index < blockOffsetBytes.Count; index += 2)
            {
                UInt16 blockOffset = (UInt16)((blockOffsetBytes[index] | (blockOffsetBytes[index + 1] << 8)));

                if ((blockOffset != 0) || (index == 0))
                    blockOffsets.Add(blockOffset);
                else
                    break;
            }

            int numRealBlockOffsetBytes = blockOffsets.Count * 2;
            UInt16 blockOffsetAddend = (UInt16)(numRealBlockOffsetBytes + 2);
            for (int index = 0; index < blockOffsets.Count; index++)
            {
                blockOffsets[index] += blockOffsetAddend;
            }

            List<byte> setBytes = new List<byte>(commandBytes.Count + numRealBlockOffsetBytes + 2);
            setBytes.AddRange(new byte[2] { 0x02, 0x00 });
            setBytes.AddRange(blockOffsets.ToBytesLE());
            setBytes.AddRange(commandBytes);

            List<ConditionalSet> resultSets = LoadConditionalSetsFromByteArray(type, setBytes);
            ConditionalSet result = ((resultSets.Count > 0) ? resultSets[0] : null);

            if (result != null)
            {
                result.Index = setIndex;
                result.Name = name;
            }

            return result;
        }

        public List<byte[]> ConditionalSetToActiveByteArrays(CommandType type, ConditionalSet conditionalSet)
        {
            int blockOffsetCount = conditionalSet.ConditionalBlocks.Count;
            int commandByteIndex = (blockOffsetCount * 2) + 2;

            byte[] rawBytes = ConditionalSetsToByteArray(type, new List<ConditionalSet>() { conditionalSet });
            IList<byte> blockOffsetBytes = rawBytes.Sub(2, commandByteIndex - 1);
            IList<byte> commandBytes = rawBytes.Sub(commandByteIndex);

            UInt16[] blockOffsets = new UInt16[blockOffsetCount];
            for (int index = 0; index < blockOffsets.Length; index++)
            {
                blockOffsets[index] = (UInt16)((blockOffsetBytes[index * 2] | (blockOffsetBytes[(index * 2) + 1] << 8)) - blockOffsetBytes.Count - 2);
            }

            return new List<byte[]>() { blockOffsets.ToBytesLE().ToArray(), commandBytes.ToArray() };
        }

        public List<ConditionalSet> LoadAllConditionalSetScripts(CommandType type, string path)
        {
            try
            {
                List<ConditionalSet> result = new List<ConditionalSet>();

                string[] innerPaths = Directory.GetDirectories(path);
                innerPaths.Sort();
                for (int index = 0; index < innerPaths.Length; index++)
                {
                    string name = entryNameMaps[type][index];
                    List<ConditionalBlock> blocks = LoadAllConditionalBlockScripts(type, innerPaths[index]);
                    result.Add(new ConditionalSet(index, name, blocks));
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SaveAllConditionalSetScripts(IList<ConditionalSet> conditionalSets, string path)
        {
            try
            {
                for (int index = 0; index < conditionalSets.Count; index++)
                {
                    ConditionalSet set = conditionalSets[index];
                    string innerDirectoryName = string.Format("{0} {1} {2}", set.Index.ToString("000"), set.Index.ToString("X2"), set.Name);
                    string innerFilepath = Path.Combine(path, innerDirectoryName);
                    Directory.CreateDirectory(innerFilepath);
                    SaveAllConditionalBlockScripts(set.ConditionalBlocks, innerFilepath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Event> LoadAllEventScripts(IList<Event> events, string path)
        {
            try
            {
                List<Event> result = new List<Event>();
                HashSet<int> sentinelCommands = sentinelCommandIDMap[CommandType.EventCommand];
                string[] filepaths = Directory.GetFiles(path);
                filepaths.Sort();
                for (int index = 0; index < filepaths.Length; index++)
                {
                    string script = File.ReadAllText(filepaths[index]);
                    result.Add(GetEventFromScript(script, events[index], sentinelCommands));
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SaveAllEventScripts(IList<Event> events, string path)
        {
            try
            {
                Directory.CreateDirectory(path);

                for (int index = 0; index < events.Count; index++)
                {
                    Event ev = events[index];
                    string filename = string.Format("{0} {1} {2}.txt", ev.Index.ToString("000"), ev.Index.ToString("X4"), ev.Name);
                    File.WriteAllText(Path.Combine(path, filename), ev.GetScript(), Encoding.UTF8);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ConditionalBlock GetConditionalBlockFromScript(CommandType type, int blockIndex, string script)
        {
            List<Command> commandList = GetCommandListFromScript(type, script);
            if (commandList != null)
            {
                ConditionalBlock block = new ConditionalBlock(blockIndex, commandList);
                block.FindName(parameterValueMaps, settings.IgnoreParameterTypes);
                return block;
            }
            else
            {
                return null;
            }
        }

        public Event GetEventFromScript(string script, Event originalEvent, HashSet<int> sentinelCommands = null)
        {
            //int textMarkerIndex = script.LastIndexOf("{TEXT}");
            //int dataMarkerIndex = script.LastIndexOf("{DATA}");
            //int firstMarkerIndex = unchecked((int)Math.Min(unchecked((uint)textMarkerIndex), unchecked((uint)dataMarkerIndex)));

            const string constSectionText = "{SECTION:";
            const string constTextSectionName = "TEXT";

            try
            {
                int sectionTextIndex = script.IndexOf(constSectionText);

                string strCommandSection = (sectionTextIndex == -1) ? script : script.Substring(0, sectionTextIndex);
                string strTextSection = string.Empty;
                string strDataSection = string.Empty;

                List<Command> commandList = GetCommandListFromScript(CommandType.EventCommand, strCommandSection);
                if (commandList == null)
                    return null;

                while (sectionTextIndex >= 0)
                {
                    int sectionNameIndex = sectionTextIndex + constSectionText.Length;
                    int endBraceIndex = script.IndexOf("}", sectionNameIndex);
                    string sectionTitle = script.Substring(sectionNameIndex, endBraceIndex - sectionNameIndex);
                    bool isTextSection = (sectionTitle.ToUpper().Trim().Equals(constTextSectionName));
                    int nextSectionTextIndex = script.IndexOf(constSectionText, endBraceIndex);
                    string sectionBody = (nextSectionTextIndex == -1) ? script.Substring(endBraceIndex + 1) : script.Substring(endBraceIndex + 1, nextSectionTextIndex - endBraceIndex);

                    if (isTextSection)
                        strTextSection = sectionBody;
                    else
                        strDataSection = sectionBody;

                    sectionTextIndex = nextSectionTextIndex;
                }

                /*
                if (textMarkerIndex >= 0)
                {
                    strTextSection = (dataMarkerIndex == -1) ? script.Substring(textMarkerIndex) : script.Substring(textMarkerIndex, textMarkerIndex - dataMarkerIndex + 1);
                }
                if (dataMarkerIndex >= 0)
                {
                    strDataSection = script.Substring(dataMarkerIndex);
                }
                */

                CustomSection textSection = GetCustomSectionFromScript(strTextSection, true);
                CustomSection dataSection = GetCustomSectionFromScript(strDataSection, false);

                Event newEvent = new Event(originalEvent.Index, originalEvent.Name, commandList, dataSection, textSection, textSection, originalEvent.OriginalBytes);
                return GetEventFromBytes(originalEvent.Index, EventToByteArray(newEvent, true), false, sentinelCommands);
            }
            catch (Exception)
            {
                return null;
            }
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

        private List<Command> GetCommandListFromScript(CommandType type, string script)
        {
            try
            {
                List<Command> commandList = new List<Command>();
                string[] lines = PatcherLib.Utilities.Utilities.SplitIntoLines(script);
                Dictionary<string, CommandTemplate> commandTemplateNameMap = GetCommandTemplateNameMap(type, true);

                foreach (string origLine in lines)
                {
                    if (string.IsNullOrEmpty(origLine))
                        continue;

                    string line = PatcherLib.Utilities.Utilities.RemoveWhitespace(origLine.Trim());

                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (line.StartsWith("//"))
                        continue;

                    int openingParenIndex = line.IndexOf('(');
                    int closingParenIndex = line.LastIndexOf(')');
                    string commandName = line.Substring(0, openingParenIndex);
                    CommandTemplate commandTemplate = null;
                    if (commandTemplateNameMap.TryGetValue(commandName, out commandTemplate))
                    {
                        string[] strParams = line.Substring(openingParenIndex + 1, closingParenIndex - openingParenIndex - 1).Split(',');
                        List<CommandParameter> commandParameterList = new List<CommandParameter>(commandTemplate.Parameters.Capacity);
                        for (int index = 0; index < commandTemplate.Parameters.Count; index++)
                        {
                            int value = 0;
                            bool isHex = strParams[index].StartsWith("0x");
                            string strParam = strParams[index].Replace("0x", "");

                            if (int.TryParse(strParam, isHex ? NumberStyles.HexNumber : NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                            {
                                commandParameterList.Add(new CommandParameter(commandTemplate.Parameters[index], value));
                            }
                            else
                            {
                                return null;
                            }
                        }

                        commandList.Add(new Command(commandTemplate, commandParameterList));
                    }
                    else
                    {
                        return null;
                    }
                }

                return commandList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<ConditionalBlock> LoadAllConditionalBlockScripts(CommandType type, string path)
        {
            try
            {
                List<ConditionalBlock> result = new List<ConditionalBlock>();

                string[] filepaths = Directory.GetFiles(path);
                filepaths.Sort();
                for (int index = 0; index < filepaths.Length; index++)
                {
                    string script = File.ReadAllText(filepaths[index]);
                    result.Add(GetConditionalBlockFromScript(type, index, script));
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool SaveAllConditionalBlockScripts(IList<ConditionalBlock> conditionalBlocks, string path)
        {
            try
            {
                for (int index = 0; index < conditionalBlocks.Count; index++)
                {
                    ConditionalBlock block = conditionalBlocks[index];
                    string filename = string.Format("{0} {1} {2}.txt", block.Index.ToString("000"), block.Index.ToString("X2"), block.Name);
                    File.WriteAllText(Path.Combine(path, filename), block.GetScript(), Encoding.UTF8);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private CustomSection GetCustomSectionFromScript(string script, bool isTextSection)
        {
            KeyValuePair<int, int> entryTextIndexes = FindCustomSectionEntryTextIndex(script, 0);
            KeyValuePair<int, int> nextEntryTextIndexes = FindCustomSectionEntryTextIndex(script, entryTextIndexes.Value + 1);

            List<CustomEntry> customEntryList = new List<CustomEntry>();
            int entryIndex = 0;
            while (entryTextIndexes.Value != -1)
            {
                int length = nextEntryTextIndexes.Key - entryTextIndexes.Value + 1;
                string strEntry = script.Substring(entryTextIndexes.Value, length);

                if (strEntry.EndsWith("\r"))
                    strEntry = strEntry.Substring(0, strEntry.Length - 1);

                if (isTextSection)
                {
                    customEntryList.Add(new CustomEntry(entryIndex, strEntry, context));
                }
                else
                {
                    customEntryList.Add(new CustomEntry(entryIndex, new List<byte>(PatcherLib.Utilities.Utilities.GetBytesFromHexString(strEntry)), context));
                }

                entryTextIndexes = nextEntryTextIndexes;
                nextEntryTextIndexes = FindCustomSectionEntryTextIndex(script, entryTextIndexes.Value + 1);
                entryIndex++;
            }

            return new CustomSection(customEntryList, context);
        }

        private KeyValuePair<int, int> FindCustomSectionEntryTextIndex(string script, int textIndex)
        {
            int entryTextIndex = script.IndexOf("\r\n\r\nEntry", textIndex);
            if (entryTextIndex == -1)
                entryTextIndex = script.IndexOf("\n\nEntry", textIndex);

            if (entryTextIndex == -1)
            {
                entryTextIndex = script.IndexOf("\r\n\r\n", textIndex);
                if (entryTextIndex == -1)
                    entryTextIndex = script.IndexOf("\n\n", textIndex);

                return new KeyValuePair<int, int>(entryTextIndex, -1);
            }
            else
            {
                return new KeyValuePair<int, int>(entryTextIndex, script.IndexOf('\n', entryTextIndex + 4) + 1);
            }
        }

        private Dictionary<string, CommandTemplate> GetCommandTemplateNameMap(CommandType type, bool removeSpaces)
        {
            Dictionary<int, CommandTemplate> commandTemplateMap = commandTemplateMaps[type];
            Dictionary<string, CommandTemplate> result = new Dictionary<string, CommandTemplate>();

            foreach (CommandTemplate commandTemplate in commandTemplateMap.Values)
            {
                string name = commandTemplate.Name;

                if (commandTemplate.IsUnknown)
                    name = String.Format("{{{0}}}", commandTemplate.ID.ToString("X" + (commandTemplate.ByteLength << 1)));
                else if (removeSpaces)
                    name = PatcherLib.Utilities.Utilities.RemoveWhitespace(commandTemplate.Name);

                result.Add(name, commandTemplate);
            }

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
                case CommandType.BattleConditional: return settings.FilepathDefaultBattleConditionals;
                case CommandType.WorldConditional: return settings.FilepathDefaultWorldConditionals;
                case CommandType.EventCommand: return settings.FilepathDefaultEvents;
                default: return null;
            }
        }
        
        private string GetCommandFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return settings.FilepathCommandBattleConditionals;
                case CommandType.WorldConditional: return settings.FilepathCommandWorldConditionals;
                case CommandType.EventCommand: return settings.FilepathCommandEvents;
                default: return null;
            }
        }

        private string GetEntryNameFilepath(CommandType type)
        {
            switch (type)
            {
                case CommandType.BattleConditional: return settings.FilepathNamesBattleConditionals;
                case CommandType.WorldConditional: return settings.FilepathNamesWorldConditionals;
                case CommandType.EventCommand: return settings.FilepathNamesEvents;
                default: return null;
            }
        }

        private string GetParameterValueListFilepath(string type)
        {
            return settings.GetFilepath(type + "Names.xml");
        }

        /*
        private string GetParameterValueListFilepath(string type)
        {
            string normalFilepath = "EntryData/" + type + "Names.xml";

            if (!string.IsNullOrEmpty(settings.ModSuffix))
            {
                string typeSuffixFilepath = "EntryData/" + type + "Names" + settings.ModSuffix + ".xml";
                return File.Exists(typeSuffixFilepath) ? typeSuffixFilepath : normalFilepath;
            }
            else
            {
                return normalFilepath;
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
        */

        private List<string> GetParameterEntryNames(CommandParameterTemplate template)
        {
            return GetParameterEntryNames(template, parameterValueMaps[template.Type]);
        }

        private Dictionary<string, Dictionary<int, string>> GetParameterValueMaps()
        {
            Dictionary<string, Dictionary<int, string>> result = new Dictionary<string, Dictionary<int, string>>();

            if (!settings.IgnoreParameterTypes)
            {
                foreach (string type in parameterTypes)
                {
                    Dictionary<int, string> map = GetXMLNameMap(GetParameterValueListFilepath(type));
                    if (map.Count > 0)
                        result.Add(type, map);
                }
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

        private Dictionary<int, string> GetParameterValueMap(string type)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            //if (type != CommandParameterType.Number)
            if (!string.IsNullOrEmpty(type))
            {
                string filepath = GetParameterValueListFilepath(type);

                if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
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
            }

            return result;
        }
        */

        private Dictionary<CommandType, Dictionary<int, string>> GetEntryNameMaps()
        {
            Dictionary<CommandType, Dictionary<int, string>> result = new Dictionary<CommandType, Dictionary<int, string>>();
            result.Add(CommandType.BattleConditional, GetXMLNameMap(GetEntryNameFilepath(CommandType.BattleConditional)));
            result.Add(CommandType.WorldConditional, GetXMLNameMap(GetEntryNameFilepath(CommandType.WorldConditional)));
            result.Add(CommandType.EventCommand, GetXMLNameMap(GetEntryNameFilepath(CommandType.EventCommand)));
            return result;
        }

        private Dictionary<CommandType, HashSet<int>> GetSentinelCommandIDMap()
        {
            return new Dictionary<CommandType, HashSet<int>>()
            {
                { CommandType.BattleConditional, FindSentinelCommandIDs(CommandType.BattleConditional) },
                { CommandType.WorldConditional, FindSentinelCommandIDs(CommandType.WorldConditional) },
                { CommandType.EventCommand, FindSentinelCommandIDs(CommandType.EventCommand) }
            };
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
                XmlAttribute attrUnknown = node.Attributes["unknown"];
                XmlAttribute attrSentinel = node.Attributes["sentinel"];

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
                    string lowerCommandTemplateName = commandTemplateName.ToLower().Trim();
                    bool isUnknown = ((lowerCommandTemplateName.Equals(StrUnknown)) || (lowerCommandTemplateName.Equals(StrBlank)) || ((attrUnknown != null) && (attrUnknown.InnerText.ToLower().Trim() == strTrue)));
                    bool isSentinel = ((attrSentinel != null) && (attrSentinel.InnerText.ToLower().Trim() == strTrue));

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

                    CommandTemplate commandTemplate = new CommandTemplate(commandTemplateID, commandTemplateName, commandTemplateByteLength, isUnknown, isSentinel, type, commandTemplateParameters);
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

        public EntryData LoadTrimmedEntryData()
        {
            return new EntryData(LoadTrimmedBattleConditionals(), LoadWorldConditionalDefaults(), LoadDefaultEvents());
        }

        public EntryData LoadEntryDataFromBytes(EntryBytes entryBytes)
        {
            return LoadEntryDataFromBytes(entryBytes.BattleConditionals, entryBytes.WorldConditionals, entryBytes.Events);
        }

        public EntryData LoadEntryDataFromBytes(byte[] bytesBattleConditionals, byte[] bytesWorldConditionals, byte[] bytesEvents)
        {
            return new EntryData(LoadConditionalSetsFromByteArray(CommandType.BattleConditional, bytesBattleConditionals), LoadConditionalSetsFromByteArray(CommandType.WorldConditional, bytesWorldConditionals),
                GetEventsFromBytes(bytesEvents));
        }

        public EntryBytes GetEntryBytesFromData(EntryData entryData)
        {
            return new EntryBytes(ConditionalSetsToByteArray(CommandType.BattleConditional, entryData.BattleConditionals), ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals),
                EventsToByteArray(entryData.Events));
        }

        public EntryBytes LoadDefaultEntryBytes()
        {
            return new EntryBytes(File.ReadAllBytes(GetDefaultDataFilepath(CommandType.BattleConditional)), File.ReadAllBytes(GetDefaultDataFilepath(CommandType.WorldConditional)),
                File.ReadAllBytes(GetDefaultDataFilepath(CommandType.EventCommand)));
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
            HashSet<int> sentinelCommands = sentinelCommandIDMap[CommandType.EventCommand];
            for (int startIndex = 0; startIndex < bytes.Count; startIndex += EventSize)
            {
                result.Add(GetEventFromBytes(index, bytes.SubLength(startIndex, EventSize), false, sentinelCommands));
                index++;
            }

            return result;
        }

        public Event GetEventFromBytes(int index, IList<byte> bytes, bool forceNaturalTextOffset = false, HashSet<int> sentinelCommands = null)
        {
            //List<Command> commandList = CommandsFromByteArray(CommandType.EventCommand, bytes.Sub(4), new HashSet<int>() { 0xDB, 0xE3 });
            List<Command> commandList = CommandsFromByteArray(CommandType.EventCommand, bytes.Sub(4), sentinelCommands ?? sentinelCommandIDMap[CommandType.EventCommand]);

            int numCommandBytes = 0;
            foreach (Command command in commandList)
                if (command != null)
                    numCommandBytes += command.GetTotalByteLength();

            int naturalTextOffset = numCommandBytes + 4;
            uint textOffset = forceNaturalTextOffset ? (uint)naturalTextOffset : bytes.SubLength(0, 4).ToUInt32();

            CustomSection dataSection, textSection;

            CustomSection originalTextSection = null;
            if (textOffset == BlankTextOffsetValue)
            {
                dataSection = new CustomSection(context);
                textSection = new CustomSection(context);
                originalTextSection = new CustomSection(context);
            }
            else
            {
                dataSection = (textOffset > naturalTextOffset) ? new CustomSection(bytes.SubLength(naturalTextOffset, ((int)textOffset - naturalTextOffset)), context) : new CustomSection(context);
                int numTextEntries = Event.FindNumTextEntries(commandList);
                numTextEntries = (numTextEntries > 0) ? numTextEntries : 1;
                IList<byte> textBytes = bytes.Sub(textOffset);
                //IList<IList<byte>> textByteLists = textBytes.Split((byte)0xFE);
                //IList<string> textList = TextUtility.DecodeList(textBytes);
                //textSection = new CustomSection(textByteLists, textList, numTextEntries);
                //originalTextSection = new CustomSection(textByteLists, textList, textByteLists.Count);
                //textSection = new CustomSection(textByteLists, numTextEntries);
                //originalTextSection = new CustomSection(textByteLists, textByteLists.Count);
                textSection = new CustomSection(textBytes, context, numTextEntries);
                originalTextSection = new CustomSection(textBytes, context, -1);
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

        public List<ConditionalSet> LoadConditionalSetsFromByteArray(CommandType type, IList<byte> bytes)
        {
            int setByteOffset = bytes.ToUInt16LE();
            int numSets = setByteOffset / 2;
            List<ConditionalSet> result = new List<ConditionalSet>(numSets);
            HashSet<int> sentinelCommands = sentinelCommandIDMap[type];

            Dictionary<int, string> setNameMap = entryNameMaps[type];

            UInt16[] setOffsets = bytes.SubLength(0, setByteOffset).ToUInt16ArrayLE();
            int blockByteOffset = bytes.SubLength(setOffsets[0], 2).ToUInt16LE();

            for (int setIndex = 0; setIndex < numSets; setIndex++)
            {
                List<ConditionalBlock> conditionalBlocks = new List<ConditionalBlock>();

                string setName = null;
                setNameMap.TryGetValue(setIndex, out setName);
                if (string.IsNullOrEmpty(setName))
                    setName = "New Set";

                //string setName = setNameMap[setIndex];

                int setStartIndex = setOffsets[setIndex];
                int setEndIndex = ((setIndex < (numSets - 1)) ? setOffsets[setIndex + 1] : blockByteOffset);

                int numBlocks = 0;
                UInt16 prevBlockIndex = 0;
                for (int setByteIndex = setStartIndex; setByteIndex <= setEndIndex; setByteIndex += 2)
                {
                    UInt16 blockIndex = (setByteIndex == setEndIndex) ? (UInt16)0 : bytes.SubLength(setByteIndex, 2).ToUInt16LE();
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

                        ConditionalBlock newBlock = new ConditionalBlock(numBlocks, CommandsFromByteArray(type, bytes.SubLength(startIndex, endIndex - startIndex), sentinelCommands));
                        newBlock.FindName(parameterValueMaps, settings.IgnoreParameterTypes);
                        conditionalBlocks.Add(newBlock);
                        numBlocks++;
                    }
                    
                    prevBlockIndex = blockIndex;
                }

                result.Add(new ConditionalSet(setIndex, setName, conditionalBlocks));
            }

            return result;
        }

        private List<ConditionalSet> LoadConditionalSetDefaults(CommandType type)
        {
            return LoadConditionalSetsFromFile(type, GetDefaultDataFilepath(type));
        }

        private List<ConditionalSet> LoadConditionalSetsFromFile(CommandType type, string filepath)
        {
            return LoadConditionalSetsFromByteArray(type, File.ReadAllBytes(filepath));
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
                int initialValue = bytes.SubLength(index, byteLength).ToIntLE();

                int range = (1 << (parameterTemplate.ByteLength << 3));
                int paramValue = (parameterTemplate.IsSigned && (initialValue > ((range / 2) - 1))) ? -(range - initialValue) : initialValue;

                parameters.Add(new CommandParameter(parameterTemplate, paramValue));
                index += byteLength;
            }

            return new Command(resultTemplate, parameters);
        }

        private HashSet<int> FindSentinelCommandIDs(CommandType type)
        {
            HashSet<int> result = new HashSet<int>();
            Dictionary<int, CommandTemplate> templateMap = commandTemplateMaps[type];
            foreach (CommandTemplate commandTemplate in templateMap.Values)
            {
                if (commandTemplate.IsSentinel)
                    result.Add(commandTemplate.ID);
            }

            return result;
        }
    }
}
