using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryEdit
{
    public class EntryData
    {
        public List<List<List<Command>>> BattleConditionals { get; set; }
        public List<List<List<Command>>> WorldConditionals { get; set; }
        public List<Event> Events { get; set; }

        public EntryData(List<List<List<Command>>> battleConditionals, List<List<List<Command>>> worldConditionals, List<Event> events)
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
}
