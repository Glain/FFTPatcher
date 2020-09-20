using System;
using System.Collections.Generic;
using System.Text;

namespace EntryEdit
{
    public interface ICopyableEntry<T>
    {
        T Copy();
    }

    public static class CopyableEntry
    {
        public static List<T> CopyList<T>(List<T> list) where T: ICopyableEntry<T>
        {
            List<T> listCopy = new List<T>(list.Capacity);
            foreach (T entry in list)
            {
                listCopy.Add(entry.Copy());
            }

            return listCopy;
        }
    }

    public class EntryData : ICopyableEntry<EntryData>
    {
        public List<ConditionalSet> BattleConditionals { get; private set; }
        public List<ConditionalSet> WorldConditionals { get; private set; }
        public List<Event> Events { get; private set; }

        public static T GetEntry<T>(IList<T> list, int index) where T: class
        {
            return ((index < list.Count) ? list[index] : null);
        }

        public EntryData(List<ConditionalSet> battleConditionals, List<ConditionalSet> worldConditionals, List<Event> events)
        {
            this.BattleConditionals = battleConditionals;
            this.WorldConditionals = worldConditionals;
            this.Events = events;
        }

        public EntryData Copy()
        {
            return new EntryData(CopyableEntry.CopyList<ConditionalSet>(BattleConditionals), CopyableEntry.CopyList<ConditionalSet>(WorldConditionals),
                CopyableEntry.CopyList<Event>(Events));
        }
    }

    public class EntryBytes : ICopyableEntry<EntryBytes>
    {
        public byte[] BattleConditionals { get; private set; }
        public byte[] WorldConditionals { get; private set; }
        public byte[] Events { get; private set; }

        public EntryBytes(byte[] battleConditionals, byte[] worldConditionals, byte[] events)
        {
            this.BattleConditionals = battleConditionals;
            this.WorldConditionals = worldConditionals;
            this.Events = events;
        }

        public EntryBytes Copy()
        {
            byte[] battleConditionalsCopy = new byte[BattleConditionals.Length];
            byte[] worldConditionalsCopy = new byte[WorldConditionals.Length];
            byte[] eventsCopy = new byte[Events.Length];
            Array.Copy(BattleConditionals, battleConditionalsCopy, BattleConditionals.Length);
            Array.Copy(WorldConditionals, worldConditionalsCopy, WorldConditionals.Length);
            Array.Copy(Events, eventsCopy, Events.Length);
            return new EntryBytes(battleConditionalsCopy, worldConditionalsCopy, eventsCopy);
        }
    }

    public class Event : ICopyableEntry<Event>
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<Command> CommandList { get; private set; }
        public CustomSection DataSection { get; private set; }
        public CustomSection TextSection { get; private set; }
        public CustomSection OriginalTextSection { get; private set; }
        public IList<byte> OriginalBytes { get; private set; }

        public Event(int index, string name, List<Command> commandList, CustomSection dataSection, CustomSection textSection, CustomSection originalTextSection, IList<byte> originalBytes)
        {
            this.Index = index;
            this.Name = name;
            this.CommandList = commandList;
            this.DataSection = dataSection;
            this.TextSection = textSection;
            this.OriginalTextSection = originalTextSection;
            this.OriginalBytes = originalBytes;
        }

        public Event Copy()
        {
            return new Event(Index, Name, CopyableEntry.CopyList<Command>(CommandList), DataSection.Copy(), TextSection.Copy(), OriginalTextSection.Copy(), new List<byte>(OriginalBytes));
        }

        public void AddOffsetToIndex(int offset)
        {
            Index = Index + offset;
        }

        public void IncrementIndex()
        {
            AddOffsetToIndex(1);
        }

        public void DecrementIndex()
        {
            AddOffsetToIndex(-1);
        }

        public override string ToString()
        {
            return Index.ToString("X4") + " " + Name;
        }

        public static int FindNumTextEntries(IEnumerable<Command> commands)
        {
            int maxTextID = 0;
            foreach (Command command in commands)
            {
                foreach (CommandParameter parameter in command.Parameters)
                {
                    short paramValue = unchecked((short)parameter.Value);
                    if ((parameter.Template.IsTextReference) && (paramValue > maxTextID))
                        maxTextID = paramValue;
                }
            }

            return maxTextID;
        }
    }

    public class ConditionalSet : ICopyableEntry<ConditionalSet>
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<ConditionalBlock> ConditionalBlocks { get; private set; }

        public ConditionalSet(int index, string name, List<ConditionalBlock> conditionalBlocks)
        {
            this.Index = index;
            this.Name = name;
            this.ConditionalBlocks = conditionalBlocks;
        }

        public ConditionalSet Copy()
        {
            return new ConditionalSet(Index, Name, CopyableEntry.CopyList<ConditionalBlock>(ConditionalBlocks));
        }

        public void AddOffsetToIndex(int offset)
        {
            Index = Index + offset;
        }

        public void IncrementIndex()
        {
            AddOffsetToIndex(1);
        }

        public void DecrementIndex()
        {
            AddOffsetToIndex(-1);
        }

        public override string ToString()
        {
            return Index.ToString("X2") + " " + Name;
        }
    }

    public class ConditionalBlock : ICopyableEntry<ConditionalBlock>
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<Command> Commands { get; set; }

        public ConditionalBlock(int index, List<Command> commands, string name = null)
        {
            this.Index = index;
            this.Commands = commands;
            this.Name = name ?? string.Empty;
        }

        public ConditionalBlock Copy()
        {
            return new ConditionalBlock(Index, CopyableEntry.CopyList<Command>(Commands), Name);
        }

        public void AddOffsetToIndex(int offset)
        {
            Index = Index + offset;
        }

        public void IncrementIndex()
        {
            AddOffsetToIndex(1);
        }

        public void DecrementIndex()
        {
            AddOffsetToIndex(-1);
        }

        public string FindName(Dictionary<string, Dictionary<int, string>> parameterValueMaps)
        {
            StringBuilder sb = new StringBuilder();

            if ((Commands != null) && (Commands.Count > 0))
            {
                Command lastCommand = Commands[Commands.Count - 1];
                if ((lastCommand != null) && (lastCommand.Template != null) && !string.IsNullOrEmpty(lastCommand.Template.Name))
                {
                    sb.Append(lastCommand.Template.Name);
                    if ((lastCommand.Parameters != null) && (lastCommand.Parameters.Count <= 2))
                    {
                        foreach (CommandParameter parameter in lastCommand.Parameters)
                        {
                            if ((parameter != null) && (parameter.Template != null))
                            {
                                string parameterName = parameter.Value.ToString();

                                if (!string.IsNullOrEmpty(parameter.Template.Type))
                                {
                                    Dictionary<int, string> valueMap = null;
                                    parameterValueMaps.TryGetValue(parameter.Template.Type, out valueMap);
                                    if (valueMap != null)
                                    {
                                        valueMap.TryGetValue(parameter.Value, out parameterName);
                                    }

                                    sb.AppendFormat(" ({0})", parameterName);
                                }
                            }
                        }
                    }
                }
            }

            Name = sb.ToString();
            return Name;
        }

        public override string ToString()
        {
            return (Index + 1).ToString("X2") + " " + Name;
        }
    }
}
