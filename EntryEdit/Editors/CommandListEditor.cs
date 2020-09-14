using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class CommandListEditor : UserControl
    {
        const int DefaultPageSize = 10;
        const float RowHeight = 57.0F;

        private bool _isPopulate = false;

        private List<Command> _commandList;
        public List<Command> CommandList
        {
            get { return _commandList; }
        }

        private CommandType _commandType;
        private int _defaultCommandByteLength;
        private Dictionary<int, CommandTemplate> _commandMap;
        private List<string> _commandNames;
        private Dictionary<string, Dictionary<int, string>> _parameterValueMaps;
        private int _maxParameters = 1;

        private int _commandPageSize = DefaultPageSize;
        private int _commandPageIndex = 0;
        private int _commandNumPages = 1;

        public CommandListEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandType commandType, int defaultCommandByteLength, Dictionary<int, CommandTemplate> commandMap, List<string> commandNames, Dictionary<string, Dictionary<int, string>> parameterValueMaps, int maxParameters, int commandPageSize = DefaultPageSize)
        {
            this._commandType = commandType;
            this._defaultCommandByteLength = defaultCommandByteLength;
            this._commandMap = commandMap;
            this._commandNames = commandNames;
            this._parameterValueMaps = parameterValueMaps;
            this._commandPageSize = commandPageSize;
            this._maxParameters = maxParameters;

            InitRows();
        }

        public void Populate(List<Command> commandList)
        {
            _isPopulate = true;

            _commandList = commandList;
            _commandNumPages = (_commandList.Count + _commandPageSize - 1) / _commandPageSize;

            int minPageValue = Math.Min(_commandNumPages, 1);
            spinner_Page.Minimum = minPageValue;
            spinner_Page.Maximum = _commandNumPages;
            spinner_Page.Value = minPageValue;

            SetCommandPageIndex(0);
            _isPopulate = false;
        }

        public void Clear()
        {
            Populate(new List<Command>());
        }

        private void InitRows()
        {
            ClearPanel();
            for (int index = 0; index < _commandPageSize; index++)
                AddHiddenCommandRow();
        }

        private void PopulateRows()
        {
            btn_Page_Prev.Enabled = false;
            btn_Page_Next.Enabled = false;
            btn_Page_First.Enabled = false;
            btn_Page_Last.Enabled = false;
            spinner_Page.Enabled = false;

            int commandIndex = (_commandPageIndex * _commandPageSize);
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                if (commandIndex < _commandList.Count)
                    SetCommandRow(rowIndex, commandIndex, _commandList[commandIndex]);
                else
                    SetHiddenCommandRow(rowIndex);

                commandIndex++;
            }

            bool isNotFirstPage = (_commandPageIndex > 0);
            bool isNotLastPage = (_commandPageIndex < (_commandNumPages - 1));

            btn_Page_Prev.Enabled = isNotFirstPage;
            btn_Page_Next.Enabled = isNotLastPage;
            btn_Page_First.Enabled = isNotFirstPage;
            btn_Page_Last.Enabled = isNotLastPage;
            spinner_Page.Enabled = true;
        }

        private void ClearPanel()
        {
            for (int i = tlp_Commands.Controls.Count - 1; i >= 0; i--)
                tlp_Commands.Controls[i].Dispose();

            tlp_Commands.Controls.Clear();
            tlp_Commands.RowCount = 0;
        }

        private void SetCommandRow(int rowIndex, int index, Command command)
        {
            tlp_Commands.RowStyles[rowIndex].Height = RowHeight;
            CommandEditor commandEditor = ((CommandEditor)(tlp_Commands.Controls[rowIndex]));
            commandEditor.Populate(command);
            commandEditor.Visible = true;
        }

        private void SetHiddenCommandRow(int rowIndex)
        {
            tlp_Commands.RowStyles[rowIndex].Height = 0.0F;
            CommandEditor commandEditor = ((CommandEditor)(tlp_Commands.Controls[rowIndex]));
            commandEditor.Visible = false;
        }

        private void AddHiddenCommandRow()
        {
            CommandEditor commandEditor = new CommandEditor();
            commandEditor.Visible = false;
            commandEditor.Init(_commandType, _defaultCommandByteLength, _commandMap, _commandNames, _parameterValueMaps, _maxParameters);
            commandEditor.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            tlp_Commands.RowCount++;
            tlp_Commands.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0F));
            tlp_Commands.Controls.Add(commandEditor);
        }

        private void SetCommandPageIndex(int index)
        {
            _commandPageIndex = index;
            PopulateRows();
        }

        private void spinner_Page_ValueChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
                SetCommandPageIndex(((int)spinner_Page.Value) - 1);
        }

        private void btn_Page_Prev_Click(object sender, EventArgs e)
        {
            if (_commandPageIndex > 0)
                spinner_Page.Value = spinner_Page.Value - 1;
        }

        private void btn_Page_Next_Click(object sender, EventArgs e)
        {
            if (_commandPageIndex < (_commandNumPages - 1))
                spinner_Page.Value = spinner_Page.Value + 1;
        }

        private void btn_Page_First_Click(object sender, EventArgs e)
        {
            if (_commandPageIndex > 0)
                spinner_Page.Value = 1;
        }

        private void btn_Page_Last_Click(object sender, EventArgs e)
        {
            if (_commandPageIndex < (_commandNumPages - 1))
                spinner_Page.Value = _commandNumPages;
        }
    }
}
