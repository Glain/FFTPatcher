using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class EventEditor : UserControl
    {
        private Event _event;

        public EventEditor()
        {
            InitializeComponent();
        }

        public void Populate(Event inputEvent)
        {
            _event = inputEvent;
            commandListEditor.Populate(inputEvent.CommandList);
            textSectionEditor.Populate(new CustomSection[2] { inputEvent.TextSection, inputEvent.DataSection });
        }
    }
}
