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

namespace FFTPatcher.SpriteEditor
{
    public partial class MainForm : Form
    {

        const string titleFormatString = "Shishi Sprite Manager - {0}";
		string _fileName = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private Stream currentStream = null;

        private void openIsoMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Stream openedStream = File.Open( openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite );
                if (openedStream != null)
                {
                    bool psx = openedStream.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0;
                    bool psp = openedStream.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0;
                    if (psp || psx)
                    {
                        DialogResult result = DialogResult.None;
                        bool expanded = psx && AllSprites.DetectExpansionOfPsxIso( openedStream );
                        if (psx && !expanded &&
                            (result = MyMessageBox.Show( this, "ISO needs to be restructured." + Environment.NewLine + "Restructure?", "Restructure ISO?", MessageBoxButtons.YesNoCancel ))
                              == DialogResult.Yes)
                        {
                            AllSprites.ExpandPsxIso( openedStream );
                        }
                        else if (psx && !expanded && result == DialogResult.Cancel)
                        {
                            openedStream.Close();
                            openedStream.Dispose();
                            return;
                        }

                        if (currentStream != null)
                        {
                            currentStream.Flush();
                            currentStream.Close();
                            currentStream.Dispose();
                        }
                        currentStream = openedStream;

                        bool userRequestedExpansion = psx && !expanded && result == DialogResult.Yes;
                        AllSprites s = AllSprites.FromIso( currentStream, userRequestedExpansion );
                        allSpritesEditor1.BindTo( s, currentStream );
                        tabControl1.Enabled = true;
                        spriteMenuItem.Enabled = true;
                        sp2Menu.Enabled = true;

                        var otherImages = AllOtherImages.FromIso( currentStream );
                        allOtherImagesEditor1.BindTo( otherImages, currentStream );

                        Text = string.Format( titleFormatString, Path.GetFileName( openFileDialog.FileName ) );
                    }
                }
            }
        }
        
        private void reimportButtonKeyPress(object sender, KeyPressEventArgs e) 
        { 
            
            if (e.KeyChar.Equals('r')) 
            { 
                 Sprite currentSprite = allSpritesEditor1.CurrentSprite; 
                 if (_fileName != null && currentSprite != null ) 
                    { 
                       currentSprite.ImportBitmap( currentStream, _fileName ); 
                       allSpritesEditor1.ReloadCurrentSprite(); 
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

        private void importSprMenuItem_Click( object sender, EventArgs e )
        {
			/* R999 instnat reimport function. Overrides Import SPR function.  
			// the win form menu item should now have a ShortcutKeys property set to Key.R
            Sprite currentSprite = allSpritesEditor1.CurrentSprite; 
            if (_fileName != null && currentSprite != null )
			{
				currentSprite.ImportBitmap( currentStream, _fileName );
            	allSpritesEditor1.ReloadCurrentSprite();
			}
 */
			
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

        private void Reimportbmp_Click(object sender, EventArgs e)
        {
            // R999 instnat reimport function. Overrides Import SPR function.  
            // the win form menu item should now have a ShortcutKeys property set to Key.R
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            if (_fileName != null && currentSprite != null)
            {
                currentSprite.ImportBitmap(currentStream, _fileName);
                allSpritesEditor1.ReloadCurrentSprite();
            }
        }

        private void importBmpMenuItem_Click( object sender, EventArgs e )
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            openFileDialog.Filter = "8bpp paletted bitmap (*.BMP)|*.bmp";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;
            if (currentSprite != null && openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
				_fileName = openFileDialog.FileName;
                currentSprite.ImportBitmap( currentStream, openFileDialog.FileName );
                allSpritesEditor1.ReloadCurrentSprite();
            }

        }

        private void exportBmpMenuItem_Click( object sender, EventArgs e )
        {
            Sprite currentSprite = allSpritesEditor1.CurrentSprite;
            saveFileDialog.Filter = "8bpp paletted bitmap (*.BMP)|*.bmp";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CreatePrompt = false;
            saveFileDialog.FileName = string.Empty;

            if (currentSprite != null && saveFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                currentSprite.GetAbstractSpriteFromIso( currentStream ).ToBitmap().Save( saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp );
            }
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

        private void sp2Menu_Popup( object sender, EventArgs e )
        {
            foreach (MenuItem mi in sp2Menu.MenuItems)
            {
                mi.Enabled = false;
            }

            MonsterSprite sprite = allSpritesEditor1.CurrentSprite.GetAbstractSpriteFromIso( currentStream ) as MonsterSprite;
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

        private void importImageMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.FileName = string.Empty;
            const string allImagesFilter = "All images (*.png, *.gif, *.jpg, *.bmp, *.tiff)|*.png;*.gif;*.jpg;*.jpeg;*.bmp;*.tiff;*.tif";
            const string pngFilter = "PNG images (*.png)|*.png";
            const string gifFilter = "GIF images (*.gif)|*.gif";
            const string jpgFilter = "JPEG images (*.jpg)|*.jpg;*.jpeg";
            const string bmpFilter = "Bitmap images (*.bmp)|*.bmp";
            const string tifFilter = "TIFF images (*.tiff)|*.tif;*.tiff";
            openFileDialog.Filter = string.Join( "|", new string[] { allImagesFilter, pngFilter, gifFilter, jpgFilter, bmpFilter, tifFilter } );

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
                allOtherImagesEditor1.SaveCurrentImage( saveFileDialog.FileName );
            }
        }

        private void tabControl1_SelectedIndexChanged( object sender, EventArgs e )
        {
            bool image = tabControl1.SelectedTab == otherTabPage;
            spriteMenuItem.Visible = !image;
            sp2Menu.Visible = !image;
            imageMenuItem.Visible = image;
        }

        private void importAllImagesMenuItem_Click( object sender, EventArgs e )
        {
            using ( Ionic.Utils.FolderBrowserDialogEx fbd = new Ionic.Utils.FolderBrowserDialogEx() )
            {
                fbd.ShowNewFolderButton = true;
                fbd.ShowFullPathInEditBox = true;
                fbd.ShowEditBox = true;
                fbd.ShowBothFilesAndFolders = false;
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.NewStyle = true;
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
                        backgroundWorker1.DoWork -= allOtherImagesEditor1.AllOtherImages.LoadAllImages;
                        MyMessageBox.Show( this, string.Format( "{0} images imported", result.ImagesProcessed ), result.DoWorkResult.ToString(), MessageBoxButtons.OK );
                    };
                    if (InvokeRequired) Invoke( mi );
                    else mi();
                };

                if (fbd.ShowDialog( this ) == DialogResult.OK)
                {
                    progressBar1.Bounds = new Rectangle( ClientRectangle.Left + 10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width - 20, progressBar1.Height );
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;
                    backgroundWorker1.DoWork += allOtherImagesEditor1.AllOtherImages.LoadAllImages;
                    backgroundWorker1.ProgressChanged += progressHandler;
                    backgroundWorker1.RunWorkerCompleted += completeHandler;
                    backgroundWorker1.WorkerReportsProgress = true;
                    tabControl1.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    progressBar1.BringToFront();
                    backgroundWorker1.RunWorkerAsync( new AllOtherImages.AllImagesDoWorkData( currentStream, fbd.SelectedPath ) );
                }
            }
        }

        private void dumpAllImagesMenuItem_Click( object sender, EventArgs e )
        {
            using ( Ionic.Utils.FolderBrowserDialogEx fbd = new Ionic.Utils.FolderBrowserDialogEx() )
            {
                fbd.ShowNewFolderButton = true;
                fbd.ShowFullPathInEditBox = true;
                fbd.ShowEditBox = true;
                fbd.ShowBothFilesAndFolders = false;
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.NewStyle = true;
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
                        backgroundWorker1.DoWork -= allOtherImagesEditor1.AllOtherImages.DumpAllImages;
                        MyMessageBox.Show(this, string.Format( "{0} images saved", result.ImagesProcessed ), result.DoWorkResult.ToString(), MessageBoxButtons.OK );
                    };
                    if (InvokeRequired) Invoke( mi );
                    else mi();
                };

                if (fbd.ShowDialog( this ) == DialogResult.OK)
                {
                    progressBar1.Bounds = new Rectangle( ClientRectangle.Left+10, (ClientRectangle.Height - progressBar1.Height) / 2, ClientRectangle.Width-20, progressBar1.Height );
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;
                    backgroundWorker1.DoWork += allOtherImagesEditor1.AllOtherImages.DumpAllImages;
                    backgroundWorker1.ProgressChanged += progressHandler;
                    backgroundWorker1.RunWorkerCompleted += completeHandler;
                    backgroundWorker1.WorkerReportsProgress = true;
                    tabControl1.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    progressBar1.BringToFront();
                    backgroundWorker1.RunWorkerAsync( new AllOtherImages.AllImagesDoWorkData( currentStream, fbd.SelectedPath ) );
                    //backgroundWorker1.RunWorkerCompleted

                    //allOtherImagesEditor1.AllOtherImages.DumpAllImages( currentStream, fbd.SelectedPath );
                }
            }
        }

    }
}
