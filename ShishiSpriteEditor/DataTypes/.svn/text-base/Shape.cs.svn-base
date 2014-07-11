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
using System.Collections.ObjectModel;
using System.Drawing;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    [Serializable]
    public class Shape
    {

		#region Fields (1) 

        private List<Frame> frames;
        private string name = string.Empty;

        private static Shape type1 = new Shape( Properties.Resources.TYPE1_SHP, "TYPE1", new Rectangle( 106, 85, 48, 56 ) );
        private static Shape type2 = new Shape( Properties.Resources.TYPE2_SHP, "TYPE2", new Rectangle( 106, 85, 48, 56 ) );
        private static Shape kanzen = new Shape( Properties.Resources.KANZEN_SHP, "KANZEN", new Rectangle( 75, 5, 100, 115 ) );
        private static Shape arute = new Shape( Properties.Resources.ARUTE_SHP, "ARUTE", new Rectangle( 75, 35, 90, 100 ) );
        private static Shape cyoko = new Shape( Properties.Resources.CYOKO_SHP, "CYOKO", new Rectangle( 100, 80, 56, 56 ) );

        public static Shape TYPE1 { get { return type1; } }
        public static Shape TYPE2 { get { return type2; } }
        public static Shape KANZEN { get { return kanzen; } }
        public static Shape ARUTE { get { return arute; } }
        public static Shape CYOKO { get { return cyoko; } }

        private static IDictionary<SpriteType, Shape> shapes =
            new ReadOnlyDictionary<SpriteType, Shape>(
            new Dictionary<SpriteType, Shape> {
                    { SpriteType.TYPE1, type1 },
                    {SpriteType.TYPE2, type2},
                    {SpriteType.KANZEN, kanzen},
                    {SpriteType.ARUTE, arute},
                    {SpriteType.CYOKO, cyoko} } );

        public static IDictionary<SpriteType, Shape> Shapes { get { return shapes; } }

		#endregion Fields 

		#region Properties (2) 


        public IList<Frame> Frames { get { return frames.AsReadOnly(); } }

        public string Name 
        {
            get { return name; }
        }


        public Rectangle RelevantRectangle { get; private set; }

		#endregion Properties 

		#region Constructors (1) 

        private Shape( IList<byte> bytes, string name, Rectangle relevantRectangle )
        {
            RelevantRectangle = relevantRectangle;
            this.name = name;
            int jump = (int)bytes.Sub( 0, 3 ).ToUInt32();
            int secondHalf = bytes[4] + bytes[5] * 256;

            List<uint> offsets = new List<uint>();
            offsets.Add( 0 );
            uint addy = 0;
            int i =0 ;
            do
            {
                addy = bytes.Sub( 0x0C + 4 * i, 0x0C + 4 * i + 3 ).ToUInt32();
                i++;
                if( addy != 0 )
                {
                    offsets.Add( addy );
                }
            } while( addy != 0 );

            frames = new List<Frame>( offsets.Count );
            for( i = 0; i < offsets.Count; i++ )
            {
                frames.Add( new Frame( bytes.Sub( (int)(offsets[i] + 0x40A) ), i >= secondHalf ? 256 : 0 ) );
            }

            if( jump > 8 )
            {
                offsets = new List<uint>();
                offsets.Add( 0 );
                addy = 0;
                i = 0;
                do
                {
                    addy = bytes.Sub( jump + 4 * i + 4, jump + 4 * i + 3 + 4 ).ToUInt32();
                    i++;
                    if( addy != 0 )
                    {
                        offsets.Add( addy );
                    }
                } while( addy != 0 );
            }
            for( i = 0; i < offsets.Count; i++ )
            {
                frames.Add( new Frame( bytes.Sub( (int)(offsets[i] + jump + 0x402) ), i >= secondHalf ? 256 : 0 ) );
            }
        }

		#endregion Constructors 

		#region Methods (2) 


        public IList<Bitmap> GetFrames( AbstractSprite source )
        {
            List<Bitmap> result = new List<Bitmap>( frames.Count );
            foreach( Frame f in frames )
            {
                result.Add( f.GetFrame( source ) );
            }

            return result;
        }



        public override string ToString()
        {
            return Name;
        }


		#endregion Methods 

    }
}