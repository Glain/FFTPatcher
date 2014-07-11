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

namespace PatcherLib.Datatypes
{
    public enum FontColor
    {
        Transparent = 0,
        Black = 1,
        Dark = 2,
        Light = 3
    }

    /// <summary>
    /// <para>Represents an individual glyph in a <see cref="FFTFont"/>.</para>
    /// <para>Each glyph is 10 pixels wide and 14 pixels tall.</para>
    /// <para>Four pixels are packed into a single byte, for a total glyph size of 35 bytes.</para>
    /// </summary>
    public class Glyph : IChangeable
    {
		#region Public Properties (3) 

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get { return true; }
        }

        public FontColor[] Pixels { get; private set; }

        public byte Width { get; set; }

        public int Index { get; private set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public Glyph( int index, byte width, IList<byte> bytes )
        {
            Index = index;
            Width = width;
            Pixels = new FontColor[14 * 10];
            for( int i = 0; i < bytes.Count; i++ )
            {
                CopyByteToPixels( bytes[i], Pixels, i * 4 );
            }
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public byte[] ToByteArray()
        {
            byte[] result = new byte[35];
            for( int i = 0; i < 35; i++ )
            {
                result[i] = CopyPixelsToByte( Pixels, i * 4 );
            }

            return result;
        }

		#endregion Public Methods 

		#region Private Methods (2) 

        private void CopyByteToPixels( byte b, FontColor[] destination, int index )
        {
            destination[index] = (FontColor)((b & 0xC0) >> 6);
            destination[index + 1] = (FontColor)((b & 0x30) >> 4);
            destination[index + 2] = (FontColor)((b & 0x0C) >> 2);
            destination[index + 3] = (FontColor)(b & 0x03);
        }

        private byte CopyPixelsToByte( FontColor[] source, int index )
        {
            byte result = 0;
            result |= (byte)(((int)source[index]) << 6);
            result |= (byte)(((int)source[index+1]) << 4);
            result |= (byte)(((int)source[index+2]) << 2);
            result |= (byte)((int)source[index+3]);
            return result;
        }

		#endregion Private Methods 
    }
}
