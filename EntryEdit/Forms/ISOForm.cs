﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Controls;
using PatcherLib.Datatypes;
using PatcherLib.Helpers;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace EntryEdit.Forms
{
    public partial class ISOForm : Form
    {
        public enum Mode
        {
            Load = 0,
            Patch = 1
        }

        private DataHelper _dataHelper;
        private EntryData _entryData;
        private Mode _mode;

        private SectorPair[] _sectorPairs = null;
        private Dictionary<Enum, int> _sectorIndexMap = null;

        private Context context;
        private SettingsData settings = null;

        public ISOForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            
        }

        public DialogResult InitDialog(DataHelper dataHelper, EntryData entryData, Mode mode, Context context)
        {
            GetSectorPairs(context);

            this._dataHelper = dataHelper;
            this._entryData = entryData;
            this._mode = mode;

            this.context = context;
            settings = Settings.GetSettings(context);

            pnl_Params.Visible = false;

            spinner_BattleConditionals_Sector.Maximum = settings.MaxSectors;
            spinner_WorldConditionals_Sector.Maximum = settings.MaxSectors;
            spinner_Events_Sector.Maximum = settings.MaxSectors;

            txt_ISO.Text = string.Empty;

            chk_BattleConditionals.Checked = true;
            chk_Events.Checked = true;
            chk_WorldConditionals.Checked = true;

            btn_Load.Enabled = false;
            btn_Patch.Enabled = false;

            if (mode == Mode.Load)
            {
                Text = "Load ISO";
                btn_Load.Visible = true;
                btn_Patch.Visible = false;
            }
            else if (mode == Mode.Patch)
            {
                Text = "Patch ISO";
                btn_Load.Visible = false;
                btn_Patch.Visible = true;
            }

            bool isLoad = (_mode == Mode.Load);
            lbl_BattleConditionals_Size.Visible = isLoad;
            lbl_WorldConditionals_Size.Visible = isLoad;
            lbl_Events_Size.Visible = isLoad;
            spinner_BattleConditionals_Size.Visible = isLoad;
            spinner_WorldConditionals_Size.Visible = isLoad;
            spinner_Events_Size.Visible = isLoad;

            spinner_BattleConditionals_Size.Value = settings.BattleConditionalsSize;
            spinner_WorldConditionals_Size.Value = settings.WorldConditionalsSize;
            spinner_Events_Size.Value = settings.TotalEventSize;

            btn_ISO.Focus();

            InitComboBoxes();
            SetSectorOffsetDefaults();
            
            return ShowDialog();
        }

        private void GetSectorPairs(Context context)
        {
            _sectorPairs = PsxIso.GetSectorPairs();

            _sectorIndexMap = new Dictionary<Enum, int>();
            for (int index = 0; index < _sectorPairs.Length; index++)
            {
                _sectorIndexMap.Add(_sectorPairs[index].Sector, index);
            }
        }

        private void InitComboBoxes()
        {
            cmb_BattleConditionals_Sector.Items.Clear();
            cmb_WorldConditionals_Sector.Items.Clear();
            cmb_Events_Sector.Items.Clear();

            cmb_BattleConditionals_Sector.Items.AddRange(_sectorPairs);
            cmb_WorldConditionals_Sector.Items.AddRange(_sectorPairs);
            cmb_Events_Sector.Items.AddRange(_sectorPairs);
        }

        private void SetSectorOffsetDefaults()
        {
            SetSectorDefault(settings.BattleConditionalsSector, spinner_BattleConditionals_Sector, cmb_BattleConditionals_Sector);
            SetSectorDefault(settings.WorldConditionalsSector, spinner_WorldConditionals_Sector, cmb_WorldConditionals_Sector);
            SetSectorDefault(settings.EventsSector, spinner_Events_Sector, cmb_Events_Sector);

            spinner_BattleConditionals_Offset.Value = settings.BattleConditionalsOffset;
            spinner_WorldConditionals_Offset.Value = settings.WorldConditionalsOffset;
            spinner_Events_Offset.Value = settings.EventsOffset;
        }

        private void SetSectorDefault(Enum sector, NumericUpDownBase spinner, ComboBox comboBox)
        {
            spinner.Value = ISOHelper.GetSectorValue(sector);
            SetSectorComboBoxValue(sector, comboBox);
        }

        private void SetSectorComboBoxValue(Enum sector, ComboBox comboBox)
        {
            int index = 0;
            bool isSectorPresent = _sectorIndexMap.TryGetValue(sector, out index);
            comboBox.SelectedIndex = isSectorPresent ? index : -1;
        }

        private void SetButtonEnabledState()
        {
            bool hasEnabledSections = (chk_BattleConditionals.Checked || chk_WorldConditionals.Checked || chk_Events.Checked);
            btn_Load.Enabled = (_mode == Mode.Load) && hasEnabledSections;
            btn_Patch.Enabled = (_mode == Mode.Patch) && hasEnabledSections;
        }

        private void CheckISO(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                pnl_Params.Visible = true;

                using (Stream file = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (settings.WorldConditionalsRepoint)
                    {
                        if (context == Context.US_PSX)
                        {
                            int ramOffset = PsxIso.ReadFile(file, (PsxIso.Sectors)settings.WorldConditionalsPointerSector, settings.WorldConditionalsPointerOffset, 3).ToIntLE();
                            int wldcoreRamOffset = PsxIso.GetRamOffset(PsxIso.Sectors.WORLD_WLDCORE_BIN);
                            int worldRamOffset = PsxIso.GetRamOffset(PsxIso.Sectors.WORLD_WORLD_BIN);
                            if (ramOffset >= worldRamOffset)
                            {
                                spinner_WorldConditionals_Sector.Value = (int)PsxIso.Sectors.WORLD_WORLD_BIN;
                                spinner_WorldConditionals_Offset.Value = ramOffset - worldRamOffset;
                            }
                            else if (ramOffset >= wldcoreRamOffset)
                            {
                                spinner_WorldConditionals_Sector.Value = (int)PsxIso.Sectors.WORLD_WLDCORE_BIN;
                                spinner_WorldConditionals_Offset.Value = ramOffset - wldcoreRamOffset;
                            }
                        }
                    }
                }

                EnableActionButton();
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
            spinner_BattleConditionals_Sector.Enabled = isEnabled;
            cmb_BattleConditionals_Sector.Enabled = isEnabled;
            spinner_BattleConditionals_Offset.Enabled = isEnabled;
            spinner_BattleConditionals_Size.Enabled = isEnabled;
            SetButtonEnabledState();
        }

        private void chk_WorldConditionals_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chk_WorldConditionals.Checked;
            spinner_WorldConditionals_Sector.Enabled = isEnabled;
            cmb_WorldConditionals_Sector.Enabled = isEnabled;
            spinner_WorldConditionals_Offset.Enabled = isEnabled;
            spinner_WorldConditionals_Size.Enabled = isEnabled;
            SetButtonEnabledState();
        }

        private void chk_Events_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chk_Events.Checked;
            spinner_Events_Sector.Enabled = isEnabled;
            cmb_Events_Sector.Enabled = isEnabled;
            spinner_Events_Offset.Enabled = isEnabled;
            spinner_Events_Size.Enabled = isEnabled;
            SetButtonEnabledState();
        }

        private void spinner_BattleConditionals_Sector_ValueChanged(object sender, EventArgs e)
        {
            SetSectorComboBoxValue((PsxIso.Sectors)spinner_BattleConditionals_Sector.Value, cmb_BattleConditionals_Sector);
        }

        private void spinner_WorldConditionals_Sector_ValueChanged(object sender, EventArgs e)
        {
            SetSectorComboBoxValue((PsxIso.Sectors)spinner_WorldConditionals_Sector.Value, cmb_WorldConditionals_Sector);
        }

        private void spinner_Events_Sector_ValueChanged(object sender, EventArgs e)
        {
            SetSectorComboBoxValue((PsxIso.Sectors)spinner_Events_Sector.Value, cmb_Events_Sector);
        }

        private void cmb_BattleConditionals_Sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_BattleConditionals_Sector.SelectedIndex >= 0)
            {
                spinner_BattleConditionals_Sector.Value = ISOHelper.GetSectorValue(((SectorPair)cmb_BattleConditionals_Sector.SelectedItem).Sector);
            }
        }

        private void cmb_WorldConditionals_Sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_WorldConditionals_Sector.SelectedIndex >= 0)
            {
                spinner_WorldConditionals_Sector.Value = ISOHelper.GetSectorValue(((SectorPair)cmb_WorldConditionals_Sector.SelectedItem).Sector);
            }
        }

        private void cmb_Events_Sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Events_Sector.SelectedIndex >= 0)
            {
                spinner_Events_Sector.Value = ISOHelper.GetSectorValue(((SectorPair)cmb_Events_Sector.SelectedItem).Sector);
            }
        }

        private void btn_ISO_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ISO images (*.iso, *.bin, *.img)|*.iso;*.bin;*.img";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                txt_ISO.Text = openFileDialog.FileName;
                CheckISO(openFileDialog.FileName);
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            btn_Load.Enabled = false;
            string filepath = txt_ISO.Text;

            if (!string.IsNullOrEmpty(filepath))
            {
                try
                {
                    using (Stream file = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (chk_BattleConditionals.Checked)
                        {
                            PsxIso.Sectors battleSector = (PsxIso.Sectors)spinner_BattleConditionals_Sector.Value;
                            int battleOffset = (int)spinner_BattleConditionals_Offset.Value;
                            int battleSize = (int)spinner_BattleConditionals_Size.Value;
                            byte[] battleBytes = PsxIso.ReadFile(file, battleSector, battleOffset, battleSize);
                            _entryData.BattleConditionals = _dataHelper.LoadConditionalSetsFromByteArray(CommandType.BattleConditional, battleBytes);
                        }

                        if (chk_WorldConditionals.Checked)
                        {
                            PsxIso.Sectors worldSector = (PsxIso.Sectors)spinner_WorldConditionals_Sector.Value;
                            int worldOffset = (int)spinner_WorldConditionals_Offset.Value;
                            int worldSize = (int)spinner_WorldConditionals_Size.Value;
                            byte[] worldBytes = PsxIso.ReadFile(file, worldSector, worldOffset, worldSize);
                            _entryData.WorldConditionals = _dataHelper.LoadConditionalSetsFromByteArray(CommandType.WorldConditional, worldBytes);
                        }

                        if (chk_Events.Checked)
                        {
                            PsxIso.Sectors eventSector = (PsxIso.Sectors)spinner_Events_Sector.Value;
                            int eventOffset = (int)spinner_Events_Offset.Value;
                            int eventSize = (int)spinner_Events_Size.Value;
                            byte[] eventBytes = PsxIso.ReadFile(file, eventSector, eventOffset, eventSize);
                            _entryData.Events = _dataHelper.GetEventsFromBytes(eventBytes);
                        }
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    PatcherLib.MyMessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
                    btn_Load.Enabled = true;
                }
            }
        }

        private void btn_Patch_Click(object sender, EventArgs e)
        {
            btn_Patch.Enabled = false;
            string filepath = txt_ISO.Text;

            if (!string.IsNullOrEmpty(filepath))
            {
                List<PatchedByteArray> patches = new List<PatchedByteArray>();

                if (chk_BattleConditionals.Checked)
                {
                    PsxIso.Sectors battleSector = (PsxIso.Sectors)spinner_BattleConditionals_Sector.Value;
                    int battleOffset = (int)spinner_BattleConditionals_Offset.Value;
                    byte[] battleBytes =_dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, _entryData.BattleConditionals);
                    patches.Add(new PatchedByteArray(battleSector, battleOffset, battleBytes));

                    if ((settings.BattleConditionalsApplyLimitPatch) && (DataHelper.GetMaxBlocks(_entryData.BattleConditionals) > 10))
                    {
                        patches.Add(new PatchedByteArray(settings.BattleConditionalsLimitPatchSector, settings.BattleConditionalsLimitPatchOffset, settings.BattleConditionalsLimitPatchBytes));
                    }
                }

                if (chk_WorldConditionals.Checked)
                {
                    PsxIso.Sectors worldSector = (PsxIso.Sectors)spinner_WorldConditionals_Sector.Value;
                    int worldOffset = (int)spinner_WorldConditionals_Offset.Value;
                    byte[] worldBytes = _dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, _entryData.WorldConditionals);
                    patches.Add(new PatchedByteArray(worldSector, worldOffset, worldBytes));

                    if (settings.WorldConditionalsRepoint)
                    {
                        byte[] patchBytes = (((uint)(PsxIso.GetRamOffset(worldSector) + worldOffset)) | PsxIso.KSeg0Mask).ToBytes();
                        patches.Add(new PatchedByteArray(settings.WorldConditionalsPointerSector, settings.WorldConditionalsPointerOffset, patchBytes));
                    }
                }

                if (chk_Events.Checked)
                {
                    PsxIso.Sectors eventSector = (PsxIso.Sectors)spinner_Events_Sector.Value;
                    int eventOffset = (int)spinner_Events_Offset.Value;
                    byte[] eventBytes = _dataHelper.EventsToByteArray(_entryData.Events);
                    patches.Add(new PatchedByteArray(eventSector, eventOffset, eventBytes));
                }

                try
                {
                    using (Stream file = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    {
                        PsxIso.PatchPsxIso(file, patches);
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    PatcherLib.MyMessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK);
                    btn_Patch.Enabled = true;
                }
            }
        }
    }
}
