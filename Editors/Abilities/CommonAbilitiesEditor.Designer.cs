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

namespace FFTPatcher.Editors
{
    partial class CommonAbilitiesEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	System.Windows.Forms.Label jpCostLabel;
        	System.Windows.Forms.Label learnChanceLabel;
        	System.Windows.Forms.Label percentLabel;
        	System.Windows.Forms.GroupBox aiGroupBox;
        	this.aiCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	this.jpCostSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.chanceSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.propertiesCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	this.abilityTypeComboBox = new FFTPatcher.Controls.ComboBoxWithDefault();
        	jpCostLabel = new System.Windows.Forms.Label();
        	learnChanceLabel = new System.Windows.Forms.Label();
        	percentLabel = new System.Windows.Forms.Label();
        	aiGroupBox = new System.Windows.Forms.GroupBox();
        	aiGroupBox.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.jpCostSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.chanceSpinner)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// jpCostLabel
        	// 
        	jpCostLabel.AutoSize = true;
        	jpCostLabel.Location = new System.Drawing.Point(3, 7);
        	jpCostLabel.Name = "jpCostLabel";
        	jpCostLabel.Size = new System.Drawing.Size(46, 13);
        	jpCostLabel.TabIndex = 0;
        	jpCostLabel.Text = "JP Cost:";
        	// 
        	// learnChanceLabel
        	// 
        	learnChanceLabel.AutoSize = true;
        	learnChanceLabel.Location = new System.Drawing.Point(3, 31);
        	learnChanceLabel.Name = "learnChanceLabel";
        	learnChanceLabel.Size = new System.Drawing.Size(88, 13);
        	learnChanceLabel.TabIndex = 2;
        	learnChanceLabel.Text = "Chance to learn: ";
        	// 
        	// percentLabel
        	// 
        	percentLabel.AutoSize = true;
        	percentLabel.Location = new System.Drawing.Point(148, 31);
        	percentLabel.Name = "percentLabel";
        	percentLabel.Size = new System.Drawing.Size(15, 13);
        	percentLabel.TabIndex = 10;
        	percentLabel.Text = "%";
        	// 
        	// aiGroupBox
        	// 
        	aiGroupBox.AutoSize = true;
        	aiGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	aiGroupBox.Controls.Add(this.aiCheckedListBox);
        	aiGroupBox.Location = new System.Drawing.Point(169, 3);
        	aiGroupBox.Name = "aiGroupBox";
        	aiGroupBox.Size = new System.Drawing.Size(386, 162);
        	aiGroupBox.TabIndex = 4;
        	aiGroupBox.TabStop = false;
        	aiGroupBox.Text = "AI Behavior";
        	// 
        	// aiCheckedListBox
        	// 
        	this.aiCheckedListBox.FormattingEnabled = true;
        	this.aiCheckedListBox.Location = new System.Drawing.Point(6, 19);
        	this.aiCheckedListBox.MultiColumn = true;
        	this.aiCheckedListBox.Name = "aiCheckedListBox";
        	this.aiCheckedListBox.Size = new System.Drawing.Size(374, 124);
        	this.aiCheckedListBox.TabIndex = 0;
        	this.aiCheckedListBox.TabStop = false;
        	// 
        	// jpCostSpinner
        	// 
        	this.jpCostSpinner.AutoSize = true;
        	this.jpCostSpinner.Location = new System.Drawing.Point(95, 5);
        	this.jpCostSpinner.Maximum = new decimal(new int[] {
        	        	        	65535,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.jpCostSpinner.MinimumSize = new System.Drawing.Size(51, 0);
        	this.jpCostSpinner.Name = "jpCostSpinner";
        	this.jpCostSpinner.Size = new System.Drawing.Size(53, 20);
        	this.jpCostSpinner.TabIndex = 0;
        	this.jpCostSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// chanceSpinner
        	// 
        	this.chanceSpinner.Location = new System.Drawing.Point(95, 28);
        	this.chanceSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.chanceSpinner.MinimumSize = new System.Drawing.Size(51, 0);
        	this.chanceSpinner.Name = "chanceSpinner";
        	this.chanceSpinner.Size = new System.Drawing.Size(53, 20);
        	this.chanceSpinner.TabIndex = 1;
        	this.chanceSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// propertiesCheckedListBox
        	// 
        	this.propertiesCheckedListBox.FormattingEnabled = true;
        	this.propertiesCheckedListBox.Location = new System.Drawing.Point(6, 81);
        	this.propertiesCheckedListBox.Name = "propertiesCheckedListBox";
        	this.propertiesCheckedListBox.Size = new System.Drawing.Size(142, 184);
        	this.propertiesCheckedListBox.TabIndex = 3;
        	this.propertiesCheckedListBox.TabStop = false;
        	// 
        	// abilityTypeComboBox
        	// 
        	this.abilityTypeComboBox.BackColor = System.Drawing.SystemColors.Window;
        	this.abilityTypeComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
        	this.abilityTypeComboBox.FormattingEnabled = true;
        	this.abilityTypeComboBox.Location = new System.Drawing.Point(6, 54);
        	this.abilityTypeComboBox.Name = "abilityTypeComboBox";
        	this.abilityTypeComboBox.Size = new System.Drawing.Size(142, 21);
        	this.abilityTypeComboBox.TabIndex = 2;
        	// 
        	// CommonAbilitiesEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoSize = true;
        	this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.Controls.Add(aiGroupBox);
        	this.Controls.Add(percentLabel);
        	this.Controls.Add(this.abilityTypeComboBox);
        	this.Controls.Add(this.propertiesCheckedListBox);
        	this.Controls.Add(this.chanceSpinner);
        	this.Controls.Add(learnChanceLabel);
        	this.Controls.Add(this.jpCostSpinner);
        	this.Controls.Add(jpCostLabel);
        	this.Name = "CommonAbilitiesEditor";
        	this.Size = new System.Drawing.Size(558, 268);
        	aiGroupBox.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.jpCostSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.chanceSpinner)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault aiCheckedListBox;
        private FFTPatcher.Controls.ComboBoxWithDefault abilityTypeComboBox;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault propertiesCheckedListBox;
        private FFTPatcher.Controls.NumericUpDownWithDefault chanceSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault jpCostSpinner;
    }
}
