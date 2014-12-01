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
		#region Fields
		Color _ledColor = Color.Yellow;
		ASMEncodingUtility _asmUtility;
		#endregion

        #region Global Variables
        string Function = "Decode ASM";
        #endregion
		
		#region Form Initialization
		public MainForm()
		{
			InitializeComponent();
			Process();
		}
		
		// Invoked when the form is initialized.
		public void Process()
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
            if (AutoNotateButton.Checked)
            {
                Function = "AutoNotate";
            }
        }

        private void JalFindButton_CheckedChanged(object sender, EventArgs e)
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

        }
        private void PrintHex()
        {

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

                        if(chk_SpaceBox.Checked)
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
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CollapseRoutines()
        {
            bool started = false;
            string[] AllLines = File.ReadAllLines(txt_InputFile.Text);

            int i = 0;
            int count = 0;

            while(AllLines[i] != null)
            {
                string line = AllLines[i];
                if (line.IndexOf(":") > 0)
                {
                    if (started && line.Contains("jr r31") && AllLines[i+2] == "")
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
            
        }
        private void AutoNotate()
        {
            
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
		#endregion

       
    }
}
