using System;

namespace FFTPatcher.TextEditor
{
    public class InvalidStringException : Exception
    {
        public string Guid { get; private set; }
        public int Section { get; private set; }
        public int Entry { get; private set; }
        public string InvalidString { get; private set; }

        public InvalidStringException( string fileGuid, int section, int entry, string invalidString )
        {
            Guid = fileGuid;
            Section = section;
            Entry = entry;
            InvalidString = invalidString;
        }

        public override string ToString()
        {
            return string.Format( "File {0} had invalid string \"{1}\" at section {2} entry {3}", Guid, InvalidString, Section, Entry );
        }
    }

}
