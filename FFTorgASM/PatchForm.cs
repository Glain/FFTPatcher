﻿using ASMEncoding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFTorgASM
{
    public partial class PatchForm : Form
    {
        private IList<AsmPatch> PatchList { get; set; }
        private ASMEncodingUtility AsmUtility { get; set; }
        private int NumSelectedPatches { get; set; }

        public PatchForm(IList<AsmPatch> patchList, ASMEncodingUtility asmUtility)
        {
            InitializeComponent();
            this.PatchList = patchList;
            this.AsmUtility = asmUtility;
            InitPatchesListBox(patchList);
            CheckConflicts(patchList, asmUtility);
        }

        private void InitPatchesListBox(IList<AsmPatch> patchList)
        {
            clb_Patches.Items.Clear();
            for (int index = 0; index < patchList.Count; index++)
            {
                clb_Patches.Items.Add(patchList[index]);
                clb_Patches.ForceSetItemChecked(index, true);
            }
        }

        private void CheckConflicts(IList<AsmPatch> patchList, ASMEncodingUtility asmUtility)
        {
            FreeSpaceMode mode = FreeSpace.GetMode(asmUtility);
            ConflictCheckResult conflictCheckResult = ConflictHelper.CheckConflicts(patchList, mode);
            if (conflictCheckResult.PatchList.Count == 0)
            {
                chk_ResolveConflicts.Checked = false;
                chk_ResolveConflicts.Visible = false;
                Height -= 32;
            }
        }

        private List<AsmPatch> GetAllSelectedPatches()
        {
            List<AsmPatch> resultList = new List<AsmPatch>();

            for (int index = 0; index < clb_Patches.Items.Count; index++)
            {
                if (clb_Patches.GetItemChecked(index))
                    resultList.Add((AsmPatch)clb_Patches.Items[index]);
            }

            return resultList;
        }

        private void SavePatchXML()
        {
            List<AsmPatch> patches = GetAllSelectedPatches();
            foreach (AsmPatch patch in patches)
            {
                patch.Update(AsmUtility);
            }

            FreeSpaceMode mode = FreeSpace.GetMode(AsmUtility);
            string xml = PatchXmlReader.CreatePatchXML(patches, mode);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                btn_SavePatchXML.Enabled = false;
                System.IO.File.WriteAllText(saveFileDialog.FileName, xml, Encoding.UTF8);
                PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                Close();
            }
        }

        private void btn_Patch_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = (AsmUtility.EncodingMode == ASMEncodingMode.PSX) 
                ? "ISO or PSV files (*.bin, *.iso, *.img, *.psv)|*.bin;*.iso;*.img;*.psv"
                : "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.CheckFileExists = true;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                btn_Patch.Enabled = false;
                PatchResult patchResult = PatchHelper.PatchFile(saveFileDialog.FileName, GetAllSelectedPatches(), AsmUtility, chk_ResolveConflicts.Checked);
                PatcherLib.MyMessageBox.Show(this, patchResult.Message, ((patchResult.IsSuccess) ? "Complete!" : "Error"), MessageBoxButtons.OK);
                Close();
            }
        }

        private void clb_Patches_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                NumSelectedPatches++;
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                NumSelectedPatches--;
            }

            btn_Patch.Enabled = (NumSelectedPatches > 0);
        }

        private void btn_SavePatchXML_Click(object sender, EventArgs e)
        {
            SavePatchXML();
        }
    }
}
