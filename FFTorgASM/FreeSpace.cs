using PatcherLib.Datatypes;
using PatcherLib.Iso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTorgASM
{
    public static class FreeSpace
    {
        private enum PsxFreeSpaceLocation
        {
            BATTLE_BIN = 0,
            WORLD_BIN = 1,
            SCUS_1 = 2,
            SCUS_2 = 3
        }

        public const int PsxNumRanges = 4;

        public static PatchRange[] PsxRanges = new PatchRange[PsxNumRanges] {
            //new PatchRange(PsxIso.Sectors.BATTLE_BIN, 0xEA0E4, 0xF8E00),          // 0xEA0E4 to 0xF8E00 (Length 0x0ED1C)
            new PatchRange(PsxIso.Sectors.BATTLE_BIN, 0xE92AC, 0xFA2DC),            // 0xE92AC to 0xFA2DC (Length 0x11030)
            //new PatchRange(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5E3C8, 0x6D0E4),     // 0x5E3C8 to 0x6D0E4 (Length 0x0ED1C)
            new PatchRange(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5D590, 0x6E5C0),       // 0x5D590 to 0x6E5C0 (Length 0x11030)
            new PatchRange(PsxIso.Sectors.SCUS_942_21, 0x1785C, 0x17B04),           // 0x1785C to 0x17B04 (Length 0x002A8)
            new PatchRange(PsxIso.Sectors.SCUS_942_21, 0x17DC0, 0x18F3C)            // 0x17DC0 to 0x18F3C (Length 0x0117C) 
        };

        public static bool HasPsxFreeSpaceOverlap(PatchRange range)
        {
            foreach (PatchRange freeSpaceRange in PsxRanges)
            {
                if (range.HasOverlap(freeSpaceRange))
                    return true;
            }

            return false;
        }

        public static bool IsContainedWithinPsxFreeSpace(PatchRange range)
        {
            foreach (PatchRange freeSpaceRange in PsxRanges)
            {
                if (range.IsContainedWithin(freeSpaceRange))
                    return true;
            }

            return false;
        }
    }
}
