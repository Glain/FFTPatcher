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

using System.Collections.Generic;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public class MoveFindItem : IChangeable, ISupportDigest, ISupportDefault<MoveFindItem>
    {
		#region Instance Variables (1) 

        private static string[] digestableProperties = new string[] {
            "X", "Y", "CommonItem", "RareItem",
            "Unknown1", "Unknown2", "Unknown3", "Unknown4", 
            "SteelNeedle", "SleepingGas", "Deathtrap", "Degenerator" };

		#endregion Instance Variables 

		#region Public Properties (16) 

        public Item CommonItem { get; set; }

        public bool Deathtrap { get; set; }

        public MoveFindItem Default { get; private set; }

        public bool Degenerator { get; set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public bool HasChanged
        {
            get
            {
                return Default != null && (
                    X != Default.X ||
                    Y != Default.Y ||
                    CommonItem.Offset != Default.CommonItem.Offset ||
                    RareItem.Offset != Default.RareItem.Offset ||
                    ( Unknown1 ^ Default.Unknown1 ) ||
                    ( Unknown2 ^ Default.Unknown2 ) ||
                    ( Unknown3 ^ Default.Unknown3 ) ||
                    ( Unknown4 ^ Default.Unknown4 ) ||
                    ( SteelNeedle ^ Default.SteelNeedle ) ||
                    ( SleepingGas ^ Default.SleepingGas ) ||
                    ( Deathtrap ^ Default.Deathtrap ) ||
                    ( Degenerator ^ Default.Degenerator ) );

            }
        }

        public Item RareItem { get; set; }

        public bool SleepingGas { get; set; }

        public bool SteelNeedle { get; set; }

        public byte Trap { get; set; }

        public bool Unknown1 { get; set; }

        public bool Unknown2 { get; set; }

        public bool Unknown3 { get; set; }

        public bool Unknown4 { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public MoveFindItem( IList<byte> bytes, Context context )
        {
            X = (byte)( ( bytes[0] & 0xF0 ) >> 4 );
            Y = (byte)( bytes[0] & 0x0F );
            CommonItem = Item.GetDummyItems(context)[bytes[3]];
            RareItem = Item.GetDummyItems(context)[bytes[2]];
            bool[] b = PatcherLib.Utilities.Utilities.BooleansFromByteMSB( bytes[1] );
            Unknown1 = b[0];
            Unknown2 = b[1];
            Unknown3 = b[2];
            Unknown4 = b[3];
            SteelNeedle = b[4];
            SleepingGas = b[5];
            Deathtrap = b[6];
            Degenerator = b[7];
        }

        public MoveFindItem( IList<byte> bytes, MoveFindItem def, Context context )
            : this( bytes, context )
        {
            this.Default = def;
        }

		#endregion Constructors 

		#region Public Methods (3) 

        public IList<byte> ToByteArray()
        {
            return new byte[] { 
                (byte)( ( ( X & 0x0F ) << 4 ) | ( Y & 0x0F ) ), 
                PatcherLib.Utilities.Utilities.ByteFromBooleans(Unknown1, Unknown2, Unknown3, Unknown4, SteelNeedle, SleepingGas, Deathtrap, Degenerator),
                (byte)(RareItem.Offset & 0xFF),
                (byte)(CommonItem.Offset & 0xFF) };
        }

        public static void Copy(MoveFindItem source, MoveFindItem destination)
        {
            destination.X = source.X;
            destination.Y = source.Y;
            destination.CommonItem = source.CommonItem;
            destination.RareItem = source.RareItem;
            destination.Unknown1 = source.Unknown1;
            destination.Unknown2 = source.Unknown2;
            destination.Unknown3 = source.Unknown3;
            destination.Unknown4 = source.Unknown4;
            destination.SteelNeedle = source.SteelNeedle;
            destination.SleepingGas = source.SleepingGas;
            destination.Deathtrap = source.Deathtrap;
            destination.Degenerator = source.Degenerator;
        }

        public void CopyTo(MoveFindItem destination)
        {
            Copy(this, destination);
        }

		#endregion Public Methods 
    }

    public class MapMoveFindItems : IChangeable, IXmlDigest, ISupportDefault<MapMoveFindItems>
    {
		#region Public Properties (4) 

        public MapMoveFindItems Default { get; private set; }

        public bool HasChanged
        {
            get { return Default != null && Items.Exists( item => item.HasChanged ); }
        }

        public IList<MoveFindItem> Items { get; private set; }

        public string Name { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public MapMoveFindItems( IList<byte> bytes, string name, Context context )
            : this( bytes, name, null, context )
        {
        }

        public MapMoveFindItems( IList<byte> bytes, string name, MapMoveFindItems def, Context context )
        {
            Default = def;
            Name = name;

            Items = new List<MoveFindItem>( 4 );
            if ( Default != null )
            {
                for ( int i = 0; i < 4; i++ )
                {
                    Items.Add( new MoveFindItem( bytes.Sub( i * 4, ( i + 1 ) * 4 - 1 ), def.Items[i], context ) );
                }
            }
            else
            {
                for ( int i = 0; i < 4; i++ )
                {
                    Items.Add( new MoveFindItem( bytes.Sub( i * 4, ( i + 1 ) * 4 - 1 ), context ) );
                }
            }
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public IList<byte> ToByteArray()
        {
            List<byte> result = new List<byte>( 4 * 4 );
            foreach ( MoveFindItem item in Items )
            {
                result.AddRange( item.ToByteArray() );
            }
            return result;
        }

        public override string ToString()
        {
            return ( HasChanged ? "*" : "" ) + Name;
        }

        public static void Copy(MapMoveFindItems source, MapMoveFindItems destination)
        {
            for (int i = 0; i < source.Items.Count; i++)
            {
                source.Items[i].CopyTo(destination.Items[i]);
            }
        }

        public void CopyTo(MapMoveFindItems destination)
        {
            Copy(this, destination);
        }

		#endregion Public Methods 


        #region IXmlDigest Members

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( GetType().Name );
                writer.WriteAttributeString( "name", Name );

                DigestGenerator.WriteDigestEntry( writer, "Tile1", Default.Items[0], Items[0] );
                DigestGenerator.WriteDigestEntry( writer, "Tile2", Default.Items[1], Items[1] );
                DigestGenerator.WriteDigestEntry( writer, "Tile3", Default.Items[2], Items[2] );
                DigestGenerator.WriteDigestEntry( writer, "Tile4", Default.Items[3], Items[3] );

                writer.WriteEndElement();
            }
        }

        #endregion
    }

    public class AllMoveFindItems : PatchableFile, IChangeable, IXmlDigest, ISupportDefault<AllMoveFindItems>, IGenerateCodes
    {
        private Context ourContext = Context.Default;

		#region Public Properties (3) 

        public AllMoveFindItems Default { get; private set; }

        public override bool HasChanged
        {
            get { return Default != null && MoveFindItems.Exists( item => item.HasChanged ); }
        }

        public MapMoveFindItems[] MoveFindItems { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public AllMoveFindItems( Context context, IList<byte> bytes )
            : this( context, bytes, null )
        {
        }

        public AllMoveFindItems( Context context, IList<byte> bytes, AllMoveFindItems def )
        {
            Default = def;
            ourContext = context;
            const int numMaps = 128;
            IList<string> names = context == Context.US_PSP ? PSPResources.Lists.MapNames : PSXResources.Lists.MapNames;

            List<MapMoveFindItems> moveFindItems = new List<MapMoveFindItems>( numMaps * 4 );
            if ( Default == null )
            {
                for ( int i = 0; i < numMaps; i++ )
                {
                    moveFindItems.Add( new MapMoveFindItems( bytes.Sub( i * 4 * 4, ( i + 1 ) * 4 * 4 - 1 ), names[i], context ) );
                }
            }
            else
            {
                for ( int i = 0; i < numMaps; i++ )
                {
                    moveFindItems.Add( new MapMoveFindItems( bytes.Sub( i * 4 * 4, ( i + 1 ) * 4 * 4 - 1 ), names[i], def.MoveFindItems[i], context ) );
                }
            }
            MoveFindItems = moveFindItems.ToArray();
        }

		#endregion Constructors 

		#region Public Methods (3) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            List<PatchedByteArray> result = new List<PatchedByteArray>();
            byte[] bytes = ToByteArray();

            if( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.MoveFindItems.GetPatchedByteArray(bytes));
            }
            else if( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.MoveFindItems.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            int numMaps = ourContext == Context.US_PSP ? 134 : 128;
            List<byte> result = new List<byte>( 4 * 4 * numMaps );
            foreach ( MapMoveFindItems items in MoveFindItems )
            {
                result.AddRange( items.ToByteArray() );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( MapMoveFindItems m in MoveFindItems )
                {
                    m.WriteXmlDigest( writer, FFTPatch );
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Move/Find Items" : "\"Move/Find Items";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.MoveFind, this.ToByteArray(), 0x274754 );
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.MoveFind, this.ToByteArray(), 0xF5E74, Codes.CodeEnabledOnlyWhen.Battle );
            }
        }

        #endregion
    }
}
