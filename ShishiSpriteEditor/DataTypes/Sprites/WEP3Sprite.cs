using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FFTPatcher.SpriteEditor
{
    class WEP3Sprite : WEPSprite
    {
        public override int Height
        {
            get { return 144; }
        }
        protected override Rectangle PortraitRectangle
        {
            get { return Rectangle.Empty; }
        }
        public WEP3Sprite(IList<byte> bytes)
            : base(bytes)
        {
        }
        public override Shape Shape
        {
            get { return Shape.EFF1; }
        }
    }
}
