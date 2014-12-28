using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class Register
    {
        public long Value = 0;
        public string Name = "";
        public string Description = "";
        public string SpecialCommand = "";
        public bool originalvalue = false;
        public int multiplier = 0;
        public string Inputis = "";

        public Register(int input)
        {
            Name = "r" + input;
            Description = "r" + input + "Input";
            if(input == 0)
            {
                Description = "0";
            }
            if(input == 31)
            {
                Description = "Return Address";
            }
            if(input == 29)
            {
                Description = "Stack Pointer";
            }
            if (input == 32)
            {
                Description = "Lo";
            }
            if (input == 33)
            {
                Description = "Hi";
            }
          

        }

          public void GetDescription(MainForm mainform)
        {
              if(Value != 0)
              {
                  foreach (MainAddress address in mainform.MainAddresses)
                  {
                      if(address.Value == Value)
                      {
                          Description = address.Description;    //register description = Mainaddress description
                      }
                  }
              }
        }
      
    }

   
}
