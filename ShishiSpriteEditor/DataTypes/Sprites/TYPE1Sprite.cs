using System.Collections.Generic;

namespace FFTPatcher.SpriteEditor
{
    public class TYPE1Sprite : AbstractShapedSprite
    {
        public override Shape Shape
        {
            get { return Shape.TYPE1; }
        }

        internal TYPE1Sprite( SerializedSprite sprite )
            : base( sprite )
        {
        }

        public TYPE1Sprite( IList<byte> bytes )
            : base( bytes )
        {
        }
    }
}
