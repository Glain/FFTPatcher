/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 5/2/2011
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ASMEncoding;

namespace MassHexASM
{		
	public partial class MainForm : Form
	{
		public ASMEncodingUtility _asmUtility;
		
		public MainForm()
		{
			// The InitializeComponent() call is required for Windows Forms designer support.
			InitializeComponent();
			InitASMUtility();
            InitForm();
		}

        public void InitForm()
        {
            // Set properties of form
            txt_Messages.BackColor = SystemColors.Control;
            txt_Messages.ForeColor = Color.Red;
            txt_SpacePad.Enabled = false;
            chk_littleEndian.Checked = true;

            Text = string.Format("MassHexASM (FFTPS v0.{0})", About.FFTPatcherSuiteRevision);

            cb_Mode.Items.Add("Base");
            cb_Mode.Items.Add("PSX");
            cb_Mode.Items.Add("PSP");
            cb_Mode.SelectedIndex = ASMEncodingMode.PSX;
        }
		
		public void InitASMUtility()
		{
			_asmUtility = new ASMEncodingUtility();
            _asmUtility.EncodingMode = ASMEncodingMode.PSX;
		}
		
		void Btn_EncodeClick(object sender, EventArgs e)
		{
			txt_Hex.Text = "";
			txt_Messages.Text = "";
			
			ASMEncoderResult encodeResult = _asmUtility.EncodeASM(txt_ASM.Text, txt_StartPC.Text, GetSpacePadding(), chk_IncludeAddress.Checked, chk_littleEndian.Checked);
            
			txt_Hex.Text = encodeResult.EncodedASMText;
			txt_ASM.Text = encodeResult.ModifiedText;

            txt_Messages.ForeColor = Color.Red;
            txt_Messages.Text = encodeResult.ErrorText;

            if (string.IsNullOrEmpty(encodeResult.ErrorText))
            {
                uint pc = ASMEncodingUtility.ProcessStartPC(txt_ASM.Text, txt_StartPC.Text);
                ASMCheckResult checkResult = _asmUtility.CheckASMFromHex(encodeResult.EncodedASMText, pc, chk_littleEndian.Checked, chk_NameRegs.Checked);
                txt_Messages.Text = checkResult.ErrorText;
            }
		}
		
		void Btn_DecodeClick(object sender, EventArgs e)
		{
			txt_ASM.Text = "";
			txt_Messages.Text = "";
			
			ASMDecoderResult decodeResult = _asmUtility.DecodeASM(txt_Hex.Text, txt_StartPC.Text, GetSpacePadding(), chk_littleEndian.Checked, chk_IncludeAddress.Checked, chk_NameRegs.Checked);

			txt_ASM.Text = decodeResult.DecodedASM;

            txt_Messages.ForeColor = Color.Red;
            txt_Messages.Text = decodeResult.ErrorText;

            if (string.IsNullOrEmpty(decodeResult.ErrorText))
            {
                ASMCheckResult checkResult = _asmUtility.CheckASM(decodeResult.DecodedASM, txt_StartPC.Text, chk_littleEndian.Checked, chk_NameRegs.Checked, false);
                txt_Messages.Text = checkResult.ErrorText;
            }
		}

        private void btn_Check_Click(object sender, EventArgs e)
        {
            txt_Messages.Text = "";
            ASMCheckResult checkResult = _asmUtility.CheckASM(txt_ASM.Text, txt_StartPC.Text, chk_littleEndian.Checked, chk_NameRegs.Checked, true);

            if (string.IsNullOrEmpty(checkResult.ErrorText))
            {
                txt_Messages.ForeColor = Color.Green;
                txt_Messages.Text = "No warnings.";
            }
            else
            {
                txt_Messages.ForeColor = Color.Red;
                txt_Messages.Text = checkResult.ErrorText;
            }
        }
		
		void Chk_SpacePadCheckedChanged(object sender, EventArgs e)
		{
			txt_SpacePad.Enabled = chk_SpacePad.Checked;	
		}
		
		void Txt_ASMKeyDown(object sender, KeyEventArgs e)
		{
			CauseSelectAll(txt_ASM,e);
		}
		
		void Txt_HexKeyDown(object sender, KeyEventArgs e)
		{
			CauseSelectAll(txt_Hex,e);
		}
		
		private void CauseSelectAll(TextBox box, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.A) && e.Control)
			{
				box.SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
			}
		}

        private void cb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _asmUtility.EncodingMode = cb_Mode.SelectedIndex;
        }

		// Gets space padding based on input
		private string GetSpacePadding()
		{
			if (!chk_SpacePad.Checked)
				return "";
			
			int numSpaces = 0;
			if (!int.TryParse(txt_SpacePad.Text, out numSpaces))
				return "";
			
			if (numSpaces <= 0)
				return "";
			
			string retval = "";
			for (int i=0; i < numSpaces; i++)
				retval += " ";
			
			return retval;
		}

        private void menuItem_Form_LEDecoder_Click(object sender, EventArgs e)
        {
            new LEDecoder.LEDecoder().ShowDialog();
        }

        private void menuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, About.Message, About.Title, MessageBoxButtons.OK);
        }
	}
}
