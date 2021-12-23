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
        private enum PatchSortType
        {
            Ordinal = 0,
            Name = 1
        }

        private ASMEncodingUtility asmUtility;
        private PatchData patchData;
        private AsmPatch[] patches;
        private PatchSortType sortType = PatchSortType.Ordinal;

        private List<AsmPatch> OriginalAllPatches { get; set; }
        private List<AsmPatch> OriginalAllShownPatches { get; set; }
        private Dictionary<PatchData.PatchFile, List<AsmPatch>> OriginalFilePatches { get; set; }

        private bool ignoreChanges = true;
        private bool skipCheckEventHandler = false;

        private Dictionary<VariableType.VariablePreset, int> variablePresetIndexMap = null;

        private int OriginalVariableSpinnerX { get; set; }
        private int displacedVariableSpinnerX = 0;
        public int DisplacedVariableSpinnerX
        {
            get
            {
                if (displacedVariableSpinnerX == 0)
                {
                    displacedVariableSpinnerX = OriginalVariableSpinnerX + cmb_Variable_Preset.Width + 6;
                }

                return displacedVariableSpinnerX;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            string versionText = string.Format("v0.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString());
            versionLabel.Text = versionText;
            Text = string.Format("FFTorgASM ({0})", versionText);

            asmUtility = new ASMEncodingUtility(ASMEncodingMode.PSX);
            SetupModes();

            //LoadFiles();
            FreeSpace.ReadFreeSpaceXML();

            btnPatch.Click += new EventHandler( btnPatch_Click );
            reloadButton.Click += new EventHandler( reloadButton_Click );
            clb_Patches.ItemCheck += new ItemCheckEventHandler( clb_Patches_ItemCheck );
            btnPatch.Enabled = false;
            clb_Patches.SelectedIndexChanged += new EventHandler( clb_Patches_SelectedIndexChanged );
            variableSpinner.ValueChanged += new EventHandler( variableSpinner_ValueChanged );
            variableComboBox.SelectedIndexChanged += new EventHandler(variableComboBox_SelectedIndexChanged);

            lsb_FilesList.SelectedIndex = 0;
            OriginalVariableSpinnerX = variableSpinner.Location.X;
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

            SetOriginalPatches();
            //OriginalFilePatches[patchData.FilePatches[index]] = patchData.FilePatches[index].Patches;
        }

        private void LoadFiles(IList<string> fileList = null)
        {
            string strMode = Enum.GetName(typeof(ASMEncodingMode), asmUtility.EncodingMode);
            string readPath = Path.Combine(Path.Combine(Application.StartupPath, "XmlPatches"), strMode);
            string[] files = (fileList == null) ? Directory.GetFiles(readPath, "*.xml", SearchOption.TopDirectoryOnly) : fileList.ToArray();
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

            SetOriginalPatches();
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

        private void SetOriginalPatches()
        {
            OriginalAllPatches = patchData.AllPatches;
            OriginalAllShownPatches = patchData.AllShownPatches;

            OriginalFilePatches = new Dictionary<PatchData.PatchFile, List<AsmPatch>>();
            foreach (PatchData.PatchFile patchFile in patchData.FilePatches)
            {
                OriginalFilePatches.Add(patchFile, patchFile.Patches);
            }
        }

        private void SetupModes()
        {
            cmb_Mode.Items.Clear();
            cmb_Mode.Items.Add("PSX");
            cmb_Mode.Items.Add("PSP");
            cmb_Mode.SelectedIndex = 0;
        }

        private void ClearCurrentPatch()
        {
            textBox1.Text = "";
            txt_Messages.Text = "";
            variableSpinner.Visible = false;
            ignoreChanges = true;
            variableComboBox.Visible = false;
            cmb_Variable_Preset.Visible = false;
        }

        private void SavePatchXML()
        {
            //List<AsmPatch> patches = GetCurrentFileSelectedPatches();
            List<AsmPatch> patches = GetAllSelectedPatches();
            foreach (AsmPatch patch in patches)
            {
                patch.Update(asmUtility);
            }

            FreeSpaceMode mode = FreeSpace.GetMode(asmUtility);
            string xml = PatchXmlReader.CreatePatchXML(patches, mode);

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
            return GetSelectedPatchesFromList(patchData.AllPatches);
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
                clb_Patches.ForceSetItemChecked(index, patchData.SelectedPatches.Contains(asmPatch));
            }

            skipCheckEventHandler = false;

            //bool enablePatchButtons = (patchData.CurrentSelectedPatches.Count > 0);
            bool enablePatchButtons = (patchData.SelectedPatches.Count > 0);
            btnPatch.Enabled = enablePatchButtons;
            //btnPatchSaveState.Enabled = enablePatchButtons;
        }

        private void LoadCurrentFilePatches()
        {
            if (lsb_FilesList.SelectedItem != null)
                LoadFilePatches(lsb_FilesList.SelectedIndex);
        }

        private void ToggleSort()
        {
            sortType = 1 - sortType;
            FormSort();
        }

        private void FormSort()
        {
            SortPatches();
            patchData.RecalcBackgroundColors();
            LoadCurrentFilePatches();
        }

        private void FormFilter()
        {
            FilterPatches();
            patchData.RecalcBackgroundColors();
            LoadCurrentFilePatches();
        }

        private void FormSortAndFilter()
        {
            SortPatches();
            FilterPatches();
            patchData.RecalcBackgroundColors();
            LoadCurrentFilePatches();
        }

        private void SortPatchesOrdinal()
        {
            patchData.AllPatches.Sort((x, y) => patchData.AllOrdinalMap[x].CompareTo(patchData.AllOrdinalMap[y]));
            patchData.AllShownPatches.Sort((x, y) => patchData.AllOrdinalMap[x].CompareTo(patchData.AllOrdinalMap[y]));
            OriginalAllPatches.Sort((x, y) => patchData.AllOrdinalMap[x].CompareTo(patchData.AllOrdinalMap[y]));
            OriginalAllShownPatches.Sort((x, y) => patchData.AllOrdinalMap[x].CompareTo(patchData.AllOrdinalMap[y]));

            for (int index = 0; index < patchData.FilePatches.Length; index++)
            {
                PatchData.PatchFile patchFile = patchData.FilePatches[index];
                if ((patchFile != null) && (patchFile.Patches != null))
                {
                    patchFile.Patches.Sort((x, y) => patchData.FileOrdinalMaps[index][x].CompareTo(patchData.FileOrdinalMaps[index][y]));
                    OriginalFilePatches[patchFile].Sort((x, y) => patchData.FileOrdinalMaps[index][x].CompareTo(patchData.FileOrdinalMaps[index][y]));
                }
            }
        }

        private void SortPatches(Comparison<AsmPatch> comparer)
        {
            patchData.AllPatches.Sort(comparer);
            patchData.AllShownPatches.Sort(comparer);
            OriginalAllPatches.Sort(comparer);
            OriginalAllShownPatches.Sort(comparer);

            foreach (PatchData.PatchFile patchFile in patchData.FilePatches)
            {
                if ((patchFile != null) && (patchFile.Patches != null))
                {
                    patchFile.Patches.Sort(comparer);
                    OriginalFilePatches[patchFile].Sort(comparer);
                }
            }
        }

        private void SortPatches(PatchSortType sortType)
        {
            if (sortType == PatchSortType.Ordinal)
            {
                SortPatchesOrdinal();
            }
            else if (sortType == PatchSortType.Name)
            {
                SortPatches((x, y) => x.Name.CompareTo(y.Name));
            }
        }

        private void SortPatches()
        {
            SortPatches(sortType);
        }

        private void FilterPatches(string filter = null)
        {
            if (filter == null)
                filter = txt_Search.Text;

            //patchData.AllPatches = GetFilteredPatches(OriginalAllPatches, filter);
            patchData.AllShownPatches = GetFilteredPatches(OriginalAllShownPatches, filter);
            foreach (PatchData.PatchFile patchFile in patchData.FilePatches)
            {
                if ((patchFile != null) && (patchFile.Patches != null))
                    patchFile.Patches = GetFilteredPatches(OriginalFilePatches[patchFile], filter);
            }
        }

        private List<AsmPatch> GetFilteredPatches(IList<AsmPatch> patches, string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return new List<AsmPatch>(patches);

            filter = filter.ToUpper().Trim();
            List<AsmPatch> resultList = new List<AsmPatch>();

            foreach (AsmPatch patch in patches)
            {
                if (patch.Name.ToUpper().Trim().Contains(filter))
                {
                    resultList.Add(patch);
                }
            }

            return resultList;
        }

        private void HandleVariablePresets(VariableType variable)
        {
            if (variable.PresetValues.Count > 0)
            {
                variableSpinner.Location = new Point(DisplacedVariableSpinnerX, variableSpinner.Location.Y);
                cmb_Variable_Preset.Visible = true;
                cmb_Variable_Preset.Items.Clear();

                int selectedIndex = -1;
                variablePresetIndexMap = new Dictionary<VariableType.VariablePreset, int>();
                VariableType.VariablePreset selectedPreset = FindSelectedPreset(variable);
                for (int index = 0; index < variable.PresetValues.Count; index++)
                {
                    VariableType.VariablePreset preset = variable.PresetValues[index];
                    variablePresetIndexMap.Add(preset, index);
                    cmb_Variable_Preset.Items.Add(preset);
                    if (variable.PresetValues[index] == selectedPreset)
                    {
                        selectedIndex = index;
                    }
                }

                if (selectedPreset == null)
                {
                    cmb_Variable_Preset.SelectedIndex = -1;
                    variableSpinner.Visible = true;
                }
                else
                {
                    cmb_Variable_Preset.SelectedIndex = selectedIndex;
                    variableSpinner.Visible = selectedPreset.IsModifiable;
                }
            }
            else
            {
                variableSpinner.Location = new Point(OriginalVariableSpinnerX, variableSpinner.Location.Y);
                cmb_Variable_Preset.Visible = false;
                variableSpinner.Visible = true;
            }
        }

        private VariableType.VariablePreset FindSelectedPreset(VariableType variable)
        {
            uint value = Utilities.GetUnsignedByteArrayValue_LittleEndian(variable.ByteArray);

            foreach (VariableType.VariablePreset preset in variable.PresetValues)
            {
                if (value == preset.Value)
                {
                    return preset;
                }
            }

            foreach (VariableType.VariablePreset preset in variable.PresetValues)
            {
                if (preset.IsModifiable)
                    return preset;
            }

            return null;
        }

        private void SetFormButtonStatus()
        {
            //bool isPSX = (asmUtility.EncodingMode == ASMEncodingMode.PSX);
            //btn_ViewFreeSpace.Enabled = isPSX;
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = lsb_FilesList.SelectedIndex;
            if (selectedIndex > 0)
                LoadFile(selectedIndex - 1);
            else
                LoadFiles();

            FormSortAndFilter();
        }

        private void variableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
                VariableType selectedVariable = patch.VariableMap[(string)variableComboBox.SelectedItem];

                byte[] byteArray = selectedVariable.ByteArray;

                // Setting Maximum can trigger the variableSpinner_ValueChanged event, but we don't want to change the variable value here,
                // so set ignoreChanges = true before setting Maximum.
                ignoreChanges = true;
                variableSpinner.Maximum = (decimal)Math.Pow(256, selectedVariable.NumBytes) - 1;
                ignoreChanges = false;

                variableSpinner.Value = Utilities.GetUnsignedByteArrayValue_LittleEndian(byteArray);

                HandleVariablePresets(selectedVariable);
            }
        }
        
        private void cmb_Variable_Preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                AsmPatch patch = (clb_Patches.SelectedItem as AsmPatch);
                VariableType selectedVariable = patch.VariableMap[(string)variableComboBox.SelectedItem];
                VariableType.VariablePreset preset = (VariableType.VariablePreset)cmb_Variable_Preset.SelectedItem;
                variableSpinner.Visible = preset.IsModifiable;

                if (!preset.IsModifiable)
                    variableSpinner.Value = preset.Value;
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

                /*
                if (variable.PresetValues.Count > 0)
                {
                    VariableType.VariablePreset preset = FindSelectedPreset(variable);
                    if (preset == null)
                    {
                        cmb_Variable_Preset.SelectedIndex = -1;
                    }
                    else
                    {
                        cmb_Variable_Preset.SelectedIndex = variablePresetIndexMap[preset];
                    }
                }
                */
            }
        }

        private void lsb_FilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCurrentFilePatches();
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
                        if (!variable.IsReference)
                        {
                            variableComboBox.Items.Add(variable.Name);
                            if (!foundFirst)
                            {
                                firstNonReferenceVariable = variable;
                                foundFirst = true;
                            }
                        }
                    }
                    variableComboBox.SelectedIndex = 0;

                    byte[] byteArray = firstNonReferenceVariable.ByteArray;
                    variableSpinner.Maximum = (decimal)Math.Pow(256, firstNonReferenceVariable.NumBytes) - 1;
                    variableSpinner.Value = Utilities.GetUnsignedByteArrayValue_LittleEndian(byteArray);

                    variableSpinner.Visible = true;
                    ignoreChanges = false;
                    variableComboBox.Visible = true;

                    HandleVariablePresets(firstNonReferenceVariable);
                }
                else
                {
                    variableSpinner.Visible = false;
                    ignoreChanges = true;
                    variableComboBox.Visible = false;
                    cmb_Variable_Preset.Visible = false;
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
            //btnPatchSaveState.Enabled = enablePatchButtons;
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            PatchForm patchForm = new PatchForm(GetAllSelectedPatches(), asmUtility);
            patchForm.Show();
        }

        /*
        private void btnPatch_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "ISO or PSV files (*.bin, *.iso, *.img, *.psv)|*.bin;*.iso;*.img;*.psv";
            saveFileDialog1.FileName = string.Empty;

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                PatchResult patchResult = PatchHelper.PatchFile(saveFileDialog1.FileName, GetAllSelectedPatches(), asmUtility);
                PatcherLib.MyMessageBox.Show(this, patchResult.Message, ((patchResult.IsSuccess) ? "Complete!" : "Error"), MessageBoxButtons.OK);
            }
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            saveFileDialog1.FileName = string.Empty;

            if ( saveFileDialog1.ShowDialog( this ) == DialogResult.OK )
            {
                PatchResult patchResult = PatchHelper.PatchISO(saveFileDialog1.FileName, GetAllSelectedPatches(), asmUtility);
                PatcherLib.MyMessageBox.Show(this, patchResult.Message, ((patchResult.IsSuccess) ? "Complete!" : "Error"), MessageBoxButtons.OK);    
            }
        }

        private void btnPatchSaveState_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.Filter = "PSV files (*.psv)|*.psv|All files (*.*)|*.*";
            saveFileDialog1.Filter = "PSV files (*.psv)|*.psv";
            saveFileDialog1.FileName = string.Empty;

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                PatchResult patchResult = PatchHelper.PatchPSV(saveFileDialog1.FileName, GetAllSelectedPatches(), asmUtility);
                PatcherLib.MyMessageBox.Show(this, patchResult.Message, ((patchResult.IsSuccess) ? "Complete!" : "Error"), MessageBoxButtons.OK);
            }
        }
        */

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
                    clb_Patches.ForceSetItemChecked( i, true );
            }
        }

        private void btn_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clb_Patches.Items.Count; i++)
            {
                // never check a FileAsmPatch
                if (!(clb_Patches.Items[i] is FileAsmPatch) || clb_Patches.GetItemChecked(i))
                    clb_Patches.ForceSetItemChecked(i, false);
            }
        }

        private void toggleButton_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < clb_Patches.Items.Count; i++ )
            {
                // never check a FileAsmPatch
                if ( !( clb_Patches.Items[i] is FileAsmPatch ) || clb_Patches.GetItemChecked( i ) )
                    clb_Patches.ForceSetItemChecked( i, !clb_Patches.GetItemChecked( i ) );
            }
        }

        private void btn_SavePatchXML_Click(object sender, EventArgs e)
        {
            SavePatchXML();
        }

        private void btn_OpenConflictChecker_Click(object sender, EventArgs e)
        {
            FreeSpaceMode mode = FreeSpace.GetMode(asmUtility);
            ConflictCheckerForm conflictCheckerForm = new ConflictCheckerForm(GetCurrentFilePatches(), mode);
            conflictCheckerForm.Show();
        }

        private void btn_ViewFreeSpace_Click(object sender, EventArgs e)
        {
            FreeSpaceForm freeSpaceForm = new FreeSpaceForm(GetCurrentFilePatches(), asmUtility);
            freeSpaceForm.Show();
        }

        private void btn_Sort_Click(object sender, EventArgs e)
        {
            ToggleSort();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_Search.Text = string.Empty;
            FormFilter();
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            FormFilter();
        }

        private void cmb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            asmUtility.EncodingMode = (ASMEncodingMode)(cmb_Mode.SelectedIndex + 1);
            LoadFiles();
            FormSortAndFilter();
            SetFormButtonStatus();
        }
    }
}
