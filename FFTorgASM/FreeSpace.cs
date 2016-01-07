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
                                    if (Location.NodeType != XmlNodeType.Comment)
                                    {
                                        if (Location.Attributes["offset"] != null)
                                        {
                                            UInt32 offset = UInt32.Parse(Location.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber);
                                            
                                            string file2 = Location.Attributes["file"].InnerText;

                                            if ((file2.Contains("BATTLE") && (offset < 0xE92AC || offset > 0xF929B || name.Contains("Kanji"))) ||
                                                (file2.Contains("WORLD") && (offset < 0x0005D400 || offset > 0x0006B90F || name.Contains("Kanji"))))
                                                goto next;

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
                                            }
                                        }
                                    }
                                next: ;
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
            foreach (long startaddress in File.startaddress)
            {
                if(startaddress != 0)
                {
                    if(startaddress < currentaddress)
                    {
                        if (File.endaddress[k] > currentendaddress) ;
                            //currentaddress = File.endaddress[k];
                    }
                    else
                    {
                        currentFreeSpaceLength = startaddress - currentaddress;
                        dgv_FreeSpace.Rows.Add(currentaddress.ToString("X"), currentFreeSpaceLength.ToString("X"));
                        currentaddress = File.endaddress[k];
                    }

                   
                }
              
                k++;
            }
        }


    }
}
