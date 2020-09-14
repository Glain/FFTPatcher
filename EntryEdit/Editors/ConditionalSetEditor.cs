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
            _isPopulate = true;

            this._conditionalSet = conditionalSet;
            this._defaultConditionalSet = defaultConditionalSet;

            cmb_Block.Items.Clear();
            cmb_Block.Items.AddRange(conditionalSet.ConditionalBlocks.ToArray());

            if (conditionalSet.ConditionalBlocks.Count > 0)
            {
                cmb_Block.SelectedIndex = 0;
                SetBlockIndex(0);
            }
            else
            {
                cmb_Block.SelectedIndex = -1;
                _blockIndex = -1;
                commandListEditor.Clear();
            }

            _isPopulate = false;
        }

        private void SetBlockIndex(int index)
        {
            _blockIndex = index;
            commandListEditor.Populate(_conditionalSet.ConditionalBlocks[index].Commands);
        }

        private void cmb_Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
                SetBlockIndex(cmb_Block.SelectedIndex);
        }
    }
}
