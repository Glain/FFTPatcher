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

        private Dictionary<int, CommandTemplate> _commandMap;
        private List<string> _commandNames;

        public EventEditor()
        {
            InitializeComponent();
        }

        public void Init(CommandType commandType, int defaultCommandByteLength, Dictionary<int, CommandTemplate> commandMap, List<string> commandNames, Dictionary<string, Dictionary<int, string>> parameterValueMaps, int maxParameters)
        {
            _commandMap = commandMap;
            _commandNames = commandNames;
            commandListEditor.Init(commandType, defaultCommandByteLength, commandMap, commandNames, parameterValueMaps, maxParameters);
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
