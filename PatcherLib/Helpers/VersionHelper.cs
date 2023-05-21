using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib.Helpers
{
    public class VersionHelper
    {
        public const int VersionFirst = 0;
        public const int VersionSecond = 497;
        public const int VersionThird = 1;

        public static readonly string VersionString = VersionFirst + "." + VersionSecond + "." + VersionThird;
    }
}
