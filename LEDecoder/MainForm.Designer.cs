/*
 * Created by SharpDevelop.
 * User: Jeremy
 * Date: 9/17/2011
 * Time: 14:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LEDecoder
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.lbl_InputFile = new System.Windows.Forms.Label();
            this.lbl_OutputFile = new System.Windows.Forms.Label();
            this.txt_InputFile = new System.Windows.Forms.TextBox();
            this.txt_OutputFile = new System.Windows.Forms.TextBox();
            this.btn_InputFile = new System.Windows.Forms.Button();
            this.btn_OutputFile = new System.Windows.Forms.Button();
            this.btn_Process = new System.Windows.Forms.Button();
            this.chk_LittleEndian = new System.Windows.Forms.CheckBox();
            this.pic_LED = new System.Windows.Forms.PictureBox();
            this.lbl_Overwrite = new System.Windows.Forms.Label();
            this.txt_StartingAddress = new System.Windows.Forms.TextBox();
            this.lbl_StartingAddress = new System.Windows.Forms.Label();
            this.chk_NameRegisters = new System.Windows.Forms.CheckBox();
            this.cb_Mode = new System.Windows.Forms.ComboBox();
            this.lbl_Mode = new System.Windows.Forms.Label();
            this.Functionpanel = new System.Windows.Forms.Panel();
            this.CollapseRoutinesButton = new System.Windows.Forms.RadioButton();
            this.AutoNotateButton = new System.Windows.Forms.RadioButton();
            this.JalFindButton = new System.Windows.Forms.RadioButton();
            this.PrintHexButton = new System.Windows.Forms.RadioButton();
            this.DecodeASMButton = new System.Windows.Forms.RadioButton();
            this.lab_Length = new System.Windows.Forms.Label();
            this.txt_Length = new System.Windows.Forms.TextBox();
            this.lab_BPL = new System.Windows.Forms.Label();
            this.txt_BPLbox = new System.Windows.Forms.TextBox();
            this.lab_PSX = new System.Windows.Forms.Label();
            this.chk_SpaceBox = new System.Windows.Forms.CheckBox();
            this.btn_UpdateWiki = new System.Windows.Forms.Button();
            this.cmb_FileofRoutine = new System.Windows.Forms.ComboBox();
            this.lbl_FileofRoutine = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btn_AutoNotateForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pic_LED)).BeginInit();
            this.Functionpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_InputFile
            // 
            this.lbl_InputFile.Location = new System.Drawing.Point(28, 64);
            this.lbl_InputFile.Name = "lbl_InputFile";
            this.lbl_InputFile.Size = new System.Drawing.Size(114, 22);
            this.lbl_InputFile.TabIndex = 0;
            this.lbl_InputFile.Text = "Input File:";
            // 
            // lbl_OutputFile
            // 
            this.lbl_OutputFile.Location = new System.Drawing.Point(28, 119);
            this.lbl_OutputFile.Name = "lbl_OutputFile";
            this.lbl_OutputFile.Size = new System.Drawing.Size(114, 22);
            this.lbl_OutputFile.TabIndex = 1;
            this.lbl_OutputFile.Text = "Output File:";
            // 
            // txt_InputFile
            // 
            this.txt_InputFile.Location = new System.Drawing.Point(120, 61);
            this.txt_InputFile.Name = "txt_InputFile";
            this.txt_InputFile.Size = new System.Drawing.Size(291, 20);
            this.txt_InputFile.TabIndex = 2;
            // 
            // txt_OutputFile
            // 
            this.txt_OutputFile.Location = new System.Drawing.Point(120, 121);
            this.txt_OutputFile.Name = "txt_OutputFile";
            this.txt_OutputFile.Size = new System.Drawing.Size(291, 20);
            this.txt_OutputFile.TabIndex = 3;
            // 
            // btn_InputFile
            // 
            this.btn_InputFile.Location = new System.Drawing.Point(424, 59);
            this.btn_InputFile.Name = "btn_InputFile";
            this.btn_InputFile.Size = new System.Drawing.Size(78, 22);
            this.btn_InputFile.TabIndex = 4;
            this.btn_InputFile.Text = "Browse...";
            this.btn_InputFile.UseVisualStyleBackColor = true;
            this.btn_InputFile.Click += new System.EventHandler(this.Btn_InputFileClick);
            // 
            // btn_OutputFile
            // 
            this.btn_OutputFile.Location = new System.Drawing.Point(424, 121);
            this.btn_OutputFile.Name = "btn_OutputFile";
            this.btn_OutputFile.Size = new System.Drawing.Size(78, 22);
            this.btn_OutputFile.TabIndex = 5;
            this.btn_OutputFile.Text = "Browse...";
            this.btn_OutputFile.UseVisualStyleBackColor = true;
            this.btn_OutputFile.Click += new System.EventHandler(this.Btn_OutputFileClick);
            // 
            // btn_Process
            // 
            this.btn_Process.Location = new System.Drawing.Point(467, 261);
            this.btn_Process.Name = "btn_Process";
            this.btn_Process.Size = new System.Drawing.Size(85, 30);
            this.btn_Process.TabIndex = 6;
            this.btn_Process.Text = "Process";
            this.btn_Process.UseVisualStyleBackColor = true;
            this.btn_Process.Click += new System.EventHandler(this.Btn_ProcessClick);
            // 
            // chk_LittleEndian
            // 
            this.chk_LittleEndian.Location = new System.Drawing.Point(467, 196);
            this.chk_LittleEndian.Name = "chk_LittleEndian";
            this.chk_LittleEndian.Size = new System.Drawing.Size(89, 27);
            this.chk_LittleEndian.TabIndex = 7;
            this.chk_LittleEndian.Text = "Little Endian";
            this.chk_LittleEndian.UseVisualStyleBackColor = true;
            // 
            // pic_LED
            // 
            this.pic_LED.Location = new System.Drawing.Point(424, 261);
            this.pic_LED.Name = "pic_LED";
            this.pic_LED.Size = new System.Drawing.Size(35, 30);
            this.pic_LED.TabIndex = 8;
            this.pic_LED.TabStop = false;
            this.pic_LED.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_LEDPaint);
            // 
            // lbl_Overwrite
            // 
            this.lbl_Overwrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Overwrite.Location = new System.Drawing.Point(120, 144);
            this.lbl_Overwrite.Name = "lbl_Overwrite";
            this.lbl_Overwrite.Size = new System.Drawing.Size(233, 24);
            this.lbl_Overwrite.TabIndex = 9;
            this.lbl_Overwrite.Text = "(Output file will be overwritten)";
            // 
            // txt_StartingAddress
            // 
            this.txt_StartingAddress.Location = new System.Drawing.Point(120, 199);
            this.txt_StartingAddress.Name = "txt_StartingAddress";
            this.txt_StartingAddress.Size = new System.Drawing.Size(97, 20);
            this.txt_StartingAddress.TabIndex = 10;
            // 
            // lbl_StartingAddress
            // 
            this.lbl_StartingAddress.AutoSize = true;
            this.lbl_StartingAddress.Location = new System.Drawing.Point(28, 199);
            this.lbl_StartingAddress.Name = "lbl_StartingAddress";
            this.lbl_StartingAddress.Size = new System.Drawing.Size(87, 13);
            this.lbl_StartingAddress.TabIndex = 11;
            this.lbl_StartingAddress.Text = "Starting Address:";
            // 
            // chk_NameRegisters
            // 
            this.chk_NameRegisters.AutoSize = true;
            this.chk_NameRegisters.Location = new System.Drawing.Point(467, 227);
            this.chk_NameRegisters.Name = "chk_NameRegisters";
            this.chk_NameRegisters.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chk_NameRegisters.Size = new System.Drawing.Size(101, 17);
            this.chk_NameRegisters.TabIndex = 12;
            this.chk_NameRegisters.Text = "Name Registers";
            this.chk_NameRegisters.UseVisualStyleBackColor = true;
            // 
            // cb_Mode
            // 
            this.cb_Mode.FormattingEnabled = true;
            this.cb_Mode.Location = new System.Drawing.Point(123, 231);
            this.cb_Mode.Name = "cb_Mode";
            this.cb_Mode.Size = new System.Drawing.Size(65, 21);
            this.cb_Mode.TabIndex = 13;
            this.cb_Mode.SelectedIndexChanged += new System.EventHandler(this.cb_Mode_SelectedIndexChanged);
            // 
            // lbl_Mode
            // 
            this.lbl_Mode.AutoSize = true;
            this.lbl_Mode.Location = new System.Drawing.Point(76, 234);
            this.lbl_Mode.Name = "lbl_Mode";
            this.lbl_Mode.Size = new System.Drawing.Size(37, 13);
            this.lbl_Mode.TabIndex = 14;
            this.lbl_Mode.Text = "Mode:";
            // 
            // Functionpanel
            // 
            this.Functionpanel.Controls.Add(this.CollapseRoutinesButton);
            this.Functionpanel.Controls.Add(this.AutoNotateButton);
            this.Functionpanel.Controls.Add(this.JalFindButton);
            this.Functionpanel.Controls.Add(this.PrintHexButton);
            this.Functionpanel.Controls.Add(this.DecodeASMButton);
            this.Functionpanel.Location = new System.Drawing.Point(12, 3);
            this.Functionpanel.Name = "Functionpanel";
            this.Functionpanel.Size = new System.Drawing.Size(556, 38);
            this.Functionpanel.TabIndex = 15;
            // 
            // CollapseRoutinesButton
            // 
            this.CollapseRoutinesButton.AutoSize = true;
            this.CollapseRoutinesButton.Location = new System.Drawing.Point(442, 9);
            this.CollapseRoutinesButton.Name = "CollapseRoutinesButton";
            this.CollapseRoutinesButton.Size = new System.Drawing.Size(110, 17);
            this.CollapseRoutinesButton.TabIndex = 2;
            this.CollapseRoutinesButton.TabStop = true;
            this.CollapseRoutinesButton.Text = "Collapse Routines";
            this.CollapseRoutinesButton.UseVisualStyleBackColor = true;
            this.CollapseRoutinesButton.CheckedChanged += new System.EventHandler(this.CollapseRoutinesButton_CheckedChanged);
            // 
            // AutoNotateButton
            // 
            this.AutoNotateButton.AutoSize = true;
            this.AutoNotateButton.Location = new System.Drawing.Point(345, 9);
            this.AutoNotateButton.Name = "AutoNotateButton";
            this.AutoNotateButton.Size = new System.Drawing.Size(79, 17);
            this.AutoNotateButton.TabIndex = 1;
            this.AutoNotateButton.TabStop = true;
            this.AutoNotateButton.Text = "AutoNotate";
            this.AutoNotateButton.UseVisualStyleBackColor = true;
            this.AutoNotateButton.CheckedChanged += new System.EventHandler(this.AutoNotateButton_CheckedChanged);
            // 
            // JalFindButton
            // 
            this.JalFindButton.AutoSize = true;
            this.JalFindButton.Location = new System.Drawing.Point(219, 9);
            this.JalFindButton.Name = "JalFindButton";
            this.JalFindButton.Size = new System.Drawing.Size(107, 17);
            this.JalFindButton.TabIndex = 0;
            this.JalFindButton.Text = "Find Jal Structure";
            this.JalFindButton.UseVisualStyleBackColor = true;
            this.JalFindButton.CheckedChanged += new System.EventHandler(this.JalFindButton_CheckedChanged);
            // 
            // PrintHexButton
            // 
            this.PrintHexButton.AutoSize = true;
            this.PrintHexButton.Location = new System.Drawing.Point(124, 9);
            this.PrintHexButton.Name = "PrintHexButton";
            this.PrintHexButton.Size = new System.Drawing.Size(68, 17);
            this.PrintHexButton.TabIndex = 0;
            this.PrintHexButton.Text = "Print Hex";
            this.PrintHexButton.UseVisualStyleBackColor = true;
            this.PrintHexButton.CheckedChanged += new System.EventHandler(this.PrintHexButton_CheckedChanged);
            // 
            // DecodeASMButton
            // 
            this.DecodeASMButton.AutoSize = true;
            this.DecodeASMButton.Checked = true;
            this.DecodeASMButton.Location = new System.Drawing.Point(16, 9);
            this.DecodeASMButton.Name = "DecodeASMButton";
            this.DecodeASMButton.Size = new System.Drawing.Size(89, 17);
            this.DecodeASMButton.TabIndex = 0;
            this.DecodeASMButton.TabStop = true;
            this.DecodeASMButton.Text = "Decode ASM";
            this.DecodeASMButton.UseVisualStyleBackColor = true;
            this.DecodeASMButton.CheckedChanged += new System.EventHandler(this.DecodeASMButton_CheckedChanged);
            // 
            // lab_Length
            // 
            this.lab_Length.AutoSize = true;
            this.lab_Length.Location = new System.Drawing.Point(228, 199);
            this.lab_Length.Name = "lab_Length";
            this.lab_Length.Size = new System.Drawing.Size(43, 13);
            this.lab_Length.TabIndex = 11;
            this.lab_Length.Text = "Length:";
            this.lab_Length.Visible = false;
            // 
            // txt_Length
            // 
            this.txt_Length.Location = new System.Drawing.Point(278, 196);
            this.txt_Length.Name = "txt_Length";
            this.txt_Length.Size = new System.Drawing.Size(85, 20);
            this.txt_Length.TabIndex = 16;
            this.txt_Length.Visible = false;
            // 
            // lab_BPL
            // 
            this.lab_BPL.AutoSize = true;
            this.lab_BPL.Location = new System.Drawing.Point(202, 230);
            this.lab_BPL.Name = "lab_BPL";
            this.lab_BPL.Size = new System.Drawing.Size(73, 13);
            this.lab_BPL.TabIndex = 11;
            this.lab_BPL.Text = "Bytes per line:";
            this.lab_BPL.Visible = false;
            // 
            // txt_BPLbox
            // 
            this.txt_BPLbox.Location = new System.Drawing.Point(278, 227);
            this.txt_BPLbox.Name = "txt_BPLbox";
            this.txt_BPLbox.Size = new System.Drawing.Size(85, 20);
            this.txt_BPLbox.TabIndex = 16;
            this.txt_BPLbox.Visible = false;
            // 
            // lab_PSX
            // 
            this.lab_PSX.AutoSize = true;
            this.lab_PSX.Location = new System.Drawing.Point(123, 234);
            this.lab_PSX.Name = "lab_PSX";
            this.lab_PSX.Size = new System.Drawing.Size(28, 13);
            this.lab_PSX.TabIndex = 11;
            this.lab_PSX.Text = "PSX";
            this.lab_PSX.Visible = false;
            // 
            // chk_SpaceBox
            // 
            this.chk_SpaceBox.AutoSize = true;
            this.chk_SpaceBox.Location = new System.Drawing.Point(231, 253);
            this.chk_SpaceBox.Name = "chk_SpaceBox";
            this.chk_SpaceBox.Size = new System.Drawing.Size(149, 17);
            this.chk_SpaceBox.TabIndex = 17;
            this.chk_SpaceBox.Text = "Add space between bytes";
            this.chk_SpaceBox.UseVisualStyleBackColor = true;
            this.chk_SpaceBox.Visible = false;
            // 
            // btn_UpdateWiki
            // 
            this.btn_UpdateWiki.Location = new System.Drawing.Point(12, 282);
            this.btn_UpdateWiki.Name = "btn_UpdateWiki";
            this.btn_UpdateWiki.Size = new System.Drawing.Size(130, 30);
            this.btn_UpdateWiki.TabIndex = 18;
            this.btn_UpdateWiki.Text = "Update Wiki Resource";
            this.btn_UpdateWiki.UseVisualStyleBackColor = true;
            this.btn_UpdateWiki.Visible = false;
            this.btn_UpdateWiki.Click += new System.EventHandler(this.btn_UpdateWiki_Click);
            // 
            // cmb_FileofRoutine
            // 
            this.cmb_FileofRoutine.FormattingEnabled = true;
            this.cmb_FileofRoutine.Items.AddRange(new object[] {
            "BATTLE - REQUIRE",
            "BATTLE - EQUIP",
            "WLDCORE - WORLD "});
            this.cmb_FileofRoutine.Location = new System.Drawing.Point(120, 169);
            this.cmb_FileofRoutine.Name = "cmb_FileofRoutine";
            this.cmb_FileofRoutine.Size = new System.Drawing.Size(97, 21);
            this.cmb_FileofRoutine.TabIndex = 19;
            this.cmb_FileofRoutine.Visible = false;
            // 
            // lbl_FileofRoutine
            // 
            this.lbl_FileofRoutine.AutoSize = true;
            this.lbl_FileofRoutine.Location = new System.Drawing.Point(35, 172);
            this.lbl_FileofRoutine.Name = "lbl_FileofRoutine";
            this.lbl_FileofRoutine.Size = new System.Drawing.Size(78, 13);
            this.lbl_FileofRoutine.TabIndex = 11;
            this.lbl_FileofRoutine.Text = "File of Routine:";
            this.lbl_FileofRoutine.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(310, 143);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Overwrite File";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btn_AutoNotateForm
            // 
            this.btn_AutoNotateForm.Location = new System.Drawing.Point(205, 282);
            this.btn_AutoNotateForm.Name = "btn_AutoNotateForm";
            this.btn_AutoNotateForm.Size = new System.Drawing.Size(115, 30);
            this.btn_AutoNotateForm.TabIndex = 21;
            this.btn_AutoNotateForm.Text = "Autonotate Settings";
            this.btn_AutoNotateForm.UseVisualStyleBackColor = true;
            this.btn_AutoNotateForm.Visible = false;
            this.btn_AutoNotateForm.Click += new System.EventHandler(this.btn_AutoNotateForm_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 324);
            this.Controls.Add(this.btn_AutoNotateForm);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cmb_FileofRoutine);
            this.Controls.Add(this.btn_UpdateWiki);
            this.Controls.Add(this.chk_SpaceBox);
            this.Controls.Add(this.txt_BPLbox);
            this.Controls.Add(this.txt_Length);
            this.Controls.Add(this.Functionpanel);
            this.Controls.Add(this.lbl_Mode);
            this.Controls.Add(this.chk_NameRegisters);
            this.Controls.Add(this.lab_BPL);
            this.Controls.Add(this.lab_PSX);
            this.Controls.Add(this.lab_Length);
            this.Controls.Add(this.lbl_FileofRoutine);
            this.Controls.Add(this.lbl_StartingAddress);
            this.Controls.Add(this.txt_StartingAddress);
            this.Controls.Add(this.lbl_Overwrite);
            this.Controls.Add(this.pic_LED);
            this.Controls.Add(this.chk_LittleEndian);
            this.Controls.Add(this.btn_Process);
            this.Controls.Add(this.btn_OutputFile);
            this.Controls.Add(this.btn_InputFile);
            this.Controls.Add(this.txt_OutputFile);
            this.Controls.Add(this.txt_InputFile);
            this.Controls.Add(this.lbl_OutputFile);
            this.Controls.Add(this.lbl_InputFile);
            this.Controls.Add(this.cb_Mode);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "LEDecoder";
            ((System.ComponentModel.ISupportInitialize)(this.pic_LED)).EndInit();
            this.Functionpanel.ResumeLayout(false);
            this.Functionpanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Label lbl_Overwrite;
		private System.Windows.Forms.PictureBox pic_LED;
		private System.Windows.Forms.CheckBox chk_LittleEndian;
		private System.Windows.Forms.Button btn_Process;
		private System.Windows.Forms.Button btn_OutputFile;
		private System.Windows.Forms.Button btn_InputFile;
		private System.Windows.Forms.TextBox txt_OutputFile;
		private System.Windows.Forms.TextBox txt_InputFile;
		private System.Windows.Forms.Label lbl_OutputFile;
		private System.Windows.Forms.Label lbl_InputFile;
        private System.Windows.Forms.TextBox txt_StartingAddress;
        private System.Windows.Forms.Label lbl_StartingAddress;
        private System.Windows.Forms.CheckBox chk_NameRegisters;
        private System.Windows.Forms.ComboBox cb_Mode;
        private System.Windows.Forms.Label lbl_Mode;
        private System.Windows.Forms.Panel Functionpanel;
        private System.Windows.Forms.RadioButton JalFindButton;
        private System.Windows.Forms.RadioButton PrintHexButton;
        private System.Windows.Forms.RadioButton DecodeASMButton;
        private System.Windows.Forms.Label lab_Length;
        private System.Windows.Forms.TextBox txt_Length;
        private System.Windows.Forms.RadioButton AutoNotateButton;
        private System.Windows.Forms.RadioButton CollapseRoutinesButton;
        private System.Windows.Forms.Label lab_BPL;
        private System.Windows.Forms.TextBox txt_BPLbox;
        private System.Windows.Forms.Label lab_PSX;
        private System.Windows.Forms.CheckBox chk_SpaceBox;
        private System.Windows.Forms.Button btn_UpdateWiki;
        private System.Windows.Forms.ComboBox cmb_FileofRoutine;
        private System.Windows.Forms.Label lbl_FileofRoutine;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btn_AutoNotateForm;
	}
}
