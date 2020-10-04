using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace EntryEdit.Forms
{
    public partial class StateForm : Form
    {
        public enum Mode
        {
            Load = 0,
            Patch = 1
        }

        private DataHelper _dataHelper;
        private EntryData _entryData;
        private SelectedIndexResult _selectedIndexResult;
        private Mode _mode;

        private byte[] worldConditionalsBytes;

        public StateForm()
        {
            InitializeComponent();
        }

        public DialogResult InitDialog(DataHelper dataHelper, EntryData entryData, SelectedIndexResult selectedIndexResult, Mode mode)
        {
            this._dataHelper = dataHelper;
            this._entryData = entryData;
            this._selectedIndexResult = selectedIndexResult;
            this._mode = mode;

            pnl_Battle.Visible = false;
            pnl_World.Visible = false;

            txt_File.Text = string.Empty;

            chk_BattleConditionals.Checked = true;
            chk_Event.Checked = true;
            chk_WorldConditionals.Checked = true;

            btn_Load.Enabled = false;
            btn_Patch.Enabled = false;

            if (mode == Mode.Load)
            {
                Text = "Load State Data";
                btn_Load.Visible = true;
                btn_Patch.Visible = false;
            }
            else if (mode == Mode.Patch)
            {
                Text = "Patch State Data";
                btn_Load.Visible = false;
                btn_Patch.Visible = true;
            }

            btn_File.Focus();
            InitComboBoxes();
            return ShowDialog();
        }

        private void InitComboBoxes()
        {
            cmb_BattleConditionals_ConditionalSet.Items.Clear();
            cmb_BattleConditionals_ConditionalSet.Items.AddRange(_entryData.BattleConditionals.ToArray());
            cmb_Event.Items.Clear();
            cmb_Event.Items.AddRange(_entryData.Events.ToArray());
        }

        private void CheckStateFile(string filepath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
            {
                Stream stream = reader.BaseStream;

                pnl_Battle.Visible = false;
                pnl_World.Visible = false;

                if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.BATTLE_BIN))
                {
                    pnl_Battle.Visible = true;
                    EnableActionButton();

                    spinner_BattleConditionals_RamLocation_Blocks.Value = Settings.BattleConditionalBlockOffsetsRAMLocation;
                    spinner_BattleConditionals_RamLocation_Commands.Value = Settings.BattleConditionalsRAMLocation;
                    spinner_Event_RamLocation.Value = Settings.EventRAMLocation;

                    chk_BattleConditionals.Checked = (PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.BattleConditionalBlockOffsetsRAMLocation, 4).ToIntLE() != 0);
                    chk_Event.Checked = (PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.EventRAMLocation, 1).ToIntLE() != 0);

                    int loadedEventID = PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.EventIDRAMLocation, 2).ToIntLE();
                    cmb_Event.SelectedIndex = ((loadedEventID >= 0) && (loadedEventID < _entryData.Events.Count)) ? loadedEventID : ((_selectedIndexResult.EventIndex >= 0) ? _selectedIndexResult.EventIndex : 0);

                    int battleConditionalsIndex = 0;
                    if (PsxIso.IsSectorInPsxSaveState(stream, Settings.ScenariosSector))
                        battleConditionalsIndex = PsxIso.LoadFromPsxSaveState(reader, (uint)(Settings.ScenariosRAMLocation + (loadedEventID * 24) + 22), 2).ToIntLE();
                    else
                        battleConditionalsIndex = (_selectedIndexResult.BattleConditionalIndex >= 0) ? _selectedIndexResult.BattleConditionalIndex : 0;

                    cmb_BattleConditionals_ConditionalSet.SelectedIndex = battleConditionalsIndex;
                }
                else if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.WORLD_WLDCORE_BIN))
                {
                    pnl_World.Visible = true;
                    EnableActionButton();

                    bool isLoad = (_mode == Mode.Load);
                    lbl_WorldConditionals_Size.Visible = isLoad;
                    spinner_WorldConditionals_Size.Visible = isLoad;

                    spinner_WorldConditionals_Size.Value = Settings.WorldConditionalsSize;
                    spinner_WorldConditionals_RamLocation.Value = Settings.WorldConditionalsRepoint
                        ? PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.WorldConditionalsPointerRAMLocation, 3).ToIntLE()
                        : Settings.WorldConditionalsCalcRAMLocation;

                    if (_mode == Mode.Patch)
                    {
                        worldConditionalsBytes = _dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, _entryData.WorldConditionals);
                        if (worldConditionalsBytes.Length > Settings.WorldConditionalsSize)
                        {
                            chk_WorldConditionals.Checked = false;
                        }
                    }
                }
            }
        }

        private void EnableActionButton()
        {
            if (_mode == Mode.Load)
            {
                btn_Load.Enabled = true;
                btn_Patch.Enabled = false;
            }
            else if (_mode == Mode.Patch)
            {
                btn_Load.Enabled = false;
                btn_Patch.Enabled = true;
            }
        }

        private void chk_BattleConditionals_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chk_BattleConditionals.Checked;
            spinner_BattleConditionals_RamLocation_Blocks.Enabled = isEnabled;
            spinner_BattleConditionals_RamLocation_Commands.Enabled = isEnabled;
            cmb_BattleConditionals_ConditionalSet.Enabled = isEnabled;

            bool hasEnabledSections = (isEnabled || chk_Event.Checked);
            btn_Load.Enabled = (_mode == Mode.Load) && hasEnabledSections;
            btn_Patch.Enabled = (_mode == Mode.Patch) && hasEnabledSections;
        }

        private void chk_WorldConditionals_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chk_WorldConditionals.Checked;
            spinner_WorldConditionals_RamLocation.Enabled = isEnabled;
            spinner_WorldConditionals_Size.Enabled = isEnabled;
            btn_Load.Enabled = (_mode == Mode.Load) && isEnabled;
            btn_Patch.Enabled = (_mode == Mode.Patch) && isEnabled;
        }

        private void chk_Event_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chk_Event.Checked;
            spinner_Event_RamLocation.Enabled = isEnabled;
            cmb_Event.Enabled = isEnabled;

            bool hasEnabledSections = (isEnabled || chk_BattleConditionals.Checked);
            btn_Load.Enabled = (_mode == Mode.Load) && hasEnabledSections;
            btn_Patch.Enabled = (_mode == Mode.Patch) && hasEnabledSections;
        }

        private void btn_File_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PSV file (*.psv)|*.psv";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                txt_File.Text = openFileDialog.FileName;
                CheckStateFile(openFileDialog.FileName);
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            btn_Load.Enabled = false;
            string filepath = txt_File.Text;

            if (!string.IsNullOrEmpty(filepath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
                {
                    if (pnl_Battle.Visible)
                    {
                        if (chk_BattleConditionals.Checked)
                        {
                            List<byte[]> byteArrays = PsxIso.LoadFromPsxSaveState(reader, new List<KeyValuePair<uint, int>>() 
                            { 
                                new KeyValuePair<uint, int>((uint)spinner_BattleConditionals_RamLocation_Blocks.Value, Settings.BattleConditionalBlockOffsetsRAMLength),
                                new KeyValuePair<uint, int>((uint)spinner_BattleConditionals_RamLocation_Commands.Value, Settings.BattleConditionalsRAMLength)
                            });

                            int setIndex = cmb_BattleConditionals_ConditionalSet.SelectedIndex;
                            if (setIndex >= 0)
                            {
                                _entryData.BattleConditionals[setIndex] = _dataHelper.LoadActiveConditionalSet(setIndex, _entryData.BattleConditionals[setIndex].Name, 
                                    CommandType.BattleConditional, byteArrays[0], byteArrays[1]);
                            }
                        }

                        if (chk_Event.Checked)
                        {
                            int eventIndex = cmb_Event.SelectedIndex;
                            if (eventIndex >= 0)
                            {
                                int eventRamLocation = (int)spinner_Event_RamLocation.Value;
                                byte[] eventBytes = PsxIso.LoadFromPsxSaveState(reader, (uint)eventRamLocation, Settings.EventSize);

                                if (eventBytes.SubLength(0, 4).ToUInt32() == DataHelper.BlankTextOffsetValue)
                                {
                                    int textRamLocation = PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.TextOffsetRAMLocation, 3).ToIntLE();
                                    int textOffset = textRamLocation - eventRamLocation;

                                    if (textOffset < Settings.EventSize)
                                    {
                                        byte[] textOffsetBytes = ((uint)textOffset).ToBytes();
                                        Array.Copy(textOffsetBytes, 0, eventBytes, 0, 4);
                                    }
                                }

                                _entryData.Events[eventIndex] = _dataHelper.GetEventFromBytes(eventIndex, eventBytes, true);
                            }
                        }
                    }
                    else if (pnl_World.Visible)
                    {
                        if (chk_WorldConditionals.Checked)
                        {
                            byte[] bytes = PsxIso.LoadFromPsxSaveState(reader, (uint)spinner_WorldConditionals_RamLocation.Value, (int)spinner_WorldConditionals_Size.Value);
                            _entryData.WorldConditionals = _dataHelper.LoadConditionalSetsFromByteArray(CommandType.WorldConditional, bytes);
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btn_Patch_Click(object sender, EventArgs e)
        {
            btn_Patch.Enabled = false;
            string filepath = txt_File.Text;
            Dictionary<uint, byte[]> ramPatches = new Dictionary<uint, byte[]>();

            if (!string.IsNullOrEmpty(filepath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
                {
                    if (pnl_Battle.Visible)
                    {
                        if (chk_BattleConditionals.Checked)
                        {
                            int setIndex = cmb_BattleConditionals_ConditionalSet.SelectedIndex;
                            if (setIndex >= 0)
                            {
                                uint blockRamOffset = (uint)spinner_BattleConditionals_RamLocation_Blocks.Value;
                                uint commandRamOffset = (uint)spinner_BattleConditionals_RamLocation_Commands.Value;

                                List<byte[]> byteArrays = _dataHelper.ConditionalSetToActiveByteArrays(CommandType.BattleConditional, _entryData.BattleConditionals[setIndex]);
                                ramPatches.Add(blockRamOffset, byteArrays[0]);
                                ramPatches.Add(commandRamOffset, byteArrays[1]);

                                if (Settings.BattleConditionalsApplyLimitPatch)
                                {
                                    int numBlocks = _entryData.BattleConditionals[setIndex].ConditionalBlocks.Count;
                                    if (numBlocks > 10)
                                    {
                                        ramPatches.Add((uint)Settings.BattleConditionalsLimitPatchRAMLocation, Settings.BattleConditionalsLimitPatchBytes);
                                    }
                                }
                            }

                        }

                        if (chk_Event.Checked)
                        {
                            int eventIndex = cmb_Event.SelectedIndex;
                            if (eventIndex >= 0)
                            {
                                uint eventRamOffset = (uint)spinner_Event_RamLocation.Value;
                                byte[] eventBytes = _dataHelper.EventToByteArray(_entryData.Events[eventIndex], true);
                                ramPatches.Add(eventRamOffset, eventBytes);

                                uint textOffset = eventBytes.SubLength(0, 4).ToUInt32();
                                if (textOffset != DataHelper.BlankTextOffsetValue)
                                {
                                    ramPatches.Add((uint)Settings.TextOffsetRAMLocation, (eventRamOffset + textOffset).ToBytes().SubLength(0, 3).ToArray());
                                }
                            }
                        }
                    }
                    else if (pnl_World.Visible)
                    {
                        if (chk_WorldConditionals.Checked)
                        {
                            uint ramOffset = (uint)spinner_WorldConditionals_RamLocation.Value;
                            ramPatches.Add(ramOffset, worldConditionalsBytes);
                            if ((Settings.WorldConditionalsRepoint) && (ramOffset != PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.WorldConditionalsPointerRAMLocation, 3).ToIntLE()))
                            {
                                ramPatches.Add((uint)Settings.WorldConditionalsPointerRAMLocation, ramOffset.ToBytes().SubLength(0, 3).ToArray());
                            }
                        }
                    }

                    PsxIso.PatchPsxSaveState(reader, ramPatches);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
