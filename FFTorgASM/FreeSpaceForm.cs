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
        internal enum FreeSpaceLocation
        {
            BATTLE_BIN = 0,
            WORLD_BIN = 1,
            SCUS_1 = 2,
            SCUS_2 = 3
        }

        private const int NumFreeSpaceRanges = 4;

        List<AsmPatch> patchList;
        PsxIso.KnownPosition[] FreeSpacePositions;
        Dictionary<PatchedByteArray, AsmPatch> innerPatchMap;
        Dictionary<PsxIso.KnownPosition, List<PatchedByteArray>> patchPositionMap;
        Dictionary<PsxIso.KnownPosition, HashSet<AsmPatch>> outerPatchPositionMap;

        public FreeSpaceForm(List<AsmPatch> patchList)
        {
            InitializeComponent();
            Init(patchList);
        }

        private void Init(List<AsmPatch> patchList)
        {
            this.patchList = patchList;

            FreeSpacePositions = new PsxIso.KnownPosition[NumFreeSpaceRanges] {
                //new PsxIso.KnownPosition(PsxIso.Sectors.BATTLE_BIN, 0xEA0E4, 0xED1C),         // 0xEA0E4 to 0xF8E00
                new PsxIso.KnownPosition(PsxIso.Sectors.BATTLE_BIN, 0xE92AC, 0x11030),          // 0xE92AC to 0xFA2DC
                //new PsxIso.KnownPosition(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5E3C8, 0xED1C),    // 0x5E3C8 to 0x6D0E4
                new PsxIso.KnownPosition(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5D590, 0x11030),     // 0x5D590 to 0x6E5C0
                new PsxIso.KnownPosition(PsxIso.Sectors.SCUS_942_21, 0x1785C, 0x2A8),           // 0x1785C to 0x17B04
                new PsxIso.KnownPosition(PsxIso.Sectors.SCUS_942_21, 0x17DC0, 0x117C)           // 0x17DC0 to 0x18F3C
            };

            innerPatchMap = new Dictionary<PatchedByteArray, AsmPatch>();
            patchPositionMap = new Dictionary<PsxIso.KnownPosition, List<PatchedByteArray>>();
            outerPatchPositionMap = new Dictionary<PsxIso.KnownPosition, HashSet<AsmPatch>>();

            foreach (AsmPatch patch in patchList)
            {
                foreach (PatchedByteArray patchedByteArray in patch)
                {
                    if (patchedByteArray is InputFilePatch)
                        continue;

                    if (!innerPatchMap.ContainsKey(patchedByteArray))
                        innerPatchMap.Add(patchedByteArray, patch);

                    long patchedByteArrayEndOffset = patchedByteArray.Offset + patchedByteArray.GetBytes().Length - 1;
                    foreach (PsxIso.KnownPosition position in FreeSpacePositions)
                    {
                        long positionEndOffset = position.StartLocation + position.Length - 1;
                        if ((((PsxIso.Sectors)patchedByteArray.Sector) == position.Sector) && (patchedByteArrayEndOffset >= position.StartLocation) && (patchedByteArray.Offset <= positionEndOffset))
                        {
                            if (patchPositionMap.ContainsKey(position))
                                patchPositionMap[position].Add(patchedByteArray);
                            else
                                patchPositionMap.Add(position, new List<PatchedByteArray>() { patchedByteArray });

                            if (outerPatchPositionMap.ContainsKey(position))
                                outerPatchPositionMap[position].Add(patch);
                            else
                                outerPatchPositionMap.Add(position, new HashSet<AsmPatch>() { patch });
                        }
                    }
                }

                foreach (PsxIso.KnownPosition position in FreeSpacePositions)
                {
                    if (patchPositionMap.ContainsKey(position))
                    {
                        patchPositionMap[position].Sort(
                            delegate(PatchedByteArray patchedByteArray1, PatchedByteArray patchedByteArray2)
                            {
                                return patchedByteArray1.Offset.CompareTo(patchedByteArray2.Offset);
                            }
                        );
                    }
                }
            }
        }

        private void LoadItems(FreeSpaceLocation location)
        {
            dgv_FreeSpace.Rows.Clear();

            PsxIso.KnownPosition position = FreeSpacePositions[(int)location];
            if (!patchPositionMap.ContainsKey(position))
                return;

            long positionEndOffset = position.StartLocation + position.Length - 1;
            List<PatchedByteArray> patchedByteArrayList = patchPositionMap[position];
            HashSet<AsmPatch> asmPatchSet = outerPatchPositionMap[position];

            for (int index = 0; index < patchedByteArrayList.Count; index++)
            {
                PatchedByteArray patchedByteArray = patchedByteArrayList[index];
                AsmPatch asmPatch = innerPatchMap[patchedByteArray];

                // Column order: Number, Address, Length, Next Address, Space to Next Patch, File, Name
                int length = patchedByteArray.GetBytes().Length;
                long nextAddress = patchedByteArray.Offset + length;
                long nextPatchLocation = (index < (patchedByteArrayList.Count - 1)) ? patchedByteArrayList[index + 1].Offset : positionEndOffset;
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
            LoadItems((FreeSpaceLocation)Filelistbox.SelectedIndex);
        }
    }
}
