using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StringExtensionMethods
    {
        //example
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        public static void Clear(this RichTextBox Box, int LineIndex)
        {
            string[] cleared = Box.Lines;
            cleared[LineIndex] = "";
            Box.Lines = cleared;
        }
        public static string[] Delete(this string[] array, int index)
        {
            int i = 0;
            string[] newarray = new string[array.Length - 1];
            foreach (string str in array)
            {
                if (index != i && i < newarray.Length)
                {
                    newarray[i] = array[i];
                }
                i++;
            }
            return newarray;
        }
    }
}
