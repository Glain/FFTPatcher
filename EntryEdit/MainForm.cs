using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.WindowsAPICodePack.Dialogs;
using EntryEdit.Forms;

namespace EntryEdit
{
    public partial class MainForm : Form
    {
        private DataHelper _dataHelper;
        private Dictionary<CommandType, CommandData> _commandDataMap;

        private EntryData _entryData;
        private EntryData _entryDataDefault;

        private List<Command> _commandListCopy;
        private CommandType _commandListCopyType;

        private ConditionalSet _battleConditionalSetCopy;
        private ConditionalSet _worldConditionalSetCopy;
        private Event _eventCopy;

        private StateForm _stateForm = null;
        public StateForm StateForm
        {
            get
            {
                if (_stateForm == null)
                    _stateForm = new StateForm();

                return _stateForm;
            }
        }

        private ISOForm _isoForm = null;
        public ISOForm ISOForm
        {
            get
            {
                if (_isoForm == null)
                    _isoForm = new ISOForm();

                return _isoForm;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            Start();
            //WriteLoadedDataToTestFiles();
            //WriteByteDataToTestFiles();
        }

        private void Start()
        {
            _dataHelper = new DataHelper();
            _commandDataMap = _dataHelper.GetCommandDataMap();

            tabControl.Enabled = false;
        }

        private void LoadNewPatch()
        {
            _entryData = _dataHelper.LoadDefaultEntryData();
            _entryDataDefault = _entryData.Copy();
            PopulateTabs();
        }

        private void LoadPatch()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Patch file (*.eepatch)|*.eepatch";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadPatch(openFileDialog.FileName);
                EnableMenu();
            }
        }

