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

        public ConditionalSetEditor()
        {
            InitializeComponent();
        }

        public void Populate(ConditionalSet conditionalSet)
        {
            _conditionalSet = conditionalSet;
        }
    }
}
