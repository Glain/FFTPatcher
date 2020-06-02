using System;
using System.Collections.Generic;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System.Text;

namespace FFTPatcher.TextEditor
{
    class PartitionedFile : AbstractFile
    {
        public int PartitionSize { get; private set; }

        public PartitionedFile( GenericCharMap map, FFTTextFactory.FileInfo layout, IList<IList<string>> strings, string fileComments, IList<string> sectionComments )
            : base( map, layout, strings, fileComments, sectionComments, false )
        {
            PartitionSize = layout.Size / NumberOfSections;
        }

        public PartitionedFile( GenericCharMap map, FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, IList<byte> bytes, string fileComments, IList<string> sectionComments )
            : base( map, layout, fileComments, sectionComments, false )
        {
            PartitionSize = layout.Size / NumberOfSections;
            List<IList<string>> sections = new List<IList<string>>( NumberOfSections );
            for ( int i = 0; i < NumberOfSections; i++ )
            {
                sections.Add(TextUtilities.ProcessList(bytes.Sub(i * PartitionSize, (i + 1) * PartitionSize - 1), layout.AllowedTerminators, map));

                if ( sections[i].Count < SectionLengths[i] )
                {
                    string[] newSection = new string[SectionLengths[i]];
                    sections[i].CopyTo( newSection, 0 );
                    new string[SectionLengths[i] - sections[i].Count].CopyTo( newSection, sections[i].Count );
                    sections[i] = newSection;
                }
                else if (sections[i].Count > SectionLengths[i])
                {
                    sections[i] = sections[i].Sub(0, SectionLengths[i] - 1);
                }

                if (layout.AllowedTerminators.Count > 1)
                {
                    Dictionary<byte, int> counts = new Dictionary<byte, int>();
                    layout.AllowedTerminators.ForEach(b => counts[b] = 0);

                    bytes.FindAll(b => layout.AllowedTerminators.Contains(b)).ForEach(b => counts[b]++);
                    List<KeyValuePair<byte, int>> countList = new List<KeyValuePair<byte, int>>(counts);
                    countList.Sort((a, b) => b.Value.CompareTo(a.Value));
                    this.SelectedTerminator = countList[0].Key;
                }

                System.Diagnostics.Debug.Assert(sections[i].Count == SectionLengths[i]);
            }
            Sections = sections.AsReadOnly();
            PopulateDisallowedSections();
        }

        private Set<KeyValuePair<string, byte>> GetPreferredDTEPairsForSection(IList<IList<string>> allSections, int index, Set<string> replacements, Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes)
        {
            IList<IList<string>> secs = new List<IList<string>>();
            allSections.ForEach( ls => secs.Add( new List<string>( ls ) ) );

            var bytes = GetSectionByteArrays( secs, SelectedTerminator, CharMap, CompressionAllowed );
            IList<byte> ourBytes = bytes[index];

            Set<KeyValuePair<string, byte>> result = new Set<KeyValuePair<string, byte>>();

            int bytesNeeded = ourBytes.Count - this.PartitionSize;
            if (bytesNeeded <= 0)
            {
                return result;
            }

            result.AddRange( currentPairs );

            TextUtilities.DoDTEEncoding( secs[index], Utilities.DictionaryFromKVPs( currentPairs ) );
            bytes = GetSectionByteArrays( secs, SelectedTerminator, CharMap, CompressionAllowed );
            ourBytes = bytes[index];
            bytesNeeded = ourBytes.Count - this.PartitionSize;
            if (bytesNeeded <= 0)
            {
                return result;
            }

            string terminatorString = string.Format( "{{0x{0:X2}", SelectedTerminator ) + "}";

            // Get the pair counts for the string AS IT EXISTS AFTER ENCODING WITH currentPairs
            StringBuilder sb = new StringBuilder( PartitionSize );
            if (DteAllowed[index])
            {
                secs[index].ForEach( t => sb.Append( t ).Append( terminatorString ) );
            }

            var dict = TextUtilities.GetPairAndTripleCounts(sb.ToString(), replacements);

            var l = new List<KeyValuePair<string, int>>(dict);
            l.Sort((a, b) => b.Value.CompareTo(a.Value));



            while (bytesNeeded > 0 && l.Count > 0 && dteBytes.Count > 0)
            {
                // Start with a fresh set of strings for DTE
                secs = new List<IList<string>>();
                allSections.ForEach( ls => secs.Add( new List<string>( ls ) ) );

                result.Add( new KeyValuePair<string, byte>( l[0].Key, dteBytes.Pop() ) );
                TextUtilities.DoDTEEncoding(secs, DteAllowed, PatcherLib.Utilities.Utilities.DictionaryFromKVPs(result));

                bytes = GetSectionByteArrays(secs, SelectedTerminator, CharMap, CompressionAllowed);

                ourBytes = bytes[index];
                bytesNeeded = ourBytes.Count - PartitionSize;

                if (bytesNeeded > 0)
                {
                    // Get the pair counts for the string AS IT EXISTS AFTER ENCODING WITH currentPairs
                    StringBuilder sb2 = new StringBuilder(PartitionSize);
                    if (DteAllowed[index])
                    {
                        secs[index].ForEach(t => sb2.Append(t).Append(terminatorString));
                    }
                    l = new List<KeyValuePair<string, int>>(TextUtilities.GetPairAndTripleCounts(sb2.ToString(), replacements));
                    l.Sort((a, b) => b.Value.CompareTo(a.Value));
                }
            }


            if (bytesNeeded > 0)
            {
                return null;
            }
            return result;
        }

