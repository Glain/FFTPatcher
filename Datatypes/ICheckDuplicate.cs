using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFTPatcher.Datatypes
{
    public interface ICheckDuplicate<T> : PatcherLib.Datatypes.IChangeable
    {
        bool IsInUse { get; }
        bool IsDuplicate { get; set; }
        int Index { get; set; }
        int DuplicateIndex { get; set; }

        bool CheckDuplicate(T compareObject);
    }
}
