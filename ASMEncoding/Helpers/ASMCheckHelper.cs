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
        StackPointerOffset4 = 4,
        BranchInBranchDelaySlot = 5,
        LoadInBranchDelaySlot = 6,
        StackPointerOffset8 = 7
    }
}

namespace ASMEncoding.Helpers
{
    public class BlockMove
    {
        public uint Location { get; set; }
        public uint EndLocation { get; set; }
        public long Offset { get; set; }

        public BlockMove() { }
        public BlockMove(uint Location, uint EndLocation, long Offset)
        {
            this.Location = Location;
            this.EndLocation = EndLocation;
            this.Offset = Offset;
        }

        public bool IsEqual(BlockMove blockMove)
        {
            return ((blockMove.Location == Location) && (blockMove.EndLocation == EndLocation) && (blockMove.Offset == Offset));
        }
    }

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
                ASMCheckCondition.StackPointerOffset4,
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

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, string pcText, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions = null)
        {
            _errorTextBuilder = new StringBuilder();
            ASMProcessPCResult result = ASMPCHelper.ProcessPC(0, pcText, true, true);
            uint pc = result.PC;
            _errorTextBuilder.Append(result.ErrorMessage);
            return CheckASMFromBytes(bytes, pc, littleEndian, useRegAliases, conditions, false);
        }

        public ASMCheckResult CheckASMFromBytes(byte[] bytes, uint pc, bool littleEndian, bool useRegAliases, ICollection<ASMCheckCondition> conditions, bool clearErrorText = true)
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
            bool isLastCommandUnconditionalJumpDelaySlot = false;

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
                
                string strPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(pc, 8);
                string strArgs = parts[1];

                if (!string.IsNullOrEmpty(strArgs))
                {
                    string[] args = strArgs.Split(',');

                    if (conditions.Contains(ASMCheckCondition.LoadDelay))
                    {
                        if (!isLastCommandUnconditionalJumpDelaySlot)
                            CheckLoadDelay(command, args, gprLoad, strPC);
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
                                uint endPC = startPC + ((uint)(lines.Length - 1) * 4);
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
                                            CheckLoadDelay(branchParts[0], branchArgs, gprLoad, strBranchPC);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if ((conditions.Contains(ASMCheckCondition.StackPointerOffset4)) || ((conditions.Contains(ASMCheckCondition.StackPointerOffset8))))
                    {
                        if (IsAddImmediateCommand(command))
                        {
                            string stackPointerRegName = Encoder.RegHelper.GetGPRegisterName(29, useRegAliases).ToLower().Trim();
                            if ((args[0].ToLower().Trim() == stackPointerRegName) && (args[1].ToLower().Trim() == stackPointerRegName))
                            {
                                uint immed = Encoder.ValueHelper.GetAnyUnsignedValue(args[2].ToLower().Trim());
                                if ((conditions.Contains(ASMCheckCondition.StackPointerOffset4)) && ((immed % 4) != 0))
                                {
                                    _errorTextBuilder.AppendLine("WARNING: Stack pointer offset not a multiple of 4 at address " + strPC);
                                }
                                else if ((conditions.Contains(ASMCheckCondition.StackPointerOffset8)) && ((immed % 8) != 0))
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

                // Check if we're jumping into a hazard
                if (isLastCommandBranch)
                {
                    uint endPC = startPC + ((uint)(lines.Length - 1) * 4);
                    uint branchPC = lastBranchAddress;
                    string strBranchPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(branchPC, 8);

                    if ((startPC <= branchPC) && (branchPC <= endPC))
                    {
                        int index = (int)((branchPC - startPC) / 4);
                        string branchLine = lines[index];
                        string[] branchParts = ASMStringHelper.SplitLine(branchLine);
                        if (branchParts.Length > 1)
                        {
                            string strCommand = branchParts[0];
                            string strBranchArgs = branchParts[1];
                            if (!string.IsNullOrEmpty(strBranchArgs))
                            {
                                string[] branchArgs = strBranchArgs.Split(',');

                                if (conditions.Contains(ASMCheckCondition.LoadDelay))
                                {
                                    if ((IsLoadCommand(strCommand)) && ((branchPC + 4) <= endPC))
                                    {
                                        string secondBranchLine = lines[index + 1];
                                        string[] secondBranchParts = ASMStringHelper.SplitLine(secondBranchLine);
                                        if (secondBranchParts.Length > 1)
                                        {
                                            string strSecondBranchArgs = secondBranchParts[1];
                                            if (!string.IsNullOrEmpty(strSecondBranchArgs))
                                            {
                                                string[] secondBranchArgs = strSecondBranchArgs.Split(',');
                                                string strSecondBranchPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(branchPC + 4, 8);
                                                string branchGprLoad = branchArgs[0].Trim();
                                                CheckLoadDelay(secondBranchParts[0], secondBranchArgs, branchGprLoad, strSecondBranchPC);
                                            }
                                        }
                                    }
                                }

                                if (conditions.Contains(ASMCheckCondition.MultCountdown))
                                {
                                    string strCommandLower = strCommand.ToLower();
                                    if ((strCommandLower == "mfhi") || (strCommandLower == "mflo"))
                                    {
                                        if (((branchPC + 4) <= endPC))
                                        {
                                            string nextBranchLine = lines[index + 1];
                                            string[] nextBranchParts = ASMStringHelper.SplitLine(nextBranchLine);
                                            if (nextBranchParts.Length > 0)
                                            {
                                                string nextBranchCommand = nextBranchParts[0].ToLower();
                                                bool isMultiplication = ((nextBranchCommand == "mult") || (nextBranchCommand == "multu"));
                                                bool isDivision = ((nextBranchCommand == "div") || (nextBranchCommand == "divu"));
                                                if ((isMultiplication) || (isDivision))
                                                {
                                                    string operation = isMultiplication ? "Multiplication" : "Division";
                                                    string strNextPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(branchPC + 4, 8);
                                                    _errorTextBuilder.AppendLine("WARNING: " + operation + " within 2 commands of " + strCommandLower + " at address " + strNextPC);
                                                }
                                            }
                                        }

                                        if (((branchPC + 8) <= endPC))
                                        {
                                            string nextBranchLine = lines[index + 2];
                                            string[] nextBranchParts = ASMStringHelper.SplitLine(nextBranchLine);
                                            if (nextBranchParts.Length > 0)
                                            {
                                                string nextBranchCommand = nextBranchParts[0].ToLower();
                                                bool isMultiplication = ((nextBranchCommand == "mult") || (nextBranchCommand == "multu"));
                                                bool isDivision = ((nextBranchCommand == "div") || (nextBranchCommand == "divu"));
                                                if ((isMultiplication) || (isDivision))
                                                {
                                                    string operation = isMultiplication ? "Multiplication" : "Division";
                                                    string strNextPC = "0x" + ASMValueHelper.UnsignedToHex_WithLength(branchPC + 8, 8);
                                                    _errorTextBuilder.AppendLine("WARNING: " + operation + " within 2 commands of " + strCommandLower + " at address " + strNextPC);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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

                if (isLastCommandUnconditionalJump)
                {
                    hiLoCountdown = 0;
                }
                else if ((commandLower == "mfhi") || (commandLower == "mflo"))
                {
                    hiLoCountdown = 2;
                    hiLoCommand = commandLower;
                }
                else if (hiLoCountdown > 0)
                {
                    hiLoCountdown--;
                }

                isLastCommandBranch = isBranch;
                isLastCommandUnconditionalJumpDelaySlot = isLastCommandUnconditionalJump;
                isLastCommandUnconditionalJump = isUnconditionalJump;

                pc += 4;
            }

            return new ASMCheckResult
            {
                ErrorText = _errorTextBuilder.ToString(),
                IsASM = isASM
            };
        }

        public byte[] UpdateJumps(byte[] bytes, uint pc, bool littleEndian, IEnumerable<BlockMove> blockMoves)
        {
            int byteCount = bytes.Length;
            if (byteCount < 4)
                return bytes;

            byte[] resultBytes = new byte[byteCount];
            int startIndex = 0;
            byte[] asmBytes = bytes;

            if (byteCount > 4)
            {
                uint offsetBytes = pc % 4;
                if (offsetBytes != 0)
                {
                    uint skipBytes = 4 - offsetBytes;
                    pc = pc + skipBytes;
                    startIndex += (int)skipBytes;
                    int length = (int)(bytes.Length - skipBytes);
                    byte[] newBytes = new byte[length];
                    Array.Copy(bytes, skipBytes, newBytes, 0, length);
                    Array.Copy(bytes, 0, resultBytes, 0, startIndex);
                    asmBytes = newBytes;
                }
            }

            uint[] instructions = ASMValueHelper.GetUintArrayFromBytes(asmBytes, littleEndian);
            
            int numInstructions = instructions.Length;
            uint[] newInstructions = new uint[numInstructions];

            for (int index = 0; index < numInstructions; index++)
            {
                uint uBinaryLine = instructions[index];
                uint opcode = (uBinaryLine >> 26);
                uint newInstruction = uBinaryLine;

                // Is unconditional jump literal command J or JAL
                if ((opcode & 0x3E) == 0x02)  // ((opcode & 0b111110) == 0b000010)
                {
                    uint jumpAddress = (((uBinaryLine & 0x03FFFFFFU) << 2) | (pc & 0xF0000000U));

                    foreach (BlockMove blockMove in blockMoves)
                    {
                        if ((jumpAddress >= blockMove.Location) && (jumpAddress < blockMove.EndLocation))
                        {
                            uint newJumpAddress = (uint)(jumpAddress + blockMove.Offset);
                            newInstruction = (opcode << 26) | ((newJumpAddress >> 2) & 0x03FFFFFFU);
                        }
                    }
                }

                byte[] newBytes = ASMValueHelper.ConvertUIntToBytes(newInstruction, littleEndian);
                Array.Copy(newBytes, 0, resultBytes, (index * 4) + startIndex, 4);
            }

            for (int index = (numInstructions * 4) + startIndex; index < byteCount; index++)
                resultBytes[index] = bytes[index];

            return resultBytes;
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

        private string[] GetASMLinesFromBytes(byte[] bytes, uint pc, bool littleEndian, bool useRegAliases)
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

        private string GetASMTextFromBytes(byte[] bytes, uint pc, bool littleEndian, bool useRegAliases)
        {
            if (bytes.Length > 4)
            {
                uint offsetBytes = pc % 4;
                if (offsetBytes != 0)
                {
                    uint skipBytes = 4 - offsetBytes;
                    pc = pc + skipBytes;
                    int length = (int)(bytes.Length - skipBytes);
                    byte[] newBytes = new byte[length];
                    Array.Copy(bytes, skipBytes, newBytes, 0, length);
                    bytes = newBytes;
                }
            }

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

        private bool IsUnconditionalJumpToLiteral(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "j":
                case "jal":
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

        private bool IsFirstArgWritebackCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return false;

            if (IsLoadCommand(command))
                return true;

            string commandLower = command.ToLower().Trim();
            switch (commandLower)
            {
                case "add":
                case "addi":
                case "addiu":
                case "addu":
                case "and":
                case "andi":
                case "div":
                case "divu":
                case "lui":
                case "mfhi":
                case "mflo":
                case "mult":
                case "multu":
                case "nor":
                case "or":
                case "ori":
                case "sll":
                case "sllv":
                case "slt":
                case "slti":
                case "sltiu":
                case "sltu":
                case "sra":
                case "srav":
                case "srl":
                case "srlv":
                case "sub":
                case "subu":
                case "xor":
                case "xori":

                case "cfc0":
                case "mfc0":
                case "cfc2":
                case "mfc2":
                case "lwc2":
                    return true;
                default:
                    return false;
            }
        }

        private void CheckLoadDelay(string command, string[] args, string gprLoad, string strPC)
        {
            bool foundLoadDelay = false;
            //bool isLoad = IsLoadCommand(command);
            bool isFirstArgWriteback = IsFirstArgWritebackCommand(command);
            bool isFirstArg = true;

            foreach (string arg in args)
            {
                if (isFirstArg)
                {
                    isFirstArg = false;
                    if (isFirstArgWriteback) //if (isLoad) 
                        continue;
                }

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
