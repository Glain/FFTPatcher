/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 11:33
 * 
 */
using System;
using System.Text;
using System.Collections.Generic;

namespace ASMEncoding
{
    public class ASMEncoderResult
    {
        public string EncodedASMText { get; set; }
        public byte[] EncodedBytes { get; set; }
        public string ModifiedText { get; set; }
        public string ErrorText { get; set; }

        public ASMEncoderResult(string encodedASMText, byte[] encodedBytes, string modifiedText, string errorText)
        {
            EncodedASMText = encodedASMText;
            EncodedBytes = encodedBytes;
            ModifiedText = modifiedText;
            ErrorText = errorText;
        }
    }
}

namespace ASMEncoding.Helpers
{
	public class ASMSingleEncodeResult
	{
		public string ASMText { get; set; }
		public byte[] Bytes { get; set; }
		
		public ASMSingleEncodeResult(string asmText, byte[] bytes)
		{
			ASMText = asmText;
			Bytes = bytes;
		}
	}

	public class ASMEncoder
	{		
		private StringBuilder _errorTextBuilder;
		
		public ASMPseudoHelper PseudoHelper { get; set; }
		public ASMValueHelper ValueHelper { get; set; }
		public ASMFormatHelper FormatHelper { get; set; }
		public ASMRegisterHelper RegHelper { get; set; }
		
		public ASMEncoder(ASMPseudoHelper pseudoHelper, ASMValueHelper valueHelper, ASMFormatHelper formatHelper, ASMRegisterHelper regHelper) 
		{ 
			PseudoHelper = pseudoHelper;
			ValueHelper = valueHelper;
			FormatHelper = formatHelper;
			RegHelper = regHelper;
			
			_errorTextBuilder = new StringBuilder();
		}

        public ASMEncoder(ASMEncoder encoder)
        {
            PseudoHelper = new ASMPseudoHelper(encoder.PseudoHelper);
            ValueHelper = PseudoHelper.ValueHelper;
            FormatHelper = ValueHelper.LabelHelper.FormatHelper;
            RegHelper = encoder.RegHelper;

            _errorTextBuilder = new StringBuilder();
        }

        public EncodeLine[] TranslatePseudo(string[] lines, uint pc, bool skipLabelAssertion = false)
        {
            EncodeLine[] encodeLines = null;

            ASMTranslatePseudoResult translatePseudoResult = PseudoHelper.TranslatePseudo(lines, pc, skipLabelAssertion);
            
            /*
            if (translatePseudoResult.lines == null)
                _errorTextBuilder.Append(translatePseudoResult.errorMessage);
            else
                encodeLines = translatePseudoResult.lines;
            */

            encodeLines = translatePseudoResult.lines;
            _errorTextBuilder.Append(translatePseudoResult.errorMessage);

            return encodeLines;
        }

        public EncodeLine[] PreprocessLines(string[] lines, uint pc)
        {
            ASMProcessEquivalencesResult processEquivalencesResult = ASMEquivalenceHelper.ProcessEquivalences(lines);
            if (processEquivalencesResult.ErrorCode > 0)
                _errorTextBuilder.Append(processEquivalencesResult.ErrorMessage);
            else
                lines = processEquivalencesResult.Lines;

            EncodeLine[] encodeLines = TranslatePseudo(lines, pc, true);

            ASMFindLabelsResult findLabelsResult = ValueHelper.LabelHelper.FindLabels(lines, encodeLines, pc);
            if (findLabelsResult.ErrorCode > 0)
                _errorTextBuilder.Append(findLabelsResult.ErrorMessage);

            encodeLines = TranslatePseudo(lines, pc);

            if (encodeLines == null)
                encodeLines = new List<EncodeLine>().ToArray();

            return encodeLines;
        }

        public string[] AddLabelLines(string[] lines)
        {
            List<string> resultLines = new List<string>();

            foreach (string line in lines)
            {
                string modLine = ASMStringHelper.RemoveLeadingBracketBlock(line);
				string processLine = ASMStringHelper.RemoveLeadingSpaces(modLine);
                processLine = ASMStringHelper.RemoveComment(processLine);
                //if ((!processLine.StartsWith("#")) && (!processLine.StartsWith(";")))
                if (processLine.Contains(":"))
                {
                    string[] newResultLines = line.Split(':');
                    int newResultLinesLength = newResultLines.Length;
                    for (int i=0; i < newResultLinesLength; i++)
                    {
                        string newResultLine = newResultLines[i] + ((i < (newResultLinesLength - 1)) ? ":" : "");
                        resultLines.Add(newResultLine);
                    }
                }
                else
                {
                    resultLines.Add(line);
                }
            }

            return resultLines.ToArray();
        }

