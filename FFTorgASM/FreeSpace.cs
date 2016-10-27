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

namespace FFTorgASM
{
    public partial class FreeSpace : Form
    {
        FileWrite BATTLEBIN = new FileWrite();
        FileWrite WORLDBIN = new FileWrite();
        Patch[] Patches = new Patch[1];

        bool startup = false;
        string errorstring = "";

        public FreeSpace()
        {
            InitializeComponent();
            EventArgs e = new EventArgs();
            startup = true;
            openToolStripMenuItem_Click(this, e);
            startup = false;

        }

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


    }
}
