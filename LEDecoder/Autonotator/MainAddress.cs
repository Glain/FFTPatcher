using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class MainAddress
    {
        public int Value;
        public string Description = "";
        public int FrameSize;
        public int NumberofSections = 1;
        public SubData[] Frame;


        public MainAddress()
        {

        }

    }
}
