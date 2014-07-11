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
using System.Collections.Generic;
using System.ComponentModel;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents a generic item.
    /// </summary>
    public class Item : IChangeable, ISupportDigest, ISupportDefault<Item>
    {
		#region Instance Variables (11) 

        private bool accessory;
        private bool blank1;
        private bool blank2;
        private bool body;
        protected static readonly List<string> digestableProperties = new List<string>( new string[] {
            "Palette", "Graphic", "EnemyLevel", "ItemType", "SIA", "Price", "ShopAvailability", "Weapon", 
            "Shield", "Head", "Body", "Accessory", "Blank1", "Rare", "Blank2", "SecondTableId" } );
        private bool head;
        private static List<Item> pspEventItems;
        private static List<Item> psxEventItems;
        private bool rare;
        private bool shield;
        private bool weapon;

		#endregion Instance Variables 

		#region Public Properties (31) 

        public bool Accessory { get { return accessory; } set { accessory = value; } }

        public bool Blank1 { get { return blank1; } set { blank1 = value; } }

        public bool Blank2 { get { return blank2; } set { blank2 = value; } }

        public bool Body { get { return body; } set { body = value; } }

        public Item Default { get; private set; }

        public virtual IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public static List<Item> DummyItems
        {
            get
            {
                return FFTPatch.Context == Context.US_PSP ? PSPDummies : PSXDummies;
            }
        }

        public byte EnemyLevel { get; set; }

        public static List<Item> EventItems
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspEventItems : psxEventItems; }
        }

        [Hex]
        public byte Graphic { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public virtual bool HasChanged
        {
            get
            {
                return (Default != null) &&
                    (ItemType != Default.ItemType ||
                    Accessory != Default.Accessory ||
                    Blank1 != Default.Blank1 ||
                    Blank2 != Default.Blank2 ||
                    Body != Default.Body ||
                    EnemyLevel != Default.EnemyLevel ||
                    Graphic != Default.Graphic ||
                    Head != Default.Head ||
                    Palette != Default.Palette ||
                    Price != Default.Price ||
                    Rare != Default.Rare ||
                    SecondTableId != Default.SecondTableId ||
                    Shield != Default.Shield ||
                    ShopAvailability.ToByte() != Default.ShopAvailability.ToByte() ||
                    SIA != Default.SIA ||
                    Unknown1 != Default.Unknown1 ||
                    Unknown2 != Default.Unknown2 ||
                    Weapon != Default.Weapon);
            }
        }

        public bool Head { get { return head; } set { head = value; } }

        public static IList<string> ItemNames
        {
            get
            {
                return FFTPatch.Context == Context.US_PSP ? PSPNames : PSXNames;
            }
        }

        public ItemSubType ItemType { get; set; }

        public string Name { get; private set; }

        public UInt16 Offset { get; private set; }

        [Hex]
        public byte Palette { get; set; }

        public UInt16 Price { get; set; }

        public static List<Item> PSPDummies { get; private set; }

        public static IList<string> PSPNames { get; private set; }

        public static List<Item> PSXDummies { get; private set; }

        public static IList<string> PSXNames { get; private set; }

        public bool Rare { get { return rare; } set { rare = value; } }

        [Hex]
        public byte SecondTableId { get; set; }

        public Item Self { get { return this; } }

        public bool Shield { get { return shield; } set { shield = value; } }

        public ShopAvailability ShopAvailability { get; set; }

        [Hex]
        public byte SIA { get; set; }

        public byte Unknown1 { get; set; }

        public byte Unknown2 { get; set; }

        public bool Weapon { get { return weapon; } set { weapon = value; } }

		#endregion Public Properties 

		#region Constructors (4) 

        static Item()
        {
            PSPDummies = new List<Item>( 316 );
            pspEventItems = new List<Item>( 256 );
            psxEventItems = new List<Item>( 256 );

            PSPNames = PSPResources.Lists.Items;
            PSXNames = PSXResources.Lists.Items;

            for( int i = 0; i < 316; i++ )
            {
                Item item = new Item();
                item.Offset = (UInt16)i;
                item.Name = PSPNames[i];
                PSPDummies.Add( item );
                if( i <= 255 )
                {
                    pspEventItems.Add( item );
                }
            }

            PSXDummies = new List<Item>( 256 );
            for( int i = 0; i < 256; i++ )
            {
                Item item = new Item();
                item.Offset = (UInt16)i;
                item.Name = PSXNames[i];
                PSXDummies.Add( item );
                psxEventItems.Add( item );
            }

            Item rand = new Item();
            rand.Name = "<Random>";
            rand.Offset = 0xFE;

            Item none = new Item();
            none.Name = "<None>";
            none.Offset = 0xFF;
            pspEventItems[0xFE] = rand;
            psxEventItems[0xFE] = rand;
            pspEventItems[0xFF] = none;
            psxEventItems[0xFF] = none;
        }

        private Item()
        {
        }

        protected Item( UInt16 offset, IList<byte> bytes )
        {
            Name = ItemNames[offset];
            Offset = offset;
            Palette = bytes[0];
            Graphic = bytes[1];
            EnemyLevel = bytes[2];
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[3], ref weapon, ref shield, ref head, ref body, ref accessory, ref blank1, ref rare, ref blank2 );
            SecondTableId = bytes[4];
            ItemType = (ItemSubType)bytes[5];
            Unknown1 = bytes[6];
            SIA = bytes[7];
            Price = PatcherLib.Utilities.Utilities.BytesToUShort( bytes[8], bytes[9] );
            ShopAvailability = ShopAvailability.AllAvailabilities[bytes[10]];
            Unknown2 = bytes[11];
        }

        protected Item( UInt16 offset, IList<byte> bytes, Item defaults )
            : this( offset, bytes )
        {
            Default = defaults;
        }

		#endregion Constructors 

		#region Public Methods (9) 

        public static void CopyAll( Item source, Item destination )
        {
            if( source.GetType() != destination.GetType() )
            {
                throw new ArgumentException( "Can't copy between different item types" );
            }

            if( source is Accessory )
            {
                FFTPatcher.Datatypes.Accessory.CopyAll( source as Accessory, destination as Accessory );
            }
            else if( source is Armor )
            {
                FFTPatcher.Datatypes.Armor.CopyAll(source as Armor, destination as Armor);
            }
            else if( source is ChemistItem )
            {
                FFTPatcher.Datatypes.ChemistItem.CopyAll(source as ChemistItem, destination as ChemistItem);
            }
            else if( source is Shield )
            {
                FFTPatcher.Datatypes.Shield.CopyAll(source as Shield, destination as Shield);
            }
            else if( source is Weapon )
            {
                FFTPatcher.Datatypes.Weapon.CopyAll(source as Weapon, destination as Weapon);
            }
            else
            {
                throw new Exception( "Something terrible happened" );
            }
        }

        public void CopyAllTo( Item destination )
        {
            CopyAll( this, destination );
        }

        public static void CopyCommon( Item source, Item destination )
        {
            destination.Palette = source.Palette;
            destination.Graphic = source.Graphic;
            destination.EnemyLevel = source.EnemyLevel;
            destination.Weapon = source.Weapon;
            destination.Shield = source.Shield;
            destination.Head = source.Head;
            destination.Body = source.Body;
            destination.Accessory = source.Accessory;
            destination.Blank1 = source.Blank1;
            destination.Rare = source.Rare;
            destination.Blank2 = source.Blank2;
            destination.SecondTableId = source.SecondTableId;
            destination.ItemType = source.ItemType;
            destination.Unknown1 = source.Unknown1;
            destination.SIA = source.SIA;
            destination.Price = source.Price;
            destination.ShopAvailability = source.ShopAvailability;
            destination.Unknown2 = source.Unknown2;
        }

        public void CopyCommonTo( Item destination )
        {
            CopyCommon( this, destination );
        }

        public static Item GetItemAtOffset( UInt16 offset )
        {
            return DummyItems.Find( i => i.Offset == offset );
        }

        public bool[] ToBoolArray()
        {
            return new bool[8] {
                Weapon, Shield, Head, Body, Accessory, Blank1, Rare, Blank2 };
        }

        public virtual byte[] ToFirstByteArray()
        {
            return new byte[0];
        }

        public virtual byte[] ToSecondByteArray()
        {
            return new byte[0];
        }

        public override string ToString()
        {
            return (HasChanged ? "*" : "") +  Offset.ToString( "X2" ) + " " + Name;
        }

		#endregion Public Methods 

		#region Protected Methods (1) 

        protected List<byte> ToByteArray()
        {
            List<byte> result = new List<byte>( 12 );
            result.Add( Palette );
            result.Add( Graphic );
            result.Add( EnemyLevel );
            result.Add( PatcherLib.Utilities.Utilities.ByteFromBooleans( weapon, shield, head, body, accessory, blank1, rare, blank2 ) );
            result.Add( SecondTableId );
            result.Add( (byte)ItemType );
            result.Add( Unknown1 );
            result.Add( SIA );
            result.AddRange( Price.ToBytes() );
            result.Add( ShopAvailability.ToByte() );
            result.Add( Unknown2 );
            return result;
        }

		#endregion Protected Methods 
    }

    public class ShopAvailability
    {

        #region Static Fields (1)

        private static List<ShopAvailability> all;

        #endregion Static Fields

        #region Fields (3)

        private byte b;
        private string name;
        private string psxName;

        #endregion Fields

        #region Static Properties (1)


        public static List<ShopAvailability> AllAvailabilities
        {
            get
            {
                if( all == null )
                {
                    all = new List<ShopAvailability>( 256 );
                    for ( byte i = 0; i < PSPResources.Lists.ShopAvailabilities.Count; i++ )
                    {
                        ShopAvailability a = new ShopAvailability();
                        a.b = i;
                        a.name = PSPResources.Lists.ShopAvailabilities[i];
                        a.psxName = PSXResources.Lists.ShopAvailabilities[i];
                        all.Add( a );
                    }
                    for ( int i = PSPResources.Lists.ShopAvailabilities.Count; i <= 0xFF; i++ )
                    {
                        ShopAvailability a = new ShopAvailability();
                        a.b = (byte)i;
                        a.name = string.Format( "Unknown ({0})", i );
                        a.psxName = a.name;
                        all.Add( a );
                    }
                }

                return all;
            }
        }


        #endregion Static Properties

        #region Constructors (1)

        private ShopAvailability()
        {
        }

        #endregion Constructors

        #region Methods (2)


        public byte ToByte()
        {
            return b;
        }



        public override string ToString()
        {
            return FFTPatch.Context == Context.US_PSP ? name : psxName;
        }


        #endregion Methods

    }

    /// <summary>
    /// Represents types of items
    /// </summary>
    public enum ItemType
    {
        [Description( "Hand" )]
        Hand,

        [Description( "Item" )]
        Item,

        [Description( "Head" )]
        Head,

        [Description( "Body" )]
        Body,

        [Description( "Accessory" )]
        Accessory,

        [Description( "None" )]
        None
    }

    /// <summary>
    /// Represents subtypes of items
    /// </summary>
    public enum ItemSubType
    {
        Nothing,

        [Description( "Knife" )]
        Knife,

        [Description( "Ninja Blade" )]
        NinjaBlade,

        [Description( "Sword" )]
        Sword,

        [Description( "Knight's Sword" )]
        KnightsSword,

        [Description( "Katana" )]
        Katana,

        [Description( "Axe" )]
        Axe,

        [Description( "Rod" )]
        Rod,

        [Description( "Staff" )]
        Staff,

        [Description( "Flail" )]
        Flail,

        [Description( "Gun" )]
        Gun,

        [Description( "Crossbow" )]
        Crossbow,

        [Description( "Bow" )]
        Bow,

        [Description( "Instrument" )]
        Instrument,

        [Description( "Book" )]
        Book,

        [Description( "Polearm" )]
        Polearm,

        [Description( "Pole" )]
        Pole,

        [Description( "Bag" )]
        Bag,

        [Description( "Cloth" )]
        Cloth,

        [Description( "Shield" )]
        Shield,

        [Description( "Helmet" )]
        Helmet,

        [Description( "Hat" )]
        Hat,

        [Description( "Hair Adornment" )]
        HairAdornment,

        [Description( "Armor" )]
        Armor,

        [Description( "Clothing" )]
        Clothing,

        [Description( "Robe" )]
        Robe,

        [Description( "Shoes" )]
        Shoes,

        [Description( "Armguard" )]
        Armguard,

        [Description( "Ring" )]
        Ring,

        [Description( "Armlet" )]
        Armlet,

        [Description( "Cloak" )]
        Cloak,

        [Description( "Perfume" )]
        Perfume,

        [Description( "Throwing" )]
        Throwing,

        [Description( "Bomb" )]
        Bomb,

        [Description( "Chemist Item" )]
        None,

        [Description( "Fell Sword" )]
        FellSword,

        [Description( "Lip Rouge" )]
        LipRouge
    }
}
