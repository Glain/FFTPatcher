using PatcherLib.TextUtilities;
using System.Collections.Generic;
using PatcherLib.Datatypes;

namespace FFTPatcher.TextEditor
{
    class CompressibleOneShotFile : AbstractFile
    {
        public CompressibleOneShotFile( GenericCharMap map, FFTTextFactory.FileInfo layout, IList<IList<string>> strings, string fileComments, IList<string> sectionComments )
            : base( map, layout, strings, fileComments, sectionComments, true )
        {
        }

        public CompressibleOneShotFile( GenericCharMap map, FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, IList<byte> bytes, string fileComments, IList<string> sectionComments )
            : base( map, layout, fileComments, sectionComments, true )
        {
            List<IList<string>> sections = new List<IList<string>>( NumberOfSections );
            System.Diagnostics.Debug.Assert( NumberOfSections == 1 );
            System.Text.StringBuilder sbMessage = new System.Text.StringBuilder();
            for ( int i = 0; i < NumberOfSections; i++ )
            {
                GenericCharMap processCharMap = DteAllowed[i] ? map : GetContextCharmap(layout.Context);
                sections.Add(TextUtilities.ProcessList(TextUtilities.Decompress(bytes, bytes, 0), layout.AllowedTerminators, processCharMap));
                if ( sections[i].Count < SectionLengths[i] )
                {
                    string[] newSection = new string[SectionLengths[i]];
                    sections[i].CopyTo( newSection, 0 );

                    //new string[SectionLengths[i] - sections[i].Count].CopyTo( newSection, sections[i].Count );
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
        }

        protected override IList<byte> ToByteArray()
        {
            IList<uint> offsets;
            return Compress( this.Sections, out offsets );
        }

        protected override IList<byte> ToByteArray( IDictionary<string, byte> dteTable )
        {
            IList<uint> offsets;
            return Compress( dteTable, out offsets );
        }
    }
}
