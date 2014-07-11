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
using System.Windows.Forms;
using FFTPatcher.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllMoveFindItemsEditor : UserControl
    {
        #region Instance Variables (3)

        private MapMoveFindItems copiedEntry;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        #endregion

		#region Constructors (1) 

        public AllMoveFindItemsEditor()
        {
            InitializeComponent();
            mapMoveFindItemEditor1.DataChanged += new EventHandler( mapMoveFindItemEditor1_DataChanged );
            mapListBox.IncludePrefix = true;

            mapListBox.MouseDown += new MouseEventHandler(itemListBox_MouseDown);
            mapListBox.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll),
            });
            mapListBox.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            mapListBox.KeyDown += new KeyEventHandler(itemListBox_KeyDown);
        }

		#endregion Constructors 

		#region Public Methods (2) 

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mapListBox.SelectedIndex = mapListBox.IndexFromPoint(e.Location);
            }
        }

        public void UpdateView( AllMoveFindItems items )
        {
            mapListBox.SelectedIndexChanged -= mapListBox_SelectedIndexChanged;
            mapListBox.DataSource = items.MoveFindItems;
            mapListBox.SelectedIndexChanged += mapListBox_SelectedIndexChanged;
            mapListBox.SelectedIndex = 0;
            mapMoveFindItemEditor1.MapMoveFindItems = mapListBox.SelectedItem as MapMoveFindItems;
        }

		#endregion Public Methods 

		#region Private Methods (6) 

        void mapListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            MapMoveFindItems map = mapListBox.SelectedItem as MapMoveFindItems;
            mapMoveFindItemEditor1.MapMoveFindItems = map;
        }

        void mapMoveFindItemEditor1_DataChanged( object sender, EventArgs e )
        {
            mapListBox.BeginUpdate();
            var top = mapListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[mapListBox.DataSource];
            cm.Refresh();
            mapListBox.TopIndex = top;
            mapListBox.EndUpdate();
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            mapListBox.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
        }

        private void itemListBox_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.C && args.Control)
                copyAll(sender, args);
            else if (args.KeyCode == Keys.V && args.Control)
                pasteAll(sender, args);
        }

        private void copyAll(object sender, EventArgs args)
        {
            copiedEntry = mapListBox.SelectedItem as MapMoveFindItems;
        }

        private void pasteAll(object sender, EventArgs args)
        {
            if (copiedEntry != null)
            {
                MapMoveFindItems destEntry = mapListBox.SelectedItem as MapMoveFindItems;
                copiedEntry.CopyTo(destEntry);
                mapMoveFindItemEditor1.MapMoveFindItems = destEntry;
                mapMoveFindItemEditor1.UpdateView();
                mapMoveFindItemEditor1.Invalidate(true);
                mapMoveFindItemEditor1_DataChanged(mapMoveFindItemEditor1, EventArgs.Empty);
            }
        }

		#endregion Private Methods 
    }
}
