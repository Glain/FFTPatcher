using System;
using FFTPatcher.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Editors.Items
{
    public partial class AllStoreInventoryEditor : BaseEditor
    {
        public AllStoreInventoryEditor()
        {
            InitializeComponent();
        }

        public void UpdateView( AllStoreInventories inventories, AllItems items, PatcherLib.Datatypes.Context context )
        {
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.DataSource = inventories.Stores.ToArray();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = 0;

            storeInventoryEditor1.Items = items;
            //storeInventoryEditor1.StoreInventory = inventories.Stores[0];
            storeInventoryEditor1.SetStoreInventory(inventories.Stores[0], context);
        }

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            storeInventoryEditor1.StoreInventory = comboBox1.SelectedItem as StoreInventory;
        }
    }
}
