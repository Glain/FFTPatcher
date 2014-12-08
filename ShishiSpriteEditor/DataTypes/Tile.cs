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
using System.Drawing;
using PatcherLib.Datatypes;
using System.Collections.ObjectModel;

namespace FFTPatcher.SpriteEditor
{
    [Serializable]
    public class Tile
    {

        private static IList<Size> sizes;

        public Point Location { get; private set; }

        public Rectangle Rectangle { get; private set; }

        public bool ReverseX { get; private set; }
        public bool ReverseY { get; private set; }

        static Tile()
        {
            sizes = new ReadOnlyCollection<Size>( new Size[16] {
                new Size(  8,  8 ),
                new Size( 16,  8 ),
                new Size( 16, 16 ),
                new Size( 16, 24 ),
                new Size( 24,  8 ),
                new Size( 24, 16 ),
                new Size( 24, 24 ),
                new Size( 32,  8 ),
                new Size( 32, 16 ),
                new Size( 32, 24 ),
                new Size( 32, 32 ),
                new Size( 32, 40 ),
                new Size( 48, 16 ),
                new Size( 40, 32 ),
                new Size( 48, 48 ),
                new Size( 56, 56 ) } );
        }

        internal Tile( IList<byte> bytes, int yOffset )
        {
            byte xByte = bytes[0];
            byte yByte = bytes[1];
            sbyte x = (sbyte)xByte;
            sbyte y = (sbyte)yByte ;

            ushort flags = (ushort)(bytes[2] + bytes[3] * 256);
            ReverseX = (flags & 0x4000) == 0x4000;
            ReverseY = (flags & 0x8000) == 0x8000;
            byte f = (byte)((flags >> 10) & 0x0F);
            int tileX = (flags & 0x1F) * 8;
            int tileY = ((flags >> 5) & 0x1F) * 8 + yOffset;

            Location = new Point(x + 53, y + 118);

            Rectangle = new Rectangle( new Point( tileX, tileY ), sizes[f] );
        }
    }
}
