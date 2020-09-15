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

        private CommandData _commandData;

        private int _commandPageSize = DefaultPageSize;
        private int _commandPageIndex = 0;
        private int _commandNumPages = 1;

        public CommandListEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandData commandData, int commandPageSize = DefaultPageSize)
        {
            this._commandData = commandData;
            this._commandPageSize = commandPageSize;
            InitRows(commandData);
        }

        public void Populate(List<Command> commandList, int pageIndex = 0)
        {
            _isPopulate = true;

            _commandList = commandList;
            SetCommandListFormProperties(pageIndex);

            SetCommandPageIndex(pageIndex);
            _isPopulate = false;
        }

        public void Clear()
        {
            Populate(new List<Command>());
        }

        public void SavePage()
        {
            int commandIndex = (_commandPageIndex * _commandPageSize);
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                if (commandIndex < _commandList.Count)
                {
                    CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
                    commandEditor.SaveFormCommand();
                    _commandList[commandIndex] = commandEditor.Command;
                }

                commandIndex++;
            }
        }

        public void SetEnabledState(bool state)
        {
            SetInputControlEnabledState(state);
        }

        private void InitRows(CommandData commandData)
        {
            ClearPanel();
            for (int index = 0; index < _commandPageSize; index++)
                AddHiddenCommandRow(commandData);
        }

        private void PopulateRows()
        {
            SetInputControlEnabledState(false);

            int commandIndex = (_commandPageIndex * _commandPageSize);
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                if (commandIndex < _commandList.Count)
                    SetCommandRow(rowIndex, _commandList[commandIndex]);
                else
                    SetHiddenCommandRow(rowIndex);

                commandIndex++;
            }

            SetInputControlEnabledState(true);
        }

        private void ClearPanel()
        {
            for (int i = tlp_Commands.Controls.Count - 1; i >= 0; i--)
                tlp_Commands.Controls[i].Dispose();

            tlp_Commands.Controls.Clear();
            tlp_Commands.RowCount = 0;
        }

        private void SetCommandListFormProperties(int pageIndex)
        {
            _commandNumPages = (_commandList.Count + _commandPageSize - 1) / _commandPageSize;

            int minPageValue = Math.Min(_commandNumPages, (pageIndex + 1));
            spinner_Page.Minimum = minPageValue;
            spinner_Page.Maximum = _commandNumPages;
            spinner_Page.Value = minPageValue;

            btn_Delete.Enabled = (_commandList.Count > 0);
        }

        private void SetInputControlEnabledState(bool isNormal)
        {
            if (isNormal)
            {
                bool isNotFirstPage = (_commandPageIndex > 0);
                bool isNotLastPage = (_commandPageIndex < (_commandNumPages - 1));

                btn_Page_Prev.Enabled = isNotFirstPage;
                btn_Page_Next.Enabled = isNotLastPage;
                btn_Page_First.Enabled = isNotFirstPage;
                btn_Page_Last.Enabled = isNotLastPage;
                btn_Add.Enabled = true;
                btn_Delete.Enabled = (_commandList.Count > 0);
                spinner_Page.Enabled = true;
            }
            else
            {
                btn_Page_Prev.Enabled = false;
                btn_Page_Next.Enabled = false;
                btn_Page_First.Enabled = false;
                btn_Page_Last.Enabled = false;
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
                spinner_Page.Enabled = false;
            }
        }

        private void SetCommandRow(int rowIndex, Command command)
        {
            tlp_Commands.RowStyles[rowIndex].Height = RowHeight;
            CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
            commandEditor.Populate(command);
            commandEditor.Visible = true;
        }

        private void SetHiddenCommandRow(int rowIndex)
        {
            tlp_Commands.RowStyles[rowIndex].Height = 0.0F;
            CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
            commandEditor.Visible = false;
            commandEditor.SetCheckedState(false);
        }

        private void AddHiddenCommandRow(CommandData commandData)
        {
            CommandEditor commandEditor = new CommandEditor();
            commandEditor.Visible = false;
            commandEditor.Init(commandData);
            commandEditor.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            tlp_Commands.RowCount++;
            tlp_Commands.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0F));
            tlp_Commands.Controls.Add(commandEditor);
        }

        private CommandEditor GetRowCommandEditor(int rowIndex)
        {
            return ((CommandEditor)(tlp_Commands.Controls[rowIndex]));
        }

        private void SetCommandPageIndex(int index)
        {
            _commandPageIndex = index;
            PopulateRows();
        }

        private List<int> FindCheckedRowIndexes()
        {
            List<int> result = new List<int>();

            int commandIndex = (_commandPageIndex * _commandPageSize);
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                if (commandIndex < _commandList.Count)
                {
                    if (GetRowCommandEditor(rowIndex).IsChecked())
                        result.Add(rowIndex);
                }

                commandIndex++;
            }

            return result;
        }

        private int GetNumEntriesOnPage(int pageIndex)
        {
            if ((pageIndex >= _commandNumPages) || (pageIndex < 0))
                return 0;
            else if (pageIndex < (_commandNumPages - 1))
                return _commandPageSize;
            else
                return _commandList.Count - (_commandPageSize * (_commandNumPages - 1));
        }

        private void spinner_Page_ValueChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
            {
                SavePage();
                SetCommandPageIndex(((int)spinner_Page.Value) - 1);
            }
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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (_commandList.Count > 0)
            {
                List<int> checkedRowIndexes = FindCheckedRowIndexes();
                if (checkedRowIndexes.Count > 0)
                {
                    SavePage();
                    int numPageEntries = GetNumEntriesOnPage(_commandPageIndex);

                    foreach (int rowIndex in checkedRowIndexes)
                    {
                        _commandList.Remove(GetRowCommandEditor(rowIndex).Command);
                    }

                    int newPageIndex = Math.Max(0, ((checkedRowIndexes.Count == numPageEntries) ? (_commandPageIndex - 1) : _commandPageIndex));
                    Populate(_commandList, newPageIndex);
                }
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Command newCommand = new Command(_commandData.DefaultCommandTemplate);
            List<int> checkedRowIndexes = FindCheckedRowIndexes();
            int pageStartIndex = (_commandPageIndex * _commandPageSize);

            SavePage();
            if (checkedRowIndexes.Count > 0)
            {
                int newIndex = pageStartIndex + checkedRowIndexes[0] + 1;
                _commandList.Insert(newIndex, newCommand);
                bool isNewPage = (checkedRowIndexes[0] == (_commandPageSize - 1));
                int newPageIndex = isNewPage ? (_commandPageIndex + 1) : _commandPageIndex;
                Populate(_commandList, newPageIndex);
            }
            else
            {
                int numPageEntries = GetNumEntriesOnPage(_commandPageIndex);
                int rowIndex = (numPageEntries == _commandPageSize) ? (numPageEntries - 1) : numPageEntries;
                int newIndex = pageStartIndex + rowIndex;
                _commandList.Insert(newIndex, newCommand);

                //Populate(_commandList, _commandPageIndex);
                SetCommandRow(rowIndex, newCommand);
                SetCommandListFormProperties(_commandPageIndex);
                SetInputControlEnabledState(true);
            }
        }

        private void btn_CheckAll_Click(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
                if (commandEditor.Visible)
                    commandEditor.SetCheckedState(true);
            }
        }

        private void btn_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
                if (commandEditor.Visible)
                    commandEditor.SetCheckedState(false);
            }
        }

        private void btn_ToggleAll_Click(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < _commandPageSize; rowIndex++)
            {
                CommandEditor commandEditor = GetRowCommandEditor(rowIndex);
                if (commandEditor.Visible)
                    commandEditor.ToggleCheckedState();
            }
        }
    }
}
