using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    public class PSXWorldMap : AbstractImage
    {
        public override string DescribeXml()
        {
            return string.Empty;
        }
        public PSXWorldMap()
            : base( "World Map", 496, 368 )
        {
        }
        static PSXWorldMap()
        {
            int[][,] posLengths = new int[][,] { topLeftPosLengths, topRightPosLengths, bottomLeftPosLengths, bottomRightPosLengths };

            IList<PatcherLib.Iso.PsxIso.KnownPosition>[] positions = new IList<PatcherLib.Iso.PsxIso.KnownPosition>[4];
            for(int i =0; i < 4; i++)
            {
                int length = posLengths[i].GetLength( 0 );
                PatcherLib.Iso.PsxIso.KnownPosition[] thisPositions = new PatcherLib.Iso.PsxIso.KnownPosition[length];
                for (int j = 0; j < length; j++)
                {
                    int pos = posLengths[i][j,0];
                    int len = posLengths[i][j,1];
                    thisPositions[j] = new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDTEX_TM2, pos, len );
                }
                positions[i] = thisPositions;
            }

            topLeft = positions[0];
            topRight = positions[1];
            bottomLeft = positions[2];
            bottomRight = positions[3];

            isoPositions = new IList<PatcherLib.Iso.PsxIso.KnownPosition>[] {
                topLeft,
                topRight,
                bottomLeft,
                bottomRight
            };
        }

        public static PSXWorldMap ConstructFromXml( System.Xml.XmlNode node )
        {
            return new PSXWorldMap();
        }

        private static IList<PatcherLib.Iso.PsxIso.KnownPosition> palettePositions = new PatcherLib.Iso.PsxIso.KnownPosition[] {
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDTEX_TM2, 12, 256*2),
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDTEX_TM2, 59404, 256*2),
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDTEX_TM2, 122892, 256*2),
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.WORLD_WLDTEX_TM2, 155660, 256*2),
        };

        private static IList<PatcherLib.Iso.PsxIso.KnownPosition> topLeft;
        private static IList<PatcherLib.Iso.PsxIso.KnownPosition> topRight;
        private static IList<PatcherLib.Iso.PsxIso.KnownPosition> bottomLeft;
        private static IList<PatcherLib.Iso.PsxIso.KnownPosition> bottomRight;

        private static int[,] topLeftPosLengths = new int[85,2] {
            { 532, 1440 },
            { 1980, 68 },
            { 2060, 172 },
            { 2240, 1680 },
            { 3928, 168 },
            { 4108, 72 },
            { 4188, 1920 },
            { 6116, 28 },
            { 6156, 212 },
            { 6376, 1680 },
            { 8064, 128 },
            { 8204, 112 },
            { 8324, 1680 },
            { 10012, 228 },
            { 10252, 12 },
            { 10272, 1920 },
            { 12200, 88 },
            { 12300, 152 },
            { 12460, 1680 },
            { 14148, 188 },
            { 14348, 52 },
            { 14408, 1920 },
            { 16336, 48 },
            { 16396, 192 },
            { 16596, 1680 },
            { 18284, 148 },
            { 18444, 92 },
            { 18544, 1920 },
            { 20472, 8 },
            { 20492, 232 },
            { 20732, 1680 },
            { 22420, 108 },
            { 22540, 132 },
            { 22680, 1680 },
            { 24368, 208 },
            { 24588, 32 },
            { 24628, 1920 },
            { 26556, 68 },
            { 26636, 172 },
            { 26816, 1680 },
            { 28504, 168 },
            { 28684, 72 },
            { 28764, 1920 },
            { 30692, 28 },
            { 30732, 212 },
            { 30952, 1680 },
            { 32640, 128 },
            { 32780, 112 },
            { 32900, 1680 },
            { 34588, 228 },
            { 34828, 12 },
            { 34848, 1920 },
            { 36776, 88 },
            { 36876, 152 },
            { 37036, 1680 },
            { 38724, 188 },
            { 38924, 52 },
            { 38984, 1920 },
            { 40912, 48 },
            { 40972, 192 },
            { 41172, 1680 },
            { 42860, 148 },
            { 43020, 92 },
            { 43120, 1920 },
            { 45048, 8 },
            { 45068, 232 },
            { 45308, 1680 },
            { 46996, 108 },
            { 47116, 132 },
            { 47256, 1680 },
            { 48944, 208 },
            { 49164, 32 },
            { 49204, 1920 },
            { 51132, 68 },
            { 51212, 172 },
            { 51392, 1680 },
            { 53080, 168 },
            { 53260, 72 },
            { 53340, 1920 },
            { 55268, 28 },
            { 55308, 212 },
            { 55528, 1680 },
            { 57216, 128 },
            { 57356, 112 },
            { 57476, 1440 } };

        private static int[,] topRightPosLengths = new int[91, 2] {
            { 59924, 1280 },
            { 61212, 228 },
            { 61452, 28 },
            { 61488, 1792 },
            { 63288, 200 },
            { 63500, 56 },
            { 63564, 1792 },
            { 65364, 172 },
            { 65548, 84 },
            { 65640, 1792 },
            { 67440, 144 },
            { 67596, 112 },
            { 67716, 1792 },
            { 69516, 116 },
            { 69644, 140 },
            { 69792, 1792 },
            { 71592, 88 },
            { 71692, 168 },
            { 71868, 1792 },
            { 73668, 60 },
            { 73740, 196 },
            { 73944, 1792 },
            { 75744, 32 },
            { 75788, 224 },
            { 76020, 1792 },
            { 77820, 4 },
            { 77836, 252 },
            { 78096, 1536 },
            { 79640, 232 },
            { 79884, 24 },
            { 79916, 1792 },
            { 81716, 204 },
            { 81932, 52 },
            { 81992, 1792 },
            { 83792, 176 },
            { 83980, 80 },
            { 84068, 1792 },
            { 85868, 148 },
            { 86028, 108 },
            { 86144, 1792 },
            { 87944, 120 },
            { 88076, 136 },
            { 88220, 1792 },
            { 90020, 92 },
            { 90124, 164 },
            { 90296, 1792 },
            { 92096, 64 },
            { 92172, 192 },
            { 92372, 1792 },
            { 94172, 36 },
            { 94220, 220 },
            { 94448, 1792 },
            { 96248, 8 },
            { 96268, 248 },
            { 96524, 1536 },
            { 98068, 236 },
            { 98316, 20 },
            { 98344, 1792 },
            { 100144, 208 },
            { 100364, 48 },
            { 100420, 1792 },
            { 102220, 180 },
            { 102412, 76 },
            { 102496, 1792 },
            { 104296, 152 },
            { 104460, 104 },
            { 104572, 1792 },
            { 106372, 124 },
            { 106508, 132 },
            { 106648, 1792 },
            { 108448, 96 },
            { 108556, 160 },
            { 108724, 1792 },
            { 110524, 68 },
            { 110604, 188 },
            { 110800, 1792 },
            { 112600, 40 },
            { 112652, 216 },
            { 112876, 1792 },
            { 114676, 12 },
            { 114700, 244 },
            { 114952, 1536 },
            { 116496, 240 },
            { 116748, 16 },
            { 116772, 1792 },
            { 118572, 212 },
            { 118796, 44 },
            { 118848, 1792 },
            { 120648, 184 },
            { 120844, 72 },
            { 120924, 1280 } };
        private static int[,] bottomLeftPosLengths = new int[46, 2] {
            { 123412, 1440 },
            { 124860, 68 },
            { 124940, 172 },
            { 125120, 1680 },
            { 126808, 168 },
            { 126988, 72 },
            { 127068, 1920 },
            { 128996, 28 },
            { 129036, 212 },
            { 129256, 1680 },
            { 130944, 128 },
            { 131084, 112 },
            { 131204, 1680 },
            { 132892, 228 },
            { 133132, 12 },
            { 133152, 1920 },
            { 135080, 88 },
            { 135180, 152 },
            { 135340, 1680 },
            { 137028, 188 },
            { 137228, 52 },
            { 137288, 1920 },
            { 139216, 48 },
            { 139276, 192 },
            { 139476, 1680 },
            { 141164, 148 },
            { 141324, 92 },
            { 141424, 1920 },
            { 143352, 8 },
            { 143372, 232 },
            { 143612, 1680 },
            { 145300, 108 },
            { 145420, 132 },
            { 145560, 1680 },
            { 147248, 208 },
            { 147468, 32 },
            { 147508, 1920 },
            { 149436, 68 },
            { 149516, 172 },
            { 149696, 1680 },
            { 151384, 168 },
            { 151564, 72 },
            { 151644, 1920 },
            { 153572, 28 },
            { 153612, 212 },
            { 153832, 720 } };
        private static int[,] bottomRightPosLengths = new int[49, 2] {
            { 156180, 1280 },
            { 157468, 228 },
            { 157708, 28 },
            { 157744, 1792 },
            { 159544, 200 },
            { 159756, 56 },
            { 159820, 1792 },
            { 161620, 172 },
            { 161804, 84 },
            { 161896, 1792 },
            { 163696, 144 },
            { 163852, 112 },
            { 163972, 1792 },
            { 165772, 116 },
            { 165900, 140 },
            { 166048, 1792 },
            { 167848, 88 },
            { 167948, 168 },
            { 168124, 1792 },
            { 169924, 60 },
            { 169996, 196 },
            { 170200, 1792 },
            { 172000, 32 },
            { 172044, 224 },
            { 172276, 1792 },
            { 174076, 4 },
            { 174092, 252 },
            { 174352, 1536 },
            { 175896, 232 },
            { 176140, 24 },
            { 176172, 1792 },
            { 177972, 204 },
            { 178188, 52 },
            { 178248, 1792 },
            { 180048, 176 },
            { 180236, 80 },
            { 180324, 1792 },
            { 182124, 148 },
            { 182284, 108 },
            { 182400, 1792 },
            { 184200, 120 },
            { 184332, 136 },
            { 184476, 1792 },
            { 186276, 92 },
            { 186380, 164 },
            { 186552, 1792 },
            { 188352, 64 },
            { 188428, 192 },
            { 188628, 768 } };


        private static void WritePixelsToBitmap( Palette palette, IList<byte> pixels, System.Drawing.Bitmap destination )
        {
            int width = destination.Width;
            int height = destination.Height;
            for ( int i = 0; i < pixels.Count; i++ )
            {
                destination.SetPixel( i % width, i / width, palette.Colors[pixels[i]] );
            }
        }

        private static void CopyBitmapToBitmap( System.Drawing.Bitmap source, System.Drawing.Bitmap destination, System.Drawing.Point destPoint )
        {
            int count = source.Width * source.Height;
            int width = source.Width;
            int destX = destPoint.X;
            int destY = destPoint.Y;

            for ( int i = 0; i < count; i++ )
            {
                destination.SetPixel( destX + i % width, destY + i / width, source.GetPixel( i % width, i / width ) );
            }
        }

        private static System.Drawing.Size[] sizes = new System.Drawing.Size[] { 
            new System.Drawing.Size(240,240),
            new System.Drawing.Size(256,240),
            new System.Drawing.Size(240,128),
            new System.Drawing.Size(256,128) };
        private static System.Drawing.Point[] positions = new System.Drawing.Point[] {
            new System.Drawing.Point(0,0),
            new System.Drawing.Point(240,0),
            new System.Drawing.Point(0,240),
            new System.Drawing.Point(240,240) };

        private static IList<PatcherLib.Iso.PsxIso.KnownPosition>[] isoPositions;

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            using ( System.Drawing.Bitmap sourceBitmap = new System.Drawing.Bitmap( image ) )
            {
                Set<System.Drawing.Color> colors = GetColors( sourceBitmap );
                if ( colors.Count > 256 )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( 255, 8 );
                    using ( var newBmp = q.Quantize( sourceBitmap ) )
                    {
                        WriteImageToIsoInner( iso, newBmp );
                    }
                }
                else
                {
                    for ( int i = 0; i < 4; i++ )
                    {
                        int width = sizes[i].Width;
                        int height = sizes[i].Height;
                        int xOffset = positions[i].X;
                        int yOffset = positions[i].Y;
                        List<byte> bytes = new List<byte>( width * height );
                        for ( int y = 0; y < height; y++ )
                        {
                            for ( int x = 0; x < width; x++ )
                            {
                                bytes.Add( (byte)colors.IndexOf( sourceBitmap.GetPixel( x + xOffset, y + yOffset ) ) );
                            }
                        }

                        int currentIndex = 0;
                        foreach ( var kp in isoPositions[i] )
                        {
                            kp.PatchIso( iso, bytes.Sub( currentIndex, currentIndex + kp.Length - 1 ) );
                            currentIndex += kp.Length;
                        }
                    }

                    var palBytes = GetPaletteBytes( colors, Palette.ColorDepth._16bit );
                    palettePositions.ForEach( pp => pp.PatchIso( iso, palBytes ) );
                }
            }
        }

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            Palette palette = new Palette( palettePositions[0].ReadIso( iso ), Palette.ColorDepth._16bit );

            System.Drawing.Bitmap result = new System.Drawing.Bitmap( 496, 368 );

            for ( int i = 0; i < 4; i++ )
            {
                using ( System.Drawing.Bitmap bmp = new System.Drawing.Bitmap( sizes[i].Width, sizes[i].Height ) )
                {
                    List<byte> bytes = new List<byte>();
                    isoPositions[i].ForEach( kp => bytes.AddRange( kp.ReadIso( iso ) ) );
                    WritePixelsToBitmap( palette, bytes, bmp );
                    CopyBitmapToBitmap( bmp, result, positions[i] );
#if DEBUG
                    bmp.Save( string.Format( "worldmap{0}.png", i ), System.Drawing.Imaging.ImageFormat.Png );
#endif
                }
            }

            return result;
        }

        public override string FilenameFilter
        {
            get
            {
                return "GIF image (*.gif)|*.gif";
            }
        }

        protected override string SaveFileName
        {
            get { return "WorldMap.png"; }
        }

    }
}
