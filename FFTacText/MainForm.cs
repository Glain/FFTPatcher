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
using System.IO;
using System.Windows.Forms;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using PatcherLib;

namespace FFTPatcher.TextEditor
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            exitMenuItem.Click += exitMenuItem_Click;
            aboutMenuItem.Click += aboutMenuItem_Click;
            allowedSymbolsMenuItem.Click += allowedSymbolsMenuItem_Click;
            importPsxIsoMenuItem.Click += new EventHandler( importPsxIsoMenuItem_Click );
            importPspIsoMenuItem.Click += new EventHandler( importPspIsoMenuItem_Click );
            saveMenuItem.Click += new EventHandler( saveMenuItem_Click );
            openMenuItem.Click += new EventHandler( openMenuItem_Click );
            menuItem2.Click += new EventHandler( menuItem2_Click );
            generateResourcesZipMenuItem.Click += new EventHandler(generateResourcesZipMenuItem_Click);
            fileMenuItem.Popup += new EventHandler(menuItem_Popup);
            isoMenuItem.Popup += new EventHandler(menuItem_Popup);
        }

        private void GenerateResourcesZip()
        {
            saveFileDialog.FileName = "Resources.zip";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "ZIP file (*.zip)|*.zip";
            saveFileDialog.CheckFileExists = false;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                internalFile.GenerateResourcesZip(saveFileDialog.FileName);
            }
        }

        void menuItem_Popup(object sender, EventArgs e)
        {
            fileEditor1.Validate(false);
        }

        void menuItem2_Click( object sender, EventArgs e )
        {
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.Filter = "ISO files (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
            saveFileDialog.CheckFileExists = true;
            if ( saveFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += internalFile.BuildAndApplyPatches;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( worker_RunWorkerCompleted );
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                ProgressForm f = new ProgressForm();
                IList<ISerializableFile> files = new List<ISerializableFile>( internalFile.Files.Count );
                internalFile.Files.FindAll( g => g is ISerializableFile ).ForEach( g => files.Add( (ISerializableFile)g ) );
                f.BuildNodes( files );
                f.DoWork( this, worker,
                    new FFTText.PatchIsoArgs
                    {
                        Patcher = internalFile.Filetype == Context.US_PSP ? (FFTText.PatchIso)PatcherLib.Iso.PspIso.PatchISO : (FFTText.PatchIso)PatcherLib.Iso.PsxIso.PatchPsxIso,
                        Filename = saveFileDialog.FileName
                    } );
            }
        }

        void generateResourcesZipMenuItem_Click(object sender, EventArgs e)
        {
            GenerateResourcesZip();
        }

        void worker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            worker.DoWork -= internalFile.BuildAndApplyPatches;
        }

        void openMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "FFTText files (*.ffttext)|*.ffttext";
            if ( openFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                LoadFile( LoadType.Open, openFileDialog.FileName, null, null );
            }
        }

        enum LoadType
        {
            Open,
            PspFilename,
            PsxFilename
        }

        private void LoadFile( LoadType loadType, string filename, Stream isoStream, Stream tblStream )
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            DialogResult missingFilesResult = DialogResult.No;
            string missingFilesIsoFilename = null;
            MethodInvoker missingPrompt = null;
            missingPrompt = delegate()
                {
                    var res = MyMessageBox.Show( this, "Some files are missing." + Environment.NewLine + "Load missing files from ISO?", "Files missing", MessageBoxButtons.YesNoCancel );
                    if (res == DialogResult.Yes)
                    {
                        openFileDialog.Filter = "ISO files (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
                        openFileDialog.FileName = string.Empty;
                        if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
                        {
                            missingFilesIsoFilename = openFileDialog.FileName;
                        }
                        else
                        {
                            missingPrompt();
                        }
                    }
                    missingFilesResult = res;
                };
            worker.DoWork +=
                delegate( object sender, DoWorkEventArgs args )
                {
                    FFTText text = null;
                    switch ( loadType )
                    {
                        case LoadType.Open:
                            Set<Guid> missing = FFTTextFactory.DetectMissingGuids( filename );
                            if (missing.Count > 0)
                            {
                                if (InvokeRequired) Invoke( missingPrompt );
                                else missingPrompt();
                                if (missingFilesResult == DialogResult.Yes)
                                {
                                    using (Stream missingStream = File.OpenRead( missingFilesIsoFilename ))
                                    {
                                        text = FFTTextFactory.GetFilesXml( filename, worker, missing, missingStream );
                                    }
                                }
                                else if (missingFilesResult == DialogResult.Cancel)
                                {
                                    text = null;
                                }
                                else if (missingFilesResult == DialogResult.No)
                                {
                                    text = FFTTextFactory.GetFilesXml( filename, worker );
                                }
                            }
                            else
                            {
                                text = FFTTextFactory.GetFilesXml( filename, worker );
                            }
                            break;
                        case LoadType.PspFilename:
                            text = FFTText.ReadPSPIso( filename, worker );
                            break;
                        case LoadType.PsxFilename:
                            text = FFTText.ReadPSXIso( filename, worker );
                            break;
                    }
                    if ( text == null || worker.CancellationPending )
                    {
                        args.Cancel = true;
                        return;
                    }

                    LoadFile( text );
                };
            MethodInvoker enableForm =
                delegate()
                {
                    fileMenuItem.Enabled = true;
                    isoMenuItem.Enabled = true;
                    textMenuItem.Enabled = true;
                    fileEditor1.Enabled = true;
                    helpMenuItem.Enabled = true;
                    generateResourcesZipMenuItem.Enabled = true;
                    Cursor = Cursors.Default;
                };
            worker.RunWorkerCompleted +=
                delegate( object sender, RunWorkerCompletedEventArgs args )
                {
                    if ( args.Error != null )
                    {
                        MyMessageBox.Show( this, "Error loading file: " + args.Error.Message, "Error", MessageBoxButtons.OK );
                    }
                    if ( InvokeRequired )
                    {
                        Invoke( enableForm );
                    }
                    else
                    {
                        enableForm();
                    }
                };

            fileMenuItem.Enabled = false;
            isoMenuItem.Enabled = false;
            textMenuItem.Enabled = false;
            fileEditor1.Enabled = false;
            helpMenuItem.Enabled = false;
            generateResourcesZipMenuItem.Enabled = false;
            Cursor = Cursors.WaitCursor;
            worker.RunWorkerAsync();
        }

        void saveMenuItem_Click( object sender, EventArgs e )
        {
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.Filter = "FFTText files (*.ffttext)|*.ffttext";
            if ( saveFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                FFTTextFactory.WriteXml( internalFile, saveFileDialog.FileName );
            }
        }

        void importPspIsoMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "ISO files (*.iso)|*.iso";
            openFileDialog.FileName = string.Empty;
            if ( openFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                LoadFile( LoadType.PspFilename, openFileDialog.FileName, null, null );
            }
        }

        void importPsxIsoMenuItem_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "ISO files (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
            openFileDialog.FileName = string.Empty;
            if ( openFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                LoadFile( LoadType.PsxFilename, openFileDialog.FileName, null, null );
            }
        }


        private FFTText internalFile;

        private void LoadFile( FFTText file )
        {
            MethodInvoker whatever = delegate()
            {
                internalFile = file;
                textMenuItem.MenuItems.Clear();
                foreach ( IFile ifile in file.Files )
                {
                    MenuItem mi = new MenuItem( ifile.DisplayName, fileClick );
                    mi.Tag = ifile;
                    textMenuItem.MenuItems.Add( mi );
                }

                fileClick( textMenuItem.MenuItems[0], EventArgs.Empty );
                textMenuItem.Enabled = true;
                saveMenuItem.Enabled = true;
                menuItem2.Enabled = true;
                allowedSymbolsMenuItem.Enabled = true;
                generateResourcesZipMenuItem.Enabled = true;
            };

            if ( this.InvokeRequired )
            {
                Invoke( whatever );
            }
            else
            {
                whatever();
            }
        }

        private void fileClick( object sender, EventArgs e )
        {
            MenuItem senderItem = sender as MenuItem;
            IFile file = senderItem.Tag as IFile;
            fileEditor1.BindTo( file );
            foreach ( MenuItem mi in senderItem.Parent.MenuItems )
            {
                mi.Checked = false;
            }
            senderItem.Checked = true;

        }

        private void aboutMenuItem_Click( object sender, EventArgs e )
        {
            using ( About a = new About() )
                a.ShowDialog( this );
        }

        private void allowedSymbolsMenuItem_Click( object sender, EventArgs e )
        {
            CharmapForm.Show( internalFile.CharMap );
        }

        private void exitMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }
    }
}