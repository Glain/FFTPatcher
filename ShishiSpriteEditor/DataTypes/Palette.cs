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
using System.Drawing;
using System.Drawing.Imaging;
using PatcherLib.Datatypes;

namespace FFTPatcher.SpriteEditor
{
    /// <summary>
    /// A palette for a FFT sprite.
    /// </summary>
    public class Palette
    {
        public static Color RealTransparent = Color.FromArgb(0, 0, 0, 0);

        public static byte[] PalletesToPALFile( IList<Palette> palettes )
        {
            List<byte> result = new List<byte>( 0x418 );
            result.AddRange( new byte[] { 
                0x52, 0x49, 0x46, 0x46, // RIFF
                0x10, 0x04, 0x00, 0x00, // Filesize or sommat
                0x50, 0x41, 0x4C, 0x20, 0x64, 0x61, 0x74, 0x61,  // PAL data
                0x04, 0x04, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01 } ); // filesize of sommat

            foreach ( Palette p in palettes )
            {
                result.AddRange( p.ToPALByteArray() );
            }

            return result.ToArray();
        }

		#region Properties (1) 

        public Color this[int index]
        {
            get { return Colors[index]; }
            set { Colors[index] = value; }
        }

        public Color[] Colors { get; private set; }


		#endregion Properties 

		#region Constructors (3) 

        public static Palette EmptyPalette
        {
            get
            {
                return new Palette( new Color[16] {
                    Color.Black, Color.Black, Color.Black, Color.Black, 
                    Color.Black, Color.Black, Color.Black, Color.Black, 
                    Color.Black, Color.Black, Color.Black, Color.Black, 
                    Color.Black, Color.Black, Color.Black, Color.Black } );
            }
        }

        private Palette()
        {
        }

        public static void FixupColorPalette( ColorPalette destination, IList<Palette> palettes )
        {
            for ( int i = 0; i < palettes.Count; i++ )
            {
                FixupColorPalette( destination, palettes, i, i * 16 );
            }
        }

        public static void FixupColorPalette( ColorPalette destination, IList<Palette> palettes, int whichPalette, int destStartIndex )
        {
            for ( int i = 0; i < palettes[whichPalette].Colors.Length; i++ )
            {
                if ( palettes[whichPalette][i].ToArgb() == Color.Transparent.ToArgb() )
                {
                    destination.Entries[destStartIndex + i] = Color.Black;
                }
                else
                {
                    destination.Entries[destStartIndex + i] = palettes[whichPalette][i];
                }
            }
        }
        public enum ColorDepth
        {
            _16bit = 2,
            _32bit = 4
        }


        public Palette( IList<byte> bytes, ColorDepth depth, bool useRealTransparentColor = false, bool applyTransparent = false )
        {
            switch (depth)
            {
                case ColorDepth._16bit:
                    Build16BitPalette(bytes, useRealTransparentColor, applyTransparent);
                    break;
                case ColorDepth._32bit:
                    Build32BitPalette( bytes );
                    break;
            }
        }

        private void Build32BitPalette( IList<byte> bytes )
        {
            int count = bytes.Count / 4;
            Colors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                Colors[i] = Color.FromArgb( 
                    bytes[i * 4 + 3], 
                    bytes[i * 4], 
                    bytes[i * 4 + 1], 
                    bytes[i * 4 + 2] );
            }
        }

        private void Build16BitPalette(IList<byte> bytes, bool useRealTransparentColor = false, bool applyTransparent = false)
        {
            int count = bytes.Count / 2;
            Colors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                Colors[i] = BytesToColor(bytes[i * 2], bytes[i * 2 + 1], applyTransparent);
            }

