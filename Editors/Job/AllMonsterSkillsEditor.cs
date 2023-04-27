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

using System.Drawing;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib;
using System;
using System.Collections.Generic;

namespace FFTPatcher.Editors
{
    public partial class AllMonsterSkillsEditor : UserControl
    {
        #region Instance Variables

        private MonsterSkill copiedMonsterSkill;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        private AllAbilities _abilities;

        private HashSet<int> highlightedIndexes = new HashSet<int>();
        #endregion

        #region Public Properties

        public int SelectedIndex
        {
            get { return dataGridView.CurrentRow.Index; }
            set
            {
                dataGridView[0, value].Selected = true;
                dataGridView.CurrentCell = dataGridView[0, value];
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllMonsterSkillsEditor()
        {
            InitializeComponent();

            dataGridView.AutoSize = true;
            dataGridView.CellParsing += dataGridView_CellParsing;

            dataGridView.AutoGenerateColumns = false;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.CellToolTipTextNeeded += dataGridView_CellToolTipTextNeeded;

            dataGridView.MouseUp += new MouseEventHandler(dataGridView_MouseUp);
            dataGridView.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll)
            });
            dataGridView.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            dataGridView.KeyDown += new KeyEventHandler(dataGridView_KeyDown);

            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
        }

		#endregion Constructors 

		#region Public Methods 

        public void UpdateView( AllMonsterSkills skills, AllAbilities abilities, PatcherLib.Datatypes.Context context )
        {
            _abilities = abilities;

            dataGridView.DataSource = null;
            foreach( DataGridViewComboBoxColumn col in new DataGridViewComboBoxColumn[] { Ability1, Ability2, Ability3, Beastmaster } )
            {
                col.Items.Clear();
                col.Items.AddRange( AllAbilities.GetDummyAbilities(context) );
                col.ValueType = typeof( Ability );
            }
            dataGridView.DataSource = skills.MonsterSkills;
        }

        public void SetHighlightedIndexes(IEnumerable<int> highlightedIndexes)
        {
            dataGridView.SetHighlightedIndexes(highlightedIndexes);
        }

        #endregion Public Methods 

        #region Private Methods () 

        public void ClearHighlightedIndexes()
        {
            dataGridView.ClearHighlightedIndexes();
        }

        public void RefreshDataGridView()
        {
            dataGridView.Invalidate();
        }

        private void UpdateAbilities(int monsterSkillIndex)
        {
            if (monsterSkillIndex < 0)
                return;

            MonsterSkill monsterSkill = ((MonsterSkill[])dataGridView.DataSource)[monsterSkillIndex];

            Ability[] abilities = new Ability[4] { monsterSkill.Ability1, monsterSkill.Ability2, monsterSkill.Ability3, monsterSkill.Beastmaster };
            Ability[] oldAbilities = new Ability[4] { monsterSkill.OldAbility1, monsterSkill.OldAbility2, monsterSkill.OldAbility3, monsterSkill.OldBeastmaster };
            HashSet<Ability> newAbilitySet = new HashSet<Ability>(abilities);
            HashSet<Ability> oldAbilitySet = new HashSet<Ability>(oldAbilities);
            
            for (int index = 0; index < oldAbilities.Length; index++)
            {
                Ability ability = oldAbilities[index];
                if (!newAbilitySet.Contains(ability))
                {
                    if (_abilities.Abilities[ability.Offset].ReferencingMonsterSkillIDs.Contains(monsterSkillIndex))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingMonsterSkillIDs.Remove(monsterSkillIndex);
                    }
                }
            }

            for (int index = 0; index < abilities.Length; index++)
            {
                Ability ability = abilities[index];
                if (!oldAbilitySet.Contains(ability))
                {
                    if (!_abilities.Abilities[ability.Offset].ReferencingMonsterSkillIDs.Contains(monsterSkillIndex))
                    {
                        _abilities.Abilities[ability.Offset].ReferencingMonsterSkillIDs.Add(monsterSkillIndex);
                    }
                }
            }

