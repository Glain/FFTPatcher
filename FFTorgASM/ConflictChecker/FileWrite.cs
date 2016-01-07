using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using ExtensionMethods;

namespace FFTorgASM
{
    public class FileWrite
    {
        public int index = 0;
        public bool[] conflicts = new bool[1];
        public int[] conflictindex = new int[1];
        public int[,] conflictarray = new int[1,1];

        public string[] xmlfilename = new string[1];

        public long[] startaddress = new long[1];
        public long[] length = new long[1];
        public long[] endaddress = new long[1];

        public string[] Patchname = new string[1];
        public string[] ShortPatchnames = new string[1];
        public int[] PatchNumberList = new int[1];
                
        public void AddWrite(long address, long lengthin, string name, string filename)
        {
            xmlfilename[index] = filename;
            Patchname[index] = name;
            startaddress[index] = address;
            length[index] = lengthin;
            endaddress[index] = address + lengthin;

            index++;
            Array.Resize(ref xmlfilename, index + 1);
            Array.Resize(ref Patchname, index + 1);
            Array.Resize(ref startaddress, index + 1);
            Array.Resize(ref length, index + 1);
            Array.Resize(ref endaddress, index + 1);

        }

        public string[] FindAllPatchNames()
        {
            bool match = false;
            string[] names = new string[1];
            names[0] = "";
            int i = 0;
            foreach (string name in Patchname)
            {
                match = false;
                i = 0;
                foreach (string name2 in names)
                {
                    if (name == names[i])
                    {
                        match = true;
                    }

                    i++;
                }
                if (match == false)
                {
                    names[i-1] = name;
                    Array.Resize(ref names, names.Length + 1);
                    names[i] = "";
                }    

            }
            i = 0;
            foreach(string name in names)
            {
                if (name == null || name == "")
                {
                    names = names.Delete(i);
                }
                i++;
            }
            ShortPatchnames = names;
            return names;
        }



    }

}
