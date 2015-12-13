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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.patchButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.reloadButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.variableComboBox = new System.Windows.Forms.ComboBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.toggleButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.PatchSaveStbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lsb_FilesList = new System.Windows.Forms.ListView();
            this.btn_OpenConflictChecker = new System.Windows.Forms.Button();
            this.variableSpinner = new FFTorgASM.HexNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.variableSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.AllowDrop = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(159, 30);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(294, 319);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragDrop);
            this.checkedListBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragEnter);
            // 
            // patchButton
            // 
            this.patchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.patchButton.Location = new System.Drawing.Point(378, 528);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new System.Drawing.Size(75, 23);
            this.patchButton.TabIndex = 1;
            this.patchButton.Text = "Patch ISO...";
            this.patchButton.UseVisualStyleBackColor = true;
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
            this.textBox1.Size = new System.Drawing.Size(441, 109);
            this.textBox1.TabIndex = 3;
            // 
            // variableComboBox
            // 
            this.variableComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableComboBox.FormattingEnabled = true;
            this.variableComboBox.Location = new System.Drawing.Point(12, 501);
            this.variableComboBox.Name = "variableComboBox";
            this.variableComboBox.Size = new System.Drawing.Size(161, 21);
            this.variableComboBox.TabIndex = 6;
            this.variableComboBox.Visible = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(418, 502);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(35, 13);
            this.versionLabel.TabIndex = 7;
            this.versionLabel.Text = "label1";
            // 
            // toggleButton
            // 
            this.toggleButton.Location = new System.Drawing.Point(378, 355);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(75, 23);
            this.toggleButton.TabIndex = 8;
            this.toggleButton.Text = "Toggle all";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Location = new System.Drawing.Point(297, 355);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(75, 23);
            this.checkAllButton.TabIndex = 9;
            this.checkAllButton.Text = "Check all";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // PatchSaveStbutton
            // 
            this.PatchSaveStbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PatchSaveStbutton.Enabled = false;
            this.PatchSaveStbutton.Location = new System.Drawing.Point(230, 528);
            this.PatchSaveStbutton.Name = "PatchSaveStbutton";
            this.PatchSaveStbutton.Size = new System.Drawing.Size(142, 23);
            this.PatchSaveStbutton.TabIndex = 10;
            this.PatchSaveStbutton.Text = "Patch to PSX Savestate";
            this.PatchSaveStbutton.UseVisualStyleBackColor = true;
            this.PatchSaveStbutton.Click += new System.EventHandler(this.PatchSaveStbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(194, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Only visible patches in this list will be applied";
            // 
            // lsb_FilesList
            // 
            this.lsb_FilesList.LabelEdit = true;
            this.lsb_FilesList.LabelWrap = false;
            this.lsb_FilesList.Location = new System.Drawing.Point(12, 30);
            this.lsb_FilesList.MultiSelect = false;
            this.lsb_FilesList.Name = "lsb_FilesList";
            this.lsb_FilesList.Size = new System.Drawing.Size(141, 319);
            this.lsb_FilesList.TabIndex = 13;
            this.lsb_FilesList.UseCompatibleStateImageBehavior = false;
            this.lsb_FilesList.View = System.Windows.Forms.View.List;
            this.lsb_FilesList.SelectedIndexChanged += new System.EventHandler(this.lsb_FilesList_SelectedIndexChanged);
            // 
            // btn_OpenConflictChecker
            // 
            this.btn_OpenConflictChecker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OpenConflictChecker.Location = new System.Drawing.Point(311, 557);
            this.btn_OpenConflictChecker.Name = "btn_OpenConflictChecker";
            this.btn_OpenConflictChecker.Size = new System.Drawing.Size(142, 23);
            this.btn_OpenConflictChecker.TabIndex = 10;
            this.btn_OpenConflictChecker.Text = "Open Conflict Checker";
            this.btn_OpenConflictChecker.UseVisualStyleBackColor = true;
            this.btn_OpenConflictChecker.Click += new System.EventHandler(this.btn_OpenConflictChecker_Click);
            // 
            // variableSpinner
            // 
            this.variableSpinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableSpinner.Hexadecimal = true;
            this.variableSpinner.Location = new System.Drawing.Point(179, 502);
            this.variableSpinner.Name = "variableSpinner";
            this.variableSpinner.Size = new System.Drawing.Size(102, 20);
            this.variableSpinner.TabIndex = 5;
            this.variableSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.variableSpinner.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 605);
            this.Controls.Add(this.lsb_FilesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableSpinner);
            this.Controls.Add(this.btn_OpenConflictChecker);
            this.Controls.Add(this.PatchSaveStbutton);
            this.Controls.Add(this.checkAllButton);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.variableComboBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.patchButton);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "MainForm";
            this.Text = "FFTorgASM";
            ((System.ComponentModel.ISupportInitialize)(this.variableSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button patchButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TextBox textBox1;
        private FFTorgASM.HexNumericUpDown variableSpinner;
        private System.Windows.Forms.ComboBox variableComboBox;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button PatchSaveStbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lsb_FilesList;
        private System.Windows.Forms.Button btn_OpenConflictChecker;
    }
}

