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
        public int Index { get; private set; }
        public string Name { get; private set; }
        public uint TextOffset { get; private set; }
        public List<Command> CommandList { get; private set; }
        public CustomSection DataSection { get; private set; }
        public CustomSection TextSection { get; private set; }

        public Event(int index, string name, uint textOffset, List<Command> commandList, CustomSection dataSection, CustomSection textSection)
        {
            this.Index = index;
            this.Name = name;
            this.TextOffset = textOffset;
            this.CommandList = commandList;
            this.DataSection = dataSection;
            this.TextSection = textSection;
        }

        public Event Copy()
        {
            return new Event(Index, Name, TextOffset, CopyableEntry.CopyList<Command>(CommandList), DataSection.Copy(), TextSection.Copy());
        }

        public override string ToString()
        {
            return Index.ToString("X4") + " " + Name;
        }
    }

    public class ConditionalSet : ICopyableEntry<ConditionalSet>
    {
        public int Index { get; private set; }
        public string Name { get; private set; }
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

        public override string ToString()
        {
            return Index.ToString("X2") + " " + Name;
        }
    }

    public class ConditionalBlock : ICopyableEntry<ConditionalBlock>
    {
        public int Index { get; set; }
        public List<Command> Commands { get; set; }

        public ConditionalBlock(int index, List<Command> commands)
        {
            this.Index = index;
            this.Commands = commands;
        }

        public ConditionalBlock Copy()
        {
            return new ConditionalBlock(Index, CopyableEntry.CopyList<Command>(Commands));
        }

        public override string ToString()
        {
            return (Index + 1).ToString();
        }
    }
}
