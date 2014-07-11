using System;
using System.Text;

namespace ImageMaster
{
    public class IsoRecord : ImageRecord
    {
        [Flags]
        private enum FileFlags
        {
            None = 0,
            Directory = 1 << 1
        }

        internal IsoRecordDateTime _DateTime = new IsoRecordDateTime();
        internal int VolSequenceNumber;
        internal long DataLength;
        internal byte Flags;
        internal byte FileUnitSize;
        internal byte InterleaveGapSize;
        internal byte ExtendedAttributeRecordLen;
        internal byte[] FileId;
        internal byte[] SystemUse;
        internal bool IsJoliet;

        public override DateTime DateTime
        {
            get
            {
                _dateTime = _DateTime.FileTime;
                return _dateTime;
            }
        }

        public override bool IsDirectory
        {
            get
            {
                FileFlags f = (FileFlags)Flags;
                _isDirectory = ((f & FileFlags.Directory) != 0);
                return _isDirectory;
            }
        }

        public override bool IsSystemItem
        {
            get
            {
                if (FileId.Length != 1)
                    return _isSystem;
                byte b = FileId[0];
                _isSystem = (b == 0 || b == 1);
                return _isSystem;
            }
        }

        public override long Size
        {
            get
            {
               return DataLength;
            }
        }

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = this.GetName();
                return _name;
            }
        }

        public override void Clear()
        {
            _DateTime = new IsoRecordDateTime();
            FileId = null;
            SystemUse = null;
            VolSequenceNumber = 0;
            Flags = 0;
            FileUnitSize = 0;
            InterleaveGapSize = 0;
            ExtendedAttributeRecordLen = 0;
            IsJoliet = false;
            base.Clear();
        }

        internal static bool CheckSusp(byte[] p, int startPos)
        {
            if (p[0] == 'S' &&
                p[1] == 'P' &&
                p[2] == 0x7 &&
                p[3] == 0x1 &&
                p[4] == 0xBE &&
                p[5] == 0xEF)
            {
                startPos = p[6];
                return true;
            }
            return false;
        }

        internal bool CheckSusp(int startPos)
        {
            byte[] p = SystemUse;
            int length = SystemUse.Length;
            const int kMinLen = 7;
            if (length < kMinLen)
                return false;
            if (CheckSusp(p, startPos))
                return true;
            const int kOffset2 = 14;
            if (length < kOffset2 + kMinLen)
                return false;
            int cnt = 0;
            byte[] p1 = new byte[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                if (i >= kOffset2)
                {
                    p1[cnt] = p[i];
                    cnt++;
                }
            }
            return CheckSusp(p1, startPos);
        }

        private string GetName()
        {
            StringBuilder sb = new StringBuilder();
            if (IsJoliet)
            {
                int curLen = (int)(this.FileId.Length / 2);
                for (int i = 0; i < curLen; i++)
                {
                    byte b0, b1;
                    b0 = this.FileId[i * 2];
                    b1 = this.FileId[i * 2 + 1];
                    sb.Append(((char)((b0 << 8) | b1)));
                }
            }
            else
            {
                for (int i = 0; i < this.FileId.Length; i++)
                {
                    char c = (char)this.FileId[i];
                    if (c == 0)
                        break;
                    sb.Append(c);
                }
            }

            return sb.ToString().Replace(";1", "");
        }
    }
}