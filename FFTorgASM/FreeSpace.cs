using PatcherLib.Datatypes;
using PatcherLib.Iso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FFTorgASM
{
    public class FreeSpaceMaps
    {
        public Dictionary<PatchedByteArray, AsmPatch> InnerPatchMap { get; set; }
        public Dictionary<PatchRange, List<PatchedByteArray>> PatchRangeMap { get; set; }
        public Dictionary<PatchRange, HashSet<AsmPatch>> OuterPatchRangeMap { get; set; }
    }

    public class FreeSpaceAnalyzeResult
    {
        public bool HasConflicts { get; set; }
        public HashSet<int> ConflictIndexes { get; set; }
        public int LargestGapOffset { get; set; }
        public int LargestGapSize { get; set; }
    }

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

        public static FreeSpaceMaps GetFreeSpaceMaps(IEnumerable<AsmPatch> patches)
        {
            Dictionary<PatchedByteArray, AsmPatch> innerPatchMap = new Dictionary<PatchedByteArray, AsmPatch>();
            Dictionary<PatchRange, List<PatchedByteArray>> patchRangeMap = new Dictionary<PatchRange, List<PatchedByteArray>>();
            Dictionary<PatchRange, HashSet<AsmPatch>> outerPatchRangeMap = new Dictionary<PatchRange, HashSet<AsmPatch>>();

            foreach (AsmPatch patch in patches)
            {
                List<PatchedByteArray> combinedPatchList = patch.GetCombinedPatchList();

                foreach (PatchedByteArray patchedByteArray in combinedPatchList)
                {
                    if (patchedByteArray is InputFilePatch)
                        continue;

                    if (!innerPatchMap.ContainsKey(patchedByteArray))
                        innerPatchMap.Add(patchedByteArray, patch);

                    //long patchedByteArrayEndOffset = patchedByteArray.Offset + patchedByteArray.GetBytes().Length - 1;
                    foreach (PatchRange range in FreeSpace.PsxRanges)
                    {
                        //long positionEndOffset = position.StartLocation + position.Length - 1;
                        //if ((((PsxIso.Sectors)patchedByteArray.Sector) == position.Sector) && (patchedByteArrayEndOffset >= position.StartLocation) && (patchedByteArray.Offset <= positionEndOffset))
                        if (range.HasOverlap(patchedByteArray))
                        {
                            if (patchRangeMap.ContainsKey(range))
                                patchRangeMap[range].Add(patchedByteArray);
                            else
                                patchRangeMap.Add(range, new List<PatchedByteArray>() { patchedByteArray });

                            if (outerPatchRangeMap.ContainsKey(range))
                                outerPatchRangeMap[range].Add(patch);
                            else
                                outerPatchRangeMap.Add(range, new HashSet<AsmPatch>() { patch });
                        }
                    }
                }

                foreach (PatchRange range in FreeSpace.PsxRanges)
                {
                    if (patchRangeMap.ContainsKey(range))
                    {
                        patchRangeMap[range].Sort(
                            delegate(PatchedByteArray patchedByteArray1, PatchedByteArray patchedByteArray2)
                            {
                                return patchedByteArray1.Offset.CompareTo(patchedByteArray2.Offset);
                            }
                        );
                    }
                }
            }

            return new FreeSpaceMaps
            {
                InnerPatchMap = innerPatchMap,
                PatchRangeMap = patchRangeMap,
                OuterPatchRangeMap = outerPatchRangeMap
            };
        }

        public static FreeSpaceAnalyzeResult Analyze(List<PatchedByteArray> innerPatches, PatchRange freeSpaceRange, bool isSorted = true)
        {
            if (!isSorted)
            {
                innerPatches = new List<PatchedByteArray>(innerPatches);
                innerPatches.Sort(
                    delegate(PatchedByteArray patchedByteArray1, PatchedByteArray patchedByteArray2)
                    {
                        return patchedByteArray1.Offset.CompareTo(patchedByteArray2.Offset);
                    }
                );
            }

            FreeSpaceAnalyzeResult result = new FreeSpaceAnalyzeResult();
            result.ConflictIndexes = new HashSet<int>();

            if (innerPatches.Count == 0)
                return result;

            result.LargestGapOffset = (int)freeSpaceRange.StartOffset;
            result.LargestGapSize = (int)(innerPatches[0].Offset - freeSpaceRange.StartOffset);

            int endIndex = innerPatches.Count - 1;
            for (int index = 0; index < endIndex; index++)
            {
                PatchedByteArray patchedByteArray = innerPatches[index];
                int length = patchedByteArray.GetBytes().Length;
                long nextAddress = patchedByteArray.Offset + length;
                long nextPatchLocation = innerPatches[index + 1].Offset;
                long spaceToNextPatch = nextPatchLocation - nextAddress;

                if (spaceToNextPatch > result.LargestGapSize)
                {
                    result.LargestGapOffset = (int)nextAddress;
                    result.LargestGapSize = (int)spaceToNextPatch;
                }

                if ((innerPatches[index + 1].Offset - nextAddress) < 0)
                {
                    if (innerPatches[index].HasConflict(innerPatches[index + 1]))
                    {
                        result.HasConflicts = true;
                        result.ConflictIndexes.Add(index);
                    }
                }
            }

            int finalNextAddress = (int)(innerPatches[endIndex].Offset + innerPatches[endIndex].GetBytes().Length);
            int endGapSize = (int)(freeSpaceRange.EndOffset - finalNextAddress);
            if (endGapSize > result.LargestGapSize)
            {
                result.LargestGapOffset = finalNextAddress;
                result.LargestGapSize = endGapSize;
            }

            return result;
        }
    }
}
