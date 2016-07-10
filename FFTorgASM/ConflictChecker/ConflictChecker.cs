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

namespace FFTorgASM
{

    public partial class ConflictChecker : Form
    {
        FolderBrowserDialog o = new FolderBrowserDialog();
        FileWrite BATTLEBIN = new FileWrite();
        FileWrite WORLDBIN = new FileWrite();
        FileWrite WLDCOREBIN = new FileWrite();
        FileWrite SCUS = new FileWrite();
        FileWrite REQUIREOUT = new FileWrite();
        FileWrite EQUIPOUT = new FileWrite();

        Patch[] Patches = new Patch[1];

        bool startup = false;
        string errorstring = "";
        
        public ConflictChecker()
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

             files = Directory.GetFiles(Application.StartupPath, "*.xml", SearchOption.TopDirectoryOnly);

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
                             
                                string name = Patch.Attributes["name"].InnerText;
                                XmlAttribute ignoreNode = Patch.Attributes["ignore"];
                                if (!(ignoreNode != null && Boolean.Parse(ignoreNode.InnerText)))
                                {
                                    foreach (XmlNode Location in Patch)         //For each Description and Location
                                    {
                                        if (Location.NodeType != XmlNodeType.Comment && Location.NodeType != XmlNodeType.Text)
                                        {
                                            if (Location.Attributes["offset"] != null)
                                            {
                                                UInt32 offset = UInt32.Parse(Location.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                                string file2 = Location.Attributes["file"].InnerText;
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
                                                        BATTLEBIN.AddWrite(offset, bytes.Length / 2, name, shortfile);
                                                        break;
                                                    case "WORLD_WORLD_BIN":
                                                        WORLDBIN.AddWrite(offset, bytes.Length / 2, name, shortfile);
                                                        break;
                                                    case "WORLD_WLDCORE_BIN":
                                                        WLDCOREBIN.AddWrite(offset, bytes.Length / 2, name, shortfile);
                                                        break;
                                                    case "SCUS_942_21":
                                                        SCUS.AddWrite(offset, bytes.Length / 2, name, shortfile);
                                                        break;
                                                    case "EVENT_REQUIRE_OUT":
                                                        REQUIREOUT.AddWrite(offset, bytes.Length / 2, name, shortfile);
                                                        break;
                                                    case "EVENT_EQUIP_OUT":
                                                        EQUIPOUT.AddWrite(offset, bytes.Length / 2, name, shortfile);
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
                        MessageBox.Show("Error in format of " + errorstring + "\n" + ex);
                    }
            }

                Filelistbox.Items.Add("Battle.Bin");
                Filelistbox.Items.Add("World.Bin");
                Filelistbox.Items.Add("Wldcore.Bin");
                Filelistbox.Items.Add("Scus.942.21");
                Filelistbox.Items.Add("Require.Out");
                Filelistbox.Items.Add("Equip.Out");
        }

        private string Validatetext(string bytes)
        {
            bytes = bytes.Replace("\n","");
            bytes = bytes.Replace("\r", "");
            bytes = bytes.Replace(" ", "");
            return bytes;
        }

        private void Filelistbox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Patcheslistview.Items.Clear();
            string[] patchnames = new string[1];
            FileWrite File = new FileWrite();
            switch(Filelistbox.SelectedIndex)
            {
                case -1:
                    patchnames = BATTLEBIN.FindAllPatchNames();
                    File = BATTLEBIN;
                    break;
                case 0:
                     patchnames = BATTLEBIN.FindAllPatchNames();
                     File = BATTLEBIN;
                    break;
                case 1:
                     patchnames = WORLDBIN.FindAllPatchNames();
                     File = WORLDBIN;
                    break;
                case 2:
                     patchnames = WLDCOREBIN.FindAllPatchNames();
                     File = WLDCOREBIN;
                    break;
                case 3:
                     patchnames = SCUS.FindAllPatchNames();
                     File = SCUS;
                    break;
                case 4:
                     patchnames = REQUIREOUT.FindAllPatchNames();
                     File = REQUIREOUT;
                    break;
                case 5:
                     patchnames = EQUIPOUT.FindAllPatchNames();
                     File = EQUIPOUT;
                    break;
            }
            int k = 1;

            Patcheslistview.View = View.Details;
            Patcheslistview.HeaderStyle = ColumnHeaderStyle.None;
            Patcheslistview.Columns.Add("");
            Patcheslistview.Columns[0].Width = 500;
            //Patcheslistview.AutoResizeColumn(0,ColumnHeaderAutoResizeStyle.ColumnContent);

            Patches = new Patch[patchnames.Length];

            foreach (string name in patchnames)
            {
                Patches[k - 1] = new Patch();
                Patches[k - 1].name = name;
                Patcheslistview.Items.Add(k + " " + name);
                CheckforConflicts(name, File, Patches, k);
                k++;
            }

            int j = 0;
            foreach(Patch patch in Patches)
            {
                patch.ConvertConflicts(patchnames, File);
                if(patch.Conflictexists)
                {
                    Patcheslistview.Items[j].BackColor = Color.Red;
                }
                j++;
            }

        }

        public void CheckforConflicts(string name, FileWrite BIN, Patch[] Patches,int k)
        {
            
            int patchindex = 0;
            int otherpatchindex = 0;
            int[] conflictarray = new int[1];

            for (patchindex = 0; patchindex < BIN.index; patchindex++)
            {
                if (name == BIN.Patchname[patchindex])
                {
                    Patches[k-1].AddOffset(BIN.startaddress[patchindex]);
                    Patches[k-1].AddEndaddress(BIN.endaddress[patchindex]);
                    Patches[k - 1].XMLFile = BIN.xmlfilename[patchindex];

                    for (otherpatchindex = 0; otherpatchindex < BIN.index; otherpatchindex++)
                    {  //if()

                        if (BIN.startaddress[patchindex] < BIN.startaddress[otherpatchindex] &&
                           BIN.endaddress[patchindex] > BIN.startaddress[otherpatchindex])
                        {
                            Patches[k-1].Conflictexists = true;
                            Patches[k-1].AddConflict(BIN.startaddress[patchindex],
                                                   otherpatchindex,
                                                   BIN.startaddress[otherpatchindex]);
                        }
                        else if (BIN.startaddress[patchindex] > BIN.startaddress[otherpatchindex] &&
                           BIN.startaddress[patchindex] < BIN.endaddress[otherpatchindex])
                        {
                            Patches[k-1].Conflictexists = true;
                            Patches[k-1].AddConflict(BIN.startaddress[patchindex],
                                                   otherpatchindex,
                                                   BIN.startaddress[otherpatchindex]);
                        }

                    }//scan patch vs otherpatches

                }

            }

            #region code to scan each patch vs each other patch
            //for (patchindex = 0; patchindex < BIN.index; patchindex++)
            //{
            //    if (patchindex != otherpatchindex)
            //    {
            //        if (BIN.startaddress[patchindex] < BIN.startaddress[otherpatchindex] &&
            //           BIN.endaddress[patchindex] > BIN.startaddress[otherpatchindex])
            //        {
            //            BIN.conflicts[patchindex] = true;
            //            BIN.conflictindex[patchindex] = otherpatchindex;
            //            conflictarray = otherpatchindex;
            //        }
            //        if (BIN.startaddress[patchindex] > BIN.startaddress[otherpatchindex] &&
            //           BIN.startaddress[patchindex] < BIN.endaddress[otherpatchindex])
            //        {
            //            BIN.conflicts[patchindex] = true;
            //            BIN.conflictindex[patchindex] = otherpatchindex;
            //        }

            //    }
            //}
            #endregion
        }

        public void LoadConflictsIntoListView()
        {
            int intselectedindex = Patcheslistview.SelectedIndices[0];
            if (Patches[intselectedindex].Conflicts[0] != null)
            {
                foreach (Conflict conflict in Patches[intselectedindex].Conflicts)
                {

                    ListViewItem item = new ListViewItem();
                    item.Text = conflict.offset.ToString("X");
                    item.SubItems.Add(conflict.Patchnumber.ToString());
                    item.SubItems.Add(conflict.patchoffset.ToString("X"));
                    ConflictListview.Items.Add(item);
                }
            }
        }
     
        private void Patcheslistview_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ConflictListview.Items.Clear();
            ConflictListview.View = View.Details;
            if(Patcheslistview.SelectedIndices.Count > 0)
            {
                int intselectedindex = Patcheslistview.SelectedIndices[0];
                Filelabel.Text = Patches[intselectedindex].XMLFile;

                 LoadConflictsIntoListView();
                
            }
        }
        private void ConflictListview_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
