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

namespace FFTPatcher.SpriteEditor
{
    [Serializable]
    public class Frame
    {
        private static readonly Size defaultFrameSize = new Size(185,250);
        public static Size DefaultFrameSize { get { return new Size( defaultFrameSize.Width, defaultFrameSize.Height ); } }

		#region Fields (1) 

        private List<Tile> tiles;

		#endregion Fields 

		#region Properties (1) 


        /// <summary>
        /// Gets the tiles in this frame.
        /// </summary>
        public IList<Tile> Tiles { get { return tiles.AsReadOnly(); } }


		#endregion Properties 

		#region Constructors (1) 

        public Frame( IList<byte> bytes, int yOffset,string name )
        {
            int rotation = 0;
            int ydisplayoffset = 0;
            int numberOfTiles = bytes[0] + bytes[1] * 256;
            if (name == "WEP1" || name == "WEP2" ||name == "EFF1" || name == "EFF2" )
            {
                numberOfTiles = 0;
                rotation = bytes[0] + bytes[1] * 256;
                ydisplayoffset = bytes[0] / 4;

            }
            tiles = new List<Tile>( numberOfTiles + 1 );
            for( int i = 0; i <= numberOfTiles; i++ )
            {
                tiles.Add( new Tile( bytes.Sub( 2 + i * 4, 2 + i * 4 + 3 ), yOffset,rotation) );
                
            }

            tiles.Reverse();
        }

		#endregion Constructors 

		#region Methods (1) 


        /// <summary>
        /// Gets this frame from the specified sprite.
        /// </summary>
        public Bitmap GetFrame( AbstractSprite source, int paletteIndex = 0 )
        {
            Bitmap result = new Bitmap( defaultFrameSize.Width, defaultFrameSize.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

            if (source == null)
                return result;

            Bitmap sourceBmp = source.ToBitmap(paletteIndex, true);
            //sourceBmp = sourceBmp.RotateImg(90, Color.Transparent);
            result.Palette = sourceBmp.Palette;
            float rotation = 0;

            foreach ( Tile t in tiles )
            {
                rotation = t.Rotation;
                sourceBmp.CopyRectangleToPoint( t.Rectangle, result, t.Location, source.Palettes[0], t.ReverseX, t.ReverseY );
                
            }
           
            return result;
        }

      
		#endregion Methods 

    }
}
