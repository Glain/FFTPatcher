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

            cmb_Event.Items.Clear();
            cmb_Event.Items.AddRange(_conditionalSets.ToArray());
            cmb_Event.SelectedIndex = 0;

            _conditionalSetIndex = 0;
            conditionalSetEditor.Populate(_conditionalSets[_conditionalSetIndex]);
        }

        private void cmb_Event_SelectedIndexChanged(object sender, EventArgs e)
        {
            _conditionalSetIndex = cmb_Event.SelectedIndex;
            conditionalSetEditor.Populate(_conditionalSets[_conditionalSetIndex]);
        }
    }
}
