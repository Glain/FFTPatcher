/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 9:59
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASMEncoding.Helpers
{
    public class EncodingFormat
    {
        public string Command { get; set; }
        public string Binary { get; set; }
        public string ExpandedFormat { get; set; }
        public string Syntax { get; set; }
        public List<char> RegisterTypes { get; set; }
        public List<char> ImmediateTypes { get; set; }
        public List<int> ImmediateLengths { get; set; }
        public List<int> ShiftedImmediateAmounts { get; set; }
        public List<int> VFPURegisterTypes { get; set; }
        public List<int> PartialRegisterSizes { get; set; }
        public int VFPUPrefixType { get; set; }

        public List<int> RegisterPositions { get; set; }
        public List<int[]> PartialRegisterPositions { get; set; }
        public List<int> ImmediatePositions { get; set; }

        public List<int> RegisterIncludeMasks { get; set; }
        public List<int[]> PartialRegisterIncludeMasks { get; set; }
        public List<int> ImmediateIncludeMasks { get; set; }

        public int NumRegs { get; set; }
        public int NumImmeds { get; set; }

        public int Opcode { get; set; }
        public int Function { get; set; }

        public uint BaseEncoding { get; set; }
        public uint BitMask { get; set; }

        public EncodingFormat() { }
    }
	
    public class ASMElementTypeCharacter
    {
        public const char GPRegister = 'r';
        public const char GenericRegister = 'c';
        public const char FloatRegister = 'f';
        public const char VFPURegister = 'g';
        public const char PartialVFPURegister = 'p';
        public const char InvertedSingleBitVFPURegister = 'n';
        public const char SignedImmediate = 'i';
        public const char UnsignedImmediate = 'u';
        public const char BranchImmediate = 'b';
        public const char JumpImmediate = 'j';
        public const char DecrementedImmediate = 'd';
        public const char ModifiedImmediate = 'm';
        public const char ShiftedImmediate = 'h';
        public const char VFPUPrefixImmediate = 'x';
    }

    public class ASMElementMetaType
    {
        public const int Register = 0;
        public const int Immediate = 1;
    }

    public class ASMVFPUPrefix
    {
        internal class Type
        {
            public const int Destination = 0;
            public const int Source = 1;
        }

        public static char[] Elements = { 'x', 'y', 'z', 'w' };

        public static readonly Dictionary<char, int> ElementIndexDict = new Dictionary<char, int> {
            { 'x', 0 },
            { 'y', 1 },
            { 'z', 2 },
            { 'w', 3 }
        };
    }

	public class EncodeLine
	{
		public string[] LineParts { get; set; }
		public int LineIndex { get; set; }
		
		public EncodeLine() { }
		public EncodeLine(string[] lineParts, int lineIndex)
		{
			LineParts = lineParts;
			LineIndex = lineIndex;
		}
	}
	
	public class ASMFormatHelper
	{
        internal class Opcodes
        {
            public static int NumPossibleOpcodes = 64;
            public static int NumPossibleFunctions = 64;

            public static int Special   = 0x00; // "000000"
            public static int Special2  = 0x1C; // "011100"
            public static int Special3  = 0x1F; // "011111"

            public static bool IsSpecialOpcode(int opcode)
            {
                return (opcode == Special) || (opcode == Special2) || (opcode == Special3);
            }
        }

        public static int InstructionBitLength = 32;

        public List<EncodingFormat> EncodeList { get; set; }
        public Dictionary<string, EncodingFormat> EncodeDict { get; set; }

        public List<EncodingFormat>[] DecodeOpcodeList { get; set; }
        public List<EncodingFormat>[][] DecodeOpcodeFunctionList { get; set; }
		
		public ASMFormatHelper() 
		{
            EncodeList = new List<EncodingFormat>();
            EncodeDict = new Dictionary<string, EncodingFormat>();

            DecodeOpcodeList = new List<EncodingFormat>[Opcodes.NumPossibleOpcodes];
            DecodeOpcodeFunctionList = new List<EncodingFormat>[Opcodes.NumPossibleOpcodes][];
		}

        public ASMFormatHelper(ASMFormatHelper helper)
        {
            CopyFormatHelper(helper.EncodeList, helper.EncodeDict, helper.DecodeOpcodeList, helper.DecodeOpcodeFunctionList);
        }

        public void CopyFormatHelper(List<EncodingFormat> encodeList, Dictionary<string, EncodingFormat> encodeDict, List<EncodingFormat>[] decodeOpcodeList, List<EncodingFormat>[][] decodeOpcodeFunctionList)
        {
            EncodeList = new List<EncodingFormat>(encodeList);
            EncodeDict = new Dictionary<string, EncodingFormat>(encodeDict);
            DecodeOpcodeList = new List<List<EncodingFormat>>(decodeOpcodeList).ToArray();
            DecodeOpcodeFunctionList = new List<List<EncodingFormat>[]>(decodeOpcodeFunctionList).ToArray();
        }

		// For reading encoding files
		public void ReadEncodeList(string filepath)
		{
	    	StreamReader reader = new StreamReader(filepath);

	        try   
	        {    
	            while(reader.Peek() != -1)
	            {
	            	string line = reader.ReadLine();
	            	
	            	if (!string.IsNullOrEmpty(line))
	            	{
		            	line = ASMStringHelper.RemoveSpaces(line);
		            	string[] lineArray = line.Split(':');
		            
		            	EncodingFormat encoding = new EncodingFormat();
		            	encoding.Command = lineArray[0].ToLower();
		            	encoding.Binary = lineArray[1];
                        encoding.Syntax = lineArray[2];

                        if (lineArray.Length > 3)
                            encoding.ImmediateLengths = ASMStringHelper.CreateIntList(lineArray[3]);
                        if (lineArray.Length > 4)
                            encoding.VFPURegisterTypes = ASMStringHelper.CreateIntList(lineArray[4]);
                        if (lineArray.Length > 5)
                            encoding.PartialRegisterSizes = ASMStringHelper.CreateIntList(lineArray[5]);
                        if (lineArray.Length > 6)
                            encoding.ShiftedImmediateAmounts = ASMStringHelper.CreateIntList(lineArray[6]);
                        if (lineArray.Length > 7)
                            encoding.VFPUPrefixType = int.Parse(lineArray[7]);

                        encoding = FindFormatDetails(encoding);

                        string expFormat = ExpandFormat(encoding);
                        encoding.ExpandedFormat = expFormat;

                        encoding = FindFormatBaseValues(encoding);
                        encoding = FindParameterIndexes(encoding);

                        EncodeList.Add(encoding);
                        EncodeDict.Add(encoding.Command, encoding);

                        AddToDecodeLists(expFormat, encoding);
	            	}
	            }   
	        }      
	        catch 
	        { 
	            
	        }
	        finally
	        {
	            reader.Close();
	        }
		}

        public void AddToDecodeLists(string expFormat, EncodingFormat encoding)
        {
            AddToOpcodeList(encoding.Opcode, encoding);

            if (Opcodes.IsSpecialOpcode(encoding.Opcode))
            {
                AddToOpcodeFunctionList(encoding.Opcode, encoding.Function, encoding);
            }
        }

        public void AddToOpcodeList(int opcode, EncodingFormat encoding)
        {
            List<EncodingFormat> decodeList = null;

            if (DecodeOpcodeList[opcode] != null)
            {
                decodeList = DecodeOpcodeList[opcode];
                decodeList.Add(encoding);
                DecodeOpcodeList[opcode] = decodeList;
            }
            else
            {
                decodeList = new List<EncodingFormat>();
                decodeList.Add(encoding);
                DecodeOpcodeList[opcode] = decodeList;
            }
        }

        public void AddToOpcodeFunctionList(int opcode, int function, EncodingFormat encoding)
        {
            List<EncodingFormat>[] decodeList = null;

            if (DecodeOpcodeFunctionList[opcode] != null)
            {
                decodeList = DecodeOpcodeFunctionList[opcode];
            }
            else
            {
                decodeList = new List<EncodingFormat>[Opcodes.NumPossibleFunctions];
            }

            List<EncodingFormat> encFormatList = null;
            if (decodeList[function] != null)
            {
                encFormatList = decodeList[function];
                encFormatList.Add(encoding);
                decodeList[function] = encFormatList;
            }
            else
            {
                encFormatList = new List<EncodingFormat>();
                encFormatList.Add(encoding);
                decodeList[function] = encFormatList;
            }

            DecodeOpcodeFunctionList[opcode] = decodeList;            
        }

        // Find whether or not this is a valid command (and has an entry in our encoding list). If it
        // does, return it; otherwise, return null.
        public EncodingFormat FindFormatByCommand(string command)
        {
            EncodingFormat format = null;
            EncodeDict.TryGetValue(command, out format);
            return format;
        }

        public EncodingFormat FindFormatByBinary(uint uBinaryLine)
        {
            List<EncodingFormat> encodeList = new List<EncodingFormat>();

            int opcode = (int)(uBinaryLine >> 26);
            int function = (int)(uBinaryLine & 0x3F);

            if (DecodeOpcodeFunctionList[opcode] != null)
            {
                List<EncodingFormat>[] decodeLookupList = DecodeOpcodeFunctionList[opcode];

                if (decodeLookupList[function] != null)
                {
                    encodeList = decodeLookupList[function];
                }
                else
                {
                    encodeList = DecodeOpcodeList[opcode];
                }
            }
            else if (DecodeOpcodeList[opcode] != null)
            {
                encodeList = DecodeOpcodeList[opcode];
            }

            foreach (EncodingFormat encodingFormat in encodeList)
            {
                //if (IsMatchingFormat(encodingFormat, binaryLine))
                if ((uBinaryLine & encodingFormat.BitMask) == encodingFormat.BaseEncoding)
                    return encodingFormat;
            }

            return null;
        }

        // Regs use uppercase letters, immediates use lowercase letters: [r1] becomes AAAAA, [i1] becomes aaa if length = 3, etc.
        public static string ExpandFormat(EncodingFormat format)
		{
            int numRegs = format.NumRegs;
            int numImmeds = format.NumImmeds;
            string newFormatBinary = format.Binary;

            for (int i = 0; i < numRegs; i++)
            {
                char elementTypeChar = format.RegisterTypes[i];
                string strElementTypeChar = elementTypeChar.ToString();
                int iPlusOne = i + 1;
                string strIPlusOne = iPlusOne.ToString();

                newFormatBinary = newFormatBinary.Replace("[" + strElementTypeChar + strIPlusOne + "]", ASMStringHelper.CreateCharacterString(ASMStringHelper.CreateUpperLetterChar(i), ASMRegisterHelper.GetEncodingBitLength(elementTypeChar)));

                if (format.PartialRegisterSizes != null)
                {
                    newFormatBinary = newFormatBinary.Replace("[" + strElementTypeChar + strIPlusOne + "-1]", ASMStringHelper.CreateCharacterString(ASMStringHelper.CreateUpperLetterChar(i), format.PartialRegisterSizes[0]));
                    newFormatBinary = newFormatBinary.Replace("[" + strElementTypeChar + strIPlusOne + "-2]", ASMStringHelper.CreateCharacterString(ASMStringHelper.CreateUpperLetterChar(i), format.PartialRegisterSizes[1]));
                }
            }
            for (int i = 0; i < numImmeds; i++)
            {
                newFormatBinary = newFormatBinary.Replace("[" + format.ImmediateTypes[i] + (i + 1) + "]", ASMStringHelper.CreateCharacterString(ASMStringHelper.CreateLowerLetterChar(i), format.ImmediateLengths[i]));
            }

            return newFormatBinary;
        }

        public static EncodingFormat FindParameterIndexes(EncodingFormat format)
        {
            List<int> regPositions = new List<int>();
            List<int[]> partialRegPositions = new List<int[]>();
            List<int> immedPositions = new List<int>();

            List<int> regIncludeMasks = new List<int>();
            List<int[]> partialRegIncludeMasks = new List<int[]>();
            List<int> immedIncludeMasks = new List<int>();

            string expFormat = format.ExpandedFormat;
            string formatBinary = format.Binary;

            char currentChar = (char)ASMStringHelper.CharOffsets.UpperLetter;
            for (int i = 0; i < format.NumRegs; i++)
            {
                if (format.RegisterTypes[i] == ASMElementTypeCharacter.PartialVFPURegister)
                {
                    int[] partialPositions = new int[2];
                    int[] partialIncludeMasks = new int[2];

                    int part1Index = formatBinary.IndexOf("[p" + (i + 1).ToString() + "-1");
                    int part2Index = formatBinary.IndexOf("[p" + (i + 1).ToString() + "-2");

                    int firstIndex = expFormat.IndexOf(currentChar);
                    int searchPartIndex = firstIndex;
                    for (; expFormat[searchPartIndex] == currentChar; searchPartIndex++) ;
                    int secondIndex = expFormat.IndexOf(currentChar, searchPartIndex);

                    if (part1Index > part2Index)
                    {
                        partialPositions[0] = secondIndex;
                        partialPositions[1] = firstIndex;
                    }
                    else
                    {
                        partialPositions[0] = firstIndex;
                        partialPositions[1] = secondIndex;
                    }

                    partialPositions[0] = InstructionBitLength - partialPositions[0] - format.PartialRegisterSizes[0];
                    partialPositions[1] = InstructionBitLength - partialPositions[1] - format.PartialRegisterSizes[1];

                    partialIncludeMasks[0] = ASMValueHelper.GetIncludeMask(format.PartialRegisterSizes[0]);
                    partialIncludeMasks[1] = ASMValueHelper.GetIncludeMask(format.PartialRegisterSizes[1]);

                    partialRegPositions.Add(partialPositions);
                    partialRegIncludeMasks.Add(partialIncludeMasks);
                    regPositions.Add(0);
                    regIncludeMasks.Add(0);
                }
                else
                {
                    regPositions.Add(InstructionBitLength - expFormat.IndexOf(currentChar) - ASMRegisterHelper.GetEncodingBitLength(format.RegisterTypes[i]));
                    regIncludeMasks.Add(ASMRegisterHelper.GetRegisterIncludeMask(format.RegisterTypes[i]));
                    partialRegPositions.Add(null);
                    partialRegIncludeMasks.Add(null);
                }

                currentChar++;
            }

            currentChar = (char)ASMStringHelper.CharOffsets.LowerLetter;
            for (int i = 0; i < format.NumImmeds; i++)
            {
                immedPositions.Add(InstructionBitLength - expFormat.IndexOf(currentChar) - format.ImmediateLengths[i]);
                immedIncludeMasks.Add(ASMValueHelper.GetIncludeMask(format.ImmediateLengths[i]));
                currentChar++;
            }

            format.RegisterPositions = regPositions;
            format.PartialRegisterPositions = partialRegPositions;
            format.ImmediatePositions = immedPositions;

            format.RegisterIncludeMasks = regIncludeMasks;
            format.PartialRegisterIncludeMasks = partialRegIncludeMasks;
            format.ImmediateIncludeMasks = immedIncludeMasks;

            return format;
        }

        public static int FindElementMetaType(char elementTypeChar)
        {
            switch (elementTypeChar)
            {
                case ASMElementTypeCharacter.GPRegister:
                case ASMElementTypeCharacter.GenericRegister:
                case ASMElementTypeCharacter.FloatRegister:
                case ASMElementTypeCharacter.VFPURegister:
                case ASMElementTypeCharacter.PartialVFPURegister:
                case ASMElementTypeCharacter.InvertedSingleBitVFPURegister:
                    return ASMElementMetaType.Register;
                default:
                    return ASMElementMetaType.Immediate;
            }
        }

        private EncodingFormat FindFormatDetails(EncodingFormat encoding)
        {
            List<char> registerTypes = new List<char>();
            List<char> immediateTypes = new List<char>();

            string syntax = encoding.Syntax;
            int numRegs = 0;
            int numImmeds = 0;

            foreach (char c in syntax)
            {
                if (char.IsLetter(c))
                {
                    int metaType = FindElementMetaType(c);

                    if (metaType == ASMElementMetaType.Register)
                    {
                        numRegs++;
                        registerTypes.Add(c);
                    }
                    else if (metaType == ASMElementMetaType.Immediate)
                    {
                        numImmeds++;
                        immediateTypes.Add(c);
                    }
                }
            }

            encoding.RegisterTypes = registerTypes;
            encoding.ImmediateTypes = immediateTypes;
            encoding.NumRegs = numRegs;
            encoding.NumImmeds = numImmeds;

            return encoding;
        }

        private EncodingFormat FindFormatBaseValues(EncodingFormat encoding)
        {
            uint[] baseValues = FindBaseValues(encoding.ExpandedFormat);
            encoding.BaseEncoding = baseValues[0];
            encoding.BitMask = baseValues[1];
            encoding.Opcode = (int)baseValues[2];
            encoding.Function = (int)baseValues[3];
            return encoding;
        }

        private uint[] FindBaseValues(string expFormat)
        {
            char[] chars = expFormat.ToCharArray();
            char[] bitMaskChars = new char[chars.Length];

            for (int i=0; i < chars.Length; i++)
            {
                if (!char.IsDigit(chars[i]))
                {
                    chars[i] = '0';
                    bitMaskChars[i] = '0';
                }
                else
                {
                    bitMaskChars[i] = '1';
                }
            }

            string strBaseEncoding = new string(chars);
            string strBitMask = new string(bitMaskChars);

            uint baseEncoding = ASMValueHelper.BinaryToUnsigned(strBaseEncoding);
            uint bitMask = ASMValueHelper.BinaryToUnsigned(strBitMask);
            uint opcode = baseEncoding >> 26;
            uint function = baseEncoding & 0x3F;

            uint[] result = new uint[4];
            result[0] = baseEncoding;
            result[1] = bitMask;
            result[2] = opcode;
            result[3] = function;

            return result;
        }
	}
}
