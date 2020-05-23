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
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents all items in memory.
    /// </summary>
    public class AllItems : PatchableFile, IXmlDigest, IGenerateCodes
    {
		#region Instance Variables (1) 

        private byte[] afterPhoenixDown;

		#endregion Instance Variables 

		#region Public Properties (2) 

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                foreach( Item i in Items )
                {
                    if( i.HasChanged )
                        return true;
                }

                return false;
            }
        }

        public List<Item> Items { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public AllItems( IList<byte> first, Context context )
            : this( first, null, context )
        {
        }

        public AllItems(IList<byte> first, IList<byte> second, Context context)
        {
            Items = new List<Item>();
            IList<byte> defaultFirst = second == null ? PSXResources.Binaries.OldItems : PSPResources.Binaries.OldItems;
            IList<byte> defaultSecond = second == null ? null : PSPResources.Binaries.NewItems;

            for( UInt16 i = 0; i < 0x80; i++ )
            {
                Items.Add( new Weapon(
                    i,
                    first.Sub( i * 12, (i + 1) * 12 - 1 ),
                    first.Sub( 0xC00 + i * 8, 0xC00 + (i + 1) * 8 - 1 ),
                    new Weapon( i,
                        defaultFirst.Sub( i * 12, (i + 1) * 12 - 1 ),
                        defaultFirst.Sub( 0xC00 + i * 8, 0xC00 + (i + 1) * 8 - 1 ),
                        context),
                        context) );
            }
            for( UInt16 i = 0x80; i < 0x90; i++ )
            {
                Items.Add( new Shield(
                    i,
                    first.Sub( i * 12, (i + 1) * 12 - 1 ),
                    first.Sub( 0x1000 + (i - 0x80) * 2, 0x1000 + ((i - 0x80) + 1) * 2 - 1 ),
                    new Shield(
                        i,
                        defaultFirst.Sub( i * 12, (i + 1) * 12 - 1 ),
                        defaultFirst.Sub( 0x1000 + (i - 0x80) * 2, 0x1000 + ((i - 0x80) + 1) * 2 - 1 ),
                        context),
                        context) );
            }
            for( UInt16 i = 0x90; i < 0xD0; i++ )
            {
                Items.Add( new Armor(
                    i,
                    first.Sub( i * 12, (i + 1) * 12 - 1 ),
                    first.Sub( 0x1020 + (i - 0x90) * 2, 0x1020 + ((i - 0x90) + 1) * 2 - 1 ),
                    new Armor(
                        i,
                        defaultFirst.Sub( i * 12, (i + 1) * 12 - 1 ),
                        defaultFirst.Sub( 0x1020 + (i - 0x90) * 2, 0x1020 + ((i - 0x90) + 1) * 2 - 1 ),
                        context),
                        context) );
            }
            for( UInt16 i = 0xD0; i < 0xF0; i++ )
            {
                Items.Add( new Accessory(
                    i,
                    first.Sub( i * 12, (i + 1) * 12 - 1 ),
                    first.Sub( 0x10A0 + (i - 0xD0) * 2, 0x10A0 + ((i - 0xD0) + 1) * 2 - 1 ),
                    new Accessory(
                        i,
                        defaultFirst.Sub( i * 12, (i + 1) * 12 - 1 ),
                        defaultFirst.Sub( 0x10A0 + (i - 0xD0) * 2, 0x10A0 + ((i - 0xD0) + 1) * 2 - 1 ),
                        context),
                        context) );
            }
            for( UInt16 i = 0xF0; i < 0xFE; i++ )
            {
                Items.Add( new ChemistItem(
                    i,
                    first.Sub( i * 12, (i + 1) * 12 - 1 ),
                    first.Sub( 0x10E0 + (i - 0xF0) * 3, 0x10E0 + ((i - 0xF0) + 1) * 3 - 1 ),
                    new ChemistItem(
                        i,
                        defaultFirst.Sub( i * 12, (i + 1) * 12 - 1 ),
                        defaultFirst.Sub( 0x10E0 + (i - 0xF0) * 3, 0x10E0 + ((i - 0xF0) + 1) * 3 - 1 ),
                        context),
                        context) );
            }
            afterPhoenixDown = new byte[0x18];
            for( int i = 0; i < 0x18; i++ )
            {
                afterPhoenixDown[i] = first[0xBE8 + i];
            }
            if( second != null )
            {
                for( UInt16 i = 0; i < 0x20; i++ )
                {
                    Items.Add( new Weapon(
                        (UInt16)(i + 0x100),
                        second.Sub( i * 12, (i + 1) * 12 - 1 ),
                        second.Sub( 0x2D0 + i * 8, 0x2D0 + (i + 1) * 8 - 1 ),
                        new Weapon(
                            (UInt16)(i + 0x100),
                            defaultSecond.Sub( i * 12, (i + 1) * 12 - 1 ),
                            defaultSecond.Sub( 0x2D0 + i * 8, 0x2D0 + (i + 1) * 8 - 1 ),
                            context),
                            context) );
                }
                for( UInt16 i = 0x20; i < 0x24; i++ )
                {
                    Items.Add( new Shield(
                        (UInt16)(i + 0x100),
                        second.Sub( i * 12, (i + 1) * 12 - 1 ),
                        second.Sub( 0x3D0 + (i - 0x20) * 2, 0x3D0 + ((i - 0x20) + 1) * 2 - 1 ),
                        new Shield(
                            (UInt16)(i + 0x100),
                            defaultSecond.Sub( i * 12, (i + 1) * 12 - 1 ),
                            defaultSecond.Sub( 0x3D0 + (i - 0x20) * 2, 0x3D0 + ((i - 0x20) + 1) * 2 - 1 ),
                            context),
                            context) );
                }
                for( UInt16 i = 0x24; i < 0x34; i++ )
                {
                    Items.Add( new Armor(
                        (UInt16)(i + 0x100),
                        second.Sub( i * 12, (i + 1) * 12 - 1 ),
                        second.Sub( 0x3D8 + (i - 0x24) * 2, 0x3D8 + ((i - 0x24) + 1) * 2 - 1 ),
                        new Armor(
                            (UInt16)(i + 0x100),
                            defaultSecond.Sub( i * 12, (i + 1) * 12 - 1 ),
                            defaultSecond.Sub( 0x3D8 + (i - 0x24) * 2, 0x3D8 + ((i - 0x24) + 1) * 2 - 1 ),
                            context),
                            context) );
                }
                for( UInt16 i = 0x34; i < 0x3C; i++ )
                {
                    Items.Add( new Accessory(
                        (UInt16)(i + 0x100),
                        second.Sub( i * 12, (i + 1) * 12 - 1 ),
                        second.Sub( 0x3F8 + (i - 0x34) * 2, 0x3F8 + ((i - 0x34) + 1) * 2 - 1 ),
                        new Accessory(
                            (UInt16)(i + 0x100),
                            defaultSecond.Sub( i * 12, (i + 1) * 12 - 1 ),
                            defaultSecond.Sub( 0x3F8 + (i - 0x34) * 2, 0x3F8 + ((i - 0x34) + 1) * 2 - 1 ),
                            context),
                            context) );
                }
            }
        }

		#endregion Constructors 

		#region Public Methods (5) 


        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 4 );

            var first = ToFirstByteArray();
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.OldItems.GetPatchedByteArray(first));
            }
            else if (context == Context.US_PSP)
            {
                var second = ToSecondByteArray();
                PatcherLib.Iso.PspIso.OldItems.ForEach(kl => result.Add(kl.GetPatchedByteArray(first)));
                PatcherLib.Iso.PspIso.NewItems.ForEach(kl => result.Add(kl.GetPatchedByteArray(second)));
            }

            return result;
        }

        public byte[] ToFirstByteArray()
        {
            List<byte> result = new List<byte>( 0x110A );
            for( int i = 0; i < 0xFE; i++ )
            {
                result.AddRange( Items[i].ToFirstByteArray() );
            }
            result.AddRange( afterPhoenixDown );
            for( int i = 0; i < 0xFE; i++ )
            {
                result.AddRange( Items[i].ToSecondByteArray() );
            }

            return result.ToArray();
        }

        public byte[] ToSecondByteArray()
        {
            List<byte> result = new List<byte>( 0x408 );
            for( int i = 0x100; i < 0x13C; i++ )
            {
                result.AddRange( Items[i - 2].ToFirstByteArray() );
            }
            for( int i = 0x100; i < 0x13C; i++ )
            {
                result.AddRange( Items[i - 2].ToSecondByteArray() );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( Item i in Items )
                {
                    if( i.HasChanged )
                    {
                        writer.WriteStartElement( i.GetType().Name );
                        writer.WriteAttributeString( "name", i.Name );
                        DigestGenerator.WriteXmlDigest( i, writer, false, false );
                        if( i is Weapon )
                        {
                            Weapon w = i as Weapon;
                            if( w.Formula.Value == 0x02 &&
                                (w.Formula.Value != w.WeaponDefault.Formula.Value || w.InflictStatus != w.WeaponDefault.InflictStatus) )
                            {
                                writer.WriteStartElement( "CastSpell" );
                                writer.WriteAttributeString( "default", AllAbilities.GetNames(FFTPatch.Context)[w.WeaponDefault.InflictStatus] );
                                writer.WriteAttributeString( "value", AllAbilities.GetNames(FFTPatch.Context)[w.InflictStatus] );
                                writer.WriteEndElement();
                            }
                            else if( w.InflictStatus != w.WeaponDefault.InflictStatus )
                            {
                                writer.WriteStartElement( "InflictStatusDescription" );
                                writer.WriteAttributeString( "default", FFTPatch.InflictStatuses.InflictStatuses[w.WeaponDefault.InflictStatus].Statuses.ToString() );
                                writer.WriteAttributeString( "value", FFTPatch.InflictStatuses.InflictStatuses[w.InflictStatus].Statuses.ToString() );
                                writer.WriteEndElement();
                            }
                        }

                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Items" : "\"Items";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                List<string> strings = new List<string>();
                strings.AddRange( Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.NewItems, this.ToSecondByteArray(), 0x25ADAC ) );
                strings.AddRange( Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.OldItems, this.ToFirstByteArray(), 0x329288 ) );
                return strings;
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.OldItems, this.ToFirstByteArray(), 0x062EB8 );
            }
        }

        #endregion
    }
}
