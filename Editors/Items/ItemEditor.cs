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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class ItemEditor : BaseEditor
    {
		#region Instance Variables (12) 

        private List<ComboBoxWithDefault> comboBoxes = new List<ComboBoxWithDefault>();
        private bool ignoreChanges = false;
        private Item item;
        private string[] itemBools = new string[] {
            "Weapon", "Shield", "Head", "Body",
            "Accessory", "Blank1", "Rare", "Blank2" };
        private List<string> itemFormulaItems;
        private Context ourContext = Context.Default;
        private List<ItemSubType> pspItemTypes = new List<ItemSubType>( (ItemSubType[])Enum.GetValues( typeof( ItemSubType ) ) );
        private List<ItemSubType> psxItemTypes = new List<ItemSubType>( (ItemSubType[])Enum.GetValues( typeof( ItemSubType ) ) );
        private List<LinkLabel> secondTableLinks = new List<LinkLabel>();
        private List<NumericUpDownWithDefault> spinners = new List<NumericUpDownWithDefault>();
        private string[] weaponBools = new string[] {
            "Striking", "Lunging", "Direct", "Arc",
            "TwoSwords", "TwoHands", "Throwable", "Force2Hands" };
        private List<string> weaponCastSpellItems;
        private ShopsFlags[] shops = new ShopsFlags[16] { 
            ShopsFlags.None,
            ShopsFlags.Lesalia, ShopsFlags.Riovanes, ShopsFlags.Igros, ShopsFlags.Lionel,
            ShopsFlags.Limberry, ShopsFlags.Zeltennia, ShopsFlags.Gariland, ShopsFlags.Yardrow,
            ShopsFlags.Goland, ShopsFlags.Dorter, ShopsFlags.Zaland, ShopsFlags.Goug,
            ShopsFlags.Warjilis, ShopsFlags.Bervenia, ShopsFlags.Zarghidas };

		#endregion Instance Variables 

		#region Public Properties

        public Item Item
        {
            get { return item; }
            set
            {
                SetItem(value, ourContext);
            }
        }

        public AllStoreInventories StoreInventories { get; set; }
            
		#endregion Public Properties 

		#region Constructors

        public ItemEditor()
        {
            InitializeComponent();
            ignoreChanges = true;

            spinners.AddRange( new NumericUpDownWithDefault[] { 
                paletteSpinner, graphicSpinner, enemyLevelSpinner, itemAttributesSpinner, priceSpinner, 
                weaponRangeSpinner, weaponPowerSpinner, weaponEvadePercentageSpinner, weaponSpellStatusSpinner, 
                armorHPBonusSpinner, armorMPBonusSpinner, 
                shieldMagicBlockRateSpinner, shieldPhysicalBlockRateSpinner, 
                accessoryMagicEvadeRateSpinner, accessoryPhysicalEvadeRateSpinner, 
                chemistItemSpellStatusSpinner, chemistItemXSpinner, secondTableIdSpinner,
				unknown1Spinner, unknown2Spinner, weaponUnknownSpinner                              
            } );
            
            comboBoxes.AddRange( new ComboBoxWithDefault[] { itemTypeComboBox, shopAvailabilityComboBox } );
            secondTableLinks.AddRange( new LinkLabel[] { weaponJumpLabel, shieldJumpLabel, headBodyJumpLabel, accessoryJumpLabel, chemistItemJumpLabel } );

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            foreach( ComboBoxWithDefault comboBox in comboBoxes )
            {
                comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            }
            foreach( LinkLabel jumpLabel in secondTableLinks )
            {
                jumpLabel.Click += new EventHandler( jumpLabel_Click );
                jumpLabel.TabStop = false;
            }
            weaponJumpLabel.Tag = LabelClickedEventArgs.SecondTableType.Weapon;
            shieldJumpLabel.Tag = LabelClickedEventArgs.SecondTableType.Shield;
            headBodyJumpLabel.Tag = LabelClickedEventArgs.SecondTableType.HeadBody;
            accessoryJumpLabel.Tag = LabelClickedEventArgs.SecondTableType.Accessory;
            chemistItemJumpLabel.Tag = LabelClickedEventArgs.SecondTableType.ChemistItem;

            weaponFormulaComboBox.SelectedIndexChanged += weaponFormulaComboBox_SelectedIndexChanged;
            chemistItemFormulaComboBox.SelectedIndexChanged += chemistItemFormulaComboBox_SelectedIndexChanged;
            weaponCastSpellComboBox.SelectedIndexChanged += weaponCastSpellComboBox_SelectedIndexChanged;

            itemAttributesCheckedListBox.ItemCheck += itemAttributesCheckedListBox_ItemCheck;
            weaponAttributesCheckedListBox.ItemCheck += weaponAttributesCheckedListBox_ItemCheck;

            shopAvailabilityComboBox.Items.Clear();
            //shopAvailabilityComboBox.Items.AddRange( ShopAvailability.AllAvailabilities.ToArray() );
            psxItemTypes.Remove( ItemSubType.LipRouge );
            psxItemTypes.Remove( ItemSubType.FellSword );

            chemistItemSpellStatusLabel.Click += chemistItemSpellStatusLabel_Click;
            chemistItemSpellStatusLabel.TabStop = false;
            weaponSpellStatusLabel.Click += weaponSpellStatusLabel_Click;
            weaponSpellStatusLabel.TabStop = false;
            itemAttributesLabel.Click += itemAttributesLabel_Click;
            itemAttributesLabel.TabStop = false;
            weaponCastSpellComboBox.DataSource = weaponCastSpellItems;
            chemistItemFormulaComboBox.DataSource = itemFormulaItems;
            weaponElementsEditor.DataChanged += OnDataChanged;

            storeInventoryCheckedListBox.ItemCheck += new ItemCheckEventHandler( storeInventoryCheckedListBox_ItemCheck );
            ignoreChanges = false;
        }

        public void SetItem(Item value, Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                this.item = null;
            }
            else if (value != item)
            {
                this.item = value;
                this.Enabled = true;
                UpdateView(context);
            }
        }

        void storeInventoryCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if ( !ignoreChanges )
            {
                if ( e.NewValue == CheckState.Checked )
                {
                    StoreInventories.AddToInventory( shops[e.Index], item );
                }
                else if ( e.NewValue == CheckState.Unchecked )
                {
                    StoreInventories.RemoveFromInventory( shops[e.Index], item );
                }
            }
        }

        public void BuildItemNameLists(Context context)
        {
            weaponCastSpellItems = new List<string>( 256 );
            for( int i = 0; i < 256; i++ )
            {
                weaponCastSpellItems.Add( string.Format( "{0:X2} - {1}", i, AllAbilities.GetNames(context)[i] ) );
            }

            itemFormulaItems = new List<string>( 256 );
            Dictionary<int, string> t = new Dictionary<int, string>( 5 );
            t.Add( 0x38, "Remove status" );
            t.Add( 0x48, "Restore 10*X HP" );
            t.Add( 0x49, "Restore 10*X MP" );
            t.Add( 0x4A, "Restore all HP/MP" );
            t.Add( 0x4B, "Remove status and restore (1..(X-1)) HP" );
            for( int i = 0; i < 256; i++ )
            {
                if( t.ContainsKey( i ) )
                {
                    itemFormulaItems.Add( t[i] );
                }
                else
                {
                    itemFormulaItems.Add( string.Format( "{0:X2}", i ) );
                }
            }

        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            //UpdateView();
        }

		#endregion Constructors 

		#region Private Methods (12) 

        private void chemistItemFormulaComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges && item is ChemistItem )
            {
                (item as ChemistItem).Formula = (byte)chemistItemFormulaComboBox.SelectedIndex;
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void chemistItemSpellStatusLabel_Click( object sender, EventArgs e )
        {
            if( InflictStatusClicked != null && chemistItemSpellStatusSpinner.Value <= 127 )
            {
                InflictStatusClicked( this, new LabelClickedEventArgs( (byte)chemistItemSpellStatusSpinner.Value ) );
            }
        }

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty( item, c.Tag.ToString(), c.SelectedItem );
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void itemAttributesCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                ReflectionHelpers.SetFieldOrProperty( item, itemBools[e.Index], e.NewValue == CheckState.Checked );
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void itemAttributesLabel_Click( object sender, EventArgs e )
        {
            if( ItemAttributesClicked != null && itemAttributesSpinner.Value <= (ourContext == Context.US_PSP ? 0x64 : 0x4F) )
            {
                ItemAttributesClicked( this, new LabelClickedEventArgs( (byte)itemAttributesSpinner.Value ) );
            }
        }

        private void jumpLabel_Click( object sender, EventArgs e )
        {
            if( (secondTableLinks as IList).Contains( sender ) )
            {
                if( SecondTableLinkClicked != null )
                {
                    LabelClickedEventArgs.SecondTableType type = (LabelClickedEventArgs.SecondTableType)((sender as LinkLabel).Tag);
                    SecondTableLinkClicked( this, new LabelClickedEventArgs( (byte)secondTableIdSpinner.Value, type ) );
                }
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
                if( spinner == priceSpinner )
                {
                    ReflectionHelpers.SetFieldOrProperty( item, spinner.Tag.ToString(), (UInt16)spinner.Value );
                }
                else
                {
                    ReflectionHelpers.SetFieldOrProperty( item, spinner.Tag.ToString(), (byte)spinner.Value );
                }
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        public void UpdateView(Context context)
        {
            ignoreChanges = true;
            SuspendLayout();
            weaponPanel.SuspendLayout();
            shieldPanel.SuspendLayout();
            armorPanel.SuspendLayout();
            accessoryPanel.SuspendLayout();
            chemistItemPanel.SuspendLayout();
            storeInventoryCheckedListBox.BeginUpdate();

            weaponPanel.Visible = item is Weapon;
            weaponPanel.Enabled = item is Weapon;

            shieldPanel.Visible = item is Shield;
            shieldPanel.Enabled = item is Shield;

            armorPanel.Visible = item is Armor;
            armorPanel.Enabled = item is Armor;

            accessoryPanel.Visible = item is Accessory;
            accessoryPanel.Enabled = item is Accessory;

            chemistItemPanel.Visible = item is ChemistItem;
            chemistItemPanel.Enabled = item is ChemistItem;

            if (context == Context.US_PSP && ourContext != Context.US_PSP)
            {
                itemTypeComboBox.DataSource = new List<ItemSubType>(pspItemTypes);
                weaponFormulaComboBox.DataSource = new List<AbilityFormula>(AbilityFormula.PSPAbilityFormulas);
                storeInventoryCheckedListBox.Items.Clear();
                foreach ( ShopsFlags shop in shops )
                {
                    storeInventoryCheckedListBox.Items.Add( PatcherLib.PSPResources.Lists.ShopNames[shop] );
                }

                ourContext = Context.US_PSP;

            }
            else if (context == Context.US_PSX && ourContext != Context.US_PSX)
            {
                itemTypeComboBox.DataSource = new List<ItemSubType>(psxItemTypes);
                weaponFormulaComboBox.DataSource = new List<AbilityFormula>(AbilityFormula.PSXAbilityFormulas);
                storeInventoryCheckedListBox.BeginUpdate();
                storeInventoryCheckedListBox.Items.Clear();
                foreach ( ShopsFlags shop in shops )
                {
                    storeInventoryCheckedListBox.Items.Add( PatcherLib.PSXResources.Lists.ShopNames[shop] );
                }
                storeInventoryCheckedListBox.EndUpdate();

                ourContext = Context.US_PSX;
            }

            shopAvailabilityComboBox.Items.Clear();
            shopAvailabilityComboBox.Items.AddRange( ShopAvailability.GetAllAvailabilities(context).ToArray() );

            if( item is Weapon )
            {
                Weapon w = item as Weapon;

                if( w.WeaponDefault != null )
                {
                    weaponAttributesCheckedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( w, weaponBools ), w.WeaponDefault.ToWeaponBoolArray() );
                }

                weaponRangeSpinner.SetValueAndDefault(
                    w.Range,
                    w.WeaponDefault.Range );
                weaponFormulaComboBox.SetValueAndDefault(
                    weaponFormulaComboBox.Items[w.Formula.Value],
                    weaponFormulaComboBox.Items[w.WeaponDefault.Formula.Value] );
                weaponPowerSpinner.SetValueAndDefault( 
                    w.WeaponPower, 
                    w.WeaponDefault.WeaponPower );
                weaponEvadePercentageSpinner.SetValueAndDefault(
                    w.EvadePercentage,
                    w.WeaponDefault.EvadePercentage );

                if( w.Formula.Value == 2 )
                {
                    weaponCastSpellComboBox.SetValueAndDefault(
                        weaponCastSpellComboBox.Items[w.InflictStatus],
                        weaponCastSpellComboBox.Items[w.WeaponDefault.InflictStatus] );
                    weaponCastSpellComboBox.Visible = true;
                    weaponCastSpellLabel.Visible = true;
                    weaponSpellStatusSpinner.Visible = false;
                    hLabel4.Visible = false;
                    weaponSpellStatusLabel.Visible = false;
                }
                else
                {
                    weaponSpellStatusSpinner.SetValueAndDefault(
                        w.InflictStatus,
                        w.WeaponDefault.InflictStatus );
                    weaponCastSpellComboBox.Visible = false;
                    weaponCastSpellLabel.Visible = false;
                    weaponSpellStatusSpinner.Visible = true;
                    hLabel4.Visible = true;
                    weaponSpellStatusLabel.Visible = true;
                }

                weaponElementsEditor.SetValueAndDefaults( w.Elements, w.WeaponDefault.Elements );
                weaponUnknownSpinner.SetValueAndDefault( w.Unknown, w.WeaponDefault.Unknown );
            }
            else if( item is Shield )
            {
                shieldPhysicalBlockRateSpinner.SetValueAndDefault(
                    (item as Shield).PhysicalBlockRate,
                    (item as Shield).ShieldDefault.PhysicalBlockRate );
                shieldMagicBlockRateSpinner.SetValueAndDefault( 
                    (item as Shield).MagicBlockRate, 
                    (item as Shield).ShieldDefault.MagicBlockRate );
            }
            else if( item is Armor )
            {
                armorHPBonusSpinner.SetValueAndDefault(
                    (item as Armor).HPBonus,
                    (item as Armor).ArmorDefault.HPBonus );
                armorMPBonusSpinner.SetValueAndDefault( 
                    (item as Armor).MPBonus,
                    (item as Armor).ArmorDefault.MPBonus );
            }
            else if( item is Accessory )
            {
                accessoryMagicEvadeRateSpinner.SetValueAndDefault( 
                    (item as Accessory).MagicEvade,
                    (item as Accessory).AccessoryDefault.MagicEvade );
                accessoryPhysicalEvadeRateSpinner.SetValueAndDefault( 
                    (item as Accessory).PhysicalEvade,
                    (item as Accessory).AccessoryDefault.PhysicalEvade );
            }
            else if( item is ChemistItem )
            {
                chemistItemFormulaComboBox.SetValueAndDefault(
                    chemistItemFormulaComboBox.Items[(item as ChemistItem).Formula],
                    chemistItemFormulaComboBox.Items[(item as ChemistItem).ChemistItemDefault.Formula] );
                chemistItemSpellStatusSpinner.SetValueAndDefault( 
                    (item as ChemistItem).InflictStatus,
                    (item as ChemistItem).ChemistItemDefault.InflictStatus );
                chemistItemXSpinner.SetValueAndDefault( 
                    (item as ChemistItem).X,
                    (item as ChemistItem).ChemistItemDefault.X );
            }

            paletteSpinner.SetValueAndDefault( item.Palette, item.Default.Palette );
            graphicSpinner.SetValueAndDefault( item.Graphic, item.Default.Graphic );
            enemyLevelSpinner.SetValueAndDefault( item.EnemyLevel, item.Default.EnemyLevel );

            itemTypeComboBox.SetValueAndDefault( item.ItemType, item.Default.ItemType );

            itemAttributesSpinner.SetValueAndDefault( item.SIA, item.Default.SIA );
            secondTableIdSpinner.SetValueAndDefault( item.SecondTableId, item.Default.SecondTableId );
            priceSpinner.SetValueAndDefault( item.Price, item.Default.Price );
            shopAvailabilityComboBox.SetValueAndDefault( item.ShopAvailability, item.Default.ShopAvailability );

            unknown1Spinner.SetValueAndDefault( item.Unknown1, item.Default.Unknown1 );
            unknown2Spinner.SetValueAndDefault( item.Unknown2, item.Default.Unknown2 );
            
            if (item.Offset < 256)
            {
                storeInventoryCheckedListBox.Visible = true;
                storeInventoryCheckedListBox.SetValuesAndDefaults(
                    StoreInventories.IsItemInShops(item, shops),
                    StoreInventories.Default.IsItemInShops(item, shops));
            }
            else
            {
                storeInventoryCheckedListBox.Visible = false;
            }

            if( item.Default != null )
            {
                itemAttributesCheckedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( item, itemBools ), item.Default.ToBoolArray() );
            }

            weaponPanel.ResumeLayout();
            shieldPanel.ResumeLayout();
            armorPanel.ResumeLayout();
            accessoryPanel.ResumeLayout();
            chemistItemPanel.ResumeLayout();
            storeInventoryCheckedListBox.EndUpdate();
            ResumeLayout();
            ignoreChanges = false;
        }

        private void weaponAttributesCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                Weapon w = item as Weapon;
                ReflectionHelpers.SetFieldOrProperty( w, weaponBools[e.Index], e.NewValue == CheckState.Checked );
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void weaponCastSpellComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges && item is Weapon && weaponFormulaComboBox.SelectedIndex == 2 )
            {
                (item as Weapon).InflictStatus = (byte)weaponCastSpellComboBox.SelectedIndex;
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void weaponFormulaComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges && item is Weapon )
            {
                (item as Weapon).Formula = weaponFormulaComboBox.SelectedItem as AbilityFormula;
                if( weaponFormulaComboBox.SelectedIndex == 2 )
                {
                    weaponSpellStatusLabel.Visible = false;
                    hLabel4.Visible = false;
                    weaponSpellStatusSpinner.Visible = false;

                    weaponCastSpellComboBox.Visible = true;
                    weaponCastSpellLabel.Visible = true;

                    bool old = ignoreChanges;
                    ignoreChanges = true;
                    weaponCastSpellComboBox.SelectedIndex = (byte)((item as Weapon).InflictStatus);
                    ignoreChanges = old;
                }
                else
                {
                    weaponSpellStatusLabel.Visible = true;
                    hLabel4.Visible = true;
                    weaponSpellStatusSpinner.Visible = true;

                    weaponCastSpellComboBox.Visible = false;
                    weaponCastSpellLabel.Visible = false;

                    bool old = ignoreChanges;
                    ignoreChanges = true;
                    weaponSpellStatusSpinner.SetValueAndDefault(
                        (byte)((item as Weapon).InflictStatus),
                        (item as Weapon).WeaponDefault.InflictStatus );
                    ignoreChanges = old;
                }
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void weaponSpellStatusLabel_Click( object sender, EventArgs e )
        {
            if( InflictStatusClicked != null && weaponSpellStatusSpinner.Value <= 127 )
            {
                InflictStatusClicked( this, new LabelClickedEventArgs( (byte)weaponSpellStatusSpinner.Value ) );
            }
        }

		#endregion Private Methods 

        public event EventHandler<LabelClickedEventArgs> InflictStatusClicked;
        public event EventHandler<LabelClickedEventArgs> ItemAttributesClicked;
        public event EventHandler<LabelClickedEventArgs> SecondTableLinkClicked;
    }
}
