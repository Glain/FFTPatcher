using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace FFTorgASM
{
    internal static class Settings
    {
        private static readonly string XmlPatchesPathConfig = ConfigurationManager.AppSettings["XmlPatchesPath"];
        private static readonly string XmlPatchesPathDefault = "./XmlPatches";

        internal static readonly string XmlPatchesPath = GetXmlPatchesPath();

        private static string GetXmlPatchesPath()
        {
            string result = XmlPatchesPathConfig;
            if (!Directory.Exists(XmlPatchesPathConfig))
                result = XmlPatchesPathDefault;

            return result;
        }
    }
}
