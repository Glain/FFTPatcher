using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    class WepSprite : Sprite
    {
        public enum Wep
        {
            WEP1,
            WEP2,
            WEP3
        }

        public Wep WEP { get; private set; }

        public WepSprite(PatcherLib.Datatypes.Context context, Wep wep, string name, PatcherLib.Iso.KnownPosition pos)
            : base(context, name, pos)
        {
            this.WEP = wep;
        }

        private AbstractSprite GetFromIso(System.IO.Stream iso, bool ignoreCache)
        {
            if (CachedSprite == null || ignoreCache)
            {
                IList<byte> bytes = Position.ReadIso(iso);

                System.Diagnostics.Debug.Assert(bytes.Count == this.Size);
                switch (WEP)
                {
                    case Wep.WEP1:
                    case Wep.WEP2:
                        CachedSprite = new WEPSprite(bytes);
                        break;
                    case Wep.WEP3:
                        CachedSprite = new WEP3Sprite(bytes);
                        break;
                    default:
                        CachedSprite = null;
                        break;
                }
            }

            return CachedSprite;
        }

        protected override AbstractSprite GetAbstractSpriteFromPspIso(System.IO.Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info, bool ignoreCache)
        {
            return GetFromIso(iso, ignoreCache);
        }

        protected override AbstractSprite GetAbstractSpriteFromPsxIso(System.IO.Stream iso, bool ignoreCache)
        {
            return GetFromIso(iso, ignoreCache);
        }
    }
}
