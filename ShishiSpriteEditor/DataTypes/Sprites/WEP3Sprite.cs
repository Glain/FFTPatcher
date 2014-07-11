using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    class WEP3Sprite : WEPSprite
    {
        public override int Height
        {
            get { return 144; }
        }

        public WEP3Sprite(IList<byte> bytes)
            : base(bytes)
        {
        }
    }
}
