namespace EntryEdit.Forms
{
    partial class ISOForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Params = new System.Windows.Forms.Panel();
            this.spinner_BattleConditionals_Offset = new System.Windows.Forms.NumericUpDown();
            this.lbl_BattleConditionals_Offset = new System.Windows.Forms.Label();
            this.spinner_Events_Sector = new System.Windows.Forms.NumericUpDown();
            this.lbl_Events_Sector = new System.Windows.Forms.Label();
            this.chk_Events = new System.Windows.Forms.CheckBox();
            this.spinner_BattleConditionals_Sector = new System.Windows.Forms.NumericUpDown();
            this.lbl_BattleConditionals_Sector = new System.Windows.Forms.Label();
            this.chk_BattleConditionals = new System.Windows.Forms.CheckBox();
            this.lbl_Events_Offset = new System.Windows.Forms.Label();
            this.spinner_Events_Offset = new System.Windows.Forms.NumericUpDown();
            this.cmb_BattleConditionals_Sector = new System.Windows.Forms.ComboBox();
            this.cmb_Events_Sector = new System.Windows.Forms.ComboBox();
            this.chk_WorldConditionals = new System.Windows.Forms.CheckBox();
            this.lbl_WorldConditionals_Sector = new System.Windows.Forms.Label();
            this.spinner_WorldConditionals_Sector = new System.Windows.Forms.NumericUpDown();
            this.cmb_WorldConditionals_Sector = new System.Windows.Forms.ComboBox();
            this.lbl_WorldConditionals_Offset = new System.Windows.Forms.Label();
            this.spinner_WorldConditionals_Offset = new System.Windows.Forms.NumericUpDown();
            this.lbl_ISO = new System.Windows.Forms.Label();
            this.btn_ISO = new System.Windows.Forms.Button();
            this.txt_ISO = new System.Windows.Forms.TextBox();
            this.btn_Load = new System.Windows.Forms.Button();
            this.btn_Patch = new System.Windows.Forms.Button();
            this.lbl_BattleConditionals_Size = new System.Windows.Forms.Label();
            this.lbl_WorldConditionals_Size = new System.Windows.Forms.Label();
            this.lbl_Events_Size = new System.Windows.Forms.Label();
            this.spinner_BattleConditionals_Size = new System.Windows.Forms.NumericUpDown();
            this.spinner_WorldConditionals_Size = new System.Windows.Forms.NumericUpDown();
            this.spinner_Events_Size = new System.Windows.Forms.NumericUpDown();
            this.pnl_Params.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Sector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Sector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Sector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_Params
            // 
            this.pnl_Params.Controls.Add(this.spinner_Events_Size);
            this.pnl_Params.Controls.Add(this.spinner_WorldConditionals_Size);
            this.pnl_Params.Controls.Add(this.spinner_BattleConditionals_Size);
            this.pnl_Params.Controls.Add(this.lbl_Events_Size);
            this.pnl_Params.Controls.Add(this.lbl_WorldConditionals_Size);
            this.pnl_Params.Controls.Add(this.lbl_BattleConditionals_Size);
            this.pnl_Params.Controls.Add(this.spinner_WorldConditionals_Offset);
            this.pnl_Params.Controls.Add(this.lbl_WorldConditionals_Offset);
            this.pnl_Params.Controls.Add(this.cmb_WorldConditionals_Sector);
            this.pnl_Params.Controls.Add(this.spinner_WorldConditionals_Sector);
            this.pnl_Params.Controls.Add(this.lbl_WorldConditionals_Sector);
            this.pnl_Params.Controls.Add(this.chk_WorldConditionals);
            this.pnl_Params.Controls.Add(this.cmb_Events_Sector);
            this.pnl_Params.Controls.Add(this.cmb_BattleConditionals_Sector);
            this.pnl_Params.Controls.Add(this.spinner_Events_Offset);
            this.pnl_Params.Controls.Add(this.lbl_Events_Offset);
            this.pnl_Params.Controls.Add(this.spinner_BattleConditionals_Offset);
            this.pnl_Params.Controls.Add(this.lbl_BattleConditionals_Offset);
            this.pnl_Params.Controls.Add(this.spinner_Events_Sector);
            this.pnl_Params.Controls.Add(this.lbl_Events_Sector);
            this.pnl_Params.Controls.Add(this.chk_Events);
            this.pnl_Params.Controls.Add(this.spinner_BattleConditionals_Sector);
            this.pnl_Params.Controls.Add(this.lbl_BattleConditionals_Sector);
            this.pnl_Params.Controls.Add(this.chk_BattleConditionals);
            this.pnl_Params.Location = new System.Drawing.Point(13, 37);
            this.pnl_Params.Name = "pnl_Params";
            this.pnl_Params.Size = new System.Drawing.Size(809, 122);
            this.pnl_Params.TabIndex = 4;
            this.pnl_Params.Visible = false;
            // 
            // spinner_BattleConditionals_Offset
            // 
            this.spinner_BattleConditionals_Offset.Hexadecimal = true;
            this.spinner_BattleConditionals_Offset.Location = new System.Drawing.Point(600, 5);
            this.spinner_BattleConditionals_Offset.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_BattleConditionals_Offset.Name = "spinner_BattleConditionals_Offset";
            this.spinner_BattleConditionals_Offset.Size = new System.Drawing.Size(70, 20);
            this.spinner_BattleConditionals_Offset.TabIndex = 10;
            // 
            // lbl_BattleConditionals_Offset
            // 
            this.lbl_BattleConditionals_Offset.AutoSize = true;
            this.lbl_BattleConditionals_Offset.Location = new System.Drawing.Point(545, 8);
            this.lbl_BattleConditionals_Offset.Name = "lbl_BattleConditionals_Offset";
            this.lbl_BattleConditionals_Offset.Size = new System.Drawing.Size(55, 13);
            this.lbl_BattleConditionals_Offset.TabIndex = 9;
            this.lbl_BattleConditionals_Offset.Text = "Offset:  0x";
            // 
            // spinner_Events_Sector
            // 
            this.spinner_Events_Sector.Location = new System.Drawing.Point(200, 86);
            this.spinner_Events_Sector.Maximum = new decimal(new int[] {
            358400,
            0,
            0,
            0});
            this.spinner_Events_Sector.Name = "spinner_Events_Sector";
            this.spinner_Events_Sector.Size = new System.Drawing.Size(62, 20);
            this.spinner_Events_Sector.TabIndex = 7;
            this.spinner_Events_Sector.ValueChanged += new System.EventHandler(this.spinner_Events_Sector_ValueChanged);
            // 
            // lbl_Events_Sector
            // 
            this.lbl_Events_Sector.AutoSize = true;
            this.lbl_Events_Sector.Location = new System.Drawing.Point(150, 89);
            this.lbl_Events_Sector.Name = "lbl_Events_Sector";
            this.lbl_Events_Sector.Size = new System.Drawing.Size(41, 13);
            this.lbl_Events_Sector.TabIndex = 6;
            this.lbl_Events_Sector.Text = "Sector:";
            // 
            // chk_Events
            // 
            this.chk_Events.AutoSize = true;
            this.chk_Events.Checked = true;
            this.chk_Events.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Events.Location = new System.Drawing.Point(10, 88);
            this.chk_Events.Name = "chk_Events";
            this.chk_Events.Size = new System.Drawing.Size(59, 17);
            this.chk_Events.TabIndex = 5;
            this.chk_Events.Text = "Events";
            this.chk_Events.UseVisualStyleBackColor = true;
            this.chk_Events.CheckedChanged += new System.EventHandler(this.chk_Events_CheckedChanged);
            // 
            // spinner_BattleConditionals_Sector
            // 
            this.spinner_BattleConditionals_Sector.Location = new System.Drawing.Point(200, 6);
            this.spinner_BattleConditionals_Sector.Maximum = new decimal(new int[] {
            358400,
            0,
            0,
            0});
            this.spinner_BattleConditionals_Sector.Name = "spinner_BattleConditionals_Sector";
            this.spinner_BattleConditionals_Sector.Size = new System.Drawing.Size(62, 20);
            this.spinner_BattleConditionals_Sector.TabIndex = 4;
            this.spinner_BattleConditionals_Sector.ValueChanged += new System.EventHandler(this.spinner_BattleConditionals_Sector_ValueChanged);
            // 
            // lbl_BattleConditionals_Sector
            // 
            this.lbl_BattleConditionals_Sector.AutoSize = true;
            this.lbl_BattleConditionals_Sector.Location = new System.Drawing.Point(150, 9);
            this.lbl_BattleConditionals_Sector.Name = "lbl_BattleConditionals_Sector";
            this.lbl_BattleConditionals_Sector.Size = new System.Drawing.Size(41, 13);
            this.lbl_BattleConditionals_Sector.TabIndex = 3;
            this.lbl_BattleConditionals_Sector.Text = "Sector:";
            // 
            // chk_BattleConditionals
            // 
            this.chk_BattleConditionals.AutoSize = true;
            this.chk_BattleConditionals.Checked = true;
            this.chk_BattleConditionals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_BattleConditionals.Location = new System.Drawing.Point(10, 8);
            this.chk_BattleConditionals.Name = "chk_BattleConditionals";
            this.chk_BattleConditionals.Size = new System.Drawing.Size(113, 17);
            this.chk_BattleConditionals.TabIndex = 0;
            this.chk_BattleConditionals.Text = "Battle Conditionals";
            this.chk_BattleConditionals.UseVisualStyleBackColor = true;
            this.chk_BattleConditionals.CheckedChanged += new System.EventHandler(this.chk_BattleConditionals_CheckedChanged);
            // 
            // lbl_Events_Offset
            // 
            this.lbl_Events_Offset.AutoSize = true;
            this.lbl_Events_Offset.Location = new System.Drawing.Point(545, 88);
            this.lbl_Events_Offset.Name = "lbl_Events_Offset";
            this.lbl_Events_Offset.Size = new System.Drawing.Size(55, 13);
            this.lbl_Events_Offset.TabIndex = 11;
            this.lbl_Events_Offset.Text = "Offset:  0x";
            // 
            // spinner_Events_Offset
            // 
            this.spinner_Events_Offset.Hexadecimal = true;
            this.spinner_Events_Offset.Location = new System.Drawing.Point(600, 85);
            this.spinner_Events_Offset.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_Events_Offset.Name = "spinner_Events_Offset";
            this.spinner_Events_Offset.Size = new System.Drawing.Size(70, 20);
            this.spinner_Events_Offset.TabIndex = 12;
            // 
            // cmb_BattleConditionals_Sector
            // 
            this.cmb_BattleConditionals_Sector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_BattleConditionals_Sector.FormattingEnabled = true;
            this.cmb_BattleConditionals_Sector.Location = new System.Drawing.Point(268, 5);
            this.cmb_BattleConditionals_Sector.Name = "cmb_BattleConditionals_Sector";
            this.cmb_BattleConditionals_Sector.Size = new System.Drawing.Size(244, 21);
            this.cmb_BattleConditionals_Sector.TabIndex = 13;
            this.cmb_BattleConditionals_Sector.SelectedIndexChanged += new System.EventHandler(this.cmb_BattleConditionals_Sector_SelectedIndexChanged);
            // 
            // cmb_Events_Sector
            // 
            this.cmb_Events_Sector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Events_Sector.FormattingEnabled = true;
            this.cmb_Events_Sector.Location = new System.Drawing.Point(268, 85);
            this.cmb_Events_Sector.Name = "cmb_Events_Sector";
            this.cmb_Events_Sector.Size = new System.Drawing.Size(244, 21);
            this.cmb_Events_Sector.TabIndex = 14;
            this.cmb_Events_Sector.SelectedIndexChanged += new System.EventHandler(this.cmb_Events_Sector_SelectedIndexChanged);
            // 
            // chk_WorldConditionals
            // 
            this.chk_WorldConditionals.AutoSize = true;
            this.chk_WorldConditionals.Checked = true;
            this.chk_WorldConditionals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_WorldConditionals.Location = new System.Drawing.Point(10, 48);
            this.chk_WorldConditionals.Name = "chk_WorldConditionals";
            this.chk_WorldConditionals.Size = new System.Drawing.Size(114, 17);
            this.chk_WorldConditionals.TabIndex = 15;
            this.chk_WorldConditionals.Text = "World Conditionals";
            this.chk_WorldConditionals.UseVisualStyleBackColor = true;
            this.chk_WorldConditionals.CheckedChanged += new System.EventHandler(this.chk_WorldConditionals_CheckedChanged);
            // 
            // lbl_WorldConditionals_Sector
            // 
            this.lbl_WorldConditionals_Sector.AutoSize = true;
            this.lbl_WorldConditionals_Sector.Location = new System.Drawing.Point(150, 49);
            this.lbl_WorldConditionals_Sector.Name = "lbl_WorldConditionals_Sector";
            this.lbl_WorldConditionals_Sector.Size = new System.Drawing.Size(41, 13);
            this.lbl_WorldConditionals_Sector.TabIndex = 16;
            this.lbl_WorldConditionals_Sector.Text = "Sector:";
            // 
            // spinner_WorldConditionals_Sector
            // 
            this.spinner_WorldConditionals_Sector.Location = new System.Drawing.Point(200, 46);
            this.spinner_WorldConditionals_Sector.Maximum = new decimal(new int[] {
            358400,
            0,
            0,
            0});
            this.spinner_WorldConditionals_Sector.Name = "spinner_WorldConditionals_Sector";
            this.spinner_WorldConditionals_Sector.Size = new System.Drawing.Size(62, 20);
            this.spinner_WorldConditionals_Sector.TabIndex = 17;
            this.spinner_WorldConditionals_Sector.ValueChanged += new System.EventHandler(this.spinner_WorldConditionals_Sector_ValueChanged);
            // 
            // cmb_WorldConditionals_Sector
            // 
            this.cmb_WorldConditionals_Sector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_WorldConditionals_Sector.FormattingEnabled = true;
            this.cmb_WorldConditionals_Sector.Location = new System.Drawing.Point(268, 45);
            this.cmb_WorldConditionals_Sector.Name = "cmb_WorldConditionals_Sector";
            this.cmb_WorldConditionals_Sector.Size = new System.Drawing.Size(244, 21);
            this.cmb_WorldConditionals_Sector.TabIndex = 18;
            this.cmb_WorldConditionals_Sector.SelectedIndexChanged += new System.EventHandler(this.cmb_WorldConditionals_Sector_SelectedIndexChanged);
            // 
            // lbl_WorldConditionals_Offset
            // 
            this.lbl_WorldConditionals_Offset.AutoSize = true;
            this.lbl_WorldConditionals_Offset.Location = new System.Drawing.Point(545, 48);
            this.lbl_WorldConditionals_Offset.Name = "lbl_WorldConditionals_Offset";
            this.lbl_WorldConditionals_Offset.Size = new System.Drawing.Size(55, 13);
            this.lbl_WorldConditionals_Offset.TabIndex = 19;
            this.lbl_WorldConditionals_Offset.Text = "Offset:  0x";
            // 
            // spinner_WorldConditionals_Offset
            // 
            this.spinner_WorldConditionals_Offset.Hexadecimal = true;
            this.spinner_WorldConditionals_Offset.Location = new System.Drawing.Point(600, 45);
            this.spinner_WorldConditionals_Offset.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_WorldConditionals_Offset.Name = "spinner_WorldConditionals_Offset";
            this.spinner_WorldConditionals_Offset.Size = new System.Drawing.Size(70, 20);
            this.spinner_WorldConditionals_Offset.TabIndex = 20;
            // 
            // lbl_ISO
            // 
            this.lbl_ISO.AutoSize = true;
            this.lbl_ISO.Location = new System.Drawing.Point(10, 13);
            this.lbl_ISO.Name = "lbl_ISO";
            this.lbl_ISO.Size = new System.Drawing.Size(28, 13);
            this.lbl_ISO.TabIndex = 5;
            this.lbl_ISO.Text = "ISO:";
            // 
            // btn_ISO
            // 
            this.btn_ISO.Location = new System.Drawing.Point(793, 8);
            this.btn_ISO.Name = "btn_ISO";
            this.btn_ISO.Size = new System.Drawing.Size(29, 22);
            this.btn_ISO.TabIndex = 6;
            this.btn_ISO.Text = "...";
            this.btn_ISO.UseVisualStyleBackColor = true;
            this.btn_ISO.Click += new System.EventHandler(this.btn_ISO_Click);
            // 
            // txt_ISO
            // 
            this.txt_ISO.Location = new System.Drawing.Point(45, 10);
            this.txt_ISO.Name = "txt_ISO";
            this.txt_ISO.ReadOnly = true;
            this.txt_ISO.Size = new System.Drawing.Size(734, 20);
            this.txt_ISO.TabIndex = 7;
            // 
            // btn_Load
            // 
            this.btn_Load.Enabled = false;
            this.btn_Load.Location = new System.Drawing.Point(759, 177);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(63, 24);
            this.btn_Load.TabIndex = 8;
            this.btn_Load.Text = "Load";
            this.btn_Load.UseVisualStyleBackColor = true;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_Patch
            // 
            this.btn_Patch.Enabled = false;
            this.btn_Patch.Location = new System.Drawing.Point(759, 177);
            this.btn_Patch.Name = "btn_Patch";
            this.btn_Patch.Size = new System.Drawing.Size(63, 24);
            this.btn_Patch.TabIndex = 9;
            this.btn_Patch.Text = "Patch";
            this.btn_Patch.UseVisualStyleBackColor = true;
            this.btn_Patch.Click += new System.EventHandler(this.btn_Patch_Click);
            // 
            // lbl_BattleConditionals_Size
            // 
            this.lbl_BattleConditionals_Size.AutoSize = true;
            this.lbl_BattleConditionals_Size.Location = new System.Drawing.Point(700, 8);
            this.lbl_BattleConditionals_Size.Name = "lbl_BattleConditionals_Size";
            this.lbl_BattleConditionals_Size.Size = new System.Drawing.Size(30, 13);
            this.lbl_BattleConditionals_Size.TabIndex = 21;
            this.lbl_BattleConditionals_Size.Text = "Size:";
            // 
            // lbl_WorldConditionals_Size
            // 
            this.lbl_WorldConditionals_Size.AutoSize = true;
            this.lbl_WorldConditionals_Size.Location = new System.Drawing.Point(700, 48);
            this.lbl_WorldConditionals_Size.Name = "lbl_WorldConditionals_Size";
            this.lbl_WorldConditionals_Size.Size = new System.Drawing.Size(30, 13);
            this.lbl_WorldConditionals_Size.TabIndex = 22;
            this.lbl_WorldConditionals_Size.Text = "Size:";
            // 
            // lbl_Events_Size
            // 
            this.lbl_Events_Size.AutoSize = true;
            this.lbl_Events_Size.Location = new System.Drawing.Point(700, 88);
            this.lbl_Events_Size.Name = "lbl_Events_Size";
            this.lbl_Events_Size.Size = new System.Drawing.Size(30, 13);
            this.lbl_Events_Size.TabIndex = 23;
            this.lbl_Events_Size.Text = "Size:";
            // 
            // spinner_BattleConditionals_Size
            // 
            this.spinner_BattleConditionals_Size.Location = new System.Drawing.Point(732, 5);
            this.spinner_BattleConditionals_Size.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_BattleConditionals_Size.Name = "spinner_BattleConditionals_Size";
            this.spinner_BattleConditionals_Size.Size = new System.Drawing.Size(70, 20);
            this.spinner_BattleConditionals_Size.TabIndex = 24;
            // 
            // spinner_WorldConditionals_Size
            // 
            this.spinner_WorldConditionals_Size.Location = new System.Drawing.Point(732, 45);
            this.spinner_WorldConditionals_Size.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_WorldConditionals_Size.Name = "spinner_WorldConditionals_Size";
            this.spinner_WorldConditionals_Size.Size = new System.Drawing.Size(70, 20);
            this.spinner_WorldConditionals_Size.TabIndex = 25;
            // 
            // spinner_Events_Size
            // 
            this.spinner_Events_Size.Location = new System.Drawing.Point(732, 85);
            this.spinner_Events_Size.Maximum = new decimal(new int[] {
            734003200,
            0,
            0,
            0});
            this.spinner_Events_Size.Name = "spinner_Events_Size";
            this.spinner_Events_Size.Size = new System.Drawing.Size(70, 20);
            this.spinner_Events_Size.TabIndex = 26;
            // 
            // ISOForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 211);
            this.Controls.Add(this.btn_Patch);
            this.Controls.Add(this.btn_Load);
            this.Controls.Add(this.txt_ISO);
            this.Controls.Add(this.btn_ISO);
            this.Controls.Add(this.lbl_ISO);
            this.Controls.Add(this.pnl_Params);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ISOForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.pnl_Params.ResumeLayout(false);
            this.pnl_Params.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Sector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Sector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Sector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Events_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Params;
        private System.Windows.Forms.NumericUpDown spinner_BattleConditionals_Offset;
        private System.Windows.Forms.Label lbl_BattleConditionals_Offset;
        private System.Windows.Forms.NumericUpDown spinner_Events_Sector;
        private System.Windows.Forms.Label lbl_Events_Sector;
        private System.Windows.Forms.CheckBox chk_Events;
        private System.Windows.Forms.NumericUpDown spinner_BattleConditionals_Sector;
        private System.Windows.Forms.Label lbl_BattleConditionals_Sector;
        private System.Windows.Forms.CheckBox chk_BattleConditionals;
        private System.Windows.Forms.NumericUpDown spinner_Events_Offset;
        private System.Windows.Forms.Label lbl_Events_Offset;
        private System.Windows.Forms.NumericUpDown spinner_WorldConditionals_Offset;
        private System.Windows.Forms.Label lbl_WorldConditionals_Offset;
        private System.Windows.Forms.ComboBox cmb_WorldConditionals_Sector;
        private System.Windows.Forms.NumericUpDown spinner_WorldConditionals_Sector;
        private System.Windows.Forms.Label lbl_WorldConditionals_Sector;
        private System.Windows.Forms.CheckBox chk_WorldConditionals;
        private System.Windows.Forms.ComboBox cmb_Events_Sector;
        private System.Windows.Forms.ComboBox cmb_BattleConditionals_Sector;
        private System.Windows.Forms.Label lbl_ISO;
        private System.Windows.Forms.Button btn_ISO;
        private System.Windows.Forms.TextBox txt_ISO;
        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.Button btn_Patch;
        private System.Windows.Forms.NumericUpDown spinner_BattleConditionals_Size;
        private System.Windows.Forms.Label lbl_Events_Size;
        private System.Windows.Forms.Label lbl_WorldConditionals_Size;
        private System.Windows.Forms.Label lbl_BattleConditionals_Size;
        private System.Windows.Forms.NumericUpDown spinner_Events_Size;
        private System.Windows.Forms.NumericUpDown spinner_WorldConditionals_Size;
    }
}