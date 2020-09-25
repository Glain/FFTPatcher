using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class ConditionalSetEditor : UserControl
    {
        private ConditionalSet _conditionalSet;
        private ConditionalSet _defaultConditionalSet;

        private CommandData _commandData;

        private int _blockIndex = -1;
        public int BlockIndex
        {
            get { return _blockIndex; } 
        }

        private int _maxBlocks = -1;

        private bool _isPopulate = false;

        public ConditionalSetEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandData commandData, int maxBlocks = -1)
        {
            this._commandData = commandData;
            this._maxBlocks = maxBlocks;

            commandListEditor.Init(commandData);
            //commandListEditor.SetSaveCallback(FindBlockName);
        }

        public void Populate(ConditionalSet conditionalSet, ConditionalSet defaultConditionalSet)
        {
            this._conditionalSet = conditionalSet;
            this._defaultConditionalSet = defaultConditionalSet;

            PopulateBlocks();
        }

        public void PopulateBlocks(int blockIndex = 0, bool reloadCommandList = true)
        {
            _isPopulate = true;

            if (_conditionalSet.ConditionalBlocks.Count > 0)
            {
                cmb_Block.Items.Clear();
                cmb_Block.Items.AddRange(_conditionalSet.ConditionalBlocks.ToArray());
                cmb_Block.SelectedIndex = blockIndex;
                SetBlockIndex(blockIndex, reloadCommandList);
                btn_Delete.Enabled = true;
                btn_Add.Enabled = ((_maxBlocks <= 0) || (_conditionalSet.ConditionalBlocks.Count < (_maxBlocks - 1)));
            }
            else
            {
                ClearBlock();
            }

            _isPopulate = false;
        }

        public void SaveBlock()
        {
            commandListEditor.SavePage();
            FindBlockName();
        }

        public void ClearBlock()
        {
            cmb_Block.Items.Clear();
            cmb_Block.SelectedIndex = -1;
            _blockIndex = -1;
            commandListEditor.Clear();
            commandListEditor.SetEnabledState(false);
            btn_Delete.Enabled = false;
            btn_Add.Enabled = true;
        }

        public string GetCommandListScript()
        {
            return ((_conditionalSet != null) && (_blockIndex >= 0)) ? _conditionalSet.ConditionalBlocks[_blockIndex].GetScript() : string.Empty;   
        }

        public void LoadBlock(ConditionalBlock block)
        {
            if ((_conditionalSet != null) && (_blockIndex >= 0))
            {
                _conditionalSet.ConditionalBlocks[_blockIndex] = block;
                PopulateBlocks(_blockIndex);
            }
        }

        public void SetEnabledState(bool isEnabled)
        {
            if (isEnabled)
            {
                btn_Add.Enabled = true;
                btn_Delete.Enabled = (_conditionalSet.ConditionalBlocks.Count > 0);
            }
            else
            {
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
            }
        }

        private void SetBlockIndex(int index, bool reloadCommandList = true)
        {
            _blockIndex = index;
            List<Command> defaultCommandList = ((_defaultConditionalSet != null) && (index < _defaultConditionalSet.ConditionalBlocks.Count)) 
                ? _defaultConditionalSet.ConditionalBlocks[index].Commands : new List<Command>();

            if (reloadCommandList)
            {
                commandListEditor.Populate(_conditionalSet.ConditionalBlocks[index].Commands, defaultCommandList);
            }
            else
            {
                commandListEditor.SetDefaultCommandList(defaultCommandList);
            }
        }

        private void FindBlockName()
        {
            if ((_conditionalSet != null) && (_blockIndex >= 0))
            {
                bool isBlockSelected = (cmb_Block.SelectedIndex == _blockIndex);
                _conditionalSet.ConditionalBlocks[_blockIndex].FindName(_commandData.ParameterValueMaps);
                cmb_Block.Items.Remove(_conditionalSet.ConditionalBlocks[_blockIndex]);
                cmb_Block.Items.Insert(_blockIndex, _conditionalSet.ConditionalBlocks[_blockIndex]);

                if (isBlockSelected)
                {
                    bool tempIsPopulate = _isPopulate;
                    _isPopulate = true;
                    cmb_Block.SelectedIndex = _blockIndex;
                    _isPopulate = tempIsPopulate;
                }
            }
        }

        private void SwapBlockByOffset(int offset)
        {
            if (PatcherLib.Utilities.Utilities.SafeSwap<ConditionalBlock>(_conditionalSet.ConditionalBlocks, _blockIndex, _blockIndex + offset))
            {
                _conditionalSet.ConditionalBlocks[_blockIndex].AddOffsetToIndex(-offset);
                _conditionalSet.ConditionalBlocks[_blockIndex + offset].AddOffsetToIndex(offset);
            }
        }

        private void cmb_Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Block.SelectedIndex != _blockIndex)
            {
                if (!_isPopulate)
                {
                    SaveBlock();
                    SetBlockIndex(cmb_Block.SelectedIndex);
                }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (_conditionalSet.ConditionalBlocks.Count > 0)
            {
                _conditionalSet.ConditionalBlocks.RemoveAt(_blockIndex);

                if (_conditionalSet.ConditionalBlocks.Count > 0)
                {
                    bool isNotFirstIndex = (_blockIndex > 0);
                    int newIndex = isNotFirstIndex ? (_blockIndex - 1) : 0;
                    int startIndex = isNotFirstIndex ? (newIndex + 1) : 0;

                    for (int index = startIndex; index < _conditionalSet.ConditionalBlocks.Count; index++)
                        _conditionalSet.ConditionalBlocks[index].DecrementIndex();

                    PopulateBlocks(newIndex);
                }
                else
                {
                    ClearBlock();
                }
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if ((_maxBlocks <= 0) || (_conditionalSet.ConditionalBlocks.Count < (_maxBlocks - 1)))
            {
                SaveBlock();

                int newIndex = _blockIndex + 1;
                _conditionalSet.ConditionalBlocks.Insert(newIndex, new ConditionalBlock(newIndex, new List<Command>(), string.Empty));

                for (int index = newIndex + 1; index < _conditionalSet.ConditionalBlocks.Count; index++)
                    _conditionalSet.ConditionalBlocks[index].IncrementIndex();

                PopulateBlocks(newIndex);
            }
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {
            if (_blockIndex > 0)
            {
                SwapBlockByOffset(-1);
                PopulateBlocks(_blockIndex - 1, false);
            }
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            if (_blockIndex < (_conditionalSet.ConditionalBlocks.Count - 1))
            {
                SwapBlockByOffset(1);
                PopulateBlocks(_blockIndex + 1, false);
            }
        }
    }
}
