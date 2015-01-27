using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class UnknownData
    {
        string Description;
        MainAddress MainData;
        int SubDataOffset;

        public UnknownData(string description, MainAddress mainaddress, int Offset)
        {
            Description = description;
            MainData = mainaddress;
            SubDataOffset = Offset;
        }
    }
}
