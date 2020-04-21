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
    partial class AllAbilitiesEditor
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
            this.abilitiesListBox = new PatcherLib.Controls.ModifiedListBox();
            this.abilityEditor = new FFTPatcher.Editors.AbilityEditor();
            this.SuspendLayout();
            // 
            // abilitiesListBox
            // 
            this.abilitiesListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.abilitiesListBox.FormattingEnabled = true;
            this.abilitiesListBox.IncludePrefix = false;
            this.abilitiesListBox.Location = new System.Drawing.Point(0, 0);
            this.abilitiesListBox.Name = "abilitiesListBox";
            this.abilitiesListBox.Size = new System.Drawing.Size(167, 695);
            this.abilitiesListBox.TabIndex = 0;
            this.abilitiesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.abilitiesListBox_KeyDown);
            // 
            // abilityEditor
            // 
            this.abilityEditor.Ability = null;
            this.abilityEditor.AutoScroll = true;
            this.abilityEditor.AutoSize = true;
            this.abilityEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.abilityEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.abilityEditor.Location = new System.Drawing.Point(167, 0);
            this.abilityEditor.Name = "abilityEditor";
            this.abilityEditor.Size = new System.Drawing.Size(607, 695);
            this.abilityEditor.TabIndex = 1;
            // 
            // AllAbilitiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.abilityEditor);
            this.Controls.Add(this.abilitiesListBox);
            this.Name = "AllAbilitiesEditor";
            this.Size = new System.Drawing.Size(774, 695);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PatcherLib.Controls.ModifiedListBox abilitiesListBox;
        private AbilityEditor abilityEditor;
    }
}
