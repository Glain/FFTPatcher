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

        private List<string> _commandNames;

        public EventEditor()
        {
            InitializeComponent();
        }

        public void Init(List<string> commandNames, Dictionary<string, Dictionary<int, string>> parameterValueMaps, int maxParameters)
        {
            _commandNames = commandNames;
            commandListEditor.Init(commandNames, parameterValueMaps, maxParameters);
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
