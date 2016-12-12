/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 10:16
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace ASMEncoding.Helpers
{
	public class ASMRegisterHelper
	{
        internal class EncodingBitLengths
        {
            public static int GPRegister = 5;
            public static int FloatRegister = 5;
            public static int VFPURegister = 7;
            public static int GenericRegister = 5;
        }

        internal class MaxValues
        {
            public static uint GPRegister = 31;
            public static uint FloatRegister = 31;
            public static uint VFPURegister = 127;
            public static uint GenericRegister = 31;
        }

        internal class Prefixes
        {
            public static string GPRegister = "r";
            public static string FloatRegister = "f";
            public static string VFPURegister = "vfr";
            public static string GenericRegister = "$";
        }

        internal class VFPURegisterModes
        {
            public static int Single = 0;
            public static int Pair = 1;
            public static int Triple = 2;
            public static int Quad = 3;
            public static int MatrixPair = 4;
            public static int MatrixTriple = 5;
            public static int MatrixQuad = 6;
            public static int NumModes = 7;
        }

        internal class VFPUAliases
        {
            public int Index { get; set; }
            public string GenericName { get; set; }

            public string[] Aliases { get; set; }

            public string this[int index]
            {
                get { return Aliases[index]; }
                set
                {
                    Aliases[index] = (string.IsNullOrEmpty(value) ? GenericName : value);
                }
            }

            public string Single 
            {
                get { return Aliases[VFPURegisterModes.Single]; }
                set { this[VFPURegisterModes.Single] = value; }
            }

            public string Pair
            {
                get { return Aliases[VFPURegisterModes.Pair]; }
                set { this[VFPURegisterModes.Pair] = value; }
            }

            public string Triple
            {
                get { return Aliases[VFPURegisterModes.Triple]; }
                set { this[VFPURegisterModes.Triple] = value; }
            }

            public string Quad
            {
                get { return Aliases[VFPURegisterModes.Quad]; }
                set { this[VFPURegisterModes.Quad] = value; }
            }

            public string MatrixPair
            {
                get { return Aliases[VFPURegisterModes.MatrixPair]; }
                set { this[VFPURegisterModes.MatrixPair] = value; }
            }

            public string MatrixTriple
            {
                get { return Aliases[VFPURegisterModes.MatrixTriple]; }
                set { this[VFPURegisterModes.MatrixTriple] = value; }
            }

            public string MatrixQuad
            {
                get { return Aliases[VFPURegisterModes.MatrixQuad]; }
                set { this[VFPURegisterModes.MatrixQuad] = value; }
            }

            public VFPUAliases()
            {
                Aliases = new string[VFPURegisterModes.NumModes];
            }

            public string GetAliasByRegisterMode(int vfpuRegisterMode)
            {
                return Aliases[vfpuRegisterMode];
            }
        }

        public static int VFPUInvertSingleBitStringIndex = 1;
        public static int VFPUInvertedSingleBitMask = 0x20; // binary 0010 0000

		List<string> GPRegisterList { get; set; }
        Dictionary<string, int> GPRegisterIndexDict { get; set; }
        List<VFPUAliases> VFPURegisterList { get; set; }
        Dictionary<string, int>[] VFPURegisterIndexDictArray { get; set; }
        List<string> Cop0RegisterList { get; set; }
        Dictionary<string, int> Cop0RegisterIndexDict { get; set; }
        List<string> GTEControlRegisterList { get; set; }
        Dictionary<string, int> GTEControlRegisterIndexDict { get; set; }
        List<string> GTEDataRegisterList { get; set; }
        Dictionary<string, int> GTEDataRegisterIndexDict { get; set; }
        
		public ASMRegisterHelper() 
		{ 
            GPRegisterList = new List<string>();
            GPRegisterIndexDict = new Dictionary<string, int>();
            VFPURegisterList = new List<VFPUAliases>();
            VFPURegisterIndexDictArray = new Dictionary<string, int>[VFPURegisterModes.NumModes];
            Cop0RegisterList = new List<string>();
            Cop0RegisterIndexDict = new Dictionary<string, int>();
            GTEControlRegisterList = new List<string>();
            GTEControlRegisterIndexDict = new Dictionary<string, int>();
            GTEDataRegisterList = new List<string>();
            GTEDataRegisterIndexDict = new Dictionary<string, int>();

            for (int i = 0; i < VFPURegisterModes.NumModes; i++)
            {
                VFPURegisterIndexDictArray[i] = new Dictionary<string, int>();
            }
		}

        public static int GetRegisterIncludeMask(char elementTypeChar)
        {
            return ASMValueHelper.GetIncludeMask(GetEncodingBitLength(elementTypeChar));
        }

        public static int GetEncodingBitLength(char elementTypeChar)
        {
            switch (elementTypeChar)
            {
                case ASMElementTypeCharacter.GPRegister: return EncodingBitLengths.GPRegister;
                case ASMElementTypeCharacter.GenericRegister: return EncodingBitLengths.GenericRegister;
                case ASMElementTypeCharacter.FloatRegister: return EncodingBitLengths.FloatRegister;
                case ASMElementTypeCharacter.VFPURegister: return EncodingBitLengths.VFPURegister;
                case ASMElementTypeCharacter.InvertedSingleBitVFPURegister: return EncodingBitLengths.VFPURegister;
                case ASMElementTypeCharacter.PartialVFPURegister: return EncodingBitLengths.VFPURegister;
                default: return EncodingBitLengths.GenericRegister;
            }
        }

        public static string GetRegisterPrefix(char elementTypeChar)
        {
            switch (elementTypeChar)
            {
                case ASMElementTypeCharacter.GPRegister: return Prefixes.GPRegister;
                case ASMElementTypeCharacter.GenericRegister: return Prefixes.GenericRegister;
                case ASMElementTypeCharacter.FloatRegister: return Prefixes.FloatRegister;
                case ASMElementTypeCharacter.VFPURegister: return Prefixes.VFPURegister;
                case ASMElementTypeCharacter.InvertedSingleBitVFPURegister: return Prefixes.VFPURegister;
                case ASMElementTypeCharacter.PartialVFPURegister: return Prefixes.VFPURegister;
                default: return Prefixes.GenericRegister;
            }
        }

        public static bool IsVFPURegister(char elementTypeChar)
        {
            return ((elementTypeChar == ASMElementTypeCharacter.VFPURegister) || (elementTypeChar == ASMElementTypeCharacter.PartialVFPURegister) 
                || (elementTypeChar == ASMElementTypeCharacter.InvertedSingleBitVFPURegister));
        }

        public string GetRegisterName(int index, bool useAlias, char elementTypeChar, int vfpuRegisterMode)
        {
            switch (elementTypeChar)
            {
                case ASMElementTypeCharacter.GPRegister: return GetGPRegisterName(index, useAlias);
                case ASMElementTypeCharacter.FloatRegister: return GetFloatRegisterName(index);
                case ASMElementTypeCharacter.VFPURegister: return GetVFPURegisterName(index, vfpuRegisterMode);
                case ASMElementTypeCharacter.InvertedSingleBitVFPURegister: return GetVFPURegisterName(index, vfpuRegisterMode);
                case ASMElementTypeCharacter.PartialVFPURegister: return GetVFPURegisterName(index, vfpuRegisterMode);
                case ASMElementTypeCharacter.Cop0Register: return GetCop0RegisterName(index);
                case ASMElementTypeCharacter.GTEControlRegister: return GetGTEControlRegisterName(index);
                case ASMElementTypeCharacter.GTEDataRegister: return GetGTEDataRegisterName(index);
                default: return GetGenericRegisterName(index);
            }
        }

        public string GetGPRegisterName(int index, bool useAlias)
        {
            return useAlias ? GPRegisterList[index] : (Prefixes.GPRegister + index.ToString());
        }

        public string GetGenericRegisterName(int index)
        {
            return Prefixes.GenericRegister + index.ToString();
        }

        public string GetFloatRegisterName(int index)
        {
            return Prefixes.FloatRegister + index.ToString();
        }

        public string GetVFPURegisterName(int index, int vpuRegisterMode)
        {
            return VFPURegisterList[index].GetAliasByRegisterMode(vpuRegisterMode);
        }

        public string GetCop0RegisterName(int index)
        {
            return Cop0RegisterList[index];
        }

        public string GetGTEControlRegisterName(int index)
        {
            return GTEControlRegisterList[index];
        }

        public string GetGTEDataRegisterName(int index)
        {
            return GTEDataRegisterList[index];
        }

		// For reading general purpose register file. This allows the 'named' format for registers,
		// e.g. sp = r29, ra = r31
		public void ReadGPRegisterList()
		{
            ReadRegisterList(GPRegisterList, GPRegisterIndexDict, ASMDataFileMap.MIPS_GPRegisters);
		}

        public void ReadCop0RegisterList()
        {
            ReadRegisterList(Cop0RegisterList, Cop0RegisterIndexDict, ASMDataFileMap.MIPS_COP0Registers);
        }

        public void ReadGTEControlRegisterList()
        {
            ReadRegisterList(GTEControlRegisterList, GTEControlRegisterIndexDict, ASMDataFileMap.PSX_GTEControlRegisters);
        }

        public void ReadGTEDataRegisterList()
        {
            ReadRegisterList(GTEDataRegisterList, GTEDataRegisterIndexDict, ASMDataFileMap.PSX_GTEDataRegisters);
        }

        public void ReadRegisterList(List<string> registerList, Dictionary<string, int> registerIndexDict, string filename)
        {
            StreamReader reader = new StreamReader(filename);

            try
            {
                int regIndex = 0;
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        registerList.Add(line);
                        registerIndexDict.Add(line, regIndex);
                    }

                    regIndex++;
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

        // For reading VFPU register aliases from file.
        public void ReadVFPURegisterAliasList()
        {
            StreamReader reader = new StreamReader(ASMDataFileMap.PSP_VFPURegisters);

            try
            {
                int regIndex = 0;
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] strAliases = line.Split(',');

                        VFPUAliases aliases = new VFPUAliases();
                        aliases.Index = regIndex;
                        aliases.GenericName = Prefixes.VFPURegister + regIndex.ToString();
                        aliases.Single = strAliases[VFPURegisterModes.Single];
                        aliases.Pair = strAliases[VFPURegisterModes.Pair];
                        aliases.Triple = strAliases[VFPURegisterModes.Triple];
                        aliases.Quad = strAliases[VFPURegisterModes.Quad];
                        aliases.MatrixPair = strAliases[VFPURegisterModes.MatrixPair];
                        aliases.MatrixTriple = strAliases[VFPURegisterModes.MatrixTriple];
                        aliases.MatrixQuad = strAliases[VFPURegisterModes.MatrixQuad];

                        VFPURegisterList.Add(aliases);
                        AddVFPUAliasesToDictionary(aliases, regIndex);
                    }

                    regIndex++;
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

        public int TranslateRegister(string register, char elementTypeChar, int vfpuRegisterMode)
        {
            switch (elementTypeChar)
            {
                case ASMElementTypeCharacter.GPRegister: return TranslateGPRegister(register);
                case ASMElementTypeCharacter.FloatRegister: return TranslateFloatRegister(register);
                case ASMElementTypeCharacter.VFPURegister: return TranslateVFPURegister(register, vfpuRegisterMode);
                case ASMElementTypeCharacter.InvertedSingleBitVFPURegister: return TranslateVFPURegister(register, vfpuRegisterMode);
                case ASMElementTypeCharacter.PartialVFPURegister: return TranslateVFPURegister(register, vfpuRegisterMode);
                case ASMElementTypeCharacter.GenericRegister: return TranslateGenericRegister(register);
                case ASMElementTypeCharacter.Cop0Register: return TranslateCop0Register(register);
                case ASMElementTypeCharacter.GTEControlRegister: return TranslateGTEControlRegister(register);
                case ASMElementTypeCharacter.GTEDataRegister: return TranslateGTEDataRegister(register);
                default: return TranslateGenericRegister(register);
            }
        }

        public int TranslateGPRegister(string register)
        {
            if (GPRegisterIndexDict.ContainsKey(register))
                return GPRegisterIndexDict[register];

            string cutoffRegister = register.Substring(1);
            if (GPRegisterIndexDict.ContainsKey(cutoffRegister))
                return GPRegisterIndexDict[cutoffRegister];

            return FindGenericRegisterIndex(register);
        }

        public int TranslateGenericRegister(string register)
        {
            return FindGenericRegisterIndex(register);
        }

        public int TranslateFloatRegister(string register)
        {
            return FindGenericRegisterIndex(register);
        }

        public int TranslateVFPURegister(string register, int vfpuRegisterMode)
        {
            Dictionary<string,int> vfpuRegisterIndexDict = VFPURegisterIndexDictArray[vfpuRegisterMode];

            if (vfpuRegisterIndexDict.ContainsKey(register))
                return vfpuRegisterIndexDict[register];

            return FindGenericRegisterIndex(register);
        }

        public int TranslateCop0Register(string register)
        {
            if (Cop0RegisterIndexDict.ContainsKey(register))
                return Cop0RegisterIndexDict[register];

            return FindGenericRegisterIndex(register);
        }

        public int TranslateGTEControlRegister(string register)
        {
            if (GTEControlRegisterIndexDict.ContainsKey(register))
                return GTEControlRegisterIndexDict[register];

            return FindGenericRegisterIndex(register);
        }

        public int TranslateGTEDataRegister(string register)
        {
            if (GTEDataRegisterIndexDict.ContainsKey(register))
                return GTEDataRegisterIndexDict[register];

            return FindGenericRegisterIndex(register);
        }
         
        private int FindGenericRegisterIndex(string register)
        {
            int firstDigitChar = 0;
            for (; ((firstDigitChar < register.Length) && !char.IsDigit(register[firstDigitChar])); firstDigitChar++) ;
            string prefix = register.Substring(0, firstDigitChar).ToLower();
            register = register.Replace(prefix, "");
            return int.Parse(register);
        }

        private void AddVFPUAliasesToDictionary(VFPUAliases aliases, int regIndex)
        {
            for (int i=0; i < VFPURegisterModes.NumModes; i++)
            {
                string value = aliases.Aliases[i].ToLower();
                if (!VFPURegisterIndexDictArray[i].ContainsKey(value))
                    VFPURegisterIndexDictArray[i].Add(value, regIndex);
            }
        }
	}
}
