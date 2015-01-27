using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class RegisterState
    {
        public Register[] SavedRegisters = new Register[34];
        public string destinationaddress = "";

        public RegisterState(Register[] Registers, string DestinationAddress)
        {
            for(int i = 0;i < Registers.Length;i++)
            {
                SavedRegisters[i] = Registers[i];
            }
            destinationaddress = DestinationAddress;
        }
        public void Set(MainForm mainform)
        {
            for(int i = 0;i < SavedRegisters.Length;i++)
            {
                mainform.Registers[i] = SavedRegisters[i];
            }
        }
    }
}
