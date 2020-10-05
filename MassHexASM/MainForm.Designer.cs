/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 5/2/2011
 * Time: 18:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
  
namespace MassHexASM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txt_ASM = new System.Windows.Forms.TextBox();
            this.txt_Hex = new System.Windows.Forms.TextBox();
            this.btn_Encode = new System.Windows.Forms.Button();
            this.chk_littleEndian = new System.Windows.Forms.CheckBox();
            this.lbl_ASM_Text = new System.Windows.Forms.Label();
            this.lbl_Hex_Text = new System.Windows.Forms.Label();
            this.txt_Messages = new System.Windows.Forms.TextBox();
            this.lbl_Messages_Text = new System.Windows.Forms.Label();
            this.btn_Decode = new System.Windows.Forms.Button();
            this.txt_StartPC = new System.Windows.Forms.TextBox();
            this.lbl_StartPC_Text = new System.Windows.Forms.Label();
            this.chk_SpacePad = new System.Windows.Forms.CheckBox();
            this.txt_SpacePad = new System.Windows.Forms.TextBox();
            this.lbl_SpacePadNum_Text = new System.Windows.Forms.Label();
            this.chk_IncludeAddress = new System.Windows.Forms.CheckBox();
            this.chk_NameRegs = new System.Windows.Forms.CheckBox();
            this.cb_Mode = new System.Windows.Forms.ComboBox();
            this.lbl_Mode = new System.Windows.Forms.Label();
            this.pnl_RightSide = new System.Windows.Forms.Panel();
            this.btn_Check = new System.Windows.Forms.Button();
            this.pnl_BottomSection = new System.Windows.Forms.Panel();
            this.menu_Top = new System.Windows.Forms.MenuStrip();
            this.menuItem_Form = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Form_LEDecoder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            this.pnl_RightSide.SuspendLayout();
            this.pnl_BottomSection.SuspendLayout();
            this.menu_Top.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_ASM
            // 
            this.txt_ASM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_ASM.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ASM.Location = new System.Drawing.Point(36, 52);
            this.txt_ASM.MaxLength = 0;
            this.txt_ASM.Multiline = true;
            this.txt_ASM.Name = "txt_ASM";
            this.txt_ASM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_ASM.Size = new System.Drawing.Size(354, 353);
            this.txt_ASM.TabIndex = 0;
            this.txt_ASM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Txt_ASMKeyDown);
            // 
            // txt_Hex
            // 
            this.txt_Hex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_Hex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Hex.Location = new System.Drawing.Point(128, 35);
            this.txt_Hex.MaxLength = 0;
            this.txt_Hex.Multiline = true;
            this.txt_Hex.Name = "txt_Hex";
            this.txt_Hex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Hex.Size = new System.Drawing.Size(274, 353);
            this.txt_Hex.TabIndex = 1;
            this.txt_Hex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Txt_HexKeyDown);
            // 
            // btn_Encode
            // 
            this.btn_Encode.Location = new System.Drawing.Point(14, 35);
            this.btn_Encode.Name = "btn_Encode";
            this.btn_Encode.Size = new System.Drawing.Size(80, 31);
            this.btn_Encode.TabIndex = 2;
            this.btn_Encode.Text = "Encode >>";
            this.btn_Encode.UseVisualStyleBackColor = true;
            this.btn_Encode.Click += new System.EventHandler(this.Btn_EncodeClick);
            // 
            // chk_littleEndian
            // 
            this.chk_littleEndian.Location = new System.Drawing.Point(8, 179);
            this.chk_littleEndian.Name = "chk_littleEndian";
            this.chk_littleEndian.Size = new System.Drawing.Size(94, 26);
            this.chk_littleEndian.TabIndex = 3;
            this.chk_littleEndian.Text = "Little Endian";
            this.chk_littleEndian.UseVisualStyleBackColor = true;
            // 
            // lbl_ASM_Text
            // 
            this.lbl_ASM_Text.Location = new System.Drawing.Point(33, 29);
            this.lbl_ASM_Text.Name = "lbl_ASM_Text";
            this.lbl_ASM_Text.Size = new System.Drawing.Size(132, 20);
            this.lbl_ASM_Text.TabIndex = 4;
            this.lbl_ASM_Text.Text = "MIPS Assembly";
            // 
            // lbl_Hex_Text
            // 
            this.lbl_Hex_Text.Location = new System.Drawing.Point(125, 12);
            this.lbl_Hex_Text.Name = "lbl_Hex_Text";
            this.lbl_Hex_Text.Size = new System.Drawing.Size(132, 21);
            this.lbl_Hex_Text.TabIndex = 5;
            this.lbl_Hex_Text.Text = "Hex Encoding";
            // 
            // txt_Messages
            // 
            this.txt_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Messages.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txt_Messages.Location = new System.Drawing.Point(3, 28);
            this.txt_Messages.Multiline = true;
            this.txt_Messages.Name = "txt_Messages";
            this.txt_Messages.ReadOnly = true;
            this.txt_Messages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Messages.Size = new System.Drawing.Size(770, 95);
            this.txt_Messages.TabIndex = 6;
            // 
            // lbl_Messages_Text
            // 
            this.lbl_Messages_Text.Location = new System.Drawing.Point(5, 4);
            this.lbl_Messages_Text.Name = "lbl_Messages_Text";
            this.lbl_Messages_Text.Size = new System.Drawing.Size(84, 24);
            this.lbl_Messages_Text.TabIndex = 7;
            this.lbl_Messages_Text.Text = "Messages";
            // 
            // btn_Decode
            // 
            this.btn_Decode.Location = new System.Drawing.Point(14, 72);
            this.btn_Decode.Name = "btn_Decode";
            this.btn_Decode.Size = new System.Drawing.Size(80, 31);
            this.btn_Decode.TabIndex = 8;
            this.btn_Decode.Text = "<< Decode";
            this.btn_Decode.UseVisualStyleBackColor = true;
            this.btn_Decode.Click += new System.EventHandler(this.Btn_DecodeClick);
            // 
            // txt_StartPC
            // 
            this.txt_StartPC.Location = new System.Drawing.Point(14, 211);
            this.txt_StartPC.Name = "txt_StartPC";
            this.txt_StartPC.Size = new System.Drawing.Size(94, 20);
            this.txt_StartPC.TabIndex = 9;
            // 
            // lbl_StartPC_Text
            // 
            this.lbl_StartPC_Text.Location = new System.Drawing.Point(19, 234);
            this.lbl_StartPC_Text.Name = "lbl_StartPC_Text";
            this.lbl_StartPC_Text.Size = new System.Drawing.Size(89, 21);
            this.lbl_StartPC_Text.TabIndex = 10;
            this.lbl_StartPC_Text.Text = "Starting address";
            // 
            // chk_SpacePad
            // 
            this.chk_SpacePad.Location = new System.Drawing.Point(8, 320);
            this.chk_SpacePad.Name = "chk_SpacePad";
            this.chk_SpacePad.Size = new System.Drawing.Size(94, 26);
            this.chk_SpacePad.TabIndex = 11;
            this.chk_SpacePad.Text = "Pad (spaces)";
            this.chk_SpacePad.UseVisualStyleBackColor = true;
            this.chk_SpacePad.CheckedChanged += new System.EventHandler(this.Chk_SpacePadCheckedChanged);
            // 
            // txt_SpacePad
            // 
            this.txt_SpacePad.Location = new System.Drawing.Point(60, 352);
            this.txt_SpacePad.Name = "txt_SpacePad";
            this.txt_SpacePad.Size = new System.Drawing.Size(34, 20);
            this.txt_SpacePad.TabIndex = 12;
            // 
            // lbl_SpacePadNum_Text
            // 
            this.lbl_SpacePadNum_Text.Location = new System.Drawing.Point(11, 355);
            this.lbl_SpacePadNum_Text.Name = "lbl_SpacePadNum_Text";
            this.lbl_SpacePadNum_Text.Size = new System.Drawing.Size(48, 19);
            this.lbl_SpacePadNum_Text.TabIndex = 13;
            this.lbl_SpacePadNum_Text.Text = "Number:";
            // 
            // chk_IncludeAddress
            // 
            this.chk_IncludeAddress.Location = new System.Drawing.Point(8, 258);
            this.chk_IncludeAddress.Name = "chk_IncludeAddress";
            this.chk_IncludeAddress.Size = new System.Drawing.Size(105, 22);
            this.chk_IncludeAddress.TabIndex = 14;
            this.chk_IncludeAddress.Text = "Show Addresses";
            this.chk_IncludeAddress.UseVisualStyleBackColor = true;
            // 
            // chk_NameRegs
            // 
            this.chk_NameRegs.Location = new System.Drawing.Point(8, 296);
            this.chk_NameRegs.Name = "chk_NameRegs";
            this.chk_NameRegs.Size = new System.Drawing.Size(105, 25);
            this.chk_NameRegs.TabIndex = 15;
            this.chk_NameRegs.Text = "Name Registers";
            this.chk_NameRegs.UseVisualStyleBackColor = true;
            // 
            // cb_Mode
            // 
            this.cb_Mode.FormattingEnabled = true;
            this.cb_Mode.Location = new System.Drawing.Point(46, 155);
            this.cb_Mode.Name = "cb_Mode";
            this.cb_Mode.Size = new System.Drawing.Size(57, 21);
            this.cb_Mode.TabIndex = 16;
            this.cb_Mode.SelectedIndexChanged += new System.EventHandler(this.cb_Mode_SelectedIndexChanged);
            // 
            // lbl_Mode
            // 
            this.lbl_Mode.AutoSize = true;
            this.lbl_Mode.Location = new System.Drawing.Point(3, 158);
            this.lbl_Mode.Name = "lbl_Mode";
            this.lbl_Mode.Size = new System.Drawing.Size(37, 13);
            this.lbl_Mode.TabIndex = 17;
            this.lbl_Mode.Text = "Mode:";
            // 
            // pnl_RightSide
            // 
            this.pnl_RightSide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_RightSide.Controls.Add(this.btn_Check);
            this.pnl_RightSide.Controls.Add(this.lbl_Mode);
            this.pnl_RightSide.Controls.Add(this.cb_Mode);
            this.pnl_RightSide.Controls.Add(this.chk_NameRegs);
            this.pnl_RightSide.Controls.Add(this.chk_IncludeAddress);
            this.pnl_RightSide.Controls.Add(this.lbl_SpacePadNum_Text);
            this.pnl_RightSide.Controls.Add(this.txt_SpacePad);
            this.pnl_RightSide.Controls.Add(this.chk_SpacePad);
            this.pnl_RightSide.Controls.Add(this.lbl_StartPC_Text);
            this.pnl_RightSide.Controls.Add(this.txt_StartPC);
            this.pnl_RightSide.Controls.Add(this.btn_Decode);
            this.pnl_RightSide.Controls.Add(this.lbl_Hex_Text);
            this.pnl_RightSide.Controls.Add(this.chk_littleEndian);
            this.pnl_RightSide.Controls.Add(this.btn_Encode);
            this.pnl_RightSide.Controls.Add(this.txt_Hex);
            this.pnl_RightSide.Location = new System.Drawing.Point(404, 17);
            this.pnl_RightSide.Name = "pnl_RightSide";
            this.pnl_RightSide.Size = new System.Drawing.Size(413, 397);
            this.pnl_RightSide.TabIndex = 18;
            // 
            // btn_Check
            // 
            this.btn_Check.Location = new System.Drawing.Point(14, 109);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(80, 31);
            this.btn_Check.TabIndex = 18;
            this.btn_Check.Text = "Check";
            this.btn_Check.UseVisualStyleBackColor = true;
            this.btn_Check.Visible = false;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // pnl_BottomSection
            // 
            this.pnl_BottomSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_BottomSection.Controls.Add(this.lbl_Messages_Text);
            this.pnl_BottomSection.Controls.Add(this.txt_Messages);
            this.pnl_BottomSection.Location = new System.Drawing.Point(33, 424);
            this.pnl_BottomSection.Name = "pnl_BottomSection";
            this.pnl_BottomSection.Size = new System.Drawing.Size(783, 135);
            this.pnl_BottomSection.TabIndex = 19;
            // 
            // menu_Top
            // 
            this.menu_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Form,
            this.menuItem_About});
            this.menu_Top.Location = new System.Drawing.Point(0, 0);
            this.menu_Top.Name = "menu_Top";
            this.menu_Top.Size = new System.Drawing.Size(837, 24);
            this.menu_Top.TabIndex = 20;
            this.menu_Top.Text = "menuStrip1";
            // 
            // menuItem_Form
            // 
            this.menuItem_Form.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Form_LEDecoder});
            this.menuItem_Form.Name = "menuItem_Form";
            this.menuItem_Form.Size = new System.Drawing.Size(47, 20);
            this.menuItem_Form.Text = "Form";
            // 
            // menuItem_Form_LEDecoder
            // 
            this.menuItem_Form_LEDecoder.Name = "menuItem_Form_LEDecoder";
            this.menuItem_Form_LEDecoder.Size = new System.Drawing.Size(130, 22);
            this.menuItem_Form_LEDecoder.Text = "LEDecoder";
            this.menuItem_Form_LEDecoder.Click += new System.EventHandler(this.menuItem_Form_LEDecoder_Click);
            // 
            // menuItem_About
            // 
            this.menuItem_About.Name = "menuItem_About";
            this.menuItem_About.Size = new System.Drawing.Size(61, 20);
            this.menuItem_About.Text = "About...";
            this.menuItem_About.Click += new System.EventHandler(this.menuItem_About_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 571);
            this.Controls.Add(this.pnl_BottomSection);
            this.Controls.Add(this.pnl_RightSide);
            this.Controls.Add(this.lbl_ASM_Text);
            this.Controls.Add(this.txt_ASM);
            this.Controls.Add(this.menu_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu_Top;
            this.Name = "MainForm";
            this.Text = "MassHexASM";
            this.pnl_RightSide.ResumeLayout(false);
            this.pnl_RightSide.PerformLayout();
            this.pnl_BottomSection.ResumeLayout(false);
            this.pnl_BottomSection.PerformLayout();
            this.menu_Top.ResumeLayout(false);
            this.menu_Top.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.CheckBox chk_NameRegs;
		private System.Windows.Forms.CheckBox chk_IncludeAddress;
		private System.Windows.Forms.CheckBox chk_SpacePad;
		private System.Windows.Forms.Label lbl_SpacePadNum_Text;
		private System.Windows.Forms.TextBox txt_SpacePad;
		private System.Windows.Forms.TextBox txt_StartPC;
		private System.Windows.Forms.Label lbl_StartPC_Text;
		private System.Windows.Forms.Button btn_Decode;
		private System.Windows.Forms.TextBox txt_Messages;
		private System.Windows.Forms.Label lbl_Messages_Text;
		private System.Windows.Forms.Label lbl_Hex_Text;
		private System.Windows.Forms.Label lbl_ASM_Text;
		private System.Windows.Forms.CheckBox chk_littleEndian;
		private System.Windows.Forms.Button btn_Encode;
		private System.Windows.Forms.TextBox txt_Hex;
		private System.Windows.Forms.TextBox txt_ASM;
        private System.Windows.Forms.ComboBox cb_Mode;
        private System.Windows.Forms.Label lbl_Mode;
        private System.Windows.Forms.Panel pnl_RightSide;
        private System.Windows.Forms.Panel pnl_BottomSection;
        private System.Windows.Forms.Button btn_Check;
        private System.Windows.Forms.MenuStrip menu_Top;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Form;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Form_LEDecoder;
        private System.Windows.Forms.ToolStripMenuItem menuItem_About;
	}
}
