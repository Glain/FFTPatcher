using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class MainAddress
    {
        public long Value;
        public int OffsetMod = 0;
        public string Description = "";
        public int FrameSize;
        public int NumberofSections = 1;
        public SubData[] Frame;


        public MainAddress()
        {

        }
        public MainAddress(string value)
        {
            Value = StringToAddress(value);
        }

       public void AddFrame(long length)
        {
            Frame = new SubData[length];
        }

        public static long StringToAddress(string InString)
        {

            if (InString.Contains("0x"))
            {
                InString = InString.Replace("0x", "");
            }
            long intout = 0;
            //intout = HextoSignedLong(InString);
            byte[] byteOut = new byte[3];

            bool negative = false;
            if (InString.Contains("-"))
            {
                negative = true;
                InString = InString.Replace("-", "");
            }
            if (InString != "")
            {
                String strInString = InString;
                int l = strInString.Length;
                if (l >= 6)
                {
                    strInString = strInString.Substring(l - 6, 6);
                }
                int p = l;
                int c = 0;
                while ((p > 0) == true)
                {
                    if ((((l - p) % 2) == 0) & (p != l))
                    {
                        strInString = strInString.Insert(p, " ");
                        c++;
                    }
                    p--;
                }
                //while ((p < l) == true)
                //{
                //    if (((p % 2) == 0) & (p != 0))
                //    {
                //        strInString = strInString.Insert(p + c, " ");
                //        c++;
                //    }
                //    p++;
                //}

                string[] strInputSplit = strInString.Split(' ');

                if (strInputSplit.Length > 3)
                {
                    string[] newstringsplit = new string[3];
                    newstringsplit[0] = strInputSplit[0];
                    newstringsplit[1] = strInputSplit[1];
                    newstringsplit[2] = strInputSplit[2];
                    strInputSplit = newstringsplit;
                }
                //strInString.Split(' ');

                if (InString.Contains("-"))
                {
                    sbyte byteout1 = Convert.ToSByte(strInputSplit[0].ToString());
                }
                else
                {
                    byteOut[0] = Convert.ToByte(strInputSplit[0], 16);
                    if (strInputSplit.Length > 1)
                    {
                        byteOut[1] = Convert.ToByte(strInputSplit[1], 16);
                    }
                    if (strInputSplit.Length > 2)
                    {
                        byteOut[2] = Convert.ToByte(strInputSplit[2], 16);
                    }
                    int mod = 1;
                    for (int i = 1; i < strInputSplit.Length; i++)
                    {
                        mod *= 256;
                    }

                    intout += byteOut[0] * mod;
                    intout += byteOut[1] * mod / 256;
                    intout += byteOut[2] * mod / 65536;

                    if (negative)
                    {
                        intout = 0 - intout;
                    }
                }


            }
            else
            {
                byteOut[0] = 0;
            }

            return intout;
        }
      
    }
}
