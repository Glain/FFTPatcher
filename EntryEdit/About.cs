using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntryEdit
{
    internal static class About
    {
        public const int FFTPatcherSuiteRevision = 493;

        public static readonly string Title = "About";
        public static readonly string Message = string.Format(@"EntryEdit is a modding tool created by Glain that makes use of modified FFTPatcher suite project libraries originally created by melonhead.

This version of EntryEdit is included in the FFTPatcher Suite continuation project at https://github.com/Glain/FFTPatcher/.

FFTPatcher Suite version 0.{0}", FFTPatcherSuiteRevision);
    }
}
