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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllSkillSetsEditor : UserControl, IHandleSelectedIndex
    {
		#region Instance Variables 

        private SkillSet cbSkillSet = null;
        private Context ourContext = Context.Default;

        private AllAbilities _abilities;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                skillSetEditor.ToolTip = value;
            }
        }

		#region Public Properties (1) 

        public int SelectedIndex { get { return skillSetListBox.SelectedIndex; } set { skillSetListBox.SelectedIndex = value; } }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllSkillSetsEditor()
        {
            InitializeComponent();
            skillSetEditor.DataChanged += new EventHandler( skillSetEditor_DataChanged );
            skillSetListBox.ContextMenu = new ContextMenu( new MenuItem[] {
                new MenuItem("Clone", CloneClick),
                new MenuItem("Paste", PasteClick) } );
            skillSetListBox.ContextMenu.Popup += new EventHandler( ContextMenu_Popup );
            skillSetListBox.MouseDown += new MouseEventHandler( skillSetListBox_MouseDown );

            skillSetEditor.JobClicked += OnJobClicked;
            skillSetEditor.ENTDClicked += OnENTDClicked;
        }

		#endregion Constructors 

		#region Public Methods 

        public void UpdateView( AllSkillSets skills, AllAbilities abilities, Context context )
        {
            if( ourContext != context )
            {
                ourContext = context;
                cbSkillSet = null;
            }

            _abilities = abilities;

            skillSetListBox.SelectedIndexChanged -= skillSetListBox_SelectedIndexChanged;
            skillSetListBox.DataSource = skills.SkillSets;
            skillSetListBox.SelectedIndexChanged += skillSetListBox_SelectedIndexChanged;
            skillSetListBox.SelectedIndex = 0;
            //skillSetEditor.SkillSet = skillSetListBox.SelectedItem as SkillSet;
            skillSetEditor.SetSkillSet(skillSetListBox.SelectedItem as SkillSet, context);
            skillSetListBox.SetChangedColors();
        }

        public void SetListBoxHighlightedIndexes(IEnumerable<int> highlightedIndexes)
        {
            skillSetListBox.SetHighlightedIndexes(highlightedIndexes);
        }

        #endregion Public Methods 

        #region Private Methods

        private void ClearListBoxHighlightedIndexes()
        {
            skillSetListBox.ClearHighlightedIndexes();
        }

        private void CloneClick( object sender, EventArgs args )
        {
            cbSkillSet = skillSetListBox.SelectedItem as SkillSet;
        }

        void ContextMenu_Popup( object sender, EventArgs e )
        {
            skillSetListBox.ContextMenu.MenuItems[1].Enabled = cbSkillSet != null;
        }

        private void PasteClick( object sender, EventArgs args )
        {
            if( cbSkillSet != null )
            {
                cbSkillSet.CopyTo( skillSetListBox.SelectedItem as SkillSet );
                skillSetEditor.UpdateView(ourContext);
                skillSetEditor_DataChanged( skillSetEditor, EventArgs.Empty );
            }
        }

        private void skillSetEditor_DataChanged( object sender, EventArgs e )
        {
            skillSetListBox.BeginUpdate();
            var top = skillSetListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[skillSetListBox.DataSource];
            cm.Refresh();
            skillSetListBox.TopIndex = top;
            skillSetListBox.EndUpdate();
            skillSetListBox.SetChangedColor();

            UpdateAbilities(skillSetListBox.SelectedIndex);
        }

        void skillSetListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                skillSetListBox.SelectedIndex = skillSetListBox.IndexFromPoint( e.Location );
            }
        }

        private void skillSetListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            SkillSet s = skillSetListBox.SelectedItem as SkillSet;
            skillSetEditor.SkillSet = s;
        }

       	private void skillSetListBox_KeyDown( object sender, KeyEventArgs args )
		{
            if (args.KeyCode == Keys.Escape)
            {
                ClearListBoxHighlightedIndexes();
                skillSetListBox.SetChangedColors();
                skillSetListBox.Invalidate();
            }
            else if (args.KeyCode == Keys.C && args.Control)
				CloneClick( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClick( sender, args );
		}
        
		#endregion Private Methods 

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = skillSetListBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < skillSetListBox.Items.Count))
                skillSetListBox.SelectedIndex = newIndex;
        }

        public void UpdateSelectedEntry()
        {
            skillSetEditor.UpdateView(ourContext);
        }

        public void UpdateListBox()
        {
            skillSetListBox.SetChangedColors();
        }

        private void UpdateAbilities(int skillSetIndex)
        {
            if (skillSetIndex < 0)
                return;

            SkillSet skillSet = ((SkillSet[])skillSetListBox.DataSource)[skillSetIndex];

            HashSet<Ability> newActionSet = new HashSet<Ability>(skillSet.Actions);
            HashSet<Ability> oldActionSet = new HashSet<Ability>(skillSet.OldActions);

            for (int index = 0; index < skillSet.OldActions.Length; index++)
            {
                Ability ability = skillSet.OldActions[index];
                if (!newActionSet.Contains(ability))
                {
                    if (_abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Contains(skillSetIndex))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Remove(skillSetIndex);
                    }
                }
            }

            for (int index = 0; index < skillSet.Actions.Length; index++)
            {
                Ability ability = skillSet.Actions[index];
                if (!oldActionSet.Contains(ability))
                {
                    if (!_abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Contains(skillSetIndex))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Add(skillSetIndex);
                    }
                }
            }

            HashSet<Ability> newRSMSet = new HashSet<Ability>(skillSet.TheRest);
            HashSet<Ability> oldRSMSet = new HashSet<Ability>(skillSet.OldTheRest);

            for (int index = 0; index < skillSet.OldTheRest.Length; index++)
            {
                Ability ability = skillSet.OldTheRest[index];
                if (!newRSMSet.Contains(ability))
                {
                    if (_abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Contains(index))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Remove(index);
                    }
                }
            }

            for (int index = 0; index < skillSet.TheRest.Length; index++)
            {
                Ability ability = skillSet.TheRest[index];
                if (!oldRSMSet.Contains(ability))
                {
                    if (!_abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Contains(index))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Add(index);
                    }
                }
            }

            skillSet.OldActions = (Ability[])skillSet.Actions.Clone();
            skillSet.OldTheRest = (Ability[])skillSet.TheRest.Clone();
        }

        public event EventHandler<ReferenceEventArgs> JobClicked;
        private void OnJobClicked(object sender, ReferenceEventArgs e)
        {
            if (JobClicked != null)
            {
                JobClicked(this, e);
            }
        }

        public event EventHandler<ReferenceEventArgs> ENTDClicked;
        private void OnENTDClicked(object sender, ReferenceEventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, e);
            }
        }
    }
}
