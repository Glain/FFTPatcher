using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FFTPatcher.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class StoreInventoryEditor : BaseEditor
    {

        public class CustomSortedItemListBox : ListBox
        {
            public void SortPub()
            {
                Sort();
            }

            protected override void Sort()
            {
                BeginUpdate();
                object selectedItem = SelectedItem;
                PatcherLib.Utilities.Utilities.SortList(Items, (a, b) => a.ToString().CompareTo(b.ToString()));
                SelectedItems.Clear();
                SelectedItem = selectedItem;
                EndUpdate();
            }
        }

        private StoreInventory storeInventory;
        private List<CustomSortedItemListBox> listBoxes;
        private Dictionary<Type, CustomSortedItemListBox> fromListBoxes;
        private Dictionary<Type, CustomSortedItemListBox> toListBoxes;

        public StoreInventoryEditor()
        {
            InitializeComponent();
            listBoxes = new List<CustomSortedItemListBox>();
            listBoxes.AddRange(new CustomSortedItemListBox[] {
                weaponsToListBox, weaponsFromListBox, itemsToListBox, itemsFromListBox,
                accessoriesToListBox, accessoriesFromListBox, armorFromListBox, armorToListBox,
                shieldsFromListBox, shieldsToListBox } );
            fromListBoxes = new Dictionary<Type, CustomSortedItemListBox> { 
                { typeof(Weapon), weaponsFromListBox },
                { typeof(ChemistItem), itemsFromListBox },
                { typeof(Accessory), accessoriesFromListBox },
                { typeof(Armor), armorFromListBox },
                { typeof(Shield), shieldsFromListBox } };
            toListBoxes = new Dictionary<Type, CustomSortedItemListBox> { 
                { typeof(Weapon), weaponsToListBox },
                { typeof(ChemistItem), itemsToListBox },
                { typeof(Accessory), accessoriesToListBox },
                { typeof(Armor), armorToListBox },
                { typeof(Shield), shieldsToListBox } };
            listBoxes.ForEach(listBox => listBox.Format += listBox_Format);
        }

        void listBox_Format(object sender, ListControlConvertEventArgs e)
        {
            ListBox lb = sender as ListBox;
            Item i = e.ListItem as Item;
            bool newVal = storeInventory[i];
            bool defaultVal = storeInventory.Default[i];
            if (newVal != defaultVal)
            {
                e.Value = string.Format("*{0}", e.Value);
            }
        }

        public StoreInventory StoreInventory
        {
            get
            {
                return storeInventory;
            }
            set
            {
                if ( value == null )
                {
                    Enabled = false;
                    storeInventory = null;
                }
                else if (value != storeInventory)
                {
                    storeInventory = value;
                    Enabled = true;
                    UpdateView();
                }
            }
        }


        private void UpdateView()
        {
            storeInventory.DataChanged -= storeInventory_DataChanged;
            foreach ( var listbox in listBoxes )
            {
                listbox.BeginUpdate();
                listbox.SuspendLayout();
                listbox.Items.Clear();
            }

            for ( int i = 0; i < 254; i++ )
            {
                if ( StoreInventory[Item.DummyItems[i]] )
                {
                    toListBoxes[FFTPatch.Items.Items[i].GetType()].Items.Add( Item.DummyItems[i] );
                }
                else
                {
                    fromListBoxes[FFTPatch.Items.Items[i].GetType()].Items.Add( Item.DummyItems[i] );
                }
            }

            for ( int i = 254; i < 256; i++ )
            {
                if ( StoreInventory[Item.DummyItems[i]] )
                {
                    toListBoxes[typeof( ChemistItem )].Items.Add( Item.DummyItems[i] );
                }
                else
                {
                    fromListBoxes[typeof( ChemistItem )].Items.Add( Item.DummyItems[i] );
                }
            }
            foreach ( var listbox in listBoxes )
            {
                listbox.SortPub();
                listbox.ResumeLayout();
                listbox.EndUpdate();
            }
            storeInventory.DataChanged += storeInventory_DataChanged;
        }

        private bool valueChangedOffScreen = false;

        private void storeInventory_DataChanged(object sender, EventArgs args)
        {
            if (!Visible)
            {
                valueChangedOffScreen = true;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && valueChangedOffScreen)
            {
                UpdateView();
            }
            else if (!Visible)
            {
                valueChangedOffScreen = false;
            }
        }

        private void addDualList_BeforeAction( object sender, FFTPatcher.Controls.DualListCancelEventArgs e )
        {
            Item i = e.Item as Item;
            this.StoreInventory[i] = true;
            OnDataChanged( this, EventArgs.Empty );
        }

        private void removeDualList_BeforeAction(object sender, FFTPatcher.Controls.DualListCancelEventArgs e)
        {
            Item i = e.Item as Item;
            this.StoreInventory[i] = false;
            OnDataChanged( this, EventArgs.Empty );
        }

        private void dualList_AfterAction(object sender, FFTPatcher.Controls.DualListActionEventArgs e)
        {
            Item i = e.Item as Item;
            Type t = FFTPatch.Items.Items[i.Offset].GetType();
            
            if (storeInventory[i] && !storeInventory.Default[i])
            {
                // Added to right box
                toListBoxes[t].SortPub();
            }
            else if (!storeInventory[i] && storeInventory.Default[i])
            {
                // Added to left box
                fromListBoxes[t].SortPub();
            }
        }
    }
}
