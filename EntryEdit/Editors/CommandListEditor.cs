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
        const float RowHeight = 30.0F;

        private bool _isPopulate = false;

        private List<Command> _commandList;
        private List<string> _commandNames;
        private int _commandPageSize = DefaultPageSize;
        private int _commandPageIndex = 0;
        private int _commandNumPages = 1;

        public CommandListEditor()
        {
            InitializeComponent();
        }

        public void Init(List<string> commandNames)
        {
            _commandNames = commandNames;
            InitRows();
        }

        public void Populate(List<Command> commandList, int commandPageSize = DefaultPageSize)
        {
            _isPopulate = true;

            _commandList = commandList;
            _commandPageSize = commandPageSize;
            _commandNumPages = (_commandList.Count + _commandPageSize - 1) / _commandPageSize;

            spinner_Page.Value = 1;
            spinner_Page.Maximum = _commandNumPages;

            SetCommandPageIndex(0);
            _isPopulate = false;
        }

        public void Clear()
        {
            _isPopulate = true;

            _commandList = null;
            _commandPageSize = DefaultPageSize;
            _commandNumPages = 1;
            spinner_Page.Value = 1;
            spinner_Page.Maximum = _commandNumPages;
            tlp_Commands.Controls.Clear();

            _isPopulate = false;
        }

        private void InitRows()
        {
            ClearPanel();
            for (int index = 0; index < _commandPageSize; index++)
                AddHiddenCommandRow();
        }

        private void PopulateRows()
        {
            int commandIndex = (_commandPageIndex * _commandPageSize);
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                if (commandIndex < _commandList.Count)
                    SetCommandRow(rowIndex, commandIndex, _commandList[commandIndex]);
                else
                    SetHiddenCommandRow(rowIndex);

                commandIndex++;
            }

            btn_Page_Prev.Enabled = (_commandPageIndex > 0);
            btn_Page_Next.Enabled = (_commandPageIndex < (_commandNumPages - 1));
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
            CheckBox cb = ((CheckBox)(tlp_Commands.Controls[rowIndex * 2]));
            CommandEditor commandEditor = ((CommandEditor)(tlp_Commands.Controls[rowIndex * 2 + 1]));

            cb.Tag = index;
            cb.Visible = true;
            commandEditor.Populate(command);
            commandEditor.Visible = true;
        }

        private void SetHiddenCommandRow(int rowIndex)
        {
            tlp_Commands.RowStyles[rowIndex].Height = 0.0F;
            CheckBox cb = ((CheckBox)(tlp_Commands.Controls[rowIndex * 2]));
            CommandEditor commandEditor = ((CommandEditor)(tlp_Commands.Controls[rowIndex * 2 + 1]));
            cb.Visible = false;
            commandEditor.Visible = false;
        }

        private void AddHiddenCommandRow()
        {
            CheckBox cb = new CheckBox();
            cb.AutoSize = true;
            cb.Visible = false;

            CommandEditor commandEditor = new CommandEditor();
            commandEditor.Visible = false;
            commandEditor.InitCommandList(_commandNames);

            tlp_Commands.RowCount++;
            tlp_Commands.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0F));
            tlp_Commands.Controls.Add(cb);
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
            {
                spinner_Page.Value = spinner_Page.Value - 1;
            }
        }

        private void btn_Page_Next_Click(object sender, EventArgs e)
        {
            if (_commandPageIndex < (_commandNumPages - 1))
            {
                spinner_Page.Value = spinner_Page.Value + 1;
            }
        }
    }
}
