using System.Collections.Generic;
using PatcherLib.Datatypes;

namespace FFTPatcher.TextEditor
{
    interface ISerializableFile : IFile
    {
        FFTPatcher.TextEditor.FFTTextFactory.FileInfo Layout { get; }
        byte[] ToCDByteArray();
        byte[] ToCDByteArray( IDictionary<string, byte> dteTable );
        Set<KeyValuePair<string, byte>> GetPreferredDTEPairs( Set<string> replacements, Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes );
        Set<KeyValuePair<string, byte>> GetPreferredDTEPairs( Set<string> replacements, Set<KeyValuePair<string, byte>> currentPairs, Stack<byte> dteBytes, System.ComponentModel.BackgroundWorker worker );

        bool IsDteNeeded();

        IList<PatchedByteArray> GetNonDtePatches();
        IList<PatchedByteArray> GetDtePatches( IDictionary<string, byte> dteBytes );
    }
}
