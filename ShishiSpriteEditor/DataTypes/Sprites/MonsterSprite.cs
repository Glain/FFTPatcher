using System;
using System.Collections.Generic;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System.Drawing;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public class MonsterSprite : AbstractCompressedSprite
    {
        public override int Height
        {
            get
            {
                return 488 + sp2Count * 256;
            }
        }

        private int sp2Count;

        internal MonsterSprite( SerializedSprite sprite )
            : base( sprite )
        {
            sp2Count = ( sprite.Pixels.Count - ( 256 + 32 + 200 ) * 256 ) / 256 / 256;
        }

        public MonsterSprite( IList<byte> bytes, params IList<byte>[] sp2Files )
            : base( bytes, sp2Files )
        {
            sp2Count = sp2Files.Length;
        }

        protected override IList<byte> BuildPixels(IList<byte> bytes, params IList<byte>[] extraBytes)
        {
            List<byte> result = new List<byte>(36864 * 2);
            foreach (byte b in bytes.Sub(0, 36863))
            {
                result.Add(b.GetLowerNibble());
                result.Add(b.GetUpperNibble());
            }

            result.AddRange(Decompress(bytes.Sub(36864)));

            foreach (IList<byte> extra in extraBytes)
            {
                foreach (byte b in extra)
                {
                    result.Add(b.GetLowerNibble());
                    result.Add(b.GetUpperNibble());
                }
            }

            result.AddRange(new byte[Math.Max(0, 488 * 256 - result.Count)]);
            return result.ToArray();
        }

        public override byte[] ToByteArray( int index )
        {
            if ( index == 0 )
            {
                return base.ToByteArray( index );
            }
            else
            {
                return GetSp2ByteArray( index - 1 );
            }
        }

        private byte[] GetSp2ByteArray( int sp2Index )
        {
            IList<byte> sp2 = Pixels.Sub( 256 * 488 + sp2Index * 256 * 256, 256 * 488 + ( sp2Index + 1 ) * 256 * 256 - 1 );
            byte[] sp2Array = new byte[32768];
            for ( int j = 0; j < sp2.Count; j += 2 )
            {
                sp2Array[j / 2] = (byte)( ( sp2[j + 1] << 4 ) | ( sp2[j] & 0x0F ) );
            }
            return sp2Array;
        }

        protected override void ToBitmapInner( System.Drawing.Bitmap bmp, System.Drawing.Imaging.BitmapData bmd )
        {
            base.ToBitmapInner( bmp, bmd );
            for ( int i = 256 * 488; ( i < Pixels.Count ) && ( i / Width < Height ); i++ )
            {
                bmd.SetPixel8bpp( i % Width, i / Width, Pixels[i] );
            }
        }

        public override Bitmap To4bppBitmapUncached(int whichPalette)
        {
            Bitmap result = base.To4bppBitmapUncached(whichPalette);

            BitmapData bmd = result.LockBits(new Rectangle(Point.Empty, result.Size), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed);

            for (int i = 256 * 488; (i < Pixels.Count) && (i / Width < Height); i++)
            {
                bmd.SetPixel4bpp(i % Width, i / Width, Pixels[i]);
            }

            result.UnlockBits(bmd);

            return result;
        }

        public override Shape Shape
        {
            get { return Shape.MON; }
        }
    }
}
