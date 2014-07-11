using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace FFTPatcher.SpriteEditor
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }


        EVTCHR evtchr;
        FullSpriteSet set;

        private void GetBitmap( Bitmap b, int segment, int palette )
        {
            EVTCHR.Segment seg = evtchr[segment];
            Palette p = seg.Palettes[palette];

            for ( int i = 0; i < seg.Pixels.Count; i++ )
            {
                b.SetPixel(i % seg.Width, i / seg.Width, p[seg.Pixels[i]]);
            }
        }

        private void button1_Click( object sender, EventArgs e )
        {
            evtchr = new EVTCHR(File.ReadAllBytes(@"Resources\EVENT\EVTCHR.BIN"));
            Bitmap b = new Bitmap(evtchr[0].Width, evtchr[0].Height);
            pictureBox1.Image = b;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            set = FullSpriteSet.FromPsxISO(@"N:\dev\fft\images\fft-usa.bin", bw);
            numericUpDown2_ValueChanged(numericUpDown1, EventArgs.Empty);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap b = pictureBox1.Image as Bitmap;
            pictureBox1.Image = null;
            GetBitmap(b, (int)numericUpDown2.Value, (int)numericUpDown1.Value);
            pictureBox1.Image = b;
            textBox3.Text = StringFromBytes(evtchr[(int)numericUpDown2.Value].Palettes[(int)numericUpDown1.Value].ToByteArray()).ToString();

            IList<byte> paletteBytes = evtchr[(int)numericUpDown2.Value].Palettes[(int)numericUpDown1.Value].ToByteArray();
            int foundSprite = -1;
            int foundPalette = -1;
            for (int i = 0; i < set.Sprites.Count && foundPalette == -1 && foundSprite == -1; i++)
            {
                for (int j = 0; j < set.Sprites[i].Palettes.Length && foundPalette == -1 && foundSprite == -1; j++)
                {
                    if (PatcherLib.Utilities.Utilities.CompareArrays(set.Sprites[i].Palettes[j].ToByteArray(), paletteBytes))
                    {
                        foundSprite = i;
                        foundPalette = j;
                    }
                }
            }
            if (foundSprite != -1 && foundPalette != -1)
            {
                AbstractSprite sprite = set.Sprites[foundSprite];
                textBox2.Text = string.Format("{0}: {1}", sprite.Name, foundPalette);
            }
            else { textBox2.Text = string.Empty; }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1_ValueChanged(sender, e);
            textBox1.Text = StringFromBytes(evtchr[(int)numericUpDown2.Value].Unknown0x0000).ToString();
            //textBox2.Text = StringFromBytes(evtchr[(int)numericUpDown2.Value].Unknown0x0880).ToString();
            //textBox1.Text = evtchr[(int)numericUpDown1.Value].Unknown0x0000.
        }

        private StringBuilder StringFromBytes(IList<byte> btyes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < btyes.Count/16; i++)
            {
                for (int j = i * 16; j < (i + 1) * 16 && j < btyes.Count; j++)
                {
                    sb.AppendFormat("{0:X02} ", btyes[j]);
                }
                sb.AppendLine();
            }
            return sb;
        }
    }

    public interface IDrawable
    {
        IList<byte> Pixels { get; }
        IList<Palette> Palettes { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface ICreateByteArray
    {
        IList<byte> ToByteArray();
    }

    public class EVTCHR : ICreateByteArray
    {
        private const int numSegments = 137;
        private Segment[] segments;
        public class Segment : IDrawable, ICreateByteArray
        {
            private const int width = 256;
            private const int height = 200;
            public const int NumBytes = 30720;
            private const int numPalettes = 20;
            public IList<byte> Unknown0x0000 { get; private set; }
            //public IList<byte> Unknown0x0880 { get; private set; }
            public IList<byte> Unknown0x6E00 { get; private set; }
            public IList<Palette> Palettes { get; private set; }
            public IList<byte> Pixels { get; private set; }

            public int Width { get { return width; } }

            public int Height { get { return height; } }

            public Segment(IList<byte> bytes)
            {
                System.Diagnostics.Debug.Assert(bytes.Count == NumBytes);
                Unknown0x0000 = new List<byte>(bytes.Sub(0, 1920 - 1)).AsReadOnly();
                //Unknown0x0880 = new List<byte>(bytes.Sub(0x880, 0x880 + 384 - 1)).AsReadOnly();
                Unknown0x6E00 = new List<byte>(bytes.Sub(0x6e00, 0x6e00 + 2560 - 1)).AsReadOnly();

                System.Diagnostics.Debug.Assert(Unknown0x6E00.TrueForAll(b => b == 0xFF) || Unknown0x6E00.TrueForAll(b => b == 0x00));

                Palettes = new Palette[numPalettes];
                for (int i = 0; i < numPalettes; i++)
                {
                    Palettes[i] = new Palette(bytes.Sub(1920 + i * 32, 1920 + (i + 1) * 32 - 1));
                }
                Pixels = new byte[width * height];
                for (int i = 0; i < (width * height / 2); i++)
                {
                    Pixels[i * 2] = bytes[i + 1920 + 256 + 384].GetLowerNibble();
                    Pixels[i * 2 + 1] = bytes[i + 1920 + 256 + 384].GetUpperNibble();
                }
            }

            public IList<byte> ToByteArray()
            {
                byte[] result = new byte[NumBytes];
                Unknown0x0000.Copy(0, result, 0, 1920);
                //Unknown0x0880.Copy(0, result, 0x880, 384);
                Unknown0x6E00.Copy(0, result, 0x6E00, 2560);
                for (int i = 0; i < numPalettes; i++)
                {
                    Palettes[i].ToByteArray().Copy(0, result, 1920 + i * 32, 32);
                }
                for (int i = 0; i < width * height; i += 2)
                {
                    result[1920 + 256 + 384 + i / 2] = PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles(Pixels[i + 1], Pixels[i]);
                }
                return result.AsReadOnly();
            }
        }
        public Segment this[int i] { get { return segments[i]; } }
        public EVTCHR(IList<byte> bytes)
        {
            System.Diagnostics.Debug.Assert(bytes.Count == numSegments * Segment.NumBytes);
            segments = new Segment[numSegments];
            for (int i = 0; i < numSegments; i++)
            {
                segments[i] = new Segment(bytes.Sub(i * Segment.NumBytes, (i + 1) * Segment.NumBytes - 1));
            }
        }

        public IList<byte> ToByteArray()
        {
            byte[] result = new byte[numSegments * Segment.NumBytes];
            for (int i = 0; i < numSegments; i++)
            {
                segments[i].ToByteArray().Copy(0, result, i * Segment.NumBytes, Segment.NumBytes);
            }
            return result.AsReadOnly();
        }
    }

}
