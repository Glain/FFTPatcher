using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PatcherLib
{
    using static PatcherLib.ResourcesClass;
    using LabelType = ResourcesClass.LabelType;

    public static partial class PSPResources
    {
        public static class Labels
        {
            private static IDictionary<LabelType, IDictionary<string, string>> typeMap = new Dictionary<LabelType, IDictionary<string, string>>();

            private static IDictionary<LabelType, ResourceListInfo> typeInfo = new Dictionary<LabelType, ResourceListInfo>
            {
                {
                    LabelType.ENTDUnused,
                    new ResourceListInfo {
                        Doc = PSPResources.ENTDDoc,
                        XPath = "/ENTD/UnusedSection"
                    }
                }
            };

            public static IDictionary<string, string> ENTDUnused
            {
                get { return GetLabelMapForType(LabelType.ENTDUnused); }
            }

            private static IDictionary<string, string> GetLabelMapForType(LabelType type)
            {
                if (!typeMap.ContainsKey(type))
                {
                    typeMap[type] =
                        ResourcesClass.GetLabelsFromXmlNodes(typeInfo[type]).AsReadOnly();
                }
                return typeMap[type];
            }
        }
    }
}
