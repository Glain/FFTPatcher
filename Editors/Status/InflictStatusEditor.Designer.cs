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
            this.pnl_ItemUsage = new System.Windows.Forms.Panel();
            this.lbl_ItemUsage_4 = new System.Windows.Forms.LinkLabel();
            this.lbl_ItemUsage_3 = new System.Windows.Forms.Label();
            this.lbl_ItemUsage_2 = new System.Windows.Forms.LinkLabel();
            this.lbl_ItemUsage_1 = new System.Windows.Forms.Label();
            this.pnl_AbilityUsage = new System.Windows.Forms.Panel();
            this.lbl_AbilityUsage_4 = new System.Windows.Forms.LinkLabel();
            this.lbl_AbilityUsage_3 = new System.Windows.Forms.Label();
            this.lbl_AbilityUsage_2 = new System.Windows.Forms.LinkLabel();
            this.lbl_AbilityUsage_1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Repoint)).BeginInit();
            this.pnl_ItemUsage.SuspendLayout();
            this.pnl_AbilityUsage.SuspendLayout();
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
            // pnl_ItemUsage
            // 
            this.pnl_ItemUsage.Controls.Add(this.lbl_ItemUsage_4);
            this.pnl_ItemUsage.Controls.Add(this.lbl_ItemUsage_3);
            this.pnl_ItemUsage.Controls.Add(this.lbl_ItemUsage_2);
            this.pnl_ItemUsage.Controls.Add(this.lbl_ItemUsage_1);
            this.pnl_ItemUsage.Location = new System.Drawing.Point(312, 226);
            this.pnl_ItemUsage.Name = "pnl_ItemUsage";
            this.pnl_ItemUsage.Size = new System.Drawing.Size(289, 34);
            this.pnl_ItemUsage.TabIndex = 22;
            // 
            // lbl_ItemUsage_4
            // 
            this.lbl_ItemUsage_4.AutoSize = true;
            this.lbl_ItemUsage_4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_ItemUsage_4.Location = new System.Drawing.Point(139, 6);
            this.lbl_ItemUsage_4.Name = "lbl_ItemUsage_4";
            this.lbl_ItemUsage_4.Size = new System.Drawing.Size(0, 13);
            this.lbl_ItemUsage_4.TabIndex = 3;
            // 
            // lbl_ItemUsage_3
            // 
            this.lbl_ItemUsage_3.AutoSize = true;
            this.lbl_ItemUsage_3.Location = new System.Drawing.Point(81, 6);
            this.lbl_ItemUsage_3.Name = "lbl_ItemUsage_3";
            this.lbl_ItemUsage_3.Size = new System.Drawing.Size(58, 13);
            this.lbl_ItemUsage_3.TabIndex = 2;
            this.lbl_ItemUsage_3.Text = "items, e.g. ";
            // 
            // lbl_ItemUsage_2
            // 
            this.lbl_ItemUsage_2.AutoSize = true;
            this.lbl_ItemUsage_2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_ItemUsage_2.Location = new System.Drawing.Point(57, 6);
            this.lbl_ItemUsage_2.Name = "lbl_ItemUsage_2";
            this.lbl_ItemUsage_2.Size = new System.Drawing.Size(13, 13);
            this.lbl_ItemUsage_2.TabIndex = 1;
            this.lbl_ItemUsage_2.TabStop = true;
            this.lbl_ItemUsage_2.Text = "0";
            // 
            // lbl_ItemUsage_1
            // 
            this.lbl_ItemUsage_1.AutoSize = true;
            this.lbl_ItemUsage_1.Location = new System.Drawing.Point(6, 6);
            this.lbl_ItemUsage_1.Name = "lbl_ItemUsage_1";
            this.lbl_ItemUsage_1.Size = new System.Drawing.Size(50, 13);
            this.lbl_ItemUsage_1.TabIndex = 0;
            this.lbl_ItemUsage_1.Text = "In use by";
            // 
            // pnl_AbilityUsage
            // 
            this.pnl_AbilityUsage.Controls.Add(this.lbl_AbilityUsage_4);
            this.pnl_AbilityUsage.Controls.Add(this.lbl_AbilityUsage_3);
            this.pnl_AbilityUsage.Controls.Add(this.lbl_AbilityUsage_2);
            this.pnl_AbilityUsage.Controls.Add(this.lbl_AbilityUsage_1);
            this.pnl_AbilityUsage.Location = new System.Drawing.Point(312, 191);
            this.pnl_AbilityUsage.Name = "pnl_AbilityUsage";
            this.pnl_AbilityUsage.Size = new System.Drawing.Size(289, 34);
            this.pnl_AbilityUsage.TabIndex = 23;
            // 
            // lbl_AbilityUsage_4
            // 
            this.lbl_AbilityUsage_4.AutoSize = true;
            this.lbl_AbilityUsage_4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_AbilityUsage_4.Location = new System.Drawing.Point(148, 6);
            this.lbl_AbilityUsage_4.Name = "lbl_AbilityUsage_4";
            this.lbl_AbilityUsage_4.Size = new System.Drawing.Size(0, 13);
            this.lbl_AbilityUsage_4.TabIndex = 3;
            // 
            // lbl_AbilityUsage_3
            // 
            this.lbl_AbilityUsage_3.AutoSize = true;
            this.lbl_AbilityUsage_3.Location = new System.Drawing.Point(81, 6);
            this.lbl_AbilityUsage_3.Name = "lbl_AbilityUsage_3";
            this.lbl_AbilityUsage_3.Size = new System.Drawing.Size(68, 13);
            this.lbl_AbilityUsage_3.TabIndex = 2;
            this.lbl_AbilityUsage_3.Text = "abilities, e.g. ";
            // 
            // lbl_AbilityUsage_2
            // 
            this.lbl_AbilityUsage_2.AutoSize = true;
            this.lbl_AbilityUsage_2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_AbilityUsage_2.Location = new System.Drawing.Point(57, 6);
            this.lbl_AbilityUsage_2.Name = "lbl_AbilityUsage_2";
            this.lbl_AbilityUsage_2.Size = new System.Drawing.Size(13, 13);
            this.lbl_AbilityUsage_2.TabIndex = 1;
            this.lbl_AbilityUsage_2.TabStop = true;
            this.lbl_AbilityUsage_2.Text = "0";
            // 
            // lbl_AbilityUsage_1
            // 
            this.lbl_AbilityUsage_1.AutoSize = true;
            this.lbl_AbilityUsage_1.Location = new System.Drawing.Point(6, 6);
            this.lbl_AbilityUsage_1.Name = "lbl_AbilityUsage_1";
            this.lbl_AbilityUsage_1.Size = new System.Drawing.Size(50, 13);
            this.lbl_AbilityUsage_1.TabIndex = 0;
            this.lbl_AbilityUsage_1.Text = "In use by";
            // 
            // InflictStatusEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnl_AbilityUsage);
            this.Controls.Add(this.pnl_ItemUsage);
            this.Controls.Add(this.spinner_Repoint);
            this.Controls.Add(this.lblRepoint);
            this.Controls.Add(this.btnRepoint);
            this.Controls.Add(this.flagsCheckedListBox);
            this.Controls.Add(this.inflictStatusesEditor);
            this.Name = "InflictStatusEditor";
            this.Size = new System.Drawing.Size(604, 263);
            ((System.ComponentModel.ISupportInitialize)(this.spinner_Repoint)).EndInit();
            this.pnl_ItemUsage.ResumeLayout(false);
            this.pnl_ItemUsage.PerformLayout();
            this.pnl_AbilityUsage.ResumeLayout(false);
            this.pnl_AbilityUsage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusesEditor inflictStatusesEditor;
        private FFTPatcher.Controls.CheckedListBoxNoHighlightWithDefault flagsCheckedListBox;
        private System.Windows.Forms.Button btnRepoint;
        private System.Windows.Forms.Label lblRepoint;
        private System.Windows.Forms.NumericUpDown spinner_Repoint;
        private System.Windows.Forms.Panel pnl_ItemUsage;
        private System.Windows.Forms.LinkLabel lbl_ItemUsage_4;
        private System.Windows.Forms.Label lbl_ItemUsage_3;
        private System.Windows.Forms.LinkLabel lbl_ItemUsage_2;
        private System.Windows.Forms.Label lbl_ItemUsage_1;
        private System.Windows.Forms.Panel pnl_AbilityUsage;
        private System.Windows.Forms.LinkLabel lbl_AbilityUsage_4;
        private System.Windows.Forms.Label lbl_AbilityUsage_3;
        private System.Windows.Forms.LinkLabel lbl_AbilityUsage_2;
        private System.Windows.Forms.Label lbl_AbilityUsage_1;
    }
}
