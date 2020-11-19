/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 13:14
 */
using System;
using ASMEncoding.Helpers;
using System.Collections.Generic;

namespace ASMEncoding
{		
    public class ASMEncodingMode
    {
        public const int NumModes = 3;

        public const int Base = 0;
        public const int PSX = 1;
        public const int PSP = 2;
    }

	public class ASMEncodingUtilityHelper
	{
		private ASMEncoder _asmEncoder;
		private ASMDecoder _asmDecoder;
        
        private ASMCheckHelper _asmCheckHelper;

        private ASMRegisterHelper _regHelper;
        private ASMLabelHelper _labelHelper;
        private ASMValueHelper _valueHelper;
        private ASMFormatHelper _formatHelper;
        private ASMPseudoHelper _pseudoHelper;
		
		public ASMEncodingUtilityHelper(bool loadDefaults = true)
		{
			_regHelper = new ASMRegisterHelper();
			_formatHelper = new ASMFormatHelper();
			_labelHelper = new ASMLabelHelper(_formatHelper);
			_valueHelper = new ASMValueHelper(_labelHelper);
			_pseudoHelper = new ASMPseudoHelper(_valueHelper, _formatHelper);

            if (loadDefaults)
            {
                _formatHelper.ReadEncodeList(ASMDataFileMap.MIPS_Encoding);
                _regHelper.ReadGPRegisterList();
                _regHelper.ReadVFPURegisterAliasList();
                _regHelper.ReadCop0RegisterList();
                _regHelper.ReadGTEControlRegisterList();
                _regHelper.ReadGTEDataRegisterList();
            }

			_asmEncoder = new ASMEncoder(_pseudoHelper, _valueHelper, _formatHelper, _regHelper);
			_asmDecoder = new ASMDecoder(_formatHelper, _regHelper);

            _asmCheckHelper = new ASMCheckHelper(_asmEncoder, _asmDecoder);
		}

        public ASMEncodingUtilityHelper(ASMEncodingUtilityHelper utilityHelper)
        {
            _asmEncoder = new ASMEncoder(utilityHelper._asmEncoder);

            _pseudoHelper = _asmEncoder.PseudoHelper;
            _valueHelper = _pseudoHelper.ValueHelper;
            _labelHelper = _valueHelper.LabelHelper;

            _formatHelper = _labelHelper.FormatHelper;
            _regHelper = utilityHelper._regHelper;

            _asmDecoder = new ASMDecoder(_formatHelper, _regHelper);

            _asmCheckHelper = new ASMCheckHelper(_asmEncoder, _asmDecoder);
        }

        public void LoadEncodingModeFiles(int encodingMode)
        {
            switch (encodingMode)
            {
                case ASMEncodingMode.Base: 
                    LoadEncodingFile(ASMDataFileMap.MIPS_Encoding); 
                    break;
                case ASMEncodingMode.PSX: 
                    LoadEncodingFile(ASMDataFileMap.PSX_Encoding);
                    break;
                case ASMEncodingMode.PSP:
                    LoadEncodingFile(ASMDataFileMap.MIPS_R2_Encoding);
                    LoadEncodingFile(ASMDataFileMap.MIPS_FPU_Float_Encoding);
                    LoadEncodingFile(ASMDataFileMap.PSP_Encoding);
                    break;
                default: break;
            }
        }

        public void LoadEncodingFile(string filepath)
        {
            _formatHelper.ReadEncodeList(filepath);
        }

        public void LoadGPRegisterFile()
        {
            _regHelper.ReadGPRegisterList();
        }

        public void LoadVFPURegisterAliasList()
        {
            _regHelper.ReadVFPURegisterAliasList();
        }

        public void LoadCop0RegisterFile()
        {
            _regHelper.ReadCop0RegisterList();
        }

        public void LoadGTEControlRegisterFile()
        {
            _regHelper.ReadGTEControlRegisterList();
        }

        public void LoadGTEDataRegisterFile()
        {
            _regHelper.ReadGTEDataRegisterList();
        }

		public ASMEncoderResult EncodeASM(string asm, string pcText, string spacePadding, bool includeAddress, bool littleEndian)
		{
			//uint pc = ASMPCHelper.ProcessPC(0, pcText);
			return _asmEncoder.EncodeASM(asm, pcText, spacePadding, includeAddress, littleEndian);
		}
		
		public ASMEncoderResult EncodeASM(string asm, uint pc, string spacePadding, bool includeAddress, bool littleEndian)
		{
			return _asmEncoder.EncodeASM(asm, pc, spacePadding, includeAddress, littleEndian, true);
		}
		
		public ASMEncoderResult EncodeASM(string asm, uint pc)
		{
			return _asmEncoder.EncodeASM(asm, pc, "", false, true, true);
		}
		
		public ASMDecoderResult DecodeASM(string hex, string pcText, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases)
		{
			//uint pc = ASMPCHelper.ProcessPC(0, pcText);
			return _asmDecoder.DecodeASM(hex, pcText, spacePadding, littleEndian, includeAddress, useRegAliases);
		}
		
		public ASMDecoderResult DecodeASM(string hex, uint pc, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases)
		{
			return _asmDecoder.DecodeASM(hex, pc, spacePadding, littleEndian, includeAddress, useRegAliases, true);
		}

        public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, string pcText)
        {
            //uint pc = ASMPCHelper.ProcessPC(0, pcText);
            return _asmDecoder.DecodeASMToFile(inputFilename, outputFilename, littleEndian, useRegAliases, pcText);
        }

		public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, uint pc)
		{
			return _asmDecoder.DecodeASMToFile(inputFilename, outputFilename, littleEndian, useRegAliases, pc);
		}

        public ASMCheckResult CheckASM(string asm, string pcText, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASM(asm, pcText, littleEndian, useRegAliases, reEncode, conditions);
        }

        public ASMCheckResult CheckASM(string asm, uint pc, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASM(asm, pc, littleEndian, useRegAliases, reEncode, conditions);
        }

        public ASMCheckResult CheckASMFromHex(string hex, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASMFromHex(hex, pcText, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromHex(string hex, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASMFromHex(hex, pc, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASMFromBytes(bytes, pcText, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            return _asmCheckHelper.CheckASMFromBytes(bytes, pc, littleEndian, useRegAliases, conditions);
        }

        public byte[] UpdateBlockReferences(byte[] bytes, uint pc, bool littleEndian, IEnumerable<BlockMove> blockMoves)
        {
            return _asmCheckHelper.UpdateBlockReferences(bytes, pc, littleEndian, blockMoves);
        }

        public string ReplaceLabelsInHex(string hex, bool littleEndian, bool skipAssertion = false)
        {
            return _asmEncoder.ReplaceLabelsInHex(hex, littleEndian, skipAssertion);
        }

        public static uint ProcessStartPC(string asm, string pcText)
        {
            return ASMPCHelper.ProcessStartPC(asm, pcText);
        }

        public static uint ProcessStartPC(string asm, uint pc)
        {
            return ASMPCHelper.ProcessStartPC(asm, pc);
        }
	}
}
