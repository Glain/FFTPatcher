using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using PatcherLib.Iso;
using PatcherLib.Datatypes;

namespace FFTorgASM
{
    public partial class FreeSpaceForm : Form
    {
        private ASMEncoding.ASMEncodingUtility asmUtility;
        private FreeSpaceMode mode;
        private List<AsmPatch> patchList;
        private FreeSpaceMaps freeSpaceMaps;
        private Dictionary<DataGridViewRow, PatchedByteArray> rowPatchMap;

        public FreeSpaceForm(List<AsmPatch> patchList, ASMEncoding.ASMEncodingUtility asmUtility)
        {
            InitializeComponent();
            FreeSpaceMode mode = (asmUtility.EncodingMode == ASMEncoding.ASMEncodingMode.PSP) ? FreeSpaceMode.PSP : FreeSpaceMode.PSX;
            InitFiles(mode);
            Init(patchList, asmUtility, mode);
        }

        private void InitFiles(FreeSpaceMode mode)
        {
            this.Filelistbox.Items.Clear();
            this.Filelistbox.Items.AddRange(FreeSpace.GetRangeNames(mode));
        }

        private void Init(List<AsmPatch> patchList, ASMEncoding.ASMEncodingUtility asmUtility, FreeSpaceMode mode)
        {
            this.patchList = patchList;
            this.asmUtility = asmUtility;
            this.freeSpaceMaps = FreeSpace.GetFreeSpaceMaps(patchList, mode); 
        }

        private void Reload()
        {
            Init(patchList, asmUtility, mode);
            LoadItems(Filelistbox.SelectedIndex);
        }

        private void LoadItems(int freeSpacePositionIndex)
        {
            lbl_NumberOfPatches.Text = "0";
            lbl_NumberOfWrites.Text = "0";

            if (freeSpacePositionIndex < 0)
                return;

            dgv_FreeSpace.Rows.Clear();
            txtAddress.Clear();

            rowPatchMap = new Dictionary<DataGridViewRow, PatchedByteArray>();

            PatchRange range = FreeSpace.GetRanges(mode)[freeSpacePositionIndex];
            if (!freeSpaceMaps.PatchRangeMap.ContainsKey(range))
                return;

            //long positionEndOffset = position.StartLocation + position.Length - 1;
            List<PatchedByteArray> patchedByteArrayList = freeSpaceMaps.PatchRangeMap[range];
            HashSet<AsmPatch> asmPatchSet = freeSpaceMaps.OuterPatchRangeMap[range];

            for (int index = 0; index < patchedByteArrayList.Count; index++)
            {
                PatchedByteArray patchedByteArray = patchedByteArrayList[index];
                AsmPatch asmPatch = freeSpaceMaps.InnerPatchMap[patchedByteArray];

                // Column order: Number, Address, Length, Next Address, Space to Next Patch, File, Name
                int length = patchedByteArray.GetBytes().Length;
                long nextAddress = patchedByteArray.Offset + length;
                long nextPatchLocation = (index < (patchedByteArrayList.Count - 1)) ? patchedByteArrayList[index + 1].Offset : range.EndOffset;
                long spaceToNextPatch = nextPatchLocation - nextAddress;
                bool isSpaceToNextPatchNegative = (spaceToNextPatch < 0);
                //string strSpaceToNextPatch = isSpaceToNextPatchNegative ? ("-" + (-spaceToNextPatch).ToString("X")) : spaceToNextPatch.ToString("X");

                //dgv_FreeSpace.Rows.Add(index, patchedByteArray.Offset.ToString("X"), length.ToString("X"), nextAddress.ToString("X"), strSpaceToNextPatch, asmPatch.Filename, asmPatch.Name);
                dgv_FreeSpace.Rows.Add(index, patchedByteArray.Offset, length, nextAddress, spaceToNextPatch, asmPatch.Filename, asmPatch.Name);

                if (isSpaceToNextPatchNegative)
                {
                    dgv_FreeSpace.Rows[index].Cells[4].Style.BackColor = Color.FromArgb(225, 125, 125);
                }

                rowPatchMap.Add(dgv_FreeSpace.Rows[index], patchedByteArray);
            }

            lbl_NumberOfPatches.Text = asmPatchSet.Count.ToString();
            lbl_NumberOfWrites.Text = patchedByteArrayList.Count.ToString();
        }

        private void dgv_FreeSpace_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex >= 1) && (e.ColumnIndex <= 4) && (e.Value != null))
            {
                long value = 0L;
                if (long.TryParse(e.Value.ToString(), out value))
                {
                    e.Value = (value < 0) ? ("-" + (-value).ToString("X")) : value.ToString("X");
                    e.FormattingApplied = true;
                }
            }
        }

        private void Filelistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems(Filelistbox.SelectedIndex);
        }

        private void dgv_FreeSpace_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dgv_FreeSpace.SelectedRows;
            if (selectedRows.Count > 0)
            {
                DataGridViewRow row = selectedRows[0];
                long offset = (long)(row.Cells[1].Value);
                txtAddress.Text = offset.ToString("X");
                txtAddress.BackColor = Color.White;
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            long newOffset = 0U;

            if (long.TryParse(txtAddress.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out newOffset))
            {
                DataGridViewSelectedRowCollection selectedRows = dgv_FreeSpace.SelectedRows;
                if (selectedRows.Count > 0)
                {
                    DataGridViewRow row = selectedRows[0];
                    long offset = (long)(row.Cells[1].Value);

                    if (offset != newOffset)
                    {
                        PatchedByteArray patchedByteArray = rowPatchMap[row];
                        AsmPatch asmPatch = freeSpaceMaps.InnerPatchMap[patchedByteArray];
                        long moveOffset = newOffset - offset;

                        MovePatchRange movePatchRange = new MovePatchRange(new PatchRange(patchedByteArray), moveOffset);
                        asmPatch.MoveBlock(asmUtility, movePatchRange);
                        asmPatch.Update(asmUtility);

                        txtAddress.BackColor = Color.White;
                        Reload();
                    }
                }
            }
            else
            {
                txtAddress.BackColor = Color.FromArgb(225, 125, 125);
            }
        }
    }
}
