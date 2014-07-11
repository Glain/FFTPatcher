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
    public class FFTFontWidths : IPatchableFile
    {
		#region Instance Variables (1) 

        private FFTFont owner;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public bool HasChanged
        {
            get { return true; }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public FFTFontWidths( FFTFont owner )
        {
            this.owner = owner;
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var width = owner.ToWidthsByteArray();

            if( context == Context.US_PSX )
            {
                result.Add( new PatchedByteArray( PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0xFF0FC, width ) );
            }
            else if( context == Context.US_PSP )
            {
                result.Add( new PatchedByteArray( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x293F40, width ) );
                result.Add( new PatchedByteArray( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x293F40, width ) );
            }

            return result;
        }

		#endregion Public Methods 
    }


    /// <summary>
    /// Represents a font used in FFT, which is an array of 2200 bitmaps.
    /// </summary>
    public class FFTFont : IPatchableFile
    {

        public const int CharacterWidth = 10;
        public const int CharacterHeight = 14;

        #region Public Properties (3) 

        public Glyph[] Glyphs { get; private set; }

        public FFTFontWidths GlyphWidths { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get { return true; }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public FFTFont( IList<byte> bytes, IList<byte> widthBytes )
        {
            GlyphWidths = new FFTFontWidths( this );
            Glyphs = new Glyph[2200];
            for( int i = 0; i < 2200; i++ )
            {
                Glyphs[i] = new Glyph( i, widthBytes[i], bytes.Sub( i * 35, (i + 1) * 35 - 1 ) );
            }

#if DEBUG
            using ( System.Drawing.Bitmap b = new System.Drawing.Bitmap( 550, 560 ) )
            {
                for ( int i = 0; i < 2200; i++ )
                {
                    DrawGlyphOnBitmap( b, Glyphs[i], new System.Drawing.Point( 10 * ( i % 55 ), 14 * ( i / 55 ) ) );
                }
                b.Save( "font.png", System.Drawing.Imaging.ImageFormat.Png );
            }
#endif            
        }

#if DEBUG
        System.Drawing.Color[] colors = new System.Drawing.Color[4] { 
                    //System.Drawing.Color.Transparent,
                    System.Drawing.Color.FromArgb( 166, 157, 133 ),
                    System.Drawing.Color.FromArgb(49, 41, 33),
                    System.Drawing.Color.FromArgb(82,82,66),
                    System.Drawing.Color.FromArgb(133,124,108) };

        private void DrawGlyphOnBitmap( System.Drawing.Bitmap b, Glyph g, System.Drawing.Point loc )
        {
            for ( int i = 0; i < g.Pixels.Length; i++ )
            {
                b.SetPixel( loc.X + i % 10, loc.Y + i / 10, colors[(int)g.Pixels[i]] );
            }
        }
#endif

        #endregion Constructors

        #region Public Methods (4)

        public IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 5 );

            var font = ToByteArray();
            var width = ToWidthsByteArray();

            if ( context == Context.US_PSX )
            {
                result.Add( new PatchedByteArray( PatcherLib.Iso.PsxIso.Sectors.EVENT_FONT_BIN, 0, font ) );
            }
            else if ( context == Context.US_PSP )
            {
                result.Add( new PatchedByteArray( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x27B80C, font ) );
                result.Add( new PatchedByteArray( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x27B80C, font ) );
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 35 * 2200 );
            foreach( Glyph g in Glyphs )
            {
                result.AddRange( g.ToByteArray() );
            }
            return result.ToArray();
        }

        public byte[] ToWidthsByteArray()
        {
            byte[] result = new byte[2200];
            for( int i = 0; i < 2200; i++ )
            {
                result[i] = Glyphs[i].Width;
            }

            return result;
        }

		#endregion Public Methods 
    }
}
