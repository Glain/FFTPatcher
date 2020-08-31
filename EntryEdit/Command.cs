using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using PatcherLib.TextUtilities;

namespace EntryEdit
{
    public class Command
    {
        public CommandTemplate Template { get; set; }
        public List<CommandParameter> Parameters { get; set; }

        public int GetTotalByteLength()
        {
            int result = (Template != null) ? Template.ByteLength : 0;
            if (Parameters != null)
            {
                foreach (CommandParameter parameter in Parameters)
                {
                    result += parameter.GetByteLength();
                }
            }

            return result;
        }
    }

    public class CommandParameter
    {
        public CommandParameterTemplate Template { get; set; }
        public int Value { get; set; }

        public int GetByteLength()
        {
            return (Template != null) ? Template.ByteLength : 0;
        }
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

    public class CustomSection
    {
        public int ByteLength { get; set; }
        public List<CustomEntry> CustomEntryList { get; set; }

        public CustomSection(int byteLength, List<CustomEntry> customEntryList)
        {
            this.ByteLength = byteLength;
            this.CustomEntryList = customEntryList;
        }

        public CustomSection(int byteLength, CustomEntry customEntry): this(byteLength, new List<CustomEntry>() { customEntry }) { }
        public CustomSection() : this(0, new CustomEntry()) { }

        public CustomSection(IList<byte> bytes)
        {
            ByteLength = bytes.Count;
            CustomEntryList = new List<CustomEntry>();

            IList<IList<byte>> byteLists = bytes.Split((byte)0xFE);
            IList<string> textSection = PatcherLib.TextUtilities.TextUtilities.ProcessList(bytes, 0xFE, PatcherLib.TextUtilities.TextUtilities.PSXMap);

            for (int index = 0; index < byteLists.Count; index++)
            {
                CustomEntryList.Add(new CustomEntry(new List<byte>(byteLists[index]), textSection[index]));
            }
        }
    }

    public class CustomEntry
    {
        public List<byte> Bytes { get; set; }
        public string Text { get; set; }
        public string ASM { get; set; }

        public CustomEntry(List<byte> bytes, string text, string asm)
        {
            this.Bytes = bytes;
            this.Text = text;
            this.ASM = asm;
        }

        public CustomEntry(List<byte> bytes, string text) : this(bytes, text, "") { }
        public CustomEntry() : this(new List<byte>(), "", "") { }
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
