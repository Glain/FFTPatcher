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
    partial class InflictStatusEditor
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
            this.spinner_Repoint = new System.Windows.Forms.NumericUpDown();
            this.lblRepoint = new System.Windows.Forms.Label();
            this.btnRepoint = new System.Windows.Forms.Button();
            this.flagsCheckedListBox = new FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault();
            this.inflictStatusesEditor = new FFTPatcher.Editors.StatusesEditor();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Repoint)).BeginInit();
            this.SuspendLayout();
            // 
            // spinner_Repoint
            // 
            this.spinner_Repoint.Hexadecimal = true;
            this.spinner_Repoint.Location = new System.Drawing.Point(146, 195);
            this.spinner_Repoint.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinner_Repoint.Name = "spinner_Repoint";
            this.spinner_Repoint.Size = new System.Drawing.Size(43, 20);
            this.spinner_Repoint.TabIndex = 4;
            this.spinner_Repoint.ValueChanged += new System.EventHandler(this.spinner_Repoint_ValueChanged);
            // 
            // lblRepoint
            // 
            this.lblRepoint.AutoSize = true;
            this.lblRepoint.Location = new System.Drawing.Point(87, 198);
            this.lblRepoint.Name = "lblRepoint";
            this.lblRepoint.Size = new System.Drawing.Size(53, 13);
            this.lblRepoint.TabIndex = 3;
            this.lblRepoint.Text = "entries to:";
            // 
            // btnRepoint
            // 
            this.btnRepoint.Location = new System.Drawing.Point(13, 194);
            this.btnRepoint.Name = "btnRepoint";
            this.btnRepoint.Size = new System.Drawing.Size(68, 21);
            this.btnRepoint.TabIndex = 2;
            this.btnRepoint.Text = "Repoint";
            this.btnRepoint.UseVisualStyleBackColor = true;
            this.btnRepoint.Click += new System.EventHandler(this.btnRepoint_Click);
            // 
            // flagsCheckedListBox
            // 
            this.flagsCheckedListBox.FormattingEnabled = true;
            this.flagsCheckedListBox.Items.AddRange(new object[] {
            "All or nothing",
            "Random",
            "Separate",
            "Cancel",
            "",
            "",
            "",
            ""});
            this.flagsCheckedListBox.Location = new System.Drawing.Point(3, 25);
            this.flagsCheckedListBox.Name = "flagsCheckedListBox";
            this.flagsCheckedListBox.Size = new System.Drawing.Size(92, 124);
            this.flagsCheckedListBox.TabIndex = 0;
            this.flagsCheckedListBox.TabStop = false;
            // 
            // inflictStatusesEditor
            // 
            this.inflictStatusesEditor.Location = new System.Drawing.Point(101, 3);
            this.inflictStatusesEditor.Name = "inflictStatusesEditor";
            this.inflictStatusesEditor.Size = new System.Drawing.Size(497, 182);
            this.inflictStatusesEditor.Status = "Status";
            this.inflictStatusesEditor.Statuses = null;
            this.inflictStatusesEditor.TabIndex = 1;
            this.inflictStatusesEditor.TabStop = false;
            // 
            // InflictStatusEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.spinner_Repoint);
            this.Controls.Add(this.lblRepoint);
            this.Controls.Add(this.btnRepoint);
            this.Controls.Add(this.flagsCheckedListBox);
            this.Controls.Add(this.inflictStatusesEditor);
            this.Name = "InflictStatusEditor";
            this.Size = new System.Drawing.Size(601, 218);
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Repoint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusesEditor inflictStatusesEditor;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault flagsCheckedListBox;
        private System.Windows.Forms.Button btnRepoint;
        private System.Windows.Forms.Label lblRepoint;
        private System.Windows.Forms.NumericUpDown spinner_Repoint;
    }
}
