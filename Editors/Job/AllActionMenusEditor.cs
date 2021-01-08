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
    public partial class AllActionMenusEditor : UserControl
    {
        #region Instance Variables (3)

        private ActionMenu copiedEntry;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        #endregion

		#region Constructors (1) 

        public AllActionMenusEditor()
        {
            InitializeComponent();

            ActionColumn.DataSource = ActionMenuEntry.AllActionMenuEntries;
            ActionColumn.ValueType = typeof( ActionMenuEntry );

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

        public void UpdateView( AllActionMenus actionMenus )
        {
            dataGridView.DataSource = actionMenus.ActionMenus;
        }

		#endregion Public Methods 

		#region Private Methods (10) 

        private void Control_KeyDown( object sender, KeyEventArgs e )
        {
            if( (e.KeyData == Keys.F12) &&
                (dataGridView.CurrentCell is DataGridViewComboBoxCell) &&
                (dataGridView.CurrentRow.DataBoundItem is ActionMenu) )
            {
                ActionMenu action = dataGridView.CurrentRow.DataBoundItem as ActionMenu;
                DataGridViewComboBoxEditingControl c = dataGridView.EditingControl as DataGridViewComboBoxEditingControl;
                c.SelectedItem = ReflectionHelpers.GetFieldOrProperty<ActionMenuEntry>( action.Default, dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].DataPropertyName );
                dataGridView.EndEdit();
            }
        }

        private void dataGridView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
        {
            if( (e.ColumnIndex == Offset.Index) && (e.Value != null) )
            {
                byte b = (byte)e.Value;
                e.Value = b.ToString( "X2" );
                e.FormattingApplied = true;
            }
            else if( e.ColumnIndex == ActionColumn.Index )
            {
                if( (e.RowIndex >= 0) && (e.ColumnIndex >= 0) &&
                    (dataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell) &&
                    (dataGridView.Rows[e.RowIndex].DataBoundItem is ActionMenu) )
                {
                    ActionMenu menu = dataGridView.Rows[e.RowIndex].DataBoundItem as ActionMenu;
                    if( menu.Default != null )
                    {
                        ActionMenuEntry a = menu.Default.MenuAction;
                        if( a != (e.Value as ActionMenuEntry) )
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
                (dataGridView.Rows[e.RowIndex].DataBoundItem is ActionMenu) )
            {
                ActionMenu menu = dataGridView.Rows[e.RowIndex].DataBoundItem as ActionMenu;
                if( menu.Default != null )
                {
                    e.ToolTipText = "Default: " + menu.Default.MenuAction.ToString();
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
            dataGridView.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
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
            copiedEntry = dataGridView.CurrentRow.DataBoundItem as ActionMenu;
        }

        private void pasteAll(object sender, EventArgs e)
        {
            if (copiedEntry != null)
            {
                ActionMenu destEntry = dataGridView.CurrentRow.DataBoundItem as ActionMenu;
                copiedEntry.CopyAllTo(destEntry);
                dataGridView.Invalidate();
            }
        }

		#endregion Private Methods 
    }
}
