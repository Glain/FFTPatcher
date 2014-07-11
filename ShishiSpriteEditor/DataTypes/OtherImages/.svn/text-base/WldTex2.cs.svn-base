using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor 
{
    public class WldTex2Reader
    {
        [System.Diagnostics.DebuggerDisplay( "Count: {Count}" )]
        private class GraphicStructureList
        {
            public UInt16 Count { get; private set; }
            public UInt16 Unknown { get; private set; }
            public IList<GraphicStructure> Structures { get; private set; }

            public GraphicStructureList( IList<byte> bytes
#if DEBUG
                , int startIndex
#endif
                )

            {
                Count = (UInt16)( ( bytes[1] << 8 ) + bytes[0] );
                Unknown = (UInt16)( ( bytes[3] << 8 ) + bytes[2] );
                GraphicStructure[] structs = new GraphicStructure[Count];
                int currentIndex = 4;
                for ( int i = 0; i < Count; i++ )
                {
                    int consumed = 0;
                    structs[i] = GraphicStructure.GetStructure( bytes.Sub( currentIndex ), out consumed );
#if DEBUG
                    Console.Out.WriteLine( "{0} from {1}-{2}", structs[i].Bounds, startIndex + currentIndex+8, startIndex + currentIndex + consumed );
#endif
                    currentIndex += consumed;
                }

                Structures = structs.AsReadOnly();
            }

        }

        private class GraphicsStructureImage
        {
            public Palette Palette { get; private set; }
            public IList<byte> Pixels { get; private set; }

            public GraphicsStructureImage( IList<GraphicStructureList> lists )
            {
                List<GraphicStructure> structs = new List<GraphicStructure>();
                lists.ForEach( l => structs.AddRange( l.Structures ) );
                PopulateProperties( structs[0], structs.Sub( 1 ) );
            }

            private void PopulateProperties( GraphicStructure paletteStructure, IList<GraphicStructure> pixelStructures )
            {
                Palette = new Palette( paletteStructure.Pixels, Palette.ColorDepth._16bit );
                List<byte> pix = new List<byte>();
                pixelStructures.ForEach( p => pix.AddRange( p.Pixels ) );
                Pixels = pix.AsReadOnly();
            }


            public GraphicsStructureImage( GraphicStructure paletteStructure, IList<GraphicStructure> pixelStructures )
            {
                PopulateProperties( paletteStructure, pixelStructures );
            }
        }

        [System.Diagnostics.DebuggerDisplay( "Bounds: {Bounds}" )]
        private class GraphicStructure
        {
            public System.Drawing.Rectangle Bounds { get; private set; }
            public IList<byte> Pixels { get; private set; }
            private GraphicStructure() { }

            public static GraphicStructure GetStructure( IList<byte> bytes, out int bytesConsumed )
            {
                int x = ( bytes[1] << 8 ) + bytes[0];
                int y = ( bytes[3] << 8 ) + bytes[2];
                int w = ( bytes[5] << 8 ) + bytes[4];
                int h = ( bytes[7] << 8 ) + bytes[6];

                int pixBytes = w * h * 2;
                byte[] pix = new byte[pixBytes];
                bytes.Sub( 8, 8 + pixBytes - 1 ).CopyTo( pix, 0 );

                bytesConsumed = pixBytes + 8;
                return new GraphicStructure { Bounds = new System.Drawing.Rectangle( x, y, w, h ), Pixels = pix.AsReadOnly() };
            }

        }

        // Stitch them together
        //            IList<byte> bytes = pos.ReadIso( iso );
        //            int listCount = bytes.Count / 0x800;
        //            GraphicStructureList[] lists = new GraphicStructureList[listCount];
        //            for ( int i = 0; i < listCount; i++ )
        //            {
        //#if DEBUG
        //                lists[i] = new GraphicStructureList( bytes.Sub( i * 0x800, ( i + 1 ) * 0x800 - 1 ), i*0x800 );
        //#else
        //                lists[i] = new GraphicStructureList( bytes.Sub( i * 0x800, ( i + 1 ) * 0x800 - 1 ) );
        //#endif
        //            }

    }
}
