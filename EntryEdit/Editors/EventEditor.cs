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
        private Event _defaultEvent;

        public EventEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandData commandData)
        {
            commandListEditor.Init(commandData);
        }

        public void Populate(Event inputEvent, Event defaultEvent)
        {
            this._event = inputEvent;
            this._defaultEvent = defaultEvent;

            commandListEditor.Populate(inputEvent.CommandList);
            textSectionEditor.Populate(new CustomSection[2] { inputEvent.TextSection, inputEvent.DataSection });
        }
    }
}
