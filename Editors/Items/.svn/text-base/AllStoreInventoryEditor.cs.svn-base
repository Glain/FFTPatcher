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

        public void UpdateView( AllStoreInventories inventories )
        {
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.DataSource = inventories.Stores.ToArray();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = 0;
            storeInventoryEditor1.StoreInventory = inventories.Stores[0];
        }

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            storeInventoryEditor1.StoreInventory = comboBox1.SelectedItem as StoreInventory;
        }
    }
}
