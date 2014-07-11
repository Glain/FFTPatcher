using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public abstract class AbstractShapedSprite : AbstractCompressedSprite
    {
        public IList<Bitmap> GetFrames()
        {
            return Shape.GetFrames( this );
        }

        internal AbstractShapedSprite( SerializedSprite sprite )
            : base( sprite )
        {
        }

        public AbstractShapedSprite( IList<byte> bytes, params IList<byte>[] otherBytes )
            : base( bytes, otherBytes )
        {
        }

        public override void Import( Image file )
        {
            base.Import( file );
        }

        public override void ImportBitmap( Bitmap bmp, out bool foundBadPixels )
        {
            base.ImportBitmap( bmp, out foundBadPixels );
        }

        protected override void ImportSPRInner( IList<byte> bytes )
        {
            base.ImportSPRInner( bytes );
        }

    }
}
