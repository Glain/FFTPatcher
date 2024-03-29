﻿/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/16/2012
 * Time: 23:55
 * 
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ASMEncoding.Helpers
{
	public class ASMValueHelper
	{
        public static uint JumpImmediateMask = 0x0FFFFFFC; // 0000 1111 1111 1111 1111 1111 1111 1100

		public ASMLabelHelper LabelHelper { get; set; }
		
		public ASMValueHelper() { }
		public ASMValueHelper(ASMLabelHelper labelHelper)
		{
			LabelHelper = labelHelper;
		}

        public ASMValueHelper(ASMValueHelper valueHelper)
        {
            LabelHelper = new ASMLabelHelper(valueHelper.LabelHelper);
        }

		public uint FindUnsignedValue(string val, bool skipLabelAssertion = false)
		{
            Nullable<uint> value = FindUnsignedValueGeneric(val);

            if (value == null)
            {
                return LabelHelper.LabelToUnsigned(val, skipLabelAssertion);
            }
            else
            {
                return value.Value;
            }
		}

        public static Nullable<uint> FindUnsignedValueGeneric(string val)
        {
            if (val.StartsWith("0x"))
			{
				//if (val.Length >= 10)
				//	return HexToUnsigned(val.Substring(3,val.Length-3));
				//else
					return HexToUnsigned(val.Substring(2,val.Length-2));
			}
            else if (ASMStringHelper.StringIsNumeric(val))
            {
                return Convert.ToUInt32(val);
            }
            else
            {
                return null;
            }
        }

		public uint GetAnyUnsignedValue(string val, bool skipLabelAssertion = false)
		{
            return LabelHelper.GetAnyUnsignedValue(val, skipLabelAssertion);
		}

        public uint GetImmediateValue(string val, int length, bool isSigned, bool skipLabelAssertion = false)
        {
            uint value = 0;

            uint uMaxValue = (length == 32) ? uint.MaxValue : (uint)((1L << length) - 1);
            int iMaxValue = (length == 32) ? int.MaxValue : (int)((1L << (length - 1)) - 1);
            int iMinValue = (length == 32) ? int.MinValue : (int)(-(1L << (length - 1)));

            uint minNegatedValue = ((uint)Int32.MaxValue + 1);

            if (val.StartsWith("0x"))
            {
                value = HexToUnsigned(val.Substring(2, val.Length - 2));
                ASMDebugHelper.assert(isSigned || (value <= uMaxValue), string.Format("Unsigned immediate ({0}) greater than maximum value (0x{1})", val, uMaxValue.ToString("x")));
                ASMDebugHelper.assert(!isSigned || (value <= iMaxValue), string.Format("Signed immediate ({0}) greater than maximum value (0x{1})", val, iMaxValue.ToString("x")));
            }
            else if (val.StartsWith("-0x"))
            {
                ASMDebugHelper.assert(isSigned, "Negative unsigned immediate: " + val);
                value = HexToUnsigned(val.Substring(3, val.Length - 3));
                
                ASMDebugHelper.assert(value <= minNegatedValue, string.Format("Signed immediate ({0}) less than minimum value (-0x{1})", val, minNegatedValue.ToString("x")));
                int iValue = -(Convert.ToInt32(value));
                ASMDebugHelper.assert(iValue >= iMinValue, string.Format("Signed immediate ({0}) less than minimum value (-0x{1})", val, (-iMinValue).ToString("x")));
                ASMDebugHelper.assert(iValue <= iMaxValue, string.Format("Signed immediate ({0}) greater than maximum value (0x{1})", val, iMaxValue.ToString("x")));
                value = (value == 0) ? 0 : (uint)(0x100000000 - value);

            }
            else if (ASMStringHelper.StringIsNumeric(val))
            {
                value = Convert.ToUInt32(val);
                ASMDebugHelper.assert(isSigned || (value <= uMaxValue), string.Format("Unsigned immediate ({0}) greater than maximum value (0x{1})", val, uMaxValue.ToString("x")));
                ASMDebugHelper.assert(!isSigned || (value <= iMaxValue), string.Format("Signed immediate ({0}) greater than maximum value (0x{1})", val, iMaxValue.ToString("x")));
            }
            else if ((val.StartsWith("-")) && (val.Length > 1))
            {
                ASMDebugHelper.assert(isSigned, "Negative unsigned immediate: " + val);
                string str_uvalue = val.Substring(1);
                bool isNumeric = ASMStringHelper.StringIsNumeric(str_uvalue);
                ASMDebugHelper.assert(isNumeric, "Could not parse negative value: " + val);

                uint uvalue = Convert.ToUInt32(str_uvalue);
                ASMDebugHelper.assert(value <= minNegatedValue, string.Format("Signed immediate ({0}) less than minimum value (-0x{1})", val, minNegatedValue.ToString("x")));
                int iValue = -(Convert.ToInt32(value));
                ASMDebugHelper.assert(iValue >= iMinValue, string.Format("Signed immediate ({0}) less than minimum value (0x{1})", val, iMinValue.ToString("x")));
                ASMDebugHelper.assert(iValue <= iMaxValue, string.Format("Signed immediate ({0}) greater than maximum value (0x{1})", val, iMaxValue.ToString("x")));
                value = (uvalue == 0) ? 0 : (uint)(0x100000000 - uvalue);
            }
            else
            {
                return LabelHelper.LabelToUnsigned(val, skipLabelAssertion);
            }

            return value;
        }

        public static string UnsignedToHex_WithLength(uint num, int reqLength)
        {
            return AddLeadingZeroes(UnsignedToHex(num), reqLength);
        }

        /*
        public static string UnsignedToBinaryAny_WithLength(int num, int reqLength)
        {
            // Treat negative as positive
            if (num < 0)
            {
                int posNum = Math.Abs(num);
                num = (int)Math.Pow(2, reqLength) - posNum;
            }

            return AddLeadingZeroes(UnsignedToBinary((uint)num), reqLength);
        }
        */

        public static string UnsignedToBinary_WithLength(uint num, int reqLength)
        {
            return AddLeadingZeroes(UnsignedToBinary(num), reqLength);
        }
         
		public static uint HexToUnsigned_AnySign(string hex, int binaryDigits)
		{
			if (string.IsNullOrEmpty(hex))
				return 0;

            if (hex[0] == '-')
            {
                //return (uint)(Math.Pow(2, binaryDigits) - HexToUnsigned(hex.Substring(3, hex.Length - 3)));
                return (uint)( (1L << binaryDigits) - HexToUnsigned(hex.Substring(3, hex.Length - 3)) );
            }
            else
                return HexToUnsigned(hex.Substring(2, hex.Length - 2));
		}
         
        public static string BinaryToHex(string binary)
        {
            return Convert.ToString(BinaryToUnsigned(binary), 16);
        }

        public static uint BinaryToUnsigned(string binary)
        {
            return Convert.ToUInt32(binary, 2);
        }

        public static uint HexToUnsigned(string hex)
        {
            return Convert.ToUInt32(hex, 16);
        }

        public static string HexToBinary(string hex)
        {
            return Convert.ToString(HexToUnsigned(hex), 2);
        }

        public static string UnsignedToHex(uint num)
        {
            return Convert.ToString(num, 16);
        }

        public static string UnsignedToBinary(uint num)
        {
            return Convert.ToString(num, 2);
        }

        public static short BinaryToSignedShort(string binary)
        {
            return Convert.ToInt16(binary, 2);
        }

        public static string SignedToHex_WithLength(int num, int reqLength)
        {
            return EnforceLength(Convert.ToString(num, 16), reqLength);
        }

        public static string BinaryToHex_WithLength(string binary, int reqLength)
        {
            return EnforceLength(BinaryToHex(binary), reqLength);
        }

        public static string HexToBinary_WithLength(string hex, int reqLength)
        {
            return EnforceLength(HexToBinary(hex), reqLength);
        }

        /*
        public static string BinaryWordToHex(string binary, bool littleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(BinaryToUnsigned(binary));

            if (!littleEndian)
            {
                Array.Reverse(bytes);
            }

            return AddLeadingZeroes(BitConverter.ToString(bytes).Replace("-",""), 8);
        }
        */

        public static byte[] ConvertUIntToBytes(uint value, bool littleEndian)
        {
            //return BitConverter.GetBytes(value);
            byte[] result = { 
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)((value & 0xFF))
            };

            if (littleEndian)
                Array.Reverse(result);

            return result;
        }

        public static string AddLeadingZeroes(string str, int reqLength)
        {
            return str.PadLeft(reqLength, '0');
        }

        public static string EnforceLength(string str, int reqLength)
        {
            string newStr = AddLeadingZeroes(str, reqLength);
            int newStrLength = newStr.Length;
            return (newStrLength > reqLength) ? newStr.Substring(newStrLength - reqLength, reqLength) : newStr;
        }

        public static int GetIncludeMask(int encodingBitLength)
        {
            return (int)((1U << encodingBitLength) - 1);
        }

        public static uint ReverseBytes(uint value)
        {
            return ((value & 0xFF) << 24) + ((value & 0xFF00) << 8) + ((value & 0xFF0000) >> 8) + ((value & 0xFF000000) >> 24);
        }

        public static uint SignedToUnsigned(int signed)
        {
            return unchecked((uint)signed);
        }

        public static short UnsignedShortToSignedShort(ushort unsigned)
        {
            return unchecked((short)unsigned);
        }

        public static string[] GetHexLines(string hexString)
        {
            List<string> result = new List<string>();

            hexString = Regex.Replace(hexString, @"\s+", "");
            hexString = Regex.Replace(hexString, "(?s)<!--.*?-->", "");

            int pos = 0;
            int length = hexString.Length;

            for (; pos < length; pos += 8)
            {
                if (hexString.Length >= (pos + 8))
                {
                    string str = hexString.Substring(pos, 8);
                    result.Add(str);
                }
            }

            return result.ToArray();
        }

        public static uint[] GetUintArrayFromBytes(IEnumerable<byte> bytes, bool littleEndian)
        {
            List<uint> uintList = new List<uint>();

            uint uintValue = 0;
            int offset = 0;
            foreach (byte b in bytes)
            {
                int shiftAmount = littleEndian ? (offset * 8) : (24 - (offset * 8));
                uintValue |= (((uint)b) << shiftAmount);

                offset = (offset + 1) % 4;
                if (offset == 0)
                {
                    uintList.Add(uintValue);
                    uintValue = 0;
                }
            }

            return uintList.ToArray();
        }
	}
}
