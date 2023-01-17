/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 10:53
 * 
 */
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace ASMEncoding
{
    public class ASMDecoderResult
    {
        public string DecodedASM { get; set; }
        public string ErrorText { get; set; }

        public ASMDecoderResult(string decodedASM, string errorText)
        {
            DecodedASM = decodedASM;
            ErrorText = errorText;
        }
    }

    public class ASMFileDecoderResult
    {
        public const int Success = 0;
        public const int ASMDecodeError = -1;
        public const int FileOpenError = -2;
    }
}

namespace ASMEncoding.Helpers
{
	public class ASMDecoder
	{
		private StringBuilder _errorTextBuilder;
        private bool _illegalFlag;
        private string _illegalMessage;
		
		public ASMFormatHelper FormatHelper { get; set; }
		public ASMRegisterHelper RegHelper { get; set; }
		
		public ASMDecoder(ASMFormatHelper formatHelper, ASMRegisterHelper regHelper) 
		{ 
			FormatHelper = formatHelper;
			RegHelper = regHelper;
			
			_errorTextBuilder = new StringBuilder();
		}

        public ASMDecoder(ASMDecoder decoder)
        {
            FormatHelper = new ASMFormatHelper(decoder.FormatHelper);
            RegHelper = decoder.RegHelper;

            _errorTextBuilder = new StringBuilder();
        }

        public ASMDecoderResult DecodeASM(string hex, string pcText, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases)
        {
            ClearErrorText();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return DecodeASM(hex, pc, spacePadding, littleEndian, includeAddress, useRegAliases, false);
        }
        
		public ASMDecoderResult DecodeASM(string hex, uint pc, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases, bool clearErrorText)
		{
            if (clearErrorText)
                ClearErrorText();
			
			//string[] lines = hex.Split('\n');
			//lines = ASMStringHelper.RemoveFromLines(lines, "\r");

            string[] lines = ASMValueHelper.GetHexLines(hex);           
            StringBuilder sb = new StringBuilder();
			
			foreach (string line in lines)
			{				
				if (string.IsNullOrEmpty(line))
					continue;

                string strAddress = "[0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8) + "] ";
                string strPrefix = (includeAddress ? strAddress : "") + spacePadding;
                string modLine = line.Replace(" ", "").Replace("\t", "");

                try
                {
                    uint uLine = Convert.ToUInt32(modLine, 16);
                    if (littleEndian)
                    {
                        uLine = ASMValueHelper.ReverseBytes(uLine);
                    }

                    string decodedLine = TryDecodeSingle(uLine, pc, useRegAliases);
                    sb.Append(strPrefix);
                    sb.Append(decodedLine);
                    sb.Append("\r\n");
                    pc += 4;
                }
                catch (Exception ex)
                {
                    _errorTextBuilder.AppendLine("FAILED TO DECODE LINE: " + line + " (" + ex.Message + ")");
                }
			}
			
			return new ASMDecoderResult(sb.ToString(), _errorTextBuilder.ToString());
		}

        public ASMDecoderResult DecodeASM(IEnumerable<byte> bytes, uint pc, bool littleEndian = true, bool useRegAliases = false)
        {
            uint[] uintArray = ASMValueHelper.GetUintArrayFromBytes(bytes, littleEndian);
            return DecodeASM(uintArray, pc, useRegAliases);
        }

        public ASMDecoderResult DecodeASM(IEnumerable<uint> uLines, uint pc, bool useRegAliases = false)
        {
            return DecodeASM(uLines, pc, "", false, useRegAliases, true);
        }

        public ASMDecoderResult DecodeASM(IEnumerable<uint> uLines, uint pc, string spacePadding, bool includeAddress, bool useRegAliases, bool clearErrorText)
        {
            if (clearErrorText)
                ClearErrorText();

            StringBuilder sb = new StringBuilder();

            foreach (uint uLine in uLines)
            {
                string strAddress = "[0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8) + "] ";
                string strPrefix = (includeAddress ? strAddress : "") + spacePadding;

                try
                {
                    string decodedLine = TryDecodeSingle(uLine, pc, useRegAliases);
                    sb.Append(strPrefix);
                    sb.Append(decodedLine);
                    sb.Append("\r\n");
                    pc += 4;
                }
                catch (Exception ex)
                {
                    _errorTextBuilder.AppendLine("FAILED TO DECODE LINE: " + uLine.ToString("{0:X8}") + " (" + ex.Message + ")");
                }
            }

