using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryEdit
{
    public class Command
    {
        public CommandTemplate Template { get; set; }
        public List<CommandParameter> Parameters { get; set; }
    }

    public class CommandParameter
    {
        public CommandParameterTemplate Template { get; set; }
        public int Value { get; set; }
    }

    public class CommandTemplate
    {
        public const string DefaultName = "Unknown";

        public int ID { get; set; }
        public string Name { get; set; }
        public int ByteLength { get; set; }
        public CommandType Type { get; set; }
        public List<CommandParameterTemplate> Parameters { get; set; }
    }

    public class CommandParameterTemplate
    {
        public const string DefaultName = "Unknown";

        public string Name { get; set; }
        public int ByteLength { get; set; }
        public bool IsHex { get; set; }
        public bool IsSigned { get; set; }
        public CommandParameterType Type { get; set; }
    }

    public enum CommandType
    {
        BattleConditional = 1,
        WorldConditional = 2,
        EventCommand = 3
    }

    public enum CommandParameterType
    {
        Number = 0,
        Variable = 1,
        Unit = 2,
        Item = 3,
        Scenario = 4,
        Map = 5,
        Location = 6,
        AbilityEffect = 7,
        Spritesheet = 8
    }
}
