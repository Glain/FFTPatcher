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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuItem separator1;
            System.Windows.Forms.MenuItem separator2;
            System.Windows.Forms.MenuItem separator3;
            System.Windows.Forms.MenuItem utilitiesMenuItem;
            System.Windows.Forms.MenuItem separator6;
            System.Windows.Forms.MenuItem separator5;
            System.Windows.Forms.MenuItem generateResourcesMenuItem;
            System.Windows.Forms.MenuItem separator7;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.extractFFTPackMenuItem = new System.Windows.Forms.MenuItem();
            this.rebuildFFTPackMenuItem = new System.Windows.Forms.MenuItem();
            this.decryptMenuItem = new System.Windows.Forms.MenuItem();
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.newPSXMenuItem = new System.Windows.Forms.MenuItem();
            this.newPSPMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.saveMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAsPspMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.psxMenu = new System.Windows.Forms.MenuItem();
            this.patchPsxIsoMenuItem = new System.Windows.Forms.MenuItem();
            this.openPatchedPsxIso = new System.Windows.Forms.MenuItem();
            this.separator_PSXMenu = new System.Windows.Forms.MenuItem();
            this.menuItem_PatchPSXSavestate = new System.Windows.Forms.MenuItem();
            this.pspMenu = new System.Windows.Forms.MenuItem();
            this.patchPspIsoMenuItem = new System.Windows.Forms.MenuItem();
            this.cheatdbMenuItem = new System.Windows.Forms.MenuItem();
            this.openPatchedPspItem = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.editMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem_SetCurrentDataAsDefaults = new System.Windows.Forms.MenuItem();
            this.menuItem_RestoreDefaults = new System.Windows.Forms.MenuItem();
            this.separator_Edit1 = new System.Windows.Forms.MenuItem();
            this.menuItem_ConsolidateItemAttributes = new System.Windows.Forms.MenuItem();
            this.menuItem_ConsolidateInflictStatuses = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.applyPatchOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.patchPsxBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.menuItem_SavePatchXML = new System.Windows.Forms.MenuItem();
            this.fftPatchEditor1 = new FFTPatcher.Editors.FFTPatchEditor();
            separator1 = new System.Windows.Forms.MenuItem();
            separator2 = new System.Windows.Forms.MenuItem();
            separator3 = new System.Windows.Forms.MenuItem();
            utilitiesMenuItem = new System.Windows.Forms.MenuItem();
            separator6 = new System.Windows.Forms.MenuItem();
            separator5 = new System.Windows.Forms.MenuItem();
            generateResourcesMenuItem = new System.Windows.Forms.MenuItem();
            separator7 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // separator1
            // 
            separator1.Index = 2;
            separator1.Text = "-";
            // 
            // separator2
            // 
            separator2.Index = 7;
            separator2.Text = "-";
            // 
            // separator3
            // 
            separator3.Index = -1;
            separator3.Text = "";
            // 
            // utilitiesMenuItem
            // 
            utilitiesMenuItem.Index = 4;
            utilitiesMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.extractFFTPackMenuItem,
            this.rebuildFFTPackMenuItem,
            separator6,
            this.decryptMenuItem});
            utilitiesMenuItem.Text = "&Utilities";
            // 
            // extractFFTPackMenuItem
            // 
            this.extractFFTPackMenuItem.Index = 0;
            this.extractFFTPackMenuItem.Text = "E&xtract fftpack.bin...";
            // 
            // rebuildFFTPackMenuItem
            // 
            this.rebuildFFTPackMenuItem.Index = 1;
            this.rebuildFFTPackMenuItem.Text = "&Rebuild fftpack.bin...";
            // 
            // separator6
            // 
            separator6.Index = 2;
            separator6.Text = "-";
            // 
            // decryptMenuItem
            // 
            this.decryptMenuItem.Index = 3;
            this.decryptMenuItem.Text = "&Decrypt War of the Lions ISO...";
            // 
            // separator5
            // 
            separator5.Index = 3;
            separator5.Text = "-";
            // 
            // generateResourcesMenuItem
            // 
            generateResourcesMenuItem.Index = 8;
            generateResourcesMenuItem.Text = "&Generate Resources.zip...";
            generateResourcesMenuItem.Click += new System.EventHandler(this.generateResourcesMenuItem_Click);
            // 
            // separator7
            // 
            separator7.Index = 9;
            separator7.Text = "-";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newPSXMenuItem,
            this.newPSPMenuItem,
            separator1,
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsPspMenuItem,
            this.menuItem_SavePatchXML,
            separator2,
            generateResourcesMenuItem,
            separator7,
            this.exitMenuItem});
            this.fileMenuItem.Text = "&File";
            // 
            // newPSXMenuItem
            // 
            this.newPSXMenuItem.Index = 0;
            this.newPSXMenuItem.Text = "New PS&X patch";
            // 
            // newPSPMenuItem
            // 
            this.newPSPMenuItem.Index = 1;
            this.newPSPMenuItem.Text = "New PS&P patch";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 3;
            this.openMenuItem.Text = "&Open patch...";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Enabled = false;
            this.saveMenuItem.Index = 4;
            this.saveMenuItem.Text = "&Save patch...";
            // 
            // saveAsPspMenuItem
            // 
            this.saveAsPspMenuItem.Enabled = false;
            this.saveAsPspMenuItem.Index = 5;
            this.saveAsPspMenuItem.Text = "Save &as PSP patch...";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 10;
            this.exitMenuItem.Text = "E&xit";
            // 
            // psxMenu
            // 
            this.psxMenu.Index = 2;
            this.psxMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.patchPsxIsoMenuItem,
            this.openPatchedPsxIso,
            this.separator_PSXMenu,
            this.menuItem_PatchPSXSavestate});
            this.psxMenu.Text = "PS&X";
            // 
            // patchPsxIsoMenuItem
            // 
            this.patchPsxIsoMenuItem.Enabled = false;
            this.patchPsxIsoMenuItem.Index = 0;
            this.patchPsxIsoMenuItem.Text = "Patch &ISO...";
            // 
            // openPatchedPsxIso
            // 
            this.openPatchedPsxIso.Index = 1;
            this.openPatchedPsxIso.Text = "Open patched ISO...";
            // 
            // separator_PSXMenu
            // 
            this.separator_PSXMenu.Index = 2;
            this.separator_PSXMenu.Text = "-";
            // 
            // menuItem_PatchPSXSavestate
            // 
            this.menuItem_PatchPSXSavestate.Enabled = false;
            this.menuItem_PatchPSXSavestate.Index = 3;
            this.menuItem_PatchPSXSavestate.Text = "Patch pSX Savestate";
            this.menuItem_PatchPSXSavestate.Click += new System.EventHandler(this.menuItem_PatchPSXSaveState_Click);
            // 
            // pspMenu
            // 
            this.pspMenu.Index = 3;
            this.pspMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.patchPspIsoMenuItem,
            this.cheatdbMenuItem,
            this.openPatchedPspItem,
            separator5,
            utilitiesMenuItem});
            this.pspMenu.Text = "&PSP";
            // 
            // patchPspIsoMenuItem
            // 
            this.patchPspIsoMenuItem.Enabled = false;
            this.patchPspIsoMenuItem.Index = 0;
            this.patchPspIsoMenuItem.Text = "&Patch War of the Lions ISO...";
            // 
            // cheatdbMenuItem
            // 
            this.cheatdbMenuItem.Enabled = false;
            this.cheatdbMenuItem.Index = 1;
            this.cheatdbMenuItem.Text = "&Generate cheat.db...";
            // 
            // openPatchedPspItem
            // 
            this.openPatchedPspItem.Index = 2;
            this.openPatchedPspItem.Text = "Open patched ISO...";
            this.openPatchedPspItem.Click += new System.EventHandler(this.openPatchedPspItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.psxMenu,
            this.pspMenu,
            this.aboutMenuItem});
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_SetCurrentDataAsDefaults,
            this.menuItem_RestoreDefaults,
            this.separator_Edit1,
            this.menuItem_ConsolidateItemAttributes,
            this.menuItem_ConsolidateInflictStatuses});
            this.editMenuItem.Text = "Edit";
            // 
            // menuItem_SetCurrentDataAsDefaults
            // 
            this.menuItem_SetCurrentDataAsDefaults.Enabled = false;
            this.menuItem_SetCurrentDataAsDefaults.Index = 0;
            this.menuItem_SetCurrentDataAsDefaults.Text = "Set Current Data as Defaults";
            this.menuItem_SetCurrentDataAsDefaults.Click += new System.EventHandler(this.menuItem_SetCurrentDataAsDefaults_Click);
            // 
            // menuItem_RestoreDefaults
            // 
            this.menuItem_RestoreDefaults.Enabled = false;
            this.menuItem_RestoreDefaults.Index = 1;
            this.menuItem_RestoreDefaults.Text = "Restore Defaults";
            this.menuItem_RestoreDefaults.Click += new System.EventHandler(this.menuItem_RestoreDefaults_Click);
            // 
            // separator_Edit1
            // 
            this.separator_Edit1.Index = 2;
            this.separator_Edit1.Text = "-";
            // 
            // menuItem_ConsolidateItemAttributes
            // 
            this.menuItem_ConsolidateItemAttributes.Enabled = false;
            this.menuItem_ConsolidateItemAttributes.Index = 3;
            this.menuItem_ConsolidateItemAttributes.Text = "Consolidate Item Attributes";
            this.menuItem_ConsolidateItemAttributes.Click += new System.EventHandler(this.menuItem_ConsolidateItemAttributes_Click);
            // 
            // menuItem_ConsolidateInflictStatuses
            // 
            this.menuItem_ConsolidateInflictStatuses.Enabled = false;
            this.menuItem_ConsolidateInflictStatuses.Index = 4;
            this.menuItem_ConsolidateInflictStatuses.Text = "Consolidate Inflict Statuses";
            this.menuItem_ConsolidateInflictStatuses.Click += new System.EventHandler(this.menuItem_ConsolidateInflictStatuses_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 4;
            this.aboutMenuItem.Text = "About...";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "FFTPatcher files|*.fftpatch";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "FFTPatcher files (*.fftpatch)|*.fftpatch";
            // 
            // applyPatchOpenFileDialog
            // 
            this.applyPatchOpenFileDialog.Filter = "ISO files|*.iso";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(87, 299);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(641, 23);
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;
            // 
            // patchPsxBackgroundWorker
            // 
            this.patchPsxBackgroundWorker.WorkerReportsProgress = true;
            this.patchPsxBackgroundWorker.WorkerSupportsCancellation = true;
            // 
            // menuItem_SavePatchXML
            // 
            this.menuItem_SavePatchXML.Enabled = false;
            this.menuItem_SavePatchXML.Index = 6;
            this.menuItem_SavePatchXML.Text = "Save patch XML...";
            this.menuItem_SavePatchXML.Visible = true;            
            this.menuItem_SavePatchXML.Click += new System.EventHandler(this.menuItem_SavePatchXML_Click);
            // 
            // fftPatchEditor1
            // 
            this.fftPatchEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fftPatchEditor1.Enabled = false;
            this.fftPatchEditor1.Location = new System.Drawing.Point(0, 0);
            this.fftPatchEditor1.Name = "fftPatchEditor1";
            this.fftPatchEditor1.Size = new System.Drawing.Size(910, 557);
            this.fftPatchEditor1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(910, 557);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.fftPatchEditor1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "FFTPatcher";
            this.ResumeLayout(false);

        }

        #endregion

        private FFTPatcher.Editors.FFTPatchEditor fftPatchEditor1;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem newPSXMenuItem;
        private System.Windows.Forms.MenuItem newPSPMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem saveMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog applyPatchOpenFileDialog;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem patchPspIsoMenuItem;
        private System.Windows.Forms.MenuItem cheatdbMenuItem;
        private System.Windows.Forms.MenuItem extractFFTPackMenuItem;
        private System.Windows.Forms.MenuItem rebuildFFTPackMenuItem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.MenuItem decryptMenuItem;
        private System.Windows.Forms.MenuItem patchPsxIsoMenuItem;
        private System.Windows.Forms.MenuItem saveAsPspMenuItem;
        private System.ComponentModel.BackgroundWorker patchPsxBackgroundWorker;
        private System.Windows.Forms.MenuItem openPatchedPsxIso;
        private System.Windows.Forms.MenuItem fileMenuItem;
        private System.Windows.Forms.MenuItem psxMenu;
        private System.Windows.Forms.MenuItem pspMenu;
        private System.Windows.Forms.MenuItem openPatchedPspItem;
        private System.Windows.Forms.MenuItem separator_PSXMenu;
        private System.Windows.Forms.MenuItem menuItem_PatchPSXSavestate;
        private System.Windows.Forms.MenuItem editMenuItem;
        private System.Windows.Forms.MenuItem menuItem_ConsolidateItemAttributes;
        private System.Windows.Forms.MenuItem menuItem_ConsolidateInflictStatuses;
        private System.Windows.Forms.MenuItem menuItem_SetCurrentDataAsDefaults;
        private System.Windows.Forms.MenuItem menuItem_RestoreDefaults;
        private System.Windows.Forms.MenuItem separator_Edit1;
        private System.Windows.Forms.MenuItem menuItem_SavePatchXML;

    }
}

