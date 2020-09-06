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
        private DataHelper _dataHelper = null;
        public DataHelper DataHelper
        {
            get
            {
                if (_dataHelper == null)
                    _dataHelper = new DataHelper();

                return _dataHelper;
            }
        }

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
            _entryData = DataHelper.LoadDefaultEntryData();
            _entryDataDefault = _entryData.Copy();

            battleConditionalSetsEditor.Populate(_entryData.BattleConditionals);
            worldConditionalSetsEditor.Populate(_entryData.WorldConditionals);
            eventsEditor.Populate(_entryData.Events);
        }

        private void WriteByteDataToTestFiles()
        {
            System.IO.File.WriteAllBytes("EntryData/TestBattle.bin", DataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, _entryData.BattleConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestWorld.bin", DataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, _entryData.WorldConditionals));
            System.IO.File.WriteAllBytes("EntryData/TestEvents.bin", DataHelper.EventsToByteArray(_entryData.Events));
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
