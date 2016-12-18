using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using PatcherLib.Utilities;
using ASMEncoding;
using PatcherLib.Datatypes;
using PatcherLib.Iso;

namespace FFTorgASM
{
    public partial class MainForm : Form
    {
        AsmPatch[] patches;
        private bool ignoreChanges = true;

        ASMEncodingUtility asmUtility;

        public PatchList Patchlist;
        public int numberoffiles;

        public bool skipchecked;

        //string[] PatchFiles;

        private string[] patchMessages;

        private HashSet<ASMCheckCondition> asmCheckConditions = new HashSet<ASMCheckCondition>()
        {
            ASMCheckCondition.LoadDelay,
            ASMCheckCondition.UnalignedOffset,
            ASMCheckCondition.MultCountdown,
            ASMCheckCondition.StackPointerOffset,
            ASMCheckCondition.BranchInBranchDelaySlot
        };

        public MainForm()
        {
            InitializeComponent();

            asmUtility = new ASMEncodingUtility(ASMEncodingMode.PSX);
            versionLabel.Text = string.Format( "v0.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString() );

            ReloadFiles();

            patchButton.Click += new EventHandler( patchButton_Click );
            reloadButton.Click += new EventHandler( reloadButton_Click );
            clb_Patches.ItemCheck += new ItemCheckEventHandler( clb_Patches_ItemCheck );
            patchButton.Enabled = false;
            clb_Patches.SelectedIndexChanged += new EventHandler( clb_Patches_SelectedIndexChanged );
            variableSpinner.ValueChanged += new EventHandler( variableSpinner_ValueChanged );
            variableComboBox.SelectedIndexChanged += new EventHandler(variableComboBox_SelectedIndexChanged);
        }

        void variableComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
            	AsmPatch patch = ( clb_Patches.SelectedItem as AsmPatch );
            	Byte[] byteArray = patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes();
            	variableSpinner.Maximum = (decimal)Math.Pow(256,patch.Variables[variableComboBox.SelectedIndex].bytes) - 1;
            	variableSpinner.Value = patch.GetUnsignedByteArrayValue_LittleEndian(byteArray);
            }
        }

        void variableSpinner_ValueChanged( object sender, EventArgs e )
        {
        	AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
            if ( !ignoreChanges )
            {
                UInt32 def = (UInt32)variableSpinner.Value;
                for (int i=0; i < patch.Variables[variableComboBox.SelectedIndex].bytes; i++)
                {
                	patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes()[i] = (Byte)((def >> (i * 8)) & 0xff);
                }
            }
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            ReloadFiles();
        }

