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
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib;

namespace FFTPatcher
{
    public partial class MainForm : Form
    {
        #region Instance Variables

        private PatchPSPForm patchPspForm = null;
        private PatchPSXForm patchPsxForm = null;
        private FFTPatch fftPatch = null;

        #endregion Instance Variables

        #region Private Properties

        private PatchPSPForm PatchPSPForm
        {
            get
            {
                if (patchPspForm == null)
                {
                    patchPspForm = new PatchPSPForm();
                }
                return patchPspForm;
            }
        }

        private PatchPSXForm PatchPSXForm
        {
            get
            {
                if (patchPsxForm == null)
                {
                    patchPsxForm = new PatchPSXForm();
                }
                return patchPsxForm;
            }
        }

        private FFTPatch FFTPatch
        {
            get
            {
                if (fftPatch == null)
                {
                    fftPatch = new FFTPatch();
                }
                return fftPatch;
            }
        }

        #endregion Private Properties

        #region Constructors (1)

        public MainForm(string[] args)
        {
            InitializeComponent();

            string inputFilepath = PatcherLib.Utilities.Utilities.GetInputFilepath(args, ".fftpatch");
            
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            Settings.LoadSettingsXml();

            Text = string.Format("FFTPatcher (v0.{0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision);

            openMenuItem.Click += openMenuItem_Click;
            saveMenuItem.Click += saveMenuItem_Click;
            saveAsPspMenuItem.Click += saveAsPspMenuItem_Click;
            newPSPMenuItem.Click += newPSPMenuItem_Click;
            newPSXMenuItem.Click += newPSXMenuItem_Click;
            exitMenuItem.Click += exitMenuItem_Click;
            aboutMenuItem.Click += aboutMenuItem_Click;
            saveMenuItem.Enabled = false;
            saveAsPspMenuItem.Enabled = false;
            menuItem_SavePatchXML.Enabled = false;

            extractFFTPackMenuItem.Click += extractFFTPackMenuItem_Click;
            rebuildFFTPackMenuItem.Click += rebuildFFTPackMenuItem_Click;
            decryptMenuItem.Click += decryptMenuItem_Click;

            patchPspIsoMenuItem.Click += patchPspIsoMenuItem_Click;
            patchPsxIsoMenuItem.Click += patchPsxIsoMenuItem_Click;
            cheatdbMenuItem.Click += cheatdbMenuItem_Click;
            openPatchedPsxIso.Click += new EventHandler( openPatchedPsxIso_Click );

            fileMenuItem.Popup += new EventHandler( fileMenuItem_Popup );
            editMenuItem.Popup += new EventHandler(editMenuItem_Popup);
            psxMenu.Popup += new EventHandler( psxMenu_Popup );
            pspMenu.Popup += new EventHandler( pspMenu_Popup );

            if (!string.IsNullOrEmpty(inputFilepath))
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(inputFilepath);
                TryAndHandle(delegate()
                {
                    FFTPatch.LoadPatch(inputFilepath);
                    fftPatchEditor1.LoadFFTPatch(FFTPatch);
                }, true);
            }
        }

        #endregion Constructors

        #region Public Methods (1)

        public void HandleException( Exception e )
        {
            string message = string.Empty;
            if (e is FFTPatcher.Datatypes.FFTPatch.LoadPatchException)
            {
                message = "Could not load patch";
            }
            else if (e is ArgumentNullException)
            {
                message = "Argument was null";
            }
            else if (e is ArgumentException)
            {
                message = "Bad argument";
            }
            else if (e is FileNotFoundException)
            {
            }
            else if (e is IOException)
            {
                message = "IO error occurred";
            }
            else if (e is System.Security.SecurityException)
            {
                message = "Security access";
            }
            else if (e is DirectoryNotFoundException)
            {
                message = "Folder not found";
            }
            else if (e is UnauthorizedAccessException)
            {
                message = "Incorrect permissions";
            }
            else
            {
                message = e.ToString();
            }

            MyMessageBox.Show( this, string.Format( "An error of type {0} occurred:\n{1}", e.GetType(), message ), "Error", MessageBoxButtons.OK );
        }

