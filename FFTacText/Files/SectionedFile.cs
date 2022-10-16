using System;
using System.Collections.Generic;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using PatcherLib.TextUtilities;

namespace FFTPatcher.TextEditor
{
    class SectionedFile : AbstractFile
    {
        private const int dataStart = 0x80;

        protected override int DataStart { get { return dataStart; } }

        public SectionedFile( GenericCharMap map, FFTTextFactory.FileInfo layout, IList<IList<string>> strings, string fileComments, IList<string> sectionComments )
            : this( map, layout, strings, fileComments, sectionComments, false )
        {
        }

        public SectionedFile(GenericCharMap map, FFTTextFactory.FileInfo layout, IList<IList<string>> strings, string fileComments, IList<string> sectionComments, bool compressed) :
            base( map, layout, strings, fileComments, sectionComments, compressed )
        {
        }

        public SectionedFile(GenericCharMap map, FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, IList<byte> bytes, string fileComments, IList<string> sectionComments)
            : this( map, layout, bytes, fileComments, sectionComments, false )
        {
        }

        public SectionedFile(GenericCharMap map, FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, IList<byte> bytes, string fileComments, IList<string> sectionComments, bool compressible)
            : base( map, layout, fileComments, sectionComments, compressible )
        {
            List<IList<string>> sections = new List<IList<string>>( NumberOfSections );
            System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
            for ( int i = 0; i < NumberOfSections; i++ )
            {
                uint start = PatcherLib.Utilities.Utilities.BytesToUInt32( bytes.Sub( i * 4, ( i + 1 ) * 4 - 1 ) );
                uint stop = PatcherLib.Utilities.Utilities.BytesToUInt32( bytes.Sub( ( i + 1 ) * 4, ( i + 2 ) * 4 - 1 ) ) - 1;
                if ( i == NumberOfSections - 1 )
                {
                    stop = (uint)bytes.Count - 1 - (uint)DataStart;
                }
                IList<byte> thisSection = bytes.Sub( (int)( start + DataStart ), (int)( stop + DataStart ) );
                if ( compressible )
                {
                    thisSection = TextUtilities.Decompress( bytes, thisSection, (int)( start + DataStart ) );
                }
                GenericCharMap processCharMap = DteAllowed[i] ? CharMap : GetContextCharmap(layout.Context);
                //sections.Add(TextUtilities.ProcessList(thisSection, layout.AllowedTerminators[0], processCharMap));
                sections.Add(TextUtilities.ProcessList(thisSection, layout.AllowedTerminators, processCharMap));
                if (sections[i].Count < SectionLengths[i])
                {
                    string[] newSection = new string[SectionLengths[i]];
                    sections[i].CopyTo(newSection, 0);

                    //new string[SectionLengths[i] - sections[i].Count].CopyTo(newSection, sections[i].Count);
                    for (int stringIndex = sections[i].Count; stringIndex < SectionLengths[i]; stringIndex++)
                        newSection[stringIndex] = string.Empty;

                    sections[i] = newSection;
                }
                else if (sections[i].Count > SectionLengths[i])
                {
                    if ((sections[i].Count - SectionLengths[i]) > 1)
                        sbMessage.AppendLine(string.Format("File {0} (section {1}): Section length decreased from {2} to {3}.", layout.DisplayName, i, sections[i].Count, SectionLengths[i]));

                    sections[i] = sections[i].Sub(0, SectionLengths[i] - 1);
                }
            }

            // <DEBUG>
            //string message = sbMessage.ToString();
            //if (!string.IsNullOrEmpty(message))
            //    PatcherLib.MyMessageBox.Show(message);

            Sections = sections.AsReadOnly();
            PopulateDisallowedSections();
        }

        protected override IList<byte> ToByteArray()
        {
            if ( Compressible )
            {
                IList<UInt32> offsets;
                IList<byte> bytes = Compress( out offsets );
                List<byte> result = new List<byte>( DataStart + bytes.Count );
                result.AddRange( BuildHeaderFromSectionOffsets( offsets ) );
                result.AddRange( bytes );
                return result.AsReadOnly();
            }
            else
            {
                int numberOfSections = Sections.Count;
                List<byte> result = new List<byte>();
                result.AddRange( new byte[] { 0x00, 0x00, 0x00, 0x00 } );
                int old = 0;
                IList<IList<byte>> bytes = GetUncompressedSectionBytes();
                for ( int i = 0; i < numberOfSections - 1; i++ )
                {
                    result.AddRange( ( (UInt32)( bytes[i].Count + old ) ).ToBytes() );
                    old += bytes[i].Count;
                }
                result.AddRange( new byte[Math.Max( DataStart - numberOfSections * 4, 0 )] );
                bytes.ForEach( b => result.AddRange( b ) );
                return result.AsReadOnly();
            }


        }


        private static IList<byte> BuildHeaderFromSectionOffsets( IList<UInt32> offsets )
        {
            List<byte> result = new List<byte>( dataStart );
            offsets.ForEach( o => result.AddRange( o.ToBytes() ) );
            while ( result.Count < dataStart )
            {
                result.Add( 0x00 );
            }

            return result.Sub( 0, dataStart - 1 ).AsReadOnly();
        }

        protected override IList<byte> ToByteArray( IDictionary<string, byte> dteTable )
        {
            if ( Compressible )
            {
                IList<UInt32> offsets;
                IList<byte> bytes = Compress( dteTable, out offsets );
                List<byte> result = new List<byte>( DataStart + bytes.Count );
                result.AddRange( BuildHeaderFromSectionOffsets( offsets ) );
                result.AddRange( bytes );
                return result.AsReadOnly();
            }
            else
            {
                int numberOfSections = Sections.Count;
                List<byte> result = new List<byte>();
                result.AddRange( new byte[] { 0x00, 0x00, 0x00, 0x00 } );
                int old = 0;
                IList<IList<byte>> bytes = GetUncompressedSectionBytes( GetDteStrings( dteTable ), SelectedTerminator, CharMap );
                for ( int i = 0; i < numberOfSections - 1; i++ )
                {
                    result.AddRange( ( (UInt32)( bytes[i].Count + old ) ).ToBytes() );
                    old += bytes[i].Count;
                }
                result.AddRange( new byte[Math.Max( DataStart - numberOfSections * 4, 0 )] );
                bytes.ForEach( b => result.AddRange( b ) );
                return result.AsReadOnly();
            }
        }

    }
}