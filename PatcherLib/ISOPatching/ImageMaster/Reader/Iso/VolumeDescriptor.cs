using System.Text;

namespace ImageMaster
{
    public class VolumeDescriptor
    {
        public byte VolFlags;
        public byte FileStructureVersion;
        public byte[] EscapeSequence = new byte[32];
        public byte[] VolumeSetIdentifier = new byte[128];
        public byte[] PublisherIdentifier = new byte[128];
        public byte[] DataPreparerIdentifier = new byte[128];
        public byte[] ApplicationIdentifier = new byte[128];
        public byte[] CopyrightFileIdentifier = new byte[37];
        public byte[] AbstractFileIdentifier = new byte[37];
        public byte[] BibFileIdentifier = new byte[37];
        public byte[] ApplicationUse = new byte[512];
        /// <summary>
        /// d-characters. An identification of the volume.
        /// </summary>
        public byte[] VolumeIdentifier = new byte[32];
        /// <summary>
        ///  a-characters. An identification of a system
        /// which can recognize and act upon the content of the Logical
        /// Sectors with logical Sector Numbers 0 to 15 of the volume.
        /// </summary>
        public byte[] SystemIdentifier = new byte[32];
        public int VolumeSetSize;
        /// <summary>
        /// the ordinal number of the volume in the Volume Set of which the volume is a member.
        /// </summary>
        public int VolumeSequenceNumber;
        public int LogicalBlockSize;
        public long PathTableSize;
        public int LPathTableLocation;
        public int LOptionalPathTableLocation;
        public int MPathTableLocation;
        public int MOptionalPathTableLocation;
        /// <summary>
        /// the number of Logical Blocks in which the Volume Space of the volume is recorded
        /// </summary>
        public long VolumeSpaceSize;
        public IsoRecord RootDirRecord = new IsoRecord();
        public IsoDateTime CTime = new IsoDateTime();
        public IsoDateTime MTime = new IsoDateTime();
        public IsoDateTime ExpirationTime = new IsoDateTime();
        public IsoDateTime EffectiveTime = new IsoDateTime();

        public bool IsJoliet
        {
            get
            {
                if ((VolFlags & 1) != 0)
                    return false;
                byte b = EscapeSequence[2];
                return (EscapeSequence[0] == 0x25 && EscapeSequence[1] == 0x2F && (b == 0x40 || b == 0x43 || b == 0x45));
            }
        }

        public string GetString()
        {
            string retval = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < VolumeIdentifier.Length; i++)
            {
                char c = (char)VolumeIdentifier[i];
                if (c == 0)
                    break;
                sb.Append(c);
            }

            retval = sb.ToString().TrimEnd();
            if (retval.Length < 2)
            {
                sb = new StringBuilder();
                for (int i = 1; i + 2 <= VolumeIdentifier.Length; i += 2)
                {
                    char c = (char)((VolumeIdentifier[i + 1] << 8) | VolumeIdentifier[i]);
                    sb.Append(c);
                }
                retval = sb.ToString().TrimEnd();
            }
            return retval;
        }
    }
}