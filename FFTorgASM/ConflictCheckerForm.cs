using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ExtensionMethods;
using PatcherLib.Iso;
using PatcherLib.Datatypes;

namespace FFTorgASM
{
    public partial class ConflictCheckerForm : Form
    {
        private Color backColor_Normal = Color.White;
        private Color backColor_Conflict_NonFreeSpace = Color.FromArgb(225, 125, 125);
        private Color backColor_Conflict_FreeSpace = Color.White; // Color.FromArgb(137, 216, 166);

        private IList<AsmPatch> origPatchList = null;
        private FreeSpaceMode mode;

        private ConflictCheckResult conflictPatchData = null;
        private List<Color> patchColors = null;
        private Random rng = new Random();

        public ConflictCheckerForm(IList<AsmPatch> patchList, FreeSpaceMode mode)
        {
            InitializeComponent();
            this.origPatchList = patchList;
            this.conflictPatchData = ConflictHelper.CheckConflicts(patchList, mode);
            FindPatchColors(this.conflictPatchData.PatchList);
            ShowPatches(this.conflictPatchData);
        }

        private void FindPatchColors(IEnumerable<AsmPatch> patchList)
        {
            patchColors = new List<Color>();
            foreach (AsmPatch patch in patchList)
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
                //PsxIso.Sectors sector = (PsxIso.Sectors)(conflict.ConflictRange.Sector);
                //string strSector = Enum.GetName(typeof(PsxIso.Sectors), sector);
                //string strSector = PatcherLib.Iso.PsxIso.GetSectorName(sector);

                Type sectorType = (mode == FreeSpaceMode.PSP) ? typeof(PspIso.Sectors) : typeof(PsxIso.Sectors);
                Enum sector = (Enum)Enum.ToObject(sectorType, conflict.ConflictRange.Sector);
                string strSector = (mode == FreeSpaceMode.PSP) ? PspIso.GetSectorName((PspIso.Sectors)sector) : PsxIso.GetSectorName((PsxIso.Sectors)sector);

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
