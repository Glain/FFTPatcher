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
        public uint StartOffset { get; set; }
        public uint EndOffset { get; set; }

        public PatchRange(int sector, uint startOffset, uint endOffset)
        {
            this.Sector = sector;
            this.StartOffset = startOffset;
            this.EndOffset = endOffset;
        }

        public PatchRange(PsxIso.Sectors sector, uint startOffset, uint endOffset) : this((int)sector, startOffset, endOffset) { }
        public PatchRange(PspIso.Sectors sector, uint startOffset, uint endOffset) : this((int)sector, startOffset, endOffset) { }
        public PatchRange(PatchedByteArray patchedByteArray) : this(patchedByteArray.Sector, (uint)patchedByteArray.Offset, (uint)(patchedByteArray.Offset + patchedByteArray.GetBytes().Length - 1)) { }
        public PatchRange(PatchRange range) : this(range.Sector, range.StartOffset, range.EndOffset) { }

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

    public class MovePatchRange : PatchRange
    {
        public long MoveOffset { get; set; }

        public MovePatchRange(int sector, uint startOffset, uint endOffset, long moveOffset)
            : base(sector, startOffset, endOffset)
        {
            this.MoveOffset = moveOffset;
        }

        public MovePatchRange(PatchRange range, long moveOffset)
            : base(range)
        {
            this.MoveOffset = moveOffset;
        }
    }
}
