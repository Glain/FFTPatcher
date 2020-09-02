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
        private int _eventIndex = 0;

        public EventsEditor()
        {
            InitializeComponent();
        }

        public void Populate(List<Event> events)
        {
            this._events = events;

            cmb_Event.Items.Clear();
            cmb_Event.Items.AddRange(_events.ToArray());
            cmb_Event.SelectedIndex = 0;

            _eventIndex = 0;
            eventEditor.Populate(_events[_eventIndex]);
        }

        private void cmb_Event_SelectedIndexChanged(object sender, EventArgs e)
        {
            _eventIndex = cmb_Event.SelectedIndex;
            eventEditor.Populate(_events[_eventIndex]);
        }
    }
}