        public ASMEncoderResult EncodeASM(string asm, string pcText, string spacePadding, bool includeAddress, bool littleEndian)
        {
            ClearErrorText();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return EncodeASM(asm, pc, spacePadding, includeAddress, littleEndian, false);
        }

		public ASMEncoderResult EncodeASM(string asm, uint pc, string spacePadding, bool includeAddress, bool littleEndian, bool clearErrorText)
		{
            if (clearErrorText)
                ClearErrorText();

            string[] lines = asm.Split('\n');
            lines = ASMStringHelper.RemoveFromLines(lines, "\r");
            string[] processLines = AddLabelLines(lines);

            string oldErrorText = _errorTextBuilder.ToString();
            EncodeLine[] encodeLines = PreprocessLines(processLines, pc);

			int encodeLineIndex = 0;
			int lineIndex = 0;
			
			StringBuilder newTextASMBuilder = new StringBuilder();
			StringBuilder newTextASMLineBuilder = new StringBuilder();
			StringBuilder encodedASMBuilder = new StringBuilder();
			
			List<byte> byteList = new List<byte>();

            if (oldErrorText != _errorTextBuilder.ToString())
            {
                return new ASMEncoderResult(encodedASMBuilder.ToString(), byteList.ToArray(), asm, _errorTextBuilder.ToString());
            }
			
			foreach (string line in lines)
			{				
				if (string.IsNullOrEmpty(line))
				{
					newTextASMBuilder.AppendLine();
					continue;
				}
				
				newTextASMLineBuilder.Length = 0;
				
				string modLine = ASMStringHelper.RemoveLeadingBracketBlock(line);
				string processLine = ASMStringHelper.RemoveLeadingSpaces(modLine);
				processLine = ASMStringHelper.RemoveComment(processLine).ToLower();
				string[] parts = ASMStringHelper.SplitLine(processLine);
				
                //pc = ASMPCHelper.ProcessOrg(pc, parts);
                ASMProcessPCResult processPCResult = ASMPCHelper.ProcessOrg(pc, parts);
                pc = processPCResult.PC;
                _errorTextBuilder.Append(processPCResult.ErrorMessage);

                parts = ASMStringHelper.RemoveLabel(parts);
				
				// If this is an ASM command, pass off line to encoding routine
                EncodingFormat encodingOrNull = FormatHelper.FindFormatByCommand(parts[0]);
                if (encodingOrNull != null)
                {
                    if (includeAddress)
                    {
                        newTextASMLineBuilder.Append("[0x");
                        newTextASMLineBuilder.Append(ASMValueHelper.UnsignedToHex_WithLength(pc, 8));
                        newTextASMLineBuilder.Append("] ");
                    }

                    newTextASMLineBuilder.AppendLine(modLine);

                    EncodeLine eLine = new EncodeLine();

                    if (encodeLines.Length > 0)
                        eLine = encodeLines[encodeLineIndex];

                    while ((eLine.LineIndex == lineIndex) && (encodeLineIndex < encodeLines.Length))
                    {
                        encodingOrNull = FormatHelper.FindFormatByCommand(eLine.LineParts[0]);
                        EncodingFormat encoding = encodingOrNull;

                        ASMSingleEncodeResult singleEncodeResult = TryEncodeSingle(eLine.LineParts, encoding, pc, modLine, littleEndian);
                        encodedASMBuilder.Append(spacePadding);
                        encodedASMBuilder.Append(singleEncodeResult.ASMText);
                        encodedASMBuilder.AppendLine();
                        //encodedASMBuilder.Append("\r\n");
                        byteList.AddRange(singleEncodeResult.Bytes);

                        encodeLineIndex++;
                        pc += 4;

                        if (encodeLineIndex < encodeLines.Length)
                            eLine = encodeLines[encodeLineIndex];
                    }

                    lineIndex++;
                }
                else
                {
                    if (!string.IsNullOrEmpty(parts[0]))
                        if ((parts[0] != ".org") && (parts[0] != ".label") && (parts[0] != ".eqv") && (!parts[0].EndsWith(":")))
                            _errorTextBuilder.AppendLine("WARNING: Ignoring unknown command \"" + parts[0] + "\".");
                }
				
				if (string.IsNullOrEmpty(newTextASMLineBuilder.ToString()))
				{
					newTextASMLineBuilder.AppendLine(modLine);
				}
				
				newTextASMBuilder.Append(newTextASMLineBuilder.ToString());
			}
			
			string newTextASM = newTextASMBuilder.ToString();
			if (newTextASM.EndsWith("\r\n"))
				newTextASMBuilder.Length -= 2;
			else if (newTextASM.EndsWith("\n"))
				newTextASMBuilder.Length -= 1;
			
			newTextASM = newTextASMBuilder.ToString();
			return new ASMEncoderResult(encodedASMBuilder.ToString(), byteList.ToArray(), newTextASM, _errorTextBuilder.ToString());
		}
		
