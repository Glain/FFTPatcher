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

        private int _blockIndex = -1;
        private bool _isPopulate = false;

        public ConditionalSetEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandData commandData)
        {
            commandListEditor.Init(commandData);
        }

        public void Populate(ConditionalSet conditionalSet, ConditionalSet defaultConditionalSet)
        {
            this._conditionalSet = conditionalSet;
            this._defaultConditionalSet = defaultConditionalSet;

            PopulateBlocks();
        }

        public void PopulateBlocks(int blockIndex = 0)
        {
            _isPopulate = true;

            if (_conditionalSet.ConditionalBlocks.Count > 0)
            {
                cmb_Block.Items.Clear();
                cmb_Block.Items.AddRange(_conditionalSet.ConditionalBlocks.ToArray());
                cmb_Block.SelectedIndex = blockIndex;
                SetBlockIndex(blockIndex);
                btn_Delete.Enabled = true;
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
        }

        public void ClearBlock()
        {
            cmb_Block.Items.Clear();
            cmb_Block.SelectedIndex = -1;
            _blockIndex = -1;
            commandListEditor.Clear();
            commandListEditor.SetEnabledState(false);
            btn_Delete.Enabled = false;
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

        private void SetBlockIndex(int index)
        {
            _blockIndex = index;
            commandListEditor.Populate(_conditionalSet.ConditionalBlocks[index].Commands);
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
                    bool isFirstIndex = (_blockIndex > 0);
                    int newIndex = isFirstIndex ? (_blockIndex - 1) : 0;
                    int startIndex = isFirstIndex ? (newIndex + 1) : 0;

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
            SaveBlock();

            int newIndex = _blockIndex + 1;
            _conditionalSet.ConditionalBlocks.Insert(newIndex, new ConditionalBlock(newIndex, new List<Command>()));

            for (int index = newIndex + 1; index < _conditionalSet.ConditionalBlocks.Count; index++)
                _conditionalSet.ConditionalBlocks[index].IncrementIndex();

            PopulateBlocks(newIndex);
        }
    }
}
