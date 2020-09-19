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
        private List<Event> _defaultEvents;

        private int _eventIndex = 0;
        private bool _isPopulate = false;

        public EventsEditor()
        {
            InitializeComponent();
        }

        public void Populate(List<Event> events, List<Event> defaultEvents, CommandData commandData)
        {
            this._events = events;
            this._defaultEvents = defaultEvents;

            eventEditor.Init(commandData);
            PopulateEvents();
        }

        public void SavePage()
        {
            eventEditor.SavePage();
        }

        public Event CopyEvent()
        {
            return (_eventIndex >= 0) ? _events[_eventIndex].Copy() : null;
        }

        public void PasteEvent(Event inputEvent)
        {
            if ((_eventIndex >= 0) && (inputEvent != null))
            {
                _events[_eventIndex] = inputEvent.Copy();
                _events[_eventIndex].Index = _eventIndex;
                PopulateEvents(_eventIndex);
            }
        }

        private void PopulateEvents(int eventIndex = 0, bool reloadEvent = true)
        {
            _isPopulate = true;

            cmb_Event.Items.Clear();
            cmb_Event.Items.AddRange(_events.ToArray());
            cmb_Event.SelectedIndex = eventIndex;
            SetEventIndex(eventIndex, reloadEvent);

            _isPopulate = false;
        }

        private void SetEventIndex(int index, bool reloadEvent = true)
        {
            _eventIndex = index;

            if (reloadEvent)
                eventEditor.Populate(_events[index], EntryData.GetEntry<Event>(_defaultEvents, index));
        }

        private void SwapEventByOffset(int offset)
        {
            if (PatcherLib.Utilities.Utilities.SafeSwap<Event>(_events, _eventIndex, _eventIndex + offset))
            {
                _events[_eventIndex].AddOffsetToIndex(-offset);
                _events[_eventIndex + offset].AddOffsetToIndex(offset);
            }
        }

        private void Clear()
        {
            if ((_events[_eventIndex].CommandList.Count > 0) || (_events[_eventIndex].TextSection.CustomEntryList.Count > 0) || (_events[_eventIndex].DataSection.CustomEntryList.Count > 0))
            {
                _events[_eventIndex].CommandList.Clear();
                _events[_eventIndex].TextSection.CustomEntryList.Clear();
                _events[_eventIndex].DataSection.CustomEntryList.Clear();
                PopulateEvents(_eventIndex);
            }
        }

        private void Reload()
        {
            if ((_defaultEvents != null) && (_eventIndex < _defaultEvents.Count))
            {
                _events[_eventIndex].CommandList.Clear();
                _events[_eventIndex].CommandList.AddRange(CopyableEntry.CopyList<Command>(_defaultEvents[_eventIndex].CommandList));
                _events[_eventIndex].TextSection.CustomEntryList.Clear();
                _events[_eventIndex].TextSection.CustomEntryList.AddRange(CopyableEntry.CopyList<CustomEntry>(_defaultEvents[_eventIndex].TextSection.CustomEntryList));
                _events[_eventIndex].DataSection.CustomEntryList.Clear();
                _events[_eventIndex].DataSection.CustomEntryList.AddRange(CopyableEntry.CopyList<CustomEntry>(_defaultEvents[_eventIndex].DataSection.CustomEntryList));
                _events[_eventIndex].Name = _defaultEvents[_eventIndex].Name;
                PopulateEvents(_eventIndex);
            }
        }

        private void cmb_Event_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Event.SelectedIndex != _eventIndex)
            {
                if (!_isPopulate)
                {
                    eventEditor.SavePage();
                    SetEventIndex(cmb_Event.SelectedIndex);
                }
            }
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {
            if (_eventIndex > 0)
            {
                SwapEventByOffset(-1);
                PopulateEvents(_eventIndex - 1, false);
            }
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            if (_eventIndex < (_events.Count - 1))
            {
                SwapEventByOffset(1);
                PopulateEvents(_eventIndex + 1, false);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_Reload_Click(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
