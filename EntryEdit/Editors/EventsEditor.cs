using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class EventsEditor : UserControl
    {
        private List<Event> _events;
        private List<string> _commandNames;
        private int _eventIndex = 0;
        private bool _isPopulate = false;

        public EventsEditor()
        {
            InitializeComponent();
        }

        public void Populate(List<Event> events, List<string> commandNames)
        {
            _isPopulate = true;

            this._events = events;
            this._commandNames = commandNames;

            eventEditor.Init(commandNames);

            cmb_Event.Items.Clear();
            cmb_Event.Items.AddRange(_events.ToArray());
            cmb_Event.SelectedIndex = 0;

            SetEventIndex(0);
            _isPopulate = false;
        }

        private void SetEventIndex(int index)
        {
            _eventIndex = index;
            eventEditor.Populate(_events[index]);
        }

        private void cmb_Event_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
                SetEventIndex(cmb_Event.SelectedIndex);
        }
    }
}
