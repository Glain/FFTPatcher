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
using System.Collections.Generic;

namespace FFTPatcher.Editors
{
    public partial class AllItemsEditor : UserControl
    {
        #region Instance Variables

        private Item copiedItem;
        private AllInflictStatuses inflictStatuses;
        private AllItemAttributes itemAttributes;

        const int cloneCommonIndex = 0;
        const int pasteCommonIndex = 1;
        const int pasteAllIndex = 2;
        private Context ourContext = Context.Default;

        #endregion

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                itemEditor.ToolTip = value;
            }
        }

        #region Constructors

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

        public int SelectedIndex { get { return itemListBox.SelectedIndex; } set { itemListBox.SelectedIndex = value; } }

		#region Public Methods (2) 

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                itemListBox.SelectedIndex = itemListBox.IndexFromPoint(e.Location);
            }
        }

        public void UpdateView( AllItems items, AllStoreInventories storeInventories, AllInflictStatuses inflictStatuses, AllItemAttributes itemAttributes, Context context )
        {
            ourContext = context;
            this.inflictStatuses = inflictStatuses;
            this.itemAttributes = itemAttributes;

            itemListBox.SelectedIndexChanged -= itemListBox_SelectedIndexChanged;
            itemListBox.DataSource = items.Items;
            itemListBox.SelectedIndexChanged += itemListBox_SelectedIndexChanged;
            itemListBox.SelectedIndex = 0;
            itemEditor.BuildItemNameLists(context);
            itemEditor.StoreInventories = storeInventories;
            //itemEditor.Item = itemListBox.SelectedItem as Item;
            itemEditor.SetItem(itemListBox.SelectedItem as Item, context);
            itemListBox.SetChangedColors();
        }

        public void SetListBoxHighlightedIndexes(IEnumerable<int> highlightedIndexes)
        {
            itemListBox.SetHighlightedIndexes(highlightedIndexes);
        }

        public void UpdateSelectedEntry()
        {
            itemEditor.UpdateView(ourContext);
        }

        public void UpdateListBox()
        {
            itemListBox.SetChangedColors();
        }

        #endregion Public Methods

        #region Private Methods

        private void ClearListBoxHighlightedIndexes()
        {
            itemListBox.ClearHighlightedIndexes();
        }

        private void itemEditor_DataChanged( object sender, EventArgs e )
        {
            itemListBox.BeginUpdate();
            int top = itemListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[itemListBox.DataSource];
            cm.Refresh();
            itemListBox.TopIndex = top;
            itemListBox.EndUpdate();
            itemListBox.SetChangedColor();

            UpdateInflictStatus(itemListBox.SelectedIndex);
            UpdateItemAttributes(itemListBox.SelectedIndex);
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
            if (args.KeyCode == Keys.Escape)
            {
                ClearListBoxHighlightedIndexes();
                itemListBox.SetChangedColors();
                itemListBox.Invalidate();
            }
            else if (args.KeyCode == Keys.C && args.Control)
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
                itemEditor.UpdateView(ourContext);
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
                itemEditor.UpdateView(ourContext);
                itemEditor.Invalidate(true);
                itemEditor_DataChanged(itemEditor, EventArgs.Empty);
            }
        }

        private bool TypesMatch()
        {
            Item destItem = itemListBox.SelectedItem as Item;
            return (copiedItem != null) ? (copiedItem.GetType() == destItem.GetType()) : false;
        }

        private void UpdateInflictStatus(int itemIndex)
        {
            if (itemIndex >= 0)
            {
                Item item = ((List<Item>)itemListBox.DataSource)[itemIndex];
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    if (weapon.OldInflictStatus != weapon.InflictStatus)
                    {
                        if (weapon.OldInflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[weapon.OldInflictStatus].ReferencingItemIndexes.Remove(itemIndex);
                    }

                    if (weapon.Formula.Value == 2)
                    {
                        if (weapon.InflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[weapon.InflictStatus].ReferencingItemIndexes.Remove(itemIndex);
                    }
                    else
                    {
                        if (weapon.InflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[weapon.InflictStatus].ReferencingItemIndexes.Add(itemIndex);
                    }

                    weapon.OldInflictStatus = weapon.InflictStatus;
                }
                else if (item is ChemistItem)
                {
                    ChemistItem chemistItem = (ChemistItem)item;
                    if (chemistItem.OldInflictStatus != chemistItem.InflictStatus)
                    {
                        if (chemistItem.OldInflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[chemistItem.OldInflictStatus].ReferencingItemIndexes.Remove(itemIndex);
                    }

                    if (chemistItem.Formula == 2)
                    {
                        if (chemistItem.InflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[chemistItem.InflictStatus].ReferencingItemIndexes.Remove(itemIndex);
                    }
                    else
                    {
                        if (chemistItem.InflictStatus <= 0x7F)
                            inflictStatuses.InflictStatuses[chemistItem.InflictStatus].ReferencingItemIndexes.Add(itemIndex);
                    }

                    chemistItem.OldInflictStatus = chemistItem.InflictStatus;
                }
            }
        }

        private void UpdateItemAttributes(int itemIndex)
        {
            if (itemIndex >= 0)
            {
                Item item = ((List<Item>)itemListBox.DataSource)[itemIndex];

                if (item.OldSIA != item.SIA)
                {
                    itemAttributes.ItemAttributes[item.OldSIA].ReferencingItemIndexes.Remove(itemIndex);
                    itemAttributes.ItemAttributes[item.SIA].ReferencingItemIndexes.Add(itemIndex);
                }

                item.OldSIA = item.SIA;
            }
        }

		#endregion Private Methods 

        public event EventHandler<LabelClickedEventArgs> InflictStatusClicked;
        public event EventHandler<LabelClickedEventArgs> ItemAttributesClicked;
    }
}
