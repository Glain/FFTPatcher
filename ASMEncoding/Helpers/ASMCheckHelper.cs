using System;
using System.Collections.Generic;
using System.Text;

namespace ASMEncoding
{
    public class ASMCheckResult
    {
        public bool IsASM { get; set; }
        public string ErrorText { get; set; }

        public ASMCheckResult() { }
    }

    public enum ASMCheckCondition
    {
        LoadDelay = 1,
        UnalignedOffset = 2,
        MultCountdown = 3,
        StackPointerOffset = 4,
        BranchInBranchDelaySlot = 5,
        LoadInBranchDelaySlot = 6
    }
}

namespace ASMEncoding.Helpers
{
    public class ASMCheckHelper
    {
        private StringBuilder _errorTextBuilder;

        public ASMEncoder Encoder { get; private set; }
        public ASMDecoder Decoder { get; private set; }

        //private HashSet<ASMCheckCondition> allCheckConditions = GetAllCheckConditions();
        private HashSet<ASMCheckCondition> standardCheckConditions = GetStandardCheckConditions();

        /*
        public static HashSet<ASMCheckCondition> GetAllCheckConditions()
        {
            return new HashSet<ASMCheckCondition>()
            {
                ASMCheckCondition.LoadDelay,
                ASMCheckCondition.UnalignedOffset,
                ASMCheckCondition.MultCountdown,
                ASMCheckCondition.StackPointerOffset,
                ASMCheckCondition.BranchInBranchDelaySlot,
                ASMCheckCondition.LoadInBranchDelaySlot
            };
        }
        */

        public static HashSet<ASMCheckCondition> GetStandardCheckConditions()
        {
            return new HashSet<ASMCheckCondition>()
            {
                ASMCheckCondition.LoadDelay,
                ASMCheckCondition.UnalignedOffset,
                ASMCheckCondition.MultCountdown,
                ASMCheckCondition.StackPointerOffset,
                ASMCheckCondition.BranchInBranchDelaySlot
            };
        }

        public ASMCheckHelper(ASMEncoder encoder, ASMDecoder decoder)
        {
            Encoder = encoder;
            Decoder = decoder;
        }

        public ASMCheckResult CheckASM(string asm, string pcText, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null)
        {
            _errorTextBuilder = new StringBuilder();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return CheckASM(asm, pc, littleEndian, useRegAliases, reEncode, conditions, false);
        }

