﻿/*
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

namespace FFTPatcher.SpriteEditor
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
            if (disposing && (components != null))
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
            System.Windows.Forms.MainMenu mainMenu;
            System.Windows.Forms.MenuItem fileMenu;
            System.Windows.Forms.MenuItem openIsoMenuItem;
            System.Windows.Forms.MenuItem separator1;
            System.Windows.Forms.MenuItem exitMenuItem;
            System.Windows.Forms.MenuItem separator6;
            System.Windows.Forms.MenuItem importSprMenuItem;
            System.Windows.Forms.MenuItem exportSprMenuItem;
            System.Windows.Forms.MenuItem separator2;
            System.Windows.Forms.MenuItem menuItem_ImportSpriteImage;
            System.Windows.Forms.MenuItem menuItem_ExportSpriteImage;
            System.Windows.Forms.MenuItem separator3;
            System.Windows.Forms.MenuItem separator4;
            System.Windows.Forms.MenuItem separator5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuItem_ExpandIso = new System.Windows.Forms.MenuItem();
            this.menuItem_SavePatch = new System.Windows.Forms.MenuItem();
            this.menuItem_ApplyPatch = new System.Windows.Forms.MenuItem();
            this.separator_Exit = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.imageMenuItem = new System.Windows.Forms.MenuItem();
            this.importImageMenuItem = new System.Windows.Forms.MenuItem();
            this.exportImageMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem_ImportEntireFile = new System.Windows.Forms.MenuItem();
            this.menuItem_ExportEntireFile = new System.Windows.Forms.MenuItem();
            this.importAllImagesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem_ExportAllImages_BMP = new System.Windows.Forms.MenuItem();
            this.menuItem_ExportAllImages_PNG = new System.Windows.Forms.MenuItem();
            this.spriteMenuItem = new System.Windows.Forms.MenuItem();
            this.reimportMenuItem = new System.Windows.Forms.MenuItem();
            this.separatorImportExportAllSprites = new System.Windows.Forms.MenuItem();
            this.menuItem_ImportAllSprites = new System.Windows.Forms.MenuItem();
            this.menuItem_ExportAllSprites_BMP = new System.Windows.Forms.MenuItem();
            this.menuItem_ExportAllSprites_PNG = new System.Windows.Forms.MenuItem();
            this.sp2Menu = new System.Windows.Forms.MenuItem();
            this.importFirstMenuItem = new System.Windows.Forms.MenuItem();
            this.exportFirstMenuItem = new System.Windows.Forms.MenuItem();
            this.importSecondMenuItem = new System.Windows.Forms.MenuItem();
            this.exportSecondMenuItem = new System.Windows.Forms.MenuItem();
            this.importThirdMenuItem = new System.Windows.Forms.MenuItem();
            this.exportThirdMenuItem = new System.Windows.Forms.MenuItem();
            this.importFourthMenuItem = new System.Windows.Forms.MenuItem();
            this.exportFourthMenuItem = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.spriteTabPage = new System.Windows.Forms.TabPage();
            this.allSpritesEditor1 = new FFTPatcher.SpriteEditor.AllSpritesEditor();
            this.otherTabPage = new System.Windows.Forms.TabPage();
            this.allOtherImagesEditor1 = new FFTPatcher.SpriteEditor.AllOtherImagesEditor();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.reimportButton = new System.Windows.Forms.Button();
            mainMenu = new System.Windows.Forms.MainMenu(this.components);
            fileMenu = new System.Windows.Forms.MenuItem();
            openIsoMenuItem = new System.Windows.Forms.MenuItem();
            separator1 = new System.Windows.Forms.MenuItem();
            exitMenuItem = new System.Windows.Forms.MenuItem();
            separator6 = new System.Windows.Forms.MenuItem();
            importSprMenuItem = new System.Windows.Forms.MenuItem();
            exportSprMenuItem = new System.Windows.Forms.MenuItem();
            separator2 = new System.Windows.Forms.MenuItem();
            menuItem_ImportSpriteImage = new System.Windows.Forms.MenuItem();
            menuItem_ExportSpriteImage = new System.Windows.Forms.MenuItem();
            separator3 = new System.Windows.Forms.MenuItem();
            separator4 = new System.Windows.Forms.MenuItem();
            separator5 = new System.Windows.Forms.MenuItem();
            this.tabControl1.SuspendLayout();
            this.spriteTabPage.SuspendLayout();
            this.otherTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            fileMenu,
            this.imageMenuItem,
            this.spriteMenuItem,
            this.sp2Menu});
            // 
            // fileMenu
            // 
            fileMenu.Index = 0;
            fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            openIsoMenuItem,
            separator1,
            this.menuItem_ExpandIso,
            this.menuItem3,
            this.menuItem_SavePatch,
            this.menuItem_ApplyPatch,
            this.separator_Exit,
            exitMenuItem});
            fileMenu.Text = "&File";
            // 
            // openIsoMenuItem
            // 
            openIsoMenuItem.Index = 0;
            openIsoMenuItem.Text = "&Open ISO...";
            openIsoMenuItem.Click += new System.EventHandler(this.openIsoMenuItem_Click);
            // 
            // separator1
            // 
            separator1.Index = 1;
            separator1.Text = "-";
            // 
            // menuItem_ExpandIso
            // 
            this.menuItem_ExpandIso.Enabled = false;
            this.menuItem_ExpandIso.Index = 2;
            this.menuItem_ExpandIso.Text = "Expand ISO";
            this.menuItem_ExpandIso.Click += new System.EventHandler(this.menuItem_ExpandIso_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // 
            // menuItem_SavePatch
            // 
            this.menuItem_SavePatch.Enabled = false;
            this.menuItem_SavePatch.Index = 4;
            this.menuItem_SavePatch.Text = "Save Patch...";
            this.menuItem_SavePatch.Click += new System.EventHandler(this.menuItem_SavePatch_Click);
            // 
            // menuItem_ApplyPatch
            // 
            this.menuItem_ApplyPatch.Enabled = false;
            this.menuItem_ApplyPatch.Index = 5;
            this.menuItem_ApplyPatch.Text = "Apply Patch...";
            this.menuItem_ApplyPatch.Click += new System.EventHandler(this.menuItem_ApplyPatch_Click);
            // 
            // separator_Exit
            // 
            this.separator_Exit.Index = 6;
            this.separator_Exit.Text = "-";
            // 
            // exitMenuItem
            // 
            exitMenuItem.Index = 7;
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // imageMenuItem
            // 
            this.imageMenuItem.Index = 1;
            this.imageMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.importImageMenuItem,
            this.exportImageMenuItem,
            this.menuItem2,
            this.menuItem_ImportEntireFile,
            this.menuItem_ExportEntireFile,
            separator6,
            this.importAllImagesMenuItem,
            this.menuItem_ExportAllImages_BMP,
            this.menuItem_ExportAllImages_PNG});
            this.imageMenuItem.Text = "Image";
            this.imageMenuItem.Visible = false;
            // 
            // importImageMenuItem
            // 
            this.importImageMenuItem.Index = 0;
            this.importImageMenuItem.Text = "Import...";
            this.importImageMenuItem.Click += new System.EventHandler(this.importImageMenuItem_Click);
            // 
            // exportImageMenuItem
            // 
            this.exportImageMenuItem.Index = 1;
            this.exportImageMenuItem.Text = "Export...";
            this.exportImageMenuItem.Click += new System.EventHandler(this.exportImageMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            // 
            // menuItem_ImportEntireFile
            // 
            this.menuItem_ImportEntireFile.Index = 3;
            this.menuItem_ImportEntireFile.Text = "Import entire file...";
            this.menuItem_ImportEntireFile.Click += new System.EventHandler(this.menuItem_ImportEntireFile_Click);
            // 
            // menuItem_ExportEntireFile
            // 
            this.menuItem_ExportEntireFile.Index = 4;
            this.menuItem_ExportEntireFile.Text = "Export entire file...";
            this.menuItem_ExportEntireFile.Click += new System.EventHandler(this.menuItem_ExportEntireFile_Click);
            // 
            // separator6
            // 
            separator6.Index = 5;
            separator6.Text = "-";
            // 
            // importAllImagesMenuItem
            // 
            this.importAllImagesMenuItem.Index = 6;
            this.importAllImagesMenuItem.Text = "Import all images...";
            this.importAllImagesMenuItem.Click += new System.EventHandler(this.importAllImagesMenuItem_Click);
            // 
            // menuItem_ExportAllImages_BMP
            // 
            this.menuItem_ExportAllImages_BMP.Index = 7;
            this.menuItem_ExportAllImages_BMP.Text = "Export all images (BMP)...";
            this.menuItem_ExportAllImages_BMP.Click += new System.EventHandler(this.menuItem_ExportAllImages_BMP_Click);
            // 
            // menuItem_ExportAllImages_PNG
            // 
            this.menuItem_ExportAllImages_PNG.Index = 8;
            this.menuItem_ExportAllImages_PNG.Text = "Export all images (PNG)...";
            this.menuItem_ExportAllImages_PNG.Click += new System.EventHandler(this.menuItem_ExportAllImages_PNG_Click);
            // 
            // spriteMenuItem
            // 
            this.spriteMenuItem.Enabled = false;
            this.spriteMenuItem.Index = 2;
            this.spriteMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            importSprMenuItem,
            exportSprMenuItem,
            separator2,
            menuItem_ImportSpriteImage,
            menuItem_ExportSpriteImage,
            this.reimportMenuItem,
            this.separatorImportExportAllSprites,
            this.menuItem_ImportAllSprites,
            this.menuItem_ExportAllSprites_BMP,
            this.menuItem_ExportAllSprites_PNG});
            this.spriteMenuItem.Text = "Sprite";
            this.spriteMenuItem.Popup += new System.EventHandler(this.spriteMenuItem_Popup);
            // 
            // importSprMenuItem
            // 
            importSprMenuItem.Index = 0;
            importSprMenuItem.Text = "Import SPR...";
            importSprMenuItem.Click += new System.EventHandler(this.importSprMenuItem_Click);
            // 
            // exportSprMenuItem
            // 
            exportSprMenuItem.Index = 1;
            exportSprMenuItem.Text = "Export SPR...";
            exportSprMenuItem.Click += new System.EventHandler(this.exportSprMenuItem_Click);
            // 
            // separator2
            // 
            separator2.Index = 2;
            separator2.Text = "-";
            // 
            // menuItem_ImportImage
            // 
            menuItem_ImportSpriteImage.Index = 3;
            menuItem_ImportSpriteImage.Text = "Import BMP/PNG...";
            menuItem_ImportSpriteImage.Click += new System.EventHandler(this.menuItem_ImportSpriteImage_Click);
            // 
            // menuItem_ExportImage
            // 
            menuItem_ExportSpriteImage.Index = 4;
            menuItem_ExportSpriteImage.Text = "Export BMP/PNG...";
            menuItem_ExportSpriteImage.Click += new System.EventHandler(this.menuItem_ExportSpriteImage_Click);
            // 
            // reimportMenuItem
            // 
            this.reimportMenuItem.Index = 5;
            this.reimportMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.reimportMenuItem.Text = "Reimport BMP/PNG";
            this.reimportMenuItem.Click += new System.EventHandler(this.reimportMenuItem_Click);
            // 
            // separatorImportExportAllSprites
            // 
            this.separatorImportExportAllSprites.Index = 6;
            this.separatorImportExportAllSprites.Text = "-";
            // 
            // menuItem_ImportAllSprites
            // 
            this.menuItem_ImportAllSprites.Index = 7;
            this.menuItem_ImportAllSprites.Text = "Import all sprites...";
            this.menuItem_ImportAllSprites.Click += new System.EventHandler(this.importAllSpritesMenuItem_Click);
            // 
            // menuItem_ExportAllSprites_BMP
            // 
            this.menuItem_ExportAllSprites_BMP.Index = 8;
            this.menuItem_ExportAllSprites_BMP.Text = "Export all sprites (BMP)...";
            this.menuItem_ExportAllSprites_BMP.Click += new System.EventHandler(this.menuItem_ExportAllSprites_BMP_Click);
            // 
            // menuItem_ExportAllSprites_PNG
            // 
            this.menuItem_ExportAllSprites_PNG.Index = 9;
            this.menuItem_ExportAllSprites_PNG.Text = "Export all sprites (PNG)...";
            this.menuItem_ExportAllSprites_PNG.Click += new System.EventHandler(this.menuItem_ExportAllSprites_PNG_Click);
            // 
            // sp2Menu
            // 
            this.sp2Menu.Enabled = false;
            this.sp2Menu.Index = 3;
            this.sp2Menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.importFirstMenuItem,
            this.exportFirstMenuItem,
            separator3,
            this.importSecondMenuItem,
            this.exportSecondMenuItem,
            separator4,
            this.importThirdMenuItem,
            this.exportThirdMenuItem,
            separator5,
            this.importFourthMenuItem,
            this.exportFourthMenuItem});
            this.sp2Menu.Text = "SP2";
            this.sp2Menu.Popup += new System.EventHandler(this.sp2Menu_Popup);
            // 
            // importFirstMenuItem
            // 
            this.importFirstMenuItem.Enabled = false;
            this.importFirstMenuItem.Index = 0;
            this.importFirstMenuItem.Tag = "1";
            this.importFirstMenuItem.Text = "Import first SP2...";
            this.importFirstMenuItem.Click += new System.EventHandler(this.importSp2MenuItem_Click);
            // 
            // exportFirstMenuItem
            // 
            this.exportFirstMenuItem.Enabled = false;
            this.exportFirstMenuItem.Index = 1;
            this.exportFirstMenuItem.Tag = "1";
            this.exportFirstMenuItem.Text = "Export first SP2...";
            this.exportFirstMenuItem.Click += new System.EventHandler(this.exportSp2MenuItem_Click);
            // 
            // separator3
            // 
            separator3.Index = 2;
            separator3.Text = "-";
            // 
            // importSecondMenuItem
            // 
            this.importSecondMenuItem.Enabled = false;
            this.importSecondMenuItem.Index = 3;
            this.importSecondMenuItem.Tag = "2";
            this.importSecondMenuItem.Text = "Import second SP2...";
            this.importSecondMenuItem.Click += new System.EventHandler(this.importSp2MenuItem_Click);
            // 
            // exportSecondMenuItem
            // 
            this.exportSecondMenuItem.Enabled = false;
            this.exportSecondMenuItem.Index = 4;
            this.exportSecondMenuItem.Tag = "2";
            this.exportSecondMenuItem.Text = "Export second SP2...";
            this.exportSecondMenuItem.Click += new System.EventHandler(this.exportSp2MenuItem_Click);
            // 
            // separator4
            // 
            separator4.Index = 5;
            separator4.Text = "-";
            // 
            // importThirdMenuItem
            // 
            this.importThirdMenuItem.Enabled = false;
            this.importThirdMenuItem.Index = 6;
            this.importThirdMenuItem.Tag = "3";
            this.importThirdMenuItem.Text = "Import third SP2...";
            this.importThirdMenuItem.Click += new System.EventHandler(this.importSp2MenuItem_Click);
            // 
            // exportThirdMenuItem
            // 
            this.exportThirdMenuItem.Enabled = false;
            this.exportThirdMenuItem.Index = 7;
            this.exportThirdMenuItem.Tag = "3";
            this.exportThirdMenuItem.Text = "Export third SP2...";
            this.exportThirdMenuItem.Click += new System.EventHandler(this.exportSp2MenuItem_Click);
            // 
            // separator5
            // 
            separator5.Index = 8;
            separator5.Text = "-";
            // 
            // importFourthMenuItem
            // 
            this.importFourthMenuItem.Enabled = false;
            this.importFourthMenuItem.Index = 9;
            this.importFourthMenuItem.Tag = "4";
            this.importFourthMenuItem.Text = "Import fourth SP2...";
            this.importFourthMenuItem.Click += new System.EventHandler(this.importSp2MenuItem_Click);
            // 
            // exportFourthMenuItem
            // 
            this.exportFourthMenuItem.Enabled = false;
            this.exportFourthMenuItem.Index = 10;
            this.exportFourthMenuItem.Tag = "4";
            this.exportFourthMenuItem.Text = "Export fourth SP2...";
            this.exportFourthMenuItem.Click += new System.EventHandler(this.exportSp2MenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Sprite files (*.SPR)|*.SPR|Secondary sprite files (*.SP2)|*.SP2";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.spriteTabPage);
            this.tabControl1.Controls.Add(this.otherTabPage);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(2, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(643, 708);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // spriteTabPage
            // 
            this.spriteTabPage.Controls.Add(this.allSpritesEditor1);
            this.spriteTabPage.Location = new System.Drawing.Point(4, 22);
            this.spriteTabPage.Name = "spriteTabPage";
            this.spriteTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.spriteTabPage.Size = new System.Drawing.Size(635, 677);
            this.spriteTabPage.TabIndex = 0;
            this.spriteTabPage.Text = "Sprites";
            this.spriteTabPage.UseVisualStyleBackColor = true;
            // 
            // allSpritesEditor1
            //
            /*
            this.allSpritesEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            */
            //this.allSpritesEditor1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.allSpritesEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allSpritesEditor1.Enabled = false;
            this.allSpritesEditor1.Location = new System.Drawing.Point(0, 0);
            this.allSpritesEditor1.Name = "allSpritesEditor1";
            this.allSpritesEditor1.Size = new System.Drawing.Size(629, 671);
            this.allSpritesEditor1.TabIndex = 0;
            // 
            // otherTabPage
            // 
            this.otherTabPage.Controls.Add(this.allOtherImagesEditor1);
            this.otherTabPage.Location = new System.Drawing.Point(4, 22);
            this.otherTabPage.Name = "otherTabPage";
            this.otherTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.otherTabPage.Size = new System.Drawing.Size(635, 677);
            this.otherTabPage.TabIndex = 1;
            this.otherTabPage.Text = "Other Images";
            this.otherTabPage.UseVisualStyleBackColor = true;
            // 
            // allOtherImagesEditor1
            // 
            this.allOtherImagesEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allOtherImagesEditor1.Enabled = false;
            this.allOtherImagesEditor1.Location = new System.Drawing.Point(3, 3);
            this.allOtherImagesEditor1.Name = "allOtherImagesEditor1";
            this.allOtherImagesEditor1.Size = new System.Drawing.Size(629, 671);
            this.allOtherImagesEditor1.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(522, 98);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(70, 30);
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // reimportButton
            // 
            this.reimportButton.Location = new System.Drawing.Point(0, 0);
            this.reimportButton.Name = "reimportButton";
            this.reimportButton.Size = new System.Drawing.Size(75, 23);
            this.reimportButton.TabIndex = 0;
            this.reimportButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.reimportButtonKeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tabControl1);
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            //this.MaximizeBox = false;
            this.Menu = mainMenu;
            this.MinimumSize = new System.Drawing.Size(663, 762);
            this.Name = "MainForm";
            this.Size = new System.Drawing.Size(663, 762);
            this.Text = "Shishi Sprite Manager";
            this.tabControl1.ResumeLayout(false);
            this.spriteTabPage.ResumeLayout(false);
            this.otherTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.ResizeEnd += MainForm_ResizeEnd;

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private AllSpritesEditor allSpritesEditor1;
        private System.Windows.Forms.MenuItem spriteMenuItem;
        private System.Windows.Forms.MenuItem sp2Menu;
        private System.Windows.Forms.MenuItem importFirstMenuItem;
        private System.Windows.Forms.MenuItem exportFirstMenuItem;
        private System.Windows.Forms.MenuItem importSecondMenuItem;
        private System.Windows.Forms.MenuItem exportSecondMenuItem;
        private System.Windows.Forms.MenuItem importThirdMenuItem;
        private System.Windows.Forms.MenuItem exportThirdMenuItem;
        private System.Windows.Forms.MenuItem importFourthMenuItem;
        private System.Windows.Forms.MenuItem exportFourthMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage spriteTabPage;
        private System.Windows.Forms.TabPage otherTabPage;
        private FFTPatcher.SpriteEditor.AllOtherImagesEditor allOtherImagesEditor1;
        private System.Windows.Forms.MenuItem importImageMenuItem;
        private System.Windows.Forms.MenuItem exportImageMenuItem;
        private System.Windows.Forms.MenuItem importAllImagesMenuItem;
        private System.Windows.Forms.MenuItem menuItem_ExportAllImages_BMP;
        private System.Windows.Forms.MenuItem menuItem_ExportAllImages_PNG;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.MenuItem imageMenuItem;

		private System.Windows.Forms.Button reimportButton; // R999
		private System.Windows.Forms.MenuItem reimportMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem_ImportEntireFile;
        private System.Windows.Forms.MenuItem menuItem_ExportEntireFile;
        private System.Windows.Forms.MenuItem menuItem_ExpandIso;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem_SavePatch;
        private System.Windows.Forms.MenuItem menuItem_ApplyPatch;
        private System.Windows.Forms.MenuItem separator_Exit;

        private System.Windows.Forms.MenuItem menuItem_ImportAllSprites;
        private System.Windows.Forms.MenuItem menuItem_ExportAllSprites_BMP;
        private System.Windows.Forms.MenuItem menuItem_ExportAllSprites_PNG;
        private System.Windows.Forms.MenuItem separatorImportExportAllSprites;
    }
}

