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

        /*
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofile = new OpenFileDialog();
            XmlDocument Doc = new XmlDocument();
            string xmlstring = "";
            string[] files = new string[1];

            files = Directory.GetFiles(Application.StartupPath + "/XmlPatches", "*.xml", SearchOption.TopDirectoryOnly);
            string patchname = "";
            foreach (string file in files)
            {
                errorstring = file.Substring(file.LastIndexOf(@"\") + 1);
                try
                {
                    string[] Lines = new string[1];
                    xmlstring = File.ReadAllText(file, Encoding.UTF8);
                    foreach (string Line in File.ReadAllLines(file))
                    {
                        Lines[Lines.Length - 1] = Line;
                        Array.Resize(ref Lines, Lines.Length + 1);
                    }
                    Doc.LoadXml(xmlstring);

                    XmlNodeList xmlnode;
                    xmlnode = Doc.GetElementsByTagName("Patches");

                    foreach (XmlElement element in xmlnode)         //for all patches
                    {
                        XmlAttributeCollection attributes = element.Attributes;
                        XmlNodeList patchNodes = element.SelectNodes("Patch");
                        foreach (XmlNode Patch in patchNodes)           //For each Patch node
                        {
                            patchname = Patch.Attributes["name"].InnerText;

                            //if (patchname.Contains("Phoe"))
                            //    break;
                            
                            XmlAttribute ignoreNode = Patch.Attributes["ignore"];
                            if (!(ignoreNode != null && Boolean.Parse(ignoreNode.InnerText)))
                            {
                                foreach (XmlNode Location in Patch)         //For each Description and Location
                                {
                                    if (Location.NodeType != XmlNodeType.Comment && Location.NodeType != XmlNodeType.Text)
                                    {
                                        if (Location.Attributes == null || Location.Attributes["offset"] != null)
                                        {
                                            UInt32 offset = UInt32.Parse(Location.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                            
                                            string file2 = Location.Attributes["file"].InnerText;

                                            if ((file2.Contains("BATTLE") && (offset < 0xE92AC || offset > 0xF929B || patchname.Contains("Kanji"))) ||
                                                (file2.Contains("WORLD") && (offset < 0x0005D400 || offset > 0x0006B90F || patchname.Contains("Kanji"))))
                                                continue;

                                            string bytes = Location.InnerText;
                                            if (Location.Name == "Variable")
                                            {
                                                if (Location.Attributes["default"] != null)
                                                {
                                                    bytes = Location.Attributes["default"].InnerText;
                                                }
                                                else
                                                {
                                                    bytes = "00";
                                                }
                                            }
                                            bytes = Validatetext(bytes);
                                            string shortfile = file.Substring(file.LastIndexOf(@"\") + 1);
                                            switch (file2)
                                            {
                                                case "BATTLE_BIN":
                                                    BATTLEBIN.AddWrite(offset, bytes.Length / 2, patchname, shortfile);
                                                    break;
                                                case "WORLD_WORLD_BIN":
                                                    WORLDBIN.AddWrite(offset, bytes.Length / 2, patchname, shortfile);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in format of " + errorstring + "\n" + ex + "at patch: " + patchname);
                }
            }
            
            //Array.Sort(BATTLEBIN.startaddress);
            //Array.Sort(BATTLEBIN.endaddress);
            //Array.Sort(WORLDBIN.startaddress);
            //Array.Sort(WORLDBIN.endaddress);
        }

        private string Validatetext(string bytes)
        {
            bytes = bytes.Replace("\n", "");
            bytes = bytes.Replace("\r", "");
            bytes = bytes.Replace(" ", "");
            return bytes;
        }

        private void Filelistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            long currentaddress = 0;
            long currentFreeSpaceLength = 0;
            FileWrite File = new FileWrite();
            string[] patchnames = new string[1];

            dgv_FreeSpace.Rows.Clear();

            switch(Filelistbox.SelectedIndex)
            {
                case 1:
                    File = WORLDBIN;
                    currentaddress = 0x5d400;
                    patchnames = WORLDBIN.FindAllPatchNames();
                    break;
                default:
                    File = BATTLEBIN;
                    currentaddress = 0xe92ac;
                    patchnames = BATTLEBIN.FindAllPatchNames();
                    break;
            }

            //Patches = new Patch[patchnames.Length];
            int k = 0;
            long currentendaddress = 0;
            //Array.Sort(File.startaddress);
            for (k = 0; k < File.startaddress.Length;k++ )
            {
                if (File.startaddress[k] != 0)
                {
                    //if(startaddress < currentaddress)
                    //{
                    //    if (File.endaddress[k] > currentendaddress) ;
                    //        //currentaddress = File.endaddress[k];
                    //}
                    //else
                    //{
                    //currentFreeSpaceLength = startaddress - currentaddress;

                    dgv_FreeSpace.Rows.Add(File.startaddress[k].ToString("X"), File.length[k].ToString("X"),"","",File.Patchname[k]);
                    //currentaddress = File.endaddress[k];
                    //}

                    //if (k == 0x140)
                    //    this.Focus();
                }

            }

            dgv_FreeSpace.Sort(dgv_FreeSpace.Columns[0],ListSortDirection.Ascending);

            for(int r = 0;r < dgv_FreeSpace.Rows.Count - 1; r++)
            {
                long start = Convert.ToInt64(dgv_FreeSpace.Rows[r].Cells[0].Value.ToString(),16);
                long length = Convert.ToInt64(dgv_FreeSpace.Rows[r].Cells[1].Value.ToString(),16);

                long nextstart;
                if(r != dgv_FreeSpace.Rows.Count - 2 )
                     nextstart = Convert.ToInt64(dgv_FreeSpace.Rows[r + 1].Cells[0].Value.ToString(),16);
                else
                {
                    nextstart = 0xF929B;
                }
                if(nextstart > (start + 1))
                {
                    dgv_FreeSpace.Rows[r].Cells[2].Value = String.Format("{0:X}", start + length);
                    
                    short number = (short) (nextstart - (start + length));
                    bool isnegative = false;

                    if(nextstart - (start + length) < 0)
                    {
                        number = (short)( 0 - number);
                        isnegative = true;
                    }
                    
                    if(isnegative)
                    {
                        dgv_FreeSpace.Rows[r].Cells[3].Value = "-" +  String.Format("{0:X}", number);
                        dgv_FreeSpace.Rows[r].Cells[3].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        dgv_FreeSpace.Rows[r].Cells[3].Value = String.Format("{0:X}", number);
                    }
                    dgv_FreeSpace.Rows[r].Cells[5].Value = r.ToString();
                }

            }

            lbl_NumberOfPatches.Text = File.ShortPatchnames.Length.ToString();
            lbl_NumberOfWrites.Text = File.startaddress.Length.ToString();

        }
        */
    }
}