		private ASMSingleEncodeResult TryEncodeSingle(string[] parts, EncodingFormat encoding, uint pc, string processLine, bool littleEndian)
		{	
			try
			{
				return EncodeASMSingle(parts, encoding, pc, littleEndian);
			}
			catch (Exception ex)
			{
				_errorTextBuilder.Append("FAILED TO ENCODE LINE : ");
				_errorTextBuilder.Append(processLine);
                _errorTextBuilder.Append(" (");
                _errorTextBuilder.Append(ex.Message);
                _errorTextBuilder.Append(")");
				_errorTextBuilder.Append("\r\n");
				return new ASMSingleEncodeResult("", new List<byte>().ToArray());
			}
		}
		
		private ASMSingleEncodeResult EncodeASMSingle(string[] parts, EncodingFormat encoding, uint pc, bool littleEndian)
		{			
			// Initialize variables
			//string binary = "";
			//string hex = "";
			string strArgs = "";
			string[] args = null;
			
            /*
			if (!string.IsNullOrEmpty(parts[1]))
			{
				strArgs = ASMStringHelper.RemoveSpaces(parts[1]).Replace('(',',').Replace(")","");
				args = strArgs.Split(',');
			}
            */

            // Find encoding format and syntax
            string command = parts[0].ToLower();
            EncodingFormat encodingFormat = FormatHelper.FindFormatByCommand(command);
            string formatBinary = encodingFormat.Binary;
            string syntax = encodingFormat.Syntax;
            string newFormat = formatBinary;

            if (!string.IsNullOrEmpty(parts[1]))
            {
                List<string> argsList = new List<string>();
                strArgs = ASMStringHelper.RemoveSpaces(parts[1]);
                bool foundArg = false;
                int strArgCharIndex = 0;

                foreach (char currentChar in syntax)
                {
                    if (Char.IsLetter(currentChar))
                    {
                        foundArg = true;
                    }
                    else if (foundArg)
                    {
                        foundArg = false;
                        bool isHiLo = ((strArgs.IndexOf("%hi(", strArgCharIndex) == strArgCharIndex) || (strArgs.IndexOf("%lo(", strArgCharIndex) == strArgCharIndex));
                        int separatorIndex = strArgs.IndexOf(currentChar, strArgCharIndex + (isHiLo ? 4 : 0));
                        argsList.Add(strArgs.Substring(strArgCharIndex, separatorIndex - strArgCharIndex));
                        strArgCharIndex = separatorIndex + 1;
                    }
                }

                if (foundArg)
                {
                    argsList.Add(strArgs.Substring(strArgCharIndex, strArgs.Length - strArgCharIndex));
                }

                args = argsList.ToArray();
            }

            // Create array for registers and immediates
            Nullable<uint>[] regValue = new Nullable<uint>[26];
            Nullable<uint>[][] partialRegValue = new Nullable<uint>[26][];
            uint[] immedValue = new uint[26];

            int argsIndex = 0;
            int regIndex = 0;
            int immedIndex = 0;

            // Fill arrays based on order of arguments (syntax)
            foreach (char elementTypeChar in syntax)
            {
                bool incrementArgsIndex = true;

                switch (elementTypeChar)
                {
                    case ASMElementTypeCharacter.GPRegister:
                        regValue[regIndex++] = EncodeGPRegister(args[argsIndex]);
                        break;
                    case ASMElementTypeCharacter.GenericRegister:
                        regValue[regIndex++] = EncodeGenericRegister(args[argsIndex]);
                        break;
                    case ASMElementTypeCharacter.FloatRegister:
                        regValue[regIndex++] = EncodeFloatRegister(args[argsIndex]);
                        break;
                    case ASMElementTypeCharacter.VFPURegister:
                        regValue[regIndex] = EncodeVFPURegister(args[argsIndex], encodingFormat.VFPURegisterTypes[regIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.PartialVFPURegister:
                        partialRegValue[regIndex] = EncodePartialVFPURegister(args[argsIndex], encodingFormat.VFPURegisterTypes[regIndex], encodingFormat.PartialRegisterSizes, encodingFormat.PartialRegisterIncludeMasks[regIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.InvertedSingleBitVFPURegister:
                        regValue[regIndex] = EncodeInvertedSingleBitVFPURegister(args[argsIndex], encodingFormat.VFPURegisterTypes[regIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.Cop0Register:
                        regValue[regIndex] = EncodeCop0Register(args[argsIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.GTEControlRegister:
                        regValue[regIndex] = EncodeGTEControlRegister(args[argsIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.GTEDataRegister:
                        regValue[regIndex] = EncodeGTEDataRegister(args[argsIndex]);
                        regIndex++;
                        break;
                    case ASMElementTypeCharacter.SignedImmediate:
                    case ASMElementTypeCharacter.UnsignedImmediate:
                        immedValue[immedIndex] = EncodeImmediate(args[argsIndex], encodingFormat.ImmediateLengths[immedIndex], (uint)encodingFormat.ImmediateIncludeMasks[immedIndex]);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.BranchImmediate:
                        immedValue[immedIndex] = EncodeBranchImmediate(args[argsIndex], pc, (uint)encodingFormat.ImmediateIncludeMasks[immedIndex]);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.JumpImmediate:
                        immedValue[immedIndex] = EncodeJumpImmediate(args[argsIndex], 26, pc);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.DecrementedImmediate:
                        immedValue[immedIndex] = EncodeDecrementedImmediate(args[argsIndex], encodingFormat.ImmediateLengths[immedIndex], (uint)encodingFormat.ImmediateIncludeMasks[immedIndex]);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.ModifiedImmediate:
                        immedValue[immedIndex] = EncodeModifiedImmediate(args[argsIndex], args[argsIndex - 1], encodingFormat.ImmediateLengths[immedIndex], (uint)encodingFormat.ImmediateIncludeMasks[immedIndex]);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.ShiftedImmediate:
                        immedValue[immedIndex] = EncodeShiftedImmediate(args[argsIndex], encodingFormat.ImmediateLengths[immedIndex], encodingFormat.ShiftedImmediateAmounts[immedIndex], (uint)encodingFormat.ImmediateIncludeMasks[immedIndex]);
                        immedIndex++;
                        break;
                    case ASMElementTypeCharacter.VFPUPrefixImmediate:
                        immedValue[immedIndex] = EncodeVFPUPrefixImmediate(args[argsIndex], encodingFormat.VFPUPrefixType, encodingFormat.ImmediateLengths[immedIndex]);
                        immedIndex++;
                        break;
                    default:
                        incrementArgsIndex = false;
                        break;
                }

                if (incrementArgsIndex)
                    argsIndex++;
            }

            /*
            // Replace bracket blocks in format with appropriate values
            for (int i = 0; i < regIndex; i++)
            {
                newFormat = newFormat.Replace("[" + encodingFormat.RegisterTypes[i] + (i + 1) + "]", regBinary[i]);

                if (partialRegBinary[i] != null)
                {
                    newFormat = newFormat.Replace("[" + encodingFormat.RegisterTypes[i] + (i + 1) + "-1]", partialRegBinary[i][0]);
                    newFormat = newFormat.Replace("[" + encodingFormat.RegisterTypes[i] + (i + 1) + "-2]", partialRegBinary[i][1]);
                }
            }
            for (int i = 0; i < immedIndex; i++)
            {
                newFormat = newFormat.Replace("[" + encodingFormat.ImmediateTypes[i] + (i + 1) + "]", immedBinary[i]);
            }

            binary = newFormat;
            */

            uint uEncodingValue = encodingFormat.BaseEncoding;

            for (int i = 0; i < regIndex; i++)
            {
                if (regValue[i] != null)
                {
                    uEncodingValue |= (((uint)regValue[i]) << encodingFormat.RegisterPositions[i]);
                }
                else if (partialRegValue[i] != null)
                {
                    uEncodingValue |= (((uint)partialRegValue[i][0]) << encodingFormat.PartialRegisterPositions[i][0]);
                    uEncodingValue |= (((uint)partialRegValue[i][1]) << encodingFormat.PartialRegisterPositions[i][1]);
                }
            }

            for (int i = 0; i < immedIndex; i++)
            {
                uEncodingValue |= (immedValue[i] << encodingFormat.ImmediatePositions[i]);
            }

            //byte[] bytes = BitConverter.GetBytes(uEncodingValue);
            byte[] bytes = ASMValueHelper.ConvertUIntToBytes(uEncodingValue, littleEndian);

            if (littleEndian)
            {
                uEncodingValue = ASMValueHelper.ReverseBytes(uEncodingValue);
            }
            //else
            //{
            //    Array.Reverse(bytes);
            //}

            string hex = ASMValueHelper.UnsignedToHex_WithLength(uEncodingValue, 8).ToUpper();
			return new ASMSingleEncodeResult(hex, bytes);
		}
		
		private uint EncodeGPRegister(string register)
		{
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.GPRegister, 0);
            return (uint)regNum;

            //ASMDebugHelper.assert(regNum <= ASMRegisterHelper.MaxValues.GPRegister, "Invalid GPR index: " + regNum.ToString());
            //return ASMValueHelper.UnsignedToBinary_WithLength((uint)regNum, ASMRegisterHelper.EncodingBitLengths.GPRegister);
		}

        private uint EncodeGenericRegister(string register)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.GenericRegister, 0);
            return (uint)regNum;

            //ASMDebugHelper.assert(regNum <= ASMRegisterHelper.MaxValues.GPRegister, "Invalid register index: " + regNum.ToString());
            //return ASMValueHelper.UnsignedToBinary_WithLength((uint)regNum, ASMRegisterHelper.EncodingBitLengths.GenericRegister);
        }

        private uint EncodeFloatRegister(string register)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.FloatRegister, 0);
            return (uint)regNum;

            //ASMDebugHelper.assert(regNum <= ASMRegisterHelper.MaxValues.FloatRegister, "Invalid floating point register index: " + regNum.ToString());
            //return ASMValueHelper.UnsignedToBinary_WithLength((uint)regNum, ASMRegisterHelper.EncodingBitLengths.FloatRegister);
        }

        private uint EncodeVFPURegister(string register, int vfpuRegisterMode)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.VFPURegister, vfpuRegisterMode);
            return (uint)regNum;

            //ASMDebugHelper.assert(regNum <= ASMRegisterHelper.MaxValues.VFPURegister, "Invalid VFPU register index: " + regNum.ToString());
            //return ASMValueHelper.UnsignedToBinary_WithLength((uint)regNum, ASMRegisterHelper.EncodingBitLengths.VFPURegister);
        }

        private uint EncodeInvertedSingleBitVFPURegister(string register, int vfpuRegisterMode)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.VFPURegister, vfpuRegisterMode);
            ASMDebugHelper.assert(regNum <= ASMRegisterHelper.MaxValues.VFPURegister, "Invalid VFPU register index: " + regNum.ToString());
            //string binary = ASMValueHelper.UnsignedToBinary_WithLength((uint)regNum, ASMRegisterHelper.EncodingBitLengths.VFPURegister);

            /*
            char[] binaryChars = binary.ToCharArray();
            int invertIndex = ASMRegisterHelper.VFPUInvertSingleBitStringIndex;
            int digitOffset = ASMStringHelper.CharOffsets.Digit;
            binaryChars[invertIndex] = (char)(1 - (binaryChars[invertIndex] - digitOffset) + digitOffset);
            return new string(binaryChars);
            */

            return (uint)(regNum ^ ASMRegisterHelper.VFPUInvertedSingleBitMask);
        }

        private Nullable<uint>[] EncodePartialVFPURegister(string register, int vfpuRegisterMode, List<int> partialRegisterSizes, int[] partialRegisterIncludeMasks)
        {
            //string binary = EncodeVFPURegister(register, vfpuRegisterMode);
            uint uValue = EncodeVFPURegister(register, vfpuRegisterMode);

            //string[] result = new string[2];
            Nullable<uint>[] result = new Nullable<uint>[2];

            //result[0] = binary.Substring(0, partialRegisterSizes[0]);
            //result[1] = binary.Substring(partialRegisterSizes[0]);

            result[0] = (uint)((uValue >> partialRegisterSizes[1]) & partialRegisterIncludeMasks[0]);
            result[1] = (uint)(uValue & partialRegisterIncludeMasks[1]);

            return result;
        }

        private uint EncodeCop0Register(string register)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.Cop0Register, 0);
            return (uint)regNum;
        }

