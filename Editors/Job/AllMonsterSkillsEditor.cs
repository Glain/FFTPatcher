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

namespace FFTPatcher.Editors
{
    public partial class AllMonsterSkillsEditor : UserControl
    {
        #region Instance Variables (3)

        private MonsterSkill copiedMonsterSkill;
        const int cloneIndex = 0;
        const int pasteIndex = 1;
        
        #endregion

        #region Public Properties (1)

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
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView( AllMonsterSkills skills )
        {
            dataGridView.DataSource = null;
            foreach( DataGridViewComboBoxColumn col in new DataGridViewComboBoxColumn[] { Ability1, Ability2, Ability3, Beastmaster } )
            {
                col.Items.Clear();
                col.Items.AddRange( AllAbilities.DummyAbilities );
                col.ValueType = typeof( Ability );
            }
            dataGridView.DataSource = skills.MonsterSkills;
        }

		#endregion Public Methods 

		#region Private Methods (10) 

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
                            e.CellStyle.BackColor = Color.Blue;
                            e.CellStyle.ForeColor = Color.White;
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
                dataGridView.CurrentCell = dataGridView[hitTest.ColumnIndex, hitTest.RowIndex];
                dataGridView.ContextMenu.Show(dataGridView, new Point(e.X, e.Y));
            }
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            dataGridView.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedMonsterSkill != null);
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.C && args.Control)
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
            }
        }

		#endregion Private Methods 
    }
}
