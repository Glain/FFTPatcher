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

namespace FFTPatcher.TextEditor
{
    /// <summary>
    /// The main form for FFTacText.
    /// </summary>
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
            if( disposing && (components != null) )
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuItem separator2;
            System.Windows.Forms.MenuItem separator3;
            System.Windows.Forms.MenuItem separator4;
            System.Windows.Forms.MenuItem separator5;
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.saveMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.isoMenuItem = new System.Windows.Forms.MenuItem();
            this.importPsxIsoMenuItem = new System.Windows.Forms.MenuItem();
            this.importPspIsoMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.textMenuItem = new System.Windows.Forms.MenuItem();
            this.helpMenuItem = new System.Windows.Forms.MenuItem();
            this.allowedSymbolsMenuItem = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.patchPsxBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.fileEditor1 = new FFTPatcher.TextEditor.Editors.FileEditor();
            this.progressBar = new PatcherLib.Controls.ProgressBarWithText();
            separator2 = new System.Windows.Forms.MenuItem();
            separator3 = new System.Windows.Forms.MenuItem();
            separator4 = new System.Windows.Forms.MenuItem();
            separator5 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // separator2
            // 
            separator2.Index = 2;
            separator2.Text = "-";
            // 
            // separator3
            // 
            separator3.Index = 1;
            separator3.Text = "-";
            // 
            // separator4
            // 
            separator4.Index = 1;
            separator4.Text = "-";
            // 
            // separator5
            // 
            separator5.Index = 3;
            separator5.Text = "-";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.isoMenuItem,
            this.textMenuItem,
            this.helpMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            separator2,
            this.exitMenuItem});
            this.fileMenuItem.Text = "File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 0;
            this.openMenuItem.Text = "&Open .ffttext...";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Enabled = false;
            this.saveMenuItem.Index = 1;
            this.saveMenuItem.Text = "&Save .ffttext...";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 3;
            this.exitMenuItem.Text = "E&xit";
            // 
            // isoMenuItem
            // 
            this.isoMenuItem.Index = 1;
            this.isoMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.importPsxIsoMenuItem,
            separator4,
            this.importPspIsoMenuItem,
            separator5,
            this.menuItem2});
            this.isoMenuItem.Text = "ISO";
            // 
            // importPsxIsoMenuItem
            // 
            this.importPsxIsoMenuItem.Index = 0;
            this.importPsxIsoMenuItem.Text = "Import PSX ISO...";
            // 
            // importPspIsoMenuItem
            // 
            this.importPspIsoMenuItem.Index = 2;
            this.importPspIsoMenuItem.Text = "Import PSP ISO...";
            // 
            // menuItem2
            // 
            this.menuItem2.Enabled = false;
            this.menuItem2.Index = 4;
            this.menuItem2.Text = "Patch ISO...";
            // 
            // textMenuItem
            // 
            this.textMenuItem.Enabled = false;
            this.textMenuItem.Index = 2;
            this.textMenuItem.Text = "Text";
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Index = 3;
            this.helpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.allowedSymbolsMenuItem,
            separator3,
            this.aboutMenuItem});
            this.helpMenuItem.Text = "Help";
            // 
            // allowedSymbolsMenuItem
            // 
            this.allowedSymbolsMenuItem.Enabled = false;
            this.allowedSymbolsMenuItem.Index = 0;
            this.allowedSymbolsMenuItem.Text = "Allowed symbols";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 2;
            this.aboutMenuItem.Text = "About...";
            // 
            // patchPsxBackgroundWorker
            // 
            this.patchPsxBackgroundWorker.WorkerReportsProgress = true;
            this.patchPsxBackgroundWorker.WorkerSupportsCancellation = true;
            // 
            // fileEditor1
            // 
            this.fileEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileEditor1.Location = new System.Drawing.Point(13, 13);
            this.fileEditor1.Name = "fileEditor1";
            this.fileEditor1.Size = new System.Drawing.Size(753, 398);
            this.fileEditor1.TabIndex = 4;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.BackColor = System.Drawing.Color.White;
            this.progressBar.ForeColor = System.Drawing.Color.Blue;
            this.progressBar.Location = new System.Drawing.Point(12, 388);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(754, 23);
            this.progressBar.TabIndex = 3;
            this.progressBar.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 423);
            this.Controls.Add(this.fileEditor1);
            this.Controls.Add(this.progressBar);
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "FFTacText Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem fileMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem saveMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuItem helpMenuItem;
        private System.Windows.Forms.MenuItem allowedSymbolsMenuItem;
        private System.ComponentModel.BackgroundWorker patchPsxBackgroundWorker;
        private PatcherLib.Controls.ProgressBarWithText progressBar;
        private FFTPatcher.TextEditor.Editors.FileEditor fileEditor1;
        private System.Windows.Forms.MenuItem isoMenuItem;
        private System.Windows.Forms.MenuItem textMenuItem;
        private System.Windows.Forms.MenuItem importPsxIsoMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem importPspIsoMenuItem;

    }
}