            if (Colors[0].ToArgb() == Color.Black.ToArgb())
            {
                Colors[0] = useRealTransparentColor ? RealTransparent : Color.Transparent;
            }
        }

        public Palette( IList<byte> bytes )
            : this( bytes, ColorDepth._16bit )
        {
        }

        public Palette( IList<Color> colors )
        {
            Colors = new Color[colors.Count];
            for (int i = 0; i < colors.Count; i++)
            {
                Colors[i] = Color.FromArgb( colors[i].R & 0xF8, colors[i].G & 0xF8, colors[i].B & 0xF8 );
            }
            if( Colors[0].ToArgb() == Color.Black.ToArgb() )
            {
                Colors[0] = Color.Transparent;
            }
        }

		#endregion Constructors 

		#region Methods (6) 


        /// <summary>
        /// Converts two bytes into a FFT color.
        /// Format is: [GGGRRRRR][ABBBBBGG]; Reversed byte order gives [ABBBBBGG][GGGRRRRR]
        /// </summary>
        public static Color BytesToColor(byte first, byte second, bool applyTransparent = false)
        {
            int b = (second & 0x7C) << 1;
            int g = (second & 0x03) << 6 | (first & 0xE0) >> 2;
            int r = (first & 0x1F) << 3;
            //int a = (second & 0x80) >> 7;
            int a = (((second & 0x80) >> 7) == 1) ? 0 : 255;

            if (applyTransparent)
                return Color.FromArgb(a, r, g, b);
            else
                return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Converts a color into two bytes suitable for using in a FFT palette.
        /// </summary>
        public static byte[] ColorToBytes( Color c )
        {
            byte r = (byte)((c.R & 0xF8) >> 3);
            byte g = (byte)((c.G & 0xF8) >> 3);
            byte b = (byte)((c.B & 0xF8) >> 3);

            byte[] result = new byte[2];
            result[0] = (byte)(((g & 0x07) << 5) | r);
            result[1] = (byte)((b << 2) | ((g & 0x18) >> 3));

            if ( c.A == 0 )
            {
                result[1] |= 0x80;
            }

            return result;
        }

        public static byte[] ColorToBytes(Color c, byte alphaByte, ColorDepth depth = ColorDepth._16bit)
        {
            if (depth == ColorDepth._16bit)
            {
                byte r = (byte)((c.R & 0xF8) >> 3);
                byte g = (byte)((c.G & 0xF8) >> 3);
                byte b = (byte)((c.B & 0xF8) >> 3);

                byte[] result = new byte[2];
                result[0] = (byte)(((g & 0x07) << 5) | r);
                result[1] = (byte)((b << 2) | ((g & 0x18) >> 3) | (alphaByte & 0x80));

                return result;
            }
            else if (depth == ColorDepth._32bit)
            {
                return ColorToBytes_32Bit(c, alphaByte);
            }
            else
            {
                throw new System.ArgumentException("Color depth must be either 16 or 32 bit.");
            }
        }

        public static byte[] ColorToBytes_32Bit(Color c, byte alphaByte)
        {
            return new byte[4] { c.R, c.G, c.B, alphaByte };
        }

        /// <summary>
        /// Creates a colection of palettes from a PAL file.
        /// </summary>
        public static Palette[] FromPALFile( IList<byte> bytes )
        {
            Palette[] result = new Palette[16];
            for( int i = 0; i < 16; i++ )
            {
                result[i] = Palette.FromPALFiledata( bytes.Sub( 24 + 4 * 16 * i, 24 + 4 * 16 * (i + 1) - 1 ) );
            }

            return result;
        }

        /// <summary>
        /// Creates a palette from a portion of a PAL file.
        /// </summary>
        public static Palette FromPALFiledata( IList<byte> bytes )
        {
            Palette result = new Palette();
            result.Colors = new Color[16];

            for( int i = 0; i < 16 * 4; i += 4 )
            {
                result.Colors[i / 4] = Color.FromArgb( bytes[i] & 0xF8, bytes[i + 1] & 0xF8, bytes[i + 2] & 0xF8 );
            }

            if( result.Colors[0].ToArgb() == 0 )
            {
                result.Colors[0] = Color.Transparent;
            }

            return result;
        }

        /// <summary>
        /// Converts this palette into an array of bytes suitable for including in a SPR file.
        /// </summary>
        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 16 * 2 );
            for( int index = 0; index < Colors.Length; index++ )
            {
                result.AddRange( ColorToBytes( Colors[index] ) );
            }

            //if( Colors[0] == Color.Transparent )
            if (((Colors[0].A > 0) && ((Colors[0].R | Colors[0].G | Colors[0].B) == 0)) || ( Colors[0] == Color.Transparent ))
            {
                result[0] = 0x00;
                result[1] = 0x00;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts this palette into an array of bytes suitable for including in a PAL file.
        /// </summary>
        public byte[] ToPALByteArray()
        {
            List<byte> result = new List<byte>( 0x40 );

            int start = 0;
            if( Colors[0].ToArgb() == Color.Transparent.ToArgb() )
            {
                result.AddRange( new byte[] { 0x00, 0x00, 0x00, 0x00 } );
                start = 1;
            }

            for( int i = start; i < Colors.Length; i++ )
            {
                result.Add( Colors[i].R );
                result.Add( Colors[i].G );
                result.Add( Colors[i].B );
                result.Add( 0x00 );
            }

            return result.ToArray();
        }


		#endregion Methods 

    }
}
