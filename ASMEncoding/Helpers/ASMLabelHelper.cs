/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/17/2012
 * Time: 0:12
 *
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace ASMEncoding.Helpers
{
    public class ASMAddLabelResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public uint PC { get; set; }
    }

    public class ASMFindLabelsResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

	public class ASMLabelHelper
	{
        public static string PersistentLabelPrefix = "@";

        private StringBuilder _errorTextBuilder;

		public Dictionary<string,uint> LabelDict { get; set; }
        public Dictionary<string,uint> PersistentLabelDict { get; set; }
		public ASMFormatHelper FormatHelper { get; set; }
		
		public ASMLabelHelper() 
		{ 
			LabelDict = new Dictionary<string, uint>();
		}

		public ASMLabelHelper(ASMFormatHelper formatHelper)
		{
			FormatHelper = formatHelper;
			LabelDict = new Dictionary<string, uint>();
            PersistentLabelDict = new Dictionary<string, uint>();
            _errorTextBuilder = new StringBuilder();
		}

        public ASMLabelHelper(ASMLabelHelper labelHelper)
        {
            LabelDict = new Dictionary<string, uint>();
            PersistentLabelDict = new Dictionary<string, uint>();
            _errorTextBuilder = new StringBuilder();

            FormatHelper = new ASMFormatHelper(labelHelper.FormatHelper);
        }

		public ASMFindLabelsResult FindLabels(string[] lines, EncodeLine[] encodeLines, uint pc)
		{
            ASMFindLabelsResult result = new ASMFindLabelsResult();
            result.ErrorCode = 0;

            _errorTextBuilder.Length = 0;

			//LabelDict.Clear();
            ClearLabelDict();
			//int pc = ProcessPC(0, txt_StartPC.Text);
			int lineIndex = 0;
			int encodeLineIndex = 0;
			
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
					continue;
				
				string processLine = ASMStringHelper.RemoveLeadingBracketBlock(line);
				processLine = ASMStringHelper.RemoveLeadingSpaces(processLine);
				processLine = ASMStringHelper.RemoveComment(processLine).ToLower();
				
				string[] parts = ASMStringHelper.SplitLine(processLine);
				
                //pc = ASMPCHelper.ProcessOrg(pc, parts);
                ASMProcessPCResult processPCResult = ASMPCHelper.ProcessOrg(pc, parts, false);
                pc = processPCResult.PC;
                _errorTextBuilder.Append(processPCResult.ErrorMessage);

                ASMAddLabelResult processLabelResult = ProcessLabelStatement(parts);
                if (processLabelResult != null)
                {
                    if (processLabelResult.ErrorCode > 0)
                    {
                        result.ErrorCode = 1;
                        _errorTextBuilder.Append(processLabelResult.ErrorMessage);
                    }
                }

				EncodingFormat encodingOrNull = FormatHelper.FindFormatByCommand(parts[0]);
				if (encodingOrNull != null)
				{
					EncodeLine eLine = new EncodeLine();
					if ((encodeLines.Length > 0) && (encodeLineIndex < encodeLines.Length))
						eLine = encodeLines[encodeLineIndex];
					
					while ((eLine.LineIndex == lineIndex) && (encodeLineIndex < encodeLines.Length))
					{
						pc += 4;
						encodeLineIndex++;
					
						if (encodeLineIndex < encodeLines.Length)
							eLine = encodeLines[encodeLineIndex];
					}
					
					lineIndex++;
				}
				else
				{
                    ASMAddLabelResult addLabelResult = AddLabel(pc, parts);

                    if (addLabelResult.ErrorCode == 0)
                    {
                        pc = addLabelResult.PC;
                    }
                    else
                    {
                        _errorTextBuilder.Append(addLabelResult.ErrorMessage);
                        result.ErrorCode = 1;
                    }
				}
			}

            result.ErrorMessage = _errorTextBuilder.ToString();
            return result;
		}

        private ASMAddLabelResult ProcessLabelStatement(string[] parts)
        {
            if (!string.IsNullOrEmpty(parts[0]))
            {
                if (parts[0] == ".label")
                {
                    try
                    {
                        string[] innerParts = parts[1].Split(',');
                        if (!parts[1].Contains(","))
                        {
                            ASMAddLabelResult errorResult = new ASMAddLabelResult();
                            errorResult.ErrorCode = 1;
                            errorResult.ErrorMessage = "WARNING: Ignoring .label statement with bad argument list (no comma): \"" + parts[1] + "\"\r\n";
                            return errorResult;
                        }

                        string label = ASMStringHelper.RemoveSpaces(innerParts[0]);
                        uint pc = ASMValueHelper.FindUnsignedValueGeneric(ASMStringHelper.RemoveSpaces(innerParts[1])).Value;
                        return AddLabelGeneric(pc, label);
                    }
                    catch (Exception ex)
                    {
                        ASMAddLabelResult errorResult = new ASMAddLabelResult();
                        errorResult.ErrorCode = 1;
                        errorResult.ErrorMessage = "Error on .label statement: " + ex.Message + "\r\n";
                        return errorResult;
                    }
                }
            }

            return null;
        }

		private ASMAddLabelResult AddLabel(uint pc, string[] parts)
		{
            ASMAddLabelResult result = new ASMAddLabelResult();

			if (ASMStringHelper.RemoveSpaces(parts[0]).EndsWith(":"))
			{
				string preLabel = ASMStringHelper.RemoveSpaces(parts[0]).ToUpper();
				string label = preLabel.Substring(0,preLabel.Length-1);

                result = AddLabelGeneric(pc, label);
				
				// Is there an ASM command on this line? If so, advance the PC
				if (!string.IsNullOrEmpty(parts[1]))
				{
					parts = ASMStringHelper.RemoveLabel(parts);
					
					// If this is an ASM command, advance the PC
                    EncodingFormat curEncodingOrNull = FormatHelper.FindFormatByCommand(parts[0]);
					if (curEncodingOrNull != null)
					{	
						pc += 4;
					}
				}
			}

            //result.ErrorCode = 0;
            result.PC = pc;

            return result;
		}

        private ASMAddLabelResult AddLabelGeneric(uint pc, string label)
        {
            ASMAddLabelResult result = new ASMAddLabelResult();

            try
            {
                label = ASMStringHelper.RemoveSpaces(label).ToUpper();
                if (!PersistentLabelDict.ContainsKey(label))
                {
                    LabelDict.Add(label, pc);

                    if (label.StartsWith(PersistentLabelPrefix))
                        PersistentLabelDict.Add(label, pc);
                }
                else
                {
                    PersistentLabelDict[label] = pc;
                    LabelDict[label] = pc;
                }
            }
            catch
            {
                result.ErrorCode = 1;
                result.ErrorMessage = "Error adding label " + label + ": Label already exists!\r\n";
                return result;
            }

            result.ErrorCode = 0;
            result.PC = pc;

            return result;
        }

        public void ClearLabelDict()
        {
            LabelDict.Clear();
            foreach (KeyValuePair<string,uint> pair in PersistentLabelDict)
            {
                LabelDict.Add(pair.Key, pair.Value);
            }
        }

		public string LabelToHex(string label, int reqLength)
		{
			return ASMValueHelper.UnsignedToHex_WithLength(LabelToUnsigned(label),reqLength);
		}
		
        /*
		public int LabelToUnsigned(string label)
		{
			string newLabel = ASMStringHelper.RemoveSpaces(label).ToUpper();
			ASMDebugHelper.assert(LabelDict.ContainsKey(newLabel), "Label not found: " + label);
			return LabelDict[newLabel];
		}
        */

        public uint GetAnyUnsignedValue(string val, bool skipLabelAssertion = false)
        {
            if ((val.StartsWith("0x")) || (val.StartsWith("-0x")))
                return ASMValueHelper.HexToUnsigned_AnySign(val, 32);
            else if (ASMStringHelper.StringIsNumeric(val))
                return Convert.ToUInt32(val);
            else if ((val.StartsWith("-")) && (val.Length > 1))
            {
                string str_uvalue = val.Substring(1);
                bool isNumeric = ASMStringHelper.StringIsNumeric(str_uvalue);
                ASMDebugHelper.assert(isNumeric, "Could not parse negative value: " + val);
                if (isNumeric)
                {
                    uint uvalue = Convert.ToUInt32(str_uvalue);
                    if (uvalue == 0)
                        return 0;
                    else
                        return (uint)(0x100000000 - uvalue);
                }
                else
                {
                    return 0;
                }
            }
            else
                return LabelToUnsigned(val, skipLabelAssertion);
        }
				
		public uint LabelToUnsigned(string label, bool skipAssertion = false)
		{
			string newLabel = ASMStringHelper.RemoveSpaces(label).ToUpper();
            string lowerLabel = newLabel.ToLower();

            if (((lowerLabel.StartsWith("%hi(")) || (lowerLabel.StartsWith("%lo("))) && (lowerLabel.Substring(3).Contains(")")))
            {
                string newValue = lowerLabel.Substring(4, lowerLabel.IndexOf(")") - 4).Trim();
                uint uValue = GetAnyUnsignedValue(newValue, skipAssertion);
                return (lowerLabel.StartsWith("%hi(") ? ProcessHi(uValue) : ProcessLo(uValue));
            }

            if (!skipAssertion)
                ASMDebugHelper.assert(LabelDict.ContainsKey(newLabel), "Label not found: " + label);

			return (uint) (LabelDict.ContainsKey(newLabel) ? LabelDict[newLabel] : 0);
		}

        private uint ProcessHi(uint uValue)
        {
            uint rawHiValue = (uValue >> 16) & 0xffff;
            return (((uValue & 0x8000) == 0) ? (rawHiValue) : (rawHiValue + 1));
        }

        private uint ProcessLo(uint uValue)
        {
            return uValue & 0xffff;
        }

        public string ReplaceLabelsInHex(string hex, bool littleEndian, bool replaceAll = false)
        {
            string result = hex.ToUpper();

            List<string> labels = new List<string>(LabelDict.Keys);
            labels.Sort((a, b) => a.Length.CompareTo(b.Length));
            labels.Reverse();

            foreach (string label in labels)
            {
                uint labelValue = LabelToUnsigned(label);
                labelValue = littleEndian ? ASMValueHelper.ReverseBytes(labelValue) : labelValue;
                string labelHex = ASMValueHelper.UnsignedToHex_WithLength(labelValue, 8).ToUpper();
                result = result.Replace(label, labelHex);
            }

            if (replaceAll)
            {
                int persistentLabelIndex = result.IndexOf(PersistentLabelPrefix);
                HashSet<char> endChars = new HashSet<char>() { ' ', '\t', '\r', '\n' };

                while (persistentLabelIndex != -1)
                {
                    int endIndex = persistentLabelIndex;
                    for (; endIndex < result.Length; endIndex++)
                    {
                        if (endChars.Contains(result[endIndex]))
                            break;
                    }

                    string label = (endIndex < result.Length) ? result.Substring(persistentLabelIndex, endIndex - persistentLabelIndex) : result.Substring(persistentLabelIndex);
                    result = result.Replace(label, "00000000");

                    persistentLabelIndex = result.IndexOf(PersistentLabelPrefix);
                }
            }

            return result;
        }
	}
}
