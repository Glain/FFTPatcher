using PatcherLib.Iso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib.Datatypes
{
    public class PatchRange
    {
        public int Sector { get; set; }
        public long StartOffset { get; set; }
        public long EndOffset { get; set; }

        public PatchRange(int sector, long startOffset, long endOffset)
        {
            this.Sector = sector;
            this.StartOffset = startOffset;
            this.EndOffset = endOffset;
        }

        public PatchRange(PsxIso.Sectors sector, long startOffset, long endOffset) : this((int)sector, startOffset, endOffset) { }
        public PatchRange(PspIso.Sectors sector, long startOffset, long endOffset) : this((int)sector, startOffset, endOffset) { }
        public PatchRange(PatchedByteArray patchedByteArray): this(patchedByteArray.Sector, patchedByteArray.Offset, (patchedByteArray.Offset + patchedByteArray.GetBytes().Length - 1)) { }

        public bool HasOverlap(PatchRange range)
        {
            return ((Sector == range.Sector) && (EndOffset >= range.StartOffset) && (StartOffset <= range.EndOffset));
        }

        public bool HasOverlap(PatchedByteArray patchedByteArray)
        {
            return HasOverlap(new PatchRange(patchedByteArray));
        }

        public bool IsContainedWithin(PatchRange range)
        {
            return ((Sector == range.Sector) && (StartOffset >= range.StartOffset) && (EndOffset <= range.EndOffset));
        }

        public bool IsContainedWithin(PatchedByteArray patchedByteArray)
        {
            return IsContainedWithin(new PatchRange(patchedByteArray));
        }
    }
}
