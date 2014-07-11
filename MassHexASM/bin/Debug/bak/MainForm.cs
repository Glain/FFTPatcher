/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 5/2/2011
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MassHexASM
{
	public struct EncodingFormat
	{
		public string command;
		public int type;
		public string format;
	}
	
	public enum EncodingType
	{
		R = 0,
		I = 1,
		J = 2
	}
		
	public partial class MainForm : Form
	{
		// Constants
		public static int REGISTER_ENCODE_BITS = 5;
		public static int SHIFT_AMOUNT_ENCODE_BITS = 5;
		
		// Lists of loaded data: encoding formats and registers
		List<EncodingFormat> encodeList = new List<EncodingFormat>();
		List<string> registerList = new List<string>();
		Dictionary<string, int> labelDict = new Dictionary<string, int>();
		
		public string _errorMessage = "";
		
		public MainForm()
		{
			// The InitializeComponent() call is required for Windows Forms designer support.
			InitializeComponent();
			Process();
		}
		
		// Invoked when the form is loaded.
		public void Process()
		{
			// Set properties of form
			txt_Messages.BackColor = SystemColors.Control;
			txt_Messages.ForeColor = Color.Red;
			txt_SpacePad.Enabled = false;
			chk_littleEndian.Checked = true;
			
			// Load lists from files
			ReadEncodeList();
			ReadRegisterList();
		}
		
		void Btn_EncodeClick(object sender, EventArgs e)
		{
			EncodeASM();
		}
		
		void Btn_DecodeClick(object sender, EventArgs e)
		{
			DecodeASM();
		}
		
		void Chk_SpacePadCheckedChanged(object sender, EventArgs e)
		{
			txt_SpacePad.Enabled = chk_SpacePad.Checked;	
		}
		
		void Txt_ASMKeyDown(object sender, KeyEventArgs e)
		{
			CauseSelectAll(txt_ASM,e);
		}
		
		void Txt_HexKeyDown(object sender, KeyEventArgs e)
		{
			CauseSelectAll(txt_Hex,e);
		}
		
		private void CauseSelectAll(TextBox box, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Control)
			{
				box.SelectAll();
			}
		}
		
		private void FindLabels(string[] lines)
		{
			labelDict.Clear();
			
			// PC is the Program Counter (address of current instruction)
			int pc = 0;
			try
			{
				if (!string.IsNullOrEmpty(txt_StartPC.Text))
				{
					if (txt_StartPC.Text.StartsWith("0x"))
					{
						if (txt_StartPC.Text.Length >= 10)
							pc = HexToUnsigned(txt_StartPC.Text.Substring(3,txt_StartPC.Text.Length-3));	
						else
							pc = HexToUnsigned(txt_StartPC.Text.Substring(2,txt_StartPC.Text.Length-2));	
					}
					else
						pc = Convert.ToInt32(txt_StartPC.Text);
				}
			}
			catch 
			{ 
				txt_Messages.Text = "Starting address invalid.";
			}
			
			/*
			// Split using the "\r\n" (CRLF) sequence (Separates lines, at least in Windows)
			char[] charArray = new char[2];
			charArray[0] = '\r';
			charArray[1] = '\n';
			string[] lines = txt_ASM.Text.Split(charArray);
			*/
			//string[] lines = txt_ASM.Text.Split('\n');
			
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
					continue;
				
				string processLine = RemoveBracketBlocks(line);
				processLine = RemoveLeadingSpaces(processLine);
				processLine = RemoveComment(processLine);
				
				// Split the line into parts based on the first space
				int spaceIndex = processLine.IndexOf(' ');
				int tabIndex = processLine.IndexOf('\t');
				if (((tabIndex >= 0) && (tabIndex < spaceIndex)) || (spaceIndex < 0))
					spaceIndex = tabIndex;
				string[] parts = new string[2];
				
				if (spaceIndex > -1)
					parts[0] = processLine.Substring(0,spaceIndex);
				else
					parts[0] = processLine;
				
				if ((processLine.Length > spaceIndex) && (spaceIndex > -1))
					parts[1] = processLine.Substring(spaceIndex+1,processLine.Length-spaceIndex-1);
				
				// Process .org statement: update program counter
				if (!string.IsNullOrEmpty(parts[0]))
				{
					if (parts[0] == ".org")
					{
						string strArg = RemoveSpaces(parts[1]);
						if (strArg.StartsWith("0x"))
						{
							if (strArg.Length >= 10)
								pc = HexToUnsigned(strArg.Substring(3,strArg.Length-3));	
							else
								pc = HexToUnsigned(strArg.Substring(2,strArg.Length-2));	
						}
						else
							pc = Convert.ToInt32(strArg);
					}
				}
				
				EncodingFormat? encodingOrNull = EncodingFormatListCommand(encodeList, parts[0]);
				if (encodingOrNull != null)
				{
					pc += 4;
				}
				else
				{
					if (RemoveSpaces(parts[0]).EndsWith(":"))
					{
						string preLabel = RemoveSpaces(parts[0]).ToUpper();
						string label = preLabel.Substring(0,preLabel.Length-1);
						try
						{
							assert(!labelDict.ContainsKey(RemoveSpaces(label.ToUpper())), "Label already exists!");
							labelDict.Add(label, pc);
						}
						catch 
						{
							txt_Messages.Text += "Error adding label '" + label + "': " + _errorMessage + "\r\n";
						}
						
						// Is there an ASM command on this line? If so, advance the PC
						if (!string.IsNullOrEmpty(parts[1]))
						{
							// Find the part after the label...
							string curLine = RemoveLeadingSpaces(parts[1]);
							curLine = RemoveComment(curLine);
							int curSpaceIndex = curLine.IndexOf(' ');
							
							if (curSpaceIndex > -1)
								parts[0] = curLine.Substring(0,curSpaceIndex);
							else
								parts[0] = curLine;
							
							if ((curLine.Length > curSpaceIndex) && (curSpaceIndex > -1))
								parts[1] = curLine.Substring(curSpaceIndex+1,curLine.Length-curSpaceIndex-1);
							
							// If this is an ASM command, advance the PC
							EncodingFormat? curEncodingOrNull = EncodingFormatListCommand(encodeList, parts[0]);
							if (curEncodingOrNull != null)
							{	
								pc += 4;
							}
						}
						
					}
				}
			}
		}
		
		private void EncodeASM()
		{
			/*		
			// Split using the "\r\n" (CRLF) sequence (Separates lines, at least in Windows)
			char[] charArray = new char[2];
			charArray[0] = '\r';
			charArray[1] = '\n';
			string[] lines = txt_ASM.Text.Split(charArray);
			*/
			
			string[] lines = txt_ASM.Text.Split('\n');
			
			string newTextASM = "";
			string newTextASMLine = "";
			txt_Hex.Text = "";
			txt_Messages.Text = "";
			string spacePadding = GetSpacePadding();
			FindLabels(lines);
			
			// PC is the Program Counter (address of current instruction)
			int pc = 0;
			try
			{
				if (!string.IsNullOrEmpty(txt_StartPC.Text))
				{
					if (txt_StartPC.Text.StartsWith("0x"))
					{
						if (txt_StartPC.Text.Length >= 10)
							pc = HexToUnsigned(txt_StartPC.Text.Substring(3,txt_StartPC.Text.Length-3));	
						else
							pc = HexToUnsigned(txt_StartPC.Text.Substring(2,txt_StartPC.Text.Length-2));	
					}
					else
						pc = Convert.ToInt32(txt_StartPC.Text);
				}
			}
			catch 
			{ 
				txt_Messages.Text = "Starting address invalid.";
			}
			
			foreach (string line in lines)
			{				
				if (string.IsNullOrEmpty(line))
					continue;
				
				newTextASMLine = "";
				
				string modLine = RemoveBracketBlocks(line);
				string origModLine = line.StartsWith("[") ? modLine.Substring(1,modLine.Length-1) : modLine.Substring(0,modLine.Length);
				string modLine2 = RemoveLeadingSpaces(modLine);
				string processLine = RemoveComment(modLine2);
				
				// Split the line into parts based on the first space
				int spaceIndex = processLine.IndexOf(' ');
				int tabIndex = processLine.IndexOf('\t');
				if (((tabIndex >= 0) && (tabIndex < spaceIndex)) || (spaceIndex < 0))
					spaceIndex = tabIndex;
				
				string[] parts = new string[2];
				
				if (spaceIndex > -1)
					parts[0] = processLine.Substring(0,spaceIndex);
				else
					parts[0] = processLine;
				
				if ((processLine.Length > spaceIndex) && (spaceIndex > -1))
					parts[1] = processLine.Substring(spaceIndex+1,processLine.Length-spaceIndex-1);
				
				// Process .org statement: update program counter
				if (!string.IsNullOrEmpty(parts[0]))
				{
					if (parts[0] == ".org")
					{
						string strArg = RemoveSpaces(parts[1]);
						if (strArg.StartsWith("0x"))
						{
							if (strArg.Length >= 10)
								pc = HexToUnsigned(strArg.Substring(3,strArg.Length-3));	
							else
								pc = HexToUnsigned(strArg.Substring(2,strArg.Length-2));	
						}
						else
							pc = Convert.ToInt32(strArg);
					}
				}
				
				// If this is an ASM command, pass off line to encoding routine
				EncodingFormat? encodingOrNull = EncodingFormatListCommand(encodeList, parts[0]);
				if (encodingOrNull != null)
				{
					EncodingFormat encoding = (EncodingFormat)encodingOrNull;
					
					try
					{
						txt_Hex.Text += EncodeASMSingle(parts, encoding, pc, spacePadding) + "\r\n";
						newTextASMLine = (chk_Decode_IncludeAddress.Checked ? ("[0x" + UnsignedToHex_WithLength(pc, 8) + "] ") : "") + origModLine + "\r\n";
					}
					catch
					{
						txt_Messages.Text += "FAILED TO ENCODE LINE : " + processLine + " " + ((_errorMessage != "") ? ("(" + _errorMessage + ")") : "") + "\r\n";
					}
					finally
					{
						_errorMessage = "";
					}
					
					pc += 4;
				}
				// Otherwise, this may be a label; if so, encode the statement after it
				else if (!string.IsNullOrEmpty(parts[0]))
				{
					if (parts[0].EndsWith(":"))
					{
						// Labels already added by FindLabels()
						/*
						string label = RemoveSpaces(parts[0]).ToUpper().Substring(0,parts[0].Length-1);
						try
						{
							assert(!labelDict.ContainsKey(RemoveSpaces(label.ToUpper())), "Label already exists!");
							labelDict.Add(label, pc);
						}
						catch 
						{
							txt_Messages.Text += "Error adding label '" + label + "': " + _errorMessage + "\r\n";
						}
						*/
						
						if (!string.IsNullOrEmpty(parts[1]))
						{
							string curLine = RemoveLeadingSpaces(parts[1]);
							curLine = RemoveComment(curLine);
							int curSpaceIndex = curLine.IndexOf(' ');
							
							if (curSpaceIndex > -1)
								parts[0] = curLine.Substring(0,curSpaceIndex);
							else
								parts[0] = curLine;
							
							if ((curLine.Length > curSpaceIndex) && (curSpaceIndex > -1))
								parts[1] = curLine.Substring(curSpaceIndex+1,curLine.Length-curSpaceIndex-1);
							
							// If this is an ASM command, pass off line to encoding routine
							EncodingFormat? curEncodingOrNull = EncodingFormatListCommand(encodeList, parts[0]);
							if (curEncodingOrNull != null)
							{
								EncodingFormat curEncoding = (EncodingFormat)curEncodingOrNull;
								
								try
								{
									txt_Hex.Text += EncodeASMSingle(parts, curEncoding, pc, spacePadding) + "\r\n";
									newTextASMLine = (chk_Decode_IncludeAddress.Checked ? ("[0x" + UnsignedToHex_WithLength(pc, 8) + "] ") : "") + origModLine + "\r\n";
								}
								catch
								{
									txt_Messages.Text += "FAILED TO ENCODE LINE : " + processLine + " " + ((_errorMessage != "") ? ("(" + _errorMessage + ")") : "") + "\r\n";
								}
								finally
								{
									_errorMessage = "";
								}
								
								pc += 4;
							}
						}
					}
				}
				
				if (string.IsNullOrEmpty(newTextASMLine))
					newTextASMLine = origModLine + "\r\n";
				
				newTextASM += newTextASMLine;
			}
			
			txt_ASM.Text = newTextASM;
		}
		
		private string EncodeASMSingle(string[] parts, EncodingFormat encoding, int pc, string spacePadding)
		{			
			// Initialize variables
			string binary = encoding.format.Substring(0);
			string hex = "";
			string strArgs = "";
			string[] args = null;
			
			if (!string.IsNullOrEmpty(parts[1]))
			{
				strArgs = RemoveSpaces(parts[1]);
				args = strArgs.Split(',');
			}
			
			int immed = 0;
			int difference = 0;
			string hexImmed = "";
			string binaryImmed = "";
			int startParenIndex = 0;
			int endParenIndex = 0;
			string strImmed = "";
			string regS = "";
			
			// (1) -- Encode MIPS instruction in BINARY --
			
			// I've defined 17 types of MIPS instructions based on R/I/J type as well as order
			// of arguments (rd,rs,rt and rd,rt,rs would be two different types)
			// (See mips_encode.dat: Format is: [ command : type : encoding ])
			
			// For each type, determine what in the encoding is variable, and replace it with the properly
			// encoded argument (register or immediate).
			
			switch (encoding.type)
			{
				case 1: 	// R-type register-only operations: rd,rs,rt (add,and,or,sub,subu,slt,etc)
					binary = binary.Replace("d", EncodeRegister(args[0]));
					binary = binary.Replace("s", EncodeRegister(args[1]));
					binary = binary.Replace("t", EncodeRegister(args[2]));
					break;
				case 2:		// I-type conditional branches on register values: rs,rt,[immediate] (beq,bne)
					if (args[2].StartsWith("0x"))
					{
						if (args[2].Length >= 10)
							immed = HexToUnsigned(args[2].Substring(3,args[2].Length-3));
						else
							immed = HexToUnsigned(args[2].Substring(2,args[2].Length-2));
					}
					else if (StringIsNumeric(args[2]))
					{
						immed = Convert.ToInt32(args[2]);
					}
					else
					{
						immed = LabelToUnsigned(args[2]);
					}
					
					difference = (immed - pc - 4) / 4;
					hexImmed = SignedToHex_WithLength(difference, 4);
					binaryImmed = HexHalfWordToBinary(hexImmed);
					binary = binary.Replace("s", EncodeRegister(args[0]));
					binary = binary.Replace("t", EncodeRegister(args[1]));
					binary = binary.Replace("i", binaryImmed);
					break;
				case 3:		// I-type operations with immediates: rt,rs,[immediate] (addi,addiu,andi,ori,xori,slti,sltiu)
				case 17:
					binary = binary.Replace("t", EncodeRegister(args[0]));
					binary = binary.Replace("s", EncodeRegister(args[1]));
					binary = binary.Replace("i", EncodeImmediateHalfWord(args[2]));
					break;
				case 4:		// I-type conditional branches on register value vs. fixed value: rs,[immediate] (bgez,bgtz,blez,bltz)
					if (args[1].StartsWith("0x"))
					{
						if (args[1].Length >= 10)
							immed = HexToUnsigned(args[1].Substring(3,args[1].Length-3));
						else
							immed = HexToUnsigned(args[1].Substring(2,args[1].Length-2));
					}
					else if (StringIsNumeric(args[1]))
					{
						immed = Convert.ToInt32(args[1]);
					}
					else
					{
						immed = LabelToUnsigned(args[1]);
					}
					
					difference = (immed - pc - 4) / 4;
					hexImmed = SignedToHex_WithLength(difference, 4);
					binaryImmed = HexHalfWordToBinary(hexImmed);
					binary = binary.Replace("s", EncodeRegister(args[0]));
					binary = binary.Replace("i", binaryImmed);
					break;
				case 5:		// R-type operations using HI/LO as destination registers: rs,rt (mult,multu,div,divu)
					binary = binary.Replace("s", EncodeRegister(args[0]));
					binary = binary.Replace("t", EncodeRegister(args[1]));
					break;
				case 6:		// J-type unconditional jumps to address specified by long immediate: [immediate] (j,jal)
					binary = binary.Replace("i", EncodeLongImmediate(args[0], 8));
					break;
				case 7:		// R-type instructions with only one argument: rs (jr,mthi,mtlo)
					binary = binary.Replace("s", EncodeRegister(args[0]));
					break;
				case 8:		// I-type memory operations: rt, [immediate](rs) (lb,lbu,lh,lhu,lw,sb,sh,sw)
					startParenIndex = args[1].IndexOf('(');
					endParenIndex = args[1].IndexOf(')');
					strImmed = args[1].Substring(0,startParenIndex);
					regS = args[1].Substring(startParenIndex+1, endParenIndex-startParenIndex-1);
					binary = binary.Replace("t", EncodeRegister(args[0]));
					binary = binary.Replace("s", EncodeRegister(regS));
					binary = binary.Replace("i", EncodeImmediateHalfWord(strImmed));
					break;
				case 9:		// I-type operations loading immediates into registers: rt, [immediate] (lui)
					binary = binary.Replace("t", EncodeRegister(args[0]));
					binary = binary.Replace("i", EncodeImmediateHalfWord(args[1]));
					break;
				case 10:	// R-type moves from HI/LO to a destination register: rd (mfhi,mflo)
					binary = binary.Replace("d", EncodeRegister(args[0]));
					break;
				case 11:	// R-type operations with no arguments (syscall,nop)
					break;
				case 12:	// R-type shift operations using [shamt] as shift amount: rd,rt,[shamt] (sll,srl,sra)
					binary = binary.Replace("d", EncodeRegister(args[0]));
					binary = binary.Replace("t", EncodeRegister(args[1]));
					binary = binary.Replace("h", EncodeShiftAmount(args[2]));
					break;
				case 13:	// R-type shift operations using register value as shift amount: rd,rt,rs (sllv,srlv)
					binary = binary.Replace("d", EncodeRegister(args[0]));
					binary = binary.Replace("t", EncodeRegister(args[1]));
					binary = binary.Replace("s", EncodeRegister(args[2]));
					break;
				case 14:	// R-type or I-type operations using only one operand and a destination register: rt,rs (not,move)
					binary = binary.Replace("t", EncodeRegister(args[0]));
					binary = binary.Replace("s", EncodeRegister(args[1]));
					break;
				case 15:	// R-type opreations with 10-bit immediate (syscall, break)
					binary = binary.Replace("i", EncodeImmediate(args[0], 10));
					break;
				case 16: 	// J-type operations with 26-bit immediate, not using jump-encoding (trap)
					binary = binary.Replace("i", EncodeImmediate(args[0], 26));
					break;
				default: break;
			}
			
			// (2) -- Translate BINARY to HEX --
			
			if (chk_littleEndian.Checked)
				hex = BinaryWordToHex_LittleEndian(binary);
			else
				hex = BinaryWordToHex(binary);
			
			return spacePadding + hex;
		}
	
		private void DecodeASM()
		{
			txt_ASM.Text = "";
			txt_Messages.Text = "";
			string spacePadding = GetSpacePadding();
			
			// PC is the Program Counter (address of current instruction)
			int pc = 0;
			try
			{
				if (!string.IsNullOrEmpty(txt_StartPC.Text))
				{
					if (txt_StartPC.Text.StartsWith("0x"))
					{
						if (txt_StartPC.Text.Length >= 10)
							pc = HexToUnsigned(txt_StartPC.Text.Substring(3,txt_StartPC.Text.Length-3));	
						else
							pc = HexToUnsigned(txt_StartPC.Text.Substring(2,txt_StartPC.Text.Length-2));
					}
					else
						pc = Convert.ToInt32(txt_StartPC.Text);
				}
			}
			catch 
			{ 
				txt_Messages.Text = "Starting address invalid.";
			}
			
			// Split using the "\r\n" (CRLF) sequence (Separates lines, at least in Windows)
			char[] charArray = new char[2];
			charArray[0] = '\r';
			charArray[1] = '\n';
			string[] lines = txt_Hex.Text.Split(charArray);
			
			foreach (string line in lines)
			{				
				if (string.IsNullOrEmpty(line))
					continue;
				
				string strAddress = "[0x" + UnsignedToHex_WithLength(pc, 8) + "] ";
				string strPrefix = chk_Decode_IncludeAddress.Checked ? strAddress : "";
				
				try
				{
					txt_ASM.Text += strPrefix + DecodeASMSingle(RemoveSpaces(line), pc, spacePadding) + "\r\n";
				}
				catch
				{
					txt_Messages.Text += "FAILED TO DECODE LINE : " + line + " " + ((_errorMessage != "") ? ("(" + _errorMessage + ")") : "") + "\r\n";
				}
				finally
				{
					_errorMessage = "";
				}
					
				pc += 4;
			}
		}
		
		private string DecodeASMSingle(string line, int pc, string spacePadding)
		{			
			// Rearrange byte order if necessary
			if (chk_littleEndian.Checked)
				line = line.Substring(6,2) + line.Substring(4,2) + line.Substring(2,2) + line.Substring(0,2);
			
			// Technically, nop is a pseudoinstruction. The real instruction is
			// "sll r0,r0,0x00", which does nothing, but I'd rather decode to "nop", so
			// let's do that in this particular case...
			if (line == "00000000")
				return spacePadding + "nop";
			
			// Find the opcode...
			string binaryLine = HexToBinary(line);
			string opcode = binaryLine.Substring(0,6);
			EncodingType encType = EncodingType.I;	
			
			// ...and determine the encoding type.
			if (opcode == "000000")
				encType = EncodingType.R;
			else if (opcode.Substring(0,5) == "00001")
				encType = EncodingType.J;
			else 
				encType = EncodingType.I;
			
			// Find the command based on the opcode and function
			EncodingFormat? encFormatN = null;
			if (encType == EncodingType.R)
				encFormatN = FindCommandByFunction_R(encodeList, binaryLine.Substring(binaryLine.Length-6,6));
			else if ((encType == EncodingType.I) && (opcode == "000001"))
				encFormatN = FindCommandByFunction_I(encodeList, binaryLine.Substring(11,5));
			else
				encFormatN = FindCommandByOpcode(encodeList, opcode);
			
			// If we couldn't find the command, it's some kind of mystery! Either not included in
			// mips_encode.dat, or an illegal instruction.
			if (encFormatN == null)
				return "unknown";
			
			EncodingFormat encFormat = (EncodingFormat)encFormatN;
			string encoding = encFormat.command;
			string format = encFormat.format;
			
			// Variables for the 'switch' statement below
			string dRegBinary = "";
			string sRegBinary = "";
			string tRegBinary = "";
			string immedBinary = "";
			string fullBinary = "";
			string tempBinary = "";
			string shiftAmountBinary = "";
			int difference = 0;
			string hexImmed = "";
			int immed = 0;
			int rd = 0;
			int rs = 0;
			int rt = 0;
			string newFormat = "";
			newFormat = format.Replace("d", "ddddd");
			newFormat = newFormat.Replace("s", "sssss");
			newFormat = newFormat.Replace("t", "ttttt");
			newFormat = newFormat.Replace("h", "hhhhh");
			int dIndex = newFormat.IndexOf('d');
			int sIndex = newFormat.IndexOf('s');
			int tIndex = newFormat.IndexOf('t');
			int hIndex = newFormat.IndexOf('h');
			int iIndex = newFormat.IndexOf('i');    

			if (dIndex > -1)
			{
				dRegBinary = binaryLine.Substring(dIndex,5);
				rd = BinaryToUnsigned(dRegBinary);
			}
			if (sIndex > -1)
			{
				sRegBinary = binaryLine.Substring(sIndex,5);
				rs = BinaryToUnsigned(sRegBinary);
			}
			if (tIndex > -1)
			{
				tRegBinary = binaryLine.Substring(tIndex,5);
				rt = BinaryToUnsigned(tRegBinary);
			}
			
			// Based on type (the same as in the encoding section), figure out argument order and 
			// encoding of registers and immediates, then decode them in the proper format
			switch (encFormat.type)
			{
				case 1:		// R-type register-only operations: rd,rs,rt (add,and,or,sub,subu,slt,etc)
					encoding += " r" + rd + ",r" + rs + ",r" + rt;
					break;
				case 2:		// I-type conditional branches on register values: rs,rt,[immediate] (beq,bne)
					immedBinary = binaryLine.Substring(iIndex,16);
					difference = BinaryToSigned(immedBinary);
					immed = difference * 4 + pc + 4;
					hexImmed = UnsignedToHex_WithLength(immed,8).ToLower();
					encoding += " r" + rs + ",r" + rt + ",0x" + hexImmed;
					break;
				case 3:		// I-type operations with signed immediates: rt,rs,[immediate] (addi,addiu,slti)
					immedBinary = binaryLine.Substring(iIndex,16);
					immed = BinaryToSigned(immedBinary);
					
					if (immed < 0)
						hexImmed = "-0x" + UnsignedToHex_WithLength((immed * -1),4).ToLower();
					else
						hexImmed = "0x" + UnsignedToHex_WithLength(immed,4).ToLower();
					
					encoding += " r" + rt + ",r" + rs + "," + hexImmed;
					break;
				case 4:		// I-type conditional branches on register value vs. fixed value: rs,[immediate] (bgez,bgtz,blez,bltz)
					immedBinary = binaryLine.Substring(iIndex,16);
					difference = BinaryToSigned(immedBinary);
					immed = difference * 4 + pc + 4;
					hexImmed = UnsignedToHex_WithLength(immed,8).ToLower();
					encoding += " r" + rs + ",0x" + hexImmed;
					break;
				case 5:		// R-type operations using HI/LO as destination registers: rs,rt (mult,multu,div,divu)
					encoding += " r" + rs + ",r" + rt;
					break;
				case 6:		// J-type unconditional jumps to address specified by long immediate: [immediate] (j,jal)
					immedBinary = binaryLine.Substring(iIndex,26);
					tempBinary = UnsignedToBinary_WithLength(pc, 32).Substring(0,4);
					fullBinary = tempBinary + immedBinary + "00";
					hexImmed = BinaryToHex(fullBinary).ToLower();
					encoding += " 0x" + hexImmed;
					break;
				case 7:		// R-type instructions with only one argument: rs (jr,mthi,mtlo)
					encoding += " r" + rs;
					break;
				case 8:		// I-type memory operations: rt, [immediate](rs) (lb,lbu,lh,lhu,lw,sb,sh,sw)
					immedBinary = binaryLine.Substring(iIndex,16);
					immed = BinaryToSigned(immedBinary);
					
					if (immed < 0)
						hexImmed = "-0x" + UnsignedToHex_WithLength((immed * -1),4).ToLower();
					else
						hexImmed = "0x" + UnsignedToHex_WithLength(immed,4).ToLower();
					
					encoding += " r" + rt + "," + hexImmed + "(r" + rs + ")";
					break;
				case 9:		// I-type operations loading immediates into registers: rt, [immediate] (lui)
					immedBinary = binaryLine.Substring(iIndex,16);
					hexImmed = BinaryToHex(immedBinary).ToLower();
					encoding += " r" + rt + ",0x" + hexImmed;
					break;
				case 10:	// R-type moves from HI/LO to a destination register: rd (mfhi,mflo)
					encoding += " r" + rd;
					break;
				case 11:	// R-type operations with no arguments (syscall,nop)
					break;
				case 12:	// R-type shift operations using [shamt] as shift amount: rd,rt,[shamt] (sll,srl,sra)
					shiftAmountBinary = binaryLine.Substring(hIndex,5);
					hexImmed = BinaryToHex(shiftAmountBinary).ToLower();
					encoding += " r" + rd + ",r" + rt + ",0x" + hexImmed;
					break;
				case 13:	// R-type shift operations using register value as shift amount: rd,rt,rs (sllv,srlv)
					encoding += " r" + rd + ",r" + rt + ",r" + rs;
					break;
				case 14:	// R-type or I-type operations using only one operand and a destination register: rt,rs (not,move)
					encoding += " r" + rt + ",r" + rs;
					break;
				case 15:	// R-type opreations with 10-bit immediate (syscall, break)
					immedBinary = binaryLine.Substring(iIndex,10);
					hexImmed = BinaryToHex(immedBinary).ToLower();
					encoding += " 0x" + hexImmed;
					break;
				case 16:	// J-type operations with 26-bit immediate, not using jump-encoding (trap)
					immedBinary = binaryLine.Substring(iIndex,26);
					hexImmed = BinaryToHex(immedBinary).ToLower();
					encoding += " 0x" + hexImmed;
					break;
				case 17:	// I-type operations with non-signed immediates: rt,rs,[immediate] (andi,ori,xori,sltiu)
					immedBinary = binaryLine.Substring(iIndex,16);
					hexImmed = BinaryToHex(immedBinary).ToLower();
					encoding += " r" + rt + ",r" + rs + "," + "0x" + hexImmed;
					break;
				default: break;
			}
			
			return spacePadding + encoding;
		}
		
		// For reading mips_encode.dat
		public void ReadEncodeList()
		{
	    	StreamReader reader = new StreamReader("mips_encode.dat");

	        try   
	        {    
	            while(reader.Peek() != -1)
	            {
	            	string line = reader.ReadLine();
	            	line = RemoveSpaces(line);
	            	string[] lineArray = line.Split(':');
	            
	            	EncodingFormat encoding = new EncodingFormat();
	            	encoding.command = lineArray[0];
	            	encoding.type = Convert.ToInt32(lineArray[1]);
	            	encoding.format = lineArray[2];
	            	
	            	encodeList.Add(encoding);
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
		
		// For reading registers.dat. This allows the 'named' format for registers,
		// e.g. $sp = r29, $ra = r31
		public void ReadRegisterList()
		{
			StreamReader reader = new StreamReader("registers.dat");
			
			try
			{
				while (reader.Peek() != -1)
				{
					registerList.Add(reader.ReadLine());
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
		
		// Find whether or not this is a valid command (and has an entry in our encoding list). If it
		// does, return it; otherwise, return null.
		private EncodingFormat? EncodingFormatListCommand(List<EncodingFormat> list, string command)
		{
			foreach (EncodingFormat encodingFormat in list)
			{
				if (encodingFormat.command == command)
					return encodingFormat;
			}
			return null;
		}
		
		private EncodingFormat? FindCommandByOpcode(List<EncodingFormat> list, string opcode)
		{
			foreach (EncodingFormat encodingFormat in list)
			{
				if (encodingFormat.format.Substring(0,6) == opcode)
					return encodingFormat;
			}
			return null;
		}
		
		// For R-type instructions
		private EncodingFormat? FindCommandByFunction_R(List<EncodingFormat> list, string function)
		{
			foreach (EncodingFormat encodingFormat in list)
			{
				if ((encodingFormat.format.Substring(0,6) == "000000") && (encodingFormat.format.EndsWith(function)))
					return encodingFormat;
			}
			return null;
		}
		
		// For I-type instructions
		private EncodingFormat? FindCommandByFunction_I(List<EncodingFormat> list, string function)
		{
			foreach (EncodingFormat encodingFormat in list)
			{
				if ((encodingFormat.format.Substring(0,6) == "000001") && (encodingFormat.format.Substring(0,encodingFormat.format.Length-1).EndsWith(function)))
					return encodingFormat;
			}
			return null;
		}
		
		// To get register into r(num) format. e.g. $t0 -> r8
		private string TranslateRegister(string register)
		{
			// For named register: Try to find the register in our list
			for (int i=0; i < registerList.Count; i++)
				if ((registerList[i] == register) || (registerList[i].Substring(1) == register))
					return "r" + i.ToString();
			
			// For registers starting with "$r": Just use "r"
			if ((register.Length > 2) && (register.Substring(0,2) == "$r"))
				register = register.Substring(1, register.Length-1);
				
			return register;
		}
		
		private string EncodeRegister(string register)
		{
			return EncodeRegister(register, REGISTER_ENCODE_BITS);
		}
		
		// Register will be in r(num) or $(num) format, e.g. r2 or $2
		// Register encoding must be of a certain required length, so this
		// method will pad with zeroes if it is not long enough on its own
		private string EncodeRegister(string register, int reqLength)
		{
			register = TranslateRegister(register);
			int regNum = Convert.ToInt32(register.Substring(1,register.Length-1));
			assert(regNum >= 0, "Invalid register: r" + regNum); 
			assert(regNum <= 31, "Invalid register: r" + regNum);
			
			string binary = UnsignedToBinary(regNum);
			
			// Pad with zeroes if not long enough
			int length = binary.Length;
			while (length < reqLength)
			{
				binary = "0" + binary;
				length++;
			}
			
			return binary;
		}
		
		private string EncodeShiftAmount(string hexShiftAmount)
		{
			return EncodeShiftAmount(hexShiftAmount, SHIFT_AMOUNT_ENCODE_BITS);
		}
		
		private string EncodeShiftAmount(string shiftAmount, int reqLength)
		{
			string binary = "";
			
			if ((shiftAmount.StartsWith("0x")))
			{
				string hexShiftAmount = shiftAmount.Substring(2,shiftAmount.Length-2);
				int unsignedShiftAmount = HexToUnsigned(hexShiftAmount);
				assert(unsignedShiftAmount >= 0, "Invalid shift amount: " + unsignedShiftAmount); 
				assert(unsignedShiftAmount <= 31, "Invalid shift amount: " + unsignedShiftAmount);
				binary = HexToBinary(hexShiftAmount);
			}
			else
			{
				int unsignedShiftAmount = Convert.ToInt32(shiftAmount);
				assert(unsignedShiftAmount >= 0, "Invalid shift amount: " + unsignedShiftAmount); 
				assert(unsignedShiftAmount <= 31, "Invalid shift amount: " + unsignedShiftAmount);
				binary = UnsignedToBinary(unsignedShiftAmount);
			}
			
			// Pad with zeroes if not long enough
			int length = binary.Length;
			while (length < reqLength)
			{
				binary = "0" + binary;
				length++;
			}
			
			// If too long, cut off the first part (which is all zeroes anyway)
			return binary.Substring(length-reqLength, reqLength);
		}
		
		// Accepts 0x[hex] or [dec] format
		private string EncodeImmediateHalfWord(string halfWord)
		{
			if ((halfWord.StartsWith("0x")) || (halfWord.StartsWith("-0x")))
				return UnsignedToBinary_WithLength(HexToUnsigned_AnySign(halfWord,16),16);
			else
				return UnsignedToBinary_WithLength_AnySign(Convert.ToInt32(halfWord),16);
		}
		
		// Accepts 0x[hex] or [dec] format
		private string EncodeImmediate(string hex, int length)
		{
			if ((hex.StartsWith("0x")) || (hex.StartsWith("-0x")))
				return UnsignedToBinary_WithLength(HexToUnsigned_AnySign(hex,length),length);
			else
				return UnsignedToBinary_WithLength_AnySign(Convert.ToInt32(hex),length);
		}
		
		private string EncodeLongImmediate(string input, int reqLength)
		{
			// If a number or label, convert to hex
			string hex = "";
			if (!string.IsNullOrEmpty(input))
			{
				if (!input.StartsWith("0x"))
				{
					if (StringIsNumeric(input))
						hex = "0x" + UnsignedToHex_WithLength(Convert.ToInt32(input),8);
					else
						hex = "0x" + LabelToHex(input,8);
				}
				else
				{	
					hex = input.Substring(0,input.Length);
				}	
			}
			
			// Pad with zeroes if not long enough
			string hexNum = hex.Substring(2);
			int length = hexNum.Length;
			while (length < reqLength)
			{
				hexNum = "0" + hexNum;
				length++;
			}
			hex = "0x" + hexNum;
			
			return EncodeLongImmediate(hex);
		}
		
		// Chops off first four and last two bits of 32-bit immediate, as specified by MIPS J-type format
		private string EncodeLongImmediate(string hex)
		{			
			string newHex = hex.Substring(2,hex.Length-2);
			string binary = HexToBinary(newHex);
			return binary.Substring(4,binary.Length-6);
		}
		
		private string LabelToHex(string label, int reqLength)
		{
			return UnsignedToHex_WithLength(LabelToUnsigned(label),reqLength);
		}
		
		private int LabelToUnsigned(string label)
		{
			string newLabel = RemoveSpaces(label).ToUpper();
			assert(labelDict.ContainsKey(newLabel), "Label not found: " + label);
			return labelDict[newLabel];
		}
		
		private string SignedToHex_WithLength(int num, int reqLength)
		{
			// If negative, treat it like a positive...
			int binaryDigits = reqLength * 4;
			int posNum = Math.Abs(num);
			if (num < 0)
				posNum = (int)Math.Pow(2,binaryDigits) - posNum;
			
			// ...then return the hex.
			return UnsignedToHex_WithLength(posNum, reqLength);
		}
		
		private string UnsignedToHex_WithLength(int num, int reqLength)
		{
			string hex = UnsignedToHex(num);
			
			// Pad with zeroes if not long enough
			int length = hex.Length;
			while (length < reqLength)
			{
				hex = "0" + hex;
				length++;
			}
			return hex;
		}
		
		private string UnsignedToHex(int num)
		{
			string binary = UnsignedToBinary(num);
			return BinaryToHex(binary);
		}
		
		private string UnsignedToBinary_WithLength_AnySign(int num, int reqLength)
		{
			// Treat negative as positive
			if (num < 0)
			{
				int posNum = Math.Abs(num);
				num = (int)Math.Pow(2,reqLength) - posNum;
			}
				
			string binary = UnsignedToBinary(num);
			
			// Pad with zeroes if not long enough
			int length = binary.Length;
			while (length < reqLength)
			{
				binary = "0" + binary;
				length++;
			}
			return binary;
		}
		
		private string UnsignedToBinary_WithLength(int num, int reqLength)
		{
			string binary = UnsignedToBinary(num);
			
			// Pad with zeroes if not long enough
			int length = binary.Length;
			while (length < reqLength)
			{
				binary = "0" + binary;
				length++;
			}
			return binary;
		}
		
		private string UnsignedToBinary(int num)
		{
			string retval = "";
			
			// Figure out the number of binary digits, and the place value of the first digit
			int numDigits = 1;
			int powerValue = 1;
			while (powerValue <= num)
			{
				powerValue *= 2;
				numDigits++;
			}
			powerValue /= 2;
			numDigits--;
			
			// Find value of digits: for each subsequent digit, place value is divided in half
			int currentDigit = numDigits;
			int numRemainder = num;
			while (currentDigit > 0)
			{
				if (powerValue <= numRemainder)
				{
					numRemainder -= powerValue;
					retval += "1";
				}
				else
				{
					retval += "0";
				}
				
				currentDigit--;
				powerValue /= 2;
			}
			
			return retval;
		}
		
		// If negative, treat like a positive, then convert to unsigned number
		private int HexToUnsigned_AnySign(string hex, int binaryDigits)
		{
			if (string.IsNullOrEmpty(hex))
				return 0;
			
			if (hex[0] == '-')
				return (int)Math.Pow(2,binaryDigits) - HexToUnsigned(hex.Substring(3,hex.Length-3));
			else
				return HexToUnsigned(hex.Substring(2,hex.Length-2));
		}
		
		private int HexToUnsigned(string hex)
		{
			return BinaryToUnsigned(HexToBinary(hex));
		}
		
		private int BinaryToSigned(string binary)
		{
			int unsigned = BinaryToUnsigned(binary);
			int signed = 0;
			if (binary[0] == '1')
				signed = ((int)Math.Pow(2,binary.Length) - unsigned) * -1;
			else
				signed = unsigned;
			return signed;
		}
		
		private int BinaryToUnsigned(string binary)
		{
			int binaryLength = binary.Length;
			int currentValue = 0;
			int powerValue = 1;
			for (int i=binaryLength-1; i >= 0; i--)
			{
				if (binary[i] == '1')
					currentValue += powerValue;
					
				powerValue *= 2;
			}
			
			return currentValue;
		}
		
		private string BinaryToHex(string binary)
		{
			// Requires half bytes: pad with zeroes until length is a multiple of 4
			int length = binary.Length;
			while ((length % 4) > 0)
			{
				binary = "0" + binary;
				length++;
			}
			
			// Turn every 4 binary digits into a hex digit
			string hex = "";
			int binaryLength = binary.Length;
			for (int i=0; i < binaryLength; i += 4)
				hex += BinaryHalfByteToHex(binary.Substring(i,4));
			
			return hex;
		}
		
		// Turn every hex digit into 4 binary digits
		private string HexToBinary(string hex)
		{
			string binary = "";
			int hexLength = hex.Length;
			for (int i=0; i < hexLength; i++)
				binary += HexHalfByteToBinary(hex.Substring(i,1));
			return binary;
		}
		
		private string HexWordToBinary(string hex)
		{
			return HexHalfWordToBinary(hex.Substring(0,4)) + HexHalfWordToBinary(hex.Substring(4,4));
		}
		
		private string HexHalfWordToBinary(string hex)
		{
			return HexByteToBinary(hex.Substring(0,2)) + HexByteToBinary(hex.Substring(2,2));
		}
		
		private string HexByteToBinary(string hex)
		{
			return HexHalfByteToBinary(hex.Substring(0,1)) + HexHalfByteToBinary(hex.Substring(1,1));
		}
		
		// Find 4-digit binary value for hex digit
		private string HexHalfByteToBinary(string hex)
		{
			string hexUpper = hex.ToUpper();
			switch (hexUpper)
			{
				case "0" : return "0000";
				case "1" : return "0001";
				case "2" : return "0010";
				case "3" : return "0011";
				case "4" : return "0100";
				case "5" : return "0101";
				case "6" : return "0110";
				case "7" : return "0111";
				case "8" : return "1000";
				case "9" : return "1001";
				case "A" : return "1010";
				case "B" : return "1011";
				case "C" : return "1100";
				case "D" : return "1101";
				case "E" : return "1110";
				case "F" : return "1111";
				default: return "0";
			}
		}
		
		private string BinaryWordToHex_LittleEndian(string binary)
		{
			return BinaryHalfWordToHex_LittleEndian(binary.Substring(16,16)) + BinaryHalfWordToHex_LittleEndian(binary.Substring(0,16));
		}
		
		private string BinaryHalfWordToHex_LittleEndian(string binary)
		{
			return BinaryByteToHex(binary.Substring(8,8)) + BinaryByteToHex(binary.Substring(0,8));
		}
		
		private string BinaryWordToHex(string binary)
		{
			return BinaryHalfWordToHex(binary.Substring(0,16)) + BinaryHalfWordToHex(binary.Substring(16,16));
		}
		
		private string BinaryHalfWordToHex(string binary)
		{
			return BinaryByteToHex(binary.Substring(0,8)) + BinaryByteToHex(binary.Substring(8,8));
		}
		
		private string BinaryByteToHex(string binary)
		{
			return BinaryHalfByteToHex(binary.Substring(0,4)) + BinaryHalfByteToHex(binary.Substring(4,4));
		}
		
		// Find hex digit for 4-digit binary value
		private string BinaryHalfByteToHex(string binary)
		{
			switch (binary)
			{
				case "0000" : return "0";
				case "0001" : return "1";
				case "0010" : return "2";
				case "0011" : return "3";
				case "0100" : return "4";
				case "0101" : return "5";
				case "0110" : return "6";
				case "0111" : return "7";
				case "1000" : return "8";
				case "1001" : return "9";
				case "1010" : return "A";
				case "1011" : return "B";
				case "1100" : return "C";
				case "1101" : return "D";
				case "1110" : return "E";
				case "1111" : return "F";
				default: return "0";
			}
		}
		
		// Gets space padding based on input
		private string GetSpacePadding()
		{
			if (!chk_SpacePad.Checked)
				return "";
			
			if (!StringIsNumeric(txt_SpacePad.Text))
				return "";
			
			int numSpaces = Convert.ToInt32(txt_SpacePad.Text);
			
			if (numSpaces <= 0)
				return "";
			
			string retval = "";
			for (int i=0; i < numSpaces; i++)
				retval += " ";
			
			return retval;
		}
		
		// Removes blocks in brackets. For removing addresses.
		private string RemoveBracketBlocks(string str)
		{
			string retval = "";
			bool include = true;
			
			foreach (char c in str)
			{
				if (c == '[')
					include = false;
				else if (c == ']')
					include = true;
				else if (include)
					retval += c;
			}
			
			return retval;
		}
		
		// Returns a string that is a copy of the original string with spaces removed
		private string RemoveSpaces(string str)
		{
			string retval = "";
			foreach (char c in str)
				if ((c != ' ') && (c != '\t') && (c != '\r'))
					retval += c;
			return retval;
		}
		
		// Returns a string that is a copy of the original string with leading spaces removed
		private string RemoveLeadingSpaces(string str)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			
			if ((str[0] != ' ') && (str[0] != '\t'))
				return str;
			
			int startIndex = 0;
			for (; ((startIndex < str.Length) && ((str[startIndex] == ' ') || (str[startIndex] == '\t'))); startIndex++);
			
			if (startIndex == str.Length)
				return "";
			
			return str.Substring(startIndex, str.Length-startIndex);
		}
		
		private string RemoveComment(string str)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			
			int index = str.IndexOf('#');
			if (index < 0)
				return str;
			
			return str.Substring(0, index);
		}
		
		private bool StringIsNumeric(string str)
		{
			if (string.IsNullOrEmpty(str))
				return false;
			
			foreach (char c in str)
			{
				if (!char.IsNumber(c))
					return false;
			}
			
			return true;
		}
		
		private void assert(bool condition, string message)
		{
			if (!condition)
			{
				_errorMessage = message;
				throw new Exception("Assertion failed: " + message);
			}
		}
	}
}
