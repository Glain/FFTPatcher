namespace FFTorgASM
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnPatch = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.reloadButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.variableComboBox = new System.Windows.Forms.ComboBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.toggleButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lsb_FilesList = new PatcherLib.Controls.ColorListBox();
            this.btn_OpenConflictChecker = new System.Windows.Forms.Button();
            this.btn_ViewFreeSpace = new System.Windows.Forms.Button();
            this.txt_Messages = new System.Windows.Forms.TextBox();
            this.clb_Patches = new PatcherLib.Controls.ModifiedBGCheckedListBox();
            this.btn_UncheckAll = new System.Windows.Forms.Button();
            this.btn_Sort = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.cmb_Variable_Preset = new System.Windows.Forms.ComboBox();
            this.variableSpinner = new FFTorgASM.HexNumericUpDown();
            this.lbl_Mode = new System.Windows.Forms.Label();
            this.cmb_Mode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.variableSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPatch
            // 
            this.btnPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatch.Location = new System.Drawing.Point(480, 628);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(75, 23);
            this.btnPatch.TabIndex = 1;
            this.btnPatch.Text = "Patch...";
            this.btnPatch.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CheckFileExists = true;
            this.saveFileDialog1.OverwritePrompt = false;
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reloadButton.Location = new System.Drawing.Point(12, 355);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 2;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 384);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(543, 109);
            this.textBox1.TabIndex = 3;
            // 
            // variableComboBox
            // 
            this.variableComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableComboBox.FormattingEnabled = true;
            this.variableComboBox.Location = new System.Drawing.Point(12, 601);
            this.variableComboBox.Name = "variableComboBox";
            this.variableComboBox.Size = new System.Drawing.Size(208, 21);
            this.variableComboBox.TabIndex = 6;
            this.variableComboBox.Visible = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(510, 602);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(48, 13);
            this.versionLabel.TabIndex = 7;
            this.versionLabel.Text = "[Ver.]";
            // 
            // toggleButton
            // 
            this.toggleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleButton.Location = new System.Drawing.Point(480, 355);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(75, 23);
            this.toggleButton.TabIndex = 8;
            this.toggleButton.Text = "Toggle all";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAllButton.Location = new System.Drawing.Point(318, 355);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(75, 23);
            this.checkAllButton.TabIndex = 9;
            this.checkAllButton.Text = "Check all";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(296, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 12;
            // 
            // lsb_FilesList
            // 
            this.lsb_FilesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsb_FilesList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lsb_FilesList.Location = new System.Drawing.Point(12, 30);
            this.lsb_FilesList.Name = "lsb_FilesList";
            this.lsb_FilesList.Size = new System.Drawing.Size(186, 316);
            this.lsb_FilesList.TabIndex = 13;
            this.lsb_FilesList.SelectedIndexChanged += new System.EventHandler(this.lsb_FilesList_SelectedIndexChanged);
            // 
            // btn_OpenConflictChecker
            // 
            this.btn_OpenConflictChecker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OpenConflictChecker.Location = new System.Drawing.Point(413, 657);
            this.btn_OpenConflictChecker.Name = "btn_OpenConflictChecker";
            this.btn_OpenConflictChecker.Size = new System.Drawing.Size(142, 23);
            this.btn_OpenConflictChecker.TabIndex = 10;
            this.btn_OpenConflictChecker.Text = "Open Conflict Checker";
            this.btn_OpenConflictChecker.UseVisualStyleBackColor = true;
            this.btn_OpenConflictChecker.Click += new System.EventHandler(this.btn_OpenConflictChecker_Click);
            // 
            // btn_ViewFreeSpace
            // 
            this.btn_ViewFreeSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ViewFreeSpace.Location = new System.Drawing.Point(254, 657);
            this.btn_ViewFreeSpace.Name = "btn_ViewFreeSpace";
            this.btn_ViewFreeSpace.Size = new System.Drawing.Size(153, 23);
            this.btn_ViewFreeSpace.TabIndex = 14;
            this.btn_ViewFreeSpace.Text = "View Free Space";
            this.btn_ViewFreeSpace.UseVisualStyleBackColor = true;
            this.btn_ViewFreeSpace.Click += new System.EventHandler(this.btn_ViewFreeSpace_Click);
            // 
            // txt_Messages
            // 
            this.txt_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Messages.BackColor = System.Drawing.SystemColors.Control;
            this.txt_Messages.ForeColor = System.Drawing.Color.Red;
            this.txt_Messages.Location = new System.Drawing.Point(12, 499);
            this.txt_Messages.Multiline = true;
            this.txt_Messages.Name = "txt_Messages";
            this.txt_Messages.ReadOnly = true;
            this.txt_Messages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Messages.Size = new System.Drawing.Size(543, 96);
            this.txt_Messages.TabIndex = 15;
            // 
            // clb_Patches
            // 
            this.clb_Patches.AllowDrop = true;
            this.clb_Patches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clb_Patches.BackColors = null;
            this.clb_Patches.CheckOnClick = true;
            this.clb_Patches.FormattingEnabled = true;
            this.clb_Patches.IncludePrefix = true;
            this.clb_Patches.IntegralHeight = false;
            this.clb_Patches.Location = new System.Drawing.Point(204, 30);
            this.clb_Patches.Name = "clb_Patches";
            this.clb_Patches.Size = new System.Drawing.Size(351, 319);
            this.clb_Patches.TabIndex = 0;
            this.clb_Patches.DragDrop += new System.Windows.Forms.DragEventHandler(this.clb_Patches_DragDrop);
            this.clb_Patches.DragEnter += new System.Windows.Forms.DragEventHandler(this.clb_Patches_DragEnter);
            // 
            // btn_UncheckAll
            // 
            this.btn_UncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_UncheckAll.Location = new System.Drawing.Point(399, 355);
            this.btn_UncheckAll.Name = "btn_UncheckAll";
            this.btn_UncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btn_UncheckAll.TabIndex = 16;
            this.btn_UncheckAll.Text = "Uncheck all";
            this.btn_UncheckAll.UseVisualStyleBackColor = true;
            this.btn_UncheckAll.Click += new System.EventHandler(this.btn_UncheckAll_Click);
            // 
            // btn_Sort
            // 
            this.btn_Sort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Sort.Location = new System.Drawing.Point(204, 355);
            this.btn_Sort.Name = "btn_Sort";
            this.btn_Sort.Size = new System.Drawing.Size(75, 23);
            this.btn_Sort.TabIndex = 18;
            this.btn_Sort.Text = "Sort";
            this.btn_Sort.UseVisualStyleBackColor = true;
            this.btn_Sort.Click += new System.EventHandler(this.btn_Sort_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Clear.Location = new System.Drawing.Point(480, 3);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 23);
            this.btn_Clear.TabIndex = 19;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(205, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(269, 20);
            this.txt_Search.TabIndex = 20;
            this.txt_Search.TextChanged += new System.EventHandler(this.txt_Search_TextChanged);
            // 
            // lbl_Search
            // 
            this.lbl_Search.AutoSize = true;
            this.lbl_Search.Location = new System.Drawing.Point(155, 7);
            this.lbl_Search.Name = "lbl_Search";
            this.lbl_Search.Size = new System.Drawing.Size(44, 13);
            this.lbl_Search.TabIndex = 21;
            this.lbl_Search.Text = "Search:";
            // 
            // cmb_Variable_Preset
            // 
            this.cmb_Variable_Preset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmb_Variable_Preset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Variable_Preset.FormattingEnabled = true;
            this.cmb_Variable_Preset.Location = new System.Drawing.Point(226, 601);
            this.cmb_Variable_Preset.Name = "cmb_Variable_Preset";
            this.cmb_Variable_Preset.Size = new System.Drawing.Size(122, 21);
            this.cmb_Variable_Preset.TabIndex = 22;
            this.cmb_Variable_Preset.Visible = false;
            this.cmb_Variable_Preset.SelectedIndexChanged += new System.EventHandler(this.cmb_Variable_Preset_SelectedIndexChanged);
            // 
            // variableSpinner
            // 
            this.variableSpinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableSpinner.Hexadecimal = true;
            this.variableSpinner.Location = new System.Drawing.Point(226, 602);
            this.variableSpinner.Name = "variableSpinner";
            this.variableSpinner.Size = new System.Drawing.Size(102, 20);
            this.variableSpinner.TabIndex = 5;
            this.variableSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.variableSpinner.Visible = false;
            // 
            // lbl_Mode
            // 
            this.lbl_Mode.AutoSize = true;
            this.lbl_Mode.Location = new System.Drawing.Point(12, 7);
            this.lbl_Mode.Name = "lbl_Mode";
            this.lbl_Mode.Size = new System.Drawing.Size(37, 13);
            this.lbl_Mode.TabIndex = 23;
            this.lbl_Mode.Text = "Mode:";
            // 
            // cmb_Mode
            // 
            this.cmb_Mode.FormattingEnabled = true;
            this.cmb_Mode.Location = new System.Drawing.Point(55, 4);
            this.cmb_Mode.Name = "cmb_Mode";
            this.cmb_Mode.Size = new System.Drawing.Size(70, 21);
            this.cmb_Mode.TabIndex = 24;
            this.cmb_Mode.SelectedIndexChanged += new System.EventHandler(this.cmb_Mode_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 705);
            this.Controls.Add(this.cmb_Mode);
            this.Controls.Add(this.lbl_Mode);
            this.Controls.Add(this.cmb_Variable_Preset);
            this.Controls.Add(this.lbl_Search);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Sort);
            this.Controls.Add(this.btn_UncheckAll);
            this.Controls.Add(this.txt_Messages);
            this.Controls.Add(this.btn_ViewFreeSpace);
            this.Controls.Add(this.lsb_FilesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableSpinner);
            this.Controls.Add(this.btn_OpenConflictChecker);
            this.Controls.Add(this.checkAllButton);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.variableComboBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.btnPatch);
            this.Controls.Add(this.clb_Patches);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(583, 743);
            this.Name = "MainForm";
            this.Text = "FFTorgASM";
            ((System.ComponentModel.ISupportInitialize)(this.variableSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PatcherLib.Controls.ModifiedBGCheckedListBox clb_Patches;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TextBox textBox1;
        private FFTorgASM.HexNumericUpDown variableSpinner;
        private System.Windows.Forms.ComboBox variableComboBox;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Label label1;
        private PatcherLib.Controls.ColorListBox lsb_FilesList;
        private System.Windows.Forms.Button btn_OpenConflictChecker;
        private System.Windows.Forms.Button btn_ViewFreeSpace;
        private System.Windows.Forms.TextBox txt_Messages;
        private System.Windows.Forms.Button btn_UncheckAll;
        private System.Windows.Forms.Button btn_Sort;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Label lbl_Search;
        private System.Windows.Forms.ComboBox cmb_Variable_Preset;
        private System.Windows.Forms.Label lbl_Mode;
        private System.Windows.Forms.ComboBox cmb_Mode;
    }
}

