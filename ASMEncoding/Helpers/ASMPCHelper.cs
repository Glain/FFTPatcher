/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 9:57
 * 
 */
using System;

namespace ASMEncoding.Helpers
{
    public class ASMProcessPCResult
    {
        public uint PC { get; set; }
        public string ErrorMessage { get; set; }

        public ASMProcessPCResult(uint pc, string errorMessage)
        {
            PC = pc;
            ErrorMessage = errorMessage;
        }
    }

	public static class ASMPCHelper
	{
        public static ASMProcessPCResult ProcessPC(uint pc, string strPC, bool reportError = true, bool blankAsZero = false)
		{
            ASMProcessPCResult errorResult = new ASMProcessPCResult(0, reportError ? "Starting address \"" + strPC + "\" is invalid; defaulting address to 0.\r\n" : "");

			try
			{
                /*
				if (!string.IsNullOrEmpty(strPC))
				{
					if (strPC.StartsWith("0x"))
					{
						if (strPC.Length >= 10)
							pc = ASMValueHelper.HexToUnsigned(strPC.Substring(3,strPC.Length-3));	
						else
							pc = ASMValueHelper.HexToUnsigned(strPC.Substring(2,strPC.Length-2));	
					}
					else
						pc = Convert.ToUInt32(strPC);
				}
                */

                if (blankAsZero)
                {
                    ASMProcessPCResult zeroResult = new ASMProcessPCResult(0, "");
                    if (string.IsNullOrEmpty(strPC))
                        return zeroResult;
                    else if (string.IsNullOrEmpty(ASMStringHelper.RemoveSpaces(strPC)))
                        return zeroResult;
                }

                Nullable<uint> uValue = ASMValueHelper.FindUnsignedValueGeneric(strPC);
                return (uValue == null) ? errorResult : new ASMProcessPCResult(uValue.Value, "");   

                //pc = ASMValueHelper.FindUnsignedValueGeneric(strPC);
			}
			catch 
			{
                return errorResult;   
				//return 0;
				//txt_Messages.Text = "Starting address invalid.";
			}
			
			//return pc;
            //return new ASMProcessPCResult(pc, "");
		}

        public static ASMProcessPCResult ProcessOrg(uint pc, string[] parts, bool reportError = true)
		{
			if (!string.IsNullOrEmpty(parts[0]))
			{
				if (parts[0] == ".org")
				{
					string strArg = ASMStringHelper.RemoveSpaces(parts[1]);
					return ProcessPC(pc, strArg, reportError);
				}
			}
			
			//return pc;
            return new ASMProcessPCResult(pc, "");
		}

        public static uint ProcessStartPC(string asm, string pcText)
        {
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            return ProcessStartPC(asm, pc);
        }

        public static uint ProcessStartPC(string asm, uint pc)
        {
            // If the ASM starts with a .org statement, use that address as the PC
            string[] lines = asm.Split('\n');
            lines = ASMStringHelper.RemoveFromLines(lines, "\r");

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string processLine = ASMStringHelper.RemoveComment(line).ToLower();
                    string[] parts = ASMStringHelper.SplitLine(processLine);
                    ASMProcessPCResult processPCResult = ASMPCHelper.ProcessOrg(pc, parts);
                    pc = processPCResult.PC;
                    break;
                }
            }

            return pc;
        }
	}
}
