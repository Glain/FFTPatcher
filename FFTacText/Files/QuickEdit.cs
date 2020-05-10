using System;
using System.Collections.Generic;
using PatcherLib.Utilities;

namespace FFTPatcher.TextEditor.Files
{
    class QuickEdit : IFile
    {
        byte IFile.SelectedTerminator { get { return 0xFE; } set { throw new NotSupportedException(); } }
        public IList<string> SectionNames { get; private set; }
        public IList<IList<string>> EntryNames { get; private set; }
        public IList<bool> HiddenEntries { get; private set; }

        public GenericCharMap CharMap { get; private set; }

        public int NumberOfSections { get; private set; }

        public string this[int section, int entry]
        {
            get { return sections[section][entry]; }
            set
            {
                sections[section][entry] = value;
                IList<QuickEditEntry> needToUpdate = lookup[sectionTypes[section]];
                foreach (var v in needToUpdate)
                {
                    files[v.Guid][v.Section, entry] = value;
                }
            }
        }

        // Only updates QuickEdit entry and does not percolate changes to other files.
        public void UpdateEntry(int section, int entry, string value)
        {
            sections[section][entry] = value;
        }

        public IList<int> SectionLengths { get; private set; }

        private IList<IList<string>> sections;
        private Dictionary<SectionType, IList<QuickEditEntry>> lookup;
        private IList<SectionType> sectionTypes;
        private Dictionary<Guid, ISerializableFile> files;
        public PatcherLib.Datatypes.Context Context { get; private set; }

        public QuickEdit( PatcherLib.Datatypes.Context context, IDictionary<Guid, ISerializableFile> files, IDictionary<SectionType, IList<QuickEditEntry>> sections )
        {
            Context = context;
            this.files = new Dictionary<Guid, ISerializableFile>( files );
            lookup = new Dictionary<SectionType, IList<QuickEditEntry>>( sections );

            List<IList<string>> sections2 = new List<IList<string>>( sections.Count );
            List<IList<string>> entryNames = new List<IList<string>>( sections.Count );
            List<SectionType> sectionTypes = new List<SectionType>( sections.Count );
            List<int> sectionLengths = new List<int>( sections.Count );
            List<string> sectionNames = new List<string>();
            HiddenEntries = new bool[sections.Count].AsReadOnly();

            foreach (KeyValuePair<SectionType, IList<QuickEditEntry>> kvp in sections)
            {
                CharMap = CharMap ?? files[kvp.Value[0].Guid].CharMap;

                IList<QuickEditEntry> entries = kvp.Value;
                QuickEditEntry mainEntry = entries.FindAll( e => e.Main )[0];
                ISerializableFile mainFile = files[mainEntry.Guid];
                int entryCount = mainEntry.Length;
                List<string> names = new List<string>( entryCount );
                List<string> values = new List<string>( entryCount );
                for (int i = mainEntry.Offset; i < (mainEntry.Offset + entryCount); i++)
                {
                	names.Add( mainFile.EntryNames[mainEntry.Section][i] );
                    values.Add( mainFile[mainEntry.Section, i] );
                }
                entryNames.Add( names.AsReadOnly() );
                sections2.Add( values.ToArray() );
                sectionLengths.Add( entryCount );
                sectionTypes.Add( kvp.Key );
                sectionNames.Add( FormatName(kvp.Key.ToString()) );
            }

            this.sections = sections2.AsReadOnly();
            EntryNames = entryNames.AsReadOnly();
            NumberOfSections = sections.Count;
            this.sectionTypes = sectionTypes.AsReadOnly();
            this.SectionNames = sectionNames.AsReadOnly();
            this.SectionLengths = sectionLengths.AsReadOnly();
        }

        public struct QuickEditEntry
        {
            public Guid Guid { get; set; }
            public bool Main { get; set; }
            public int Section { get; set; }
            public int Offset { get; set; }
            public int Length { get; set; }
        }

        public string DisplayName
        {
            get { return "QuickEdit"; }
        }

        IList<string> IFile.SectionComments
        {
            get { return new DummyList<string>(); }
        }

        string IFile.FileComments
        {
            get { return string.Empty; }
            set { }
        }

        public string FormatName(string name)
        {
        	string result = "";
        	char oldC = '\0';
        	foreach (char c in name)
        	{
        		if (oldC == 0)
        			result += c;
        		else if ((c >= 48) && (c <= 57))
        			result += " " + c;
        		else if (((c >= 65) && (c <= 90)) && (!((oldC >= 65) && (oldC <= 90))))
        			result += " " + c;
        		else
        			result += c;
        		
        		oldC = c;
        	}
        	return result;
        }
        
        private class DummyList<T> : IList<T>
        {
            public int IndexOf( T item )
            {
                return -1;
            }

            public void Insert( int index, T item )
            {
            }

            public void RemoveAt( int index )
            {
            }

            public T this[int index]
            {
                get
                {
                    return default( T );
                }
                set
                {
                }
            }

            public void Add( T item )
            {
            }

            public void Clear()
            {
            }

            public bool Contains( T item )
            {
                return false;
            }

            public void CopyTo( T[] array, int arrayIndex )
            {
            }

            public int Count
            {
                get { return 0; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove( T item )
            {
                return false;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return (new T[0] as IList<T>).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return (System.Collections.IEnumerator)GetEnumerator();
            }
        }
    }
}