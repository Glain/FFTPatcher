using System;
using System.Collections.Generic;
using System.Text;

namespace PatcherLib.Iso
{
    public abstract class KnownPosition
    {
        public abstract void PatchIso(System.IO.Stream iso, IList<byte> bytes);
        public abstract byte[] ReadIso(System.IO.Stream iso);
        public abstract PatcherLib.Datatypes.PatchedByteArray GetPatchedByteArray(byte[] bytes);

        public abstract KnownPosition AddOffset(int offset, int length);

        public abstract int Length { get; }

        public static KnownPosition ConstructKnownPosition( Enum sector, int startLoction, int length )
        {
            Type type = sector.GetType();
            if ( type == typeof( PsxIso.Sectors ) )
            {
                return new PsxIso.KnownPosition( (PsxIso.Sectors)sector, startLoction, length );
            }
            else if (type == typeof(PspIso.Sectors))
            {
                return new PspIso.KnownPosition( (PspIso.Sectors)sector, startLoction, length );
            }
            else if ( type == typeof( FFTPack.Files ) )
            {
                return new PspIso.KnownPosition( (FFTPack.Files)sector, startLoction, length );
            }
            else
            {
                throw new ArgumentException( "sector" );
            }
        }
    }
}
