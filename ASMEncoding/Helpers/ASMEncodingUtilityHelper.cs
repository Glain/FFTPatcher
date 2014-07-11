/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 13:14
 */
using System;
using ASMEncoding.Helpers;

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
            }

			_asmEncoder = new ASMEncoder(_pseudoHelper, _valueHelper, _formatHelper, _regHelper);
			_asmDecoder = new ASMDecoder(_formatHelper, _regHelper);
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
        }

        public void LoadEncodingModeFile(int encodingMode)
        {
            string filePath = "";
            switch (encodingMode)
            {
                case ASMEncodingMode.Base: filePath = ASMDataFileMap.MIPS_Encoding; break;
                case ASMEncodingMode.PSX: filePath = ASMDataFileMap.PSX_Encoding; break;
                case ASMEncodingMode.PSP: filePath = ASMDataFileMap.PSP_Encoding; break;
                default: break;
            }

            LoadEncodingFile(filePath);
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
	}
}
