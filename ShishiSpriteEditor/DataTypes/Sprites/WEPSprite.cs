using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FFTPatcher.SpriteEditor
{
    class WEPSprite : ShortSprite
    {
        public override int Height
        {
            get { return 256; }
        }

        protected override Rectangle PortraitRectangle
        {
            get { return Rectangle.Empty; }
        }

        public WEPSprite(IList<byte> bytes)
            : base(bytes)
        {
        }
        public override Shape Shape
        {
            get { return Shape.WEP1; }
        }
    }
}
