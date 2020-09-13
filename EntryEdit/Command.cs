using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;

namespace EntryEdit
{
    public class Command : ICopyableEntry<Command>
    {
        public CommandTemplate Template { get; private set; }
        public List<CommandParameter> Parameters { get; private set; }

        public Command(CommandTemplate template, List<CommandParameter> parameters)
        {
            this.Template = template;
            this.Parameters = parameters;
        }

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

        public Command Copy()
        {
            return new Command(Template, CopyableEntry.CopyList<CommandParameter>(Parameters));
        }

        public override string ToString()
        {
            return Template.ToString();
        }
    }

    public class CommandParameter : ICopyableEntry<CommandParameter>
    {
        public CommandParameterTemplate Template { get; private set; }
        public int Value { get; private set; }

        public CommandParameter(CommandParameterTemplate template, int value)
        {
            this.Template = template;
            this.Value = value;
        }

        public int GetByteLength()
        {
            return (Template != null) ? Template.ByteLength : 0;
        }

        public CommandParameter Copy()
        {
            return new CommandParameter(Template, Value);
        }
    }

    public class CommandTemplate
    {
        public const string DefaultName = "Unknown";

        public int ID { get; private set; }
        public string Name { get; private set; }
        public int ByteLength { get; private set; }
        public CommandType Type { get; private set; }
        public List<CommandParameterTemplate> Parameters { get; private set; }

        public CommandTemplate(int id, string name, int byteLength, CommandType type, List<CommandParameterTemplate> parameters)
        {
            this.ID = id;
            this.Name = name;
            this.ByteLength = byteLength;
            this.Type = type;
            this.Parameters = parameters;
        }

        public override string ToString()
        {
            return ID.ToString("X" + (ByteLength * 2)) + " " + Name;
        }
    }

    public class CommandParameterTemplate
    {
        public const string DefaultName = "Unknown";

        public string Name { get; private set; }
        public int ByteLength { get; private set; }
        public bool IsHex { get; private set; }
        public bool IsSigned { get; private set; }
        public bool IsTextReference { get; private set; }
        public string Type { get; private set; }

        public CommandParameterTemplate(string name, int byteLength, bool isHex, bool isSigned, bool isTextReference, string type)
        {
            this.Name = name;
            this.ByteLength = byteLength;
            this.IsHex = isHex;
            this.IsSigned = isSigned;
            this.IsTextReference = isTextReference;
            this.Type = type;
        }
    }

    public class CustomSection : ICopyableEntry<CustomSection>
    {
        public List<CustomEntry> CustomEntryList { get; private set; }

        public CustomSection(List<CustomEntry> customEntryList)
        {
            this.CustomEntryList = customEntryList;
        }

        public CustomSection(CustomEntry customEntry): this(new List<CustomEntry>() { customEntry }) { }
        public CustomSection() : this(new List<CustomEntry>()) { }

        public CustomSection(IList<byte> bytes, bool isText = false, int numTextEntries = 0)
        {
            CustomEntryList = new List<CustomEntry>();

            if (isText)
            {
                IList<IList<byte>> byteLists = bytes.Split((byte)0xFE);
                IList<string> textSection = TextUtility.DecodeList(bytes);
                int numEntries = Math.Min(numTextEntries, byteLists.Count);

                for (int index = 0; index < numEntries; index++)
                {
                    CustomEntryList.Add(new CustomEntry(index, new List<byte>(byteLists[index]), textSection[index]));
                }
            }
            else
            {
                CustomEntryList.Add(new CustomEntry(0, new List<byte>(bytes)));
            }
        }

        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();
            
            foreach (CustomEntry entry in CustomEntryList)
                byteList.AddRange(entry.Bytes);

            return byteList.ToArray();
        }

        public CustomSection Copy()
        {
            return new CustomSection(CopyableEntry.CopyList<CustomEntry>(CustomEntryList));
        }
    }

    public class CustomEntry : ICopyableEntry<CustomEntry>
    {
        public int Index { get; private set; }
        public List<byte> Bytes { get; private set; }
        public string Text { get; private set; }
        public string ASM { get; private set; }

        public CustomEntry(int index, List<byte> bytes, string text, string asm)
        {
            this.Index = index;
            this.Bytes = bytes;
            this.Text = text;
            this.ASM = asm;
        }

        public CustomEntry(int index, List<byte> bytes, string text) : this(index, bytes, text, "") { }
        public CustomEntry(int index, List<byte> bytes) : this(index, bytes, "", "") { }
        public CustomEntry(int index) : this(index, new List<byte>(), "", "") { }

        public CustomEntry Copy()
        {
            return new CustomEntry(Index, new List<byte>(Bytes), Text, ASM);
        }

        public override string ToString()
        {
            return (Index + 1).ToString("X2");
        }

        public void SetText(string text)
        {
            Text = text;
            Bytes = TextUtility.Encode(text);
        }

        public void SetHex(string hex, bool isText = false)
        {
            SetBytes(PatcherLib.Utilities.Utilities.GetBytesFromHexString(hex, true), isText);
        }

        public void SetBytes(IList<byte> bytes, bool isText = false)
        {
            Bytes = new List<byte>(bytes);
            if (isText)
            {
                Text = TextUtility.Decode(bytes);
            }
        }

        public void IncrementIndex()
        {
            Index++;
        }

        public void DecrementIndex()
        {
            Index--;
        }
    }

    public enum CommandType
    {
        BattleConditional = 1,
        WorldConditional = 2,
        EventCommand = 3
    }

    /*
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
    */
}
