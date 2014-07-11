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

using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class EquipmentEditor : BaseEditor
    {
		#region Instance Variables (6) 

        private Equipment equipment;
        private static string[] FieldNames = new string[] {
            "Unused", "Knife", "NinjaBlade", "Sword", "KnightsSword", "Katana", "Axe", "Rod",
            "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
            "Pole", "Bag", "Cloth", "Shield", "Helmet", "Hat", "HairAdornment", "Armor",
            "Clothing", "Robe", "Shoes", "Armguard", "Ring", "Armlet", "Cloak", "Perfume",
            "Unknown1", "Unknown2", "Unknown3", "FellSword", "LipRouge", "Unknown6", "Unknown7", "Unknown8"};
        private bool ignoreChanges = false;
        private object[] pspItems;
        private static string[] PSXFieldNames = new string[] {
            "Unused", "Knife", "NinjaBlade", "Sword", "KnightsSword", "Katana", "Axe", "Rod",
            "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
            "Pole", "Bag", "Cloth", "Shield", "Helmet", "Hat", "HairAdornment", "Armor",
            "Clothing", "Robe", "Shoes", "Armguard", "Ring", "Armlet", "Cloak", "Perfume" };
        private object[] psxItems;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public Equipment Equipment
        {
            get { return equipment; }
            set
            {
                if (value == null)
                {
                    this.Enabled = false;
                    equipment = null;
                }
                else if (value != equipment)
                {
                    equipment = value;
                    UpdateView();
                    this.Enabled = true;
                }
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public EquipmentEditor()
        {
            InitializeComponent();
            equipmentCheckedListBox.ItemCheck += equipmentCheckedListBox_ItemCheck;
            psxItems = new object[] { 
                "", "Knife", "Ninja Blade", "Sword", "Knight\'s Sword", "Katana", "Axe", "Rod",
                "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
                "Pole", "Bag", "Cloth", "Shield", "Helmet", "Hat", "Hair Adornment", 
                "Armor", "Clothing", "Robe", "Shoes", "Armguard", "Ring", "Armlet", "Cloak", "Perfume"};
            pspItems = new object[] { 
                "", "Knife", "Ninja Blade", "Sword", "Knight\'s Sword", "Katana", "Axe", "Rod",
                "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
                "Pole", "Bag", "Cloth", "Shield", "Helmet", "Hat", "Hair Adornment", 
                "Armor", "Clothing", "Robe", "Shoes", "Armguard", "Ring", "Armlet", "Cloak", "Perfume",
                "Unknown1", "Unknown2", "Unknown3", "Fell Sword", "Lip Rouge", "Unknown6", "Unknown7", "Unknown8"};
        }

		#endregion Constructors 

		#region Private Methods (2) 

        private void equipmentCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!ignoreChanges)
            {
                ReflectionHelpers.SetFlag(equipment, FieldNames[e.Index], e.NewValue == CheckState.Checked);
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void UpdateView()
        {
            this.SuspendLayout();
            equipmentCheckedListBox.SuspendLayout();

            ignoreChanges = true;
            string[] fields = FFTPatch.Context == Context.US_PSP ? FieldNames : PSXFieldNames;
            if( FFTPatch.Context == Context.US_PSP && equipmentCheckedListBox.Items.Count != pspItems.Length )
            {
                equipmentCheckedListBox.Items.Clear();
                equipmentCheckedListBox.Items.AddRange( pspItems );
            }
            else if( FFTPatch.Context == Context.US_PSX && equipmentCheckedListBox.Items.Count != psxItems.Length )
            {
                equipmentCheckedListBox.Items.Clear();
                equipmentCheckedListBox.Items.AddRange( psxItems );
            }
            if( (equipment.Default != null) && (fields != null) )
            {
                equipmentCheckedListBox.SetValuesAndDefaults(
                    ReflectionHelpers.GetFieldsOrProperties<bool>( equipment, fields ),
                    ReflectionHelpers.GetFieldsOrProperties<bool>( equipment.Default, fields ) );
            }

            ignoreChanges = false;
            equipmentCheckedListBox.ResumeLayout();
            this.ResumeLayout();
        }

		#endregion Private Methods 
    }
}