            monsterSkill.OldAbility1 = monsterSkill.Ability1;
            monsterSkill.OldAbility2 = monsterSkill.Ability2;
            monsterSkill.OldAbility3 = monsterSkill.Ability3;
            monsterSkill.OldBeastmaster = monsterSkill.Beastmaster;
        }

        private void Control_KeyDown( object sender, KeyEventArgs e )
        {
            if( (e.KeyData == Keys.F12) &&
                (dataGridView.CurrentCell is DataGridViewComboBoxCell) &&
                (dataGridView.CurrentRow.DataBoundItem is MonsterSkill) )
            {
                MonsterSkill skill = dataGridView.CurrentRow.DataBoundItem as MonsterSkill;
                DataGridViewComboBoxEditingControl c = dataGridView.EditingControl as DataGridViewComboBoxEditingControl;
                c.SelectedItem = ReflectionHelpers.GetFieldOrProperty<Ability>( skill.Default, dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].DataPropertyName );
                dataGridView.EndEdit();
            }
        }

        private void dataGridView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
        {
            if( e.ColumnIndex == Offset.Index )
            {
                byte b = (byte)e.Value;
                e.Value = b.ToString( "X2" );
                e.FormattingApplied = true;
            }
            else if( (e.ColumnIndex == Ability1.Index) || 
                     (e.ColumnIndex == Ability2.Index) || 
                     (e.ColumnIndex == Ability3.Index) || 
                     (e.ColumnIndex == Beastmaster.Index) )
            {
                if( (e.RowIndex >= 0) && (e.ColumnIndex >= 0) &&
                    (dataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell) &&
                    (dataGridView.Rows[e.RowIndex].DataBoundItem is MonsterSkill) )
                {
                    MonsterSkill skill = dataGridView.Rows[e.RowIndex].DataBoundItem as MonsterSkill;
                    if( skill.Default != null )
                    {
                        Ability a = ReflectionHelpers.GetFieldOrProperty<Ability>( skill.Default, dataGridView.Columns[e.ColumnIndex].DataPropertyName );
                        if( a != (e.Value as Ability) )
                        {
                            e.CellStyle.BackColor = Settings.ModifiedColor.BackgroundColor;
                            e.CellStyle.ForeColor = Settings.ModifiedColor.ForegroundColor;
                        }
                    }
                }
            }
        }

        private void dataGridView_CellParsing( object sender, DataGridViewCellParsingEventArgs e )
        {
            DataGridViewComboBoxEditingControl c = dataGridView.EditingControl as DataGridViewComboBoxEditingControl;
            if( c != null )
            {
                e.Value = c.SelectedItem;
                e.ParsingApplied = true;
            }
        }

        private void dataGridView_CellToolTipTextNeeded( object sender, DataGridViewCellToolTipTextNeededEventArgs e )
        {
            if( (e.RowIndex >= 0) && (e.ColumnIndex >= 0) &&
                (dataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell) &&
                (dataGridView.Rows[e.RowIndex].DataBoundItem is MonsterSkill) )
            {
                MonsterSkill skill = dataGridView.Rows[e.RowIndex].DataBoundItem as MonsterSkill;
                if( skill.Default != null )
                {
                    Ability a = ReflectionHelpers.GetFieldOrProperty<Ability>( skill.Default, dataGridView.Columns[e.ColumnIndex].DataPropertyName );
                    e.ToolTipText = "Default: " + a.ToString();
                }
            }
        }

        private void dataGridView_EditingControlShowing( object sender, DataGridViewEditingControlShowingEventArgs e )
        {
            DataGridViewComboBoxEditingControl c = e.Control as DataGridViewComboBoxEditingControl;
            if( c != null )
            {
                c.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            e.Control.KeyDown += Control_KeyDown;
        }

        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTest = dataGridView.HitTest(e.Location.X, e.Location.Y);
                if ((hitTest.ColumnIndex >= 0) && (hitTest.RowIndex >= 0))
                {
                    dataGridView.CurrentCell = dataGridView[hitTest.ColumnIndex, hitTest.RowIndex];
                    dataGridView.ContextMenu.Show(dataGridView, new Point(e.X, e.Y));
                }
            }
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            dataGridView.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedMonsterSkill != null);
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Escape)
            {
                ClearHighlightedIndexes();
                dataGridView.Invalidate();
            }
            else if (args.KeyCode == Keys.C && args.Control)
                copyAll(sender, args);
            else if (args.KeyCode == Keys.V && args.Control)
                pasteAll(sender, args);
        }

        private void copyAll(object sender, EventArgs e)
        {
            copiedMonsterSkill = dataGridView.CurrentRow.DataBoundItem as MonsterSkill;
        }

        private void pasteAll(object sender, EventArgs e)
        {
            if (copiedMonsterSkill != null)
            {
                MonsterSkill destMonsterSkill = dataGridView.CurrentRow.DataBoundItem as MonsterSkill;
                copiedMonsterSkill.CopyAllTo(destMonsterSkill);
                dataGridView.Invalidate();
                UpdateAbilities(dataGridView.CurrentRow.Index);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateAbilities(e.RowIndex);
        }

        #endregion Private Methods 
    }
}
