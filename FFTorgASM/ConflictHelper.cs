using PatcherLib.Datatypes;
using PatcherLib.Helpers;
using PatcherLib.Iso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFTorgASM
{
    public class PatchRangeConflict
    {
        public PatchRange Range { get; set; }
        public AsmPatch ConflictPatch { get; set; }
        public int ConflictPatchNumber { get; set; }
        public PatchRange ConflictRange { get; set; }
        public bool IsInFreeSpace { get; set; }

        public PatchRangeConflict(PatchRange range, AsmPatch conflictPatch, PatchRange conflictRange, bool isInFreeSpace)
        {
            this.Range = range;
            this.ConflictPatch = conflictPatch;
            this.ConflictRange = conflictRange;
            this.IsInFreeSpace = isInFreeSpace;
        }
    }

    public class ConflictCheckResult
    {
        public List<AsmPatch> PatchList { get; set; }
        public Dictionary<AsmPatch, List<PatchRangeConflict>> ConflictMap { get; set; }

        private Dictionary<AsmPatch, int> patchNumberMap;
        public Dictionary<AsmPatch, int> PatchNumberMap
        {
            get { return patchNumberMap; }
        }

        public ConflictCheckResult() { }
        public ConflictCheckResult(List<AsmPatch> patchList, Dictionary<AsmPatch, List<PatchRangeConflict>> conflictMap)
        {
            this.PatchList = patchList;
            this.ConflictMap = conflictMap;
            this.patchNumberMap = BuildPatchNumberMap(patchList);
            AddConflictPatchNumbers(conflictMap, this.patchNumberMap);
        }

        private Dictionary<AsmPatch, int> BuildPatchNumberMap(List<AsmPatch> patchList)
        {
            Dictionary<AsmPatch, int> resultMap = new Dictionary<AsmPatch, int>();

            for (int index = 0; index < patchList.Count; index++)
            {
                resultMap.Add(patchList[index], index);
            }

            return resultMap;
        }

        private void AddConflictPatchNumbers(Dictionary<AsmPatch, List<PatchRangeConflict>> conflictMap, Dictionary<AsmPatch, int> patchNumberMap)
        {
            foreach (List<PatchRangeConflict> conflictList in conflictMap.Values)
            {
                AddConflictPatchNumbers(conflictList, patchNumberMap);
            }
        }

        private void AddConflictPatchNumbers(List<PatchRangeConflict> conflictList, Dictionary<AsmPatch, int> patchNumberMap)
        {
            foreach (PatchRangeConflict conflict in conflictList)
            {
                conflict.ConflictPatchNumber = patchNumberMap[conflict.ConflictPatch];
            }
        }
    }

    public class ConflictResolveResult
    {
        public List<AsmPatch> Patches { get; set; }
        public bool HasConflicts { get; set; }
        public string Message { get; set; }

        public ConflictResolveResult(List<AsmPatch> patches)
        {
            this.Patches = patches;
        }

        public ConflictResolveResult(List<AsmPatch> patches, bool hasConflicts)
            : this(patches)
        {
            this.HasConflicts = hasConflicts;
        }

        public ConflictResolveResult(List<AsmPatch> patches, bool hasConflicts, string message)
            : this(patches, hasConflicts)
        {
            this.Message = message;
        }
    }

    public static class ConflictHelper
    {
        public const int MaxConflictResolveAttempts = 100;
        public const int MaxConflictResolveIterations = 1000;

        public static ConflictCheckResult CheckConflicts(IList<AsmPatch> patchList, FreeSpaceMode mode)
        {
            List<AsmPatch> resultPatchList = new List<AsmPatch>();
            Dictionary<AsmPatch, List<PatchRangeConflict>> conflictMap = new Dictionary<AsmPatch, List<PatchRangeConflict>>();
            Dictionary<AsmPatch, List<PatchedByteArray>> combinedPatchListMap = new Dictionary<AsmPatch, List<PatchedByteArray>>();

            foreach (AsmPatch patch in patchList)
            {
                combinedPatchListMap[patch] = patch.GetCombinedPatchList();
            }

            for (int patchIndex = 0; patchIndex < patchList.Count; patchIndex++)
            {
                AsmPatch patch = patchList[patchIndex];

                List<PatchedByteArray> combinedPatchList = combinedPatchListMap[patch];
                foreach (PatchedByteArray patchedByteArray in combinedPatchList)
                {
                    if (patchedByteArray is InputFilePatch)
                        continue;

                    PatchRange range = new PatchRange(patchedByteArray);
                    for (int conflictPatchIndex = patchIndex + 1; conflictPatchIndex < patchList.Count; conflictPatchIndex++)
                    {
                        AsmPatch conflictPatch = patchList[conflictPatchIndex];

                        List<PatchedByteArray> conflictCombinedPatchList = combinedPatchListMap[conflictPatch];
                        foreach (PatchedByteArray conflictPatchedByteArray in conflictCombinedPatchList)
                        {
                            if (conflictPatchedByteArray is InputFilePatch)
                                continue;

                            //if (patchedByteArray.IsPatchEqual(conflictPatchedByteArray))
                            if (!patchedByteArray.HasConflict(conflictPatchedByteArray))
                                continue;

                            PatchRange conflictRange = new PatchRange(conflictPatchedByteArray);
                            if (range.HasOverlap(conflictRange))
                            {
                                bool isInFreeSpace = FreeSpace.IsContainedWithinFreeSpace(range, mode);

                                PatchRangeConflict patchConflict = new PatchRangeConflict(range, conflictPatch, conflictRange, isInFreeSpace);
                                PatchRangeConflict reversePatchConflict = new PatchRangeConflict(conflictRange, patch, range, isInFreeSpace);

                                if (conflictMap.ContainsKey(patch))
                                    conflictMap[patch].Add(patchConflict);
                                else
                                    conflictMap.Add(patch, new List<PatchRangeConflict> { patchConflict });

                                if (conflictMap.ContainsKey(conflictPatch))
                                    conflictMap[conflictPatch].Add(reversePatchConflict);
                                else
                                    conflictMap.Add(conflictPatch, new List<PatchRangeConflict> { reversePatchConflict });
                            }
                        }
                    }
                }

                if (conflictMap.ContainsKey(patch))
                {
                    resultPatchList.Add(patch);
                }
            }

            return new ConflictCheckResult(resultPatchList, conflictMap);
        }

        public static ConflictResolveResult ResolveConflicts(IList<AsmPatch> patchList, ASMEncoding.ASMEncodingUtility asmUtility, 
            int maxConflictResolveAttempts = MaxConflictResolveAttempts, int maxIterations = MaxConflictResolveIterations)
        {
            List<AsmPatch> resultPatchList = new List<AsmPatch>();
            Dictionary<AsmPatch, int> patchIndexMap = new Dictionary<AsmPatch, int>();
            StringBuilder sbMessage = new StringBuilder();
            bool hasConflicts = false;

            for (int index = 0; index < patchList.Count; index++)
            {
                resultPatchList.Add(patchList[index]);
                patchIndexMap.Add(patchList[index], index);
            }

            Context context = (asmUtility.EncodingMode == ASMEncoding.ASMEncodingMode.PSP) ? Context.US_PSP : Context.US_PSX;
            FreeSpaceMode mode = FreeSpace.GetMode(context);
            FreeSpaceMaps freeSpaceMaps = FreeSpace.GetFreeSpaceMaps(resultPatchList, mode);
            foreach (PatchRange freeSpaceRange in freeSpaceMaps.PatchRangeMap.Keys)
            {
                List<PatchedByteArray> innerPatches = freeSpaceMaps.PatchRangeMap[freeSpaceRange];
                FreeSpaceAnalyzeResult analyzeResult = FreeSpace.Analyze(innerPatches, freeSpaceMaps.InnerPatchMap, freeSpaceRange, true);
                int conflictResolveAttempts = 0;
                int conflictResolveIterations = 0;

                /*
                Type sectorType = ISOHelper.GetSectorType(context);
                Enum sector = (Enum)Enum.ToObject(sectorType, freeSpaceRange.Sector);
                string strSector = (mode == FreeSpaceMode.PSP) ? PspIso.GetSectorName((PspIso.Sectors)sector) : PsxIso.GetSectorName((PsxIso.Sectors)sector);
                */

                string strSector = ISOHelper.GetSectorName(freeSpaceRange.Sector, context);

                while ((analyzeResult.HasConflicts) && (conflictResolveAttempts < maxConflictResolveAttempts) && (conflictResolveIterations < maxIterations))
                {
                    bool wasStatic = false;
                    bool isStatic = false;
                    bool stayStatic = false;
                    bool wasConflictIndex = false;
                    bool isConflictIndex = false;

                    int endIndex = innerPatches.Count;  // innerPatches.Count - 1;
                    for (int index = 0; index < endIndex; index++)
                    {
                        PatchedByteArray innerPatch = innerPatches[index];

                        wasStatic = isStatic;
                        isStatic = innerPatch.IsStatic || stayStatic;
                        stayStatic = (innerPatch.IsStatic) && (innerPatch.IsPatchEqual(innerPatches[index + 1]));
                        wasConflictIndex = isConflictIndex;
                        isConflictIndex = analyzeResult.ConflictIndexes.Contains(index);

                        if ((!isStatic) && (isConflictIndex || (wasConflictIndex && wasStatic)))
                        {
                            // Enforce 4-byte alignment
                            int largestGapOffset = analyzeResult.LargestGapOffset + (4 - (analyzeResult.LargestGapOffset % 4)) % 4;
                            long moveOffset = largestGapOffset - innerPatch.Offset;
                            if (analyzeResult.LargestGapSize >= 4)
                            {
                                MovePatchRange movePatchRange = new MovePatchRange(new PatchRange(innerPatch), moveOffset);

                                AsmPatch asmPatch = freeSpaceMaps.InnerPatchMap[innerPatch];
                                int resultPatchIndex = patchIndexMap[asmPatch];
                                patchIndexMap.Remove(asmPatch);

                                asmPatch = asmPatch.Copy();
                                resultPatchList[resultPatchIndex] = asmPatch;
                                patchIndexMap.Add(asmPatch, resultPatchIndex);
                                asmPatch.MoveBlock(asmUtility, movePatchRange);
                                asmPatch.Update(asmUtility);

                                sbMessage.AppendLine("Conflict resolved by moving segment of patch \"" + asmPatch.Name + "\" in sector " + strSector + " from offset "
                                    + innerPatch.Offset.ToString("X") + " to " + largestGapOffset.ToString("X") + ".");

                                freeSpaceMaps = FreeSpace.GetFreeSpaceMaps(resultPatchList, mode);
                                innerPatches = freeSpaceMaps.PatchRangeMap[freeSpaceRange];
                                analyzeResult = FreeSpace.Analyze(innerPatches, freeSpaceMaps.InnerPatchMap, freeSpaceRange, false);
                                conflictResolveAttempts++;
                                break;
                            }
                        }
                    }

                    conflictResolveIterations++;
                }

                if (analyzeResult.HasConflicts)
                {
                    hasConflicts = true;
                    int endIndex = innerPatches.Count - 1;
                    for (int index = 0; index < endIndex; index++)
                    {
                        if (analyzeResult.ConflictIndexes.Contains(index))
                        {
                            sbMessage.Length = 0;
                            sbMessage.AppendLine("Conflict in sector " + strSector + " at offset " + innerPatches[index].Offset.ToString("X") + "!");
                            break;
                        }
                    }
                }
            }

            return new ConflictResolveResult(resultPatchList, hasConflicts, sbMessage.ToString());
        }
    }
}
