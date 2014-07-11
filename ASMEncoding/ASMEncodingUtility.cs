
// Main ASM Utility API intended for use across multiple programs. Makes use of other ASM classes to provide functionality.

using System;
using System.Collections.Generic;
using System.Text;
using ASMEncoding.Helpers;

namespace ASMEncoding
{
    public class ASMEncodingUtility
    {
        private ASMEncodingUtilityHelper[] _helper;

        private int _encodingMode;
        public int EncodingMode 
        {
            get { return _encodingMode; }
            set
            {
                _encodingMode = value;

                if (_helper[_encodingMode] == null)
                {
                    _helper[_encodingMode] = new ASMEncodingUtilityHelper(_helper[ASMEncodingMode.Base]);
                    _helper[_encodingMode].LoadEncodingModeFile(_encodingMode);
                }
            }
        }

        public ASMEncodingUtility(int encodingMode = ASMEncodingMode.Base)
        {
            _helper = new ASMEncodingUtilityHelper[ASMEncodingMode.NumModes];
            _helper[ASMEncodingMode.Base] = new ASMEncodingUtilityHelper();

            EncodingMode = encodingMode;
        }

        public ASMEncoderResult EncodeASM(string asm, string pcText, string spacePadding, bool includeAddress, bool littleEndian)
        {
            return _helper[EncodingMode].EncodeASM(asm, pcText, spacePadding, includeAddress, littleEndian);
        }

        public ASMEncoderResult EncodeASM(string asm, uint pc, string spacePadding, bool includeAddress, bool littleEndian)
        {
            return _helper[EncodingMode].EncodeASM(asm, pc, spacePadding, includeAddress, littleEndian);
        }

        public ASMEncoderResult EncodeASM(string asm, uint pc)
        {
            return _helper[EncodingMode].EncodeASM(asm, pc);
        }

        public ASMDecoderResult DecodeASM(string hex, string pcText, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases)
        {
            return _helper[EncodingMode].DecodeASM(hex, pcText, spacePadding, littleEndian, includeAddress, useRegAliases);
        }

        public ASMDecoderResult DecodeASM(string hex, uint pc, string spacePadding, bool littleEndian, bool includeAddress, bool useRegAliases)
        {
            return _helper[EncodingMode].DecodeASM(hex, pc, spacePadding, littleEndian, includeAddress, useRegAliases);
        }

        public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, string pcText)
        {
            return _helper[EncodingMode].DecodeASMToFile(inputFilename, outputFilename, littleEndian, useRegAliases, pcText);
        }

        public int DecodeASMToFile(string inputFilename, string outputFilename, bool littleEndian, bool useRegAliases, uint pc)
        {
            return _helper[EncodingMode].DecodeASMToFile(inputFilename, outputFilename, littleEndian, useRegAliases, pc);
        }
    }
}
