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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PatcherLib.Datatypes;
using System.IO;
using PatcherLib;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing.Imaging;
using FFTPatcher.SpriteEditor.Helpers;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    public partial class MainForm : Form
    {
        const string titleFormatString = "Shishi Sprite Manager (v{0}) - {1}";
        const string str8bppFileFilterImport = "8bpp paletted BMP/PNG (*.BMP, *.PNG)|*.bmp;*.png";
        const string str4bppFileFilterImport = "4bpp paletted BMP/PNG (*.BMP, *.PNG)|*.bmp;*.png";
        const string str8bppFileFilterExport = "8bpp paletted bitmap (*.bmp)|*.bmp|8bpp paletted PNG (*.png)|*.png";
        const string str4bppFileFilterExport = "4bpp paletted bitmap (*.bmp)|*.bmp|4bpp paletted PNG (*.png)|*.png";

        private static readonly string versionString = PatcherLib.Helpers.VersionHelper.VersionString;
        private static readonly string shortFormTitle = String.Format("Shishi Sprite Manager (v{0})", versionString);

		private string _importFilepath = null;  // R999

        public MainForm()
        {
            InitializeComponent();
            allSpritesEditor1.SpriteDragEnter += sprite_DragEnter;
            allSpritesEditor1.SpriteDragDrop += sprite_DragDrop;
            Text = shortFormTitle;
        }

        private Stream currentStream = null;

        private void ImportSpriteImage()
        {
            openFileDialog.Filter = allSpritesEditor1.ImportExport8Bpp ? str8bppFileFilterImport : str4bppFileFilterImport;
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                ImportSpriteImage(openFileDialog.FileName);
            }
        }

        private void ReimportSpriteImage()
        {
            ImportSpriteImage(_importFilepath);
        }

        private void ImportSpriteImage(string filepath)
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;

            if ((currentSprite != null) && (filepath != null) && (File.Exists(filepath)))
            {
                int paletteIndex = allSpritesEditor1.PaletteIndex;
                bool importExport8Bpp = allSpritesEditor1.ImportExport8Bpp;

                _importFilepath = filepath;

                if (filepath.ToLower().EndsWith(".png"))
                    currentSprite.ImportPNG(currentStream, filepath, !importExport8Bpp, paletteIndex);
                else if (importExport8Bpp)
                    currentSprite.ImportBitmap8bpp(currentStream, filepath);
                else
                    currentSprite.ImportBitmap4bpp(currentStream, filepath, paletteIndex);

                allSpritesEditor1.ReloadCurrentSprite();
            }
        }

        private void ExportSpriteImage()
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            int paletteIndex = allSpritesEditor1.PaletteIndex;
            bool importExport8Bpp = allSpritesEditor1.ImportExport8Bpp;

            saveFileDialog.Filter = importExport8Bpp ? str8bppFileFilterExport : str4bppFileFilterExport;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CreatePrompt = false;
            saveFileDialog.FileName = string.Empty;

            if (currentSprite != null && saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                AbstractSprite sprite = currentSprite.GetAbstractSpriteFromIso(currentStream, true);
                Bitmap bitmap = importExport8Bpp ? sprite.ToBitmap() : sprite.To4bppBitmapUncached(paletteIndex);
                bool isPNG = saveFileDialog.FileName.ToLower().EndsWith(".png");
                ImageFormat imageFormat = isPNG ? ImageFormat.Png : ImageFormat.Bmp;
                bitmap.Save(saveFileDialog.FileName, imageFormat);
            }
        }

        private void ExportAllSprites(ImageFormat format)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            Cursor oldCursor = Cursor;

            ProgressChangedEventHandler progressHandler = delegate (object sender2, ProgressChangedEventArgs args2)
            {
                MethodInvoker mi = (() => progressBar1.Value = args2.ProgressPercentage);
                if (progressBar1.InvokeRequired)
                    progressBar1.Invoke(mi);
                else mi();
            };

            RunWorkerCompletedEventHandler completeHandler = null;

            completeHandler = delegate (object sender1, RunWorkerCompletedEventArgs args1)
            {
                MethodInvoker mi = delegate ()
                {
                    var result = args1.Result as AllSprites.AllSpritesDoWorkResult;
                    tabControl1.Enabled = true;
                    progressBar1.Visible = false;
                    if (oldCursor != null) Cursor = oldCursor;
                    backgroundWorker1.RunWorkerCompleted -= completeHandler;
                    backgroundWorker1.ProgressChanged -= progressHandler;
                    backgroundWorker1.DoWork -= allSpritesEditor1.Sprites.ExportAllSprites;
                    MyMessageBox.Show(this, string.Format("{0} sprites exported", result.ImagesProcessed), result.DoWorkResult.ToString(), MessageBoxButtons.OK);
                };
                if (InvokeRequired) Invoke(mi);
                else mi();
            };

            //if (fbd.ShowDialog(this) == DialogResult.OK)
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                progressBar1.Bounds = new Rectangle(ClientRectangle.Left + 10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width - 20, progressBar1.Height);
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                backgroundWorker1.DoWork += allSpritesEditor1.Sprites.ExportAllSprites;
                backgroundWorker1.ProgressChanged += progressHandler;
                backgroundWorker1.RunWorkerCompleted += completeHandler;
                backgroundWorker1.WorkerReportsProgress = true;
                tabControl1.Enabled = false;
                Cursor = Cursors.WaitCursor;
                progressBar1.BringToFront();

                bool importExport8bpp = allSpritesEditor1.ImportExport8Bpp;
                int paletteIndex = importExport8bpp ? 0 : allSpritesEditor1.PaletteIndex;
                //backgroundWorker1.RunWorkerAsync(new AllSprites.AllSpritesDoWorkData(currentStream, fbd.SelectedPath, importExport8bpp, paletteIndex));
                backgroundWorker1.RunWorkerAsync(new AllSprites.AllSpritesDoWorkData(currentStream, format, dialog.FileName, importExport8bpp, paletteIndex));
            }
        }

        private void ExportAllImages(ImageFormat format)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            Cursor oldCursor = Cursor;

            ProgressChangedEventHandler progressHandler = delegate (object sender2, ProgressChangedEventArgs args2)
            {
                MethodInvoker mi = (() => progressBar1.Value = args2.ProgressPercentage);
                if (progressBar1.InvokeRequired)
                    progressBar1.Invoke(mi);
                else mi();
            };

            RunWorkerCompletedEventHandler completeHandler = null;

            completeHandler = delegate (object sender1, RunWorkerCompletedEventArgs args1)
            {
                MethodInvoker mi = delegate ()
                {
                    var result = args1.Result as AllOtherImages.AllImagesDoWorkResult;
                    tabControl1.Enabled = true;
                    progressBar1.Visible = false;
                    if (oldCursor != null) Cursor = oldCursor;
                    backgroundWorker1.RunWorkerCompleted -= completeHandler;
                    backgroundWorker1.ProgressChanged -= progressHandler;
                    backgroundWorker1.DoWork -= allOtherImagesEditor1.AllOtherImages.ExportAllImages;
                    MyMessageBox.Show(this, string.Format("{0} images saved", result.ImagesProcessed), result.DoWorkResult.ToString(), MessageBoxButtons.OK);
                };
                if (InvokeRequired) Invoke(mi);
                else mi();
            };

            //if (fbd.ShowDialog( this ) == DialogResult.OK)
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                progressBar1.Bounds = new Rectangle(ClientRectangle.Left + 10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width - 20, progressBar1.Height);
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                backgroundWorker1.DoWork += allOtherImagesEditor1.AllOtherImages.ExportAllImages;
                backgroundWorker1.ProgressChanged += progressHandler;
                backgroundWorker1.RunWorkerCompleted += completeHandler;
                backgroundWorker1.WorkerReportsProgress = true;
                tabControl1.Enabled = false;
                Cursor = Cursors.WaitCursor;
                progressBar1.BringToFront();
                backgroundWorker1.RunWorkerAsync(new AllOtherImages.AllImagesDoWorkData(currentStream, format, dialog.FileName));
            }
        }

        private void sprite_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && System.IO.File.Exists(files[0]))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void sprite_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths.Length == 1 && System.IO.File.Exists(paths[0]))
                {
                    ImportSpriteImage(paths[0]);
                }
            }
        }

        private void openIsoMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                FileStream openedStream = File.Open( openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read );
                if (openedStream != null)
                {
                    bool psx = openedStream.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0;
                    bool psp = openedStream.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0;
                    if (psp || psx)
                    {
                        bool expanded = psx && AllSprites.DetectExpansionOfPsxIso( openedStream );

                        if (currentStream != null)
                        {
                            currentStream.Flush();
                            currentStream.Close();
                            currentStream.Dispose();
                        }
                        currentStream = openedStream;

                        AllSprites s = AllSprites.FromIso( currentStream, false );
                        allSpritesEditor1.BindTo( s, currentStream );
                        tabControl1.Enabled = true;
                        spriteMenuItem.Enabled = true;
                        sp2Menu.Enabled = true;
                        menuItem_ExpandIso.Enabled = psx && !expanded;
                        menuItem_SavePatch.Enabled = true;
                        menuItem_ApplyPatch.Enabled = true;

                        AllOtherImages otherImages = AllOtherImages.FromIso(currentStream);
                        allOtherImagesEditor1.BindTo( otherImages, currentStream );

                        Text = string.Format(titleFormatString, versionString, Path.GetFileName(openFileDialog.FileName));
                    }
                    else
                    {
                        openedStream.Close();
                        openedStream.Dispose();
                        MyMessageBox.Show(this, "File not recognized as valid FFT disc image.", "Error", MessageBoxButtons.OK);
                    }
                }
            }
        }

        protected override void OnClosing( CancelEventArgs e )
        {
            if (currentStream != null)
            {
                currentStream.Flush();
                currentStream.Close();
                currentStream.Dispose();
                currentStream = null;
            }
            base.OnClosing( e );
        }

		// R999 reimportButton keypress event handler 
		private void reimportButtonKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar.Equals('r')) 
			{
                ReimportSpriteImage();
			}	
		}
		
		// R999
		private void reimportMenuItem_Click(object sender, EventArgs e)
		{
            ReimportSpriteImage();
		}

        private void importSprMenuItem_Click( object sender, EventArgs e )
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            openFileDialog.Filter = "FFT Sprite (*.SPR)|*.spr";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;
            if (currentSprite != null && openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                try
                {
                    currentSprite.ImportSprite( currentStream, openFileDialog.FileName );
                    allSpritesEditor1.ReloadCurrentSprite();
                }
                catch (SpriteTooLargeException ex)
                {
                    MyMessageBox.Show( this, ex.Message, "Error" );
                }
            }
        }

        private void exportSprMenuItem_Click( object sender, EventArgs e )
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            saveFileDialog.Filter = "FFT Sprite (*.SPR)|*.spr";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CreatePrompt = false;
            saveFileDialog.OverwritePrompt = true;
            if (currentSprite != null && saveFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                File.WriteAllBytes( saveFileDialog.FileName, currentSprite.GetAbstractSpriteFromIso( currentStream ).ToByteArray( 0 ) );
            }
        }

        private void menuItem_ImportSpriteImage_Click( object sender, EventArgs e )
        {
            ImportSpriteImage();
        }

        private void menuItem_ExportSpriteImage_Click( object sender, EventArgs e )
        {
            ExportSpriteImage();
        }

        private void exitMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private void importSp2MenuItem_Click( object sender, EventArgs e )
        {
            int index = Int32.Parse( (sender as Menu).Tag.ToString() );
            MonsterSprite sprite = allSpritesEditor1.CurrentSprite.GetAbstractSpriteFromIso( currentStream ) as MonsterSprite;
            if (sprite != null)
            {
                openFileDialog.Filter = "SP2 files (*.SP2)|*.sp2";
                openFileDialog.FileName = string.Empty;
                openFileDialog.CheckFileExists = true;
                if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
                {
                    (allSpritesEditor1.CurrentSprite as CharacterSprite).ImportSp2( currentStream, openFileDialog.FileName, index - 1 );
                    allSpritesEditor1.ReloadCurrentSprite();
                }
            }
        }

        private void exportSp2MenuItem_Click( object sender, EventArgs e )
        {
            int index = Int32.Parse( (sender as Menu).Tag.ToString() );
            MonsterSprite sprite = allSpritesEditor1.CurrentSprite.GetAbstractSpriteFromIso( currentStream ) as MonsterSprite;
            if (sprite != null)
            {
                saveFileDialog.Filter = "SP2 files (*.SP2)|*.sp2";
                saveFileDialog.FileName = string.Empty;
                saveFileDialog.CreatePrompt = false;
                saveFileDialog.OverwritePrompt = true;
                if (saveFileDialog.ShowDialog( this ) == DialogResult.OK)
                {
                    File.WriteAllBytes( saveFileDialog.FileName, sprite.ToByteArray( index ) );
                }
            }
        }

        private void spriteMenuItem_Popup(object sender, EventArgs e)
        {
            bool enableMenuItems = ((allSpritesEditor1.CurrentSprite != null) && (allSpritesEditor1.CurrentSprite.Size > 0));

            foreach (MenuItem mi in spriteMenuItem.MenuItems)
            {
                mi.Enabled = enableMenuItems;
            } 
        }

        private void sp2Menu_Popup( object sender, EventArgs e )
        {
            foreach (MenuItem mi in sp2Menu.MenuItems)
            {
                mi.Enabled = false;
            }

            if (allSpritesEditor1.CurrentSprite != null)
            {
                MonsterSprite sprite = allSpritesEditor1.CurrentSprite.GetAbstractSpriteFromIso(currentStream) as MonsterSprite;
                if (sprite != null && allSpritesEditor1.CurrentSprite is CharacterSprite)
                {
                    int numChildren = (allSpritesEditor1.CurrentSprite as CharacterSprite).NumChildren;
                    for (int i = 0; i < numChildren; i++)
                    {
                        sp2Menu.MenuItems[i * 3].Enabled = true;
                        sp2Menu.MenuItems[i * 3 + 1].Enabled = true;
                    }
                }
            }
        }

        private void importImageMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.FileName = string.Empty;

            string fileFilter = allOtherImagesEditor1.GetCurrentImageInputFileFilter();
            if (string.IsNullOrEmpty(fileFilter))
            {
                const string allImagesFilter = "All images (*.png, *.gif, *.jpg, *.bmp, *.tiff)|*.png;*.gif;*.jpg;*.jpeg;*.bmp;*.tiff;*.tif";
                const string pngFilter = "PNG images (*.png)|*.png";
                const string gifFilter = "GIF images (*.gif)|*.gif";
                const string jpgFilter = "JPEG images (*.jpg)|*.jpg;*.jpeg";
                const string bmpFilter = "Bitmap images (*.bmp)|*.bmp";
                const string tifFilter = "TIFF images (*.tiff)|*.tif;*.tiff";
                fileFilter = string.Join( "|", new string[] { allImagesFilter, pngFilter, gifFilter, jpgFilter, bmpFilter, tifFilter } );
            }
            openFileDialog.Filter = fileFilter;

            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                allOtherImagesEditor1.LoadToCurrentImage( openFileDialog.FileName );
            }
        }

        private void exportImageMenuItem_Click( object sender, EventArgs e )
        {
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.Filter = allOtherImagesEditor1.GetCurrentImageFileFilter();

            if (saveFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                string filepath = saveFileDialog.FileName;
                string extension = Path.GetExtension(filepath);
                ImageFormat imageFormat = ImageFormatHelper.GetFormat(extension);
                allOtherImagesEditor1.SaveCurrentImage( saveFileDialog.FileName, imageFormat );
            }
        }

        private void menuItem_ImportEntireFile_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;
            string origFilename = allOtherImagesEditor1.GetCurrentOriginalFilename();
            int startIndex = origFilename.LastIndexOf(".") + 1;
            string extension = origFilename.Substring(startIndex, origFilename.Length - startIndex).ToUpper();
            openFileDialog.Filter = origFilename + "|" + origFilename + "|" + extension + " files|*." + extension + "|All files|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                bool didImport = allOtherImagesEditor1.ImportEntireFile(openFileDialog.FileName);
                if (!didImport)
                {
                    MyMessageBox.Show(this, "Importing entire file is not implemented for this image type.", "Not Implemented", MessageBoxButtons.OK);
                }
            }
        }

        private void menuItem_ExportEntireFile_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = string.Empty;
            string origFilename = allOtherImagesEditor1.GetCurrentOriginalFilename();
            int startIndex = origFilename.LastIndexOf(".") + 1;
            string extension = origFilename.Substring(startIndex, origFilename.Length - startIndex).ToUpper();
            saveFileDialog.Filter = origFilename + "|" + origFilename + "|" + extension + " files|" + "*." + extension + "|All files|*.*";

            AbstractImage image = allOtherImagesEditor1.GetImageFromComboBoxItem();
            if (image.Filesize == 0)
            {
                MyMessageBox.Show(this, "Exporting entire file is not implemented for this image.", "Not Implemented", MessageBoxButtons.OK);
            }
            else if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                bool didExport = allOtherImagesEditor1.ExportEntireFile(saveFileDialog.FileName);
                if (!didExport)
                {
                    MyMessageBox.Show(this, "Exporting entire file is not implemented for this image.", "Not Implemented", MessageBoxButtons.OK);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged( object sender, EventArgs e )
        {
            bool image = tabControl1.SelectedTab == otherTabPage;
            spriteMenuItem.Visible = !image;
            sp2Menu.Visible = !image;
            imageMenuItem.Visible = image;

            if (image)
                allOtherImagesEditor1.RefreshPictureBox();
        }

        private void importAllImagesMenuItem_Click( object sender, EventArgs e )
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            Cursor oldCursor = Cursor;

            ProgressChangedEventHandler progressHandler = delegate( object sender2, ProgressChangedEventArgs args2 )
            {
                MethodInvoker mi = (() => progressBar1.Value = args2.ProgressPercentage);
                if (progressBar1.InvokeRequired)
                    progressBar1.Invoke( mi );
                else mi();
            };

            RunWorkerCompletedEventHandler completeHandler = null;

            completeHandler = delegate( object sender1, RunWorkerCompletedEventArgs args1 )
            {
                MethodInvoker mi = delegate()
                {
                    var result = args1.Result as AllOtherImages.AllImagesDoWorkResult;
                    tabControl1.Enabled = true;
                    progressBar1.Visible = false;
                    if (oldCursor != null) Cursor = oldCursor;
                    backgroundWorker1.RunWorkerCompleted -= completeHandler;
                    backgroundWorker1.ProgressChanged -= progressHandler;
                    backgroundWorker1.DoWork -= allOtherImagesEditor1.AllOtherImages.ImportAllImages;
                    allOtherImagesEditor1.RefreshPictureBox(true);

                    StringBuilder sb_ResultText = new StringBuilder();
                    if (!string.IsNullOrEmpty(result.ErrorText))
                        sb_ResultText.AppendLine(result.ErrorText);
                    sb_ResultText.AppendLine(string.Format("{0} images imported.", result.ImagesProcessed));

                    string caption = (result.DoWorkResult == AllOtherImages.AllImagesDoWorkResult.Result.Success) ? "Result" : "Failure";
                    MyMessageBox.Show( this, sb_ResultText.ToString(), caption, MessageBoxButtons.OK );
                };
                if (InvokeRequired) Invoke( mi );
                else mi();
            };

            //if (fbd.ShowDialog( this ) == DialogResult.OK)
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                progressBar1.Bounds = new Rectangle( ClientRectangle.Left + 10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width - 20, progressBar1.Height );
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                backgroundWorker1.DoWork += allOtherImagesEditor1.AllOtherImages.ImportAllImages;
                backgroundWorker1.ProgressChanged += progressHandler;
                backgroundWorker1.RunWorkerCompleted += completeHandler;
                backgroundWorker1.WorkerReportsProgress = true;
                tabControl1.Enabled = false;
                Cursor = Cursors.WaitCursor;
                progressBar1.BringToFront();
                //backgroundWorker1.RunWorkerAsync( new AllOtherImages.AllImagesDoWorkData( currentStream, fbd.SelectedPath ) );
                backgroundWorker1.RunWorkerAsync(new AllOtherImages.AllImagesDoWorkData(currentStream, ImageFormat.Bmp, dialog.FileName));
            }
        }

        private void menuItem_ExportAllImages_BMP_Click(object sender, EventArgs e)
        {
            ExportAllImages(ImageFormat.Bmp);
        }

        private void menuItem_ExportAllImages_PNG_Click(object sender, EventArgs e)
        {
            ExportAllImages(ImageFormat.Png);
        }

        private void importAllSpritesMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            Cursor oldCursor = Cursor;

            ProgressChangedEventHandler progressHandler = delegate(object sender2, ProgressChangedEventArgs args2)
            {
                MethodInvoker mi = (() => progressBar1.Value = args2.ProgressPercentage);
                if (progressBar1.InvokeRequired)
                    progressBar1.Invoke(mi);
                else mi();
            };

            RunWorkerCompletedEventHandler completeHandler = null;

            completeHandler = delegate(object sender1, RunWorkerCompletedEventArgs args1)
            {
                MethodInvoker mi = delegate()
                {
                    var result = args1.Result as AllSprites.AllSpritesDoWorkResult;
                    tabControl1.Enabled = true;
                    progressBar1.Visible = false;
                    if (oldCursor != null) Cursor = oldCursor;
                    backgroundWorker1.RunWorkerCompleted -= completeHandler;
                    backgroundWorker1.ProgressChanged -= progressHandler;
                    backgroundWorker1.DoWork -= allSpritesEditor1.Sprites.LoadAllSprites;
                    allSpritesEditor1.ReloadCurrentSprite();

                    StringBuilder sb_ResultText = new StringBuilder();
                    if (!string.IsNullOrEmpty(result.ErrorText))
                        sb_ResultText.AppendLine(result.ErrorText);
                    sb_ResultText.AppendLine(string.Format("{0} sprites imported.", result.ImagesProcessed));

                    string caption = (result.DoWorkResult == AllSprites.AllSpritesDoWorkResult.Result.Success) ? "Result" : "Failure";
                    MyMessageBox.Show(this, sb_ResultText.ToString(), caption, MessageBoxButtons.OK);
                };
                if (InvokeRequired) Invoke(mi);
                else mi();
            };

            //if (fbd.ShowDialog(this) == DialogResult.OK)
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                progressBar1.Bounds = new Rectangle(ClientRectangle.Left + 10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width - 20, progressBar1.Height);
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                backgroundWorker1.DoWork += allSpritesEditor1.Sprites.LoadAllSprites;
                backgroundWorker1.ProgressChanged += progressHandler;
                backgroundWorker1.RunWorkerCompleted += completeHandler;
                backgroundWorker1.WorkerReportsProgress = true;
                tabControl1.Enabled = false;
                Cursor = Cursors.WaitCursor;
                progressBar1.BringToFront();

                bool importExport8bpp = allSpritesEditor1.ImportExport8Bpp;
                int paletteIndex = importExport8bpp ? 0 : allSpritesEditor1.PaletteIndex;
                //backgroundWorker1.RunWorkerAsync(new AllSprites.AllSpritesDoWorkData(currentStream, fbd.SelectedPath, importExport8bpp, paletteIndex));
                backgroundWorker1.RunWorkerAsync(new AllSprites.AllSpritesDoWorkData(currentStream, ImageFormat.Bmp, dialog.FileName, importExport8bpp, paletteIndex));
            }
        }

        private void menuItem_ExportAllSprites_BMP_Click(object sender, EventArgs e)
        {
            ExportAllSprites(ImageFormat.Bmp);
        }

        private void menuItem_ExportAllSprites_PNG_Click(object sender, EventArgs e)
        {
            ExportAllSprites(ImageFormat.Png);
        }

        private void menuItem_ExpandIso_Click(object sender, EventArgs e)
        {
            bool isPsx = currentStream.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0;
            if (isPsx)
            {
                DialogResult result = DialogResult.None;
                bool expanded = AllSprites.DetectExpansionOfPsxIso(currentStream);
                if (!expanded &&
                    (result = MyMessageBox.Show(this, "Are you sure you want to Expand this ISO?", "Expand ISO?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                      == DialogResult.Yes)
                {
                    AllSprites.ExpandPsxIso(currentStream);

                    AllSprites s = AllSprites.FromIso(currentStream, true);
                    allSpritesEditor1.BindTo(s, currentStream);
                    tabControl1.Enabled = true;
                    spriteMenuItem.Enabled = true;
                    sp2Menu.Enabled = true;

                    AllOtherImages otherImages = AllOtherImages.FromIso(currentStream);
                    allOtherImagesEditor1.BindTo(otherImages, currentStream);

                    menuItem_ExpandIso.Enabled = false;
                }
            }
        }

        private void menuItem_SavePatch_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Patch file (*.shishipatch)|*.shishipatch";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                byte[] bytes = allSpritesEditor1.Sprites.GetShishiPatchBytes();
                File.WriteAllBytes(saveFileDialog.FileName, bytes);
            }
        }

        private void menuItem_ApplyPatch_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Patch file (*.shishipatch)|*.shishipatch";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);
                allSpritesEditor1.Sprites.ApplyShishiPatchBytes(currentStream, bytes);
                allSpritesEditor1.ReloadCurrentSprite();
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            //allSpritesEditor1.ReloadCurrentSprite(false);
            allOtherImagesEditor1.RefreshPictureBox();
        }
    }
}
