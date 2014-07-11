using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public class ShortSprite : AbstractSprite
    {
        /*
         * 10M
         * 10W
         * 20M
         * 20W
         * 40M
         * 40W
         * 60M
         * 60W
         * CYOMON1
         * CYOMON2
         * CYOMON3
         * CYOMON4
         * FURAIA
         */

        public override int Height
        {
            get { return 288; }
        }

        internal ShortSprite( SerializedSprite sprite )
            : base( sprite )
        {
        }

        public ShortSprite( IList<byte> bytes )
            : base( bytes )
        {
        }

        protected override Rectangle PortraitRectangle
        {
            get { return new Rectangle( 80, 256, 48, 32 ); }
        }

        protected override void DrawSpriteInternal( int palette, int portraitPalette, SetPixel setPixel )
        {
            for ( int i = 0; i < Pixels.Count && ( i / Width ) < Height; i++ )
            {
                setPixel( i % Width, i / Width, Palettes[palette].Colors[Pixels[i] % 16] );
            }

            Rectangle pRect = PortraitRectangle;

            for ( int x = pRect.X; x < pRect.Right; x++ )
            {
                for ( int y = pRect.Y; y < pRect.Bottom && ( x + y * Width < Pixels.Count ); y++ )
                {
                    setPixel( x, y, Palettes[portraitPalette].Colors[Pixels[x + y * Width] % 16] );
                }
            }
        }

        protected override void ToBitmapInner( System.Drawing.Bitmap bmp, System.Drawing.Imaging.BitmapData bmd )
        {
            for( int i = 0; (i < Pixels.Count) && (i / Width < Height); i++ )
            {
                bmd.SetPixel8bpp( i % Width, i / Width, Pixels[i] );
            }
        }

        protected override void ImportSPRInner( IList<byte> bytes )
        {
            BuildPixels( bytes, null ).Sub( 0, Height * Width - 1 ).CopyTo( Pixels, 0 );
        }
        
        protected override IList<byte> BuildPixels( IList<byte> bytes, IList<byte>[] extraBytes )
        {
            int length = Width * Height;
            byte[] result = new byte[length];
            for( int i = 0; i < length/2; i++ )
            {
                result[i * 2] = bytes[i].GetLowerNibble();
                result[i * 2 + 1] = bytes[i].GetUpperNibble();
            }

            return result;
        }

        public override byte[] ToByteArray( int index )
        {
            System.Diagnostics.Debug.Assert( index == 0 );
            List<byte> ourResult = new List<byte>( 37377 );
            foreach ( Palette p in Palettes )
            {
                ourResult.AddRange( p.ToByteArray() );
            }

            for ( int i = 0; i < Pixels.Count / 2; i++ )
            {
                ourResult.Add( (byte)( ( Pixels[2 * i + 1] << 4 ) | ( Pixels[2 * i] & 0x0F ) ) );
            }

            if ( ourResult.Count < OriginalSize )
            {
                ourResult.AddRange( new byte[OriginalSize - ourResult.Count] );
            }
            return ourResult.ToArray();
        }
    }
}