        private void LoadPatch(string filepath)
        {
            byte[] bytesBattleConditionals, bytesWorldConditionals, bytesEvents;
            EntryBytes defaultEntryBytes = _dataHelper.LoadDefaultEntryBytes();

            using (ICSharpCode.SharpZipLib.Zip.ZipFile file = new ICSharpCode.SharpZipLib.Zip.ZipFile(filepath))
            {
                bytesBattleConditionals = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameBattleConditionals, false) ?? defaultEntryBytes.BattleConditionals;
                bytesWorldConditionals = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameWorldConditionals, false) ?? defaultEntryBytes.WorldConditionals;
                bytesEvents = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameEvents, false) ?? defaultEntryBytes.Events;
            }

            _entryDataDefault = _dataHelper.LoadEntryDataFromBytes(defaultEntryBytes);
            _entryData = _dataHelper.LoadEntryDataFromBytes(bytesBattleConditionals, bytesWorldConditionals, bytesEvents);
            PopulateTabs();
        }

        private void SavePatch()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Patch file (*.eepatch)|*.eepatch";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SavePatch(saveFileDialog.FileName);
                //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private void SavePatch(string filepath)
        {
            SaveFormData();
            EntryBytes entryBytes = _dataHelper.GetEntryBytesFromData(_entryData);

            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream stream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(System.IO.File.Open(filepath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite)))
            {
                PatcherLib.Utilities.Utilities.WriteFileToZip(stream, DataHelper.EntryNameBattleConditionals, entryBytes.BattleConditionals);
                PatcherLib.Utilities.Utilities.WriteFileToZip(stream, DataHelper.EntryNameWorldConditionals, entryBytes.WorldConditionals);
                PatcherLib.Utilities.Utilities.WriteFileToZip(stream, DataHelper.EntryNameEvents, entryBytes.Events);
            }
        }

        private void SetDefaults()
        {
            menuItem_UseTrimmedDefaults.Checked = false;
            SaveFormData();
            _entryDataDefault = _entryData.Copy();
            PopulateTabs();
        }

        private void RestoreDefaults()
        {
            menuItem_UseTrimmedDefaults.Checked = false;
            SaveFormData();
            _entryDataDefault = _dataHelper.LoadDefaultEntryData();
            PopulateTabs();
        }

        private void PopulateTabs()
        {
            PopulateBattleTab();
            PopulateWorldTab();
            PopulateEventTab();
        }

        private void PopulateBattleTab()
        {
            battleConditionalSetsEditor.Populate(_entryData.BattleConditionals, _entryDataDefault.BattleConditionals, _commandDataMap[CommandType.BattleConditional], _dataHelper.BattleConditionalSetMaxBlocks);
        }

        private void PopulateWorldTab()
        {
            worldConditionalSetsEditor.Populate(_entryData.WorldConditionals, _entryDataDefault.WorldConditionals, _commandDataMap[CommandType.WorldConditional]);
        }

        private void PopulateEventTab()
        {
            eventsEditor.Populate(_entryData.Events, _entryDataDefault.Events, _commandDataMap[CommandType.EventCommand]);
        }

        private void SaveFormData()
        {
            battleConditionalSetsEditor.SaveBlock();
            worldConditionalSetsEditor.SaveBlock();
            eventsEditor.SavePage();
        }

        private void LoadScript()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string script = System.IO.File.ReadAllText(openFileDialog.FileName);

                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    int blockIndex = battleConditionalSetsEditor.GetBlockIndex();
                    if (blockIndex >= 0)
                    {
                        ConditionalBlock loadedBlock = _dataHelper.GetConditionalBlockFromScript(CommandType.BattleConditional, blockIndex, script);
                        if (loadedBlock != null)
                        {
                            battleConditionalSetsEditor.LoadSelectedBlock(loadedBlock);
                            //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                        }
                        else
                        {
                            PatcherLib.MyMessageBox.Show(this, "Error loading script!", "Error", MessageBoxButtons.OK);
                        }
                    }
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    int blockIndex = worldConditionalSetsEditor.GetBlockIndex();
                    if (blockIndex >= 0)
                    {
                        ConditionalBlock loadedBlock = _dataHelper.GetConditionalBlockFromScript(CommandType.WorldConditional, blockIndex, script);
                        if (loadedBlock != null)
                        {
                            worldConditionalSetsEditor.LoadSelectedBlock(loadedBlock);
                            //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                        }
                        else
                        {
                            PatcherLib.MyMessageBox.Show(this, "Error loading script!", "Error", MessageBoxButtons.OK);
                        }
                    }
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    Event loadedEvent = _dataHelper.GetEventFromScript(script, eventsEditor.CopyEvent());
                    if (loadedEvent != null)
                    {
                        eventsEditor.LoadEvent(loadedEvent);
                        //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                    }
                    else
                    {
                        PatcherLib.MyMessageBox.Show(this, "Error loading script!", "Error", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void SaveScript()
        {
            SaveFormData();
            string script = string.Empty;

            if (tabControl.SelectedTab == tabPage_BattleConditionals)
            {
                script = battleConditionalSetsEditor.GetSelectedBlockCommandListScript();
            }
            else if (tabControl.SelectedTab == tabPage_WorldConditionals)
            {
                script = worldConditionalSetsEditor.GetSelectedBlockCommandListScript();
            }
            else if (tabControl.SelectedTab == tabPage_Events)
            {
                script = eventsEditor.GetEventScript();
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, script, Encoding.UTF8);
                PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private void LoadAllScripts()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    List<ConditionalSet> conditionalSets = _dataHelper.LoadAllConditionalSetScripts(CommandType.BattleConditional, path);
                    if (conditionalSets != null)
                    {
                        _entryData.BattleConditionals = conditionalSets;
                    }
                    else
                    {
                        PatcherLib.MyMessageBox.Show(this, "Error loading scripts!", "Error", MessageBoxButtons.OK);
                    }
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    List<ConditionalSet> conditionalSets = _dataHelper.LoadAllConditionalSetScripts(CommandType.WorldConditional, path);
                    if (conditionalSets != null)
                    {
                        _entryData.WorldConditionals = conditionalSets;
                    }
                    else
                    {
                        PatcherLib.MyMessageBox.Show(this, "Error loading scripts!", "Error", MessageBoxButtons.OK);
                    }
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    List<Event> events = _dataHelper.LoadAllEventScripts(_entryData.Events, path);
                    if (events != null)
                    {
                        _entryData.Events = events;
                    }
                    else
                    {
                        PatcherLib.MyMessageBox.Show(this, "Error loading scripts!", "Error", MessageBoxButtons.OK);
                    }
                }

                PopulateTabs();
            } 
        }

        private void SaveAllScripts()
        {
            SaveFormData();

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ".";
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                bool isSuccess = false;
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    isSuccess = _dataHelper.SaveAllConditionalSetScripts(_entryData.BattleConditionals, path);
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    isSuccess = _dataHelper.SaveAllConditionalSetScripts(_entryData.WorldConditionals, path);
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    isSuccess = _dataHelper.SaveAllEventScripts(_entryData.Events, path);
                }

                if (isSuccess)
                {
                    PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
                }
                else
                {
                    PatcherLib.MyMessageBox.Show(this, "Error saving scripts!", "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void ClearAll()
        {
            if (tabControl.SelectedTab == tabPage_BattleConditionals)
            {
                foreach (ConditionalSet set in _entryData.BattleConditionals)
                {
                    set.Clear();
                }

                PopulateBattleTab();
            }
            else if (tabControl.SelectedTab == tabPage_WorldConditionals)
            {
                foreach (ConditionalSet set in _entryData.WorldConditionals)
                {
                    set.Clear();
                }

                PopulateWorldTab();
            }
            else if (tabControl.SelectedTab == tabPage_Events)
            {
                foreach (Event ev in _entryData.Events)
                {
                    ev.Clear();
                }

                PopulateEventTab();
            }
        }

        private void DeleteAll()
        {
            if (tabControl.SelectedTab == tabPage_BattleConditionals)
            {
                _entryData.BattleConditionals.Clear();
                PopulateBattleTab();
            }
            else if (tabControl.SelectedTab == tabPage_WorldConditionals)
            {
                _entryData.WorldConditionals.Clear();
                PopulateWorldTab();
            }
            else if (tabControl.SelectedTab == tabPage_Events)
            {
                //_entryData.Events.Clear();
                //PopulateEventTab();
                PatcherLib.MyMessageBox.Show(this, "Deleting All Events not supported.", "Info", MessageBoxButtons.OK);
            }
        }

        private void ReloadAll()
        {
            if (tabControl.SelectedTab == tabPage_BattleConditionals)
            {
                _entryData.BattleConditionals = CopyableEntry.CopyList<ConditionalSet>(_entryDataDefault.BattleConditionals);
                PopulateBattleTab();
            }
            else if (tabControl.SelectedTab == tabPage_WorldConditionals)
            {
                _entryData.WorldConditionals = CopyableEntry.CopyList<ConditionalSet>(_entryDataDefault.WorldConditionals);
                PopulateWorldTab();
            }
            else if (tabControl.SelectedTab == tabPage_Events)
            {
                _entryData.Events = CopyableEntry.CopyList<Event>(_entryDataDefault.Events);
                PopulateEventTab();
            }
        }

        private void EnableMenu()
        {
            menuItem_Edit.Enabled = true;
            menuItem_View.Enabled = true;
            menuItem_SavePatch.Enabled = true;
            menuItem_LoadScript.Enabled = true;
            menuItem_SaveScript.Enabled = true;
            menuItem_LoadAllScripts.Enabled = true;
            menuItem_SaveAllScripts.Enabled = true;
            menuItem_PatchISO.Enabled = true;
            menuItem_PatchPSXSaveState.Enabled = true;
            menuItem_LoadPSXSaveState.Enabled = true;
        }

        private SelectedIndexResult GetSelectedIndexResult()
        {
            return new SelectedIndexResult(battleConditionalSetsEditor.GetSetIndex(), worldConditionalSetsEditor.GetSetIndex(), eventsEditor.GetEventIndex());
        }

        private void WriteLoadedDataToTestFiles()
        {
            _entryData = _dataHelper.LoadDefaultEntryData();
            List<ConditionalSet> battleConditionals = _dataHelper.LoadAllConditionalSetScripts(CommandType.BattleConditional, "./EntryData/scripts/battle/1");
            List<ConditionalSet> worldConditionals = _dataHelper.LoadAllConditionalSetScripts(CommandType.WorldConditional, "./EntryData/scripts/world/1");
            List<Event> events = _dataHelper.LoadAllEventScripts(_entryData.Events, "./EntryData/scripts/event/1");
            System.IO.File.WriteAllBytes("EntryData/TestBattle.bin", _dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, battleConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestWorld.bin", _dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, worldConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestEvents.bin", _dataHelper.EventsToByteArray(events));
        }

        private void WriteByteDataToTestFiles()
        {
            _entryData = _dataHelper.LoadDefaultEntryData();
            System.IO.File.WriteAllBytes("EntryData/TestBattle.bin", _dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, _entryData.BattleConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestWorld.bin", _dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, _entryData.WorldConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestEvents.bin", _dataHelper.EventsToByteArray(_entryData.Events));
        }

        private void SaveXMLFromEventFilenames(string inputFilepath, string outputFilepath)
        {
            string[] filepaths = System.IO.Directory.GetFiles(inputFilepath);
            StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.Append(Environment.NewLine);
            sb.AppendLine("<Entries>");
            for (int index = 0; index < filepaths.Length; index++)
            {
                string filename = System.IO.Path.GetFileName(filepaths[index]);
                string entryName = filename.Substring(8, filename.LastIndexOf('.') - 8).Replace("&", "and");
                sb.AppendFormat("    <Entry name=\"{0}\" />{1}", entryName, Environment.NewLine);
            }
            sb.AppendLine("</Entries>");
            System.IO.File.WriteAllText(outputFilepath, sb.ToString());
        }

        private void ConvertXML(string inputFilepath, string outputFilepath)
        {
            StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.Append(Environment.NewLine);
            sb.AppendLine("<Entries>");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(inputFilepath);
            XmlNodeList nodeList = xmlDocument.SelectNodes("//Entry");
            foreach (XmlNode node in nodeList)
            {
                string oldNameText = node.Attributes["name"].InnerText;
                int spaceIndex = oldNameText.IndexOf(' ');
                string hex = oldNameText.Substring(0, spaceIndex);
                string newName = oldNameText.Substring(spaceIndex + 1, oldNameText.Length - spaceIndex - 1);
                sb.AppendFormat("    <Entry hex=\"{0}\" name=\"{1}\" />{2}", hex, newName, Environment.NewLine);
            }
            sb.AppendLine("</Entries>");
            System.IO.File.WriteAllText(outputFilepath, sb.ToString());
        }

        private void menuItem_NewPatch_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            LoadNewPatch();
            EnableMenu();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_LoadScript_Click(object sender, EventArgs e)
        {
            LoadScript();
        }

        private void menuItem_SaveScript_Click(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void menuItem_LoadAllScripts_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            LoadAllScripts();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_SaveAllScripts_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            SaveAllScripts();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItem_CopyCommands_Click(object sender, EventArgs e)
        {
            if (menuItem_Edit.Enabled)
            {
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    _commandListCopy = battleConditionalSetsEditor.CopyCommandList();
                    _commandListCopyType = CommandType.BattleConditional;
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    _commandListCopy = worldConditionalSetsEditor.CopyCommandList();
                    _commandListCopyType = CommandType.WorldConditional;
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    _commandListCopy = eventsEditor.CopyCommandList();
                    _commandListCopyType = CommandType.EventCommand;
                }
            }
        }

        private void menuItem_PasteCommands_Click(object sender, EventArgs e)
        {
            if ((menuItem_Edit.Enabled) && (_commandListCopy != null))
            {
                if ((tabControl.SelectedTab == tabPage_BattleConditionals) && (_commandListCopyType == CommandType.BattleConditional))
                {
                    battleConditionalSetsEditor.PasteCommandList(_commandListCopy);
                }
                else if ((tabControl.SelectedTab == tabPage_WorldConditionals) && (_commandListCopyType == CommandType.WorldConditional))
                {
                    worldConditionalSetsEditor.PasteCommandList(_commandListCopy);
                }
                else if ((tabControl.SelectedTab == tabPage_Events) && (_commandListCopyType == CommandType.EventCommand))
                {
                    eventsEditor.PasteCommandList(_commandListCopy);
                }
            }
        }

        private void menuItem_CopyEntry_Click(object sender, EventArgs e)
        {
            if (menuItem_Edit.Enabled)
            {
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    _battleConditionalSetCopy = battleConditionalSetsEditor.CopyConditionalSet();
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    _worldConditionalSetCopy = worldConditionalSetsEditor.CopyConditionalSet();
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    _eventCopy = eventsEditor.CopyEvent();
                }
            }
        }

        private void menuItem_PasteEntry_Click(object sender, EventArgs e)
        {
            if (menuItem_Edit.Enabled)
            {
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    battleConditionalSetsEditor.PasteConditionalSet(_battleConditionalSetCopy);
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    worldConditionalSetsEditor.PasteConditionalSet(_worldConditionalSetCopy);
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    eventsEditor.PasteEvent(_eventCopy);
                }
            }
        }

        private void menuItem_UseTrimmedDefaults_Click(object sender, EventArgs e)
        {
            menuItem_UseTrimmedDefaults.Checked = !menuItem_UseTrimmedDefaults.Checked;
            SaveFormData();
            _entryDataDefault = menuItem_UseTrimmedDefaults.Checked ? _dataHelper.LoadTrimmedEntryData() : _dataHelper.LoadDefaultEntryData();
            PopulateTabs();
        }

        private void menuItem_SetDefaults_Click(object sender, EventArgs e)
        {
            SetDefaults();
        }

        private void menuItem_RestoreDefaults_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            RestoreDefaults();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_ClearAll_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            ClearAll();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_DeleteAll_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            DeleteAll();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_ReloadAll_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            ReloadAll();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        private void menuItem_CheckSize_Click(object sender, EventArgs e)
        {
            if (menuItem_View.Enabled)
            {
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    battleConditionalSetsEditor.SaveBlock();
                    byte[] bytes = _dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, _entryData.BattleConditionals);
                    ConditionalSet selectedConditionalSet = battleConditionalSetsEditor.CopyConditionalSet();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("All Battle Conditionals Size: {0} / {1} bytes{2}", bytes.Length, Settings.BattleConditionalsSize, Environment.NewLine);

                    if (selectedConditionalSet != null)
                    {
                        int maxCommands = _dataHelper.BattleConditionalSetMaxCommands;
                        sb.AppendFormat("{0} {1}: {2} / {3} commands{4}", selectedConditionalSet.Index.ToString("X2"), selectedConditionalSet.Name, selectedConditionalSet.GetNumCommands(),
                            maxCommands, Environment.NewLine);

                        bool hasInvalidSets = false;
                        int highestCommandTotal = 0;
                        int highestCommandIndex = 0;
                        for (int index = 0; index < _entryData.BattleConditionals.Count; index++)
                        {
                            ConditionalSet conditionalSet = _entryData.BattleConditionals[index];
                            int numSetCommands = conditionalSet.GetNumCommands();

                            if (highestCommandTotal < numSetCommands)
                            {
                                highestCommandTotal = numSetCommands;
                                highestCommandIndex = index;
                            }

                            if (numSetCommands > maxCommands)
                            {
                                hasInvalidSets = true;
                                sb.AppendFormat("{0} {1}: {2} / {3} commands{4}", conditionalSet.Index.ToString("X2"), conditionalSet.Name, numSetCommands, maxCommands, Environment.NewLine);
                            }
                        }

                        if (!hasInvalidSets)
                        {
                            ConditionalSet conditionalSet = _entryData.BattleConditionals[highestCommandIndex];
                            sb.AppendFormat("Largest set: {0} {1}: {2} / {3} commands{4}", conditionalSet.Index.ToString("X2"), conditionalSet.Name, highestCommandTotal, maxCommands, Environment.NewLine);
                        }
                    }

                    PatcherLib.MyMessageBox.Show(this, sb.ToString(), "Size", MessageBoxButtons.OK);
                }
                else if (tabControl.SelectedTab == tabPage_WorldConditionals)
                {
                    worldConditionalSetsEditor.SaveBlock();
                    byte[] bytes = _dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, _entryData.WorldConditionals);
                    PatcherLib.MyMessageBox.Show(this, string.Format("All World Conditionals Size: {0} / {1} bytes", bytes.Length, Settings.WorldConditionalsSize), "Size", MessageBoxButtons.OK);
                }
                else if (tabControl.SelectedTab == tabPage_Events)
                {
                    eventsEditor.SavePage();
                    Event selectedEvent = eventsEditor.CopyEvent();
                    byte[] bytes = _dataHelper.EventToByteArray(selectedEvent, false);
                    int maxEventBytes = _dataHelper.EventSize;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} {1}: {2} / {3} bytes{4}", selectedEvent.Index.ToString("X4"), selectedEvent.Name, bytes.Length, Settings.EventSize, Environment.NewLine);

                    bool hasInvalidEvents = false;
                    int highestByteTotal = 0;
                    int highestByteIndex = 0;
                    for (int index = 0; index < _entryData.Events.Count; index++)
                    {
                        Event ev = _entryData.Events[index];
                        ev.TextSection.DecodeText();
                        byte[] eventBytes = _dataHelper.EventToByteArray(ev, false);
                        int numEventBytes = eventBytes.Length;

                        if (highestByteTotal < numEventBytes)
                        {
                            highestByteTotal = numEventBytes;
                            highestByteIndex = index;
                        }

                        if (numEventBytes > maxEventBytes)
                        {
                            hasInvalidEvents = true;
                            sb.AppendFormat("{0} {1}: {2} / {3} bytes{4}", ev.Index.ToString("X4"), ev.Name, numEventBytes, maxEventBytes, Environment.NewLine);
                        }
                    }

                    if (!hasInvalidEvents)
                    {
                        Event ev = _entryData.Events[highestByteIndex];
                        sb.AppendFormat("Largest event: {0} {1}: {2} / {3} bytes{4}", ev.Index.ToString("X4"), ev.Name, highestByteTotal, maxEventBytes, Environment.NewLine);
                    }

                    PatcherLib.MyMessageBox.Show(this, sb.ToString(), "Size", MessageBoxButtons.OK);
                }
            }
        }

        private void menuItem_LoadPatch_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            LoadPatch();
            tabControl.Enabled = (_entryData != null);
            menuBar.Enabled = true;
        }

        private void menuItem_SavePatch_Click(object sender, EventArgs e)
        {
            menuBar.Enabled = false;
            tabControl.Enabled = false;
            SavePatch();
            tabControl.Enabled = true;
            menuBar.Enabled = true;
        }

        void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage tabPage = (sender as TabControl).SelectedTab;
            menuItem_DeleteAll.Enabled = (tabPage != tabPage_Events);
        }

        private void menuItem_PatchISO_Click(object sender, EventArgs e)
        {
            SaveFormData();
            if (ISOForm.InitDialog(_dataHelper, _entryData, ISOForm.Mode.Patch) == DialogResult.OK)
            {
                //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private void menuItem_LoadISO_Click(object sender, EventArgs e)
        {
            EntryData inputEntryData = _entryData ?? new EntryData(null, null, null);
            if (ISOForm.InitDialog(_dataHelper, inputEntryData, ISOForm.Mode.Load) == DialogResult.OK)
            {
                _entryDataDefault = _entryDataDefault ?? _dataHelper.LoadDefaultEntryData();

                _entryData = _entryData ?? inputEntryData;
                _entryData.BattleConditionals = inputEntryData.BattleConditionals ?? _entryData.BattleConditionals ?? CopyableEntry.CopyList<ConditionalSet>(_entryDataDefault.BattleConditionals);
                _entryData.WorldConditionals = inputEntryData.WorldConditionals ?? _entryData.WorldConditionals ?? CopyableEntry.CopyList<ConditionalSet>(_entryDataDefault.WorldConditionals);
                _entryData.Events = inputEntryData.Events ?? _entryData.Events ?? CopyableEntry.CopyList<Event>(_entryDataDefault.Events);

                PopulateTabs();
                EnableMenu();
                tabControl.Enabled = true;
            }
        }

        private void menuItem_PatchPSXSaveState_Click(object sender, EventArgs e)
        {
            SaveFormData();
            if (StateForm.InitDialog(_dataHelper, _entryData, GetSelectedIndexResult(), StateForm.Mode.Patch) == DialogResult.OK)
            {
                //PatcherLib.MyMessageBox.Show(this, "Complete!", "Complete!", MessageBoxButtons.OK);
            }
        }

        private void menuItem_LoadPSXSaveState_Click(object sender, EventArgs e)
        {
            SaveFormData();
            if (StateForm.InitDialog(_dataHelper, _entryData, GetSelectedIndexResult(), StateForm.Mode.Load) == DialogResult.OK)
            {
                PopulateTabs();
            }
        }

        private void menuItem_About_Click(object sender, EventArgs e)
        {
            PatcherLib.MyMessageBox.Show(this, About.Message, About.Title, MessageBoxButtons.OK);
        }
    }
}
