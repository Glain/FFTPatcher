using PatcherLib.TextUtilities;
using System.Collections.Generic;

namespace FFTPatcher.TextEditor
{
    public interface IFile
    {
        byte SelectedTerminator { get; set; }
        IList<string> SectionNames { get; }
        IList<IList<string>> EntryNames { get; }
        IList<bool> HiddenEntries { get; }
        GenericCharMap CharMap { get; }
        int NumberOfSections { get; }
        string this[int section, int entry] { get; set; }
        IList<int> SectionLengths { get; }
        string DisplayName { get; }
        PatcherLib.Datatypes.Context Context { get; }
        IList<string> SectionComments { get; }
        string FileComments { get; set; }
    }
}
