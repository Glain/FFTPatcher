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
    public partial class AllInflictStatusesEditor : UserControl
    {
        #region Instance Variables (3)

        private InflictStatus copiedEntry;
        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        #endregion

		#region Public Properties (1) 

        public int SelectedIndex { get { return offsetListBox.SelectedIndex; } set { offsetListBox.SelectedIndex = value; } }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllInflictStatusesEditor()
        {
            InitializeComponent();
            inflictStatusEditor.DataChanged += new EventHandler( inflictStatusEditor_DataChanged );
            inflictStatusEditor.RepointHandler += OnRepoint;

            offsetListBox.MouseDown += new MouseEventHandler(itemListBox_MouseDown);
            offsetListBox.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll),
            });
            offsetListBox.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            offsetListBox.KeyDown += new KeyEventHandler(itemListBox_KeyDown);

        }

		#endregion Constructors 

		#region Public Methods (6) 

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                offsetListBox.SelectedIndex = offsetListBox.IndexFromPoint(e.Location);
            }
        }

        public void UpdateView( AllInflictStatuses statuses, PatcherLib.Datatypes.Context context )
        {
            ourContext = context;
            offsetListBox.SelectedIndexChanged -= offsetListBox_SelectedIndexChanged;
            offsetListBox.DataSource = statuses.InflictStatuses;
            offsetListBox.SelectedIndexChanged += offsetListBox_SelectedIndexChanged;
            offsetListBox.SelectedIndex = 0;
            PatchUtility.CheckDuplicates<InflictStatus>(statuses.InflictStatuses);
            offsetListBox.SetChangedColors<InflictStatus>();
            //inflictStatusEditor.InflictStatus = offsetListBox.SelectedItem as InflictStatus;
            inflictStatusEditor.SetInflictStatus(offsetListBox.SelectedItem as InflictStatus, context);
        }

        public void UpdateSelectedEntry()
        {
            inflictStatusEditor.UpdateView(ourContext);
        }

        public void UpdateListBox()
        {
            PatchUtility.CheckDuplicates<InflictStatus>((InflictStatus[])offsetListBox.DataSource);
            offsetListBox.SetChangedColors<InflictStatus>();
            offsetListBox.Invalidate();
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            offsetListBox.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
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
            copiedEntry = offsetListBox.SelectedItem as InflictStatus;
        }

        private void pasteAll(object sender, EventArgs args)
        {
            if (copiedEntry != null)
            {
                InflictStatus destEntry = offsetListBox.SelectedItem as InflictStatus;
                copiedEntry.CopyTo(destEntry);

                inflictStatusEditor.InflictStatus = destEntry;
                inflictStatusEditor.UpdateView(ourContext);
                inflictStatusEditor.Invalidate(true);
                inflictStatusEditor_DataChanged(inflictStatusEditor, EventArgs.Empty);
            }
        }

		#endregion Public Methods 

		#region Private Methods (2) 

        private void inflictStatusEditor_DataChanged( object sender, EventArgs e )
        {
            offsetListBox.BeginUpdate();
            var top = offsetListBox.TopIndex;
            PatchUtility.CheckDuplicates<InflictStatus>((InflictStatus[])offsetListBox.DataSource);
            CurrencyManager cm = (CurrencyManager)BindingContext[offsetListBox.DataSource];
            cm.Refresh();
            offsetListBox.TopIndex = top;
            offsetListBox.EndUpdate();
            //offsetListBox.SetChangedColor<InflictStatus>();
            offsetListBox.SetChangedColors<InflictStatus>();
        }

        private void offsetListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            inflictStatusEditor.InflictStatus = offsetListBox.SelectedItem as InflictStatus;
        }

		#endregion Private Methods 

        public event System.EventHandler<RepointEventArgs> RepointHandler;
        protected void OnRepoint(object sender, RepointEventArgs e)
        {
            if (RepointHandler != null)
            {
                e.OldID = offsetListBox.SelectedIndex;
                RepointHandler(this, e);
            }
        }
    }
}
