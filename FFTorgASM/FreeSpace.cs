using PatcherLib.Datatypes;
using PatcherLib.Iso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        private const string _xmlFilename = "FreeSpace.xml";

        public static string[] PsxRangeNames = new string[4] {
            "BATTLE.BIN",
            "WORLD.BIN",
            "SCUS 1",
            "SCUS 2"
        };

        public static PatchRange[] PsxRanges = new PatchRange[4] {
            //new PatchRange(PsxIso.Sectors.BATTLE_BIN, 0xEA0E4, 0xF8E00),          // 0xEA0E4 to 0xF8E00 (Length 0x0ED1C)
            new PatchRange(PsxIso.Sectors.BATTLE_BIN, 0xE92AC, 0xFA2DC),            // 0xE92AC to 0xFA2DC (Length 0x11030)
            //new PatchRange(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5E3C8, 0x6D0E4),     // 0x5E3C8 to 0x6D0E4 (Length 0x0ED1C)
            new PatchRange(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5D590, 0x6E5C0),       // 0x5D590 to 0x6E5C0 (Length 0x11030)
            new PatchRange(PsxIso.Sectors.SCUS_942_21, 0x1785C, 0x17B04),           // 0x1785C to 0x17B04 (Length 0x002A8)
            new PatchRange(PsxIso.Sectors.SCUS_942_21, 0x17DC0, 0x18F3C)            // 0x17DC0 to 0x18F3C (Length 0x0117C) 
        };

        public static void ReadFreeSpaceXML(string xmlFilename = _xmlFilename)
        {
            if (File.Exists(xmlFilename))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilename);

                List<PatchRange> newPsxRanges = new List<PatchRange>();
                List<string> newPsxRangeNames = new List<string>();

                XmlNode rootNode = xmlDoc.SelectSingleNode("//FreeSpace");
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    XmlAttribute attrName = node.Attributes["name"];
                    XmlAttribute attrSector = node.Attributes["sector"];
                    XmlAttribute attrStartOffset = node.Attributes["startOffset"];
                    XmlAttribute attrEndOffset = node.Attributes["endOffset"];

                    string name = attrName.InnerText;

                    int sector = 0;
                    string sectorText = attrSector.InnerText;
                    if (!int.TryParse(sectorText, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out sector))
                        sector = (int)(PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), sectorText);
                    uint startOffset = uint.Parse(attrStartOffset.InnerText, System.Globalization.NumberStyles.HexNumber);
                    uint endOffset = uint.Parse(attrEndOffset.InnerText, System.Globalization.NumberStyles.HexNumber);

                    newPsxRangeNames.Add(name);
                    newPsxRanges.Add(new PatchRange(sector, startOffset, endOffset));
                }

                PsxRangeNames = newPsxRangeNames.ToArray();
                PsxRanges = newPsxRanges.ToArray();
            }
        }

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
