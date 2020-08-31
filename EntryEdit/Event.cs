using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace EntryEdit
{
    public class Event
    {
        public List<Command> CommandList { get; set; }
        public CustomSection BetweenSection { get; set; }
        public CustomSection EndSection { get; set; }
    }
}
