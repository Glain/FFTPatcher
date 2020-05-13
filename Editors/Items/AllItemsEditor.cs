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
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllItemsEditor : UserControl
    {
        #region Instance Variables (5)

        private Item copiedItem;

        const int cloneCommonIndex = 0;
        const int pasteCommonIndex = 1;
        const int pasteAllIndex = 2;
        private Context ourContext = Context.Default;

        #endregion

        #region Constructors (1)

        public AllItemsEditor()
        {
            InitializeComponent();
            itemEditor.InflictStatusClicked += itemEditor_InflictStatusClicked;
            itemEditor.ItemAttributesClicked += itemEditor_ItemAttributesClicked;
            itemEditor.SecondTableLinkClicked += itemEditor_SecondTableLinkClicked;
            itemEditor.DataChanged += new EventHandler( itemEditor_DataChanged );

            itemListBox.MouseDown += new MouseEventHandler(itemListBox_MouseDown);
            itemListBox.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste Common", pasteCommon),
                new MenuItem("Paste All", pasteAll) });
            itemListBox.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            itemListBox.KeyDown += new KeyEventHandler(itemListBox_KeyDown);
        }

		#endregion Constructors 

		#region Public Methods (2) 

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                itemListBox.SelectedIndex = itemListBox.IndexFromPoint(e.Location);
            }
        }

        public void UpdateView( AllItems items )
        {
            itemListBox.SelectedIndexChanged -= itemListBox_SelectedIndexChanged;
            itemListBox.DataSource = items.Items;
            itemListBox.SelectedIndexChanged += itemListBox_SelectedIndexChanged;
            itemListBox.SelectedIndex = 0;
            itemEditor.Item = itemListBox.SelectedItem as Item;
            itemListBox.SetChangedColors();
        }

		#endregion Public Methods 

		#region Private Methods (11) 

        private void itemEditor_DataChanged( object sender, EventArgs e )
        {
            itemListBox.BeginUpdate();
            int top = itemListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[itemListBox.DataSource];
            cm.Refresh();
            itemListBox.TopIndex = top;
            itemListBox.EndUpdate();
            itemListBox.SetChangedColor();
        }

        private void itemEditor_InflictStatusClicked( object sender, LabelClickedEventArgs e )
        {
            if( InflictStatusClicked != null )
            {
                InflictStatusClicked( this, e );
            }
        }

        private void itemEditor_ItemAttributesClicked( object sender, LabelClickedEventArgs e )
        {
            if( ItemAttributesClicked != null )
            {
                ItemAttributesClicked( this, e );
            }
        }

        private void itemEditor_SecondTableLinkClicked( object sender, LabelClickedEventArgs e )
        {
            int navigate = -1;
            switch( e.SecondTable )
            {
                case LabelClickedEventArgs.SecondTableType.Weapon:
                    if( e.Value <= 0x7F )
                        navigate = e.Value;
                    break;
                case LabelClickedEventArgs.SecondTableType.Shield:
                    if( e.Value <= 0x0F )
                        navigate = e.Value + 0x80;
                    break;
                case LabelClickedEventArgs.SecondTableType.HeadBody:
                    if( e.Value <= 0x3F )
                        navigate = e.Value + 0x90;
                    break;
                case LabelClickedEventArgs.SecondTableType.Accessory:
                    if( e.Value <= 0x1F )
                        navigate = e.Value + 0xD0;
                    break;
                case LabelClickedEventArgs.SecondTableType.ChemistItem:
                    if( e.Value <= 0x0D )
                    {
                        if( e.Value <= 0x04 )
                            navigate = e.Value + 0xF0;
                        else if( e.Value == 0x0D )
                            navigate = 0xF5;
                        else if( e.Value <= 0x0C )
                            navigate = e.Value - 0x05 + 0xF6;
                    }
                    break;

            }
            if( navigate != -1 )
            {
                itemListBox.SelectedIndex = navigate;
            }
        }

        private void itemListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            itemEditor.Item = itemListBox.SelectedItem as Item;
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            bool typesMatch = TypesMatch();
            itemListBox.ContextMenu.MenuItems[pasteCommonIndex].Enabled = (copiedItem != null);
            itemListBox.ContextMenu.MenuItems[pasteAllIndex].Enabled = typesMatch;
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
            copiedItem = itemListBox.SelectedItem as Item;
        }

        private void pasteCommon(object sender, EventArgs args)
        {
            if (copiedItem != null)
            {
                Item destItem = itemListBox.SelectedItem as Item;
                copiedItem.CopyCommonTo(destItem);
                itemEditor.UpdateView();
                itemEditor_DataChanged(itemEditor, EventArgs.Empty);
            }
        }

        private void pasteAll(object sender, EventArgs args)
        {
            if ( TypesMatch() )
            {
                Item destItem = itemListBox.SelectedItem as Item;
                copiedItem.CopyAllTo(destItem);
                itemEditor.Item = destItem;
                itemEditor.UpdateView();
                itemEditor.Invalidate(true);
                itemEditor_DataChanged(itemEditor, EventArgs.Empty);
            }
        }

        private bool TypesMatch()
        {
            Item destItem = itemListBox.SelectedItem as Item;
            return (copiedItem != null) ? (copiedItem.GetType() == destItem.GetType()) : false;
        }

		#endregion Private Methods 

        public event EventHandler<LabelClickedEventArgs> InflictStatusClicked;
        public event EventHandler<LabelClickedEventArgs> ItemAttributesClicked;
    }
}
