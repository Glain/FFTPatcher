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

        public Command(int commandID, int byteLength, CommandType type)
        {
            Template = new CommandTemplate(commandID, byteLength, type);
            LoadTemplateParameters(Template);
        }

        public Command(CommandTemplate template)
        {
            this.Template = template;
            LoadTemplateParameters(template);
        }

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

        public Command CopyWithValues(IList<int> values)
        {
            if (values.Count != Parameters.Count)
                return Copy();

            List<CommandParameter> parameters = new List<CommandParameter>();
            for (int index = 0; index < Parameters.Count; index++)
            {
                parameters.Add(Parameters[index].CopyWithValue(values[index]));
            }

            return new Command(Template, parameters);
        }

        public override string ToString()
        {
            return Template.ToString();
        }

        private void LoadTemplateParameters(CommandTemplate template)
        {
            this.Parameters = new List<CommandParameter>(template.Parameters.Capacity);
            foreach (CommandParameterTemplate parameterTemplate in template.Parameters)
            {
                this.Parameters.Add(new CommandParameter(parameterTemplate));
            }
        }

        public bool Equals(Command command)
        {
            if ((Template == null) || (command.Template == null))
                return false;
            else if (Template.Type != command.Template.Type)
                return false;
            else if (Template.ID != command.Template.ID)
                return false;
            else if ((Template.Parameters == null) || (command.Template.Parameters == null))
                return false;
            else if (Template.Parameters.Count != command.Template.Parameters.Count)
                return false;

            for (int index = 0; index < Parameters.Count; index++)
            {
                if ((Parameters[index].Template == null) || (command.Parameters[index].Template == null))
                    return false;
                else if (Parameters[index].Value != command.Parameters[index].Value)
                    return false;
            }

            return true;
        }
    }

    public class CommandParameter : ICopyableEntry<CommandParameter>
    {
        public CommandParameterTemplate Template { get; private set; }
        public int Value { get; private set; }

        public CommandParameter(CommandParameterTemplate template)
        {
            this.Template = template;
            this.Value = template.DefaultValue;
        }

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

        public CommandParameter CopyWithValue(int value)
        {
            return new CommandParameter(Template, value);
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

        public CommandTemplate(int commandID, int byteLength, CommandType type) : this(commandID, "Unknown", byteLength, type, new List<CommandParameterTemplate>()) { }

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
        public int DefaultValue { get; private set; }

        public CommandParameterTemplate(string name, int byteLength, bool isHex, bool isSigned, bool isTextReference, string type, int defaultValue)
        {
            this.Name = name;
            this.ByteLength = byteLength;
            this.IsHex = isHex;
            this.IsSigned = isSigned;
            this.IsTextReference = isTextReference;
            this.Type = type;
            this.DefaultValue = defaultValue;
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

        public CustomSection(IList<byte> bytes)
        {
            CustomEntryList = new List<CustomEntry>() { new CustomEntry(0, new List<byte>(bytes)) };
        }

        public CustomSection(IList<IList<byte>> byteLists, IList<string> textList, int numTextEntries)
        {
            CustomEntryList = new List<CustomEntry>();

            if (textList != null)
            {
                int numEntries = Math.Min(numTextEntries, byteLists.Count);

                for (int index = 0; index < numEntries; index++)
                {
                    CustomEntryList.Add(new CustomEntry(index, new List<byte>(byteLists[index]), textList[index]));
                }
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
