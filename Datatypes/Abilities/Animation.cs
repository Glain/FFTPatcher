using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;

namespace FFTPatcher.Datatypes
{
    [System.Diagnostics.DebuggerDisplay("{Byte1} {Byte2} {Byte3}")]
    public class Animation : IChangeable, IXmlDigest, ISupportDigest, ISupportDefault<Animation>
    {
        public Animation Default { get; private set; }

        public ushort Index { get; private set; }
        private byte[] bytes;
        public byte Byte1 { get { return bytes[0]; } set { bytes[0] = value; } }
        public byte Byte2 { get { return bytes[1]; } set { bytes[1] = value; } }
        public byte Byte3 { get { return bytes[2]; } set { bytes[2] = value; } }

        public bool Bool1 { get { return this[0]; } set { this[0] = value; } }
        public bool Bool2 { get { return this[1]; } set { this[1] = value; } }
        public bool Bool3 { get { return this[2]; } set { this[2] = value; } }
        public bool Bool4 { get { return this[3]; } set { this[3] = value; } }
        public bool Bool5 { get { return this[4]; } set { this[4] = value; } }
        public bool Bool6 { get { return this[5]; } set { this[5] = value; } }
        public bool Bool7 { get { return this[6]; } set { this[6] = value; } }
        public bool Bool8 { get { return this[7]; } set { this[7] = value; } }
        public bool Bool9 { get { return this[8]; } set { this[8] = value; } }
        public bool Bool10 { get { return this[9]; } set { this[9] = value; } }
        public bool Bool11 { get { return this[10]; } set { this[10] = value; } }
        public bool Bool12 { get { return this[11]; } set { this[11] = value; } }
        public bool Bool13 { get { return this[12]; } set { this[12] = value; } }
        public bool Bool14 { get { return this[13]; } set { this[13] = value; } }
        public bool Bool15 { get { return this[14]; } set { this[14] = value; } }
        public bool Bool16 { get { return this[15]; } set { this[15] = value; } }
        public bool Bool17 { get { return this[16]; } set { this[16] = value; } }
        public bool Bool18 { get { return this[17]; } set { this[17] = value; } }
        public bool Bool19 { get { return this[18]; } set { this[18] = value; } }
        public bool Bool20 { get { return this[19]; } set { this[19] = value; } }
        public bool Bool21 { get { return this[20]; } set { this[20] = value; } }
        public bool Bool22 { get { return this[21]; } set { this[21] = value; } }
        public bool Bool23 { get { return this[22]; } set { this[22] = value; } }
        public bool Bool24 { get { return this[23]; } set { this[23] = value; } }

        public string Name { get; private set; }

        public IList<byte> ToByteArray()
        {
            return new byte[3] { bytes[0], bytes[1], bytes[2] };
        }

        public Animation(IList<byte> bytes)
        {
            Name = string.Empty;
            this.bytes = new byte[3];
            this.bytes[0] = bytes[0];
            this.bytes[1] = bytes[1];
            this.bytes[2] = bytes[2];
        }

        public Animation(ushort index, string name, IList<byte> bytes, IList<byte> defaultBytes)
            : this(bytes)
        {
            Index = index;
            Name = name;
            Default = new Animation(defaultBytes);
        }

        public bool this[int i]
        {
            get
            {
                if (i < bytes.Length * 8)
                {
                    int index = i / 8;
                    int offset = i % 8;
                    return ((bytes[index] >> offset) & 1) == 1;
                }
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (i < bytes.Length * 8)
                {
                    int index = i / 8;
                    int offset = i % 8;
                    if (value)
                    {
                        bytes[index] |= (byte)(1 << offset);
                    }
                    else
                    {
                        bytes[index] &= (byte)(~(1 << offset));
                    }
                }
                else throw new IndexOutOfRangeException();
            }
        }

        public bool HasChanged
        {
            get { return Default != null && (Default.Byte1 != Byte1 || Default.Byte2 != Byte2 || Default.Byte3 != Byte3); }
        }


        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            throw new NotImplementedException();
        }


        public IList<string> DigestableProperties
        {
            get { return new string[3] { "Byte1", "Byte2", "Byte3" }; }
        }

        public static void CopyAll(Animation source, Animation destination)
        {
            destination.Byte1 = source.Byte1;
            destination.Byte2 = source.Byte2;
            destination.Byte3 = source.Byte3;
        }

        public void CopyAllTo(Animation destination)
        {
            CopyAll(this, destination);
        }
    }
}