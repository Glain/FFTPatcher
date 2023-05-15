using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassHexASM
{
    internal static class About
    {
        public const string FFTPatcherSuiteVersionString = "0.497.0";

        public static readonly string Title = "About";
        public static readonly string Message = string.Format(@"MassHexASM, LEDecoder, and the underlying ASMEncoding library are MIPS assembly/disassembly tools created by Glain.

This version of MassHexASM is included in the FFTPatcher Suite continuation project at https://github.com/Glain/FFTPatcher/.

FFTPatcher Suite version {0}", FFTPatcherSuiteVersionString);
    }
}