        public override Set<KeyValuePair<string, byte>> GetPreferredDTEPairs(Set<string> replacements, Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes)
        {
            Set<KeyValuePair<string, byte>> result = new Set<KeyValuePair<string, byte>>();
            Set<KeyValuePair<string, byte>> ourCurrentPairs = new Set<KeyValuePair<string, byte>>(currentPairs);
            for (int i = 0; i < Sections.Count; i++)
            {
                Set<KeyValuePair<string, byte>> dtePairs = GetPreferredDTEPairsForSection(GetCopyOfSections(), i, replacements, ourCurrentPairs, dteBytes);
                if (dtePairs == null)
                {
                    return null;
                }
                else
                {
                    result.AddRange(dtePairs);
                    ourCurrentPairs.AddRange(result);
                }
            }
            return result;
        }

        public override Set<KeyValuePair<string, byte>> GetPreferredDTEPairs( Set<string> replacements, Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes, System.ComponentModel.BackgroundWorker worker )
        {
            return GetPreferredDTEPairs( replacements, currentPairs, dteBytes );
        }

        protected override IList<byte> ToByteArray()
        {
            List<byte> result = new List<byte>( Layout.Size );
            foreach ( IList<string> section in Sections )
            {
                List<byte> currentPart = new List<byte>( PartitionSize );
                section.ForEach( s => currentPart.AddRange( CharMap.StringToByteArray( s, SelectedTerminator ) ) );
                currentPart.AddRange( new byte[Math.Max( PartitionSize - currentPart.Count, 0 )] );
                result.AddRange( currentPart );
            }

            return result.AsReadOnly();
        }

        protected override IList<byte> ToByteArray( IDictionary<string, byte> dteTable )
        {
            // Clone the sections
            var secs = GetCopyOfSections();
            TextUtilities.DoDTEEncoding(secs, DteAllowed, dteTable);
            List<byte> result = new List<byte>(Layout.Size);
            foreach (IList<string> section in secs)
            {
                List<byte> currentPart = new List<byte>(PartitionSize);
                section.ForEach(s => currentPart.AddRange(CharMap.StringToByteArray(s, SelectedTerminator)));
                if (currentPart.Count > PartitionSize)
                {
                    return null;
                }
                currentPart.AddRange(new byte[Math.Max(PartitionSize - currentPart.Count, 0)]);
                result.AddRange(currentPart);
            }

            if (result.Count > Layout.Size)
            {
                return null;
            }

            return result.AsReadOnly();
        }
    }
}
