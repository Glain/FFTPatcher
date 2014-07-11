using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ImageMaster
{
    public sealed class IsoReader : ImageReader
    {
        # region "Enumerations"

        [Flags]
        private enum VolumeDescriptorType
        {
            BootRecord = 0,
            PrimaryVolume = 1,
            SupplementaryVolume = 2,
            VolumeParttition = 3,
            Terminator = 255
        }

        [Flags]
        private enum BootEntryIdentifier
        {
            InitialEntryNotBootable = 0,
            ValidationEntry = 1,
            InitialEntryBootable = 0x88
        }

        #endregion
        public const string ElToritoSpecification = "EL TORITO SPECIFICATION\0\0\0\0\0\0\0\0\0";
        public static readonly char[] SignatureCD001 = { 'C', 'D', '0', '0', '1' };

        private int StartPosition = 0x8000;
        private byte[] dataBuffer;
        private List<VolumeDescriptor> _volumeDescriptor;
        private long _currentPosition;
        private int SuspSkipSize = 0;
        private int MainVolDescIndex;
        private int dataBufferPosition;
        private bool IsSusp;
        private bool _noJoliet;
        private int _version;

        private IsoReader()
        {
            _rootDirectory = new IsoRecord();
            _volumeDescriptor = new List<VolumeDescriptor>();
        }

        private IsoReader(string path)
            : this()
        {
            Initialize(path);
        }

        private IsoReader(Stream stream)
            : this()
        {
            if (stream == null)
                throw new ArgumentNullException();
            Initialize(stream);
        }

        public static ImageRecord GetRecord(Stream stream)
        {
            IsoReader reader = new IsoReader(stream);
            if (!reader.Open())
            {
                throw new IOException("Couldn't open ISO");
            }
            return reader._rootDirectory;
        }

        public static ImageRecord GetRecord(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                return GetRecord(stream);
            }
        }

        public bool IsJoliet
        {
            get { return (_volumeDescriptor[MainVolDescIndex].IsJoliet && !this.NoJoliet); }
        }

        public bool NoJoliet
        {
            get { return _noJoliet; }
            set
            {
                if (_rootDirectory.SubItems.Count == 0)
                    _noJoliet = value;
            }
        }

        public int Version
        {
            get { return _version; }
        }

        public override bool Open()
        {
            base.Open();

            StartPosition = (ImageReader.PRIMARY_VOLUME_SECTOR * CurrentBlockSize);
            dataBuffer = new byte[CurrentBlockSize];
            BaseStream.Seek(StartPosition, SeekOrigin.Begin);
            bool primVolDescDefined = false;
            dataBufferPosition = 0;
            _volumeDescriptor.Add(new VolumeDescriptor());
            bool exit = false;
            while (!exit)
            {
                byte[] sig = new byte[7];
                ReadBytes(sig, 7);
                byte ver = sig[6];
                if (!CheckSignature(1, SignatureCD001, sig))
                    return false;

                _version = ver;

                if (Version > 2) // version = 2 for ISO 9660:1999?
                    throw new InvalidDataException();

                switch ((VolumeDescriptorType)sig[0])
                {
                    case VolumeDescriptorType.BootRecord:
                        {
                            break;
                        }
                    case VolumeDescriptorType.PrimaryVolume:
                        {
                            if (primVolDescDefined)
                                return false;
                            primVolDescDefined = true;
                            VolumeDescriptor volDesc = _volumeDescriptor[0];
                            ReadVolumeDescriptor(volDesc);
                            for (int i = 0; i < volDesc.EscapeSequence.Length; i++)
                            {
                                // some burners write "Joliet" Escape Sequence to primary volume reset them to zero
                                volDesc.EscapeSequence[i] = 0;
                            }
                            break;
                        }
                    case VolumeDescriptorType.SupplementaryVolume:
                        {
                            VolumeDescriptor sd = new VolumeDescriptor();
                            ReadVolumeDescriptor(sd);
                            _volumeDescriptor.Add(sd);
                            break;
                        }
                    case VolumeDescriptorType.Terminator:
                    default:
                        exit = true;
                        break;
                }
            }

            if (!primVolDescDefined)
                return false;
            MainVolDescIndex = 0;
            if (!this.NoJoliet)
            {
                for (int i = _volumeDescriptor.Count - 1; i >= 0; i--)
                {
                    if (_volumeDescriptor[i].IsJoliet)
                    {
                        MainVolDescIndex = i;
                        break;
                    }
                }
            }
            if (_volumeDescriptor[MainVolDescIndex].LogicalBlockSize != ImageReader.SECTOR_SIZE)
                return false;
            _rootDirectory = _volumeDescriptor[MainVolDescIndex].RootDirRecord;
            IsoRecord root = _rootDirectory as IsoRecord;
            root.IsJoliet = IsJoliet;
            ReadDir(root, 0);
            _volumename = _volumeDescriptor[MainVolDescIndex].GetString();
            return true;
        }

        public static bool CheckSignature(int start, char[] signature, byte[] data)
        {
            for (int i = 0; i < signature.Length; i++)
                if (signature[i] != (char)data[start + i])
                    return false;
            return true;
        }

        private byte ReadByte()
        {
            if (dataBufferPosition >= CurrentBlockSize)
                dataBufferPosition = 0;
            if (dataBufferPosition == 0)
            {
                int processedSize = ReadStream(dataBuffer, CurrentBlockSize);
                if (processedSize != CurrentBlockSize)
                    throw new InvalidOperationException();
            }
            byte b = dataBuffer[dataBufferPosition++];
            _currentPosition++;
            return b;
        }

        private void ReadBytes(byte[] data, int size)
        {
            for (int i = 0; i < size; i++)
                data[i] = ReadByte();
        }

        private short ReadInt16()
        {
            short value = 0;
            for (int i = 0; i < 2; i++)
                value |= (short)((short)(ReadByte()) << (8 * i));
            return value;
        }

        private int ReadInt32()
        {
            byte[] b = new byte[4];
            ReadBytes(b, 4);
            int value = 0;
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    if (b[i] != b[3 - i])
                        throw new InvalidDataException();
                    value |= (int)((b[i]) << (8 * i));
                }
            }
            catch (InvalidDataException)
            {
                // A bug in some version 1 .iso's
                for (int i = 0; i < 4; i++)
                    value |= ((int)(b[i]) << (8 * i));
            }
            return value;
        }

        private int ReadInt32Le()
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
                value |= ((int)(ReadByte()) << (8 * i));
            return value;
        }

        private int ReadInt32Be()
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value <<= 8;
                value |= ReadByte();
            }
            return value;
        }

        private long ReadInt64()
        {
            byte[] b = new byte[8];
            ReadBytes(b, 8);
            long value = 0;
            for (int i = 0; i < 4; i++)
            {
                if (b[i] != b[7 - i])
                    throw new InvalidDataException();
                value |= ((long)(b[i]) << (8 * i));
            }
            return value;
        }

        private int ReadDigits(int numDigits)
        {
            int res = 0;
            for (int i = 0; i < numDigits; i++)
            {
                byte b = ReadByte();
                if (b < '0' || b > '9')
                {
                    if (b == 0) // it's bug in some CD's
                        b = (byte)'0';
                    else
                        throw new InvalidDataException();
                }
                int d = (int)(b - '0');
                res *= 10;
                res += d;
            }
            return res;
        }

        private void ReadDateTime(IsoDateTime time)
        {
            time.Year = (short)ReadDigits(4);
            time.Month = (byte)ReadDigits(2);
            time.Day = (byte)ReadDigits(2);
            time.Hour = (byte)ReadDigits(2);
            time.Minute = (byte)ReadDigits(2);
            time.Second = (byte)ReadDigits(2);
            time.Hundredths = (byte)ReadDigits(2);
            time.GmtOffset = (char)ReadByte();
        }

        private void ReadRecordingDateTime(IsoRecordDateTime time)
        {
            time.Year = ReadByte();
            time.Month = ReadByte();
            time.Day = ReadByte();
            time.Hour = ReadByte();
            time.Minute = ReadByte();
            time.Second = ReadByte();
            time.GmtOffset = (char)ReadByte();
        }

        private void ReadDirRecord(IsoRecord record, byte length)
        {
            record.ExtendedAttributeRecordLen = ReadByte();
            if (record.ExtendedAttributeRecordLen != 0)
                throw new InvalidDataException();
            record._location = ReadInt64();
            record.DataLength = ReadInt64();
            ReadRecordingDateTime(record._DateTime);
            record.Flags = ReadByte();
            record.FileUnitSize = ReadByte();
            record.InterleaveGapSize = ReadByte();
            record.VolSequenceNumber = ReadInt32();
            byte idLen = ReadByte();
            record.FileId = new byte[idLen];
            ReadBytes(record.FileId, idLen);
            int padSize = 1 - (idLen & 1);
            Skeep(1 - ((int)idLen & 1)); // it's bug in some cd's. Must be zeros
            int curPos = 33 + idLen + padSize;
            if (curPos > length)
                throw new InvalidDataException();
            int rem = length - curPos;
            record.SystemUse = new byte[rem];
            ReadBytes(record.SystemUse, rem);
        }

        private void ReadDirRecord(IsoRecord record)
        {
            byte len = ReadByte();
            // Some CDs can have incorrect value len = 48 ('0') in VolumeDescriptor.
            // But maybe we must use real "len" for other records.
            len = 34;
            ReadDirRecord(record, len);
        }

        private void ReadVolumeDescriptor(VolumeDescriptor descriptor)
        {
            descriptor.VolFlags = ReadByte();
            ReadBytes(descriptor.SystemIdentifier, descriptor.SystemIdentifier.Length);
            ReadBytes(descriptor.VolumeIdentifier, descriptor.VolumeIdentifier.Length);
            SkeepZeros(8);
            descriptor.VolumeSpaceSize = ReadInt64();
            ReadBytes(descriptor.EscapeSequence, descriptor.EscapeSequence.Length);
            descriptor.VolumeSetSize = ReadInt32();
            descriptor.VolumeSequenceNumber = ReadInt32();
            descriptor.LogicalBlockSize = ReadInt32();
            descriptor.PathTableSize = ReadInt64();
            descriptor.LPathTableLocation = ReadInt32Le();
            descriptor.LOptionalPathTableLocation = ReadInt32Le();
            descriptor.MPathTableLocation = ReadInt32Be();
            descriptor.MOptionalPathTableLocation = ReadInt32Be();
            ReadDirRecord(descriptor.RootDirRecord);
            ReadBytes(descriptor.VolumeSetIdentifier, descriptor.VolumeSetIdentifier.Length);
            ReadBytes(descriptor.PublisherIdentifier, descriptor.PublisherIdentifier.Length);
            ReadBytes(descriptor.DataPreparerIdentifier, descriptor.DataPreparerIdentifier.Length);
            ReadBytes(descriptor.ApplicationIdentifier, descriptor.ApplicationIdentifier.Length);
            ReadBytes(descriptor.CopyrightFileIdentifier, descriptor.CopyrightFileIdentifier.Length);
            ReadBytes(descriptor.AbstractFileIdentifier, descriptor.AbstractFileIdentifier.Length);
            ReadBytes(descriptor.BibFileIdentifier, descriptor.BibFileIdentifier.Length);
            ReadDateTime(descriptor.CTime);
            ReadDateTime(descriptor.MTime);
            ReadDateTime(descriptor.ExpirationTime);
            ReadDateTime(descriptor.EffectiveTime);
            descriptor.FileStructureVersion = ReadByte();
            SkeepZeros(1);
            ReadBytes(descriptor.ApplicationUse, descriptor.ApplicationUse.Length);
            SkeepZeros(653);
        }

        private void ReadDir(IsoRecord record, int level)
        {
            record.IsJoliet = IsJoliet;
            if (!record.IsDirectory)
                return;
            SeekToBlock(record.Location);
            long startPos = _currentPosition;
            bool firstItem = true;
            for (; ; )
            {
                long offset = _currentPosition - startPos;
                if (offset >= record.DataLength)
                    break;
                byte len = ReadByte();
                if (len == 0)
                    continue;
                IsoRecord subItem = new IsoRecord();
                subItem._Parent = record;
                ReadDirRecord(subItem, len);
                if (firstItem && level == 0)
                    IsSusp = subItem.CheckSusp(SuspSkipSize);
                if (!subItem.IsSystemItem)
                {
                    record.SubItems.Add(subItem);
                }
                firstItem = false;
            }
            for (int i = 0; i < record.SubItems.Count; i++)
                ReadDir(record.SubItems[i] as IsoRecord, level + 1);
        }

        private void SeekToBlock(long blockIndex)
        {
            long block = (blockIndex * _volumeDescriptor[MainVolDescIndex].LogicalBlockSize);
            BaseStream.Seek(block, SeekOrigin.Begin);
            _currentPosition = BaseStream.Position;
            if (_currentPosition != block)
                throw new InvalidOperationException();
            dataBufferPosition = 0;
        }

        private void Skeep(int size)
        {
            while (size-- != 0)
            {
                if (size > 0)
                    ReadByte();
            }
        }

        private void SkeepZeros(int size)
        {
            while (size-- != 0)
            {
                byte b = ReadByte();
                if (b != 0)
                    throw new InvalidDataException();
            }
        }
    }
}