        private void ReloadFiles()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "/XmlPatches", "*.xml", SearchOption.TopDirectoryOnly);

            lsb_FilesList.SelectedIndices.Clear();

            clb_Patches.Items.Clear();
            ClearCurrentPatch();

            Patchlist = new PatchList(files, asmUtility);
            //Patchlist.AllCheckedPatches = clb_Patches.CheckedItems;

            lsb_FilesList.Items.Clear();
            lsb_FilesList.Items.Add("All");
            lsb_FilesList.SelectedIndices.Clear();
            //LoadFile(files[lsb_FilesList.FocusedItem.Index]);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                lsb_FilesList.Items.Add(files[i]);

                if(!Patchlist.LoadedCorrectly[i])
                    lsb_FilesList.Items[i + 1].BackColor = Color.Red;
            }
            //LoadFiles( files );
            
            numberoffiles = files.Length;

            //lsb_FilesList.SelectedIndices.Add(0);
        }

        private void lsb_FilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            skipchecked = true;
            clb_Patches.ItemCheck -= new ItemCheckEventHandler(clb_Patches_ItemCheck);

            ClearCurrentPatch();

            bool somethingchecked = false;

            if (lsb_FilesList.FocusedItem == null)
                return;
            //if ALL
            if (lsb_FilesList.FocusedItem.Index == 0)
            {
                LoadPatches(Patchlist.AllPatches);

                for (int i = 0; i < clb_Patches.Items.Count; i++)
                {
                    if (lsb_FilesList.FocusedItem.Index == 0)
                    {

                        if (Patchlist.AllCheckStates[i] == CheckState.Checked)
                        {
                            clb_Patches.SetItemChecked(i, true);
                            somethingchecked = true;
                        }
                    }
                    else
                    {
                        if (Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index].PatchCheckStates[i] == CheckState.Checked)
                        {
                            clb_Patches.SetItemChecked(i, true);
                            somethingchecked = true;
                        }

                    }
                }
            }
            else //if NOT ALL
            {

                if (Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index - 1] != null)
                {
                    LoadPatches(Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index - 1].Patches);


                    for (int i = 0; i < Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index - 1].PatchCheckStates.Length; i++)
                    {
                        if (clb_Patches.Items.Count > i)
                        {
                            if (Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index - 1].PatchCheckStates[i] == CheckState.Checked)
                            {
                                clb_Patches.SetItemChecked(i, true);
                                somethingchecked = true;
                            }
                            else
                            {
                                clb_Patches.SetItemChecked(i, false);
                            }
                        }

                    }
                }
                else
                {
                    clb_Patches.Items.Clear();
                    MessageBox.Show(lsb_FilesList.FocusedItem.Text + " did not load correctly!");
                    
                }
            }
            clb_Patches.ItemCheck += new ItemCheckEventHandler(clb_Patches_ItemCheck);
            skipchecked = false;
            if (somethingchecked)
            {
                PatchSaveStbutton.Enabled = true;
                patchButton.Enabled = true;
            }
            else
            {
                PatchSaveStbutton.Enabled = false;
                patchButton.Enabled = false;
            }
            
        }

        void clb_Patches_SelectedIndexChanged( object sender, EventArgs e )
        {
            AsmPatch p = clb_Patches.SelectedItem as AsmPatch;
            if (p != null)
            {
                textBox1.Text = p.Description;

                int index = clb_Patches.SelectedIndex;
                txt_Messages.Text = (index >= 0) ? patchMessages[index] : "";
                
                if (p.Variables.Count > 0)
                {
                    ignoreChanges = true;
                    variableComboBox.Items.Clear();
                    p.Variables.ForEach(varType => variableComboBox.Items.Add(varType.content.Key));
                    variableComboBox.SelectedIndex = 0;

                    //variableSpinner.Value = p.Variables[0].content.Value.GetBytes()[0];
                    Byte[] byteArray = p.Variables[0].content.Value.GetBytes();
                    variableSpinner.Maximum = (decimal)Math.Pow(256, p.Variables[0].bytes) - 1;
                    variableSpinner.Value = p.GetUnsignedByteArrayValue_LittleEndian(byteArray);

                    variableSpinner.Visible = true;
                    ignoreChanges = false;
                    variableComboBox.Visible = true;
                }
                else
                {
                    variableSpinner.Visible = false;
                    ignoreChanges = true;
                    variableComboBox.Visible = false;
                }
            }
        }

        void clb_Patches_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if ((skipchecked) || (lsb_FilesList.FocusedItem == null))
                return;

            if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked &&
                !(clb_Patches.Items[e.Index] as AsmPatch).ValidatePatch())
            {
                e.NewValue = CheckState.Unchecked;
            }

            patchButton.Enabled = (clb_Patches.CheckedItems.Count > 0 || e.NewValue == CheckState.Checked) &&
                                  !(clb_Patches.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked);

            PatchSaveStbutton.Enabled = (clb_Patches.CheckedItems.Count > 0 || e.NewValue == CheckState.Checked) &&
                                 !(clb_Patches.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked);


            //if NOT ALL
            if (lsb_FilesList.FocusedItem.Index != 0)
            {
                //Set indiivdual list
                Patchlist.FilePatches[lsb_FilesList.FocusedItem.Index - 1].PatchCheckStates[e.Index] = e.NewValue;
                //Set Master List
                Patchlist.SetMasterListCheckState(lsb_FilesList.FocusedItem.Index - 1, e.Index, e.NewValue);
                //Patchlist.Files[lsb_FilesList.SelectedIndex - 1].CheckedPatches = clb_Patches.CheckedItems;
            }
            else //If ALL
            {
                int ALLindex = e.Index;
                int patchindex = ALLindex;

                Patchlist.SetPatchCheckState(ALLindex, e.NewValue);
                Patchlist.AllCheckStates[e.Index] = e.NewValue;
            }
        }

        private void LoadPatches( IList<AsmPatch> patches )
        {
            clb_Patches.Items.Clear();
            if (patches != null)
            {
                this.patches = patches.ToArray();
               
                clb_Patches.Items.AddRange(this.patches);
                //patchButton.Enabled = false;
            }

            CheckPatches();
        }

        private void CheckPatches()
        {
            List<Color> bgColors = new List<Color>();
            List<string> messages = new List<string>();

            for (int index = 0; index < clb_Patches.Items.Count; index++)
            {
                Color color = clb_Patches.BackColor;
                StringBuilder sbMessage = new StringBuilder();

                object objPatch = clb_Patches.Items[index];
                if (objPatch != null)
                {
                    AsmPatch asmPatch = (AsmPatch)objPatch;
                    int byteArrayIndex = 0;
                    foreach (PatchedByteArray patchedByteArray in asmPatch)
                    {
                        if (byteArrayIndex >= (asmPatch.Count - asmPatch.Variables.Count))
                            break;

                        bool canLoadBytes = true;
                        byte[] bytes = null;
                        try
                        {
                            bytes = patchedByteArray.GetBytes();
                        }
                        catch (Exception)
                        {
                            canLoadBytes = false;
                        }

                        if (canLoadBytes)
                        {
                            UInt32 ramOffset = 0;
                            try
                            {
                                PsxIso.FileToRamOffsets ftrOffset = (PsxIso.FileToRamOffsets)Enum.Parse(typeof(PsxIso.FileToRamOffsets), "OFFSET_"
                                    + Enum.GetName(typeof(PsxIso.Sectors), patchedByteArray.Sector));
                                ramOffset = (UInt32)patchedByteArray.Offset + (UInt32)ftrOffset;
                            }
                            catch (Exception) { }

                            bool markedAsData = asmPatch.isDataSectionList[byteArrayIndex];
                            if (!markedAsData)
                            {
                                ASMCheckResult result = asmUtility.CheckASMFromBytes(bytes, ramOffset, true, false, asmCheckConditions);
                                if (result.IsASM)
                                {
                                    if (!string.IsNullOrEmpty(result.ErrorText))
                                    {
                                        color = Color.FromArgb(225, 125, 125);
                                        sbMessage.Append(result.ErrorText);
                                    }
                                }
                            }
                        }

                        byteArrayIndex++;
                    }
                }

                bgColors.Add(color);
                messages.Add(sbMessage.ToString());
            }

            clb_Patches.BackColors = bgColors.ToArray();
            patchMessages = messages.ToArray();
        }

        private void LoadFiles(IList<string> files)
        {
            List<AsmPatch> result = new List<AsmPatch>();
            foreach ( string file in files )
            {
                IList<AsmPatch> tryPatches;
                if ( PatchXmlReader.TryGetPatches( File.ReadAllText( file, Encoding.UTF8 ), file, asmUtility, out tryPatches ) )
                {
                    result.AddRange( tryPatches );
                }
                else
                {
                   // MessageBox.Show(file.Substring(file.LastIndexOf("\\")) + " Did not load correctly");
                }
            }
            LoadPatches( result );
        }

        private void ClearCurrentPatch()
        {
            textBox1.Text = "";
            txt_Messages.Text = "";
            variableSpinner.Visible = false;
            ignoreChanges = true;
            variableComboBox.Visible = false;
        }

        void patchButton_Click( object sender, EventArgs e )
        {
            saveFileDialog1.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            saveFileDialog1.FileName = string.Empty;
            if ( saveFileDialog1.ShowDialog( this ) == DialogResult.OK )
            {
                using ( Stream file = File.Open( saveFileDialog1.FileName, FileMode.Open, FileAccess.ReadWrite ) )
                {
                    foreach ( AsmPatch patch in clb_Patches.CheckedItems )
                    {
                        PatcherLib.Iso.PsxIso.PatchPsxIso( file, patch );
                    }
                }
            }
        }
        private void PatchSaveStbutton_Click(object sender, EventArgs e)
        {
            //Patchbutton copy. Modify to patch byte array right to savestate.
            saveFileDialog1.Filter = "PSV files (*.psv)|*.psv|All files (*.*)|*.*";
            saveFileDialog1.FileName = string.Empty;

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                //byte[] filecopy = File.ReadAllBytes(saveFileDialog1.FileName);

                using (BinaryReader reader = new BinaryReader(File.Open(saveFileDialog1.FileName, FileMode.Open)))
                {
                    List<PatchedByteArray> patches = new List<PatchedByteArray>();
                    foreach (AsmPatch innerPatches in clb_Patches.CheckedItems)
                    {
                        //patchList.AddRange(patch);
                        foreach (PatchedByteArray innerPatch in innerPatches)
                        {
                            patches.Add(innerPatch);
                        }
                    }

                    PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patches);

                    /*
                    foreach (AsmPatch patch in clb_Patches.CheckedItems)
                    {
                        PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patch);
                    }
                    */
                }
            }
        }

        private void clb_Patches_DragEnter( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
                if ( files.Length >= 1 && System.IO.File.Exists( files[0] ) )
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void clb_Patches_DragDrop( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] paths = (string[])e.Data.GetData( DataFormats.FileDrop );
                var filesToLoad = paths.FindAll( s => System.IO.File.Exists( s ) );
                LoadFiles( filesToLoad );
            }
        }

        private void checkAllButton_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < clb_Patches.Items.Count; i++ )
            {
                // never check a FileAsmPatch
                if ( !( clb_Patches.Items[i] is FileAsmPatch ) )
                    clb_Patches.SetItemChecked( i, true );
            }
        }

        private void toggleButton_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < clb_Patches.Items.Count; i++ )
            {
                // never check a FileAsmPatch
                if ( !( clb_Patches.Items[i] is FileAsmPatch ) || clb_Patches.GetItemChecked( i ) )
                    clb_Patches.SetItemChecked( i, !clb_Patches.GetItemChecked( i ) );
            }
        }

        private void btn_OpenConflictChecker_Click(object sender, EventArgs e)
        {
            ConflictChecker C = new ConflictChecker();
            C.Show();
        }

        private void btn_ViewFreeSpace_Click(object sender, EventArgs e)
        {
            FreeSpace FS = new FreeSpace();
            FS.Show();
        }

        public class PatchList
        {
            public class PatchFile
            {
                public string filename;
                public CheckedListBox.CheckedItemCollection FileCheckedPatches;
                public CheckState[] PatchCheckStates;
                public List<AsmPatch> Patches = new List<AsmPatch>();

                public PatchFile(int count)
                {
                    PatchCheckStates = new CheckState[count];

                    for (int i = 0; i < PatchCheckStates.Length; i++)
                    {
                        PatchCheckStates[i] = CheckState.Unchecked;
                    }
                }
            }

            public PatchFile[] FilePatches;
            public List<AsmPatch> AllPatches = new List<AsmPatch>();
            public CheckState[] AllCheckStates;
            public CheckedListBox.CheckedItemCollection AllCheckedPatches;
            public bool[] LoadedCorrectly;

            public PatchList(string[] files, ASMEncodingUtility asmUtility)
            {
                FilePatches = new PatchFile[files.Length];
                LoadedCorrectly = new bool[files.Length];
                IList<AsmPatch> tryPatches;

                int i = 0;
                foreach (string file in files)
                {
                    if (PatchXmlReader.TryGetPatches(File.ReadAllText(file, Encoding.UTF8), file, asmUtility, out tryPatches))
                    {

                        AllPatches.AddRange(tryPatches);

                        FilePatches[i] = new PatchFile(tryPatches.Count);
                        FilePatches[i].filename = file;
                        FilePatches[i].Patches.AddRange(tryPatches);
                        LoadedCorrectly[i] = true;
                    }
                    else
                    {
                        LoadedCorrectly[i] = false;
                        //MessageBox.Show(file.Substring(file.LastIndexOf("\\")) + " Did not load correctly");
                    }
                    i++;
                }

                AllCheckStates = new CheckState[AllPatches.Count];
                for (int j = 0; j < AllCheckStates.Length; j++)
                {
                    AllCheckStates[j] = new CheckState();
                    AllCheckStates[j] = CheckState.Unchecked;
                }
            }

            public void SetAllCheckStates()
            {
                int i = 0;
                foreach (PatchFile file in FilePatches)
                {
                    if (file != null)
                    {
                        foreach (CheckState check in file.PatchCheckStates)
                        {
                            AllCheckStates[i] = check;
                            i++;
                        }
                    }

                }
            }
            //saves in allcheckstates array
            public void SetMasterListCheckState(int FileIndex, int patchindex, CheckState check)
            {

                for (int i = 0; i < AllCheckStates.Length; i++)
                {
                    if (FilePatches[i] != null)
                    {
                        if (FileIndex > i)
                        {
                            patchindex += FilePatches[i].PatchCheckStates.Length;
                        }
                        else
                        {
                            AllCheckStates[patchindex] = check;
                            return;
                        }
                    }
                }


            }

            //saves in the individual files array of patch states
            public void SetPatchCheckState(int index, CheckState check)
            {
                for (int i = 0; i < FilePatches.Length; i++)
                {
                    if (FilePatches[i] != null)
                    {
                        if (FilePatches[i].PatchCheckStates.Length - 1 < index)
                        {

                            index -= FilePatches[i].PatchCheckStates.Length;
                        }
                        else
                        {
                            FilePatches[i].PatchCheckStates[index] = check;
                            return;
                        }
                    }

                }
            }
        }
    }
}
