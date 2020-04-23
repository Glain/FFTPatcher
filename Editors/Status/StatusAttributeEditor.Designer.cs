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
    partial class StatusAttributeEditor
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
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	System.Windows.Forms.Label blank1Label;
        	System.Windows.Forms.Label blank2Label;
        	System.Windows.Forms.Label orderLabel;
        	System.Windows.Forms.Label ctLabel;
        	System.Windows.Forms.Label hLabel1;
        	System.Windows.Forms.Label label1;
        	System.Windows.Forms.Label label2;
        	this.unknown1Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.unknown2Spinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.orderSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.ctSpinner = new FFTPatcher.Controls.NumericUpDownWithDefault();
        	this.cancelStatusesEditor = new FFTPatcher.Editors.StatusesEditor();
        	this.cantStackStatusesEditor = new FFTPatcher.Editors.StatusesEditor();
        	this.checkedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
        	blank1Label = new System.Windows.Forms.Label();
        	blank2Label = new System.Windows.Forms.Label();
        	orderLabel = new System.Windows.Forms.Label();
        	ctLabel = new System.Windows.Forms.Label();
        	hLabel1 = new System.Windows.Forms.Label();
        	label1 = new System.Windows.Forms.Label();
        	label2 = new System.Windows.Forms.Label();
        	((System.ComponentModel.ISupportInitialize)(this.unknown1Spinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown2Spinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.orderSpinner)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.ctSpinner)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// blank1Label
        	// 
        	blank1Label.AutoSize = true;
        	blank1Label.Location = new System.Drawing.Point(16, 11);
        	blank1Label.Name = "blank1Label";
        	blank1Label.Size = new System.Drawing.Size(56, 13);
        	blank1Label.TabIndex = 6;
        	blank1Label.Text = "Unknown:";
        	// 
        	// blank2Label
        	// 
        	blank2Label.AutoSize = true;
        	blank2Label.Location = new System.Drawing.Point(16, 37);
        	blank2Label.Name = "blank2Label";
        	blank2Label.Size = new System.Drawing.Size(56, 13);
        	blank2Label.TabIndex = 7;
        	blank2Label.Text = "Unknown:";
        	// 
        	// orderLabel
        	// 
        	orderLabel.AutoSize = true;
        	orderLabel.Location = new System.Drawing.Point(16, 63);
        	orderLabel.Name = "orderLabel";
        	orderLabel.Size = new System.Drawing.Size(36, 13);
        	orderLabel.TabIndex = 8;
        	orderLabel.Text = "Order:";
        	// 
        	// ctLabel
        	// 
        	ctLabel.AutoSize = true;
        	ctLabel.Location = new System.Drawing.Point(16, 89);
        	ctLabel.Name = "ctLabel";
        	ctLabel.Size = new System.Drawing.Size(24, 13);
        	ctLabel.TabIndex = 9;
        	ctLabel.Text = "CT:";
        	// 
        	// hLabel1
        	// 
        	hLabel1.AutoSize = true;
        	hLabel1.Location = new System.Drawing.Point(127, 11);
        	hLabel1.Name = "hLabel1";
        	hLabel1.Size = new System.Drawing.Size(13, 13);
        	hLabel1.TabIndex = 43;
        	hLabel1.Text = "h";
        	// 
        	// label1
        	// 
        	label1.AutoSize = true;
        	label1.Location = new System.Drawing.Point(127, 37);
        	label1.Name = "label1";
        	label1.Size = new System.Drawing.Size(13, 13);
        	label1.TabIndex = 44;
        	label1.Text = "h";
        	// 
        	// label2
        	// 
        	label2.AutoSize = true;
        	label2.Location = new System.Drawing.Point(127, 63);
        	label2.Name = "label2";
        	label2.Size = new System.Drawing.Size(13, 13);
        	label2.TabIndex = 45;
        	label2.Text = "h";
        	// 
        	// unknown1Spinner
        	// 
        	this.unknown1Spinner.Hexadecimal = true;
        	this.unknown1Spinner.Location = new System.Drawing.Point(78, 9);
        	this.unknown1Spinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.unknown1Spinner.Name = "unknown1Spinner";
        	this.unknown1Spinner.Size = new System.Drawing.Size(48, 20);
        	this.unknown1Spinner.TabIndex = 0;
        	this.unknown1Spinner.Tag = "Blank1";
        	this.unknown1Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// unknown2Spinner
        	// 
        	this.unknown2Spinner.Hexadecimal = true;
        	this.unknown2Spinner.Location = new System.Drawing.Point(78, 35);
        	this.unknown2Spinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.unknown2Spinner.Name = "unknown2Spinner";
        	this.unknown2Spinner.Size = new System.Drawing.Size(48, 20);
        	this.unknown2Spinner.TabIndex = 1;
        	this.unknown2Spinner.Tag = "Blank2";
        	this.unknown2Spinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// orderSpinner
        	// 
        	this.orderSpinner.Hexadecimal = true;
        	this.orderSpinner.Location = new System.Drawing.Point(78, 61);
        	this.orderSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.orderSpinner.Name = "orderSpinner";
        	this.orderSpinner.Size = new System.Drawing.Size(48, 20);
        	this.orderSpinner.TabIndex = 2;
        	this.orderSpinner.Tag = "Order";
        	this.orderSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// ctSpinner
        	// 
        	this.ctSpinner.Location = new System.Drawing.Point(78, 87);
        	this.ctSpinner.Maximum = new decimal(new int[] {
        	        	        	255,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.ctSpinner.Name = "ctSpinner";
        	this.ctSpinner.Size = new System.Drawing.Size(48, 20);
        	this.ctSpinner.TabIndex = 3;
        	this.ctSpinner.Tag = "CT";
        	this.ctSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        	// 
        	// cancelStatusesEditor
        	// 
        	this.cancelStatusesEditor.Location = new System.Drawing.Point(3, 139);
        	this.cancelStatusesEditor.Name = "cancelStatusesEditor";
        	this.cancelStatusesEditor.Size = new System.Drawing.Size(560, 179);
        	this.cancelStatusesEditor.Status = "Cancels";
        	this.cancelStatusesEditor.Statuses = null;
        	this.cancelStatusesEditor.TabIndex = 5;
        	this.cancelStatusesEditor.TabStop = false;
        	// 
        	// cantStackStatusesEditor
        	// 
        	this.cantStackStatusesEditor.Location = new System.Drawing.Point(3, 320);
        	this.cantStackStatusesEditor.Name = "cantStackStatusesEditor";
        	this.cantStackStatusesEditor.Size = new System.Drawing.Size(560, 179);
        	this.cantStackStatusesEditor.Status = "Can\'t Stack On Top Of";
        	this.cantStackStatusesEditor.Statuses = null;
        	this.cantStackStatusesEditor.TabIndex = 6;
        	this.cantStackStatusesEditor.TabStop = false;
        	// 
        	// checkedListBox
        	// 
        	this.checkedListBox.FormattingEnabled = true;
        	this.checkedListBox.Items.AddRange(new object[] {
        	        	        	"Freeze CT",
        	        	        	"",
        	        	        	"",
        	        	        	"",
        	        	        	"Cancel when Hit",
        	        	        	"",
        	        	        	"",
        	        	        	"Counts as KO",
        	        	        	"Can React",
        	        	        	"Blank",
        	        	        	"Ignore Attacks",
        	        	        	"Ignored if Mount",
        	        	        	"",
        	        	        	"",
        	        	        	"Immortal Cancels",
        	        	        	"Lower Target Priority?"});
        	this.checkedListBox.Location = new System.Drawing.Point(232, 9);
        	this.checkedListBox.MultiColumn = true;
        	this.checkedListBox.Name = "checkedListBox";
        	this.checkedListBox.Size = new System.Drawing.Size(331, 124);
        	this.checkedListBox.TabIndex = 4;
        	this.checkedListBox.TabStop = false;
        	// 
        	// StatusAttributeEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoSize = true;
        	this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.Controls.Add(label2);
        	this.Controls.Add(label1);
        	this.Controls.Add(hLabel1);
        	this.Controls.Add(this.checkedListBox);
        	this.Controls.Add(ctLabel);
        	this.Controls.Add(orderLabel);
        	this.Controls.Add(blank2Label);
        	this.Controls.Add(blank1Label);
        	this.Controls.Add(this.cantStackStatusesEditor);
        	this.Controls.Add(this.cancelStatusesEditor);
        	this.Controls.Add(this.ctSpinner);
        	this.Controls.Add(this.orderSpinner);
        	this.Controls.Add(this.unknown2Spinner);
        	this.Controls.Add(this.unknown1Spinner);
        	this.Name = "StatusAttributeEditor";
        	this.Size = new System.Drawing.Size(566, 502);
        	((System.ComponentModel.ISupportInitialize)(this.unknown1Spinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.unknown2Spinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.orderSpinner)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.ctSpinner)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private FFTPatcher.Controls.NumericUpDownWithDefault unknown1Spinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault unknown2Spinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault orderSpinner;
        private FFTPatcher.Controls.NumericUpDownWithDefault ctSpinner;
        private StatusesEditor cancelStatusesEditor;
        private StatusesEditor cantStackStatusesEditor;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault checkedListBox;
    }
}
