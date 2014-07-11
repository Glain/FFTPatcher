using System.Collections.Generic;

namespace FFTPatcher.SpriteEditor
{
    public class ARUTE : AbstractShapedSprite
    {
        public override Shape Shape
        {
            get { return Shape.ARUTE; }
        }

        internal ARUTE( SerializedSprite sprite )
            : base( sprite )
        {
        }

        public ARUTE( IList<byte> bytes )
            : base( bytes )
        {
        }
    }
}
