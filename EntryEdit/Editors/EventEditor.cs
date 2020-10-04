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

            commandListEditor.Populate(_event.CommandList, _defaultEvent.CommandList);
            textSectionEditor.Populate(new CustomSection[2] { _event.TextSection, _event.DataSection }, _event.OriginalTextSection,
                new CustomSection[2] { _defaultEvent.TextSection, _defaultEvent.DataSection }, _defaultEvent.OriginalTextSection);
        }

        public List<Command> CopyCommandList()
        {
            commandListEditor.SavePage();
            return CopyableEntry.CopyList<Command>(commandListEditor.CommandList);
        }

        public void RefreshCommandList()
        {
            commandListEditor.Populate(_event.CommandList, _defaultEvent.CommandList);
        }

        public void SavePage()
        {
            commandListEditor.SavePage();
            textSectionEditor.SaveEntry();
        }
    }
}
