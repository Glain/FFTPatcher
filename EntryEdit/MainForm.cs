using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EntryEdit
{
    public partial class MainForm : Form
    {
        private DataHelper _dataHelper;
        private Dictionary<CommandType, CommandData> _commandDataMap;

        private EntryData _entryData;
        private EntryData _entryDataDefault;

        private ConditionalSet _battleConditionalSetCopy;
        private ConditionalSet _worldConditionalSetCopy;
        private Event _eventCopy;

        public MainForm()
        {
            InitializeComponent();
            Start();
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

        private void SetDefaults()
        {
            SaveFormData();
            _entryDataDefault = _entryData.Copy();
            PopulateTabs();
        }

        private void RestoreDefaults()
        {
            SaveFormData();
            _entryDataDefault = _dataHelper.LoadDefaultEntryData();
            PopulateTabs();
        }

        private void PopulateTabs()
        {
            battleConditionalSetsEditor.Populate(_entryData.BattleConditionals, _entryDataDefault.BattleConditionals, _commandDataMap[CommandType.BattleConditional]);
            worldConditionalSetsEditor.Populate(_entryData.WorldConditionals, _entryDataDefault.WorldConditionals, _commandDataMap[CommandType.WorldConditional]);
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

        private void EnableMenu()
        {
            menuItem_Edit.Enabled = true;
            menuItem_View.Enabled = true;
            menuItem_LoadScript.Enabled = true;
            menuItem_SaveScript.Enabled = true;
        }

        private void WriteByteDataToTestFiles()
        {
            _entryData = _dataHelper.LoadDefaultEntryData();
            _entryDataDefault = _entryData.Copy();
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

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItem_Copy_Click(object sender, EventArgs e)
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

        private void menuItem_Paste_Click(object sender, EventArgs e)
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

        private void menuItem_CheckSize_Click(object sender, EventArgs e)
        {
            if (menuItem_View.Enabled)
            {
                if (tabControl.SelectedTab == tabPage_BattleConditionals)
                {
                    battleConditionalSetsEditor.SaveBlock();
                    byte[] bytes = _dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, _entryData.BattleConditionals);
                    PatcherLib.MyMessageBox.Show(this, string.Format("All Battle Conditionals Size: {0} / {1} bytes", bytes.Length, Settings.BattleConditionalsSize), "Size", MessageBoxButtons.OK);
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
                    byte[] bytes = _dataHelper.EventToByteArray(eventsEditor.CopyEvent(), false);
                    PatcherLib.MyMessageBox.Show(this, string.Format("Event Size: {0} / {1} bytes", bytes.Length, Settings.EventSize), "Size", MessageBoxButtons.OK);
                }
            }
        }
    }
}
