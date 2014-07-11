/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 11/16/2012
 * Time: 23:36
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace ASMEncoding.Helpers
{
	public class ASMStringHelper
	{
        internal class CharOffsets
        {
            public static int Digit = 48;
            public static int UpperLetter = 65;
            public static int LowerLetter = 97;
        }

		// Removes leading block in brackets. For removing address.
		public static string RemoveLeadingBracketBlock(string str)
		{
            if (string.IsNullOrEmpty(str))
                return str;

            if (str[0] != '[')
                return str;

            List<char> charList = new List<char>();

			bool include = true;
			bool pendingInclude = false;
            bool removedFirst = false;

			foreach (char c in str)
			{
                if ((c == '[') && (!removedFirst))
                {
                    include = false;
                    removedFirst = true;
                }
                else if (c == ']')
                {
                    pendingInclude = true;
                }
                else if (pendingInclude)
                {
                    pendingInclude = false;
                    include = true;
                }
                else if (include)
                {
                    charList.Add(c);
                }
			}

            return new string(charList.ToArray());
		}

        // Converts bracket blocks to use semicolons instead of commas
        public static string ConvertBracketBlocks(string str)
        {
            char[] chars = str.ToCharArray();
            bool convert = false;

            for (int i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '[': convert = true; break;
                    case ']': convert = false; break;
                    case ',': 
                        if (convert) 
                            chars[i] = ';'; 
                        break;
                    default: break;
                }
            }

            return new string(chars);
        }

		// Returns a string that is a copy of the original string with spaces removed
		public static string RemoveSpaces(string str)
		{
            List<char> charList = new List<char>();

            foreach (char c in str)
                if ((c != ' ') && (c != '\t') && (c != '\r'))
                    charList.Add(c);

            return new string(charList.ToArray());
		}
		
		// Returns a string that is a copy of the original string with leading spaces removed
		public static string RemoveLeadingSpaces(string str)
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
		
		public static string RemoveComment(string str)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			
			int index = str.IndexOf('#');
            if (index < 0)
                index = str.IndexOf(';');

			if (index < 0)
				return str;
			
			return str.Substring(0, index);
		}
		
		public static bool StringIsNumeric(string str)
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
		
		public static string[] RemoveFromLines(string[] lines, string str)
		{
			List<string> result = new List<string>();
			foreach(string line in lines)
			{
				string newLine = line.Replace("\r", "");
				result.Add(newLine);
			}
			return result.ToArray();
		}
		
		public static string[] SplitLine(string processLine)
		{
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
			
			return parts;
		}
		
		public static string[] RemoveLabel(string[] parts)
		{
			// Remove label portion, if it exists
			if (!string.IsNullOrEmpty(parts[0]))
			{
				if (parts[0].EndsWith(":"))
				{						
					if (!string.IsNullOrEmpty(parts[1]))
					{
						string curLine = ASMStringHelper.RemoveLeadingSpaces(parts[1]);
						curLine = ASMStringHelper.RemoveComment(curLine);
						parts = ASMStringHelper.SplitLine(curLine);
					}
				}
			}
			
			return parts;
		}

        public static string Concat(params string[] parts)
        {
            int length = 1;
            foreach (string part in parts)
                length += part.Length;

            StringBuilder sb = new StringBuilder(length);
            foreach (string part in parts)
                sb.Append(part);

            return sb.ToString();
        }
        
        public static string ReplaceCharAtIndex(string src, int index, string replacement)
        {
            return src.Insert(index, replacement).Remove(index + replacement.Length, 1);
        }

        public static List<int> CreateIntList(string str, char separator = ',')
        {
            List<int> result = new List<int>();
            string[] strItems = str.Split(separator);
            foreach (string strItem in strItems)
                result.Add(int.Parse(strItem));

            return result;
        }

        public static string CreateCharacterString(char c, int length)
        {
            char[] cArray = new char[length];

            for (int i = 0; i < length; i++)
                cArray[i] = c;

            return new string(cArray);
        }

        public static char CreateCharWithOffset(int iChr, int offset)
        {
            return (char)(iChr + offset);
        }

        public static char CreateDigitChar(int digit)
        {
            return CreateCharWithOffset(digit, CharOffsets.Digit);
        }

        public static char CreateUpperLetterChar(int num)
        {
            return CreateCharWithOffset(num, CharOffsets.UpperLetter);
        }

        public static char CreateLowerLetterChar(int num)
        {
            return CreateCharWithOffset(num, CharOffsets.LowerLetter);
        }

        public static char CreateRegisterChar(int index)
        {
            return CreateUpperLetterChar(index);
        }

        public static char CreateImmediateChar(int index)
        {
            return CreateLowerLetterChar(index);
        }
	}
}
