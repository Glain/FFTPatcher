using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EntryEdit
{
    public partial class MainForm : Form
    {
        private DataHelper dataHelper;

        public MainForm()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            dataHelper = new DataHelper();
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
