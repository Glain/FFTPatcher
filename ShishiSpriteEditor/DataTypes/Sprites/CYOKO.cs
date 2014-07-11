using System.Collections.Generic;

namespace FFTPatcher.SpriteEditor
{
    public class CYOKO : AbstractShapedSprite
    {
        public override Shape Shape
        {
            get { return Shape.CYOKO; }
        }

        internal CYOKO( SerializedSprite sprite )
            : base( sprite )
        {
        }

        public CYOKO( IList<byte> bytes )
            : base( bytes )
        {
        }
    }
}
