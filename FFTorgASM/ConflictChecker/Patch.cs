using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTorgASM
{
    public class Patch
    {
        public string name;
        public long[] offsets = new long[1];
        public long[] endaddresses = new long[1];
        public Conflict[] Conflicts = new Conflict[1];
        public bool Conflictexists = false;
        public string XMLFile;


        public Patch()
        {

        }

        public void AddOffset(long offset)
        {
            offsets[offsets.Length - 1] = offset;
            Array.Resize(ref offsets, offsets.Length + 1);
        }
        public void AddEndaddress(long endaddress)
        {
            endaddresses[endaddresses.Length - 1] = endaddress;
            Array.Resize(ref endaddresses, endaddresses.Length + 1);
        }
        public void AddConflict(long offsetin, int Patchnumberin, long patchoffsetin)
        {
            Conflicts[0] = new Conflict(offsetin, Patchnumberin, patchoffsetin);
        }

        public void ConvertConflicts(string[] patchnames, FileWrite BIN)
        {
            if(Conflicts[0] != null)
            { 
                foreach (Conflict conflict in Conflicts)
                {
                    string Conflictname = BIN.Patchname[conflict.Patchnumber];
                    int i = 1;
                    foreach (string name in patchnames)
                    {

                        if (name == Conflictname)
                        {
                            conflict.Patchnumber = i;
                        }
                        i++;
                    }
                }
            }
        }
    }
}
