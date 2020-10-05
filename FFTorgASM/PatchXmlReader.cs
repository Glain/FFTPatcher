using System;
using System.Collections.Generic;
using System.Xml;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using ASMEncoding;
using System.IO;
using System.Text;
using ASMEncoding.Helpers;

namespace FFTorgASM
{
    public class SpecificLocation
    {
        public PsxIso.Sectors Sector { get; set; }
        public string OffsetString { get; set; }

        public SpecificLocation(PsxIso.Sectors sector, string offsetString)
        {
            Sector = sector;
            OffsetString = offsetString;
        }
    }

    public class GetPatchResult
    {
        public string Name { get; set; }
        public string Description {get; set; }
        public IList<PatchedByteArray> StaticPatches { get; set; }
        public bool HideInDefault { get; set; }
        public string ErrorText { get; set; }

        public GetPatchResult(string Name, string Description, IList<PatchedByteArray> StaticPatches, bool HideInDefault, string ErrorText)
        {
            this.Name = Name;
            this.Description = Description;
            this.StaticPatches = StaticPatches;
            this.HideInDefault = HideInDefault;
            this.ErrorText = ErrorText;
        }
    }
    
    static class PatchXmlReader
    {
        public static bool TryGetPatches( string xmlString, string xmlFilename, ASMEncodingUtility asmUtility, out IList<AsmPatch> patches )
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml( xmlString );
                patches = GetPatches( doc.SelectSingleNode( "/Patches" ), xmlFilename, asmUtility );
                return true;
            }
            catch ( Exception ex )
            {
                patches = null;
                return false;
            }
        }

        private static KeyValuePair<string, string> GetPatchNameAndDescription( XmlNode node )
        {
            string name = node.Attributes["name"].InnerText;
            XmlNode descriptionNode = node.SelectSingleNode( "Description" );
            string description = name;
            if ( descriptionNode != null )
            {
                description = descriptionNode.InnerText;
            }

            return new KeyValuePair<string, string>(name, description);
        }

        private static GetPatchResult GetPatch(XmlNode node, string xmlFileName, ASMEncodingUtility asmUtility, List<VariableType> variables)
        {
            KeyValuePair<string, string> nameDesc = GetPatchNameAndDescription( node );

            bool hideInDefault = false;
            XmlAttribute attrHideInDefault = node.Attributes["hideInDefault"];
            if (attrHideInDefault != null)
            {
                if (attrHideInDefault.InnerText.ToLower().Trim() == "true")
                    hideInDefault = true;
            }

            bool hasDefaultSector = false;
            PsxIso.Sectors defaultSector = (PsxIso.Sectors)0;
            XmlAttribute attrDefaultFile = node.Attributes["file"];
            XmlAttribute attrDefaultSector = node.Attributes["sector"];

            if (attrDefaultFile != null)
            {
                defaultSector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), attrDefaultFile.InnerText);
                hasDefaultSector = true;
            }
            else if (attrDefaultSector != null)
            {
                defaultSector = (PsxIso.Sectors)Int32.Parse(attrDefaultSector.InnerText, System.Globalization.NumberStyles.HexNumber);
                hasDefaultSector = true;
            }

            XmlNodeList currentLocs = node.SelectNodes( "Location" );
            List<PatchedByteArray> patches = new List<PatchedByteArray>( currentLocs.Count );
            StringBuilder sbOuterErrorText = new StringBuilder();

            foreach ( XmlNode location in currentLocs )
            {
                //UInt32 offset = UInt32.Parse( location.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                XmlAttribute offsetAttribute = location.Attributes["offset"];
                XmlAttribute fileAttribute = location.Attributes["file"];
                XmlAttribute sectorAttribute = location.Attributes["sector"];
                XmlAttribute modeAttribute = location.Attributes["mode"];
                XmlAttribute offsetModeAttribute = location.Attributes["offsetMode"];
                XmlAttribute inputFileAttribute = location.Attributes["inputFile"];
                XmlAttribute replaceLabelsAttribute = location.Attributes["replaceLabels"];
                XmlAttribute attrLabel = location.Attributes["label"];
                XmlAttribute attrSpecific = location.Attributes["specific"];
                XmlAttribute attrMovable = location.Attributes["movable"];

                string strOffsetAttr = (offsetAttribute != null) ? offsetAttribute.InnerText : "";
                string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');
                bool ignoreOffsetMode = false;
                bool isSpecific = false;
                bool isSequentialOffset = false;

                List<SpecificLocation> specifics = FillSpecificAttributeData(attrSpecific, defaultSector);

                if (specifics.Count > 0)
                {
                    isSpecific = true;
                    List<string> newStrOffsets = new List<string>(specifics.Count);
                    foreach (SpecificLocation specific in specifics)
                        newStrOffsets.Add(specific.OffsetString);
                    strOffsets = newStrOffsets.ToArray();
                }
                else if ((string.IsNullOrEmpty(strOffsetAttr)) && (patches.Count > 0))
                {
                    // No offset defined -- offset is (last patch offset) + (last patch size)
                    PatchedByteArray lastPatchedByteArray = patches[patches.Count - 1];
                    long offset = lastPatchedByteArray.Offset + lastPatchedByteArray.GetBytes().Length;
                    string strOffset = offset.ToString("X");
                    strOffsets = new string[1] { strOffset };
                    ignoreOffsetMode = true;
                    isSequentialOffset = true;
                }

                PsxIso.Sectors sector = (PsxIso.Sectors)0;
                if (isSpecific)
                {
                    sector = specifics[0].Sector;
                }
                else if (fileAttribute != null)
                {
                    sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), fileAttribute.InnerText );
                }
                else if (sectorAttribute != null)
                {
                    sector = (PsxIso.Sectors)Int32.Parse( sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber );
                }
                else if (hasDefaultSector)
                {
                    sector = defaultSector;
                }
                else if (patches.Count > 0)
                {
                    sector = (PsxIso.Sectors)(patches[patches.Count - 1].Sector);
                }
                else
                {
                    throw new Exception("Error in patch XML: Invalid file/sector!");
                }

                bool asmMode = false;
                bool markedAsData = false;
                if (modeAttribute != null)
                {
                    string modeAttributeText = modeAttribute.InnerText.ToLower().Trim();
                    if (modeAttributeText == "asm")
                	{
                		asmMode = true;
                	}
                    else if (modeAttributeText == "data")
                    {
                        markedAsData = true;
                    }
                }
                
                bool isRamOffset = false;
                if ((!ignoreOffsetMode) && (offsetModeAttribute != null))
                {
                	if (offsetModeAttribute.InnerText.ToLower().Trim() == "ram")
                		isRamOffset = true;
                }

                string content = location.InnerText;
                if (inputFileAttribute != null)
                {
                    using (StreamReader streamReader = new StreamReader(inputFileAttribute.InnerText, Encoding.UTF8))
                    {
                        content = streamReader.ReadToEnd();
                    }
                }

                bool replaceLabels = false;
                if (replaceLabelsAttribute != null)
                {
                    if (replaceLabelsAttribute.InnerText.ToLower().Trim() == "true")
                        replaceLabels = true;
                }
                if (replaceLabels)
                {
                    foreach (PatchedByteArray currentPatchedByteArray in patches)
                    {
                        StringBuilder sbLabels = new StringBuilder();
                        if (!string.IsNullOrEmpty(currentPatchedByteArray.Label))
                            sbLabels.Append(String.Format(".label @{0}, {1}{2}", currentPatchedByteArray.Label, currentPatchedByteArray.RamOffset, Environment.NewLine));
                        asmUtility.EncodeASM(sbLabels.ToString(), 0);
                    }
                    content = asmUtility.ReplaceLabelsInHex(content, true);
                }

                string label = "";
                if (attrLabel != null)
                {
                    label = attrLabel.InnerText.Replace(" ", "");
                }

                bool isMoveSimple = asmMode;
                if (attrMovable != null)
                {
                    bool.TryParse(attrMovable.InnerText, out isMoveSimple);
                }

                int ftrOffset = PsxIso.GetRamOffset(sector);

                int offsetIndex = 0;
                foreach (string strOffset in strOffsets)
                {
                    UInt32 offset = UInt32.Parse(strOffset, System.Globalization.NumberStyles.HexNumber);

                    UInt32 ramOffset = offset;
                    UInt32 fileOffset = offset;

                    if (ftrOffset >= 0)
                    {
                        try
                        {
                            if (isRamOffset)
                                fileOffset -= (UInt32)ftrOffset;
                            else
                                ramOffset += (UInt32)ftrOffset;
                        }
                        catch (Exception) { }
                    }

                    ramOffset = ramOffset | PsxIso.KSeg0Mask;     // KSEG0

                    byte[] bytes;
                    string errorText = "";
                    if (asmMode)
                    {
                        string encodeContent = content;

                        StringBuilder sbPrefix = new StringBuilder();
                        foreach (PatchedByteArray currentPatchedByteArray in patches)
                        {
                            if (!string.IsNullOrEmpty(currentPatchedByteArray.Label))
                                sbPrefix.Append(String.Format(".label @{0}, {1}{2}", currentPatchedByteArray.Label, currentPatchedByteArray.RamOffset, Environment.NewLine));
                        }
                        foreach (VariableType variable in variables)
                        {
                            sbPrefix.Append(String.Format(".eqv %{0}, {1}{2}", ASMStringHelper.RemoveSpaces(variable.name).Replace(",", ""), 
                                AsmPatch.GetUnsignedByteArrayValue_LittleEndian(variable.byteArray), Environment.NewLine));
                        }

                        encodeContent = sbPrefix.ToString() + content;

                        ASMEncoderResult result = asmUtility.EncodeASM(encodeContent, ramOffset);
                        bytes = result.EncodedBytes;
                        errorText = result.ErrorText;
                    }
                    else
                    {
                        bytes = GetBytes(content);
                    }

                    bool isCheckedAsm = false;
                    if (!markedAsData)
                    {
                        ASMCheckResult checkResult = asmUtility.CheckASMFromBytes(bytes, ramOffset, true, false, new HashSet<ASMCheckCondition>() {
                            ASMCheckCondition.LoadDelay,
                            ASMCheckCondition.UnalignedOffset,
                            ASMCheckCondition.MultCountdown,
                            ASMCheckCondition.StackPointerOffset4,
                            ASMCheckCondition.BranchInBranchDelaySlot
                        });

                        if (checkResult.IsASM)
                        {
                            isCheckedAsm = true;
                            if (!string.IsNullOrEmpty(checkResult.ErrorText))
                            {
                                errorText += checkResult.ErrorText;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(errorText))
                        sbOuterErrorText.Append(errorText);

                    PatchedByteArray patchedByteArray = new PatchedByteArray(sector, fileOffset, bytes);
                    patchedByteArray.IsAsm = asmMode;
                    patchedByteArray.MarkedAsData = markedAsData;
                    patchedByteArray.IsCheckedAsm = isCheckedAsm;
                    patchedByteArray.IsSequentialOffset = isSequentialOffset;
                    patchedByteArray.IsMoveSimple = isMoveSimple;
                    patchedByteArray.AsmText = asmMode ? content : "";
                    patchedByteArray.RamOffset = ramOffset;
                    patchedByteArray.ErrorText = errorText;
                    patchedByteArray.Label = label;
                    
                    patches.Add(patchedByteArray);

                    offsetIndex++;
                    if (offsetIndex < strOffsets.Length)
                    {
                        if (isSpecific)
                        {
                            sector = specifics[offsetIndex].Sector;
                            ftrOffset = PsxIso.GetRamOffset(sector);
                        }
                    }
                }
            }

            currentLocs = node.SelectNodes("STRLocation");
            foreach (XmlNode location in currentLocs)
            {
                XmlAttribute fileAttribute = location.Attributes["file"];
                XmlAttribute sectorAttribute = location.Attributes["sector"];
                PsxIso.Sectors sector = (PsxIso.Sectors)0;
                if (fileAttribute != null)
                {
                    sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), fileAttribute.InnerText );
                }
                else if (sectorAttribute != null)
                {
                    sector = (PsxIso.Sectors)Int32.Parse( sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber );
                }
                else
                {
                    throw new Exception();
                }

                string filename = location.Attributes["input"].InnerText;
                filename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( xmlFileName ), filename );

                patches.Add(new STRPatchedByteArray(sector, filename));
            }

            return new GetPatchResult(nameDesc.Key, nameDesc.Value, patches.AsReadOnly(), hideInDefault, sbOuterErrorText.ToString());
        }

        public static IList<AsmPatch> GetPatches( XmlNode rootNode, string xmlFilename, ASMEncodingUtility asmUtility )
        {
            bool rootHideInDefault = false;
            XmlAttribute attrHideInDefault = rootNode.Attributes["hideInDefault"];
            if (attrHideInDefault != null)
            {
                rootHideInDefault = (attrHideInDefault.InnerText.ToLower().Trim() == "true");
            }

            string shortXmlFilename = xmlFilename.Substring(xmlFilename.LastIndexOf("\\") + 1);

            XmlNodeList patchNodes = rootNode.SelectNodes( "Patch" );
            List<AsmPatch> result = new List<AsmPatch>( patchNodes.Count );
            foreach ( XmlNode node in patchNodes )
            {
                XmlAttribute ignoreNode = node.Attributes["ignore"];
                if ( ignoreNode != null && Boolean.Parse( ignoreNode.InnerText ) )
                    continue;

                bool hasDefaultSector = false;
                PsxIso.Sectors defaultSector = (PsxIso.Sectors)0;
                XmlAttribute attrDefaultFile = node.Attributes["file"];
                XmlAttribute attrDefaultSector = node.Attributes["sector"];

                if (attrDefaultFile != null)
                {
                    defaultSector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), attrDefaultFile.InnerText);
                    hasDefaultSector = true;
                }
                else if (attrDefaultSector != null)
                {
                    defaultSector = (PsxIso.Sectors)Int32.Parse(attrDefaultSector.InnerText, System.Globalization.NumberStyles.HexNumber);
                    hasDefaultSector = true;
                }

                List<VariableType> variables = new List<VariableType>();
                List<PatchedByteArray> includePatches = new List<PatchedByteArray>();

                XmlNodeList includeNodes = node.SelectNodes("Include");
                foreach (XmlNode includeNode in includeNodes)
                {
                    XmlAttribute attrPatch = includeNode.Attributes["patch"];
                    if (attrPatch != null)
                    {
                        string patchName = attrPatch.InnerText.ToLower().Trim();
                        foreach (AsmPatch currentAsmPatch in result)
                        {
                            if (currentAsmPatch.Name.ToLower().Trim().Equals(patchName))
                            {
                                foreach (VariableType variable in currentAsmPatch.Variables)
                                {
                                    variables.Add(AsmPatch.CopyVariable(variable));
                                }
                                for (int index = 0; index < currentAsmPatch.NonVariableCount; index++)
                                {
                                    includePatches.Add(currentAsmPatch[index].Copy());
                                }
                            }
                        }
                    }
                }

                foreach ( XmlNode varNode in node.SelectNodes( "Variable" ) )
                {
                	XmlAttribute numBytesAttr = varNode.Attributes["bytes"];
                    string strNumBytes = (numBytesAttr == null) ? "1" : numBytesAttr.InnerText;
                    byte numBytes = (byte)(UInt32.Parse(strNumBytes) & 0xff);
                	
                    string varName = varNode.Attributes["name"].InnerText;

                    XmlAttribute fileAttribute = varNode.Attributes["file"];
                    XmlAttribute sectorAttribute = varNode.Attributes["sector"];
                    XmlAttribute attrSpecific = varNode.Attributes["specific"];

                    //PsxIso.Sectors varSec = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), varNode.Attributes["file"].InnerText );
                    //UInt32 varOffset = UInt32.Parse( varNode.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                    //string strOffsetAttr = varNode.Attributes["offset"].InnerText;
                    XmlAttribute offsetAttribute = varNode.Attributes["offset"];
                    string strOffsetAttr = (offsetAttribute != null) ? offsetAttribute.InnerText : "";
                    string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');
                    bool ignoreOffsetMode = false;
                    bool isSpecific = false;

                    List<SpecificLocation> specifics = FillSpecificAttributeData(attrSpecific, defaultSector);

                    if (specifics.Count > 0)
                    {
                        isSpecific = true;
                        List<string> newStrOffsets = new List<string>(specifics.Count);
                        foreach (SpecificLocation specific in specifics)
                            newStrOffsets.Add(specific.OffsetString);
                        strOffsets = newStrOffsets.ToArray();
                    }
                    else if ((string.IsNullOrEmpty(strOffsetAttr)) && (variables.Count > 0) && (variables[variables.Count - 1].content.Count > 0))
                    {
                        // No offset defined -- offset is (last patch offset) + (last patch size)
                        int lastIndex = variables[variables.Count - 1].content.Count - 1;
                        PatchedByteArray lastPatchedByteArray = variables[variables.Count - 1].content[lastIndex];
                        long offset = lastPatchedByteArray.Offset + lastPatchedByteArray.GetBytes().Length;
                        string strOffset = offset.ToString("X");
                        strOffsets = new string[1] { strOffset };
                        ignoreOffsetMode = true;
                    }

                    PsxIso.Sectors sector = (PsxIso.Sectors)0;
                    if (isSpecific)
                    {
                        sector = specifics[0].Sector;
                    }
                    else if (fileAttribute != null)
                    {
                        sector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), fileAttribute.InnerText);
                    }
                    else if (sectorAttribute != null)
                    {
                        sector = (PsxIso.Sectors)Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber);
                    }
                    else if (hasDefaultSector)
                    {
                        sector = defaultSector;
                    }
                    else if ((variables.Count > 0) && (variables[variables.Count - 1].content.Count > 0))
                    {
                        int lastIndex = variables[variables.Count - 1].content.Count - 1;
                        sector = (PsxIso.Sectors)(variables[variables.Count - 1].content[lastIndex].Sector);
                    }
                    else
                    {
                        throw new Exception("Error in patch XML: Invalid file/sector!");
                    }

                    XmlAttribute offsetModeAttribute = varNode.Attributes["offsetMode"];
                    bool isRamOffset = false;
                    if ((!ignoreOffsetMode) && (offsetModeAttribute != null))
                    {
                        if (offsetModeAttribute.InnerText.ToLower().Trim() == "ram")
                            isRamOffset = true;
                    }

                    int ftrOffset = PsxIso.GetRamOffset(sector);

                    XmlAttribute defaultAttr = varNode.Attributes["default"];
                    Byte[] byteArray = new Byte[numBytes];
                    UInt32 def = 0;
                    if ( defaultAttr != null )
                    {
                        def = UInt32.Parse( defaultAttr.InnerText, System.Globalization.NumberStyles.HexNumber );
                        for (int i = 0; i < numBytes; i++)
                        {
                        	byteArray[i] = (Byte)((def >> (i * 8)) & 0xff);
                        }
                    }

                    List<PatchedByteArray> patchedByteArrayList = new List<PatchedByteArray>();
                    int offsetIndex = 0;

                    foreach (string strOffset in strOffsets)
                    {
                        UInt32 offset = UInt32.Parse(strOffset, System.Globalization.NumberStyles.HexNumber);
                        //UInt32 ramOffset = offset;
                        UInt32 fileOffset = offset;

                        if (ftrOffset >= 0)
                        {
                            try
                            {
                                if (isRamOffset)
                                    fileOffset -= (UInt32)ftrOffset;
                                //else
                                //    ramOffset += (UInt32)ftrOffset;
                            }
                            catch (Exception) { }
                        }

                        //ramOffset = ramOffset | PsxIso.KSeg0Mask;     // KSEG0

                        patchedByteArrayList.Add(new PatchedByteArray(sector, fileOffset, byteArray));

                        offsetIndex++;
                        if (offsetIndex < strOffsets.Length)
                        {
                            if (isSpecific)
                            {
                                sector = specifics[offsetIndex].Sector;
                                ftrOffset = PsxIso.GetRamOffset(sector);
                            }
                        }
                    }

                    bool isReference = false;
                    string referenceName = "";
                    string referenceOperatorSymbol = "";
                    uint referenceOperand = 0;

                    XmlAttribute attrReference = varNode.Attributes["reference"];
                    XmlAttribute attrOperator = varNode.Attributes["operator"];
                    XmlAttribute attrOperand = varNode.Attributes["operand"];
                    
                    if (attrReference != null)
                    {
                        isReference = true;
                        referenceName = attrReference.InnerText;
                        referenceOperatorSymbol = (attrOperator != null) ? attrOperator.InnerText : "";
                        if (attrOperand != null)
                        {
                            //UInt32.Parse(defaultAttr.InnerText, System.Globalization.NumberStyles.HexNumber);
                            uint.TryParse(attrOperand.InnerText, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out referenceOperand);
                        }
                    }

                    VariableType vType = new VariableType();
                    vType.numBytes = numBytes;
                    vType.byteArray = byteArray;
                    vType.name = varName;
                    vType.content = patchedByteArrayList;
                    vType.isReference = isReference;
                    vType.reference = new VariableReference();
                    vType.reference.name = referenceName;
                    vType.reference.operatorSymbol = referenceOperatorSymbol;
                    vType.reference.operand = referenceOperand;

                    variables.Add( vType );
                }

                GetPatchResult getPatchResult = GetPatch(node, xmlFilename, asmUtility, variables);

                List<PatchedByteArray> patches = new List<PatchedByteArray>(includePatches.Count + getPatchResult.StaticPatches.Count);
                patches.AddRange(includePatches);
                patches.AddRange(getPatchResult.StaticPatches);

                AsmPatch asmPatch = new AsmPatch(getPatchResult.Name, shortXmlFilename, getPatchResult.Description, patches, 
                    (getPatchResult.HideInDefault | rootHideInDefault), variables);
                
                asmPatch.ErrorText = getPatchResult.ErrorText;
                result.Add(asmPatch);
            }

            patchNodes = rootNode.SelectNodes( "ImportFilePatch" );
            foreach ( XmlNode node in patchNodes )
            {
                KeyValuePair<string, string> nameDesc = GetPatchNameAndDescription(node);

                string name = nameDesc.Key;
                string description = nameDesc.Value;

                XmlNodeList fileNodes = node.SelectNodes( "ImportFile" );
                if ( fileNodes.Count != 1 ) continue;

                XmlNode theRealNode = fileNodes[0];

                PsxIso.Sectors sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), theRealNode.Attributes["file"].InnerText );
                UInt32 offset = UInt32.Parse( theRealNode.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                UInt32 expectedLength = UInt32.Parse( theRealNode.Attributes["expectedLength"].InnerText, System.Globalization.NumberStyles.HexNumber );

                result.Add(new FileAsmPatch(name, shortXmlFilename, description, new InputFilePatch(sector, offset, expectedLength)));

            }

            return result.AsReadOnly();

        }

        public static string CreatePatchXML(IEnumerable<AsmPatch> patches)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<Patches>");

            foreach (AsmPatch patch in patches)
            {
                sb.Append(patch.CreateXML());
            }

            sb.AppendLine("</Patches>");

            return sb.ToString();
        }

        private static List<SpecificLocation> FillSpecificAttributeData(XmlAttribute attrSpecific, PsxIso.Sectors defaultSector)
        {
            string strSpecificAttr = (attrSpecific != null) ? attrSpecific.InnerText : "";
            string[] strSpecifics = ASMStringHelper.RemoveSpaces(strSpecificAttr).Split(',');
            List<SpecificLocation> specifics = new List<SpecificLocation>();

            PsxIso.Sectors lastSector = defaultSector;
            if (!string.IsNullOrEmpty(strSpecificAttr))
            {
                foreach (string strSpecific in strSpecifics)
                {
                    string[] strSpecificData = strSpecific.Split(':');

                    string strSector = "";
                    string strSpecificOffset = strSpecificData[0];
                    if (strSpecificData.Length > 1)
                    {
                        strSector = strSpecificData[0];
                        strSpecificOffset = strSpecificData[1];
                    }

                    PsxIso.Sectors specificSector;
                    if (string.IsNullOrEmpty(strSector))
                        specificSector = lastSector;
                    else
                    {
                        int sectorNum = 0;
                        bool isSectorNumeric = int.TryParse(strSector, out sectorNum);
                        specificSector = isSectorNumeric ? (PsxIso.Sectors)sectorNum : (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), strSector);
                    }

                    lastSector = specificSector;
                    specifics.Add(new SpecificLocation(specificSector, strSpecificOffset));
                }
            }

            return specifics;
        }

        private static byte[] GetBytes(string byteText)
        {
            return PatcherLib.Utilities.Utilities.GetBytesFromHexString(byteText);
        }
    }
}
