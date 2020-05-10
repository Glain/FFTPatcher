using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ExtensionMethods;
using PatcherLib.Iso;
using PatcherLib.Datatypes;

namespace FFTorgASM
{
    public partial class ConflictCheckerForm : Form
    {
        internal class PatchRangeConflict
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

        internal class ConflictCheckResult
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

        private Color backColor_Normal = Color.White;
        private Color backColor_Conflict_NonFreeSpace = Color.FromArgb(225, 125, 125);
        private Color backColor_Conflict_FreeSpace = Color.White; // Color.FromArgb(137, 216, 166);

        private List<AsmPatch> origPatchList = null;
        private ConflictCheckResult conflictPatchData = null;
        private List<Color> patchColors = null;
        private Random rng = new Random();

        public ConflictCheckerForm(List<AsmPatch> patchList)
        {
            InitializeComponent();
            this.origPatchList = patchList;
            this.conflictPatchData = CheckConflicts(patchList);
            FindPatchColors(this.conflictPatchData.PatchList);
            ShowPatches(this.conflictPatchData);
        }

        private ConflictCheckResult CheckConflicts(List<AsmPatch> patchList)
        {
            List<AsmPatch> resultPatchList = new List<AsmPatch>();
            Dictionary<AsmPatch, List<PatchRangeConflict>> conflictMap = new Dictionary<AsmPatch, List<PatchRangeConflict>>();

            for (int patchIndex = 0; patchIndex < patchList.Count; patchIndex++)
            {
                AsmPatch patch = patchList[patchIndex];

                foreach (PatchedByteArray patchedByteArray in patch)
                {
                    if (patchedByteArray is InputFilePatch)
                        continue;

                    PatchRange range = new PatchRange(patchedByteArray);
                    for (int conflictPatchIndex = patchIndex + 1; conflictPatchIndex < patchList.Count; conflictPatchIndex++)
                    {
                        AsmPatch conflictPatch = patchList[conflictPatchIndex];

                        foreach (PatchedByteArray conflictPatchedByteArray in conflictPatch)
                        {
                            if (conflictPatchedByteArray is InputFilePatch)
                                continue;

                            if (patchedByteArray.IsPatchEqual(conflictPatchedByteArray))
                                continue;

                            PatchRange conflictRange = new PatchRange(conflictPatchedByteArray);
                            if (range.HasOverlap(conflictRange))
                            {
                                bool isInFreeSpace = FreeSpace.HasPsxFreeSpaceOverlap(range);

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

        private void FindPatchColors(List<AsmPatch> patchList)
        {
            patchColors = new List<Color>();
            for (int index = 0; index < patchList.Count; index++)
            {
                patchColors.Add(Color.FromArgb(rng.Next(100, 256), rng.Next(100, 256), rng.Next(100, 256)));
            }
        }

        private void ShowPatches(ConflictCheckResult conflictPatchData)
        {
            lv_Patches.Items.Clear();
            for (int patchIndex = 0; patchIndex < conflictPatchData.PatchList.Count; patchIndex++)
            {
                AsmPatch patch = conflictPatchData.PatchList[patchIndex];
                int leastConflictPatchIndex = GetLeastConflictPatchIndex(patchIndex, conflictPatchData.ConflictMap[patch]);

                // Patch #, File, Name
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = patchIndex.ToString();
                listViewItem.SubItems.Add(patch.Filename);
                listViewItem.SubItems.Add(patch.Name);
                listViewItem.BackColor = patchColors[leastConflictPatchIndex];
                lv_Patches.Items.Add(listViewItem);
            }

            lv_Patches_columnHeader_Name.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void ShowSelectedConflicts()
        {
            lv_Conflicts.Items.Clear();
            if (lv_Patches.SelectedIndices.Count > 0)
            {
                int patchIndex = lv_Patches.SelectedIndices[0];
                ShowConflicts(conflictPatchData.PatchList[patchIndex]); 
            }
        }

        private void ShowConflicts(AsmPatch patch)
        {
            List<PatchRangeConflict> conflictList = conflictPatchData.ConflictMap[patch];
            foreach (PatchRangeConflict conflict in conflictList)
            {
                PsxIso.Sectors sector = (PsxIso.Sectors)(conflict.ConflictRange.Sector);
                string strSector = Enum.GetName(typeof(PsxIso.Sectors), sector);

                // Patch #, Sector, Location, Conflict Location
                ListViewItem item = new ListViewItem();
                item.Text = conflict.ConflictPatchNumber.ToString();
                item.SubItems.Add(strSector);
                item.SubItems.Add(conflict.Range.StartOffset.ToString("X"));
                item.SubItems.Add(conflict.ConflictRange.StartOffset.ToString("X"));
                item.BackColor = conflict.IsInFreeSpace ? backColor_Conflict_FreeSpace : backColor_Conflict_NonFreeSpace; 
                lv_Conflicts.Items.Add(item);
            }
        }

        private int GetLeastConflictPatchIndex(int patchIndex, List<PatchRangeConflict> conflictList)
        {
            int leastValue = patchIndex;
            foreach (PatchRangeConflict conflict in conflictList)
            {
                if (conflict.ConflictPatchNumber < leastValue)
                    leastValue = conflict.ConflictPatchNumber;
            }

            return leastValue;
        }

        private void lv_Patches_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedConflicts();
        }
    }
}
