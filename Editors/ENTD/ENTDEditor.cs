/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class ENTDEditor : UserControl, IHandleSelectedIndex
    {
		#region Instance Variables 

        private Context ourContext = Context.Default;

        private AllJobs _jobs;
        private AllSkillSets _skillSets;
        private AllAbilities _abilities;

        #endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                eventEditor1.ToolTip = value;
            }
        }

		#region Public Properties

        public Event ClipBoardEvent { get; private set; }

        public int SelectedIndex { get { return eventListBox.SelectedIndex; } set { eventListBox.SelectedIndex = value; } }

        #endregion Public Properties 

        #region Constructors (1) 

        public ENTDEditor()
        {
            InitializeComponent();
            eventEditor1.DataChanged += new System.EventHandler( eventEditor1_DataChanged );
            eventListBox.ContextMenu = new ContextMenu(
                new MenuItem[] { new MenuItem( "Clone", CopyClickEventHandler ), new MenuItem( "Paste clone", PasteClickEventHandler ) } );
            eventListBox.ContextMenu.MenuItems[1].Enabled = false;
            eventListBox.MouseDown += new MouseEventHandler( eventListBox_MouseDown );
        }

		#endregion Constructors 

		#region Public Methods

        public void UpdateView( AllENTDs entds, AllJobs jobs, AllSkillSets skillSets, AllAbilities abilities, Context context )
        {
            if( ourContext != context )
            {
                ourContext = context;
                ClipBoardEvent = null;
                eventListBox.ContextMenu.MenuItems[1].Enabled = false;
            }

            _jobs = jobs;
            _skillSets = skillSets;
            _abilities = abilities;

            eventListBox.SelectedIndexChanged -= eventListBox_SelectedIndexChanged;
            eventListBox.DataSource = entds.Events;
            eventListBox.SelectedIndex = 0;
            //eventEditor1.Event = eventListBox.SelectedItem as Event;
            eventEditor1.SetEvent(eventListBox.SelectedItem as Event, context);
            eventListBox.SelectedIndexChanged += eventListBox_SelectedIndexChanged;

            eventListBox.SetChangedColors();
        }

        public void UpdateSelectedEntry()
        {
            eventEditor1.UpdateView(ourContext);
        }
        public void UpdateListBox()
        {
            eventListBox.SetChangedColors();
        }

        public void SetListBoxHighlightedIndexes(IEnumerable<int> highlightedIndexes)
        {
            eventListBox.SetHighlightedIndexes(highlightedIndexes);
        }

        public void ClearListBoxHighlightedIndexes()
        {
            eventListBox.ClearHighlightedIndexes();
        }

        #endregion Public Methods 

        #region Private Methods

        private void CopyClickEventHandler( object sender, System.EventArgs args )
        {
            eventListBox.ContextMenu.MenuItems[1].Enabled = true;
            ClipBoardEvent = eventListBox.SelectedItem as Event;
        }

        private void eventEditor1_DataChanged( object sender, System.EventArgs e )
        {
            eventListBox.BeginUpdate();
            var top = eventListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[eventListBox.DataSource];
            cm.Refresh();
            eventListBox.TopIndex = top;
            eventListBox.EndUpdate();
            eventListBox.SetChangedColor();

            UpdateReferences(eventListBox.SelectedIndex);
        }

        private void eventListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                eventListBox.SelectedIndex = eventListBox.IndexFromPoint( e.Location );
            }
        }

        private void eventListBox_SelectedIndexChanged( object sender, System.EventArgs e )
        {
            eventEditor1.Event = eventListBox.SelectedItem as Event;
        }

        private void PasteClickEventHandler( object sender, System.EventArgs args )
        {
            if( ClipBoardEvent != null )
            {
                ClipBoardEvent.CopyTo( eventListBox.SelectedItem as Event );
                eventEditor1.Event = eventListBox.SelectedItem as Event;
                eventEditor1.UpdateView(ourContext);
                eventEditor1_DataChanged( eventEditor1, System.EventArgs.Empty );
            }
        }

        private void eventListBox_KeyDown( object sender, KeyEventArgs args )
		{
            if (args.KeyCode == Keys.Escape)
            {
                ClearListBoxHighlightedIndexes();
                eventListBox.SetChangedColors();
                eventListBox.Invalidate();
            }
            else if (args.KeyCode == Keys.C && args.Control)
				CopyClickEventHandler( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClickEventHandler( sender, args );
		}

        #endregion Private Methods 

        private void UpdateReferences(int eventIndex)
        {
            if (eventIndex < 0)
                return;

            Event entdEvent = ((IList<Event>)eventListBox.DataSource)[eventIndex];

            List<int> jobIDsToCheck = new List<int>();
            List<int> skillSetsToCheck = new List<int>();
            List<int> abilityIDsToCheck = new List<int>();

            foreach (EventUnit eventUnit in entdEvent.Units)
            {
                if (eventUnit.SpriteSet.Value != 0)
                {
                    if (eventUnit.OldJob != eventUnit.Job)
                    {
                        _jobs.Jobs[eventUnit.Job.Value].ReferencingENTDs.Add(eventIndex);
                        jobIDsToCheck.Add(eventUnit.OldJob.Value);
                    }

                    if (eventUnit.OldSkillSet != eventUnit.SkillSet)
                    {
                        if ((eventUnit.SkillSet.Value != 0) && (eventUnit.SkillSet.Value < 0xB0))
                            _skillSets.SkillSets[eventUnit.SkillSet.Value].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.OldSkillSet.Value != 0) && (eventUnit.OldSkillSet.Value < 0xB0))
                            skillSetsToCheck.Add(eventUnit.OldSkillSet.Value);
                    }
                    if (eventUnit.OldSecondaryAction != eventUnit.SecondaryAction)
                    {
                        if ((eventUnit.SecondaryAction.Value != 0) && (eventUnit.SecondaryAction.Value < 0xB0))
                            _skillSets.SkillSets[eventUnit.SecondaryAction.Value].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.OldSecondaryAction.Value != 0) && (eventUnit.OldSecondaryAction.Value < 0xB0))
                            skillSetsToCheck.Add(eventUnit.OldSecondaryAction.Value);
                    }

                    if (eventUnit.OldReaction != eventUnit.Reaction)
                    {
                        if ((eventUnit.Reaction.Offset != 0) && (eventUnit.Reaction.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Reaction.Offset].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.OldReaction.Offset != 0) && (eventUnit.OldReaction.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.OldReaction.Offset);
                    }
                    if (eventUnit.OldSupport != eventUnit.Support)
                    {
                        if ((eventUnit.Support.Offset != 0) && (eventUnit.Support.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Support.Offset].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.OldSupport.Offset != 0) && (eventUnit.OldSupport.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.OldSupport.Offset);
                    }
                    if (eventUnit.OldMovement != eventUnit.Movement)
                    {
                        if ((eventUnit.Movement.Offset != 0) && (eventUnit.Movement.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Movement.Offset].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.OldMovement.Offset != 0) && (eventUnit.OldMovement.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.OldMovement.Offset);
                    }
                }

                if (eventUnit.OldSpriteSet != eventUnit.SpriteSet)
                {
                    if (eventUnit.SpriteSet.Value != 0)
                    {
                        _jobs.Jobs[eventUnit.Job.Value].ReferencingENTDs.Add(eventIndex);

                        byte primary = eventUnit.SkillSet.Value;
                        if ((primary != 0) && (primary < 0xB0))
                            _skillSets.SkillSets[primary].ReferencingENTDs.Add(eventIndex);

                        byte secondary = eventUnit.SecondaryAction.Value;
                        if ((secondary != 0) && (secondary < 0xB0))
                            _skillSets.SkillSets[secondary].ReferencingENTDs.Add(eventIndex);

                        if ((eventUnit.Reaction.Offset != 0) && (eventUnit.Reaction.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Reaction.Offset].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.Support.Offset != 0) && (eventUnit.Support.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Support.Offset].ReferencingENTDs.Add(eventIndex);
                        if ((eventUnit.Movement.Offset != 0) && (eventUnit.Movement.Offset < 0x1FE))
                            _abilities.Abilities[eventUnit.Movement.Offset].ReferencingENTDs.Add(eventIndex);
                    }
                    else
                    {
                        jobIDsToCheck.Add(eventUnit.Job.Value);

                        if ((eventUnit.SkillSet.Value != 0) && (eventUnit.SkillSet.Value < 0xB0))
                            skillSetsToCheck.Add(eventUnit.SkillSet.Value);
                        if ((eventUnit.SecondaryAction.Value != 0) && (eventUnit.SecondaryAction.Value < 0xB0))
                            skillSetsToCheck.Add(eventUnit.SecondaryAction.Value);

                        if ((eventUnit.Reaction.Offset != 0) && (eventUnit.Reaction.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.Reaction.Offset);
                        if ((eventUnit.Support.Offset != 0)  && (eventUnit.Support.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.Support.Offset);
                        if ((eventUnit.Movement.Offset != 0)  && (eventUnit.Movement.Offset < 0x1FE))
                            abilityIDsToCheck.Add(eventUnit.Movement.Offset);
                    }
                }

                foreach (int jobID in jobIDsToCheck)
                {
                    bool foundReference = false;
                    foreach (EventUnit eventUnitInner in entdEvent.Units)
                    {
                        if ((eventUnitInner.Job.Value == jobID) && (eventUnitInner.SpriteSet.Value != 0))
                        {
                            foundReference = true;
                            break;
                        }
                    }

                    if (!foundReference)
                        _jobs.Jobs[jobID].ReferencingENTDs.Remove(eventIndex);
                }

                foreach (int skillSetID in skillSetsToCheck)
                {
                    bool foundReference = false;
                    foreach (EventUnit eventUnitInner in entdEvent.Units)
                    {
                        if (eventUnitInner.SpriteSet.Value != 0)
                        {
                            if ((eventUnitInner.SkillSet.Value == skillSetID) || (eventUnitInner.SecondaryAction.Value == skillSetID))
                            {
                                foundReference = true;
                                break;
                            }
                        }
                    }

                    if (!foundReference)
                        _skillSets.SkillSets[skillSetID].ReferencingENTDs.Remove(eventIndex);
                }

                foreach (int abilityID in abilityIDsToCheck)
                {
                    bool foundReference = false;
                    foreach (EventUnit eventUnitInner in entdEvent.Units)
                    {
                        if (eventUnitInner.SpriteSet.Value != 0)
                        {
                            if ((eventUnitInner.Reaction.Offset == abilityID) || (eventUnitInner.Support.Offset == abilityID) || (eventUnitInner.Movement.Offset == abilityID))
                            {
                                foundReference = true;
                                break;
                            }
                        }
                    }

                    if (!foundReference)
                        _abilities.Abilities[abilityID].ReferencingENTDs.Remove(eventIndex);
                }

                eventUnit.OldJob = eventUnit.Job;
                eventUnit.OldSkillSet = eventUnit.SkillSet;
                eventUnit.OldSecondaryAction = eventUnit.SecondaryAction;

                eventUnit.OldReaction = eventUnit.Reaction;
                eventUnit.OldSupport = eventUnit.Support;
                eventUnit.OldMovement = eventUnit.Movement;
            }
        }

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = eventListBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < eventListBox.Items.Count))
                eventListBox.SelectedIndex = newIndex;
        }
    }
}
