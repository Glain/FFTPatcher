namespace ImageMaster
{
    public class IsoDateTime
    {
        public short Year;
        public byte Month;
        public byte Day;
        public byte Hour;
        public byte Minute;
        public byte Second;
        public byte Hundredths;
        /// <summary>
        /// min intervals from -48 (West) to +52 (East) recorded.
        /// </summary>
        public char GmtOffset;
        public bool NotSpecified
        {
            get { return Year == 0 && Month == 0 && Day == 0 && Hour == 0 && Minute == 0 && Second == 0 && GmtOffset == 0; }
        }
    }
}