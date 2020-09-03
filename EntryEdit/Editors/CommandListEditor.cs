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
        private List<Command> _commandList;

        public CommandListEditor()
        {
            InitializeComponent();
        }

        public void Populate(List<Command> commandList)
        {
            _commandList = commandList;
        }

        public void Clear()
        {
            _commandList = null;
        }
    }
}
