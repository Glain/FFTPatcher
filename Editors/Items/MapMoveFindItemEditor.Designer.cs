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
    partial class MapMoveFindItemEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.moveFindItemEditor1 = new FFTPatcher.Editors.MoveFindItemEditor();
            this.moveFindItemEditor3 = new FFTPatcher.Editors.MoveFindItemEditor();
            this.moveFindItemEditor2 = new FFTPatcher.Editors.MoveFindItemEditor();
            this.moveFindItemEditor4 = new FFTPatcher.Editors.MoveFindItemEditor();
            this.SuspendLayout();
            // 
            // moveFindItemEditor1
            // 
            this.moveFindItemEditor1.AutoSize = true;
            this.moveFindItemEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moveFindItemEditor1.Label = "1";
            this.moveFindItemEditor1.Location = new System.Drawing.Point( 3, 3 );
            this.moveFindItemEditor1.MoveFindItem = null;
            this.moveFindItemEditor1.Name = "moveFindItemEditor1";
            this.moveFindItemEditor1.Size = new System.Drawing.Size( 367, 164 );
            this.moveFindItemEditor1.TabIndex = 0;
            // 
            // moveFindItemEditor3
            // 
            this.moveFindItemEditor3.AutoSize = true;
            this.moveFindItemEditor3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moveFindItemEditor3.Label = "3";
            this.moveFindItemEditor3.Location = new System.Drawing.Point( 3, 343 );
            this.moveFindItemEditor3.MoveFindItem = null;
            this.moveFindItemEditor3.Name = "moveFindItemEditor3";
            this.moveFindItemEditor3.Size = new System.Drawing.Size( 367, 164 );
            this.moveFindItemEditor3.TabIndex = 2;
            // 
            // moveFindItemEditor2
            // 
            this.moveFindItemEditor2.AutoSize = true;
            this.moveFindItemEditor2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moveFindItemEditor2.Label = "2";
            this.moveFindItemEditor2.Location = new System.Drawing.Point( 3, 173 );
            this.moveFindItemEditor2.MoveFindItem = null;
            this.moveFindItemEditor2.Name = "moveFindItemEditor2";
            this.moveFindItemEditor2.Size = new System.Drawing.Size( 367, 164 );
            this.moveFindItemEditor2.TabIndex = 1;
            // 
            // moveFindItemEditor4
            // 
            this.moveFindItemEditor4.AutoSize = true;
            this.moveFindItemEditor4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moveFindItemEditor4.Label = "4";
            this.moveFindItemEditor4.Location = new System.Drawing.Point( 3, 513 );
            this.moveFindItemEditor4.MoveFindItem = null;
            this.moveFindItemEditor4.Name = "moveFindItemEditor4";
            this.moveFindItemEditor4.Size = new System.Drawing.Size( 367, 164 );
            this.moveFindItemEditor4.TabIndex = 3;
            // 
            // MapMoveFindItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add( this.moveFindItemEditor4 );
            this.Controls.Add( this.moveFindItemEditor2 );
            this.Controls.Add( this.moveFindItemEditor3 );
            this.Controls.Add( this.moveFindItemEditor1 );
            this.Name = "MapMoveFindItemEditor";
            this.Size = new System.Drawing.Size( 373, 680 );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private MoveFindItemEditor moveFindItemEditor1;
        private MoveFindItemEditor moveFindItemEditor3;
        private MoveFindItemEditor moveFindItemEditor2;
        private MoveFindItemEditor moveFindItemEditor4;
    }
}
