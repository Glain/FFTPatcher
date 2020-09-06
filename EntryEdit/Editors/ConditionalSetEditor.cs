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
        private int _blockIndex = -1;

        public ConditionalSetEditor()
        {
            InitializeComponent();
        }

        public void Populate(ConditionalSet conditionalSet)
        {
            this._conditionalSet = conditionalSet;

            cmb_Block.Items.Clear();
            cmb_Block.Items.AddRange(conditionalSet.ConditionalBlocks.ToArray());

            if (conditionalSet.ConditionalBlocks.Count > 0)
            {
                cmb_Block.SelectedIndex = 0;
            }
            else
            {
                cmb_Block.SelectedIndex = -1;
                _blockIndex = -1;
                commandListEditor.Clear();
            }
        }

        private void cmb_Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            _blockIndex = cmb_Block.SelectedIndex;
            commandListEditor.Populate(_conditionalSet.ConditionalBlocks[_blockIndex].Commands);
        }
    }
}
