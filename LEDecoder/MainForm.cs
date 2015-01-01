/*
 * Created by SharpDevelop.
 * User: Jeremy
 * Date: 9/17/2011
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using ASMEncoding;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using PatcherLib;
using PatcherLib.Resources;

namespace LEDecoder
{
	public struct LEDDialogResult
	{
		public DialogResult DialogResult;
		public Stream Stream;
		public string FileName;
	}
	
	public partial class MainForm : Form
	{		
        //Hi Glain, Why label these as "Fields" instead of "Global Variables"?
        //Are Fields nomenclature for something I'm not aware of or just preference?
        //-Choto
		#region Fields
		Color _ledColor = Color.Yellow;
		ASMEncodingUtility _asmUtility;
		#endregion

        #region Global Variables
        string Function = "Decode ASM";

        #region JalFinder Variables

        public string CurrentFile = "";
        public string SCUSWiki = "";
        public string BATTLEWiki = "";
        public string WORLDWiki = "";
        public string WLDCOREWiki = "";
        public string EQUIPWiki = "";
        public string REQUIREWiki = "";

        public string SCUSDisassembly = "";
        public string BATTLEDisassembly = "";
        public string WORLDDisassembly = "";
        public string WLDCOREDisassembly = "";
        public string EQUIPDisassembly = "";
        public string REQUIREDisassembly = "";

        public string[] SCUSLines;
        public string[] BATTLELines;
        public string[] WORLDLines;
        public string[] WLDCORELines;
        public string[] REQUIRELines;
        public string[] EQUIPLines;
        #endregion

        #region Autonotator Variables
        public Register[] Registers = new Register[32];
        public MainAddress[] MainAddresses;
        public bool settingregisters = false;
        int[] linestoinsert = new int[1];
        public int jalcommandcounter = 0;
       

        #region Lists
        public IList<string> SpriteSets = PatcherLib.PSXResources.Lists.SpriteSets;
        public IList<string> SkillSets = PatcherLib.PSXResources.Lists.SkillSets;
        public IList<string> StatusNames = PatcherLib.PSXResources.Lists.StatusNames;
        public IList<string> Jobs = PatcherLib.PSXResources.Lists.JobNames;
        public IList<string> UnitNames = PatcherLib.PSXResources.Lists.UnitNames;
        public IList<string> MonsterNames = PatcherLib.PSXResources.Lists.MonsterNames;
        public IList<string> EventNames = PatcherLib.PSXResources.Lists.EventNames;
        public IList<string> Items = PatcherLib.PSXResources.Lists.Items;
        public IList<string> MapNames = PatcherLib.PSXResources.Lists.MapNames;
        public IList<string> SpriteFiles = PatcherLib.PSXResources.Lists.SpriteFiles;
        public IList<string> SpecialNames = PatcherLib.PSXResources.Lists.SpecialNames;
        public IList<string> Treasures = PatcherLib.PSXResources.Lists.Treasures;
        public IList<string> UnexploredLands = PatcherLib.PSXResources.Lists.UnexploredLands;
        public IList<string> Propositions = PatcherLib.PSXResources.Lists.Propositions;
        public IList<string> ShopAvailabilities = PatcherLib.PSXResources.Lists.ShopAvailabilities;

        public IList<string> Abilities = PatcherLib.PSXResources.Lists.AbilityNames;
        public IList<string> AbilityEffects = PatcherLib.PSXResources.Lists.AbilityEffects;
        public IList<string> AbilityAI = PatcherLib.PSXResources.Lists.AbilityAI;
        public IList<string> AbilityAttributes = PatcherLib.PSXResources.Lists.AbilityAttributes;
        public IList<string> AbilityTypes = PatcherLib.PSXResources.Lists.AbilityTypes;


        //IList<string> TownNames = (IList<string>)PatcherLib.PSXResources.Lists.TownNames;
        //IList<string> ShopNames = (IList<string>)PatcherLib.PSXResources.Lists.ShopNames;
        #endregion


        #endregion
        #endregion

        #region Form Initialization
        public MainForm()
		{
			InitializeComponent();
			DoProcess();
             #region Autonotator
            for (int i = 0; i < 32; i++)
            {
                Registers[i] = new Register(i);
            }
            GetDataNotes();
            #endregion

        }
		
        //I had to change the name of Process() because it conflicted with Process in System.Diagnostics
		// Invoked when the form is initialized.
		public void DoProcess()
		{
            _asmUtility = new ASMEncodingUtility();
            _asmUtility.EncodingMode = ASMEncodingMode.PSX;

			// Set starting form state
			chk_LittleEndian.Checked = true;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = 10;
			timer.Enabled = true;
			timer.Start();
			
			Icon = SystemIcons.Information;

            cb_Mode.Items.Add("Base");
            cb_Mode.Items.Add("PSX");
            cb_Mode.Items.Add("PSP");
            cb_Mode.SelectedIndex = ASMEncodingMode.PSX;
		}
		
		void timer_Tick(object sender, EventArgs e)
		{
            ((System.Windows.Forms.Timer)(sender)).Stop();
			DrawLED(Color.Blue);
		}
		#endregion
		
		#region Event Handlers
		void pic_LEDPaint(object sender, PaintEventArgs e)
		{
			Pen pen = new Pen(_ledColor);
        	Brush brush = new SolidBrush(_ledColor);
        	e.Graphics.DrawEllipse(pen, 0, 0, 25, 25);
        	e.Graphics.FillEllipse(brush, 0, 0, 25, 25);
		}
		
		void Btn_InputFileClick(object sender, EventArgs e)
		{
			LEDDialogResult result = GetFileInfoFromDialog(false);
			txt_InputFile.Text = result.FileName;
		}
		
		void Btn_OutputFileClick(object sender, EventArgs e)
		{
			LEDDialogResult result = GetFileInfoFromDialog(true);
			txt_OutputFile.Text = result.FileName;
		}
		
		void Btn_ProcessClick(object sender, EventArgs e)
		{
			switch (Function)
            {
                case "Decode ASM":
                    DecodeASM();
                    break;
                case "Print Hex":
                    PrintHex();
                    break;
                case "JalFind":
                    JalFind();
                    break;
                case "Collapse Routines":
                    CollapseRoutines();
                    break;
                case "AutoNotate":
                    AutoNotate();
                    break;
            }
		}

        private void btn_UpdateWiki_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\SCUSWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/SCUS_942.21");
                SCUSWiki = reply;

                sw.Write(SCUSWiki);

            }
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\BATTLEWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/BATTLE.BIN");
                BATTLEWiki = reply;

                sw.Write(BATTLEWiki);

            }
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\WORLDWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/WORLD.BIN");
                WORLDWiki = reply;

                sw.Write(WORLDWiki);

            }
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\WLDCOREWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/WLDCORE.BIN");
                WLDCOREWiki = reply;

                sw.Write(WLDCOREWiki);

            }
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\REQUIREWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/REQUIRE.OUT");
                REQUIREWiki = reply;

                sw.Write(REQUIREWiki);

            }
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\EQUIPWIKI.txt"))
            {
                WebClient client = new WebClient();
                string reply = "";

                reply = client.DownloadString("http://ffhacktics.com/wiki/EQUIP.OUT");
                EQUIPWiki = reply;

                sw.Write(EQUIPWiki);
            }
        }


        private void CollapseRoutinesButton_CheckedChanged(object sender, EventArgs e)
        {
            txt_StartingAddress.Visible = false;
            txt_Length.Visible = false;
            txt_BPLbox.Visible = false;
            lab_PSX.Visible = true;
            cb_Mode.Visible = false;
            lab_BPL.Visible = false;
            lab_Length.Visible = false;
            lbl_StartingAddress.Visible = false;
            chk_SpaceBox.Visible = false;
            cmb_FileofRoutine.Visible = false;
            lbl_FileofRoutine.Visible = false;
            txt_InputFile.Enabled = true;
            btn_InputFile.Enabled = true;
            btn_UpdateWiki.Visible = false;
            if (CollapseRoutinesButton.Checked)
            {
                Function = "Collapse Routines";
            }
        }
        private void AutoNotateButton_CheckedChanged(object sender, EventArgs e)
        {
            txt_Length.Visible = false;
            txt_BPLbox.Visible = false;
            lab_PSX.Visible = true;
            cb_Mode.Visible = false;

            lab_BPL.Visible = false;
            lab_Length.Visible = false;
            lbl_StartingAddress.Visible = true;
            txt_StartingAddress.Visible = true; ;
            chk_SpaceBox.Visible = false;
            cmb_FileofRoutine.Visible = true;
            lbl_FileofRoutine.Visible = true;
            txt_InputFile.Enabled = false;
            btn_InputFile.Enabled = false;
            btn_UpdateWiki.Visible = false;

            if (AutoNotateButton.Checked)
            {
                Function = "AutoNotate";
            }
        }
        private void JalFindButton_CheckedChanged(object sender, EventArgs e)
        {
            txt_StartingAddress.Visible = true;
            lbl_StartingAddress.Visible = true;
            txt_Length.Visible = false;
            txt_BPLbox.Visible = false;
            lab_PSX.Visible = true;
            cb_Mode.Visible = false;
            lab_BPL.Visible = false;
            lab_Length.Visible = false;
            chk_SpaceBox.Visible = false;
            cmb_FileofRoutine.Visible = true;
            lbl_FileofRoutine.Visible = true;
            txt_InputFile.Enabled = false;
            btn_InputFile.Enabled = false;
            btn_UpdateWiki.Visible = true;

            if (JalFindButton.Checked)
            {
                Function = "JalFind";
            }
        }
        private void PrintHexButton_CheckedChanged(object sender, EventArgs e)
        {
            txt_StartingAddress.Visible = true;
            txt_Length.Visible = true;
            txt_BPLbox.Visible = true;
            lab_PSX.Visible = true;
            cb_Mode.Visible = false;
            lab_BPL.Visible = true;
            lab_Length.Visible = true;
            lbl_StartingAddress.Visible = true;
            chk_SpaceBox.Visible = true;
            cmb_FileofRoutine.Visible = false;
            lbl_FileofRoutine.Visible = false;
            txt_InputFile.Enabled = true;
            btn_InputFile.Enabled = true;
            btn_UpdateWiki.Visible = false;
            if (PrintHexButton.Checked)
            {
                Function = "Print Hex";
            }
        }
        private void DecodeASMButton_CheckedChanged(object sender, EventArgs e)
        {

            txt_StartingAddress.Visible = true;
            txt_Length.Visible = false;
            txt_BPLbox.Visible = false;
            lab_PSX.Visible = false;
            cb_Mode.Visible = true;
            lab_BPL.Visible = false;
            lab_Length.Visible = false;
            lbl_StartingAddress.Visible = true;
            chk_SpaceBox.Visible = false;
            cmb_FileofRoutine.Visible = false;
            lbl_FileofRoutine.Visible = false;
            txt_InputFile.Enabled = true;
            btn_InputFile.Enabled = true;
            btn_UpdateWiki.Visible = false;
            if (DecodeASMButton.Checked)
            {
                Function = "Collapse Routines";
            }
        }

        private void cb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _asmUtility.EncodingMode = cb_Mode.SelectedIndex;
        }

		#endregion
		
		#region Other Methods
		
		private void DrawLED(Color color)
		{
			_ledColor = color;
			pic_LED.Invalidate();
		}
		private LEDDialogResult GetFileInfoFromDialog(bool useSaveFileDialog)
		{
			//FileDialog fileDialog = useSaveFileDialog ? new SaveFileDialog() : new OpenFileDialog();
			FileDialog fileDialog = useSaveFileDialog ? (FileDialog)new SaveFileDialog() : (FileDialog)new OpenFileDialog();
			
		    //OpenFileDialog openFileDialog = new OpenFileDialog();
		    fileDialog.Filter = "All files (*.*)|*.*";
		    fileDialog.FilterIndex = 0;
		    fileDialog.RestoreDirectory = true;
		    
		    DialogResult dialogResult;
			dialogResult = fileDialog.ShowDialog();
			LEDDialogResult LDResult = new LEDDialogResult();
		    
		    if(dialogResult == DialogResult.OK)
		    {
				LDResult.FileName = fileDialog.FileName;
		    }

		    LDResult.DialogResult = dialogResult;
		    
		    return LDResult;
		}
        public string[] Delete(string[] array,int index)
        {
            int i = 0;
            string[] newarray = new string[array.Length - 1];
            foreach (string str in array)
            {
                if (index > i && i < newarray.Length)
                {
                    newarray[i] = array[i];
                    
                }
                else if(index < i && i < newarray.Length)
                {
                    newarray[i-1] = array[i];
                }
                i++;
            }
            return newarray;
        }
        public string ReverseBytes(string line)
        {
            string newline = "";
            newline = line.Substring(7);
            newline += line[6];
            newline += line[5];
            newline += line[4];
            newline += line[3];
            newline += line[2];
            newline += line[1];
            newline += line[0];
            return newline;
        }
        public long ReadHexOrDec(string Entry)
        {
            long output = 0;
            string input = "";
            if (Entry.IndexOf('x') > 0)
            {
                input = Entry.Substring(2);
                output = Convert.ToInt32(input, 16);
            }
            else
            {
                output = Convert.ToInt32(Entry);
            }
            return output;
        }

        #region JalFinder Routines
        public string scanresource(string Resource)
        {
            string result = "";

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Resource;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
        public static string[] ToLines(string wholestring)
        {
            string[] result = wholestring.Replace("\r", "").Split('\n');
            return result;
        }
        public string scanfile(string Path)
        {
            string result = "";
            using (Stream sr = File.OpenRead(Path))
            {
                byte[] asciichars = new byte[sr.Length];
                int length = sr.Read(asciichars, 0, (int)sr.Length);
                result = Encoding.ASCII.GetString(asciichars);
            }
            return result;
        }
        public string GetRoutineDescription(string address)
        {
            string result = "";
            string filewiki = "";
            switch (cmb_FileofRoutine.SelectedIndex)
            {
                case -1:
                case 0: //Battle and Require
                    if(StringToAddress(address) < 0x67000)
                    {
                        filewiki = SCUSWiki;
                    }
                    else if (StringToAddress(address) > 0x67000 && StringToAddress(address) < 0x1bf000)
                    {
                        filewiki = BATTLEWiki;
                    }
                    else if (StringToAddress(address) > 0x1bf000)
                    {
                        filewiki = REQUIREWiki;
                    }
                    break;
                case 1: //Battle and Equip
                    if (StringToAddress(address) < 0x67000)
                    {
                        filewiki = SCUSWiki;
                    }
                    else if (StringToAddress(address) > 0x67000 && StringToAddress(address) < 0x1bf000)
                    {
                        filewiki = BATTLEWiki;
                    }
                    else if (StringToAddress(address) > 0x1bf000)
                    {
                        filewiki = EQUIPWiki;
                    }
                    break;
                case 2:
                    if (StringToAddress(address) < 0x67000)
                    {
                        filewiki = SCUSWiki;
                    }
                    else if (StringToAddress(address) > 0x67000 && StringToAddress(address) < 0xE0000)
                    {
                        filewiki = BATTLEWiki;
                    }
                    else if (StringToAddress(address) > 0xE0000)
                    {
                        filewiki = WORLDWiki;
                    }
                    break;
            }
            string tempstring = "";

            address = ReformatAddress(address);
            string[] wikilines = ToLines(filewiki);
            foreach(string line in wikilines)
            {
                if(line.IndexOf(address) > 0)
                {
                    if(line.IndexOf(" title") > 0)
                    {
                        tempstring = line.Substring(line.IndexOf(address));
                        tempstring = tempstring.Substring(tempstring.IndexOf(" title"));
                        //tempstring = tempstring.Substring(tempstring.IndexOf("title", 10));
                        tempstring = tempstring.Substring(tempstring.IndexOf("\""));
                        tempstring = tempstring.Substring(1);
                        tempstring = tempstring.Remove(tempstring.IndexOf("\""));
                        tempstring = tempstring.Replace("(page does not exist)", "");
                    }
                    else
                    {
                        tempstring = "";
                    }
                  
                    break;
                }
            }
            result = tempstring;
            return result;
        }
        public string GetFileofRoutine()
        {
            string result = "";
            return result;
        }
        public static long StringToAddress(string InString)
        {
            InString = InString.Replace(" ", "");
            InString = InString.Replace("\t", "");
           
            if(InString.Contains("0x"))
            {
                InString = InString.Replace("0x","");
            }
             if(InString.Contains("/"))
            {
                InString = InString.Remove(InString.IndexOf("/"));
            }
            long intout = 0;
            //intout = HextoSignedLong(InString);
            byte[] byteOut = new byte[3];

            bool negative = false;
            if(InString.Contains("-"))
            {
                negative = true;
                InString = InString.Replace("-", "");
            }
            if (InString != "")
            {
                String strInString = InString;
                int l = strInString.Length;
                if (l >= 6)
                {
                    strInString = strInString.Substring(l - 6, 6);
                }
                int p = l;
                int c = 0;
                while ((p > 0) == true)
                {
                    if ((((l - p) % 2) == 0) & (p != l))
                    {
                        strInString = strInString.Insert(p, " ");
                        c++;
                    }
                    p--;
                }
                //while ((p < l) == true)
                //{
                //    if (((p % 2) == 0) & (p != 0))
                //    {
                //        strInString = strInString.Insert(p + c, " ");
                //        c++;
                //    }
                //    p++;
                //}

                string[] strInputSplit = strInString.Split(' ');

                if(strInputSplit.Length > 3)
                {
                    string[] newstringsplit = new string[3];
                    newstringsplit[0] = strInputSplit[0];
                    newstringsplit[1] = strInputSplit[1];
                    newstringsplit[2] = strInputSplit[2];
                    strInputSplit = newstringsplit;
                }
                //strInString.Split(' ');

                if(InString.Contains("-"))
                {
                    sbyte byteout1 = Convert.ToSByte(strInputSplit[0].ToString());
                }
                else
                {
                    byteOut[0] = Convert.ToByte(strInputSplit[0], 16);
                    if (strInputSplit.Length > 1)
                    {
                        byteOut[1] = Convert.ToByte(strInputSplit[1], 16);
                    }
                    if (strInputSplit.Length > 2)
                    {
                        byteOut[2] = Convert.ToByte(strInputSplit[2], 16);
                    }
                    int mod = 1;
                    for (int i = 1; i < strInputSplit.Length; i++)
                    {
                        mod *= 256;
                    }

                    intout += byteOut[0] * mod;
                    intout += byteOut[1] * mod / 256;
                    intout += byteOut[2] * mod / 65536;

                    if(negative)
                    {
                        intout = 0 - intout;
                    }
                }
            

            }
            else
            {
                byteOut[0] = 0;
            }

            return intout;
        }
        public static long HextoSignedLong(string InString)
        {
            long outlong = 0;
                return outlong;
        }
        public string Indent(int amount)
        {
            string result = "";
            for (int i = 0; i < amount;i++ )
            {
                result += "\t";
            }

                return result;
        }
        public string ReformatAddress(string address)
        {
            address = address.Replace("0x", "");
            if(address.Length < 8)
            {
                address = address.PadLeft(8);
                address = address.Replace(" ", "0");
            }
            return address;
        }
        #endregion

        #endregion

        #region Main Functions
        private void DecodeASM()
        {
            DrawLED(Color.Orange);
            pic_LED.Refresh();

            string strStartPC = txt_StartingAddress.Text;

            int decodeResult = _asmUtility.DecodeASMToFile(txt_InputFile.Text, txt_OutputFile.Text, chk_LittleEndian.Checked, chk_NameRegisters.Checked, strStartPC);
            switch (decodeResult)
            {
                case ASMFileDecoderResult.Success: DrawLED(Color.Green); break;
                case ASMFileDecoderResult.FileOpenError: DrawLED(Color.Red); break;
                case ASMFileDecoderResult.ASMDecodeError: DrawLED(Color.Red); break;
                default: break;
            }
        }
        private void JalFind()
        {
            DrawLED(Color.Orange);
            pic_LED.Refresh();

            #region Get Disassemblies
            SCUSDisassembly = scanresource("LEDecoder.Resources.SCUS Disassembly.txt");
            SCUSLines = ToLines(SCUSDisassembly);

            BATTLEDisassembly = scanresource("LEDecoder.Resources.BATTLE Disassembly.txt");
            BATTLELines = ToLines(BATTLEDisassembly);

            WORLDDisassembly = scanresource("LEDecoder.Resources.WORLD Disassembly.txt");
            WORLDLines = ToLines(WORLDDisassembly);

            WLDCOREDisassembly = scanresource("LEDecoder.Resources.WLDCORE Disassembly.txt");
            WLDCORELines = ToLines(WLDCOREDisassembly);

            REQUIREDisassembly = scanresource("LEDecoder.Resources.REQUIRE Disassembly.txt");
            REQUIRELines = ToLines(REQUIREDisassembly);

            EQUIPDisassembly = scanresource("LEDecoder.Resources.EQUIP Disassembly.txt");
            EQUIPLines = ToLines(EQUIPDisassembly);
            #endregion

            #region Get WIKI resources
            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\SCUSWIKI.txt"))
            {
                SCUSWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\SCUSWIKI.txt");
            }
            else
            {
                SCUSWiki = scanresource("LEDecoder.Resources.SCUSWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\BATTLEWIKI.txt"))
            {
                BATTLEWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\BATTLEWIKI.txt");
            }
            else
            {
                BATTLEWiki = scanresource("LEDecoder.Resources.BATTLEWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\WORLDWIKI.txt"))
            {
                WORLDWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\WORLDWIKI.txt");
            }
            else
            {
                WORLDWiki = scanresource("LEDecoder.Resources.WORLDWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\WLDCOREWIKI.txt"))
            {
                WLDCOREWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\WLDCOREWIKI.txt");
            }
            else
            {
                WLDCOREWiki = scanresource("LEDecoder.Resources.WLDCOREWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\REQUIREWIKI.txt"))
            {
                REQUIREWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\REQUIREWIKI.txt");
            }
            else
            {
                REQUIREWiki = scanresource("LEDecoder.Resources.REQUIREWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\EQUIPWIKI.txt"))
            {
                EQUIPWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\EQUIPWIKI.txt");
            }
            else
            {
                EQUIPWiki = scanresource("LEDecoder.Resources.EQUIPWIKI.txt");
            }
            #endregion

            #region Main Jalfinder Function
            if (txt_OutputFile.Text != "")
            {
                using (StreamWriter sw = File.AppendText(txt_OutputFile.Text))
                {
                    long address = StringToAddress(txt_StartingAddress.Text);
                    CurrentFile = SCUSDisassembly;

                    #region Determine Loaded Files
                    if (cmb_FileofRoutine.SelectedIndex < 2)
                    {
                        CurrentFile += BATTLEDisassembly;
                        if(cmb_FileofRoutine.SelectedIndex == 0)
                        {
                            CurrentFile += REQUIREDisassembly;
                        }
                        else
                        {
                            CurrentFile += EQUIPDisassembly;
                        }
                    }
                    else if (cmb_FileofRoutine.SelectedIndex >= 3)
                    {
                        CurrentFile = WORLDDisassembly;
                    }

                    #endregion

                    #region Find routine address in File
                    string[] CurrentFileLines = ToLines(CurrentFile);
                    string tempstring = "";
                    int lineindex = 0;
                    string startaddress = ReformatAddress(txt_StartingAddress.Text);

                    foreach (string line in CurrentFileLines)
                    {
                        if (line.Contains(startaddress + ":"))
                        {
                            tempstring = line;
                            break;
                        }
                        lineindex++;
                    }

                    if (tempstring == "")
                    {
                        MessageBox.Show("Could not find Base Address");
                        goto fail;
                    }
                    #endregion

                    sw.Write(txt_StartingAddress.Text + ":" + Indent(2)  + GetRoutineDescription(txt_StartingAddress.Text));

                    sw.WriteLine("\r");

                    string description = "";
                    string jaladdress = "";
                    int index = lineindex;
                    for (index = lineindex; index < CurrentFileLines.Length;index++ )
                    {
                        tempstring = CurrentFileLines[index];
                        if(tempstring.Contains("jal"))
                        {
                            if(tempstring.Contains("jalr"))
                            {
                                sw.WriteLine(Indent(1) + "jalr");
                            }
                            else
                            {
                                    jaladdress = tempstring.Substring(25, 8);
                                    description = GetRoutineDescription(jaladdress);
                                    jaladdress = Indent(1) + jaladdress;
                                   
                                    //jaladdress = jaladdress.Replace(" ", "");


                                    sw.WriteLine(jaladdress + ": " + description + "\r");
                            }
                         }
                        else if (tempstring.Contains("jr"))
                        {
                            break;
                        }
                    }
                    sw.WriteLine("\r\n");
                }
                if(JalFindButton.Checked)
                {
                    Process[] processes = Process.GetProcesses();
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName == "notepad")
                        {
                            string title = txt_OutputFile.Text.Substring(txt_OutputFile.Text.LastIndexOf("\\") + 1).Replace(".txt", "");
                            if (p.MainWindowTitle.Contains(title))
                            {
                                p.Kill();
                            }
                        }
                    }
                    Process process = new Process();
                    Process.Start("notepad.exe", txt_OutputFile.Text);
                }
              
            fail: ;
            }
            #endregion

            DrawLED(Color.Green);
        }
        private void PrintHex()
        {
            DrawLED(Color.Orange);
            pic_LED.Refresh();

            try
            {
                using (BinaryReader b = new BinaryReader(File.Open(txt_InputFile.Text, FileMode.Open)))
                {
                    BinaryWriter bw = new BinaryWriter(File.Open(txt_OutputFile.Text, FileMode.OpenOrCreate));
                    string[] output = new string[1];
                    long length = ReadHexOrDec(txt_Length.Text);
                    b.BaseStream.Position = ReadHexOrDec(txt_StartingAddress.Text);
                    for (long i = b.BaseStream.Position; i < ReadHexOrDec(txt_StartingAddress.Text) + length; i++)
                    {
                        byte[] Word = b.ReadBytes((int)ReadHexOrDec(txt_BPLbox.Text));

                        string WordtoPrint = "";

                        if (chk_SpaceBox.Checked)
                        {
                            WordtoPrint = BitConverter.ToString(Word).Replace("-", " ");
                        }
                        else
                        {
                            WordtoPrint = BitConverter.ToString(Word).Replace("-", "");
                        }


                        output[output.Length - 1] = WordtoPrint;
                        Array.Resize(ref output, output.Length + 1);


                    }
                    File.WriteAllLines(txt_OutputFile.Text + ".txt", output);
                    //Use BIN file. derp

                    //string[] Output = new string[Convert.ToInt32(txt_Length.Text) / 4];
                    //int startingaddress = Convert.ToInt32(txt_StartingAddress.Text) / 4;


                    //string line = "";
                    //long address = 0;
                    //for (int j = 0; j < AllLines.Length; j++)
                    //{
                    //    line = AllLines[j].Remove(8);
                    //    address = Convert.ToInt32(line);
                    //    startingaddress = j;
                    //}


                    //for (int i = startingaddress; i < startingaddress + length; i++)
                    //{
                    //    Output[i - startingaddress] = AllLines[i].Substring(10).Remove(18);
                    //    if (chk_LittleEndian.Checked)
                    //    {
                    //        Output[i - startingaddress] = ReverseBytes(Output[i - startingaddress]);
                    //    }
                    //}

                    //File.WriteAllLines(txt_OutputFile.Text + ".txt", Output);
                }

                DrawLED(Color.Green);
            }
            catch (Exception ex)
            {
                DrawLED(Color.Red);
                MessageBox.Show(ex.ToString());
            }
        }
        private void CollapseRoutines()
        {
            DrawLED(Color.Orange);
            pic_LED.Refresh();

            bool started = false;
            string[] AllLines = File.ReadAllLines(txt_InputFile.Text);

            int i = 0;
            int count = 0;

            while (AllLines[i] != null)
            {
                string line = AllLines[i];
                if (line.IndexOf(":") > 0)
                {
                    if (started && line.Contains("jr r31") && AllLines[i + 2] == "")
                    {
                        started = false;
                        AllLines = Delete(AllLines, i);
                        AllLines[i - 1] += line.Remove(9);
                        AllLines = Delete(AllLines, i);
                        goto end;
                    }
                    if (started)
                    {
                        AllLines = Delete(AllLines, i);
                    }
                    if (!started)
                    {
                        if (line[0] == '0')
                        {
                            started = true;
                            AllLines[i] = line.Remove(8) + " - ";
                        }
                        i++;
                    }
                end: ;
                }
                else if (started)
                {
                    AllLines = Delete(AllLines, i);
                }
                else
                {
                    started = false;
                    i++;
                }
            }
            string[] NewAllLines = AllLines;
            File.WriteAllLines(txt_OutputFile.Text + ".txt", AllLines);
            //AllLines.RemoveEmptyLines();
            DrawLED(Color.Green);
        }
        private void AutoNotate()
        {

            //Autonotator An = new Autonotator(this);
            //An.Show(); //Set initial register values

            JalFind();
            //get reference files
            #region Get Disassemblies
            SCUSDisassembly = scanresource("LEDecoder.Resources.SCUS Disassembly.txt");
            SCUSLines = ToLines(SCUSDisassembly);

            BATTLEDisassembly = scanresource("LEDecoder.Resources.BATTLE Disassembly.txt");
            BATTLELines = ToLines(BATTLEDisassembly);

            WORLDDisassembly = scanresource("LEDecoder.Resources.WORLD Disassembly.txt");
            WORLDLines = ToLines(WORLDDisassembly);

            WLDCOREDisassembly = scanresource("LEDecoder.Resources.WLDCORE Disassembly.txt");
            WLDCORELines = ToLines(WLDCOREDisassembly);

            REQUIREDisassembly = scanresource("LEDecoder.Resources.REQUIRE Disassembly.txt");
            REQUIRELines = ToLines(REQUIREDisassembly);

            EQUIPDisassembly = scanresource("LEDecoder.Resources.EQUIP Disassembly.txt");
            EQUIPLines = ToLines(EQUIPDisassembly);
            #endregion

            #region Get WIKI resources
            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\SCUSWIKI.txt"))
            {
                SCUSWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\SCUSWIKI.txt");
            }
            else
            {
                SCUSWiki = scanresource("LEDecoder.Resources.SCUSWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\BATTLEWIKI.txt"))
            {
                BATTLEWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\BATTLEWIKI.txt");
            }
            else
            {
                BATTLEWiki = scanresource("LEDecoder.Resources.BATTLEWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\WORLDWIKI.txt"))
            {
                WORLDWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\WORLDWIKI.txt");
            }
            else
            {
                WORLDWiki = scanresource("LEDecoder.Resources.WORLDWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\WLDCOREWIKI.txt"))
            {
                WLDCOREWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\WLDCOREWIKI.txt");
            }
            else
            {
                WLDCOREWiki = scanresource("LEDecoder.Resources.WLDCOREWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\REQUIREWIKI.txt"))
            {
                REQUIREWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\REQUIREWIKI.txt");
            }
            else
            {
                REQUIREWiki = scanresource("LEDecoder.Resources.REQUIREWIKI.txt");
            }

            if (File.Exists(Application.StartupPath + "\\JalFinder Resources\\EQUIPWIKI.txt"))
            {
                EQUIPWiki = scanfile(Application.StartupPath + "\\JalFinder Resources\\EQUIPWIKI.txt");
            }
            else
            {
                EQUIPWiki = scanresource("LEDecoder.Resources.EQUIPWIKI.txt");
            }
            #endregion

            #region Main Autonotator Function
            if (txt_OutputFile.Text != "")
            {
                using (StreamWriter sw = File.CreateText(txt_OutputFile.Text))
                {
                    string tempstring = "";
                    sw.WriteLine(txt_StartingAddress.Text +": " + Indent(3) + GetRoutineDescription(txt_StartingAddress.Text) + "\n");
                    InitializeRegisters();
                    PrintRegisterDescriptions(sw);
                  

                    string RoutineDisassembly = GetRoutineFromDisassembly();
                    string[] RoutineLines = ToLines(RoutineDisassembly);

                    int i = 0;

                    //For each Command
                    foreach(string line in RoutineLines)
                    {
                        if(jalcommandcounter != 0)
                        {
                            jalcommandcounter++;
                            if(jalcommandcounter == 2)
                            {
                                Registers[2].Description = "ReturnValue";
                                jalcommandcounter = 0;
                            }
                        }

                        tempstring = Notate(line,i);
                        sw.WriteLine(line + tempstring + "\n");
                    }
                    int count = 0;
                    //insert spaces after jump and branch commands
                    for(i=0;i < RoutineLines.Length + count;i++)
                    {
                        foreach(int index in linestoinsert)
                        {
                            if (i + count == index + count)
                            {
                                RoutineLines = Insert(RoutineLines, i + count);
                                count++;
                            }
                        }
                    }

                    //Change Input values that were figured out during routine

                }

                Process[] processes = Process.GetProcesses();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName == "notepad")
                    {
                        string title = txt_OutputFile.Text.Substring(txt_OutputFile.Text.LastIndexOf("\\") + 1).Replace(".txt", "");
                        if (p.MainWindowTitle.Contains(title))
                        {
                            p.Kill();
                        }
                    }
                }
                Process process = new Process();
                Process.Start("notepad.exe", txt_OutputFile.Text);
            }
            #endregion
        }
        #endregion

        #region Autonotator Routines
        public string GetRoutineFromDisassembly()
        {
            long address = StringToAddress(txt_StartingAddress.Text);
            CurrentFile = SCUSDisassembly;

            #region Determine Loaded Files
            if (cmb_FileofRoutine.SelectedIndex < 2)
            {
                CurrentFile += BATTLEDisassembly;
                if (cmb_FileofRoutine.SelectedIndex == 0)
                {
                    CurrentFile += REQUIREDisassembly;
                }
                else
                {
                    CurrentFile += EQUIPDisassembly;
                }
            }
            else if (cmb_FileofRoutine.SelectedIndex >= 3)
            {
                CurrentFile = WORLDDisassembly;
            }

            #endregion

            #region Find routine address in File
            string[] CurrentFileLines = ToLines(CurrentFile);
            string tempstring = "";
            int lineindex = 0;
            bool foundstart = false;

            string startaddress = ReformatAddress(txt_StartingAddress.Text);

            foreach (string line in CurrentFileLines)
            {
                if (foundstart)
                {
                    tempstring += line + "\n";
                    if(line.Contains("jr r31"))
                    {
                        break;
                    }
                }
                if (line.Contains(startaddress + ":") && !foundstart)
                {
                    foundstart = true;
                    tempstring = line + "\n";
                }
                lineindex++;
            }

            if (tempstring == "")
            {
                MessageBox.Show("Could not find Base Address");
            }
            #endregion
            return tempstring;
        }
        public string[] Insert(string[] Source, int lineindex)
        {
            string[] newSource = new string[Source.Length + 1];
            int indexfound = 0;
            for(int j = 0;j < newSource.Length;j++)
            {
                if(j != lineindex)
                {
                    newSource[j] = Source[j - indexfound];
                }
                else
                {
                    indexfound = 1;
                    newSource[j] = "\n";
                }
            }
            return newSource;
        }
        public string ExtractCommand(string command)
        {
            #region Get command
            if (command.Contains("j 0x"))
            {
                command = "j";
            }
            else if(command.Contains("jal 0x"))
            {
                command = "jal";
            }
            else if (command.Contains("jalr"))
            {
                command = "jalr";
            }
            else if (command.Contains("jr"))
            {
                command = "jr";
            }
            else if (command.Contains("beq"))
            {
                command = "beq";
            }
            else if (command.Contains("bne"))
            {
                command = "bne";
            }
            else if (command.Contains("sltiu"))
            {
                command = "sltiu";
            }
            else if (command.Contains("slti"))
            {
                command = "slti";
            }
            else if (command.Contains("slt"))
            {
                command = "slt";
            }
            else if (command.Contains("lui"))
            {
                command = "lui";
            }
            else if (command.Contains("lwl"))
            {
                command = "lwl";
            }
            else if (command.Contains("lwr"))
            {
                command = "lwr";
            }
            else if (command.Contains("lw"))
            {
                command = "lw";
            }
            else if (command.Contains("lhu"))
            {
                command = "lhu";
            }
            else if (command.Contains("lh"))
            {
                command = "lh";
            }
            else if (command.Contains("lbu"))
            {
                command = "lbu";
            }
            else if (command.Contains("lb"))
            {
                command = "lb";
            }
            else if (command.Contains("swr"))
            {
                command = "swr";
            }
            else if (command.Contains("swl"))
            {
                command = "swl";
            }
            else if (command.Contains("sw"))
            {
                command = "sw";
            }
            else if (command.Contains("sh"))
            {
                command = "sh";
            }
            else if (command.Contains("sb"))
            {
                command = "sb";
            }
            else if (command.Contains("addiu"))
            {
                command = "addiu";
            }
            else if (command.Contains("addu"))
            {
                command = "addu";
            }
            else if (command.Contains("subu"))
            {
                command = "subu";
            }
            else if (command.Contains("sub"))
            {
                command = "sub";
            }
            else if (command.Contains("sla"))
            {
                command = "sla";
            }
            else if (command.Contains("sll"))
            {
                command = "sll";
            }
            else if (command.Contains("srl"))
            {
                command = "srl";
            }
            else if (command.Contains("sra"))
            {
                command = "sra";
            }
            else if (command.Contains("multu"))
            {
                command = "multu";
            }
            else if (command.Contains("mult"))
            {
                command = "mult";
            }
            else if (command.Contains("divu"))
            {
                command = "divu";
            }
            else if (command.Contains("div"))
            {
                command = "div";
            }
            else if (command.Contains("mflo"))
            {
                command = "mflo";
            }
            else if (command.Contains("mfhi"))
            {
                command = "mfhi";
            }
            else if (command.Contains("ori"))
            {
                command = "ori";
            }
            else if (command.Contains("or"))
            {
                command = "or";
            }
            else if (command.Contains("andi"))
            {
                command = "andi";
            }
            else if (command.Contains("and"))
            {
                command = "and";
            }
            else if (command.Contains("xori"))
            {
                command = "xori";
            }
            else if (command.Contains("xor"))
            {
                command = "xor";
            }
            else if (command.Contains("nor"))
            {
                command = "nor";
            }
            return command;
            #endregion
        }
        public void SetInput(string register, string maindata)
        {
            for(int i = 0;i < Registers.Length;i++)
            {
                if(Registers[i].Name == register.Remove(register.IndexOf("Input") - 1))
                {
                    Registers[i].Inputis = maindata + "ID";
                }
            }
        }
        public void GetReturnValue(long JalAddress)
        {
            switch(JalAddress)
            {
                case 0x001810a0: // Get Map Tile Data
                    Registers[2].AttachDescriptiontoRegister("Map Location Tile ID");
                    Registers[2].Value = 0;
                    break;
            }
        }
       
        public string GetDescription(Register Reg, long Offset)
        {
            long MainAddress = Reg.Value;
            MainAddress = MainAddress & 0x7FFFFFFF;
            string result = "";
            long wholeaddress = MainAddress + Offset;
            foreach (MainAddress Main in MainAddresses)
            {
                if (Main.Address == MainAddress || Main.Address == wholeaddress)
                {
                    Reg.Mainaddress = Main;
                    if(Main.Frame != null)
                    {
                        if (Main.Frame.Length == Offset)
                        {
                            result = "this " + Main.Description;
                        }
                        else
                        {
                            result = GetSubDataDescription(MainAddress, Offset);
                            Reg.AttachDatatoRegister(MainAddress, Offset, this);
                        }
                    }
                    if(result == "")
                    {
                        result = Main.Description;
                    }
                }
            }
            return result;
        }
        public long GetFrameSize(long MainAddress)
        {
            MainAddress = MainAddress & 0x7FFFFFFF;
            long result = 0;
            foreach (MainAddress Main in MainAddresses)
            {
                if (Main.Address == MainAddress)
                {
                   
                    if (Main.Frame != null)
                    {
                        result = Main.Frame.Length;
                    }

                }

            }
            return result;
        }
        public string GetSubDataDescription(long MainAddress, long Offset)
        {
            string result = "";
            foreach(MainAddress Main in MainAddresses)
            {
                if (Main.Address == MainAddress)
                {
                    foreach(SubData Data in Main.Frame)
                    {
                        if(Data != null)
                        {
                            if (Data.offsetaddress == Offset)
                            {
                                result = Data.description;
                            }
                        }
                        
                    }
                }
                
            }
            return result;
        }
        public string GetMainAddressDescription(long MainAddress)
        {
            string result = "";
            foreach (MainAddress Main in MainAddresses)
            {
                if (Main.Address == MainAddress)
                {
                    result = Main.Description;
                }
            }
            return result;
        }
        public string SeeifIsSubset(long MainAddress, long Offset)
        {
            foreach(MainAddress Main in MainAddresses)
            {
                MainAddress &= 0x7fffffff;
                if(Main.Frame != null)
                {
                    if (Main.Address == MainAddress && Main.Frame.Length >= Offset)
                    {

                        if(Main.Frame[Offset] != null)
                        {
                            if(Main.Frame[Offset].IsSubset)
                            {
                                return Main.Frame[Offset].SubsetDescription;
                            }
                        }
                    }
                }
            }
            return "";
        }
        
        public void StoreValueinMainAddress(long address, long value)
        {
         
        }

        public void GetDataNotes()
        {
            if(File.Exists(Application.StartupPath + "\\UnitDataResource.txt"))
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\UnitDataResource.txt"))
                {
                    string file = sr.ReadToEnd();
                    string[] fileLines = ToLines(file);

                    //For each line in the data file
                    for(int i = 0;i < fileLines.Length;i++)
                    {
                        string line = fileLines[i];
                        string[] linesplit = fileLines[i].Split(' ');
                        //If MainAddress Found
                        if(linesplit[0].StartsWith("80") && linesplit[0].Length == 8)
                        {
                            MainAddresses = Add(linesplit[0], MainAddresses);
                             int current = MainAddresses.Length - 1;
                             MainAddresses[current].Description = line.Substring(line.IndexOf("-") + 2);
                            if(MainAddresses[current].Description.Contains("|"))
                             MainAddresses[current].Description = MainAddresses[current].Description.Remove(MainAddresses[current].Description.IndexOf("|"));

                            foreach(string s in linesplit) //linesplit = MainAddress line
                            {
                               
                                if(s.Contains("frame=0x"))
                                {
  MainAddresses[current].AddFrame(StringToAddress(s.Replace("frame=","")));
                                }
                                else if(s.Contains("frame="))
                                {
                                    MainAddresses[current].AddFrame(Convert.ToInt32(s.Replace("frame=", "")));
                                }
                                if (s.Contains("sections=0x"))
                                {
                                    MainAddresses[current].NumberofSections = (int)StringToAddress(s.Replace("sections=", ""));
                                }
                                else if(s.Contains("sections="))
                                {
                                    MainAddresses[current].NumberofSections = Convert.ToInt32(s.Replace("sections=", ""));
                                }
                                else if(s.Contains("pointedaddress="))
                                {
                                    MainAddresses[current].PointedAddress = StringToAddress(s.Substring(s.IndexOf("pointedaddress=") + 15));
                                }
                               

                            }
                           i++;
                       
                            //Begin looking for data
                           while (!fileLines[i].StartsWith("80") && fileLines[i] != "")
                            {
                                line = fileLines[i].Replace("\t", "");   // line = subdata line
                                linesplit = fileLines[i].Split(' ');     

                               //If Subdata Found
                                if(line.Contains(":"))
                                {

                                    int currentindex = (int)StringToAddress(line.Remove(line.IndexOf(":")));
                                    MainAddresses[current].Frame[currentindex] = new SubData();
                                    MainAddresses[current].Frame[currentindex].offsetaddress = currentindex;
                                    //MainAddresses[current].Frame[currentindex].description = line.Substring(line.IndexOf(":") + 2);

                                    //Set Description
                                    if (line.Length > line.IndexOf(":") + 2)
                                        MainAddresses[current].Frame[currentindex].description =
                                            line.Substring(line.IndexOf(":") + 2);
                                    else
                                    {
                                        MainAddresses[current].Frame[currentindex].description = "";
                                    }
                                    //Remove tags if present
                                    if(line.Contains("|"))
                                    {
                                        MainAddresses[current].Frame[currentindex].description = MainAddresses[current].Frame[currentindex].description.Remove(MainAddresses[current].Frame[currentindex].description.IndexOf("|"));
                                    }
                                    //Look for size, list, and subset
                                    foreach(string s in linesplit)
                                    {
                                        if (s.Contains("size"))
                                        {
                                            int numberindex = s.IndexOf("size") + 4;
                                            MainAddresses[current].Frame[currentindex].size = Convert.ToInt32(s.Substring(numberindex));
                                        }
                                        if (s.Contains("list="))
                                        {
                                             MainAddresses[current].Frame[currentindex].GetList(s.Substring(s.IndexOf("list=") + 5));
                                        }
                                       
                                        if (fileLines[i].Contains("subset="))
                                        {
                                            MainAddresses[current].Frame[currentindex].IsSubset = true;
                                            MainAddresses[current].Frame[currentindex].SubsetDescription = fileLines[i].Substring(fileLines[i].IndexOf("subset=") + 7);
                                        }
                                    }
                                    i++;                                    //Move to value lines

                                    if (!fileLines[i].Contains("list"))             //if list tag is found, goto next SubData
                                    {
                                        while (!fileLines[i].Contains(":") && i < fileLines.Length && fileLines[i] != "" && !fileLines[i].Replace("\t","").StartsWith("80"))
                                        {
                                            line = fileLines[i].Replace("\t","");
                                            linesplit = fileLines[i].Split(' ');

                                            if(line.Contains("value="))
                                            {
                                                if(MainAddresses[current].Frame[currentindex].Values == null)
                                                {
                                                    MainAddresses[current].Frame[currentindex].Values = new SubData.Value[1];
                                                }
                                                MainAddresses[current].Frame[currentindex].AddValue();
                                                foreach(string s in linesplit)
                                                {
                                                    if (s.Contains("0x"))
                                                    {
                                                        MainAddresses[current].Frame[currentindex].Values[MainAddresses[current].Frame[currentindex].Values.Length - 1].value
                                                          = (int)StringToAddress(s.Replace("\t",""));
                                                    }
                                                    if(s.Contains("value="))
                                                    {
                                                        string tempstring = s.Substring(s.IndexOf("value=") + 6);
                                                    }
                                                   
                                                }
                                            }
                                            //Set default value

                                            i++;
                                        }
                                    }
                                    i--;

                                }//SubData Found
                                i++;
                                if(i == fileLines.Length)
                                {
                                    break;
                                }

                           }
                           i--;

                        }//Mainaddress found

                    }

                }
               
            }
        }
        public string Notate(string line, int lineindex)
        {
            string Notation = "";
            string[] linesplit = line.Split(' ', ',');

            int sourceregister = 0;
            int targetregister = 0;
            int comparedregister = 0;
            long immediate = 0;
            string command = ExtractCommand(line);
            string description = "";
            long address = 0;

            Notation = Indent(3);

            switch (command)
            {
                #region Jump/Branch Commands
                case "jal":
                    Notation += GetRoutineDescription(line.Substring(23));
                    linestoinsert[linestoinsert.Length - 1] = lineindex + 2;
                    Array.Resize(ref linestoinsert, linestoinsert.Length + 1);
                    GetReturnValue(StringToAddress(linesplit[3]));
                    break;
                case "jalr":
                    Notation += "Jump and Link";

                    linestoinsert[linestoinsert.Length - 1] = lineindex + 2;
                    Array.Resize(ref linestoinsert, linestoinsert.Length + 1);
                    break;
                case "j":
                    linestoinsert[linestoinsert.Length - 1] = lineindex + 2;
                    Array.Resize(ref linestoinsert, linestoinsert.Length + 1);
                    break;
                case "jr r31":
                    Notation += "Jump to Return Address";
                    break;
                case "jr":
                    Notation += "Jump to Address";
                    break;
                #region bne
                case "bne":
                    sourceregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    immediate = StringToAddress(linesplit[5]);

                    #region special notations
                    if (Registers[sourceregister].SpecialCommand == "slt" && comparedregister == 0)
                    {
                        Notation += "Branch if so";
                        Registers[sourceregister].SpecialCommand = "";
                    }
                    else if (Registers[sourceregister].SpecialCommand == "and" && comparedregister == 0)
                    {
                        Notation += "Branch if " + Registers[sourceregister].Description;
                        Registers[sourceregister].SpecialCommand = "";
                    }
                    #endregion
                    else
                    {
                        Notation += "Branch if " + Registers[sourceregister].Description + " != " +
                                           Registers[comparedregister].Description;
                    }

                    linestoinsert[linestoinsert.Length - 1] = lineindex + 2;
                    Array.Resize(ref linestoinsert, linestoinsert.Length + 1);
                    break;
                #endregion
                #region beq
                case "beq":
                    sourceregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    immediate = StringToAddress(linesplit[5]);

                    #region special notations
                    if (Registers[sourceregister].SpecialCommand == "slt" && comparedregister == 0)
                    {
                        Notation += "Branch if not";
                        Registers[sourceregister].SpecialCommand = "";
                    }
                    else if (Registers[sourceregister].SpecialCommand == "and" && comparedregister == 0)
                    {
                        Notation += "Branch if " + Registers[sourceregister].Description;
                        Registers[sourceregister].SpecialCommand = "";
                    }
                    #endregion

                    else
                    {
                        Notation += "Branch if " + Registers[sourceregister].Description + " == " +
                        Registers[comparedregister].Description;
                    }


                    linestoinsert[linestoinsert.Length - 1] = lineindex + 2;
                    Array.Resize(ref linestoinsert, linestoinsert.Length + 1);
                    break;
                #endregion

                #endregion

                #region slt Commands
                #region slt
                case "slt":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));

                    Notation += "Set if " + Registers[sourceregister].Description + " < " +
                        Registers[comparedregister].Description;

                    if (Registers[sourceregister].Value < Registers[comparedregister].Value)
                    {
                        Registers[targetregister].Value = 1;
                    }
                    else
                    {
                        Registers[targetregister].Value = 0;
                    }

                    Registers[targetregister].SpecialCommand = "slt";
                    Registers[targetregister].originalvalue = false;

                    Registers[targetregister].Description = "Result";

                    break;
                #endregion
                #region slti
                case "slti":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    immediate = (int)StringToAddress(linesplit[5]);

                    Notation += "Set if " + Registers[sourceregister].Description + " < " +
                        immediate.ToString();

                    if (Registers[sourceregister].Value < immediate)
                    {
                        Registers[targetregister].Value = 1;
                    }
                    else
                    {
                        Registers[targetregister].Value = 0;
                    }

                    Registers[targetregister].SpecialCommand = "slt";
                    Registers[targetregister].originalvalue = false;

                    Registers[targetregister].Description = "(bool)" + Registers[sourceregister].Description +
                                            " < 0x" + immediate.ToString("X") + "(" + immediate.ToString() + ")";
                    break;
                #endregion
                #region sltiu
                case "sltiu":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    immediate = (int)StringToAddress(linesplit[5]);

                    if (Registers[sourceregister].Value < immediate)
                    {
                        Registers[targetregister].Value = 1;
                    }
                    else
                    {
                        Registers[targetregister].Value = 0;
                    }

                    Registers[targetregister].Description = "Result";

                    Registers[targetregister].SpecialCommand = "slt";
                    Registers[targetregister].originalvalue = false;

                    Notation += "Set if " + Registers[sourceregister].Description + " < " +
                        immediate.ToString();
                    break;
                #endregion
                #endregion

                #region Load Commands
                #region lui
                case "lui":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    immediate = StringToAddress(linesplit[4]);

                    //Notation += "r" + targetregister.ToString() + " = 0x" +
                    //    immediate.ToString("X") + "0000";
                    Registers[targetregister].Description = "0x" + immediate.ToString("X");
                    Registers[targetregister].Value = immediate * 65536;
                    Registers[targetregister].originalvalue = false;
                    Registers[targetregister].multiplier = 0;

                    break;
                #endregion
                #region lw
                case "lw":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Substring(7).Replace(")", "").Replace("(", "").Replace("r", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));
                    address = Registers[sourceregister].Value + immediate;

                    description = GetDescription(Registers[sourceregister], immediate);
                    if (sourceregister == 29)
                    {
                        description = "Stack Pointer" + immediate.ToString("X");
                    }
                    
                    if (description != "")
                    {
                        if (Registers[sourceregister].Mainaddress != null)
                        {
                            if (Registers[sourceregister].Mainaddress.PointedAddress != 0)
                            {
                                Registers[targetregister].Value = Registers[sourceregister].Mainaddress.PointedAddress;
                                if(GetMainAddressDescription(Registers[targetregister].Value) != "")
                                {
                                    description = GetMainAddressDescription(Registers[targetregister].Value);
                                }
                                else
                                {
                                    Registers[targetregister].SeeifIsSubsetbyAddress(Registers[targetregister].Value,this);
                                }
                                
                            }
                        }
                     
                        Notation += "Load " + description;
                        Registers[targetregister].Description = description;
                    }
                    else
                    {
                        Notation += "Load word " + " from ????"; //+ address.ToString("X");
                        Registers[targetregister].Description = "??";
                    }

                    Registers[targetregister].originalvalue = true;
                    Registers[targetregister].multiplier = 0;

                    //Notation += "r" + targetregister.ToString() + " = " +
                    //     (immediate * 256 * 256).ToString("X").Remove(4);
                    //look up new data location
                    break;
                #endregion
                #region lh
                case "lh":
                case "lhu":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Substring(7).Replace(")", "").Replace("(", "").Replace("r", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));

                    address = Registers[sourceregister].Value + immediate;

                    description = GetDescription(Registers[sourceregister], immediate);
                    if (sourceregister == 29)
                    {
                        description = "Stack Pointer" + " + " + immediate.ToString("X");
                    }
                    if (description != "")
                    {
                        Notation += "Load " + description;
                        Registers[targetregister].Description = description;
                    }
                    else
                    {
                        Notation += "Load half " + ((Int16)immediate).ToString("X") + " from " + address.ToString("X");
                        Registers[targetregister].Description = "??";
                    }

                    Registers[targetregister].originalvalue = true;
                    Registers[targetregister].multiplier = 0;

                    //Notation += "r" + targetregister.ToString() + " = " +
                    //    (immediate * 256 * 256).ToString("X");
                    break;
                #endregion
                #region lb
                case "lb":
                case "lbu":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    sourceregister = Convert.ToInt32(linesplit[4].Substring(7).Replace(")", "").Replace("(", "").Replace("r", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));



                    address = Registers[sourceregister].Value + immediate;

                    description = GetDescription(Registers[sourceregister], immediate);
                    if (sourceregister == 29)
                    {
                        description = "Stack" + " + " + immediate.ToString("X");
                    }
                    if (description != "")
                    {
                        Notation += "Load " + description;
                        Registers[targetregister].Description = description;
                    }
                    else
                    {
                        Notation += "Load byte " + ((Int16)immediate).ToString("X") + " from " + address.ToString("X");
                        Registers[targetregister].Description = "??";
                    }

                    Registers[targetregister].multiplier = 0;

                    Registers[targetregister].originalvalue = true;
                    //    Notation += "Load " + SubDataDescription + " from " +  MainAddressDescription;
                    //Notation += "Load byte " + ((Int16)immediate).ToString("X") + " from " + Registers[targetregister].Description;

                    //look up new data location
                    break;
                #endregion

                #endregion

                #region lwl/lwr/swl/swr
                case "lwl":
                    break;
                case "lwr":
                    break;
                case "swr":
                    break;
                case "swl":
                    break;
                #endregion

                #region Store Commands
                #region sw
                case "sw":
                    sourceregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[4].Substring(8).Replace(")", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));

                    address = Registers[targetregister].Value + immediate;
                    if (GetDescription(Registers[targetregister], immediate) != "")
                    {
                        description = GetDescription(Registers[targetregister], immediate);
                    }
                    else
                    {
                        description = address.ToString("X");
                    }


                    if (targetregister == 29)
                    {
                        Notation += "Store " + Registers[sourceregister].Description + " into "
             + Registers[targetregister].Description;
                    }
                    else
                    {
                        Notation += "Store " + description;
                    }

                    break;
                #endregion
                #region sh
                case "sh":
                    sourceregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[4].Substring(8).Replace(")", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));

                    address = Registers[targetregister].Value + immediate;
                    if (GetDescription(Registers[targetregister], immediate) != "")
                    {
                        description = GetDescription(Registers[targetregister], immediate);
                    }
                    else
                    {
                        description = address.ToString("X");
                    }


                    if (targetregister == 29)
                    {
                        Notation += "Store " + Registers[sourceregister].Description + " into "
             + Registers[targetregister].Description;
                    }
                    else
                    {
                        Notation += "Store " + description;
                    }

                    break;
                #endregion
                #region sb
                case "sb":
                    sourceregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[4].Substring(8).Replace(")", ""));
                    immediate = StringToAddress(linesplit[4].Remove(linesplit[4].IndexOf("(")));

                    address = Registers[targetregister].Value + immediate;
                    if(Registers[targetregister].SubsetOffset != 0)
                    {
                        immediate += Registers[targetregister].SubsetOffset;
                    }
                    if (GetDescription(Registers[targetregister], immediate) != "")
                    {
                        description = GetDescription(Registers[targetregister], immediate);
                    }
                    else
                    {
                        description = address.ToString("X");
                    }


                    if (targetregister == 29)
                    {
                        Notation += "Store " + Registers[sourceregister].Description + " into "
             + Registers[targetregister].Description;
                    }
                    else
                    {
                        Notation += "Store " + description;
                    }

                    break;
                #endregion
                #endregion

                #region Arithmatic Commands
                #region addu
                case "addu":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));

                    Notation += "r" + targetregister.ToString() + " = ";
                    #region Special cases
                    //addu r2, r2, r0
                    if ((sourceregister != 0 && comparedregister == 0))
                    {
                        Notation += Registers[sourceregister].Description;
                        Registers[targetregister].Description = Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value;
                        Registers[targetregister].originalvalue = Registers[sourceregister].originalvalue;
                    }
                    //addu r2, r0, r2
                    else if (comparedregister != 0 && sourceregister == 0)
                    {
                        Notation += Registers[comparedregister].Description;
                        Registers[targetregister].Description = Registers[comparedregister].Description;
                        Registers[targetregister].Value = Registers[comparedregister].Value;
                        Registers[targetregister].originalvalue = Registers[comparedregister].originalvalue;
                    }
                    // addu r2, r0, r0
                    else if (sourceregister == 0 && comparedregister == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Description = "";
                        Registers[targetregister].Value = 0;
                    }
                    //Calculating frame pointer
                    else if (Registers[sourceregister].multiplier > 0 && Registers[comparedregister].originalvalue)
                    {
                        Registers[sourceregister].multiplier += 1;
                        Notation += Registers[sourceregister].Description + " * 0x" + Registers[sourceregister].multiplier.ToString("X") + " (" + Registers[sourceregister].multiplier.ToString() + ")";
                    }
                    //adding frame pointer to base address
                    else if (Registers[sourceregister].multiplier > 0 && GetDescription(Registers[comparedregister], 0) != "")
                    {
                        Registers[targetregister].Value = Registers[comparedregister].Value;

                        Notation += GetDescription(Registers[targetregister], Registers[sourceregister].multiplier);

                        Registers[targetregister].Description = Registers[targetregister].Mainaddress.Description;

                        if (Registers[sourceregister].Description.Contains("Input"))
                        {
                            SetInput(Registers[sourceregister].Description, GetDescription(Registers[comparedregister], 0));
                            //Set Input Value based on what we found in routine.
                        }
                        // Registers[sourceregister].Description.Remove(Registers[sourceregister].Description.IndexOf("*");
                    }
                    else if (comparedregister != 0 && sourceregister != 0)
                    {
                        Notation += Registers[comparedregister].Description + " + " + Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value + Registers[comparedregister].Value;

                        if (Registers[sourceregister].Description.StartsWith("0x") && Registers[comparedregister].Description.StartsWith("0x"))
                        {
                            Registers[targetregister].Description = Registers[targetregister].Value.ToString("X");
                        }

                        else
                        {
                            Registers[targetregister].Description = Registers[comparedregister].Description + " + " + Registers[sourceregister].Description; ;
                        }
                        Registers[targetregister].originalvalue = false;
                    }

                    #endregion
                    break;
                #endregion
                #region addiu
                case "addiu":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    immediate = StringToAddress(linesplit[5]);



                    #region Stack Adjustment
                    if (sourceregister == 29 && targetregister == 29)
                    {
                        if (immediate >= 0x8000)
                        {
                            Notation += "Make space on stack";
                            //Registers[29].Value += immediate;
                            Registers[29].Description = "Stack";
                            break;
                        }
                        else
                        {
                            Notation += "Restore stack pointer";
                            //Registers[29].Value -= immediate;
                            Registers[29].Description = "Stack";
                            break;
                        }
                    }
                    #endregion

                    if (immediate > 0x7FFF)
                    {
                        Registers[sourceregister].Value -= 0x10000;
                        // immediate -= 0x8000;
                    }

                    Registers[targetregister].Value = Registers[sourceregister].Value + immediate;

                    if (SeeifIsSubset(Registers[sourceregister].Value, immediate) != "")
                    {
                        Registers[targetregister].Description = SeeifIsSubset(Registers[sourceregister].Value, immediate);
                    }
                    else if (GetDescription(Registers[targetregister], 0) != "")
                    {
                        Registers[targetregister].Description = GetDescription(Registers[targetregister], immediate);
                    }
                    else if (Registers[sourceregister].Description.StartsWith("0x"))
                    {
                        Registers[targetregister].Description = Registers[targetregister].Value.ToString("X");
                    }



                    Registers[targetregister].originalvalue = false;

                    Notation += Registers[targetregister].Description;

                    Registers[targetregister].Description = "0x" + Registers[targetregister].Value.ToString("X");

                    //Registers[targetregister].Description = Registers[sourceregister].Description + zeromod + immediate.ToString("X");

                    break;
                #endregion
                #region subu
                case "subu":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));

                    if (Registers[sourceregister].multiplier > 0 && Registers[comparedregister].originalvalue)
                    {
                        Registers[targetregister].multiplier -= 1;
                        Notation += Registers[sourceregister].Description + " * 0x" + Registers[sourceregister].multiplier.ToString("X") + " (" + Registers[sourceregister].multiplier.ToString() + ")";
                        Registers[targetregister].Description = Registers[sourceregister].Description + " * 0x" + Registers[sourceregister].multiplier.ToString("X") + " (" + Registers[sourceregister].multiplier.ToString() + ")";
                    }
                    else if (Registers[sourceregister].multiplier > 0 && Registers[comparedregister].multiplier > 0)
                    {
                        Registers[sourceregister].multiplier -= 1;
                        Notation += Registers[sourceregister].Description + " * 0x" + Registers[sourceregister].multiplier.ToString("X") + " (" + Registers[sourceregister].multiplier.ToString() + ")";
                        Registers[targetregister].Description = Registers[sourceregister].Description + " * 0x" + Registers[sourceregister].multiplier.ToString("X") + " (" + Registers[sourceregister].multiplier.ToString() + ")";
                    }
                    else if (Registers[sourceregister].multiplier == 0)
                    {
                        Notation += "r" + targetregister.ToString() + " = " +
                        Registers[sourceregister].Description + " - " + Registers[comparedregister].Description;
                        Registers[targetregister].Description = Registers[sourceregister].Description + " - " + Registers[comparedregister].Description;
                    }

                    Registers[targetregister].originalvalue = false;
                    Registers[targetregister].Value = Registers[sourceregister].Value - Registers[comparedregister].Value;
                    break;
                #endregion
                #region sll
                case "sllv":
                case "sll":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    immediate = StringToAddress(linesplit[5]);

                    Registers[targetregister].Value = Registers[sourceregister].Value * Exponent(2, immediate);
                    //If it's the first shift
                    if (Registers[targetregister].multiplier == 0)
                    {
                        if (immediate % 0x08 == 0)
                        {
                            Notation += "Shift left " + (immediate / 0x08).ToString() + " bytes(*0x" + Exponent(2, immediate).ToString("X") + ")" + " (*" + Exponent(2, immediate).ToString() + ")";
                            Registers[targetregister].Description = Registers[sourceregister].Description;
                        }
                        else
                        {
                            Notation += "Shift left " + (immediate).ToString() + " bits (*0x" + Exponent(2, immediate).ToString("X") + ")" + " (*" + Exponent(2, immediate).ToString() + ")"; ;
                            Registers[targetregister].Description = Registers[sourceregister].Description;
                        }
                        Registers[targetregister].multiplier = (int)Exponent(2, immediate);
                    }
                    //if it's not the first shift
                    else if (Registers[targetregister].multiplier != 0)
                    {
                        Registers[targetregister].multiplier *= (int)Exponent(2, immediate);
                        if (Registers[targetregister].Description.Contains("*"))
                        {
                            Registers[targetregister].Description = Registers[targetregister].Description.Remove(Registers[targetregister].Description.IndexOf('*'));
                            Registers[targetregister].Description += "* 0x" + Registers[targetregister].multiplier.ToString("X") + " (* " + Registers[targetregister].multiplier.ToString() + ")";
                            Notation += Registers[targetregister].Description; //+"* 0x" + Registers[targetregister].multiplier.ToString("X") + " (*" + Registers[targetregister].multiplier.ToString() + ")";
                        }
                        else
                        {
                            Notation += Registers[targetregister].Description + "* 0x" + Registers[targetregister].multiplier.ToString("X") + " (* " + Registers[targetregister].multiplier.ToString() + ")";
                            Registers[targetregister].Description = "* 0x" + Registers[targetregister].multiplier.ToString("X") + " (* " + Registers[targetregister].multiplier.ToString() + ")";

                        }
                    }

                    Registers[targetregister].originalvalue = false;
                    //Notation += "r" + targetregister.ToString() + " = " + 
                    //    Registers[sourceregister].Description + " / 0x" + immediate.ToString("X") +
                    //                                                    " (" + immediate.ToString() + ")";
                    break;
                #endregion
                #region sr
                case "sra":
                case "srav":
                case "srlv":
                case "srl":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    immediate = StringToAddress(linesplit[5]);

                    Registers[targetregister].Value = Registers[sourceregister].Value / Exponent(2, immediate);

                    if (Registers[targetregister].multiplier == 0)
                    {
                        if (immediate % 0x08 == 0)
                        {
                            Notation += "Shift right " + (immediate / 0x08).ToString() + " bytes(/0x" + Exponent(2, immediate).ToString("X") + ")" + " (/" + Exponent(2, immediate).ToString() + ")"; ;
                            Registers[targetregister].Description = Registers[sourceregister].Description + " >> " + immediate.ToString("X");
                        }
                        else
                        {
                            Notation += "Shift right " + (immediate).ToString() + " bits (/0x" + Exponent(2, immediate).ToString("X") + ")" + " (/" + Exponent(2, immediate).ToString() + ")"; ;
                            Registers[targetregister].Description = Registers[sourceregister].Description + " >> " + immediate.ToString("X");
                        }
                        // Registers[targetregister].multiplier = (int)Exponent(2, immediate);
                    }
                    else if (Registers[targetregister].multiplier != 0)
                    {
                        Registers[targetregister].multiplier /= (int)Exponent(2, immediate);
                    }

                    Registers[targetregister].originalvalue = false;
                    Notation += "r" + targetregister.ToString() + " = " +
                    Registers[sourceregister].Description + " * 0x" + immediate.ToString("X") +
                                                                        " (" + immediate.ToString() + ")";
                    break;
                #endregion
                #region mult
                case "mult":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));

                    //Store Lower 32 bits in Lo
                    Registers[32].Value = Registers[comparedregister].Value * Registers[targetregister].Value;
                    Registers[32].Description = Registers[comparedregister].Description + " *  " + Registers[targetregister].Description;

                    //Store upper 32 bits in Hi

                    Notation += Registers[targetregister].Description + " * " + Registers[comparedregister].Description;
                    break;
                case "multu":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));

                    //Store Lower 32 bits in Lo
                    Registers[32].Value = Registers[comparedregister].Value * Registers[targetregister].Value;
                    Registers[32].Description = Registers[comparedregister].Description + " *  " + Registers[targetregister].Description;

                    //Store upper 32 bits in Hi


                    Notation += Registers[targetregister].Description + " * " + Registers[comparedregister].Description;

                    break;
                #endregion
                #region div
                case "div":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));

                    Registers[32].Value = Registers[comparedregister].Value / Registers[targetregister].Value;
                    Registers[32].Description = Registers[comparedregister].Description + " / " + Registers[targetregister].Description;

                    Registers[33].Value = Registers[comparedregister].Value % Registers[targetregister].Value;
                    Registers[33].Description = Registers[comparedregister].Description + " % " + Registers[targetregister].Description;


                    Notation += Registers[targetregister].Description + " / " + Registers[comparedregister].Description;

                    break;
                case "divu":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[4].Replace("r", ""));

                    Registers[32].Value = Registers[comparedregister].Value / Registers[targetregister].Value;
                    Registers[32].Description = Registers[comparedregister].Description + " /  " + Registers[targetregister].Description;

                    Registers[33].Value = Registers[comparedregister].Value % Registers[targetregister].Value;
                    Registers[33].Description = Registers[comparedregister].Description + " % " + Registers[targetregister].Description;

                    Notation += Registers[targetregister].Description + " / " + Registers[comparedregister].Description;

                    break;
                #endregion
                #region mflo/mfhi
                case "mflo":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));

                    Registers[targetregister].Value = Registers[32].Value;
                    Registers[targetregister].Description = Registers[32].Description;
                    Notation += Registers[32].Description;
                    Registers[targetregister].originalvalue = false;
                    break;
                case "mfhi":
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));

                    Registers[targetregister].Value = Registers[33].Value;
                    Registers[targetregister].Description = Registers[33].Description;
                    Notation += Registers[33].Description;
                    Registers[targetregister].originalvalue = false;
                    break;
                #endregion
                #endregion

                #region Logic Commands
                #region or
                case "or":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));

                    Notation += "r" + targetregister.ToString() + " = ";
                    if ((sourceregister != 0 && comparedregister == 0))
                    {
                        Notation += Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value;
                        Registers[targetregister].Description = Registers[sourceregister].Description;
                    }
                    else if (comparedregister != 0 && sourceregister == 0)
                    {
                        Notation += Registers[comparedregister].Description;
                        Registers[targetregister].Value = Registers[comparedregister].Value;
                        Registers[targetregister].Description = Registers[comparedregister].Description;
                    }
                    else if (sourceregister == 0 && comparedregister == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (comparedregister != 0 && sourceregister != 0)
                    {
                        Notation += Registers[comparedregister].Description + Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[comparedregister].Value | Registers[sourceregister].Value;
                        Registers[targetregister].Description = Registers[comparedregister].Description + " | " + Registers[sourceregister].Description;
                    }



                    //look up new data location
                    break;
                case "ori":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    immediate = StringToAddress(linesplit[5].Replace("r", ""));

                    Notation += "r" + targetregister.ToString() + " = ";

                    if ((sourceregister != 0 && immediate == 0))
                    {
                        Notation += Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value;
                        Registers[targetregister].Description = Registers[sourceregister].Description;
                    }
                    else if (sourceregister == 0 && immediate != 0)
                    {
                        Notation += immediate.ToString();
                        Registers[targetregister].Value = immediate;
                        Registers[targetregister].Description = immediate.ToString();
                    }
                    else if (sourceregister == 0 && immediate == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (sourceregister != 0 && immediate != 0)
                    {
                        if (sourceregister == targetregister)
                        {
                            Notation += "Add " + immediate.ToString("X") + " to " + Registers[targetregister].Description;
                        }
                        else
                        {
                            Notation += Registers[sourceregister].Description + " | 0x" + immediate.ToString("X");
                            Registers[targetregister].Description = Registers[sourceregister].Description + " | 0x" + immediate.ToString("X");
                        }
                        Registers[targetregister].Value = Registers[sourceregister].Value | immediate;

                    }

                    //look up new data location
                    break;
                #endregion
                #region and
                case "and":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));
                    // immediate = StringToAddress(linesplit[5]);
                    //look up new data location
                    #region special conditions
                    if ((sourceregister != 0 && comparedregister == 0))
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (comparedregister != 0 && sourceregister == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (sourceregister == 0 && comparedregister == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    #endregion
                    else if (comparedregister != 0 && sourceregister != 0)
                    {
                        Notation += Registers[comparedregister].Description + " & " + Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value & Registers[comparedregister].Value;
                        Registers[targetregister].Description = Registers[comparedregister].Description + " & " + Registers[sourceregister].Description;
                    }

                    //Notation += "r" + targetregister.ToString() + " = " +
                    //    Registers[sourceregister].Description + " *AND* " + immediate.ToString("X");

                    break;
                case "andi":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    immediate = StringToAddress(linesplit[5]);

                    #region Special Conditions
                    if ((sourceregister != 0 && immediate == 0))
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (sourceregister == 0 && immediate != 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (sourceregister == 0 && immediate == 0)
                    {
                        Notation += "0";
                        Registers[targetregister].Value = 0;
                        Registers[targetregister].Description = "0";
                    }
                    else if (immediate == 0xffff)
                    {
                        Notation += "r" + targetregister.ToString() + " = " + Registers[sourceregister].Description;
                        Registers[targetregister].Value = Registers[sourceregister].Value;
                        Registers[targetregister].Description = Registers[sourceregister].Description;

                    }
                    else if (immediate == 0x00ff)
                    {
                        Notation += "Mask second byte";
                        Registers[targetregister].Value = Registers[sourceregister].Value & 0x00FF;
                        Registers[targetregister].Description = Registers[sourceregister].Description;
                    }
                    #endregion
                    else if (sourceregister != 0 && immediate != 0)
                    {
                        Notation += Registers[sourceregister].Description + " & 0x" + immediate.ToString("X");
                        Registers[targetregister].Value = Registers[sourceregister].Value & immediate;
                        Registers[targetregister].Description = Registers[sourceregister].Description + " & 0x" + immediate.ToString("X");
                    }


                    //look up new data location
                    //Notation += "r" + targetregister.ToString() + " = " + 
                    //    Registers[sourceregister].Description + " *AND* " + immediate.ToString("X");
                    //look up new data location
                    break;
                #endregion
                #region xor/nor
                case "xor":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    comparedregister = Convert.ToInt32(linesplit[5].Replace("r", ""));
                    //immediate = StringToAddress(linesplit[5]);

                    Notation += Registers[sourceregister].Description + " xor " + Registers[comparedregister].Description;
                    Registers[targetregister].Value = Registers[sourceregister].Value ^ Registers[comparedregister].Value;
                    Registers[targetregister].Description = Registers[sourceregister].Description + " xor " + Registers[comparedregister].Description;

                    // ^ operator
                    //look up new data location
                    break;
                case "xori":
                    sourceregister = Convert.ToInt32(linesplit[4].Replace("r", ""));
                    targetregister = Convert.ToInt32(linesplit[3].Replace("r", ""));
                    //comparedregister = 0;
                    immediate = StringToAddress(linesplit[5]);

                    Notation += Registers[sourceregister].Description + " xor 0x" + immediate.ToString("X");
                    Registers[targetregister].Value = Registers[sourceregister].Value ^ immediate;
                    Registers[targetregister].Description = Registers[sourceregister].Description + " xor 0x" + immediate.ToString("X");

                    //look up new data location
                    break;
                case "nor":
                    // !& = |
                    //look up new data location
                    break;
                #endregion
                #endregion

                case "":
                    break;
            }

            return Notation;
        }
       
        public MainAddress[] Add(string instring, MainAddress[] MainAddresses)
        {
            if(MainAddresses == null)
            {
                MainAddresses = new MainAddress[1];
                MainAddresses[MainAddresses.Length - 1] = new MainAddress(instring);
            }
            else
            {
                Array.Resize(ref MainAddresses, MainAddresses.Length + 1);
                MainAddresses[MainAddresses.Length - 1] = new MainAddress(instring);
            }
          
            return MainAddresses;
        }
        public void InitializeRegisters()
        {
            foreach (Register reg in Registers)
            {
                reg.Value = 0;
                reg.SpecialCommand = "";
                reg.Description = reg.Name + " Input";
                reg.multiplier = 0;
                reg.SubsetOffset = 0;
                reg.originalvalue = true;

                if (reg.Name == "r0")
                {
                    reg.Description = "0";
                }
                if (reg.Name == "r31")
                {
                    reg.Description = "Return Address";
                }
                if (reg.Name == "r29")
                {
                    reg.Description = "Stack Pointer";
                }
                if (reg.Name == "r32")
                {
                    reg.Description = "Lo";
                }
                if (reg.Name == "r33")
                {
                    reg.Description = "Hi";
                }
            }
        }
        public void PrintRegisterDescriptions(StreamWriter sw)
        {
            foreach (Register reg in Registers)
            {
                if (reg.Description != reg.Name + "Input")
                {
                    sw.WriteLine(reg.Name + " = " + reg.Description + "\n");
                }
            }
        }
        
        public long Exponent(long basenumber, long exponent)
        {
            long result = 1;
            for (int i = 0;i < exponent;i++)
            {
                 result = result * basenumber;
            }
            return result;
        }
        #endregion

    }
}
