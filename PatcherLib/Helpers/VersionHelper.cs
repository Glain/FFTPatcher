using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib.Helpers
{
    public class VersionHelper
    {
        public const int VersionFirst = 0;
        public const int VersionSecond = 496;
        public const int VersionThird = 0;

        public static readonly string VersionString = VersionFirst + "." + VersionSecond + "." + VersionThird;
    }
}
