namespace EntryEdit.Forms
{
    partial class StateForm
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
            this.txt_File = new System.Windows.Forms.TextBox();
            this.lbl_File = new System.Windows.Forms.Label();
            this.btn_File = new System.Windows.Forms.Button();
            this.pnl_Battle = new System.Windows.Forms.Panel();
            this.spinner_BattleConditionals_RamLocation_Commands = new PatcherLib.Controls.NumericUpDownBase();
            this.lbl_BattleConditionals_RamLocation_Commands = new System.Windows.Forms.Label();
            this.cmb_Event = new System.Windows.Forms.ComboBox();
            this.spinner_Event_RamLocation = new PatcherLib.Controls.NumericUpDownBase();
            this.lbl_Event_RamLocation = new System.Windows.Forms.Label();
            this.chk_Event = new System.Windows.Forms.CheckBox();
            this.spinner_BattleConditionals_RamLocation_Blocks = new PatcherLib.Controls.NumericUpDownBase();
            this.lbl_BattleConditionals_RamLocation_Blocks = new System.Windows.Forms.Label();
            this.cmb_BattleConditionals_ConditionalSet = new System.Windows.Forms.ComboBox();
            this.chk_BattleConditionals = new System.Windows.Forms.CheckBox();
            this.pnl_World = new System.Windows.Forms.Panel();
            this.spinner_WorldConditionals_RamLocation = new PatcherLib.Controls.NumericUpDownBase();
            this.lbl_WorldConditionals_RamLocation = new System.Windows.Forms.Label();
            this.chk_WorldConditionals = new System.Windows.Forms.CheckBox();
            this.btn_Load = new System.Windows.Forms.Button();
            this.btn_Patch = new System.Windows.Forms.Button();
            this.lbl_WorldConditionals_Size = new System.Windows.Forms.Label();
            this.spinner_WorldConditionals_Size = new PatcherLib.Controls.NumericUpDownBase();
            this.pnl_Battle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_RamLocation_Commands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Event_RamLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_RamLocation_Blocks)).BeginInit();
            this.pnl_World.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_RamLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_File
            // 
            this.txt_File.Location = new System.Drawing.Point(45, 10);
            this.txt_File.Name = "txt_File";
            this.txt_File.ReadOnly = true;
            this.txt_File.Size = new System.Drawing.Size(500, 20);
            this.txt_File.TabIndex = 0;
            // 
            // lbl_File
            // 
            this.lbl_File.AutoSize = true;
            this.lbl_File.Location = new System.Drawing.Point(10, 13);
            this.lbl_File.Name = "lbl_File";
            this.lbl_File.Size = new System.Drawing.Size(26, 13);
            this.lbl_File.TabIndex = 1;
            this.lbl_File.Text = "File:";
            // 
            // btn_File
            // 
            this.btn_File.Location = new System.Drawing.Point(555, 9);
            this.btn_File.Name = "btn_File";
            this.btn_File.Size = new System.Drawing.Size(29, 22);
            this.btn_File.TabIndex = 2;
            this.btn_File.Text = "...";
            this.btn_File.UseVisualStyleBackColor = true;
            this.btn_File.Click += new System.EventHandler(this.btn_File_Click);
            // 
            // pnl_Battle
            // 
            this.pnl_Battle.Controls.Add(this.spinner_BattleConditionals_RamLocation_Commands);
            this.pnl_Battle.Controls.Add(this.lbl_BattleConditionals_RamLocation_Commands);
            this.pnl_Battle.Controls.Add(this.cmb_Event);
            this.pnl_Battle.Controls.Add(this.spinner_Event_RamLocation);
            this.pnl_Battle.Controls.Add(this.lbl_Event_RamLocation);
            this.pnl_Battle.Controls.Add(this.chk_Event);
            this.pnl_Battle.Controls.Add(this.spinner_BattleConditionals_RamLocation_Blocks);
            this.pnl_Battle.Controls.Add(this.lbl_BattleConditionals_RamLocation_Blocks);
            this.pnl_Battle.Controls.Add(this.cmb_BattleConditionals_ConditionalSet);
            this.pnl_Battle.Controls.Add(this.chk_BattleConditionals);
            this.pnl_Battle.Location = new System.Drawing.Point(13, 37);
            this.pnl_Battle.Name = "pnl_Battle";
            this.pnl_Battle.Size = new System.Drawing.Size(573, 122);
            this.pnl_Battle.TabIndex = 3;
            this.pnl_Battle.Visible = false;
            // 
            // spinner_BattleConditionals_RamLocation_Commands
            // 
            this.spinner_BattleConditionals_RamLocation_Commands.Hexadecimal = true;
            this.spinner_BattleConditionals_RamLocation_Commands.Location = new System.Drawing.Point(419, 7);
            this.spinner_BattleConditionals_RamLocation_Commands.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_BattleConditionals_RamLocation_Commands.Name = "spinner_BattleConditionals_RamLocation_Commands";
            this.spinner_BattleConditionals_RamLocation_Commands.Size = new System.Drawing.Size(100, 20);
            this.spinner_BattleConditionals_RamLocation_Commands.TabIndex = 10;
            // 
            // lbl_BattleConditionals_RamLocation_Commands
            // 
            this.lbl_BattleConditionals_RamLocation_Commands.AutoSize = true;
            this.lbl_BattleConditionals_RamLocation_Commands.Location = new System.Drawing.Point(401, 8);
            this.lbl_BattleConditionals_RamLocation_Commands.Name = "lbl_BattleConditionals_RamLocation_Commands";
            this.lbl_BattleConditionals_RamLocation_Commands.Size = new System.Drawing.Size(18, 13);
            this.lbl_BattleConditionals_RamLocation_Commands.TabIndex = 9;
            this.lbl_BattleConditionals_RamLocation_Commands.Text = "0x";
            // 
            // cmb_Event
            // 
            this.cmb_Event.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Event.FormattingEnabled = true;
            this.cmb_Event.Location = new System.Drawing.Point(10, 90);
            this.cmb_Event.Name = "cmb_Event";
            this.cmb_Event.Size = new System.Drawing.Size(550, 21);
            this.cmb_Event.TabIndex = 8;
            // 
            // spinner_Event_RamLocation
            // 
            this.spinner_Event_RamLocation.Hexadecimal = true;
            this.spinner_Event_RamLocation.Location = new System.Drawing.Point(295, 66);
            this.spinner_Event_RamLocation.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_Event_RamLocation.Name = "spinner_Event_RamLocation";
            this.spinner_Event_RamLocation.Size = new System.Drawing.Size(100, 20);
            this.spinner_Event_RamLocation.TabIndex = 7;
            // 
            // lbl_Event_RamLocation
            // 
            this.lbl_Event_RamLocation.AutoSize = true;
            this.lbl_Event_RamLocation.Location = new System.Drawing.Point(200, 67);
            this.lbl_Event_RamLocation.Name = "lbl_Event_RamLocation";
            this.lbl_Event_RamLocation.Size = new System.Drawing.Size(95, 13);
            this.lbl_Event_RamLocation.TabIndex = 6;
            this.lbl_Event_RamLocation.Text = "RAM Location:  0x";
            // 
            // chk_Event
            // 
            this.chk_Event.AutoSize = true;
            this.chk_Event.Checked = true;
            this.chk_Event.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Event.Location = new System.Drawing.Point(10, 67);
            this.chk_Event.Name = "chk_Event";
            this.chk_Event.Size = new System.Drawing.Size(54, 17);
            this.chk_Event.TabIndex = 5;
            this.chk_Event.Text = "Event";
            this.chk_Event.UseVisualStyleBackColor = true;
            this.chk_Event.CheckedChanged += new System.EventHandler(this.chk_Event_CheckedChanged);
            // 
            // spinner_BattleConditionals_RamLocation_Blocks
            // 
            this.spinner_BattleConditionals_RamLocation_Blocks.Hexadecimal = true;
            this.spinner_BattleConditionals_RamLocation_Blocks.Location = new System.Drawing.Point(295, 7);
            this.spinner_BattleConditionals_RamLocation_Blocks.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_BattleConditionals_RamLocation_Blocks.Name = "spinner_BattleConditionals_RamLocation_Blocks";
            this.spinner_BattleConditionals_RamLocation_Blocks.Size = new System.Drawing.Size(100, 20);
            this.spinner_BattleConditionals_RamLocation_Blocks.TabIndex = 4;
            // 
            // lbl_BattleConditionals_RamLocation_Blocks
            // 
            this.lbl_BattleConditionals_RamLocation_Blocks.AutoSize = true;
            this.lbl_BattleConditionals_RamLocation_Blocks.Location = new System.Drawing.Point(200, 9);
            this.lbl_BattleConditionals_RamLocation_Blocks.Name = "lbl_BattleConditionals_RamLocation_Blocks";
            this.lbl_BattleConditionals_RamLocation_Blocks.Size = new System.Drawing.Size(95, 13);
            this.lbl_BattleConditionals_RamLocation_Blocks.TabIndex = 3;
            this.lbl_BattleConditionals_RamLocation_Blocks.Text = "RAM Location:  0x";
            // 
            // cmb_BattleConditionals_ConditionalSet
            // 
            this.cmb_BattleConditionals_ConditionalSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_BattleConditionals_ConditionalSet.FormattingEnabled = true;
            this.cmb_BattleConditionals_ConditionalSet.Location = new System.Drawing.Point(10, 31);
            this.cmb_BattleConditionals_ConditionalSet.Name = "cmb_BattleConditionals_ConditionalSet";
            this.cmb_BattleConditionals_ConditionalSet.Size = new System.Drawing.Size(550, 21);
            this.cmb_BattleConditionals_ConditionalSet.TabIndex = 2;
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
            // pnl_World
            // 
            this.pnl_World.Controls.Add(this.spinner_WorldConditionals_Size);
            this.pnl_World.Controls.Add(this.lbl_WorldConditionals_Size);
            this.pnl_World.Controls.Add(this.spinner_WorldConditionals_RamLocation);
            this.pnl_World.Controls.Add(this.lbl_WorldConditionals_RamLocation);
            this.pnl_World.Controls.Add(this.chk_WorldConditionals);
            this.pnl_World.Location = new System.Drawing.Point(13, 37);
            this.pnl_World.Name = "pnl_World";
            this.pnl_World.Size = new System.Drawing.Size(573, 33);
            this.pnl_World.TabIndex = 4;
            this.pnl_World.Visible = false;
            // 
            // spinner_WorldConditionals_RamLocation
            // 
            this.spinner_WorldConditionals_RamLocation.Hexadecimal = true;
            this.spinner_WorldConditionals_RamLocation.Location = new System.Drawing.Point(295, 7);
            this.spinner_WorldConditionals_RamLocation.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_WorldConditionals_RamLocation.Name = "spinner_WorldConditionals_RamLocation";
            this.spinner_WorldConditionals_RamLocation.Size = new System.Drawing.Size(100, 20);
            this.spinner_WorldConditionals_RamLocation.TabIndex = 4;
            // 
            // lbl_WorldConditionals_RamLocation
            // 
            this.lbl_WorldConditionals_RamLocation.AutoSize = true;
            this.lbl_WorldConditionals_RamLocation.Location = new System.Drawing.Point(200, 9);
            this.lbl_WorldConditionals_RamLocation.Name = "lbl_WorldConditionals_RamLocation";
            this.lbl_WorldConditionals_RamLocation.Size = new System.Drawing.Size(95, 13);
            this.lbl_WorldConditionals_RamLocation.TabIndex = 3;
            this.lbl_WorldConditionals_RamLocation.Text = "RAM Location:  0x";
            // 
            // chk_WorldConditionals
            // 
            this.chk_WorldConditionals.AutoSize = true;
            this.chk_WorldConditionals.Checked = true;
            this.chk_WorldConditionals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_WorldConditionals.Location = new System.Drawing.Point(10, 8);
            this.chk_WorldConditionals.Name = "chk_WorldConditionals";
            this.chk_WorldConditionals.Size = new System.Drawing.Size(114, 17);
            this.chk_WorldConditionals.TabIndex = 0;
            this.chk_WorldConditionals.Text = "World Conditionals";
            this.chk_WorldConditionals.UseVisualStyleBackColor = true;
            this.chk_WorldConditionals.CheckedChanged += new System.EventHandler(this.chk_WorldConditionals_CheckedChanged);
            // 
            // btn_Load
            // 
            this.btn_Load.Enabled = false;
            this.btn_Load.Location = new System.Drawing.Point(523, 177);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(63, 24);
            this.btn_Load.TabIndex = 5;
            this.btn_Load.Text = "Load";
            this.btn_Load.UseVisualStyleBackColor = true;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_Patch
            // 
            this.btn_Patch.Enabled = false;
            this.btn_Patch.Location = new System.Drawing.Point(523, 177);
            this.btn_Patch.Name = "btn_Patch";
            this.btn_Patch.Size = new System.Drawing.Size(63, 24);
            this.btn_Patch.TabIndex = 6;
            this.btn_Patch.Text = "Patch";
            this.btn_Patch.UseVisualStyleBackColor = true;
            this.btn_Patch.Click += new System.EventHandler(this.btn_Patch_Click);
            // 
            // lbl_WorldConditionals_Size
            // 
            this.lbl_WorldConditionals_Size.AutoSize = true;
            this.lbl_WorldConditionals_Size.Location = new System.Drawing.Point(416, 9);
            this.lbl_WorldConditionals_Size.Name = "lbl_WorldConditionals_Size";
            this.lbl_WorldConditionals_Size.Size = new System.Drawing.Size(30, 13);
            this.lbl_WorldConditionals_Size.TabIndex = 5;
            this.lbl_WorldConditionals_Size.Text = "Size:";
            // 
            // spinner_WorldConditionals_Size
            // 
            this.spinner_WorldConditionals_Size.Location = new System.Drawing.Point(450, 7);
            this.spinner_WorldConditionals_Size.Maximum = new decimal(new int[] {
            2097151,
            0,
            0,
            0});
            this.spinner_WorldConditionals_Size.Name = "spinner_WorldConditionals_Size";
            this.spinner_WorldConditionals_Size.Size = new System.Drawing.Size(57, 20);
            this.spinner_WorldConditionals_Size.TabIndex = 6;
            // 
            // StateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 211);
            this.Controls.Add(this.btn_Patch);
            this.Controls.Add(this.btn_Load);
            this.Controls.Add(this.pnl_World);
            this.Controls.Add(this.pnl_Battle);
            this.Controls.Add(this.btn_File);
            this.Controls.Add(this.lbl_File);
            this.Controls.Add(this.txt_File);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.pnl_Battle.ResumeLayout(false);
            this.pnl_Battle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_RamLocation_Commands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Event_RamLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_BattleConditionals_RamLocation_Blocks)).EndInit();
            this.pnl_World.ResumeLayout(false);
            this.pnl_World.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_RamLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_WorldConditionals_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_File;
        private System.Windows.Forms.Label lbl_File;
        private System.Windows.Forms.Button btn_File;
        private System.Windows.Forms.Panel pnl_Battle;
        private System.Windows.Forms.CheckBox chk_BattleConditionals;
        private PatcherLib.Controls.NumericUpDownBase spinner_BattleConditionals_RamLocation_Blocks;
        private System.Windows.Forms.Label lbl_BattleConditionals_RamLocation_Blocks;
        private System.Windows.Forms.ComboBox cmb_BattleConditionals_ConditionalSet;
        private PatcherLib.Controls.NumericUpDownBase spinner_Event_RamLocation;
        private System.Windows.Forms.Label lbl_Event_RamLocation;
        private System.Windows.Forms.CheckBox chk_Event;
        private System.Windows.Forms.ComboBox cmb_Event;
        private System.Windows.Forms.Panel pnl_World;
        private PatcherLib.Controls.NumericUpDownBase spinner_WorldConditionals_RamLocation;
        private System.Windows.Forms.Label lbl_WorldConditionals_RamLocation;
        private System.Windows.Forms.CheckBox chk_WorldConditionals;
        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.Button btn_Patch;
        private System.Windows.Forms.Label lbl_BattleConditionals_RamLocation_Commands;
        private PatcherLib.Controls.NumericUpDownBase spinner_BattleConditionals_RamLocation_Commands;
        private PatcherLib.Controls.NumericUpDownBase spinner_WorldConditionals_Size;
        private System.Windows.Forms.Label lbl_WorldConditionals_Size;
    }
}