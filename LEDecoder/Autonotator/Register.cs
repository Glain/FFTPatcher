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
        public MainAddress Mainaddress = null;
        public int SubsetOffset = 0;
        public SubData Subdata = null;


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

          public string GetDescription(long MainAddress, long Offset, MainForm mainform)
        {
            Offset += SubsetOffset;
            MainAddress = MainAddress & 0x7FFFFFFF;
            string result = "";
            long wholeaddress = MainAddress + Offset;
            foreach (MainAddress Main in mainform.MainAddresses)
            {
                if (Main.Address == MainAddress || Main.Address == wholeaddress)
                {
                    Mainaddress = Main;
                    if (Main.Frame != null)
                    {
                        if (Main.Frame.Length == Offset)
                        {
                            result = "this " + Main.Description;
                        }
                        else
                        {
                            result = GetSubDataDescription(MainAddress, Offset, mainform);
                        }
                    }
                    if (result == "")
                    {
                        result = Main.Description;
                    }
                }
            }
            return result;
        }

          public string GetSubDataDescription(long MainAddress, long Offset, MainForm mainform)
          {
              string result = "";
              foreach (MainAddress Main in mainform.MainAddresses)
              {
                  if (Main.Address == MainAddress)
                  {
                      foreach (SubData Data in Main.Frame)
                      {
                          if (Data != null)
                          {
                              if (Data.offsetaddress == Offset)
                              {
                                  result = Data.description;
                              }
                          }

                      }
                  }

              }
              return result;
          }

        //returns Subdata = null if not found
          public void AttachDatatoRegister(long address, long offset, MainForm mainform)
          {
              foreach (MainAddress Main in mainform.MainAddresses)
              {
                  if (Main.Address == address)
                  {
                      Mainaddress = Main;
                      if(Main.Frame.Length > offset)
                      {
                          if (Main.Frame[offset] != null)
                          {
                              Subdata = Main.Frame[offset];
                              Description = Main.Frame[offset].description;
                          }
                          else
                          {
                              Subdata = null;
                          }
                      }
                      else
                      {
                          Subdata = null;
                      }

                  }
              }
          }
          public void AttachAddresstoRegister(long address, MainForm mainform)
          {
             foreach (MainAddress Main in mainform.MainAddresses)
              {
                  if (Main.Address == address)
                  {
                      Mainaddress = Main;
                      Description = Main.Description;
                      Value = Main.Address;
                  }
              }
          }

          public void AttachDescriptiontoRegister(string description)
          {
              Description = description;
          }

          public void SeeifIsSubsetbyAddress(long SubsetAddress, MainForm mainform)
          {
              foreach (MainAddress Main in mainform.MainAddresses)
              {
                  if (Main.Frame != null)
                  {
                      if (SubsetAddress < Main.Address + Main.Frame.Length)
                      {
                          Mainaddress = Main;
                          Value = Main.Address;
                          long offset = SubsetAddress - Main.Address;

                          if (Main.Frame[offset].IsSubset)
                          {
                              SubsetOffset = (int)offset;
                              Description = Main.Frame[offset].SubsetDescription;
                              break;
                          }
                          else
                          {
                              SubsetOffset = 0;
                          }
                      }
                  }
              }

          }

    }

   
}
