using System;
using System.Collections.Generic;
using System.Text;

namespace EntryEdit
{
    public class EntryData
    {
        public List<ConditionalSet> BattleConditionals { get; set; }
        public List<ConditionalSet> WorldConditionals { get; set; }
        public List<Event> Events { get; set; }

        public EntryData(List<ConditionalSet> battleConditionals, List<ConditionalSet> worldConditionals, List<Event> events)
        {
            this.BattleConditionals = battleConditionals;
            this.WorldConditionals = worldConditionals;
            this.Events = events;
        }
    }

    public class EntryBytes
    {
        public byte[] BattleConditionals { get; set; }
        public byte[] WorldConditionals { get; set; }
        public byte[] Events { get; set; }

        public EntryBytes(byte[] battleConditionals, byte[] worldConditionals, byte[] events)
        {
            this.BattleConditionals = battleConditionals;
            this.WorldConditionals = worldConditionals;
            this.Events = events;
        }
    }

    public class Event
    {
        public string Name { get; set; }
        public uint TextOffset { get; set; }
        public List<Command> CommandList { get; set; }
        public CustomSection BetweenSection { get; set; }
        public CustomSection EndSection { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ConditionalSet
    {
        public string Name { get; set; }
        public List<List<Command>> ConditionalBlocks { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
