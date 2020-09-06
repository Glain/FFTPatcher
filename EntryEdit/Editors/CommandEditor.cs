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
    public partial class CommandEditor : UserControl
    {
        private Command _command;

        public CommandEditor()
        {
            InitializeComponent();
        }

        public void Populate(Command command)
        {
            _command = command;
        }
    }
}
