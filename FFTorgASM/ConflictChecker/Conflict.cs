using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTorgASM
{
    public class Conflict
    {
        public long offset;
        public int Patchnumber;
        public long patchoffset;

        public Conflict(long offsetin, int Patchnumberin, long patchoffsetin)
        {
            offset = offsetin;
            Patchnumber = Patchnumberin;
            patchoffset = patchoffsetin;
        }

    }
}
