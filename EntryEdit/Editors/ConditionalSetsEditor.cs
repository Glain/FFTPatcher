using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class ConditionalSetsEditor : UserControl
    {
        private List<ConditionalSet> _conditionalSets;
        private int _conditionalSetIndex = 0;

        public ConditionalSetsEditor()
        {
            InitializeComponent();
        }

        public void Populate(List<ConditionalSet> conditionalSets)
        {
            this._conditionalSets = conditionalSets;

            cmb_ConditionalSet.Items.Clear();
            cmb_ConditionalSet.Items.AddRange(_conditionalSets.ToArray());
            cmb_ConditionalSet.SelectedIndex = 0;
        }

        private void cmb_ConditionalSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            _conditionalSetIndex = cmb_ConditionalSet.SelectedIndex;
            conditionalSetEditor.Populate(_conditionalSets[_conditionalSetIndex]);
        }
    }
}