        #endregion Public Methods

        #region Private Methods

        private void aboutMenuItem_Click( object sender, EventArgs e )
        {
            About a = new About();
            a.ShowDialog( this );
        }

        private void cheatdbMenuItem_Click( object sender, EventArgs e )
        {
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "CWCheat DB files|cheat.db";
            if (saveFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( saveFileDialog.FileName );

                TryAndHandle( delegate() { Codes.SaveToFile( saveFileDialog.FileName, FFTPatch ); }, false );
            }
        }

        private void decryptMenuItem_Click( object sender, EventArgs e )
        {
            applyPatchOpenFileDialog.Filter = "War of the Lions ISO images (*.iso)|*.iso";
            if (applyPatchOpenFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( applyPatchOpenFileDialog.FileName );
                TryAndHandle( delegate() { PatcherLib.Iso.PspIso.DecryptISO( applyPatchOpenFileDialog.FileName ); }, false );
            }
        }

        private void exitMenuItem_Click( object sender, System.EventArgs e )
        {
            this.Close();
        }

        private void extractFFTPackMenuItem_Click( object sender, EventArgs e )
        {
            using (Ionic.Utils.FolderBrowserDialogEx folderBrowserDialog = new Ionic.Utils.FolderBrowserDialogEx())
            {
                DoWorkEventHandler doWork =
                    delegate( object sender1, DoWorkEventArgs args )
                    {
                        FFTPack.DumpToDirectory( openFileDialog.FileName, folderBrowserDialog.SelectedPath, sender1 as BackgroundWorker );
                    };
                ProgressChangedEventHandler progress =
                    delegate( object sender2, ProgressChangedEventArgs args )
                    {
                        progressBar.Visible = true;
                        progressBar.Value = Math.Min( args.ProgressPercentage, 100 );
                    };
                RunWorkerCompletedEventHandler completed = null;
                completed =
                    delegate( object sender3, RunWorkerCompletedEventArgs args )
                    {
                        progressBar.Visible = false;
                        Enabled = true;
                        patchPsxBackgroundWorker.ProgressChanged -= progress;
                        patchPsxBackgroundWorker.RunWorkerCompleted -= completed;
                        patchPsxBackgroundWorker.DoWork -= doWork;
                        if (args.Error is Exception)
                        {
                            MyMessageBox.Show( this,
                                "Could not extract file.\n" +
                                "Make sure you chose the correct file and that there \n" +
                                "enough room in the destination directory.",
                                "Error", MessageBoxButtons.OK );
                        }
                    };

                openFileDialog.Filter = "fftpack.bin|fftpack.bin|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                folderBrowserDialog.NewStyle = true;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.SelectedPath = Environment.CurrentDirectory;
                folderBrowserDialog.ShowBothFilesAndFolders = false;
                folderBrowserDialog.ShowEditBox = true;
                folderBrowserDialog.ShowFullPathInEditBox = false;
                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.Description = "Where should the files be extracted?";

                if ((openFileDialog.ShowDialog( this ) == DialogResult.OK) && (folderBrowserDialog.ShowDialog( this ) == DialogResult.OK))
                {
                    patchPsxBackgroundWorker.ProgressChanged += progress;
                    patchPsxBackgroundWorker.RunWorkerCompleted += completed;
                    patchPsxBackgroundWorker.DoWork += doWork;

                    Environment.CurrentDirectory = folderBrowserDialog.SelectedPath;

                    Enabled = false;
                    progressBar.Value = 0;
                    progressBar.Visible = true;
                    progressBar.BringToFront();
                    patchPsxBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        void fileMenuItem_Popup( object sender, EventArgs e )
        {
            openMenuItem.Enabled = true;
            saveAsPspMenuItem.Enabled = fftPatchEditor1.Enabled && (FFTPatch.Context == Context.US_PSX);
            saveMenuItem.Enabled = fftPatchEditor1.Enabled;
            menuItem_SavePatchXML.Enabled = fftPatchEditor1.Enabled;
        }

        private void editMenuItem_Popup(object sender, EventArgs e)
        {
            bool isEnabled = fftPatchEditor1.Enabled;
            menuItem_SetCurrentDataAsDefaults.Enabled = isEnabled;
            menuItem_RestoreDefaults.Enabled = isEnabled;
            menuItem_ConsolidateItemAttributes.Enabled = isEnabled;
            menuItem_ConsolidateInflictStatuses.Enabled = isEnabled;
        }

        private void newPSPMenuItem_Click( object sender, System.EventArgs e )
        {
            FFTPatch.BuildFromContext(Context.US_PSP);
            fftPatchEditor1.LoadFFTPatch(FFTPatch);
        }

        private void newPSXMenuItem_Click( object sender, System.EventArgs e )
        {
            FFTPatch.BuildFromContext(Context.US_PSX);
            fftPatchEditor1.LoadFFTPatch(FFTPatch);
        }

        private void openMenuItem_Click( object sender, System.EventArgs e )
        {
            openFileDialog.Filter = "FFTPatcher files (*.fftpatch)|*.fftpatch";
            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( openFileDialog.FileName );

                TryAndHandle( delegate() {
                    FFTPatch.LoadPatch(openFileDialog.FileName);
                    fftPatchEditor1.LoadFFTPatch(FFTPatch);
                }, true );
            }
        }

        private void openPatchedPspItem_Click( object sender, EventArgs e )
        {
            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "ISO images|*.iso;";
            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( openFileDialog.FileName );

                TryAndHandle( delegate() {
                    FFTPatch.OpenPatchedPspIso(openFileDialog.FileName);
                    fftPatchEditor1.LoadFFTPatch(FFTPatch);
                }, true );
            }
        }

        private void openPatchedPsxIso_Click( object sender, EventArgs e )
        {
            openFileDialog.Filter = "ISO images (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
            if (openFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( openFileDialog.FileName );
                TryAndHandle( delegate() {
                    FFTPatch.OpenPatchedPsxIso(openFileDialog.FileName);
                    fftPatchEditor1.LoadFFTPatch(FFTPatch);
                }, true );
            }
        }

        private void patchPspIsoMenuItem_Click( object sender, EventArgs e )
        {
            DoWorkEventHandler doWork =
                delegate( object sender1, DoWorkEventArgs args )
                {
                    PspIso.PatchISO( FFTPatch, sender1 as BackgroundWorker, args, PatchPSPForm );
                };
            ProgressChangedEventHandler progress =
                delegate( object sender2, ProgressChangedEventArgs args )
                {
                    progressBar.Visible = true;
                    progressBar.Value = Math.Max( progressBar.Minimum, Math.Min( args.ProgressPercentage, progressBar.Maximum ) );
                };
            RunWorkerCompletedEventHandler completed = null;
            completed =
                delegate( object sender3, RunWorkerCompletedEventArgs args )
                {
                    progressBar.Visible = false;
                    Enabled = true;
                    patchPsxBackgroundWorker.ProgressChanged -= progress;
                    patchPsxBackgroundWorker.RunWorkerCompleted -= completed;
                    patchPsxBackgroundWorker.DoWork -= doWork;
                    if (args.Error is NotSupportedException)
                    {
                        MyMessageBox.Show( this, "File is not a recognized War of the Lions ISO image.", "Error", MessageBoxButtons.OK );
                    }
                    else if (args.Error != null)
                    {
                        HandleException( args.Error );
                    }
                };

            if (PatchPSPForm.CustomShowDialog( this, FFTPatch ) == DialogResult.OK)
            {
                patchPsxBackgroundWorker.ProgressChanged += progress;
                patchPsxBackgroundWorker.RunWorkerCompleted += completed;
                patchPsxBackgroundWorker.DoWork += doWork;

                Enabled = false;

                progressBar.Value = 0;
                progressBar.Visible = true;

                patchPsxBackgroundWorker.RunWorkerAsync();
            }
        }

        private void patchPsxIsoMenuItem_Click( object sender, EventArgs e )
        {
            PatchPSXForm.patchISO = true;
            DoWorkEventHandler doWork =
                delegate( object sender1, DoWorkEventArgs args )
                {
                    PsxIso.PatchPsxIso( FFTPatch, sender1 as BackgroundWorker, args, PatchPSXForm );
                };
            ProgressChangedEventHandler progress =
                delegate( object sender2, ProgressChangedEventArgs args )
                {
                    progressBar.Visible = true;
                    progressBar.Value = Math.Max( progressBar.Minimum, Math.Min( args.ProgressPercentage, progressBar.Maximum ) );
                };
            RunWorkerCompletedEventHandler completed = null;
            completed =
                delegate( object sender3, RunWorkerCompletedEventArgs args )
                {
                    progressBar.Visible = false;
                    Enabled = true;
                    patchPsxBackgroundWorker.ProgressChanged -= progress;
                    patchPsxBackgroundWorker.RunWorkerCompleted -= completed;
                    patchPsxBackgroundWorker.DoWork -= doWork;

                    if (args.Error != null)
                    {
                        HandleException( args.Error );
                    }
                };


            if (PatchPSXForm.CustomShowDialog( this, FFTPatch ) == DialogResult.OK)
            {
                patchPsxBackgroundWorker.ProgressChanged += progress;
                patchPsxBackgroundWorker.RunWorkerCompleted += completed;
                patchPsxBackgroundWorker.DoWork += doWork;

                Enabled = false;

                progressBar.Value = 0;
                progressBar.Visible = true;

                patchPsxBackgroundWorker.RunWorkerAsync();
            }
        }

        void pspMenu_Popup( object sender, EventArgs e )
        {
            patchPspIsoMenuItem.Enabled = fftPatchEditor1.Enabled && FFTPatch.Context == Context.US_PSP;
            cheatdbMenuItem.Enabled = fftPatchEditor1.Enabled && FFTPatch.Context == Context.US_PSP;
        }

        void psxMenu_Popup( object sender, EventArgs e )
        {
            openPatchedPsxIso.Enabled = true;
            patchPsxIsoMenuItem.Enabled = fftPatchEditor1.Enabled && FFTPatch.Context == Context.US_PSX;
            menuItem_PatchPSXSavestate.Enabled = fftPatchEditor1.Enabled && (FFTPatch.Context == Context.US_PSX);
        }

        private void rebuildFFTPackMenuItem_Click( object sender, EventArgs e )
        {
            using (Ionic.Utils.FolderBrowserDialogEx folderBrowserDialog = new Ionic.Utils.FolderBrowserDialogEx())
            {
                DoWorkEventHandler doWork =
                    delegate( object sender1, DoWorkEventArgs args )
                    {
                        FFTPack.MergeDumpedFiles( folderBrowserDialog.SelectedPath, saveFileDialog.FileName, sender1 as BackgroundWorker );
                    };
                ProgressChangedEventHandler progress =
                    delegate( object sender2, ProgressChangedEventArgs args )
                    {
                        progressBar.Visible = true;
                        progressBar.Value = args.ProgressPercentage;
                    };
                RunWorkerCompletedEventHandler completed = null;
                completed =
                    delegate( object sender3, RunWorkerCompletedEventArgs args )
                    {
                        progressBar.Visible = false;
                        Enabled = true;
                        patchPsxBackgroundWorker.ProgressChanged -= progress;
                        patchPsxBackgroundWorker.RunWorkerCompleted -= completed;
                        patchPsxBackgroundWorker.DoWork -= doWork;
                        if (args.Error is Exception)
                        {
                            MyMessageBox.Show( this,
                                "Could not merge files.\n" +
                                "Make sure you chose the correct file and that there is\n" +
                                "enough room in the destination directory.",
                                "Error", MessageBoxButtons.OK );
                        }
                    };

                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = "fftpack.bin|fftpack.bin|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 0;
                folderBrowserDialog.Description = "Where are the extracted files?";
                folderBrowserDialog.NewStyle = true;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.SelectedPath = Environment.CurrentDirectory;
                folderBrowserDialog.ShowBothFilesAndFolders = false;
                folderBrowserDialog.ShowEditBox = true;
                folderBrowserDialog.ShowFullPathInEditBox = false;
                folderBrowserDialog.ShowNewFolderButton = false;

                if ((folderBrowserDialog.ShowDialog( this ) == DialogResult.OK) && (saveFileDialog.ShowDialog( this ) == DialogResult.OK))
                {
                    patchPsxBackgroundWorker.ProgressChanged += progress;
                    patchPsxBackgroundWorker.RunWorkerCompleted += completed;
                    patchPsxBackgroundWorker.DoWork += doWork;

                    Environment.CurrentDirectory = folderBrowserDialog.SelectedPath;
                    Enabled = false;
                    progressBar.Value = 0;
                    progressBar.Visible = true;
                    progressBar.BringToFront();
                    patchPsxBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void saveAsPspMenuItem_Click( object sender, EventArgs e )
        {
            string fn = SavePatch( false );
            if (!string.IsNullOrEmpty( fn ))
            {
                // HACK
                if (FFTPatch.Context != Context.US_PSP)
                {
                    FFTPatch.ConvertPsxPatchToPsp( fn );
                    FFTPatch.LoadPatch( fn );
                    fftPatchEditor1.LoadFFTPatch(FFTPatch);
                }
            }
        }

        private void saveMenuItem_Click( object sender, System.EventArgs e )
        {
            SavePatch( false );
        }

        private string SavePatch( bool digest )
        {
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "FFTPatcher files (*.fftpatch)|*.fftpatch";
            if (saveFileDialog.ShowDialog( this ) == DialogResult.OK)
            {
                Environment.CurrentDirectory = Path.GetDirectoryName( saveFileDialog.FileName );
                try
                {
                    TryAndHandle( delegate() { FFTPatch.SavePatchToFile( saveFileDialog.FileName, FFTPatch.Context, digest ); }, false, true );
                    return saveFileDialog.FileName;
                }
                catch (Exception)
                {
                    MyMessageBox.Show( this, "Could not open file.", "File not found", MessageBoxButtons.OK );
                }
            }
            return string.Empty;
        }

        private void TryAndHandle( MethodInvoker action, bool disableOnFail, bool rethrow )
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (disableOnFail)
                {
                    fftPatchEditor1.Enabled = false;
                }
                if (rethrow)
                {
                    throw;
                }
                else
                {
                    HandleException( e );
                }
            }
        }

        private void TryAndHandle( MethodInvoker action, bool disableOnFail )
        {
            TryAndHandle( action, disableOnFail, false );
        }

        #endregion Private Methods

        private void generateResourcesMenuItem_Click( object sender, EventArgs e )
        {
            using (Ionic.Utils.FolderBrowserDialogEx fbd = new Ionic.Utils.FolderBrowserDialogEx())
            {
                fbd.Description = "Where to save Resources.zip:";
                fbd.NewStyle = true;
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.SelectedPath = Path.GetDirectoryName( Application.ExecutablePath );
                fbd.ShowBothFilesAndFolders = false;
                fbd.ShowEditBox = true;
                fbd.ShowFullPathInEditBox = false;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog( this ) == DialogResult.OK)
                {
                    PatcherLib.ResourcesClass.GenerateDefaultResourcesZip( Path.Combine( fbd.SelectedPath, "Resources.zip" ) );
                }
            }
        }

        private void menuItem_PatchPSXSaveState_Click(object sender, EventArgs e)
        {
            PatchPSXForm.patchISO = false;
            DoWorkEventHandler doWork =
                delegate( object sender1, DoWorkEventArgs args )
                {
                    PsxIso.PatchPsxSavestate( FFTPatch, sender1 as BackgroundWorker, args, PatchPSXForm );
                };
            ProgressChangedEventHandler progress =
                delegate( object sender2, ProgressChangedEventArgs args )
                {
                    progressBar.Visible = true;
                    progressBar.Value = Math.Max( progressBar.Minimum, Math.Min( args.ProgressPercentage, progressBar.Maximum ) );
                };
            RunWorkerCompletedEventHandler completed = null;
            completed =
                delegate( object sender3, RunWorkerCompletedEventArgs args )
                {
                    progressBar.Visible = false;
                    Enabled = true;
                    patchPsxBackgroundWorker.ProgressChanged -= progress;
                    patchPsxBackgroundWorker.RunWorkerCompleted -= completed;
                    patchPsxBackgroundWorker.DoWork -= doWork;

                    if (args.Error != null)
                    {
                        HandleException( args.Error );
                    }
                };

            if (PatchPSXForm.CustomShowDialog( this, FFTPatch ) == DialogResult.OK)
            {
                patchPsxBackgroundWorker.ProgressChanged += progress;
                patchPsxBackgroundWorker.RunWorkerCompleted += completed;
                patchPsxBackgroundWorker.DoWork += doWork;

                Enabled = false;

                progressBar.Value = 0;
                progressBar.Visible = true;

                patchPsxBackgroundWorker.RunWorkerAsync();
            }
        
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            ////Patchbutton copy. Modify to patch byte array right to savestate.
            //saveFileDialog1.Filter = "PSV files (*.psv)|*.psv;*";
            //saveFileDialog1.FileName = string.Empty;
            //if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            //{
            //    byte[] filecopy = File.ReadAllBytes(saveFileDialog1.FileName);
            //    using (BinaryReader b = new BinaryReader(File.Open(saveFileDialog1.FileName, FileMode.Open)))
            //    {
            //        foreach (AsmPatch patch in clb_Patches.CheckedItems)
            //        {
            //            PatcherLib.Iso.PsxIso.PatchPsxSaveState(b, patch, filecopy);
            //        }
            //    }
            //}
        }

        private void menuItem_SetCurrentDataAsDefaults_Click(object sender, EventArgs e)
        {
            FFTPatch.SetCurrentDataAsDefault();
            fftPatchEditor1.LoadFFTPatch(FFTPatch);
        }

        private void menuItem_RestoreDefaults_Click(object sender, EventArgs e)
        {
            FFTPatch.RestoreDefaults();
            fftPatchEditor1.LoadFFTPatch(FFTPatch);
        }

        private void menuItem_ConsolidateItemAttributes_Click(object sender, EventArgs e)
        {
            if ((MyMessageBox.Show(this, "Are you sure you want to Consolidate Item Attributes?  This will repoint all duplicate entries.", "Consolidate Item Attributes?", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) == DialogResult.Yes)
            {
                fftPatchEditor1.ConsolidateItemAttributes();
            }
        }

        private void menuItem_ConsolidateInflictStatuses_Click(object sender, EventArgs e)
        {
            if ((MyMessageBox.Show(this, "Are you sure you want to Consolidate Inflict Statuses?  This will repoint all duplicate entries.", "Consolidate Inflict Statuses?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) == DialogResult.Yes)
            {
                fftPatchEditor1.ConsolidateInflictStatuses();
            }
        }

        private void menuItem_SavePatchXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = string.Empty;
            dialog.CheckFileExists = false;
            dialog.OverwritePrompt = true;
            dialog.Filter = "XML files (*.xml)|*.xml";

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string xml = null;
                    if (fftPatch.Context == Context.US_PSX)
                        xml = PsxIso.CreatePatchXML(fftPatch);
                    else if (fftPatch.Context == Context.US_PSP)
                        xml = PspIso.CreatePatchXML(fftPatch);

                    File.WriteAllText(dialog.FileName, xml, System.Text.Encoding.UTF8);
                    PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                }
                catch (Exception)
                {
                    MyMessageBox.Show(this, "Could not save patch XML.", "Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}