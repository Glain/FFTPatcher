using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using PatcherLib.Iso;
using PatcherLib.Datatypes;

namespace FFTorgASM
{
    public partial class FreeSpaceForm : Form
    {
        List<AsmPatch> patchList;
        Dictionary<PatchedByteArray, AsmPatch> innerPatchMap;
        Dictionary<PatchRange, List<PatchedByteArray>> patchRangeMap;
        Dictionary<PatchRange, HashSet<AsmPatch>> outerPatchRangeMap;

        public FreeSpaceForm(List<AsmPatch> patchList)
        {
            InitializeComponent();
            Init(patchList);
        }

        private void Init(List<AsmPatch> patchList)
        {
            this.patchList = patchList;

            innerPatchMap = new Dictionary<PatchedByteArray, AsmPatch>();
            patchRangeMap = new Dictionary<PatchRange, List<PatchedByteArray>>();
            outerPatchRangeMap = new Dictionary<PatchRange, HashSet<AsmPatch>>();

            foreach (AsmPatch patch in patchList)
            {
                foreach (PatchedByteArray patchedByteArray in patch)
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
        }

        private void LoadItems(int freeSpacePositionIndex)
        {
            dgv_FreeSpace.Rows.Clear();

            PatchRange range = FreeSpace.PsxRanges[freeSpacePositionIndex];
            if (!patchRangeMap.ContainsKey(range))
                return;

            //long positionEndOffset = position.StartLocation + position.Length - 1;
            List<PatchedByteArray> patchedByteArrayList = patchRangeMap[range];
            HashSet<AsmPatch> asmPatchSet = outerPatchRangeMap[range];

            for (int index = 0; index < patchedByteArrayList.Count; index++)
            {
                PatchedByteArray patchedByteArray = patchedByteArrayList[index];
                AsmPatch asmPatch = innerPatchMap[patchedByteArray];

                // Column order: Number, Address, Length, Next Address, Space to Next Patch, File, Name
                int length = patchedByteArray.GetBytes().Length;
                long nextAddress = patchedByteArray.Offset + length;
                long nextPatchLocation = (index < (patchedByteArrayList.Count - 1)) ? patchedByteArrayList[index + 1].Offset : range.EndOffset;
                long spaceToNextPatch = nextPatchLocation - nextAddress;
                bool isSpaceToNextPatchNegative = (spaceToNextPatch < 0);
                string strSpaceToNextPatch = isSpaceToNextPatchNegative ? ("-" + (-spaceToNextPatch).ToString("X")) : spaceToNextPatch.ToString("X");

                dgv_FreeSpace.Rows.Add(index, patchedByteArray.Offset.ToString("X"), length.ToString("X"), nextAddress.ToString("X"), strSpaceToNextPatch, asmPatch.Filename, asmPatch.Name);

                if (isSpaceToNextPatchNegative)
                {
                    dgv_FreeSpace.Rows[index].Cells[4].Style.BackColor = Color.FromArgb(225, 125, 125);
                }
            }

            lbl_NumberOfPatches.Text = asmPatchSet.Count.ToString();
            lbl_NumberOfWrites.Text = patchedByteArrayList.Count.ToString();
        }

        private void Filelistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems(Filelistbox.SelectedIndex);
        }
    }
}
