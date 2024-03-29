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
    public partial class AllStatusAttributesEditor : UserControl, IHandleSelectedIndex
    {
        #region Instance Variables

        private StatusAttribute copiedEntry;
        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        #endregion

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                statusAttributeEditor.ToolTip = value;
            }
        }

        #region Constructors (1)

        public AllStatusAttributesEditor()
        {
            InitializeComponent();
            statusAttributeEditor.DataChanged += new EventHandler( statusAttributeEditor_DataChanged );
            listBox.IncludePrefix = true;

            listBox.MouseDown += new MouseEventHandler(itemListBox_MouseDown);
            listBox.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll),
            });
            listBox.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            listBox.KeyDown += new KeyEventHandler(itemListBox_KeyDown);
        }

		#endregion Constructors 

		#region Public Methods (2) 

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listBox.SelectedIndex = listBox.IndexFromPoint(e.Location);
            }
        }

        public void UpdateView( AllStatusAttributes attributes, PatcherLib.Datatypes.Context context )
        {
            ourContext = context;
            listBox.SelectedIndexChanged -= listBox_SelectedIndexChanged;
            listBox.DataSource = attributes.StatusAttributes;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            listBox.SelectedIndex = 0;
            //statusAttributeEditor.StatusAttribute = listBox.SelectedItem as StatusAttribute;
            statusAttributeEditor.SetStatusAttribute(listBox.SelectedItem as StatusAttribute, context);
            listBox.SetChangedColors();
        }

		#endregion Public Methods 

		#region Private Methods (6) 

        private void listBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            StatusAttribute a = listBox.SelectedItem as StatusAttribute;
            statusAttributeEditor.StatusAttribute = a;
        }

        private void statusAttributeEditor_DataChanged( object sender, EventArgs e )
        {
            listBox.BeginUpdate();
            var top = listBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[listBox.DataSource];
            cm.Refresh();
            listBox.TopIndex = top;
            listBox.EndUpdate();
            listBox.SetChangedColor();
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            listBox.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
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
            copiedEntry = listBox.SelectedItem as StatusAttribute;
        }

        private void pasteAll(object sender, EventArgs args)
        {
            if (copiedEntry != null)
            {
                StatusAttribute destEntry = listBox.SelectedItem as StatusAttribute;
                copiedEntry.CopyTo(destEntry);
                statusAttributeEditor.StatusAttribute = destEntry;
                statusAttributeEditor.UpdateView(ourContext);
                statusAttributeEditor.Invalidate(true);
                statusAttributeEditor_DataChanged(statusAttributeEditor, EventArgs.Empty);
            }
        }

		#endregion Private Methods 

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = listBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < listBox.Items.Count))
                listBox.SelectedIndex = newIndex;
        }
    }
}
