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
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using ASMEncoding;
using ASMEncoding.Helpers;

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

            //lsb_FilesList.Scrollable = true;
            //lsb_FilesList.View = View.List;
            //lsb_FilesList.HeaderStyle = ColumnHeaderStyle.None;
            //lsb_FilesList.Columns.Add("", -2, HorizontalAlignment.Left);
        }

        void variableComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
            	AsmPatch patch = ( clb_Patches.SelectedItem as AsmPatch );
            	//Byte[] byteArray = patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes();
                //VariableType selectedVariable = patch.Variables[variableComboBox.SelectedIndex];
                VariableType selectedVariable = patch.VariableMap[(string)variableComboBox.SelectedItem];

                byte[] byteArray = selectedVariable.byteArray;
                
                // Setting Maximum can trigger the variableSpinner_ValueChanged event, but we don't want to change the variable value here, so set ignoreChanges = true before setting Maximum.
                ignoreChanges = true;
                variableSpinner.Maximum = (decimal)Math.Pow(256, selectedVariable.numBytes) - 1;
                ignoreChanges = false;

                variableSpinner.Value = AsmPatch.GetUnsignedByteArrayValue_LittleEndian(byteArray);
            }
        }

        void variableSpinner_ValueChanged( object sender, EventArgs e )
        {
        	AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
            if ( !ignoreChanges )
            {
                UInt32 newValue = (UInt32)variableSpinner.Value;
                //VariableType variable = patch.Variables[variableComboBox.SelectedIndex];
                VariableType variable = patch.VariableMap[(string)variableComboBox.SelectedItem];
                UpdateVariable(variable, newValue);
            }
        }

        private void UpdateVariable(VariableType variable, UInt32 newValue)
        {
            for (int i = 0; i < variable.numBytes; i++)
            {
                //patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes()[i] = (Byte)((def >> (i * 8)) & 0xff);
                byte byteValue = (byte)((newValue >> (i * 8)) & 0xff);
                variable.byteArray[i] = byteValue;
                foreach (PatchedByteArray patchedByteArray in variable.content)
                {
                    patchedByteArray.GetBytes()[i] = byteValue;
                }
            }
        }

        private void UpdateReferenceVariableValues(AsmPatch patch)
        {
            foreach (VariableType variable in patch.Variables)
            {
                if (variable.isReference)
                    UpdateReferenceVariableValue(variable, patch);
            }
        }

        private void UpdateReferenceVariableValue(VariableType variable, AsmPatch patch)
        {
            if (variable.isReference)
            {
                byte[] referenceBytes = patch.VariableMap[variable.reference.name].byteArray;
                uint value = AsmPatch.GetUnsignedByteArrayValue_LittleEndian(referenceBytes);

                switch (variable.reference.operatorSymbol)
                {
                    case "+":
                        value += variable.reference.operand;
                        break;
                    case "-":
                        value -= variable.reference.operand;
                        break;
                    case "*":
                        value *= variable.reference.operand;
                        break;
                    case "/":
                        value /= variable.reference.operand;
                        break;
                }

                UpdateVariable(variable, value);
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
            lsb_FilesList.BackColors = new Color[files.Length + 1];
            lsb_FilesList.Items.Add("All");
            lsb_FilesList.SelectedIndices.Clear();
            //LoadFile(files[lsb_FilesList.FocusedItem.Index]);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                lsb_FilesList.Items.Add(files[i]);

                lsb_FilesList.BackColors[i + 1] = Color.White;
                if (!Patchlist.LoadedCorrectly[i])
                    //lsb_FilesList.Items[i + 1].BackColor = Color.Red;
                    lsb_FilesList.BackColors[i + 1] = Color.Red;
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


            int selectedIndex = lsb_FilesList.SelectedIndex;

            if (lsb_FilesList.SelectedItem == null)
                return;
            //if ALL
            if (selectedIndex == 0)
            {
                LoadPatches(Patchlist.AllPatches);

                for (int i = 0; i < clb_Patches.Items.Count; i++)
                {
                    if (selectedIndex == 0)
                    {

                        if (Patchlist.AllCheckStates[i] == CheckState.Checked)
                        {
                            clb_Patches.SetItemChecked(i, true);
                            somethingchecked = true;
                        }
                    }
                    else
                    {
                        if (Patchlist.FilePatches[selectedIndex].PatchCheckStates[i] == CheckState.Checked)
                        {
                            clb_Patches.SetItemChecked(i, true);
                            somethingchecked = true;
                        }

                    }
                }
            }
            else //if NOT ALL
            {

                if (Patchlist.FilePatches[selectedIndex - 1] != null)
                {
                    LoadPatches(Patchlist.FilePatches[selectedIndex - 1].Patches);


                    for (int i = 0; i < Patchlist.FilePatches[selectedIndex - 1].PatchCheckStates.Length; i++)
                    {
                        if (clb_Patches.Items.Count > i)
                        {
                            if (Patchlist.FilePatches[selectedIndex - 1].PatchCheckStates[i] == CheckState.Checked)
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
                    PatcherLib.MyMessageBox.Show(this, "Error", lsb_FilesList.SelectedItem + " did not load correctly!", MessageBoxButtons.OK);   
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
                
                //if (p.Variables.Count > 0)
                if (p.CountNonReferenceVariables() > 0)
                {
                    ignoreChanges = true;
                    variableComboBox.Items.Clear();
                    //p.Variables.ForEach(varType => variableComboBox.Items.Add(varType.name));
                    bool foundFirst = false;
                    VariableType firstNonReferenceVariable = p.Variables[0];
                    foreach (VariableType variable in p.Variables)
                    {
                        if (!variable.isReference)
                        {
                            variableComboBox.Items.Add(variable.name);
                            if (!foundFirst)
                            {
                                firstNonReferenceVariable = variable;
                                foundFirst = true;
                            }
                        }
                    }
                    variableComboBox.SelectedIndex = 0;

                    //variableSpinner.Value = p.Variables[0].content.Value.GetBytes()[0];
                    //Byte[] byteArray = p.Variables[0].content.Value.GetBytes();
                    //byte[] byteArray = p.Variables[0].byteArray;
                    byte[] byteArray = firstNonReferenceVariable.byteArray;
                    variableSpinner.Maximum = (decimal)Math.Pow(256, firstNonReferenceVariable.numBytes) - 1;
                    variableSpinner.Value = AsmPatch.GetUnsignedByteArrayValue_LittleEndian(byteArray);

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
            if ((skipchecked) || (lsb_FilesList.SelectedItem == null))
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


            int selectedIndex = lsb_FilesList.SelectedIndex;

            //if NOT ALL
            if (selectedIndex != 0)
            {
                //Set indiivdual list
                Patchlist.FilePatches[selectedIndex - 1].PatchCheckStates[e.Index] = e.NewValue;
                //Set Master List
                Patchlist.SetMasterListCheckState(selectedIndex - 1, e.Index, e.NewValue);
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
                        if (byteArrayIndex >= asmPatch.NonVariableCount)
                            break;

                        if (!string.IsNullOrEmpty(patchedByteArray.ErrorText))
                        {
                            color = Color.FromArgb(225, 125, 125);
                            sbMessage.Append(patchedByteArray.ErrorText);
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

        private void ModifyPatch(AsmPatch patch)
        {
            UpdateReferenceVariableValues(patch);
            foreach (PatchedByteArray patchedByteArray in patch)
            {
                if (patchedByteArray.IsAsm)
                {
                    string encodeContent = patchedByteArray.AsmText;
                    string strPrefix = "";
                    IList<VariableType> variables = patch.Variables;

                    foreach (PatchedByteArray currentPatchedByteArray in patch)
                    {
                        if (!string.IsNullOrEmpty(currentPatchedByteArray.Label))
                            strPrefix += String.Format(".label @{0}, {1}\r\n", currentPatchedByteArray.Label, currentPatchedByteArray.RamOffset);
                    }
                    foreach (VariableType variable in variables)
                    {
                        strPrefix += String.Format(".eqv %{0}, {1}\r\n", ASMStringHelper.RemoveSpaces(variable.name).Replace(",", ""), AsmPatch.GetUnsignedByteArrayValue_LittleEndian(variable.byteArray));
                    }

                    encodeContent = strPrefix + patchedByteArray.AsmText;
                    patchedByteArray.SetBytes(asmUtility.EncodeASM(encodeContent, (uint)patchedByteArray.RamOffset).EncodedBytes);
                }
            }
        }

        private List<AsmPatch> GetCurrentFilePatches()
        {
            List<AsmPatch> resultList = new List<AsmPatch>();
            int selectedFileIndex = lsb_FilesList.SelectedIndex;

            if (selectedFileIndex <= 0)
            {
                for (int index = 0; index < Patchlist.FilePatches.Length; index++)
                {
                    resultList.AddRange(Patchlist.FilePatches[index].Patches);
                }
            }
            else
            {
                resultList = Patchlist.FilePatches[selectedFileIndex - 1].Patches;
            }

            return resultList;
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
                        ModifyPatch(patch);
                        PatcherLib.Iso.PsxIso.PatchPsxIso( file, patch );
                    }
                }
            }

            PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
        }
        private void PatchSaveStbutton_Click(object sender, EventArgs e)
        {
            //Patchbutton copy. Modify to patch byte array right to savestate.
            saveFileDialog1.Filter = "PSV files (*.psv)|*.psv|All files (*.*)|*.*";
            saveFileDialog1.FileName = string.Empty;

            StringBuilder sbResultMessage = new StringBuilder();

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                //byte[] filecopy = File.ReadAllBytes(saveFileDialog1.FileName);

                using (BinaryReader reader = new BinaryReader(File.Open(saveFileDialog1.FileName, FileMode.Open)))
                {
                    List<PatchedByteArray> patches = new List<PatchedByteArray>();
                    foreach (AsmPatch asmPatch in clb_Patches.CheckedItems)
                    {
                        ModifyPatch(asmPatch);
                        //patchList.AddRange(patch);
                        foreach (PatchedByteArray innerPatch in asmPatch)
                        {
                            patches.Add(innerPatch);
                        }
                    }

                    PatchPsxSaveStateResult patchResult = PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patches);

                    sbResultMessage.AppendLine("Complete!");
                    sbResultMessage.AppendLine();
                    if (patchResult.UnsupportedFiles.Count > 0)
                    {
                        sbResultMessage.AppendLine("Files not supported for savestate patching:");
                        foreach (PsxIso.Sectors sector in patchResult.UnsupportedFiles)
                        {
                            sbResultMessage.AppendFormat("\t{0}{1}", Enum.GetName(typeof(PsxIso.Sectors), sector), Environment.NewLine);
                        }
                        sbResultMessage.AppendLine();
                    }
                    if (patchResult.AbsentFiles.Count > 0)
                    {
                        sbResultMessage.AppendLine("Files not present in savestate:");
                        foreach (PsxIso.Sectors sector in patchResult.AbsentFiles)
                        {
                            sbResultMessage.AppendFormat("\t{0}{1}", Enum.GetName(typeof(PsxIso.Sectors), sector), Environment.NewLine);
                        }
                        sbResultMessage.AppendLine();
                    }

                    /*
                    foreach (AsmPatch patch in clb_Patches.CheckedItems)
                    {
                        PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patch);
                    }
                    */
                }
            }

            PatcherLib.MyMessageBox.Show(this, sbResultMessage.ToString(), "Complete!", MessageBoxButtons.OK);
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
            FreeSpaceForm freeSpaceForm = new FreeSpaceForm(GetCurrentFilePatches());
            freeSpaceForm.Show();
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
                        //AllPatches.AddRange(tryPatches);
                        foreach (AsmPatch patch in tryPatches)
                        {
                            if (!patch.HideInDefault)
                                AllPatches.Add(patch);
                        }

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
