using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib.TextUtilities;

namespace EntryEdit
{
    public static class TextUtility
    {
        public static List<byte> Encode(string input, Context context)
        {
            GenericCharMap charMap = (context == Context.US_PSP) ? (GenericCharMap)TextUtilities.PSPMap : (GenericCharMap)TextUtilities.PSXMap;
            TextUtilities.CompressionResult result = TextUtilities.Compress(new List<IList<string>>() { new List<string>() { input } }, 
                0xFE, charMap, charMap, 
                new List<bool>() { false }, new List<bool> () { false } );
            return new List<byte>(result.Bytes);
        }

        public static string Decode(IList<byte> bytes, Context context, Set<byte> terminators = null, GenericCharMap charmap = null)
        {
            return DecodeList(bytes, context, terminators, charmap)[0];
        }

        /// <summary>
        /// Processes a list of FFT text bytes into a list of FFTacText strings.
        /// </summary>
        /// <param name="bytes">The bytes to process</param>
        /// <param name="charmap">The charmap to use</param>
        private static IList<string> DecodeList(IList<byte> bytes, Context context, Set<byte> terminators = null, GenericCharMap charmap = null)
        {
            terminators = terminators ?? new Set<byte>() { 0xFE, 0xFF };
            IList<IList<byte>> words = bytes.Split(terminators);
            List<string> result = new List<string>(words.Count);
            charmap = charmap ?? ((context == Context.US_PSP) ? (GenericCharMap)TextUtilities.PSPMap : (GenericCharMap)TextUtilities.PSXMap);

            foreach (IList<byte> word in words)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    int pos = 0;
                    //while (pos < (word.Count - 1) || (pos == (word.Count - 1) && word[pos] != terminator))
                    while (pos < (word.Count - 1) || (pos == (word.Count - 1) && (word[pos] != 0xFE)))
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
