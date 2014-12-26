using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class Register
    {
        public int Value = 0;
        public string Name = "";
        public string Description = "";

        public Register(int input)
        {
            Name = "r" + input;
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
