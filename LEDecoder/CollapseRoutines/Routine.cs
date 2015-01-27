using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder.CollapseRoutines
{
    public class Routine
    {
        public string[] Lines = new string[1];
        public long startaddress;
        public string TitleLine;

        public Routine(string RoutineLines)
        {
            Add(RoutineLines);
        }

        public void Add(string line)
        {
            string[] temp = new string[Lines.Length + 1];
            for(int i = 0;i < Lines.Length; i++)
            {
                temp[i] = Lines[i];
            }
            temp[temp.Length - 1] = line;
            Lines = temp;
        }

    }
}
