using System.Collections.Generic;

namespace PatcherLib.Datatypes
{
    public interface IPatchableFile : IChangeable
    {
        IList<PatchedByteArray> GetPatches( Context context );
    }
}