        public ASMCheckResult CheckASM(string asm, uint pc, bool littleEndian, bool useRegAliases, bool reEncode = true, ICollection<ASMCheckCondition> conditions = null, bool clearErrorText = true)
        {
            if (clearErrorText)
                _errorTextBuilder = new StringBuilder();

            pc = ASMPCHelper.ProcessStartPC(asm, pc);
            string[] lines = GetASMLines(asm, pc, littleEndian, useRegAliases, reEncode);
            return CheckASM(lines, pc, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromHex(string hex, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            _errorTextBuilder = new StringBuilder();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return CheckASMFromHex(hex, pc, littleEndian, useRegAliases, conditions, false);
        }

        public ASMCheckResult CheckASMFromHex(string hex, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null, bool clearErrorText = true)
        {
            if (clearErrorText)
                _errorTextBuilder = new StringBuilder();

            string[] lines = GetASMLinesFromHex(hex, pc, littleEndian, useRegAliases);
            return CheckASM(lines, pc, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASMFromBytes(IEnumerable<byte> bytes, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            _errorTextBuilder = new StringBuilder();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return CheckASMFromBytes(bytes, pc, littleEndian, useRegAliases, conditions, false);
        }

        public ASMCheckResult CheckASMFromBytes(IEnumerable<byte> bytes, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions, bool clearErrorText = true)
        {
            if (clearErrorText)
                _errorTextBuilder = new StringBuilder();

            string[] lines = GetASMLinesFromBytes(bytes, pc, littleEndian, useRegAliases);
            return CheckASM(lines, pc, littleEndian, useRegAliases, conditions);
        }

        public ASMCheckResult CheckASM(string[] lines, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            if (conditions == null)
            {
                conditions = standardCheckConditions;
            }

            bool isASM = (_errorTextBuilder.Length == 0);

            uint startPC = pc;
            string gprLoad = "";
            int hiLoCountdown = 0;
            string hiLoCommand = "";
            bool isLastCommandBranch = false;
            uint lastBranchAddress = 0;

            bool isLastCommandUnconditionalJump = false;
            bool isLastCommandLoadAfterUnconditionalJump = false;

            foreach (string line in lines)
            {
                string[] parts = ASMStringHelper.SplitLine(line);
                if (parts.Length < 1)
                    continue;

                string command = parts[0];
                if (string.IsNullOrEmpty(command))
                    continue;

                string commandLower = command.ToLower().Trim();
                bool isLoad = IsLoadCommand(command);
                bool isBranch = IsBranchCommand(command);
                bool isUnconditionalJump = IsUnconditionalJump(command);
                bool skipLoadDelayCheck = false;
                
                string strPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8);
                string strArgs = parts[1];

                if (isLastCommandLoadAfterUnconditionalJump)
                {
                    isLastCommandLoadAfterUnconditionalJump = false;
                    skipLoadDelayCheck = true;
                }

                if (!string.IsNullOrEmpty(strArgs))
                {
                    string[] args = strArgs.Split(',');

                    if (conditions.Contains(ASMCheckCondition.LoadDelay))
                    {
                        if (!skipLoadDelayCheck)
                            CheckLoadDelay(args, gprLoad, strPC);
                    }

                    if (conditions.Contains(ASMCheckCondition.UnalignedOffset))
                    {
                        int alignmentMultiple = GetAlignmentMultiple(command);
                        if (alignmentMultiple > 1)
                        {
                            string strOffset = args[1].Substring(0, args[1].IndexOf("("));
                            uint offset = Encoder.ValueHelper.GetAnyUnsignedValue(strOffset);
                            if ((offset % alignmentMultiple) != 0)
                            {
                                _errorTextBuilder.AppendLine("WARNING: Unaligned offset at address " + strPC);
                            }
                        }
                    }

                    if (conditions.Contains(ASMCheckCondition.MultCountdown))
                    {
                        bool isMultiplication = ((commandLower == "mult") || (commandLower == "multu"));
                        bool isDivision = ((commandLower == "div") || (commandLower == "divu"));
                        if ((hiLoCountdown > 0) && ((isMultiplication) || (isDivision)))
                        {
                            string operation = isMultiplication ? "Multiplication" : "Division";
                            _errorTextBuilder.AppendLine("WARNING: " + operation + " within 2 commands of " + hiLoCommand + " at address " + strPC);
                        }
                    }

                    if (isLoad)
                    {
                        gprLoad = args[0].Trim();

                        if (isLastCommandBranch)
                        {
                            if (conditions.Contains(ASMCheckCondition.LoadInBranchDelaySlot))
                            {
                                _errorTextBuilder.AppendLine("WARNING: Load command in branch delay slot at address " + strPC);
                            }

                            // If there is a load in the branch delay slot, check if the branch target address tries to use the loaded value
                            if (conditions.Contains(ASMCheckCondition.LoadDelay))
                            {
                                uint endPC = startPC + ((uint)lines.Length * 4);
                                uint branchPC = lastBranchAddress;
                                string strBranchPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(branchPC, 8);

                                if ((startPC <= branchPC) && (branchPC <= endPC))
                                {
                                    int index = (int)((branchPC - startPC) / 4);
                                    string branchLine = lines[index];
                                    string[] branchParts = ASMStringHelper.SplitLine(branchLine);
                                    if (branchParts.Length > 1)
                                    {
                                        string strBranchArgs = branchParts[1];
                                        if (!string.IsNullOrEmpty(strBranchArgs))
                                        {
                                            string[] branchArgs = strBranchArgs.Split(',');
                                            CheckLoadDelay(branchArgs, gprLoad, strBranchPC);
                                        }
                                    }
                                }
                            }

                            if (isLastCommandUnconditionalJump)
                            {
                                isLastCommandLoadAfterUnconditionalJump = true;
                            }
                        }
                    }
                    
                    if (conditions.Contains(ASMCheckCondition.StackPointerOffset))
                    {
                        if (IsAddImmediateCommand(command))
                        {
                            string stackPointerRegName = Encoder.RegHelper.GetGPRegisterName(29, useRegAliases).ToLower().Trim();
                            if ((args[0].ToLower().Trim() == stackPointerRegName) && (args[1].ToLower().Trim() == stackPointerRegName))
                            {
                                uint immed = Encoder.ValueHelper.GetAnyUnsignedValue(args[2].ToLower().Trim());
                                if (immed % 8 != 0)
                                {
                                    _errorTextBuilder.AppendLine("WARNING: Stack pointer offset not a multiple of 8 at address " + strPC);
                                }
                            }
                        }
                    }

                    if (isBranch)
                    {
                        lastBranchAddress = FindBranchTargetAddress(args);
                    }
                }

                if (conditions.Contains(ASMCheckCondition.BranchInBranchDelaySlot))
                {
                    if (isBranch && isLastCommandBranch)
                    {
                        _errorTextBuilder.AppendLine("WARNING: Branch command in branch delay slot at address " + strPC);
                    }
                }

                if (!isLoad)
                {
                    gprLoad = "";
                }

                if ((commandLower == "mfhi") || (commandLower == "mflo"))
                {
                    hiLoCountdown = 2;
                    hiLoCommand = commandLower;
                }
                else if (hiLoCountdown > 0)
                {
                    hiLoCountdown--;
                }

                isLastCommandBranch = isBranch;
                isLastCommandUnconditionalJump = isUnconditionalJump;

                pc += 4;
            }

            return new ASMCheckResult
            {
                ErrorText = _errorTextBuilder.ToString(),
                IsASM = isASM
            };
        }

        private string[] GetASMLines(string asm, uint pc, bool littleEndian, bool useRegAliases, bool reEncode = true)
        {
            string asmText = reEncode ? GetASMText(asm, pc, littleEndian, useRegAliases) : asm;
            string[] lines = asmText.Split('\n');
            lines = ASMStringHelper.RemoveFromLines(lines, "\r");

            if (!reEncode)
            {
                List<string> newLines = new List<string>(lines.Length);
                
                foreach (string line in lines)
                {
                    string newLine = ASMStringHelper.RemoveLeadingBracketBlock(line);
                    newLine = ASMStringHelper.RemoveLeadingSpaces(newLine);
                    newLines.Add(newLine);
                }

                lines = newLines.ToArray();
            }

            return lines;
        }

        private string[] GetASMLinesFromHex(string hex, uint pc, bool littleEndian, bool useRegAliases)
        {
            string asmText = GetASMTextFromHex(hex, pc, littleEndian, useRegAliases);
            string[] lines = asmText.Split('\n');
            lines = ASMStringHelper.RemoveFromLines(lines, "\r");
            return lines;
        }

        private string[] GetASMLinesFromBytes(IEnumerable<byte> bytes, uint pc, bool littleEndian, bool useRegAliases)
        {
            string asmText = GetASMTextFromBytes(bytes, pc, littleEndian, useRegAliases);
            string[] lines = asmText.Split('\n');
            lines = ASMStringHelper.RemoveFromLines(lines, "\r");
            return lines;
        }

        private string GetASMText(string asm, uint pc, bool littleEndian, bool useRegAliases)
        {
            ASMEncoderResult encodeResult = Encoder.EncodeASM(asm, pc, "", false, littleEndian, true);
            _errorTextBuilder.Append(encodeResult.ErrorText);
            
            return GetASMTextFromHex(encodeResult.EncodedASMText, pc, littleEndian, useRegAliases);
        }

        private string GetASMTextFromHex(string hex, uint pc, bool littleEndian, bool useRegAliases)
        {
            ASMDecoderResult decodeResult = Decoder.DecodeASM(hex, pc, "", littleEndian, false, useRegAliases, true);
            _errorTextBuilder.Append(decodeResult.ErrorText);

            return decodeResult.DecodedASM;
        }

        private string GetASMTextFromBytes(IEnumerable<byte> bytes, uint pc, bool littleEndian, bool useRegAliases)
        {
            ASMDecoderResult decodeResult = Decoder.DecodeASM(bytes, pc, littleEndian, useRegAliases);
            _errorTextBuilder.Append(decodeResult.ErrorText);

            return decodeResult.DecodedASM;
        }

        private int GetAlignmentMultiple(string command)
        {
            if (string.IsNullOrEmpty(command))
                return 1;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "lbu":
                case "lb":
                case "sb":
                    return 1;
                case "lhu":
                case "lh":
                case "sh":
                    return 2;
                case "lw":
                case "sw":
                    return 4;
                case "ld":
                case "sd":
                    return 8;

                default:
                    return 1;
            }
        }

        private bool IsLoadCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "lbu":
                case "lb":
                case "lhu":
                case "lh":
                case "lw":
                case "lwl":
                case "lwr":
                case "ld":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsStoreCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "sb":
                case "sh":
                case "sw":
                case "swl":
                case "swr":
                case "sd":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsBranchCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "beq":
                case "bne":
                case "bltz":
                case "blez":
                case "bgtz":
                case "bgez":
                case "blezal":
                case "bltzal":
                case "bgezal":
                case "bgtzal":
                case "j":
                case "jr":
                case "jal":
                case "jalr":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsUnconditionalJump(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "j":
                case "jr":
                case "jal":
                case "jalr":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsAddImmediateCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            return ((command == "addi") || (command == "addiu"));
        }

        private void CheckLoadDelay(string[] args, string gprLoad, string strPC)
        {
            bool foundLoadDelay = false;
            foreach (string arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;

                if ((!foundLoadDelay) && (!string.IsNullOrEmpty(gprLoad)))
                {
                    string argLower = arg.ToLower();
                    if ((argLower == gprLoad) || argLower.Contains(gprLoad + ")"))
                    {
                        _errorTextBuilder.AppendLine("WARNING: Possible load delay issue with register " + gprLoad + " at address " + strPC);
                        foundLoadDelay = true;
                    }
                }
            }
        }

        private uint FindBranchTargetAddress(string[] args)
        {
            Nullable<uint> value = null;

            foreach (string arg in args)
            {
                value = ASMValueHelper.FindUnsignedValueGeneric(arg);
                if (value != null)
                    break;
            }

            return value.HasValue ? value.Value : 0;
        }
    }
}
