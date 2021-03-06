﻿namespace FFTorgASM
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
            this.btnPatchSaveState = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lsb_FilesList = new PatcherLib.Controls.ColorListBox();
            this.btn_OpenConflictChecker = new System.Windows.Forms.Button();
            this.btn_ViewFreeSpace = new System.Windows.Forms.Button();
            this.txt_Messages = new System.Windows.Forms.TextBox();
            this.clb_Patches = new PatcherLib.Controls.ModifiedBGCheckedListBox();
            this.btn_UncheckAll = new System.Windows.Forms.Button();
            this.btn_SavePatchXML = new System.Windows.Forms.Button();
            this.variableSpinner = new FFTorgASM.HexNumericUpDown();
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
            this.btnPatch.Text = "Patch ISO...";
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
            this.variableComboBox.Size = new System.Drawing.Size(161, 21);
            this.variableComboBox.TabIndex = 6;
            this.variableComboBox.Visible = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(520, 602);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(32, 13);
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
            // btnPatchSaveState
            // 
            this.btnPatchSaveState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatchSaveState.Enabled = false;
            this.btnPatchSaveState.Location = new System.Drawing.Point(332, 628);
            this.btnPatchSaveState.Name = "btnPatchSaveState";
            this.btnPatchSaveState.Size = new System.Drawing.Size(142, 23);
            this.btnPatchSaveState.TabIndex = 10;
            this.btnPatchSaveState.Text = "Patch to pSX Savestate";
            this.btnPatchSaveState.UseVisualStyleBackColor = true;
            this.btnPatchSaveState.Click += new System.EventHandler(this.btnPatchSaveState_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(296, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Only visible patches in this list will be applied";
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
            // btn_SavePatchXML
            // 
            this.btn_SavePatchXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SavePatchXML.Location = new System.Drawing.Point(144, 657);
            this.btn_SavePatchXML.Name = "btn_SavePatchXML";
            this.btn_SavePatchXML.Size = new System.Drawing.Size(104, 23);
            this.btn_SavePatchXML.TabIndex = 17;
            this.btn_SavePatchXML.Text = "Save Patch .XML";
            this.btn_SavePatchXML.UseVisualStyleBackColor = true;
            this.btn_SavePatchXML.Click += new System.EventHandler(this.btn_SavePatchXML_Click);
            // 
            // variableSpinner
            // 
            this.variableSpinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.variableSpinner.Hexadecimal = true;
            this.variableSpinner.Location = new System.Drawing.Point(179, 602);
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
            this.ClientSize = new System.Drawing.Size(567, 705);
            this.Controls.Add(this.btn_SavePatchXML);
            this.Controls.Add(this.btn_UncheckAll);
            this.Controls.Add(this.txt_Messages);
            this.Controls.Add(this.btn_ViewFreeSpace);
            this.Controls.Add(this.lsb_FilesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableSpinner);
            this.Controls.Add(this.btn_OpenConflictChecker);
            this.Controls.Add(this.btnPatchSaveState);
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
        private System.Windows.Forms.Button btnPatchSaveState;
        private System.Windows.Forms.Label label1;
        private PatcherLib.Controls.ColorListBox lsb_FilesList;
        private System.Windows.Forms.Button btn_OpenConflictChecker;
        private System.Windows.Forms.Button btn_ViewFreeSpace;
        private System.Windows.Forms.TextBox txt_Messages;
        private System.Windows.Forms.Button btn_UncheckAll;
        private System.Windows.Forms.Button btn_SavePatchXML;
    }
}

