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

            //result.Add(new EncodeLine(parts, index));
            //return result.ToArray();

            bool reportErrors = !skipLabelAssertion;

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
            bool isHiLo = false;
            bool isLabel = false;
			
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
                        //parts[1] = args[0] + ",ra";
                        parts[1] = "ra," + args[0];
                    }
                    result.Add(new EncodeLine(parts, index));
                    break;

                case "mul":
                case "div":
                case "rem":
                case "mod":
                    if (((encoding.Command == "mul") && (encoding.Opcode == ASMFormatHelper.Opcodes.Special2))
                        || ((encoding.Command == "div") && (args.Length < 3))
                        || ((encoding.Command == "mod") && (args.Length < 3)))
                    {
                        result.Add(new EncodeLine(parts, index));
                        break;
                    }

                    newParts = new string[2];
                    newParts[0] = (encoding.Command == "mul") ? "mult" : "div";
                    newParts[1] = args[1] + "," + args[2];
                    result.Add(new EncodeLine(newParts, index));

                    parts[0] = ((encoding.Command == "mul") || (encoding.Command == "div")) ? "mflo" : "mfhi";
                    parts[1] = args[0];
                    result.Add(new EncodeLine(parts, index));

                    break;

                case "add":
                case "addu":
                case "and":
                case "min":
                case "max":
                case "nor":
                case "or":
                case "rotrv":
                case "sllv":
                case "slt":
                case "sltu":
                case "srav":
                case "srlv":
                case "sub":
                case "subu":
                case "xor":

                    if (args.Length == 2)
                    {
                        parts[1] = args[0] + "," + parts[1];
                    }

                    result.Add(new EncodeLine(parts, index));
                    break;


                case "addi":
                case "addiu":
                case "andi":
                case "ori":
                case "rotr":
                case "sll":
                case "slti":
                case "sltiu":
                case "sra":
                case "srl":
                case "xori":

                    if (args.Length == 2)
                    {
                        parts[1] = args[0] + "," + parts[1];
                    }

                    result.Add(new EncodeLine(parts, index));
                    break;

                case "bgt":
                case "blt":
                case "bge":
                case "ble":
                    argIndex0 = ((encoding.Command == "bgt") || (encoding.Command == "ble")) ? 1 : 0;
                    argIndex1 = (argIndex0 > 0) ? 0 : 1;
                    doesBranchIfEqual = ((encoding.Command == "bge") || (encoding.Command == "ble"));

                    if ((IsAssemblerTemporary(args[0])) || (IsAssemblerTemporary(args[1])))
                    {
                        if (reportErrors)
                            _errorTextBuilder.AppendLine("WARNING: Cannot expand " + encoding.Command + " instruction due to use of $at register at address 0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8));
                        result.Add(new EncodeLine(parts, index));
                    }
                    else
                    {
                        newParts = new string[2];
                        newParts[0] = "slt";
                        newParts[1] = "at," + args[argIndex0] + "," + args[argIndex1];
                        result.Add(new EncodeLine(newParts, index));

                        parts[0] = doesBranchIfEqual ? "beq" : "bne";
                        parts[1] = "at,zero," + args[2];
                        result.Add(new EncodeLine(parts, index));
                    }
					
					break;
					
				case "lbu":
				case "lb":
				case "lhu":
				case "lh":
				case "lw":
				case "sb":
				case "sh":
				case "sw":
                    isHiLo = ((args[1].ToLower().StartsWith("%hi")) || (args[1].ToLower().StartsWith("%lo")));

					startParenIndex = args[1].IndexOf('(', (isHiLo ? 4 : 0)); // check -1
                    endParenIndex = args[1].IndexOf(')', (startParenIndex >= 0) ? startParenIndex : 0);
                    isStore = encoding.Command.ToLower().StartsWith("s");
					useAT = ((startParenIndex >= 0) || (isStore));
                    //useAT = isStore;

                    bool canUseAT = useAT ? !IsAssemblerTemporary(args[0]) : false;

					if (startParenIndex >= 0)
					{
						regS = args[1].Substring(startParenIndex+1, endParenIndex-startParenIndex-1);
						strImmed = args[1].Substring(0,startParenIndex);
                        canUseAT = canUseAT && !IsAssemblerTemporary(regS);
					}
					else
						strImmed = args[1];
						
					ivalue = ValueHelper.GetAnyUnsignedValue(strImmed, skipLabelAssertion);

                    isLabel = !(
                            ((strImmed.StartsWith("0x")) || (strImmed.StartsWith("-0x")))
                        || (ASMStringHelper.StringIsNumeric(strImmed))
                        || ((strImmed.StartsWith("-")) && (strImmed.Length > 1))
                        || (isHiLo)
                    );

					if (((ivalue > 0x7fff) && (strImmed[0] != '-') && (!isHiLo)) || isLabel)
					{
                        if (useAT && !canUseAT)
                        {
                            if (reportErrors)
                                _errorTextBuilder.AppendLine("WARNING: Cannot expand " + encoding.Command + 
                                    " instruction due to use of $at register at address 0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8));
                            result.Add(new EncodeLine(parts, index));
                            break;
                        }

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
                            //newParts[1] = (useAT ? "at,at," : (args[0] + "," + args[0] + ",")) + regS;
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

                    isHiLo = ((args[1].ToLower().StartsWith("%hi")) || (args[1].ToLower().StartsWith("%lo")));
                    isLabel = !(
                            ((args[1].StartsWith("0x")) || (args[1].StartsWith("-0x")))
                        || (ASMStringHelper.StringIsNumeric(args[1]))
                        || ((args[1].StartsWith("-")) && (args[1].Length > 1))
                        || (isHiLo)
                    );
					
					if (((ivalue > 0xffff) && (ivalue < 0xffff8000)) || isLA || isLabel)
					{
						newParts = new string[2];
						newParts[0] = "lui";
						newParts[1] = args[0] + "," + (ivalue >> 16);
						result.Add(new EncodeLine(newParts,index));
						
						ushortval = (ushort)(ivalue & 0xffff);
						if ((ushortval > 0) || (isLA) || (isLabel))
						{
							parts[0] = "ori";
							parts[1] = args[0] + "," + args[0] + "," + ushortval;
							result.Add(new EncodeLine(parts,index));
						}
					}
                    else if (ivalue >= 0xffff8000)
					{
						parts[0] = "addiu";
						parts[1] = args[0] + ",zero," + ASMValueHelper.UnsignedShortToSignedShort((ushort)(ivalue & 0xffff));
						result.Add(new EncodeLine(parts,index));
					}
					else
					{
						//if (!((args[1].StartsWith("0x") || ASMStringHelper.StringIsNumeric(args[1]))))
						//	parts[1] = args[0] + "," + ValueHelper.LabelHelper.LabelToUnsigned(args[1], skipLabelAssertion);
						
						result.Add(new EncodeLine(parts,index));
					}
					
					break;

                case "cfc0":
                case "ctc0":
                case "mfc0":
                case "mtc0":
                    if (args.Length == 2)
                    {
                        parts[1] = parts[1] + ",0";
                    }

                    result.Add(new EncodeLine(parts, index));
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

            bool reportErrors = !skipLabelAssertion;

            ASMTranslatePseudoResult result = new ASMTranslatePseudoResult();
			List<EncodeLine> resultLines = new List<EncodeLine>();
			int index = 0;
			uint pc = startPC;

            List<bool> isSkippingLine = new List<bool>() { false };
            int ifNestLevel = 0;

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

                string firstPart = parts[0].ToLower().Trim();

                if (firstPart == ".endif")
                {
                    if (ifNestLevel == 0)
                    {
                        if (reportErrors)
                            _errorTextBuilder.AppendLine("WARNING: No matching .if statement for .endif statement at address 0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8));
                    }
                    else
                    {
                        isSkippingLine.RemoveAt(isSkippingLine.Count - 1);
                        ifNestLevel--;
                    }
                }
                else if (firstPart == ".else")
                {
                    if (ifNestLevel == 0)
                    {
                        if (reportErrors)
                            _errorTextBuilder.AppendLine("WARNING: No matching .if statement for .else statement at address 0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8));
                    }
                    else if (!isSkippingLine[ifNestLevel - 1])
                    {
                        isSkippingLine[ifNestLevel] = !isSkippingLine[ifNestLevel];
                    }
                }
                else if (firstPart == ".if")
                {
                    try
                    {
                        string[] innerParts = parts[1].Split(',');

                        if (innerParts.Length < 1)
                        {
                            if (reportErrors)
                                _errorTextBuilder.AppendLine("WARNING: Unreachable code at address 0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8) +
                                    " inside .if statement with no argument(s): \"" + parts[1] + "\"");

                            isSkippingLine.Add(true);
                            ifNestLevel++;
                        }
                        else if (isSkippingLine[ifNestLevel])
                        {
                            isSkippingLine.Add(true);
                            ifNestLevel++;
                        }
                        else
                        {
                            string operation = string.Empty;
                            string eqvKey, eqvValue;
                            bool forceIntCompare = false;

                            if (innerParts.Length >= 3)
                            {
                                operation = ASMStringHelper.RemoveSpaces(innerParts[0]);
                                eqvKey = ASMStringHelper.RemoveSpaces(innerParts[1]);
                                eqvValue = ASMStringHelper.RemoveSpaces(innerParts[2]);
                            }
                            else if (innerParts.Length == 2)
                            {
                                operation = "=";
                                eqvKey = ASMStringHelper.RemoveSpaces(innerParts[0]);
                                eqvValue = ASMStringHelper.RemoveSpaces(innerParts[1]);
                            }
                            else
                            {
                                operation = "!=";
                                eqvKey = ASMStringHelper.RemoveSpaces(innerParts[0]);
                                eqvValue = "0";
                                forceIntCompare = true;
                            }

                            int intKey = 0;
                            int intValue = 0;
                            bool isKeyInt = int.TryParse(eqvKey, out intKey);
                            bool isValueInt = int.TryParse(eqvValue, out intValue);
                            bool isIntCompare = forceIntCompare || (isKeyInt && isValueInt);

                            bool isPass = false;
                            switch (operation)
                            {
                                case "=":
                                case "==":
                                    isPass = isIntCompare ? (intKey == intValue) : eqvKey.Equals(eqvValue);
                                    break;
                                case "!=":
                                case "<>":
                                    isPass = isIntCompare ? (intKey != intValue) : !eqvKey.Equals(eqvValue);
                                    break;
                                case "<":
                                    isPass = isIntCompare && (intKey < intValue);
                                    break;
                                case ">":
                                    isPass = isIntCompare && (intKey > intValue);
                                    break;
                                case "<=":
                                    isPass = isIntCompare && (intKey <= intValue);
                                    break;
                                case ">=":
                                    isPass = isIntCompare && (intKey >= intValue);
                                    break;
                                default:
                                    break;
                            }

                            isSkippingLine.Add(!isPass);
                            ifNestLevel++;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (reportErrors)
                            _errorTextBuilder.AppendLine("Error on .if statement: " + ex.Message + "\r\n");
                    }
                }
                else
                {
                    // If this is an ASM command, pass off line to translating routine
                    EncodingFormat encodingOrNull = FormatHelper.FindFormatByCommand(parts[0]);
                    if (encodingOrNull != null)
                    {
                        try
                        {
                            if (!isSkippingLine[ifNestLevel])
                            {
                                EncodingFormat encoding = encodingOrNull;
                                EncodeLine[] encodeLines = TranslatePseudoSingle(encoding, parts, index, pc, skipLabelAssertion);
                                foreach (EncodeLine encodeLine in encodeLines)
                                {
                                    resultLines.Add(encodeLine);
                                    pc += 4;
                                }

                                //index++;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (reportErrors)
                            {
                                //result.errorMessage = "Error translating pseudoinstruction: " + line;
                                _errorTextBuilder.AppendLine("Error translating pseudoinstruction: " + line + " (" + ex.Message + ")");
                            }
                            //result.lines = null;
                            //return result;
                            //index++;
                        }

                        index++;
                    }
                }
			}

            result.lines = resultLines.ToArray();
            result.errorMessage = _errorTextBuilder.ToString();
			return result;
		}

        private bool IsAssemblerTemporary(string register)
        {
            if (!string.IsNullOrEmpty(register))
            {
                string registerLower = register.ToLower();
                if ((registerLower.Equals("$at")) || (registerLower.Equals("at")) || (registerLower.Equals("$1")) || (registerLower.Equals("r1")) || (registerLower.Equals("$r1")))
                    return true;
            }

            return false;
        }
	}
}
