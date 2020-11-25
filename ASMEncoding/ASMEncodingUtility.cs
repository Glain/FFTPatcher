
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
                    //_helper[_encodingMode] = new ASMEncodingUtilityHelper(_helper[ASMEncodingMode.Base]);
                    _helper[_encodingMode] = new ASMEncodingUtilityHelper();
                    _helper[_encodingMode].LoadEncodingModeFiles(_encodingMode);
                }
            }
        }

        public ASMEncodingUtility(int encodingMode = ASMEncodingMode.Base)
        {
            _helper = new ASMEncodingUtilityHelper[ASMEncodingMode.NumModes];
            _helper[ASMEncodingMode.Base] = new ASMEncodingUtilityHelper();

            EncodingMode = encodingMode;
        }

        public HashSet<ASMCheckCondition> GetCheckConditions()
        {
            HashSet<ASMCheckCondition> resultSet = ASMCheckHelper.GetStandardCheckConditions();
            if (EncodingMode == ASMEncodingMode.PSP)
            {
                resultSet.Remove(ASMCheckCondition.LoadDelay);
                //resultSet.Remove(ASMCheckCondition.LoadInBranchDelaySlot);
            }
            return resultSet;
        }

        public ASMEncoderResult EncodeASM(string asm, string pcText, string spacePadding, bool includeAddress, bool littleEndian, bool skipLabelAssertion = false)
        {
            return _helper[EncodingMode].EncodeASM(asm, pcText, spacePadding, includeAddress, littleEndian, skipLabelAssertion);
        }

        public ASMEncoderResult EncodeASM(string asm, uint pc, string spacePadding, bool includeAddress, bool littleEndian, bool skipLabelAssertion = false)
        {
            return _helper[EncodingMode].EncodeASM(asm, pc, spacePadding, includeAddress, littleEndian, skipLabelAssertion);
        }

        public ASMEncoderResult EncodeASM(string asm, uint pc, bool skipLabelAssertion = false)
        {
            return _helper[EncodingMode].EncodeASM(asm, pc, skipLabelAssertion);
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

        public ASMCheckResult CheckASM(string asm, string pcText, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASM(asm, pcText, littleEndian, useRegAliases, reEncode, conditions);
        }

        public ASMCheckResult CheckASM(string asm, uint pc, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASM(asm, pc, littleEndian, useRegAliases, reEncode, conditions);
        }

        public ASMCheckResult CheckASMFromHex(string hex, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASMFromHex(hex, pcText, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromHex(string hex, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASMFromHex(hex, pc, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASMFromBytes(bytes, pcText, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            conditions = conditions ?? GetCheckConditions();
            return _helper[EncodingMode].CheckASMFromBytes(bytes, pc, littleEndian, useRegAliases, conditions);
        }

        public byte[] UpdateBlockReferences(byte[] bytes, uint pc, bool littleEndian, IEnumerable<BlockMove> blockMoves)
        {
            return _helper[EncodingMode].UpdateBlockReferences(bytes, pc, littleEndian, blockMoves);
        }

        public string ReplaceLabelsInHex(string hex, bool littleEndian, bool replaceAll = false)
        {
            return _helper[EncodingMode].ReplaceLabelsInHex(hex, littleEndian, replaceAll);
        }

        public static uint ProcessStartPC(string asm, string pcText)
        {
            return ASMEncodingUtilityHelper.ProcessStartPC(asm, pcText);
        }

        public static uint ProcessStartPC(string asm, uint pc)
        {
            return ASMEncodingUtilityHelper.ProcessStartPC(asm, pc);
        }
    }
}
