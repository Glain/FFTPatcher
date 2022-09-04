using System;
using System.Text;

namespace FFTPatcher.TextEditor
{
    public class InvalidStringException : Exception
    {
        public string LastErrorChar { get; private set; }
        public string FileName { get; private set; }
        public string InvalidString { get; private set; }
        public int Section { get; private set; }
        public int Entry { get; private set; }

        public InvalidStringException(string lastErrorChar, string fileName, string invalidString, int section, int entry) : 
            base(GetMessage(lastErrorChar, fileName, invalidString, section, entry))
        {
            SetState(lastErrorChar, fileName, invalidString, section, entry);
        }

        private void SetState(string lastErrorChar, string fileName, string invalidString, int section, int entry)
        {
            LastErrorChar = lastErrorChar;
            FileName = fileName;
            InvalidString = invalidString;
            Section = section;
            Entry = entry;
        }

        private static string GetMessage(string lastErrorChar, string fileName, string invalidString, int section, int entry)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Invalid text string.");
            sb.AppendLine();
            sb.AppendFormat("Character: {0}", lastErrorChar);
            sb.AppendLine();
            sb.AppendFormat("Text: {0}", invalidString);
            sb.AppendLine();
            sb.AppendFormat("File: {0}", fileName);
            sb.AppendLine();
            sb.AppendFormat("Section: {0}", section.ToString());
            sb.AppendLine();
            sb.AppendFormat("Entry: {0}", entry.ToString());
            sb.AppendLine();
            return sb.ToString();

            //return string.Format("File {0} has invalid string \"{1}\" at (Section {2}, Entry {3})", fileName, invalidString, section, entry);
        }

        public override string ToString()
        {
            return GetMessage(LastErrorChar, FileName, InvalidString, Section, Entry);
        }
    }
}
