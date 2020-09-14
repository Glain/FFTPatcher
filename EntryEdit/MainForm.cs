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

        private EntryData _entryData;
        private EntryData _entryDataDefault;

        public MainForm()
        {
            InitializeComponent();
            Start();
            //WriteByteDataToTestFiles();
        }

        private void Start()
        {
            _dataHelper = new DataHelper();

            _entryData = _dataHelper.LoadDefaultEntryData();
            _entryDataDefault = _entryData.Copy();

            Dictionary<CommandType, List<string>> commandNames = _dataHelper.GetCommandNames();
            Dictionary<string, Dictionary<int, string>> parameterValueMaps = _dataHelper.GetParameterMaps();
            Dictionary<CommandType, Dictionary<int, CommandTemplate>> commandMaps = _dataHelper.GetCommandMaps();
            Dictionary<CommandType, int> defaultCommandByteLengthMaps = _dataHelper.GetDefaultCommandByteLengthMaps();

            battleConditionalSetsEditor.Populate(_entryData.BattleConditionals, _entryDataDefault.BattleConditionals, CommandType.BattleConditional, defaultCommandByteLengthMaps[CommandType.BattleConditional], commandMaps[CommandType.BattleConditional], commandNames[CommandType.BattleConditional], parameterValueMaps, _dataHelper.GetParameterMax(CommandType.BattleConditional));
            worldConditionalSetsEditor.Populate(_entryData.WorldConditionals, _entryDataDefault.WorldConditionals, CommandType.WorldConditional, defaultCommandByteLengthMaps[CommandType.WorldConditional], commandMaps[CommandType.WorldConditional], commandNames[CommandType.WorldConditional], parameterValueMaps, _dataHelper.GetParameterMax(CommandType.WorldConditional));
            eventsEditor.Populate(_entryData.Events, _entryDataDefault.Events, CommandType.EventCommand, defaultCommandByteLengthMaps[CommandType.EventCommand], commandMaps[CommandType.EventCommand], commandNames[CommandType.EventCommand], parameterValueMaps, _dataHelper.GetParameterMax(CommandType.EventCommand));
        }

        private void WriteByteDataToTestFiles()
        {
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

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
