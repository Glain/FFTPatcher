using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatcherLib.Datatypes;
using PatcherLib.TextUtilities;

namespace EntryEdit
{
    public static class TextUtility
    {
        public static IList<byte> Encode(string input)
        {
            TextUtilities.CompressionResult result = TextUtilities.Compress(new List<IList<string>>() { new List<string>() { input } }, 0xFE, TextUtilities.PSXMap, new List<bool>() { false });
            return result.Bytes;
        }

        public static string Decode(IList<byte> bytes, byte terminator = (byte)0xFE, GenericCharMap charmap = null)
        {
            return DecodeList(bytes, terminator, charmap)[0];
        }

        /// <summary>
        /// Processes a list of FFT text bytes into a list of FFTacText strings.
        /// </summary>
        /// <param name="bytes">The bytes to process</param>
        /// <param name="charmap">The charmap to use</param>
        public static IList<string> DecodeList(IList<byte> bytes, byte terminator = (byte)0xFE, GenericCharMap charmap = null)
        {
            IList<IList<byte>> words = bytes.Split(terminator);
            List<string> result = new List<string>(words.Count);
            charmap = charmap ?? TextUtilities.PSXMap;

            foreach (IList<byte> word in words)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    int pos = 0;
                    while (pos < (word.Count - 1) || (pos == (word.Count - 1) && word[pos] != terminator))
                    {
                        string strNextChar = "";
                        strNextChar = charmap.GetNextChar(word, ref pos);
                        sb.Append(strNextChar);
                    }

                    sb.Replace(@"{Newline}", @"{Newline}" + Environment.NewLine);

                    result.Add(sb.ToString());
                }
                catch (Exception)
                {
                    result.Add(null);
                }
            }

            return result;
        }
    }
}