        private uint EncodeGTEControlRegister(string register)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.GTEControlRegister, 0);
            return (uint)regNum;
        }

        private uint EncodeGTEDataRegister(string register)
        {
            int regNum = RegHelper.TranslateRegister(register, ASMElementTypeCharacter.GTEDataRegister, 0);
            return (uint)regNum;
        }

		// Accepts 0x[hex] or [dec] format
        private uint EncodeImmediate(string strImmed, int length, uint mask)
        {
            /*
            if ((strImmed.StartsWith("0x")) || (strImmed.StartsWith("-0x")))
                return ASMValueHelper.UnsignedToBinary_WithLength(ASMValueHelper.HexToUnsigned_AnySign(strImmed, length), length);
			else
                return ASMValueHelper.UnsignedToBinaryAny_WithLength(Convert.ToInt32(strImmed), length);
            */

            /*
            if ((strImmed.StartsWith("0x")) || (strImmed.StartsWith("-0x")))
                return ASMValueHelper.HexToUnsigned_AnySign(strImmed, length) & mask;
            else
                return ASMValueHelper.SignedToUnsigned(Convert.ToInt32(strImmed)) & mask;
            */

            return ValueHelper.GetAnyUnsignedValue(strImmed) & mask;
        }

        private uint EncodeImmediate(int immed, int length, uint mask)
        {
            //return ASMValueHelper.UnsignedToBinaryAny_WithLength(immed, length);
            return ASMValueHelper.SignedToUnsigned(immed) & mask;
        }

        // Chops off first four and last two bits of 32-bit immediate, as specified by MIPS J-type format
        private uint EncodeJumpImmediate(string input, int reqLength, uint pc)
		{
            /*
			// If a number or label, convert to hex
			string hex = "";
			if (!string.IsNullOrEmpty(input))
			{
				if (!input.StartsWith("0x"))
				{
					if (ASMStringHelper.StringIsNumeric(input))
						hex = "0x" + ASMValueHelper.UnsignedToHex_WithLength(Convert.ToUInt32(input),8);
					else
						hex = "0x" + ValueHelper.LabelHelper.LabelToHex(input,8);
				}
				else
				{	
					hex = input.Substring(0,input.Length);
				}	
			}
            */


            //uint uValue = ValueHelper.FindUnsignedValue(input);

            //string hexNum = hex.Substring(2);
            //hexNum = ASMValueHelper.AddLeadingZeroes(hexNum, reqLength);
            //hex = "0x" + hexNum;

			//return EncodeJumpImmediate(hex);

            //return ((ValueHelper.FindUnsignedValue(input) >> 2) & ASMValueHelper.JumpImmediateMask) | (pc & 0xF0000000);
            //return ((ValueHelper.FindUnsignedValue(input) & ASMValueHelper.JumpImmediateMask) >> 2) | (pc & 0xF0000000);
            return ((ValueHelper.FindUnsignedValue(input) & ASMValueHelper.JumpImmediateMask) >> 2);
		}
		
        /*
		// Chops off first four and last two bits of 32-bit immediate, as specified by MIPS J-type format
        private uint EncodeJumpImmediate(string hex)
		{			
			string newHex = hex.Substring(2,hex.Length-2);
			string binary = ASMValueHelper.HexToBinary(newHex);
			return binary.Substring(4,binary.Length-6);
		}
        */

        private uint EncodeBranchImmediate(uint val, uint pc, uint mask)
		{
			int difference = unchecked((int)((val - pc - 4) / 4));
			//string hexImmed = ASMValueHelper.SignedToHex_WithLength(difference, 4);
            //return ASMValueHelper.HexToBinary_WithLength(hexImmed, 16);
            return ASMValueHelper.SignedToUnsigned(difference) & mask;
		}

        private uint EncodeBranchImmediate(string val, uint pc, uint mask)
		{
			return EncodeBranchImmediate(ValueHelper.FindUnsignedValue(val), pc, mask);
		}

        private uint EncodeDecrementedImmediate(string val, int length, uint mask)
        {
            return EncodeImmediate(((int)ValueHelper.FindUnsignedValue(val) - 1), length, mask);
        }

        private uint EncodeModifiedImmediate(string val, string addend, int length, uint mask)
        {
            return EncodeImmediate(((int)ValueHelper.FindUnsignedValue(val) + (int)ValueHelper.FindUnsignedValue(addend) - 1), length, mask);
        }

        private uint EncodeShiftedImmediate(string val, int length, int shiftAmount, uint mask)
        {
            //return EncodeImmediate(val, length + shiftAmount).Substring(0, length);
            return EncodeImmediate(val, length + shiftAmount, mask) << shiftAmount;
        }

        private uint EncodeVFPUPrefixImmediate(string val, int prefixType, int length)
        {
            string[] values = val.Replace("[", "").Replace("]", "").Split(';');
            //string result = "";

            int iValue = 0;
            uint uValue = 0;

            if (prefixType == ASMVFPUPrefix.Type.Destination)
            {
                int[] sat = new int[4];
                int[] msk = new int[4];

                for (int i = 0; i < 4; i++)
                {
                    if (values[i] == ASMVFPUPrefix.Elements[i].ToString())
                    {
                        msk[i] = 0;
                        sat[i] = 0;
                    }
                    else
                    {
                        switch (values[i])
                        {
                            case "m":
                                msk[i] = 1;
                                break;
                            case "0:1":
                                msk[i] = 0;
                                sat[i] = 1;
                                break;
                            case "-1:1":
                                msk[i] = 0;
                                sat[i] = 3;
                                break;
                            default: break;
                        }
                    }
                }

                iValue = sat[0] + (sat[1] << 2) + (sat[2] << 4) + (sat[3] << 6) + (msk[0] << 8) + (msk[1] << 9) + (msk[2] << 10) + (msk[3] << 11);
                uValue = (uint)iValue;
            }
            else if (prefixType == ASMVFPUPrefix.Type.Source)
            {
                int[] swz = new int[4];
                int[] abs = new int[4];
                int[] cst = new int[4];
                int[] neg = new int[4];

                for (int i = 0; i < 4; i++)
                {
                    if (values[i].StartsWith("-"))
                    {
                        neg[i] = 1;
                        values[i] = values[i].Replace("-", "");
                    }

                    cst[i] = 1;
                    switch (values[i])
                    {
                        case "0":
                            abs[i] = 0;
                            swz[i] = 0;
                            break;
                        case "3":
                            abs[i] = 1;
                            swz[i] = 0;
                            break;
                        case "1":
                            abs[i] = 0;
                            swz[i] = 1;
                            break;
                        case "1/3":
                            abs[i] = 1;
                            swz[i] = 1;
                            break;
                        case "2":
                            abs[i] = 0;
                            swz[i] = 2;
                            break;
                        case "1/4":
                            abs[i] = 1;
                            swz[i] = 2;
                            break;
                        case "1/2":
                            abs[i] = 0;
                            swz[i] = 3;
                            break;
                        case "1/6":
                            abs[i] = 1;
                            swz[i] = 3;
                            break;
                        default:
                            cst[i] = 0;
                            break;
                    }

                    if (values[i].Contains("|"))
                    {
                        char charVal = values[i].ToCharArray()[1];

                        abs[i] = 1;
                        swz[i] = ASMVFPUPrefix.ElementIndexDict[charVal];
                    }
                    else
                    {
                        char charVal = values[i].ToCharArray()[0];

                        abs[i] = 0;
                        swz[i] = ASMVFPUPrefix.ElementIndexDict[charVal];
                    }
                }

                iValue = swz[0] + (swz[1] << 2) + (swz[2] << 4) + (swz[3] << 6)
                            + (abs[0] << 8) + (abs[1] << 9) + (abs[2] << 10) + (abs[3] << 11)
                            + (cst[0] << 12) + (cst[1] << 13) + (cst[2] << 14) + (cst[3] << 15)
                            + (neg[0] << 16) + (neg[1] << 17) + (neg[2] << 18) + (neg[3] << 19);

                uValue = (uint)iValue;
            }

            //result = ASMValueHelper.UnsignedToBinary_WithLength(uValue, length);
            //return result;

            return uValue;
        }

        public string ReplaceLabelsInHex(string hex, bool littleEndian)
        {
            return ValueHelper.LabelHelper.ReplaceLabelsInHex(hex, littleEndian);
        }

		private void ClearErrorText()
		{
			_errorTextBuilder.Length = 0;
		}
	}
}
