using System;

namespace ImageMaster
{
    public class IsoRecordDateTime
    {
        public byte Year;
        public byte Month;
        public byte Day;
        public byte Hour;
        public byte Minute;
        public byte Second;
        /// <summary>
        /// min intervals from -48 (West) to +52 (East) recorded.
        /// </summary>
        public char GmtOffset;
        private DateTime _fileTime;

        public DateTime FileTime
        {
            get
            {
                if (_fileTime == new DateTime())
                {
                    DateTime dt = new DateTime(Year + 1900, Month, Day, Hour, Minute, Second);
                    long value = dt.ToFileTime();
                    value -= ((long)GmtOffset * 15 * 60);
                    _fileTime = DateTime.FromFileTime(value);
                }
                return _fileTime;
            }
        }

        public override string ToString()
        {
            return (FileTime.ToShortDateString() + " " + FileTime.ToShortTimeString());
        }
    }
}