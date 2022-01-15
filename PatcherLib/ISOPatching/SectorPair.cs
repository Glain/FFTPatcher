using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib.Iso
{
    public class SectorPair
    {
        public Enum Sector { get; private set; }
        public string SectorName { get; private set; }

        public SectorPair(Enum sector, string sectorName)
        {
            this.Sector = sector;
            this.SectorName = sectorName;
        }

        public override string ToString()
        {
            return SectorName;
        }
    }
}
