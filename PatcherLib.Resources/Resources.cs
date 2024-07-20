/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace PatcherLib
{
    using PatcherLib.Datatypes;
    using PatcherLib.Utilities;
    using System.Xml;
    public static class ResourcesClass
    {
        internal enum ListType
        {
            Propositions,
            Treasures,
            UnexploredLands,
            SpriteFiles,
            AbilityAI,
            AbilityAttributes,
            AbilityEffects,
            AbilityTypes,
            EventNames,
            MapNames,
            ShopAvailabilities,
            StatusNames,
            SkillSetNames,
            ItemNames,
            AbilityNames,
            MonsterNames,
            JobNames,
            UnitNames,
            SpecialNames,
            SpriteSets,
            AbilityProperties
        }

        internal struct ResourceListInfo
        {
            public XmlDocument Doc { get; set; }
            public string XPath { get; set; }
            public int Length { get; set; }
            public int StartIndex { get; set; }
        }

        public static IList<string> GetResourceByName( string fullName )
        {
            int lastDotIndex = fullName.LastIndexOf( Type.Delimiter );
            string typeName = fullName.Substring( 0, lastDotIndex );
            string fieldName = fullName.Substring( lastDotIndex + 1 );
            return
                PatcherLib.ReflectionHelpers.GetPublicStaticFieldOrProperty<IList<string>>(
                   Type.GetType( typeName ), fieldName, false );
        }

        // Glain - rewriting method to determine length from XML node length instead of using a fixed length.

        /*
        internal static IList<string> GetStringsFromNumberedXmlNodes( ResourceListInfo info )
        {
            string[] result = new string[info.Length];
            for ( int i = info.StartIndex; i < ( info.Length + info.StartIndex ); i++ )
            {
                XmlNode node = info.Doc.SelectSingleNode( string.Format( info.XPath, i ) );
                result[i - info.StartIndex] = node == null ? string.Empty : node.InnerText;
            }

            return result;
        }
        */

        internal static IList<string> GetStringsFromNumberedXmlNodes(ResourceListInfo info, Context context)
        {
            string newXPath = "";
            bool ignore = false;

            for (int i = 0; i < info.XPath.Length; i++)
            {
                if (info.XPath[i] == '[')
                    ignore = true;

                if (!ignore)
                    newXPath += info.XPath[i];

                if (info.XPath[i] == ']')
                    ignore = false;
            }

            XmlNodeList nodeList = info.Doc.SelectNodes(string.Format(newXPath));
            int resultCount = Math.Max(nodeList.Count, info.Length);
            string[] result = new string[resultCount];

            bool isPSX = (context == Context.US_PSX);

            for (int i = info.StartIndex; i < (info.StartIndex + resultCount); i++)
            {
                XmlNode node = info.Doc.SelectSingleNode(string.Format(info.XPath, i));

                string text = string.Empty;
                if (node != null)
                {
                    text = node.InnerText;
                    if (isPSX)
                    {
                        XmlAttribute attrPSP = node.Attributes["psp"];
                        if ((attrPSP != null) && (attrPSP.Value.ToLower() == "true"))
                        {
                            XmlAttribute attrPSXName = node.Attributes["psxName"];
                            if (attrPSXName != null)
                            {
                                text = attrPSXName.Value;
                            }
                            else
                            {
                                text = string.Empty;
                            }
                        }
                    }
                }

                result[i - info.StartIndex] = text;
            }
            for (int i = 0; i < resultCount; i++)
            {
                result[i] = result[i] ?? string.Empty;
            }

            return result;
        }

        internal static IList<string> GetStringsFromNumberedXmlNodes(ResourceListInfo info)
        {
            string newXPath = "";
            bool ignore = false;

            for (int i = 0; i < info.XPath.Length; i++)
            {
                if (info.XPath[i] == '[')
                    ignore = true;

                if (!ignore)
                    newXPath += info.XPath[i];

                if (info.XPath[i] == ']')
                    ignore = false;
            }

            XmlNodeList nodeList = info.Doc.SelectNodes(string.Format(newXPath));
            int resultCount = Math.Max(nodeList.Count, info.Length);
            string[] result = new string[resultCount];

            for (int i = info.StartIndex; i < (info.StartIndex + resultCount); i++)
            {
                XmlNode node = info.Doc.SelectSingleNode(string.Format(info.XPath, i));
                result[i - info.StartIndex] = ((node == null) ? string.Empty : node.InnerText);
            }
            for (int i = 0; i < resultCount; i++)
            {
                result[i] = result[i] ?? string.Empty;
            }
            
            return result;
        }

        private static XmlDocument abilityFormulasDoc;
        private static Dictionary<Context, IDictionary<byte, string>> abilityFormulas;

        public static IDictionary<byte, string> GetAbilityFormulas(Context context)
        {
            if (abilityFormulas == null)
            {
                abilityFormulas = new Dictionary<Context, IDictionary<byte, string>>();
            }

            if (!abilityFormulas.ContainsKey(context))
            {
                var temp = new Dictionary<byte, string>();
                var formulaNames = ResourcesClass.GetStringsFromNumberedXmlNodes(
                    new ResourceListInfo {
                        Doc = abilityFormulasDoc,
                        XPath = "/AbilityFormulas/Ability[@value='{0:X2}']",
                        Length = 256,
                        StartIndex = 0 }, context);
                for (int i = 0; i < 256; i++)
                {
                    temp.Add((byte)i, formulaNames[i]);
                }

                abilityFormulas.Add(context, new PatcherLib.Datatypes.ReadOnlyDictionary<byte, string>(temp));
            }

            return abilityFormulas[context];
        }

        public static IDictionary<string, IList<byte>> ZipFileContents
        {
            get; private set;
        }

        public static IDictionary<string, IList<byte>> DefaultZipFileContents
        {
            get; set;
        }

        static ResourcesClass()
        {
            InitResourcesClass();
        }

        private static void InitResourcesClass()
        {
            using (MemoryStream memStream = new MemoryStream(PatcherLib.Resources.Properties.Resources.ZippedResources, false))
            using (GZipInputStream gzStream = new GZipInputStream(memStream))
            using (TarInputStream tarStream = new TarInputStream(gzStream))
            {
                var tempDefault = new Dictionary<string, IList<byte>>();
                TarEntry entry;
                entry = tarStream.GetNextEntry();
                while (entry != null)
                {
                    if (entry.Size != 0)
                    {
                        byte[] bytes = new byte[entry.Size];
                        StreamUtils.ReadFully(tarStream, bytes);
                        tempDefault[entry.Name] = bytes.AsReadOnly();
                    }
                    entry = tarStream.GetNextEntry();
                }

                DefaultZipFileContents = new PatcherLib.Datatypes.ReadOnlyDictionary<string, IList<byte>>(tempDefault);
            }

            string defaultsFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources.zip");

            if (File.Exists(defaultsFile))
            {
                var tempContents = new Dictionary<string, IList<byte>>();
                try
                {
                    using (FileStream file = File.Open(defaultsFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (ZipInputStream zipStream = new ZipInputStream(file))
                    {
                        ZipEntry entry = zipStream.GetNextEntry();
                        while (entry != null)
                        {
                            if (entry.Size != 0)
                            {
                                byte[] bytes = new byte[entry.Size];
                                StreamUtils.ReadFully(zipStream, bytes);
                                tempContents[entry.Name] = bytes.AsReadOnly();
                            }
                            entry = zipStream.GetNextEntry();
                        }

                        foreach (KeyValuePair<string, IList<byte>> kvp in DefaultZipFileContents)
                        {
                            if (!tempContents.ContainsKey(kvp.Key))
                            {
                                tempContents[kvp.Key] = kvp.Value;
                            }
                        }
                    }

                    ZipFileContents = new PatcherLib.Datatypes.ReadOnlyDictionary<string, IList<byte>>(tempContents);
                }
                catch (Exception ex)
                {
                    ZipFileContents = DefaultZipFileContents;
                }
            }
            else
            {
                ZipFileContents = DefaultZipFileContents;
            }

            abilityFormulasDoc = ZipFileContents[Paths.AbilityFormulasXML].ToUTF8String().ToXmlDocument();
        }

        public static void GenerateDefaultResourcesZip(string filename)
        {
            GenerateResourcesZip(filename, DefaultZipFileContents);
        }

        public static void GenerateResourcesZip(string filename, IDictionary<string, IList<byte>> contents)
        {
            using (FileStream stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
            using (ZipOutputStream output = new ZipOutputStream(stream))
            {
                foreach (var kvp in contents)
                {
                    ZipEntry ze = new ZipEntry(kvp.Key);
                    IList<byte> bytes = kvp.Value;
                    ze.Size = bytes.Count;
                    output.PutNextEntry(ze);
                    output.Write(bytes.ToArray(), 0, bytes.Count);
                }
            }
        }

        public static string[] EnforceUniqueStrings(IList<string> strings)
        {
            int count = strings.Count;
            string[] result = new string[count];

            for (int index = 0; index < count; index++)
            {
                result[index] = strings[index];
                for (int innerIndex = 0; innerIndex < index; innerIndex++)
                {
                    while (result[index].Equals(result[innerIndex]))
                    {
                        result[index] += " ";
                    }
                }
            }

            return result;
        }

        public static string GenerateXMLSectionEntries(IList<byte> bytes, string entryTitle, string valueTitle, bool isValueHex, int startValue = 0, int endValue = 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            XmlDocument xmlDocument = new XmlDocument();
            
            //xmlDocument.LoadXml(System.Text.Encoding.UTF8.GetString(bytes.ToArray()));
            using (MemoryStream stream = new MemoryStream(bytes.ToArray()))
            {
                xmlDocument.Load(stream);
            }

            XmlNodeList xmlNodes = xmlDocument.SelectNodes("//" + entryTitle);

            foreach (XmlNode xmlNode in xmlNodes)
            {
                string strValue = xmlNode.Attributes[valueTitle].Value;
                int value = -1;

                System.Globalization.NumberStyles numberStyle = isValueHex ? System.Globalization.NumberStyles.HexNumber : System.Globalization.NumberStyles.Number;
                int.TryParse(strValue, numberStyle, System.Globalization.CultureInfo.InvariantCulture, out value);

                bool isWithinStartValue = ((startValue == 0) || (value >= startValue));
                bool isWithinEndValue = ((endValue == 0) || (value <= endValue));

                if (isWithinStartValue && isWithinEndValue)
                    sb.AppendLine("    " + xmlNode.OuterXml);
            }

            return sb.ToString();
        }

        public static string GenerateXMLSectionEntries(IList<string> section, string entryTitle, string attributeTitle, string valueTitle, bool isValueHex,
            IList<string> prefixes = null, int startValue = 0, int startIndex = 0, int length = 0, int indexAddend = 1)
        {
            int value = startValue;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            length = (length == 0) ? (section.Count - startIndex) : length;
            int endIndex = startIndex + length;

            int prefixCount = (prefixes == null) ? 0 : prefixes.Count;
            int prefixIndex = 0;

            int index = startIndex;

            while (index < endIndex)
            {
                string valueString = isValueHex ? value.ToString("X2") : value.ToString();
                string prefix = (prefixCount > 0) ? prefixes[prefixIndex] : "";
                string textValue = Utilities.Utilities.XmlEncode(section[index]);

                if (string.IsNullOrEmpty(attributeTitle))
                    sb.AppendFormat("    <{0} {3}=\"{1}\">{4}{2}</{0}>{5}", entryTitle, valueString, textValue, valueTitle, prefix, Environment.NewLine);
                else
                    sb.AppendFormat("    <{0} {4}=\"{1}\" {2}=\"{5}{3}\"/>{6}", entryTitle, valueString, attributeTitle, textValue, valueTitle, prefix, Environment.NewLine);

                if (prefixCount > 0)
                {
                    prefixIndex++;
                    if (prefixIndex >= prefixCount)
                    {
                        prefixIndex = 0;
                        index += indexAddend;
                    }
                }
                else
                {
                    index += indexAddend;
                }
                
                value++;
            }

            return sb.ToString();
        }

        public static string GenerateXMLSectionString(string sectionTitle, IEnumerable<string> entryStrings)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<{0}>{1}", sectionTitle, Environment.NewLine);

            foreach (string entryString in entryStrings)
            {
                sb.Append(entryString);
            }

            sb.AppendFormat("</{0}>{1}", sectionTitle, Environment.NewLine);
            return sb.ToString();
        }

        public static string GenerateXMLSectionString(IList<string> section, string sectionTitle, string entryTitle, string attributeTitle, string valueTitle, bool isValueHex,
            IList<string> prefixes = null, int startIndex = 0, int startValue = 0, int length = 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<{0}>{1}", sectionTitle, Environment.NewLine);
            sb.Append(GenerateXMLSectionEntries(section, entryTitle, attributeTitle, valueTitle, isValueHex, prefixes, startIndex, startValue, length));
            sb.AppendFormat("</{0}>{1}", sectionTitle, Environment.NewLine);
            return sb.ToString();
        }

        public static string GenerateXMLString(IList<string> section, string sectionTitle, string entryTitle, string attributeTitle, string valueTitle, bool isValueHex)
        {
            return GenerateXMLString(new string[1] { GenerateXMLSectionString(section, sectionTitle, entryTitle, attributeTitle, valueTitle, isValueHex) }, null);
        }

        public static string GenerateXMLString(IList<string> sections, string outerTitle)
        {
            bool hasOuterNode = !string.IsNullOrEmpty(outerTitle);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            if (hasOuterNode)
                sb.AppendFormat("<{0}>{1}", outerTitle, Environment.NewLine);
            
            foreach (string section in sections)
            {
                sb.Append(section);
            }

            if (hasOuterNode)
                sb.AppendFormat("</{0}>{1}", outerTitle, Environment.NewLine);

            return sb.ToString();
        }

        public static byte[] GenerateXMLSectionBytes(string sectionTitle, IEnumerable<string> entryStrings, string outerTitle = null)
        {
            return System.Text.Encoding.UTF8.GetBytes(GenerateXMLString(new string[1] { GenerateXMLSectionString(sectionTitle, entryStrings) }, outerTitle));
        }

        public static byte[] GenerateXMLBytes(IList<string> sections, string outerTitle)
        {
            return System.Text.Encoding.UTF8.GetBytes(GenerateXMLString(sections, outerTitle));
        }

        public static byte[] GenerateXMLBytes(IList<string> section, string sectionTitle, string entryTitle, string attributeTitle, string valueTitle, bool isValueHex)
        {
            return System.Text.Encoding.UTF8.GetBytes(GenerateXMLString(section, sectionTitle, entryTitle, attributeTitle, valueTitle, isValueHex));
        }

        public static class Paths
        {
            public const string AbilityFormulasXML = "AbilityFormulas.xml";
            public const string DigestTransform = "digestTransform.xsl";
            private const string ENTD1 = "ENTD1.ENT";
            private const string ENTD2 = "ENTD2.ENT";
            private const string ENTD3 = "ENTD3.ENT";
            private const string ENTD4 = "ENTD4.ENT";
            private const string JobFormationSprites1 = "JobFormationSprites1.bin";
            private const string JobFormationSprites2 = "JobFormationSprites2.bin";
            private const string MoveFindBin = "MoveFind.bin";
            private const string ReactionEffects = "ReactionEffects.bin";
            private const string Propositions = "Propositions.bin";

            public static class PSP
            {
                public static class Binaries
                {
                    public const string ENTD1 = Paths.ENTD1;
                    public const string ENTD2 = Paths.ENTD2;
                    public const string ENTD3 = Paths.ENTD3;
                    public const string ENTD4 = Paths.ENTD4;
                    public const string ENTD5 = "PSP/bin/ENTD5.bin";
                    public const string MoveFind = Paths.MoveFindBin;
                    public const string Propositions = Paths.Propositions;
                    public const string Abilities = "PSP/bin/Abilities.bin";
                    public const string AbilityAnimations = "PSP/bin/AbilityAnimations.bin";
                    public const string AbilityEffects = "PSP/bin/AbilityEffects.bin";
                    public const string ReactionAbilityEffects = Paths.ReactionEffects;
                    public const string ItemAbilityEffects = "PSP/bin/ItemEffects.bin";
                    public const string ActionEvents = "PSP/bin/ActionEvents.bin";
                    public const string Font = "PSP/bin/font.bin";
                    public const string FontWidths = "PSP/bin/FontWidths.bin";
                    public const string ICON0 = "PSP/bin/ICON0";
                    public const string InflictStatuses = "PSP/bin/InflictStatuses.bin";
                    public const string JobFormationSprites1 = Paths.JobFormationSprites1;
                    public const string JobFormationSprites2 = Paths.JobFormationSprites2;
                    public const string JobLevels = "PSP/bin/JobLevels.bin";
                    public const string Jobs = "PSP/bin/Jobs.bin";
                    public const string MonsterSkills = "PSP/bin/MonsterSkills.bin";
                    public const string NewItemAttributes = "PSP/bin/NewItemAttributes.bin";
                    public const string NewItems = "PSP/bin/NewItems.bin";
                    public const string OldItemAttributes = "PSP/bin/OldItemAttributes.bin";
                    public const string OldItems = "PSP/bin/OldItems.bin";
                    public const string PoachProbabilities = "PSP/bin/PoachProbabilities.bin";
                    public const string SkillSets = "PSP/bin/SkillSetsBin.bin";
                    public const string StatusAttributes = "PSP/bin/StatusAttributes.bin";
                    public const string StoreInventories = "StoreInventories.bin";
                }
                public const string PropositionsXML = "PSP/Errands.xml";
                public const string ChroniclesXML = "PSP/Chronicle.xml";
                public const string UnitNamesXML = "PSP/UnitNames.xml";
                public const string EventNamesXML = "PSP/EventNames.xml";
                public const string JobsXML = "PSP/Jobs.xml";
                public const string SkillSetsXML = "PSP/SkillSets.xml";
                public const string MapNamesXML = "PSP/MapNames.xml";
                public const string SpecialNamesXML = "PSP/SpecialNames.xml";
                public const string SpriteSetsXML = "PSP/SpriteSets.xml";
                public const string StatusNamesXML = "PSP/StatusNames.xml";
                public const string AbilitiesNamesXML = "PSP/Abilities/Abilities.xml";
                public const string AbilitiesStringsXML = "PSP/Abilities/Strings.xml";
                public const string AbilityEffectsXML = "PSP/Abilities/Effects.xml";
                public const string ItemAttributesXML = "PSP/Items/ItemAttributes.xml";
                public const string ItemsXML = "PSP/Items/Items.xml";
                public const string ItemsStringsXML = "PSP/Items/Strings.xml";
                public const string ShopNamesXML = "PSP/ShopNames.xml";
                public const string SpriteFilesXML = "PSP/SpriteFiles.xml";
            }

            public static class PSX
            {
                public static class Binaries
                {
                    public const string ENTD1 = Paths.ENTD1;
                    public const string ENTD2 = Paths.ENTD2;
                    public const string ENTD3 = Paths.ENTD3;
                    public const string ENTD4 = Paths.ENTD4;
                    public const string Propositions = Paths.Propositions;
                    public const string MoveFind = Paths.MoveFindBin;
                    public const string Abilities = "PSX-US/bin/Abilities.bin";
                    public const string AbilityAnimations = "PSX-US/bin/AbilityAnimations.bin";
                    public const string AbilityEffects = "PSX-US/bin/AbilityEffects.bin";
                    public const string ReactionAbilityEffects = Paths.ReactionEffects;
                    public const string ItemAbilityEffects = "PSX-US/bin/ItemEffects.bin";
                    public const string ActionEvents = "PSX-US/bin/ActionEvents.bin";
                    public const string Font = "PSX-US/bin/font.bin";
                    public const string FontWidths = "PSX-US/bin/FontWidths.bin";
                    public const string SCEAP = "PSX-US/bin/SCEAP.DAT.patched.bin";
                    public const string InflictStatuses = "PSX-US/bin/InflictStatuses.bin";
                    public const string JobFormationSprites1 = Paths.JobFormationSprites1;
                    public const string JobFormationSprites2 = Paths.JobFormationSprites2;
                    public const string JobLevels = "PSX-US/bin/JobLevels.bin";
                    public const string Jobs = "PSX-US/bin/Jobs.bin";
                    public const string MonsterSkills = "PSX-US/bin/MonsterSkills.bin";
                    public const string OldItemAttributes = "PSX-US/bin/OldItemAttributes.bin";
                    public const string OldItems = "PSX-US/bin/OldItems.bin";
                    public const string PoachProbabilities = "PSX-US/bin/PoachProbabilities.bin";
                    public const string SkillSets = "PSX-US/bin/SkillSetsBin.bin";
                    public const string StatusAttributes = "PSX-US/bin/StatusAttributes.bin";
                    public const string StoreInventories = "StoreInventories.bin";
                }
                public const string PropositionsXML = "PSX-US/Propositions.xml";
                public const string BraveStoryXML = "PSX-US/BraveStory.xml";
                public const string UnitNamesXML = "PSX-US/UnitNames.xml";
                public const string EventNamesXML = "PSX-US/EventNames.xml";
                public const string FileList = "PSX-US/FileList.txt";
                public const string JobsXML = "PSX-US/Jobs.xml";
                public const string SkillSetsXML = "PSX-US/SkillSets.xml";
                public const string SpecialNamesXML = "PSX-US/SpecialNames.xml";
                public const string SpriteSetsXML = "PSX-US/SpriteSets.xml";
                public const string StatusNamesXML = "PSX-US/StatusNames.xml";
                public const string AbilitiesNamesXML = "PSX-US/Abilities/Abilities.xml";
                public const string AbilitiesStringsXML = "PSX-US/Abilities/Strings.xml";
                public const string AbilityEffectsXML = "PSX-US/Abilities/Effects.xml";
                public const string ItemAttributesXML = "PSX-US/Items/ItemAttributes.xml";
                public const string ItemsXML = "PSX-US/Items/Items.xml";
                public const string ItemsStringsXML = "PSX-US/Items/Strings.xml";
                public const string ShopNamesXML = "PSX-US/ShopNames.xml";
                public const string MapNamesXML = "PSX-US/MapNames.xml";
                public const string SpriteFilesXML = "PSX-US/SpriteFiles.xml";
            }
        }
    }
}