            return new ASMDecoderResult(sb.ToString(), _errorTextBuilder.ToString());
        }

        public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, string pcText)
        {
            //uint pc = ASMPCHelper.ProcessPC(0, pcText);
            ASMProcessPCResult processPCResult = ASMPCHelper.ProcessPC(0, pcText, false, true);
            uint pc = processPCResult.PC;
            _errorTextBuilder.Append(processPCResult.ErrorMessage);
            return DecodeASMToFile(inputFilename, outputFilename, littleEndian, useRegAliases, pc);
        }

		public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, uint pc)
		{
            ClearErrorText();

			BinaryReader reader = null;
			StreamWriter writer = null;
			
			try
			{
				reader = new BinaryReader(File.Open(inputFilename, FileMode.Open, FileAccess.Read, FileShare.Read));
				writer = new StreamWriter(outputFilename, false, Encoding.ASCII);
			}
			catch (Exception)
			{
				return ASMFileDecoderResult.FileOpenError;
			}
			
			long length = reader.BaseStream.Length;
			int result = ASMFileDecoderResult.Success;
			
			try   
	        {   
				int charPos = 0;
				while(charPos < length)
	            {
                    charPos += 4;

                    uint line = reader.ReadUInt32();
	            	if (!littleEndian)
	            	{
                        line = ASMValueHelper.ReverseBytes(line);
	            	}

                    string displayHex = ASMValueHelper.UnsignedToHex_WithLength(line, 8);
                    string instruction = DecodeASMSingle(line, pc, useRegAliases);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(ASMValueHelper.UnsignedToHex_WithLength(pc, 8).ToLower());
                    sb.Append(": ");
                    sb.Append(displayHex);
                    sb.Append(" ");
                    sb.Append(instruction);
	            	writer.WriteLine(sb.ToString());

	            	pc += 4;
	            } 
	        }      
			catch
	        { 
				result = ASMFileDecoderResult.ASMDecodeError;
	        }
	        finally
	        {
	            reader.Close();
	            writer.Close();
	        }
	       	
	        return result;
		}
		
		private string TryDecodeSingle(uint line, uint pc, bool useRegAliases)
		{			
			try
			{
				//StringBuilder sb = new StringBuilder();
				//sb.Append(strPrefix);
				//sb.Append(DecodeASMSingle(line, pc, useRegAliases));
				//sb.Append("\r\n");
				//return sb.ToString();
                return DecodeASMSingle(line, pc, useRegAliases);
			}
			catch (Exception ex)
			{
				_errorTextBuilder.Append("FAILED TO DECODE LINE : ");
				_errorTextBuilder.Append(ASMValueHelper.UnsignedToHex(line).ToUpper());
                    _errorTextBuilder.Append(" (");
                    _errorTextBuilder.Append(ex.Message);
                    _errorTextBuilder.Append(")");
				_errorTextBuilder.Append("\r\n");
			//	return spacePadding + "error";
                return "error";
			}
		}
		
		private string DecodeASMSingle(uint line, uint pc, bool useRegAliases)
		{
            _illegalFlag = false;
			
			// Find the binary line and format
			//string binaryLine = ASMValueHelper.HexToBinary_WithLength(line, 32);
            //uint uBinaryLine = ASMValueHelper.BinaryToUnsigned(binaryLine);
            EncodingFormat encFormat = FormatHelper.FindFormatByBinary(line);

			// If we couldn't find the command, it's some kind of mystery! Either not included in
			// the encoding file, or an illegal instruction.
            if (encFormat == null)
            {
                _errorTextBuilder.AppendLine("WARNING: Unknown command: " + ASMValueHelper.UnsignedToHex_WithLength(line, 8).ToUpper());
                return "unknown";
            }
			
            string newFormat = encFormat.ExpandedFormat;
            string syntax = encFormat.Syntax;
            string formatBinary = encFormat.Binary;
            string newSyntax = syntax;
			
            // Loop through syntax field and replace appropriate values
            int regIndex = 0;
            int immedIndex = 0;
            string argValue = "";
            //string binaryValue = "";
            //string prevBinaryValue = "";
            
            int numericValue = 0;
            int prevNumericValue = 0;

            int syntaxLength = syntax.Length;
            int newIndex = 0;
            for (int i = 0; i < syntaxLength; i++)
            {
                char c = syntax[i];

                // If this is a register or immediate, find value to replace in the syntax for the decoding
                if (char.IsLetter(c))
                {
                    int metaType = ASMFormatHelper.FindElementMetaType(c);
                    
                    if (metaType == ASMElementMetaType.Register)
                    {
                        char lookupChar = ASMStringHelper.CreateRegisterChar(regIndex);
                        //int vfpuRegMode = (c == ASMElementTypeCharacter.VFPURegister ? encFormat.VFPURegisterTypes[regIndex] : 0);

                        switch (c)
                        {
                            case ASMElementTypeCharacter.PartialVFPURegister:
                                argValue = DecodePartialVFPURegister(encFormat, line, regIndex, lookupChar);
                                break;
                            case ASMElementTypeCharacter.InvertedSingleBitVFPURegister:
                                numericValue = FindNumericArgValue(line, encFormat.RegisterPositions[regIndex], encFormat.RegisterIncludeMasks[regIndex]);
                                argValue = DecodeInvertedSingleBitVFPURegister(numericValue, encFormat.VFPURegisterTypes[regIndex]);
                                break;
                            case ASMElementTypeCharacter.VFPURegister:
                                numericValue = FindNumericArgValue(line, encFormat.RegisterPositions[regIndex], encFormat.RegisterIncludeMasks[regIndex]);
                                argValue = DecodeRegister(numericValue, c, useRegAliases, encFormat.VFPURegisterTypes[regIndex]);
                                break;
                            default:
                                //int binaryIndex = newFormat.IndexOf(lookupChar);
                                //binaryValue = binaryLine.Substring(binaryIndex, ASMRegisterHelper.GetEncodingBitLength(c));
                                numericValue = FindNumericArgValue(line, encFormat.RegisterPositions[regIndex], encFormat.RegisterIncludeMasks[regIndex]);
                                argValue = DecodeRegister(numericValue, c, useRegAliases, 0);
                                break;
                        }
                    
                        regIndex++;
                    }
                    else if (metaType == ASMElementMetaType.Immediate)
                    {
                        char lookupChar = ASMStringHelper.CreateImmediateChar(immedIndex);
                        int binaryIndex = newFormat.IndexOf(lookupChar);
                        //prevBinaryValue = binaryValue;
                        prevNumericValue = numericValue;
                        int immedLength = encFormat.ImmediateLengths[immedIndex];
                        int hexLength = (immedLength + 3) / 4;
                        //binaryValue = binaryLine.Substring(binaryIndex, immedLength);
                        //numericValue = FindNumericArgValue(line, encFormat.ImmediatePositions[immedIndex], ASMValueHelper.GetIncludeMask(encFormat.ImmediateLengths[immedIndex]));
                        numericValue = FindNumericArgValue(line, encFormat.ImmediatePositions[immedIndex], encFormat.ImmediateIncludeMasks[immedIndex]);

                        switch (c)
                        {
                            case ASMElementTypeCharacter.SignedImmediate:
                                argValue = DecodeSignedImmediate(numericValue, hexLength);
                                break;
                            case ASMElementTypeCharacter.UnsignedImmediate:
                                argValue = DecodeUnsignedImmediate(numericValue, hexLength);
                                break;
                            case ASMElementTypeCharacter.BranchImmediate:
                                argValue = DecodeBranchImmediate(numericValue, pc);
                                break;
                            case ASMElementTypeCharacter.JumpImmediate:
                                argValue = DecodeJumpImmediate(numericValue, pc);
                                break;
                            case ASMElementTypeCharacter.DecrementedImmediate:
                                argValue = DecodeDecrementedImmediate(numericValue, hexLength);
                                break;
                            case ASMElementTypeCharacter.ModifiedImmediate:
                                argValue = DecodeModifiedImmediate(numericValue, prevNumericValue, hexLength);
                                break;
                            case ASMElementTypeCharacter.ShiftedImmediate:
                                argValue = DecodeShiftedImmediate(numericValue, encFormat.ShiftedImmediateAmounts[immedIndex], hexLength);
                                break;
                            case ASMElementTypeCharacter.VFPUPrefixImmediate:
                                argValue = DecodeVFPUPrefixImmediate(numericValue, encFormat.VFPUPrefixType);
                                break;
                            default: break;
                        }

                        immedIndex++;
                    }

                    // Replace character in syntax with correct value
                    newSyntax = ASMStringHelper.ReplaceCharAtIndex(newSyntax, newIndex, argValue);
                    newIndex += argValue.Length - 1;
                }

                newIndex++;
            }

            string spacing = string.IsNullOrEmpty(newSyntax) ? "" : " ";
            string decoding = encFormat.Command + spacing + newSyntax;

            if (_illegalFlag)
            {
                decoding = "illegal";
                _errorTextBuilder.AppendLine(ASMStringHelper.Concat("Illegal instruction: ", line.ToString("x"), ": ", encFormat.Command.ToUpper(), " (", _illegalMessage, ")"));
            }

            return decoding;
		}

        private int FindNumericArgValue(uint line, int shiftAmount, int includeMask)
        {
            return (int)((line >> shiftAmount) & includeMask);
        }

        private string DecodePartialVFPURegister(EncodingFormat encFormat, uint line, int regIndex, char lookupChar)
        {
            string formatBinary = encFormat.Binary;
            string newFormat = encFormat.ExpandedFormat;

            /*
            int[] binaryIndex = new int[2];

            int part1Index = formatBinary.IndexOf("[p" + (regIndex + 1).ToString() + "-1");
            int part2Index = formatBinary.IndexOf("[p" + (regIndex + 1).ToString() + "-2");

            int firstIndex = newFormat.IndexOf(lookupChar);
            int searchPartIndex = firstIndex;
            for (; newFormat[searchPartIndex] == lookupChar; searchPartIndex++) ;
            int secondIndex = newFormat.IndexOf(lookupChar, searchPartIndex);

            if (part1Index > part2Index)
            {
                binaryIndex[0] = secondIndex;
                binaryIndex[1] = firstIndex;
            }
            else
            {
                binaryIndex[0] = firstIndex;
                binaryIndex[1] = secondIndex;
            }
            */

            //string firstValue = binaryLine.Substring(binaryIndex[0], encFormat.PartialRegisterSizes[0]);
            //string secondValue = binaryLine.Substring(binaryIndex[1], encFormat.PartialRegisterSizes[1]);

            //int firstValue = FindNumericArgValue(line, encFormat.PartialRegisterPositions[regIndex][0], ASMValueHelper.GetIncludeMask(encFormat.PartialRegisterSizes[0]));
            //int secondValue = FindNumericArgValue(line, encFormat.PartialRegisterPositions[regIndex][1], ASMValueHelper.GetIncludeMask(encFormat.PartialRegisterSizes[1]));

            int firstValue = FindNumericArgValue(line, encFormat.PartialRegisterPositions[regIndex][0], encFormat.PartialRegisterIncludeMasks[regIndex][0]);
            int secondValue = FindNumericArgValue(line, encFormat.PartialRegisterPositions[regIndex][1], encFormat.PartialRegisterIncludeMasks[regIndex][1]);

            //string binaryValue = firstValue + secondValue;
            int totalValue = (firstValue << encFormat.PartialRegisterSizes[1]) + secondValue;
            string argValue = DecodeRegister(totalValue, ASMElementTypeCharacter.PartialVFPURegister, false, encFormat.VFPURegisterTypes[regIndex]);

            return argValue;
        }

        private string DecodeInvertedSingleBitVFPURegister(int regValue, int vfpuRegisterMode)
        {
            /*
            char[] binaryChars = regBinary.ToCharArray();
            int invertIndex = ASMRegisterHelper.VFPUInvertSingleBitStringIndex;
            int digitOffset = ASMStringHelper.CharOffsets.Digit;
            binaryChars[invertIndex] = (char)(1 - (binaryChars[invertIndex] - digitOffset) + digitOffset);
            return new string(binaryChars);
            */

            return DecodeRegister(regValue ^ ASMRegisterHelper.VFPUInvertedSingleBitMask, ASMElementTypeCharacter.InvertedSingleBitVFPURegister, false, vfpuRegisterMode);
        }

        private string DecodeRegister(int regValue, char elementTypeChar, bool useRegAlias, int vpuRegisterMode)
        {
            if (elementTypeChar != ASMElementTypeCharacter.GPRegister)
                useRegAlias = false;

            //int regIndex = (int)ASMValueHelper.BinaryToUnsigned(regBinary);
            return RegHelper.GetRegisterName(regValue, useRegAlias, elementTypeChar, vpuRegisterMode);
        }

        private string DecodeBranchImmediate(int immedValue, uint pc)
		{
			//short difference = ASMValueHelper.BinaryToSignedShort(immedBinary);
            //short difference = (short)immedValue;
            //short difference = Convert.ToInt16((UInt16)immedValue); 

            ushort usValue = (ushort)immedValue;
            short difference = ASMValueHelper.UnsignedShortToSignedShort(usValue);
            //short difference = BitConverter.ToInt16(BitConverter.GetBytes(usValue), 0);

			uint immed = (uint)(difference * 4 + pc + 4);
            string strValue = ASMValueHelper.UnsignedToHex_WithLength(immed,8).ToLower();
            return "0x" + strValue; 
		}

        private string DecodeSignedImmediate(int immedValue, int length)
		{
			//short immed = ASMValueHelper.BinaryToSignedShort(immedBinary);
            //short immed = (short)immedValue;
            //short immed = Convert.ToInt16((UInt16)immedValue);

            //short immed = BitConverter.ToInt16(BitConverter.GetBytes((ushort)immedValue), 0);

            ushort usValue = (ushort)immedValue;
            short immed = ASMValueHelper.UnsignedShortToSignedShort(usValue);

            uint uImmed = (uint)((immed < 0) ? (immed * -1) : immed);
            return ((immed < 0) ? "-0x" : "0x") + ASMValueHelper.UnsignedToHex_WithLength(uImmed, length).ToLower();
		}

        private string DecodeUnsignedImmediate(int immedValue, int length)
        {
            uint uValue = (uint)immedValue;
            return "0x" + ASMValueHelper.UnsignedToHex_WithLength(uValue, length).ToLower();
        }

        private string DecodeJumpImmediate(int immedValue, uint pc)
		{
            /*
			string tempBinary = ASMValueHelper.UnsignedToBinary_WithLength(pc, 32).Substring(0,4);
			string fullBinary = tempBinary + immedBinary + "00";
			string strValue = ASMValueHelper.BinaryToHex_WithLength(fullBinary, 8).ToLower();
            return "0x" + strValue;
            */

            uint uValue = (uint)((pc & 0xF0000000) + (immedValue << 2));
            return "0x" + ASMValueHelper.UnsignedToHex_WithLength(uValue, 8).ToLower();
		}

        private string DecodeDecrementedImmediate(int immedValue, int length)
        {
            //uint value = ASMValueHelper.BinaryToUnsigned(immedBinary) + 1;
            uint value = (uint)immedValue + 1;
            return "0x" + ASMValueHelper.UnsignedToHex_WithLength(value, length);
        }

        // Immediate binary in the form (previous + current - 1) converted to current: (previous + current - 1) - previous + 1 = current
        private string DecodeModifiedImmediate(int immedValue, int prevImmedValue, int length)
        {
            //int result = (int)ASMValueHelper.BinaryToUnsigned(immedBinary) - (int)ASMValueHelper.BinaryToUnsigned(prevBinaryValue) + 1;
            int result = immedValue - prevImmedValue + 1;

            if (result < 0)
            {
                //_errorTextBuilder.Append("Negative modified immediate " + result.ToString() + " is invalid; Resulting hex is 0x" + hex);
                _illegalFlag = true;
                _illegalMessage = "Expected unsigned result for calculated value; negative result " + result.ToString() + " is invalid.";
                return "0";
            }

            uint uResult = (uint)result;
            string hex = ASMValueHelper.UnsignedToHex_WithLength(uResult, length);
            return "0x" + hex;
            //return "0x" + ASMValueHelper.UnsignedToHex_WithLength(result, 4);
        }

        private string DecodeShiftedImmediate(int immedValue, int shiftAmount, int length)
        {
            //return DecodeSignedImmediate(immedBinary.PadRight(immedBinary.Length + shiftAmount, '0'), length);
            return DecodeSignedImmediate(immedValue << shiftAmount, length);
        }

        private string DecodeVFPUPrefixImmediate(int immedValue, int prefixType)
        {
            //int iValue = (int)ASMValueHelper.BinaryToUnsigned(immedBinary);
            int iValue = immedValue;
            string result = "";

            if (prefixType == ASMVFPUPrefix.Type.Destination)
            {
                int[] sat = new int[4];
                sat[0] = iValue & 0x03;
                sat[1] = (iValue >> 2) & 0x03;
                sat[2] = (iValue >> 4) & 0x03;
                sat[3] = (iValue >> 6) & 0x03;

                int[] msk = new int[4];
                msk[0] = (iValue >> 8) & 0x01;
                msk[1] = (iValue >> 9) & 0x01;
                msk[2] = (iValue >> 10) & 0x01;
                msk[3] = (iValue >> 11) & 0x01;

                string[] values = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    if (msk[i] == 0)
                    {
                        switch (sat[i])
                        {
                            case 0: values[i] = ASMVFPUPrefix.Elements[i].ToString(); break;
                            case 1: values[i] = "0:1"; break;
                            case 3: values[i] = "-1:1"; break;
                            default: values[i] = ""; break;
                        }
                    }
                    else
                    {
                        values[i] = "m";
                    }
                }

                result = ASMStringHelper.Concat("[", values[0], ",", values[1], ",", values[2], ",", values[3], "]");
            }
            else if (prefixType == ASMVFPUPrefix.Type.Source)
            {
                int[] swz = new int[4];
                swz[0] = iValue & 0x03;
                swz[1] = (iValue >> 2) & 0x03;
                swz[2] = (iValue >> 4) & 0x03;
                swz[3] = (iValue >> 6) & 0x03;

                bool[] abs = new bool[4];
                abs[0] = (((iValue >> 8) & 0x01) > 0);
                abs[1] = (((iValue >> 9) & 0x01) > 0);
                abs[2] = (((iValue >> 10) & 0x01) > 0);
                abs[3] = (((iValue >> 11) & 0x01) > 0);

                bool[] cst = new bool[4];
                cst[0] = (((iValue >> 12) & 0x01) > 0);
                cst[1] = (((iValue >> 13) & 0x01) > 0);
                cst[2] = (((iValue >> 14) & 0x01) > 0);
                cst[3] = (((iValue >> 15) & 0x01) > 0);

                bool[] neg = new bool[4];
                neg[0] = (((iValue >> 16) & 0x01) > 0);
                neg[1] = (((iValue >> 17) & 0x01) > 0);
                neg[2] = (((iValue >> 18) & 0x01) > 0);
                neg[3] = (((iValue >> 19) & 0x01) > 0);

                string[] values = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    if (cst[i])
                    {
                        switch (swz[i])
                        {
                            case 0: values[i] = abs[i] ? "3" : "0"; break;
                            case 1: values[i] = abs[i] ? "1/3" : "1"; break;
                            case 2: values[i] = abs[i] ? "1/4" : "2"; break;
                            case 3: values[i] = abs[i] ? "1/6" : "1/2"; break;
                        }
                    }
                    else
                    {
                        string val = ASMVFPUPrefix.Elements[swz[i]].ToString();
                        values[i] = abs[i] ? ("|" + val + "|") : val;
                    }

                    if (neg[i])
                    {
                        values[i] = "-" + values[i];
                    }
                }

                result = ASMStringHelper.Concat("[", values[0], ",", values[1], ",", values[2], ",", values[3], "]");
            }

            return result;
        }

		private void ClearErrorText()
		{
			_errorTextBuilder.Length = 0;
		}
	}
}
