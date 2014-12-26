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
			
			Timer timer = new Timer();
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
			((Timer)(sender)).Stop();
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
            if(InString.Contains("0x"))
            {
                InString = InString.Replace("0x","");
            }
            long intout = 0;
            byte[] byteOut = new byte[3];
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

            }
            else
            {
                byteOut[0] = 0;
            }

            return intout;
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
                
                Process[] processes = Process.GetProcesses();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName == "notepad")
                    {
                        string title = txt_OutputFile.Text.Substring(txt_OutputFile.Text.LastIndexOf("\\")+1).Replace(".txt","");
                        if( p.MainWindowTitle.Contains(title))
                        {
                             p.Kill();
                        }
                    }
                }
                Process process = new Process();
                Process.Start("notepad.exe",txt_OutputFile.Text);
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
            Autonotator An = new Autonotator(this);
            An.Show(); //Set initial register values

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
                using (StreamWriter sw = File.AppendText(txt_OutputFile.Text))
                {
                    string tempstring = "";
                    sw.Write(txt_StartingAddress +": " + Indent(3) + GetRoutineDescription(txt_StartingAddress.Text));
                    PrintRegisterDescriptions(sw);

                    string RoutineDisassembly = GetRoutineFromDisassembly();
                    string[] RoutineLines = ToLines(RoutineDisassembly);

                    foreach(string line in RoutineLines)
                    {
                        Notate(line);
                    }
                }
            }

            #endregion

        }
        #endregion

        #region Autonotator Routines
        public void PrintRegisterDescriptions(StreamWriter sw)
        {
            foreach (Register reg in Registers)
            {
                if( reg.Description != "")
                {
                    sw.Write(reg.Name + " = " + reg.Description + "\n");
                }
            }
        }
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

        public string Notate(string line)
        {
            string Notation = "";

            int sourceregister = 0;
            int targetregister = 0;
            int comparedregister = 0;

            string command = line.Substring(0x13); //extract the command type
            switch(command)
            {
                case "jal":
                    Notation = Indent(3) +  GetRoutineDescription(line.Substring(23));
                    break;
                case "jalr":
                    Notation = Indent(3) + "Jump and Link";
                    break;
                case "j":
                    break;

            }



            return Notation;
        }

        public string ExtractCommand(string command)
        {
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


            return command;
        }
        #endregion

    }
}
