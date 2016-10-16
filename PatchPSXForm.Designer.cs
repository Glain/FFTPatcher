/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace FFTPatcher
{
    partial class PatchPSXForm
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
            System.Windows.Forms.GroupBox sceapGroupBox;
            System.Windows.Forms.GroupBox scusGroupBox;
            System.Windows.Forms.GroupBox battleBinGroupBox;
            System.Windows.Forms.Button cancelButton;
            System.Windows.Forms.Button isoBrowseButton;
            System.Windows.Forms.Label isoLabel;
            System.Windows.Forms.GroupBox worldBinGroupBox;
            System.Windows.Forms.GroupBox wldCoreGroupBox;
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sceapBrowseButton = new System.Windows.Forms.Button();
            this.sceapFileNameTextBox = new System.Windows.Forms.TextBox();
            this.useCustomSceapRadioButton = new System.Windows.Forms.RadioButton();
            this.useDefaultSceapRadioButton = new System.Windows.Forms.RadioButton();
            this.dontChangeSceapRadioButton = new System.Windows.Forms.RadioButton();
            this.scusCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.battleCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.storeInventoryCheckBox = new System.Windows.Forms.CheckBox();
            this.propositionsCheckBox = new System.Windows.Forms.CheckBox();
            this.sceapOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.entd1CheckBox = new System.Windows.Forms.CheckBox();
            this.entd2CheckBox = new System.Windows.Forms.CheckBox();
            this.entd3CheckBox = new System.Windows.Forms.CheckBox();
            this.entd4CheckBox = new System.Windows.Forms.CheckBox();
            this.eccCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.patchIsoDialog = new System.Windows.Forms.SaveFileDialog();
            this.isoPathTextBox = new System.Windows.Forms.TextBox();
            sceapGroupBox = new System.Windows.Forms.GroupBox();
            scusGroupBox = new System.Windows.Forms.GroupBox();
            battleBinGroupBox = new System.Windows.Forms.GroupBox();
            cancelButton = new System.Windows.Forms.Button();
            isoBrowseButton = new System.Windows.Forms.Button();
            isoLabel = new System.Windows.Forms.Label();
            worldBinGroupBox = new System.Windows.Forms.GroupBox();
            wldCoreGroupBox = new System.Windows.Forms.GroupBox();
            sceapGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            scusGroupBox.SuspendLayout();
            battleBinGroupBox.SuspendLayout();
            worldBinGroupBox.SuspendLayout();
            wldCoreGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sceapGroupBox
            // 
            sceapGroupBox.AutoSize = true;
            sceapGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            sceapGroupBox.Controls.Add(this.pictureBox1);
            sceapGroupBox.Controls.Add(this.sceapBrowseButton);
            sceapGroupBox.Controls.Add(this.sceapFileNameTextBox);
            sceapGroupBox.Controls.Add(this.useCustomSceapRadioButton);
            sceapGroupBox.Controls.Add(this.useDefaultSceapRadioButton);
            sceapGroupBox.Controls.Add(this.dontChangeSceapRadioButton);
            sceapGroupBox.Location = new System.Drawing.Point(172, 134);
            sceapGroupBox.Name = "sceapGroupBox";
            sceapGroupBox.Size = new System.Drawing.Size(333, 171);
            sceapGroupBox.TabIndex = 3;
            sceapGroupBox.TabStop = false;
            sceapGroupBox.Text = "SCEAP.DAT";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(7, 120);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // sceapBrowseButton
            // 
            this.sceapBrowseButton.AutoSize = true;
            this.sceapBrowseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sceapBrowseButton.Enabled = false;
            this.sceapBrowseButton.Location = new System.Drawing.Point(179, 91);
            this.sceapBrowseButton.Name = "sceapBrowseButton";
            this.sceapBrowseButton.Size = new System.Drawing.Size(26, 23);
            this.sceapBrowseButton.TabIndex = 4;
            this.sceapBrowseButton.Text = "...";
            this.sceapBrowseButton.UseVisualStyleBackColor = true;
            this.sceapBrowseButton.Click += new System.EventHandler(this.sceapBrowseButton_Click);
            // 
            // sceapFileNameTextBox
            // 
            this.sceapFileNameTextBox.Location = new System.Drawing.Point(24, 92);
            this.sceapFileNameTextBox.Name = "sceapFileNameTextBox";
            this.sceapFileNameTextBox.ReadOnly = true;
            this.sceapFileNameTextBox.Size = new System.Drawing.Size(149, 20);
            this.sceapFileNameTextBox.TabIndex = 3;
            // 
            // useCustomSceapRadioButton
            // 
            this.useCustomSceapRadioButton.AutoSize = true;
            this.useCustomSceapRadioButton.Location = new System.Drawing.Point(7, 68);
            this.useCustomSceapRadioButton.Name = "useCustomSceapRadioButton";
            this.useCustomSceapRadioButton.Size = new System.Drawing.Size(144, 17);
            this.useCustomSceapRadioButton.TabIndex = 2;
            this.useCustomSceapRadioButton.Text = "Use custom SCEAP.DAT";
            this.useCustomSceapRadioButton.UseVisualStyleBackColor = true;
            this.useCustomSceapRadioButton.CheckedChanged += new System.EventHandler(this.sceapRadioButton_CheckedChanged);
            // 
            // useDefaultSceapRadioButton
            // 
            this.useDefaultSceapRadioButton.AutoSize = true;
            this.useDefaultSceapRadioButton.Location = new System.Drawing.Point(7, 44);
            this.useDefaultSceapRadioButton.Name = "useDefaultSceapRadioButton";
            this.useDefaultSceapRadioButton.Size = new System.Drawing.Size(166, 17);
            this.useDefaultSceapRadioButton.TabIndex = 1;
            this.useDefaultSceapRadioButton.Text = "Use FFTPatcher SCEAP.DAT";
            this.useDefaultSceapRadioButton.UseVisualStyleBackColor = true;
            this.useDefaultSceapRadioButton.CheckedChanged += new System.EventHandler(this.sceapRadioButton_CheckedChanged);
            // 
            // dontChangeSceapRadioButton
            // 
            this.dontChangeSceapRadioButton.AutoSize = true;
            this.dontChangeSceapRadioButton.Checked = true;
            this.dontChangeSceapRadioButton.Location = new System.Drawing.Point(7, 20);
            this.dontChangeSceapRadioButton.Name = "dontChangeSceapRadioButton";
            this.dontChangeSceapRadioButton.Size = new System.Drawing.Size(152, 17);
            this.dontChangeSceapRadioButton.TabIndex = 0;
            this.dontChangeSceapRadioButton.TabStop = true;
            this.dontChangeSceapRadioButton.Text = "Don\'t change SCEAP.DAT";
            this.dontChangeSceapRadioButton.UseVisualStyleBackColor = true;
            this.dontChangeSceapRadioButton.CheckedChanged += new System.EventHandler(this.sceapRadioButton_CheckedChanged);
            // 
            // scusGroupBox
            // 
            scusGroupBox.AutoSize = true;
            scusGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            scusGroupBox.Controls.Add(this.scusCheckedListBox);
            scusGroupBox.Location = new System.Drawing.Point(12, 12);
            scusGroupBox.Name = "scusGroupBox";
            scusGroupBox.Size = new System.Drawing.Size(154, 207);
            scusGroupBox.TabIndex = 0;
            scusGroupBox.TabStop = false;
            scusGroupBox.Text = "SCUS_942.21";
            // 
            // scusCheckedListBox
            // 
            this.scusCheckedListBox.FormattingEnabled = true;
            this.scusCheckedListBox.Items.AddRange(new object[] {
            "Abilities",
            "Items",
            "Item Attributes",
            "Jobs",
            "Job Levels",
            "Skillsets",
            "Monster Skills",
            "Action Menus",
            "Status Attributes",
            "Inflict Status",
            "Poach Probabilities"});
            this.scusCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.scusCheckedListBox.Name = "scusCheckedListBox";
            this.scusCheckedListBox.Size = new System.Drawing.Size(142, 169);
            this.scusCheckedListBox.TabIndex = 0;
            this.scusCheckedListBox.Tag = "SCUS_942.21";
            this.scusCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_Patches_ItemCheck);
            // 
            // battleBinGroupBox
            // 
            battleBinGroupBox.AutoSize = true;
            battleBinGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            battleBinGroupBox.Controls.Add(this.battleCheckedListBox);
            battleBinGroupBox.Location = new System.Drawing.Point(172, 12);
            battleBinGroupBox.Name = "battleBinGroupBox";
            battleBinGroupBox.Size = new System.Drawing.Size(132, 87);
            battleBinGroupBox.TabIndex = 1;
            battleBinGroupBox.TabStop = false;
            battleBinGroupBox.Text = "BATTLE.BIN";
            // 
            // battleCheckedListBox
            // 
            this.battleCheckedListBox.FormattingEnabled = true;
            this.battleCheckedListBox.Items.AddRange(new object[] {
            "Ability Effects",
            "Ability Animations",
            "Move-Find Items"});
            this.battleCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.battleCheckedListBox.Name = "battleCheckedListBox";
            this.battleCheckedListBox.Size = new System.Drawing.Size(120, 49);
            this.battleCheckedListBox.TabIndex = 0;
            this.battleCheckedListBox.Tag = "BATTLE.BIN";
            this.battleCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clb_Patches_ItemCheck);
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(431, 361);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 13;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // isoBrowseButton
            // 
            isoBrowseButton.AutoSize = true;
            isoBrowseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            isoBrowseButton.Location = new System.Drawing.Point(291, 361);
            isoBrowseButton.Name = "isoBrowseButton";
            isoBrowseButton.Size = new System.Drawing.Size(26, 23);
            isoBrowseButton.TabIndex = 11;
            isoBrowseButton.Text = "...";
            isoBrowseButton.UseVisualStyleBackColor = true;
            isoBrowseButton.Click += new System.EventHandler(this.isoBrowseButton_Click);
            // 
            // isoLabel
            // 
            isoLabel.AutoSize = true;
            isoLabel.Location = new System.Drawing.Point(12, 347);
            isoLabel.Name = "isoLabel";
            isoLabel.Size = new System.Drawing.Size(25, 13);
            isoLabel.TabIndex = 13;
            isoLabel.Text = "ISO";
            // 
            // worldBinGroupBox
            // 
            worldBinGroupBox.AutoSize = true;
            worldBinGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            worldBinGroupBox.Controls.Add(this.storeInventoryCheckBox);
            worldBinGroupBox.Location = new System.Drawing.Point(310, 12);
            worldBinGroupBox.Name = "worldBinGroupBox";
            worldBinGroupBox.Size = new System.Drawing.Size(110, 55);
            worldBinGroupBox.TabIndex = 14;
            worldBinGroupBox.TabStop = false;
            worldBinGroupBox.Text = "WORLD.BIN";
            // 
            // storeInventoryCheckBox
            // 
            this.storeInventoryCheckBox.AutoSize = true;
            this.storeInventoryCheckBox.Location = new System.Drawing.Point(6, 19);
            this.storeInventoryCheckBox.Name = "storeInventoryCheckBox";
            this.storeInventoryCheckBox.Size = new System.Drawing.Size(98, 17);
            this.storeInventoryCheckBox.TabIndex = 0;
            this.storeInventoryCheckBox.Tag = "StoreInventory";
            this.storeInventoryCheckBox.Text = "Store Inventory";
            this.storeInventoryCheckBox.UseVisualStyleBackColor = true;
            this.storeInventoryCheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // wldCoreGroupBox
            // 
            wldCoreGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            wldCoreGroupBox.Controls.Add(this.propositionsCheckBox);
            wldCoreGroupBox.Location = new System.Drawing.Point(310, 73);
            wldCoreGroupBox.Name = "wldCoreGroupBox";
            wldCoreGroupBox.Size = new System.Drawing.Size(110, 55);
            wldCoreGroupBox.TabIndex = 15;
            wldCoreGroupBox.TabStop = false;
            wldCoreGroupBox.Text = "WLDCORE.BIN";
            // 
            // propositionsCheckBox
            // 
            this.propositionsCheckBox.AutoSize = true;
            this.propositionsCheckBox.Location = new System.Drawing.Point(6, 19);
            this.propositionsCheckBox.Name = "propositionsCheckBox";
            this.propositionsCheckBox.Size = new System.Drawing.Size(83, 17);
            this.propositionsCheckBox.TabIndex = 0;
            this.propositionsCheckBox.Tag = "Propositions";
            this.propositionsCheckBox.Text = "Propositions";
            this.propositionsCheckBox.UseVisualStyleBackColor = true;
            this.propositionsCheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // sceapOpenFileDialog
            // 
            this.sceapOpenFileDialog.FileName = "SCEAP.DAT";
            this.sceapOpenFileDialog.Filter = "SCEAP.DAT|SCEAP.DAT";
            this.sceapOpenFileDialog.ShowReadOnly = true;
            // 
            // entd1CheckBox
            // 
            this.entd1CheckBox.AutoSize = true;
            this.entd1CheckBox.Location = new System.Drawing.Point(12, 253);
            this.entd1CheckBox.Name = "entd1CheckBox";
            this.entd1CheckBox.Size = new System.Drawing.Size(87, 17);
            this.entd1CheckBox.TabIndex = 4;
            this.entd1CheckBox.Tag = "ENTD1";
            this.entd1CheckBox.Text = "ENTD1.ENT";
            this.entd1CheckBox.UseVisualStyleBackColor = true;
            this.entd1CheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // entd2CheckBox
            // 
            this.entd2CheckBox.AutoSize = true;
            this.entd2CheckBox.Location = new System.Drawing.Point(12, 276);
            this.entd2CheckBox.Name = "entd2CheckBox";
            this.entd2CheckBox.Size = new System.Drawing.Size(87, 17);
            this.entd2CheckBox.TabIndex = 5;
            this.entd2CheckBox.Tag = "ENTD2";
            this.entd2CheckBox.Text = "ENTD2.ENT";
            this.entd2CheckBox.UseVisualStyleBackColor = true;
            this.entd2CheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // entd3CheckBox
            // 
            this.entd3CheckBox.AutoSize = true;
            this.entd3CheckBox.Location = new System.Drawing.Point(12, 299);
            this.entd3CheckBox.Name = "entd3CheckBox";
            this.entd3CheckBox.Size = new System.Drawing.Size(87, 17);
            this.entd3CheckBox.TabIndex = 6;
            this.entd3CheckBox.Tag = "ENTD3";
            this.entd3CheckBox.Text = "ENTD3.ENT";
            this.entd3CheckBox.UseVisualStyleBackColor = true;
            this.entd3CheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // entd4CheckBox
            // 
            this.entd4CheckBox.AutoSize = true;
            this.entd4CheckBox.Location = new System.Drawing.Point(12, 322);
            this.entd4CheckBox.Name = "entd4CheckBox";
            this.entd4CheckBox.Size = new System.Drawing.Size(87, 17);
            this.entd4CheckBox.TabIndex = 7;
            this.entd4CheckBox.Tag = "ENTD4";
            this.entd4CheckBox.Text = "ENTD4.ENT";
            this.entd4CheckBox.UseVisualStyleBackColor = true;
            this.entd4CheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // eccCheckBox
            // 
            this.eccCheckBox.AutoSize = true;
            this.eccCheckBox.Checked = true;
            this.eccCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.eccCheckBox.Location = new System.Drawing.Point(172, 315);
            this.eccCheckBox.Name = "eccCheckBox";
            this.eccCheckBox.Size = new System.Drawing.Size(103, 17);
            this.eccCheckBox.TabIndex = 8;
            this.eccCheckBox.Tag = "RegenECC";
            this.eccCheckBox.Text = "Regen ISO ECC";
            this.eccCheckBox.UseVisualStyleBackColor = true;
            this.eccCheckBox.CheckedChanged += new System.EventHandler(this.entd2CheckBox_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(349, 361);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // patchIsoDialog
            // 
            this.patchIsoDialog.CheckFileExists = true;
            this.patchIsoDialog.Filter = "ISO images (*.iso,*.psv, *.bin, *.img)|*.iso;*.bin;*.img|All files|*.*";
            this.patchIsoDialog.OverwritePrompt = false;
            // 
            // isoPathTextBox
            // 
            this.isoPathTextBox.Location = new System.Drawing.Point(12, 363);
            this.isoPathTextBox.Name = "isoPathTextBox";
            this.isoPathTextBox.ReadOnly = true;
            this.isoPathTextBox.Size = new System.Drawing.Size(273, 20);
            this.isoPathTextBox.TabIndex = 10;
            // 
            // PatchPSXForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelButton;
            this.ClientSize = new System.Drawing.Size(511, 396);
            this.ControlBox = false;
            this.Controls.Add(wldCoreGroupBox);
            this.Controls.Add(worldBinGroupBox);
            this.Controls.Add(isoLabel);
            this.Controls.Add(isoBrowseButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.isoPathTextBox);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.eccCheckBox);
            this.Controls.Add(this.entd4CheckBox);
            this.Controls.Add(this.entd3CheckBox);
            this.Controls.Add(this.entd2CheckBox);
            this.Controls.Add(this.entd1CheckBox);
            this.Controls.Add(battleBinGroupBox);
            this.Controls.Add(scusGroupBox);
            this.Controls.Add(sceapGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PatchPSXForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Patch PSX ISO";
            sceapGroupBox.ResumeLayout(false);
            sceapGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            scusGroupBox.ResumeLayout(false);
            battleBinGroupBox.ResumeLayout(false);
            worldBinGroupBox.ResumeLayout(false);
            worldBinGroupBox.PerformLayout();
            wldCoreGroupBox.ResumeLayout(false);
            wldCoreGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton useCustomSceapRadioButton;
        private System.Windows.Forms.Button sceapBrowseButton;
        private System.Windows.Forms.TextBox sceapFileNameTextBox;
        private System.Windows.Forms.OpenFileDialog sceapOpenFileDialog;
        private System.Windows.Forms.CheckedListBox scusCheckedListBox;
        private System.Windows.Forms.CheckedListBox battleCheckedListBox;
        private System.Windows.Forms.CheckBox entd1CheckBox;
        private System.Windows.Forms.CheckBox entd2CheckBox;
        private System.Windows.Forms.CheckBox entd3CheckBox;
        private System.Windows.Forms.CheckBox entd4CheckBox;
        private System.Windows.Forms.CheckBox eccCheckBox;
        private System.Windows.Forms.RadioButton useDefaultSceapRadioButton;
        private System.Windows.Forms.RadioButton dontChangeSceapRadioButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.SaveFileDialog patchIsoDialog;
        private System.Windows.Forms.TextBox isoPathTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox storeInventoryCheckBox;
        private System.Windows.Forms.CheckBox propositionsCheckBox;

    }
}