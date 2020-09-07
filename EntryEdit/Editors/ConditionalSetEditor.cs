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
        private List<string> _commandNames;
        private int _blockIndex = -1;
        private bool _isPopulate = false;

        public ConditionalSetEditor()
        {
            InitializeComponent();
        }

        public void Init(List<string> commandNames, Dictionary<string, Dictionary<int, string>> parameterValueMaps)
        {
            this._commandNames = commandNames;
            commandListEditor.Init(commandNames, parameterValueMaps);
        }

        public void Populate(ConditionalSet conditionalSet)
        {
            _isPopulate = true;

            this._conditionalSet = conditionalSet;

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
