/*
 * Created by SharpDevelop.
 * User: Glain
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

    public partial class LEDecoder : Form
    {
        #region Fields
        Color _ledColor = Color.Yellow;
        ASMEncodingUtility _asmUtility;
        #endregion

        #region Form Initialization
        public LEDecoder()
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
            cb_Mode.SelectedIndex = (int)ASMEncodingMode.PSX;
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

        private void cb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _asmUtility.EncodingMode = (ASMEncodingMode)cb_Mode.SelectedIndex;
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

            if (dialogResult == DialogResult.OK)
            {
                LDResult.FileName = fileDialog.FileName;
            }

            LDResult.DialogResult = dialogResult;

            return LDResult;
        }

        #endregion
    }
}