using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
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
        private ASMEncodingUtility asmUtility;
        private PatchData patchData;
        private AsmPatch[] patches;

        private bool ignoreChanges = true;
        private bool skipCheckEventHandler = false;

        public MainForm()
        {
            InitializeComponent();
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            asmUtility = new ASMEncodingUtility(ASMEncodingMode.PSX);
            string versionText = string.Format("v0.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());
            versionLabel.Text = versionText;
            Text = string.Format("FFTorgASM ({0})", versionText);

            LoadFiles();
            FreeSpace.ReadFreeSpaceXML();

            btnPatch.Click += new EventHandler( btnPatch_Click );
            reloadButton.Click += new EventHandler( reloadButton_Click );
            clb_Patches.ItemCheck += new ItemCheckEventHandler( clb_Patches_ItemCheck );
            btnPatch.Enabled = false;
            clb_Patches.SelectedIndexChanged += new EventHandler( clb_Patches_SelectedIndexChanged );
            variableSpinner.ValueChanged += new EventHandler( variableSpinner_ValueChanged );
            variableComboBox.SelectedIndexChanged += new EventHandler(variableComboBox_SelectedIndexChanged);
        }

        private void UpdateVariable(VariableType variable, UInt32 newValue)
        {
            AsmPatch.UpdateVariable(variable, newValue);
        }

        private void LoadFile(int index)
        {
            clb_Patches.Items.Clear();
            ClearCurrentPatch();

            foreach (AsmPatch patch in patchData.FilePatches[index].Patches)
                patchData.SelectedPatches.Remove(patch);

            //patchData.CurrentSelectedPatches.Clear();
            
            patchData.ReloadFile(index, asmUtility);
            patchData.RebuildAllList();
            
            LoadFilePatches(index + 1);

            lsb_FilesList.SetBackColor(index + 1, patchData.LoadedCorrectly[index] ? Color.White : Color.FromArgb(225, 125, 125));
        }

        private void LoadFiles(IList<string> fileList = null)
        {
            string[] files = (fileList == null) ? Directory.GetFiles(Application.StartupPath + "/XmlPatches", "*.xml", SearchOption.TopDirectoryOnly) : fileList.ToArray();
            lsb_FilesList.SelectedIndices.Clear();

            clb_Patches.Items.Clear();
            ClearCurrentPatch();

            patchData = new PatchData(files, asmUtility);

            lsb_FilesList.Items.Clear();
            //lsb_FilesList.BackColors = new Color[files.Length + 1];
            lsb_FilesList.SetColorCapacity(files.Length + 1);
            lsb_FilesList.Items.Add("All");
            lsb_FilesList.SelectedIndices.Clear();

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                lsb_FilesList.Items.Add(files[i]);

                //lsb_FilesList.BackColors[i + 1] = Color.White;
                //lsb_FilesList.SetBackColor(i + 1, Color.White);
                if (!patchData.LoadedCorrectly[i])
                    //lsb_FilesList.Items[i + 1].BackColor = Color.Red;
                    //lsb_FilesList.BackColors[i + 1] = Color.Red;
                    lsb_FilesList.SetBackColor(i + 1, Color.FromArgb(225, 125, 125));
            }
        }

        private void LoadPatches(IList<AsmPatch> patches)
        {
            clb_Patches.Items.Clear();
            if (patches != null)
            {
                this.patches = patches.ToArray();

                clb_Patches.Items.AddRange(this.patches);
            }
        }

        private void ClearCurrentPatch()
        {
            textBox1.Text = "";
            txt_Messages.Text = "";
            variableSpinner.Visible = false;
            ignoreChanges = true;
            variableComboBox.Visible = false;
        }

        private void SavePatchXML()
        {
            //List<AsmPatch> patches = GetCurrentFileSelectedPatches();
            List<AsmPatch> patches = GetAllSelectedPatches();
            foreach (AsmPatch patch in patches)
            {
                patch.Update(asmUtility);
            }

            string xml = PatchXmlReader.CreatePatchXML(patches);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, xml, Encoding.UTF8);
                PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private List<AsmPatch> GetCurrentFilePatches()
        {
            List<AsmPatch> resultList = new List<AsmPatch>();
            int selectedFileIndex = lsb_FilesList.SelectedIndex;

            if (selectedFileIndex <= 0)
            {
                for (int index = 0; index < patchData.FilePatches.Length; index++)
                {
                    resultList.AddRange(patchData.FilePatches[index].Patches);
                }
            }
            else
            {
                resultList = patchData.FilePatches[selectedFileIndex - 1].Patches;
            }

            return resultList;
        }

        private List<AsmPatch> GetCurrentFileSelectedPatches()
        {
            int selectedFileIndex = lsb_FilesList.SelectedIndex;
            List<AsmPatch> asmPatchList = (selectedFileIndex <= 0) ? patchData.AllShownPatches : patchData.FilePatches[selectedFileIndex - 1].Patches;
            return GetSelectedPatchesFromList(asmPatchList);
        }

        private List<AsmPatch> GetAllSelectedPatches()
        {
            return GetSelectedPatchesFromList(patchData.AllShownPatches);
        }

        private List<AsmPatch> GetSelectedPatchesFromList(List<AsmPatch> asmPatchList)
        {
            List<AsmPatch> resultList = new List<AsmPatch>();

            foreach (AsmPatch patch in asmPatchList)
            {
                if (patchData.SelectedPatches.Contains(patch))
                    resultList.Add(patch);
            }

            return resultList;
        }

        private void LoadFilePatches(int selectedIndex)
        {
            ClearCurrentPatch();

            if (selectedIndex == 0)
            {
                LoadPatches(patchData.AllShownPatches);
                clb_Patches.BackColors = patchData.BackgroundColors[selectedIndex];
            }
            else if (!patchData.LoadedCorrectly[selectedIndex - 1])
            {
                clb_Patches.Items.Clear();
                PatcherLib.MyMessageBox.Show(this, lsb_FilesList.SelectedItem + " did not load correctly!", "Error", MessageBoxButtons.OK);
            }
            else
            {
                LoadPatches(patchData.FilePatches[selectedIndex - 1].Patches);
                clb_Patches.BackColors = patchData.BackgroundColors[selectedIndex];
            }

            //patchData.CurrentSelectedPatches = GetCurrentFileSelectedPatches();

            skipCheckEventHandler = true;
            for (int index = 0; index < clb_Patches.Items.Count; index++)
            {
                AsmPatch asmPatch = (AsmPatch)(clb_Patches.Items[index]);
                clb_Patches.SetItemChecked(index, patchData.SelectedPatches.Contains(asmPatch));
            }

            skipCheckEventHandler = false;

            //bool enablePatchButtons = (patchData.CurrentSelectedPatches.Count > 0);
            bool enablePatchButtons = (patchData.SelectedPatches.Count > 0);
            btnPatch.Enabled = enablePatchButtons;
            btnPatchSaveState.Enabled = enablePatchButtons;
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = lsb_FilesList.SelectedIndex;
            if (selectedIndex > 0)
                LoadFile(selectedIndex - 1);
            else
                LoadFiles();
        }

        private void variableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
                VariableType selectedVariable = patch.VariableMap[(string)variableComboBox.SelectedItem];

                byte[] byteArray = selectedVariable.byteArray;

                // Setting Maximum can trigger the variableSpinner_ValueChanged event, but we don't want to change the variable value here, so set ignoreChanges = true before setting Maximum.
                ignoreChanges = true;
                variableSpinner.Maximum = (decimal)Math.Pow(256, selectedVariable.numBytes) - 1;
                ignoreChanges = false;

                variableSpinner.Value = Utilities.GetUnsignedByteArrayValue_LittleEndian(byteArray);
            }
        }

        private void variableSpinner_ValueChanged(object sender, EventArgs e)
        {
            AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
            if (!ignoreChanges)
            {
                UInt32 newValue = (UInt32)variableSpinner.Value;
                VariableType variable = patch.VariableMap[(string)variableComboBox.SelectedItem];
                UpdateVariable(variable, newValue);
            }
        }

        private void lsb_FilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsb_FilesList.SelectedItem != null)
                LoadFilePatches(lsb_FilesList.SelectedIndex);
        }

        private void clb_Patches_SelectedIndexChanged(object sender, EventArgs e)
        {
            AsmPatch p = clb_Patches.SelectedItem as AsmPatch;
            if (p != null)
            {
                string description = "";
                if (!string.IsNullOrEmpty(p.Description))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] lines = p.Description.Split('\n');

                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        string line = lines[lineIndex];
                        string newLine = line.Trim().Replace("\r", "");
                        if (((lineIndex != 0) && (lineIndex < (lines.Length - 1))) || (!string.IsNullOrEmpty(newLine)))
                            sb.AppendLine(newLine);
                    }
                    description = sb.ToString();
                    description = description.Substring(0, description.LastIndexOf(Environment.NewLine));
                }

                textBox1.Text = description;

                int index = clb_Patches.SelectedIndex;
                txt_Messages.Text = p.ErrorText;
                
                //if (p.Variables.Count > 0)
                if (p.CountNonReferenceVariables() > 0)
                {
                    ignoreChanges = true;
                    variableComboBox.Items.Clear();
                    
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

                    byte[] byteArray = firstNonReferenceVariable.byteArray;
                    variableSpinner.Maximum = (decimal)Math.Pow(256, firstNonReferenceVariable.numBytes) - 1;
                    variableSpinner.Value = Utilities.GetUnsignedByteArrayValue_LittleEndian(byteArray);

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

        private void clb_Patches_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if ((skipCheckEventHandler) || (lsb_FilesList.SelectedItem == null))
                return;

            int selectedIndex = lsb_FilesList.SelectedIndex;
            AsmPatch asmPatch = clb_Patches.Items[e.Index] as AsmPatch;

            if ((e.CurrentValue == CheckState.Unchecked) && (e.NewValue == CheckState.Checked) && (!asmPatch.ValidatePatch()))
            {
                e.NewValue = CheckState.Unchecked;
            }

            if (e.NewValue == CheckState.Checked)
            {
                patchData.SelectedPatches.Add(asmPatch);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (patchData.SelectedPatches.Contains(asmPatch))
                    patchData.SelectedPatches.Remove(asmPatch);
            }

            //patchData.CurrentSelectedPatches = GetCurrentFileSelectedPatches();
            //bool enablePatchButtons = (patchData.CurrentSelectedPatches.Count > 0);
            bool enablePatchButtons = (patchData.SelectedPatches.Count > 0);
            btnPatch.Enabled = enablePatchButtons;
            btnPatchSaveState.Enabled = enablePatchButtons;
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            saveFileDialog1.FileName = string.Empty;
            if ( saveFileDialog1.ShowDialog( this ) == DialogResult.OK )
            {
                using (Stream file = File.Open(saveFileDialog1.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    List<AsmPatch> patches = GetAllSelectedPatches();
                    //foreach ( AsmPatch patch in clb_Patches.CheckedItems )
                    foreach (AsmPatch patch in patches)
                    {
                        //ModifyPatch(patch);
                        patch.Update(asmUtility);
                        PatcherLib.Iso.PsxIso.PatchPsxIso( file, patch );
                    }
                }

                PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private void btnPatchSaveState_Click(object sender, EventArgs e)
        {
            //Patchbutton copy. Modify to patch byte array right to savestate.
            saveFileDialog1.Filter = "PSV files (*.psv)|*.psv|All files (*.*)|*.*";
            saveFileDialog1.FileName = string.Empty;

            StringBuilder sbResultMessage = new StringBuilder();

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(saveFileDialog1.FileName, FileMode.Open)))
                {
                    List<PatchedByteArray> patches = new List<PatchedByteArray>();
                    List<AsmPatch> asmPatches = GetAllSelectedPatches();

                    //foreach (AsmPatch asmPatch in clb_Patches.CheckedItems)
                    foreach (AsmPatch asmPatch in asmPatches)
                    {
                        //ModifyPatch(asmPatch);
                        asmPatch.Update(asmUtility);
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
                            sbResultMessage.AppendFormat("\t{0}{1}", PsxIso.GetSectorName(sector), Environment.NewLine);
                        }
                        sbResultMessage.AppendLine();
                    }
                    if (patchResult.AbsentFiles.Count > 0)
                    {
                        sbResultMessage.AppendLine("Files not present in savestate:");
                        foreach (PsxIso.Sectors sector in patchResult.AbsentFiles)
                        {
                            sbResultMessage.AppendFormat("\t{0}{1}", PsxIso.GetSectorName(sector), Environment.NewLine);
                        }
                        sbResultMessage.AppendLine();
                    }
                }

                PatcherLib.MyMessageBox.Show(this, sbResultMessage.ToString(), "Complete!", MessageBoxButtons.OK);
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

        private void btn_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Patches.Items.Count; i++)
            {
                // never check a FileAsmPatch
                if (!(clb_Patches.Items[i] is FileAsmPatch) || clb_Patches.GetItemChecked(i))
                    clb_Patches.SetItemChecked(i, false);
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

        private void btn_SavePatchXML_Click(object sender, EventArgs e)
        {
            SavePatchXML();
        }

        private void btn_OpenConflictChecker_Click(object sender, EventArgs e)
        {
            ConflictCheckerForm conflictCheckerForm = new ConflictCheckerForm(GetCurrentFilePatches());
            conflictCheckerForm.Show();
        }

        private void btn_ViewFreeSpace_Click(object sender, EventArgs e)
        {
            FreeSpaceForm freeSpaceForm = new FreeSpaceForm(GetCurrentFilePatches(), asmUtility);
            freeSpaceForm.Show();
        }
    }
}
