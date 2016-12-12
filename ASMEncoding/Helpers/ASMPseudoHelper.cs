/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 10:36
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace ASMEncoding.Helpers
{
    public class ASMTranslatePseudoResult
    {
        public string errorMessage;
        public EncodeLine[] lines;
    }

	/// <summary>
	/// Description of ASMPseudoHelper.
	/// </summary>
	public class ASMPseudoHelper
	{
        private StringBuilder _errorTextBuilder;

		public ASMValueHelper ValueHelper { get; set; }
		public ASMFormatHelper FormatHelper { get; set; }
		
		public ASMPseudoHelper() { }
		public ASMPseudoHelper(ASMValueHelper valueHelper, ASMFormatHelper formatHelper)
		{
			ValueHelper = valueHelper;
            FormatHelper = formatHelper;
            _errorTextBuilder = new StringBuilder();
		}

        public ASMPseudoHelper(ASMPseudoHelper pseudoHelper)
        {
            ValueHelper = new ASMValueHelper(pseudoHelper.ValueHelper);
            FormatHelper = ValueHelper.LabelHelper.FormatHelper;
            _errorTextBuilder = new StringBuilder();
        }

		// Translates a single pseudoinstruction
		public EncodeLine[] TranslatePseudoSingle(EncodingFormat encoding, string[] parts, int index, uint pc, bool skipLabelAssertion = false)
		{
			List<EncodeLine> result = new List<EncodeLine>();
			
			int startParenIndex = 0;
			int endParenIndex = 0;
			string strImmed = "";
			string[] newParts;
			uint ivalue = 0;
			//long lvalue = 0;
			ushort ushortval = 0;
			//short shortval = 0;
			bool useNegativeOffset = false;
			string regS = "";
			int argIndex0 = 0;
			int argIndex1 = 0;
			bool doesBranchIfEqual = false;
			bool isStore = false;
			bool useAT = false;
			
			// Find args
			string strArgs = "";
			string[] args = null;
			if (!string.IsNullOrEmpty(parts[1]))
			{
				strArgs = ASMStringHelper.RemoveSpaces(parts[1]);
				args = strArgs.Split(',');
			}
			
			switch (encoding.Command)
			{
				case "jalr":
					if (args.Length == 1)
					{
						parts[1] = args[0] + ",ra";
					}
					result.Add(new EncodeLine(parts,index));
					break;
					
				case "mul":
				case "div":
				case "rem":
				case "mod":
					if ((encoding.Command == "div") && (args.Length < 3))
					{
						result.Add(new EncodeLine(parts,index));
						break;
					}
					
					newParts = new string[2];
					newParts[0] = (encoding.Command == "mul") ? "mult" : "div";
					newParts[1] = args[1] + "," + args[2];
					result.Add(new EncodeLine(newParts,index));
					
					parts[0] = ((encoding.Command == "mul") || (encoding.Command == "div")) ? "mflo" : "mfhi";
					parts[1] = args[0];
					result.Add(new EncodeLine(parts,index));
					
					break;
					
				case "bgt":
				case "blt":
				case "bge":
				case "ble":										
					argIndex0 = ((encoding.Command == "bgt") || (encoding.Command == "ble")) ? 1 : 0;
					argIndex1 = (argIndex0 > 0) ? 0 : 1;
					doesBranchIfEqual = ((encoding.Command == "bge") || (encoding.Command == "ble"));
					
					newParts = new string[2];
					newParts[0] = "slt";
					newParts[1] = "at," + args[argIndex0] + "," + args[argIndex1];
					result.Add(new EncodeLine(newParts,index));
					
					parts[0] = doesBranchIfEqual ? "beq" : "bne";
					parts[1] = "at,zero," + args[2];
					result.Add(new EncodeLine(parts,index));
					
					break;
					
				case "lbu":
				case "lb":
				case "lhu":
				case "lh":
				case "lw":
				case "sb":
				case "sh":
				case "sw":
					startParenIndex = args[1].IndexOf('('); // check -1
					endParenIndex = args[1].IndexOf(')');
                    isStore = encoding.Command.ToLower().StartsWith("s");
					useAT = ((startParenIndex >= 0) || (isStore));
					
					if (startParenIndex >= 0)
					{
						regS = args[1].Substring(startParenIndex+1, endParenIndex-startParenIndex-1);
						strImmed = args[1].Substring(0,startParenIndex);
					}
					else
						strImmed = args[1];
						
					ivalue = ValueHelper.GetAnyUnsignedValue(strImmed, skipLabelAssertion);

                    bool isLabel = !(
                            ((strImmed.StartsWith("0x")) || (strImmed.StartsWith("-0x")))
                        || (ASMStringHelper.StringIsNumeric(strImmed))
                        || ((strImmed.StartsWith("-")) && (strImmed.Length > 1))
                    );

					if (((ivalue > 0x7fff) && (strImmed[0] != '-')) || isLabel)
					{
						ushortval = (ushort)(ivalue & 0xffff);
						if (ushortval >= 0x8000)
						{
							useNegativeOffset = true;
							ushortval = (ushort)(0x10000 - ushortval);
						}
						
						newParts = new string[2];
						newParts[0] = "lui";
						newParts[1] = (useAT ? "at" : args[0]) + "," + ((ivalue >> 16) + (useNegativeOffset ? 1 : 0));
						result.Add(new EncodeLine(newParts,index));
						
						if (startParenIndex >= 0)
						{
							newParts = new string[2];
							newParts[0] = "addu";
							newParts[1] = "at,at," + regS;
							result.Add(new EncodeLine(newParts,index));
						}
						
						parts[1] = args[0] + "," + (useNegativeOffset ? "-" : "") + ushortval + "(" + (useAT ? "at" : args[0]) + ")";
						result.Add(new EncodeLine(parts,index));
					}
					else
						result.Add(new EncodeLine(parts,index));
					
					break;
				
                // "la" always translates to two instructions, while "li" can translate to one.
				case "la":
				case "li":
					//ivalue = ValueHelper.GetAnyUnsignedValue(args[1]);

                    /*
                    try
                    {
                        ivalue = ValueHelper.GetAnyUnsignedValue(args[1]);
                        //ivalue = ValueHelper.FindUnsignedValue(args[1]);
                    }
                    catch
                    {
                        ivalue = 0;
                    }
                    */

                    ivalue = ValueHelper.GetAnyUnsignedValue(args[1], skipLabelAssertion);

                    bool isLA = encoding.Command.Equals("la");
					
					if (ivalue >= 0xffff8000)
					{
						parts[0] = "addiu";
						parts[1] = args[0] + ",zero," + ((ushort)(ivalue & 0xffff));
						result.Add(new EncodeLine(parts,index));
					}
					else if ((ivalue > 0xffff) || isLA)
					{
						newParts = new string[2];
						newParts[0] = "lui";
						newParts[1] = args[0] + "," + (ivalue >> 16);
						result.Add(new EncodeLine(newParts,index));
						
						ushortval = (ushort)(ivalue & 0xffff);
						if ((ushortval > 0) || (isLA))
						{
							parts[0] = "ori";
							parts[1] = args[0] + "," + args[0] + "," + ushortval;
							result.Add(new EncodeLine(parts,index));
						}
					}
					else
					{
						if (!((args[1].StartsWith("0x") || ASMStringHelper.StringIsNumeric(args[1]))))
							parts[1] = args[0] + "," + ValueHelper.LabelHelper.LabelToUnsigned(args[1], skipLabelAssertion);
						
						result.Add(new EncodeLine(parts,index));
					}
					
					break;
					
				default:
					result.Add(new EncodeLine(parts,index));
					break;
			}
			
			return result.ToArray();
		}
		
		// Translates pseudoinstructions
		public ASMTranslatePseudoResult TranslatePseudo(string[] lines, uint startPC, bool skipLabelAssertion = false)
		{
            _errorTextBuilder.Length = 0;

            ASMTranslatePseudoResult result = new ASMTranslatePseudoResult();
			List<EncodeLine> resultLines = new List<EncodeLine>();
			int index = 0;
			uint pc = startPC;
			
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
					continue;
				
				string processLine = ASMStringHelper.RemoveLeadingBracketBlock(line);
				processLine = ASMStringHelper.RemoveLeadingSpaces(processLine);
				processLine = ASMStringHelper.RemoveComment(processLine).ToLower();
                processLine = ASMStringHelper.ConvertBracketBlocks(processLine);
				string[] parts = ASMStringHelper.SplitLine(processLine);
				parts = ASMStringHelper.RemoveLabel(parts);
				
                //pc = ASMPCHelper.ProcessOrg(pc, parts);
                ASMProcessPCResult processPCResult = ASMPCHelper.ProcessOrg(pc, parts, false);
                pc = processPCResult.PC;
                _errorTextBuilder.Append(processPCResult.ErrorMessage);
				
				// If this is an ASM command, pass off line to translating routine
                EncodingFormat encodingOrNull = FormatHelper.FindFormatByCommand(parts[0]);
				if (encodingOrNull != null)
				{
                    // DEBUG 1/16
					try
					{
						EncodingFormat encoding = encodingOrNull;
						EncodeLine[] encodeLines = TranslatePseudoSingle(encoding, parts, index, pc, skipLabelAssertion);
						foreach (EncodeLine encodeLine in encodeLines)
						{
							resultLines.Add(encodeLine);
							pc += 4;
						}
					}
					catch (Exception ex)
					{
                        //result.errorMessage = "Error translating pseudoinstruction: " + line;
                        _errorTextBuilder.Append("Error translating pseudoinstruction: " + line + " (" + ex.Message + ")\r\n");
						//result.lines = null;
                        //return result;
					}
					
					index++;
				}
			}

            result.lines = resultLines.ToArray();
            result.errorMessage = _errorTextBuilder.ToString();
			return result;
		}
	}
}
