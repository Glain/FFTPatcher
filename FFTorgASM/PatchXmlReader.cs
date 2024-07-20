using System;
using System.Collections.Generic;
using System.Xml;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using ASMEncoding;
using System.IO;
using System.Text;
using ASMEncoding.Helpers;
using PatcherLib.Helpers;
using PatcherLib.Utilities;

namespace FFTorgASM
{
    public class SpecificLocation
    {
        public Enum Sector { get; set; }
        public string OffsetString { get; set; }

        public SpecificLocation(Enum sector, string offsetString)
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
        public bool IsHidden { get; set; }
        public string ErrorText { get; set; }

        public GetPatchResult(string Name, string Description, IList<PatchedByteArray> StaticPatches, bool HideInDefault, bool IsHidden, string ErrorText)
        {
            this.Name = Name;
            this.Description = Description;
            this.StaticPatches = StaticPatches;
            this.HideInDefault = HideInDefault;
            this.IsHidden = IsHidden;
            this.ErrorText = ErrorText;
        }
    }

    internal class TryGetPatchesResult
    { 
        public bool IsSuccess { get; set; }
        public string ErrorText { get; set; }

        public TryGetPatchesResult(bool isSuccess, string errorText)
        {
            this.IsSuccess = isSuccess;
            this.ErrorText = errorText;
        }
    }
    
    static class PatchXmlReader
    {
        public static TryGetPatchesResult TryGetPatches( string xmlString, string xmlFilename, ASMEncodingUtility asmUtility, out IList<AsmPatch> patches )
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml( xmlString );
                patches = GetPatches( doc.SelectSingleNode( "/Patches" ), xmlFilename, asmUtility );
                return new TryGetPatchesResult(true, string.Empty);
            }
            catch ( Exception ex )
            {
                patches = null;

                while (ex.InnerException != null)
                    ex = ex.InnerException;

                return new TryGetPatchesResult(false, ex.Message);
            }
        }

        private static KeyValuePair<string, string> GetPatchNameAndDescription( XmlNode node )
        {
            string name = Utilities.GetCaseInsensitiveAttribute(node, "name").InnerText;
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
            XmlAttribute attrHideInDefault = Utilities.GetCaseInsensitiveAttribute(node, "hideInDefault");
            if (attrHideInDefault != null)
            {
                if (attrHideInDefault.InnerText.ToLower().Trim() == "true")
                    hideInDefault = true;
            }

            bool isHidden = false;
            XmlAttribute attrIsHidden = Utilities.GetCaseInsensitiveAttribute(node, "hidden");
            if (attrIsHidden != null)
            {
                if (attrIsHidden.InnerText.ToLower().Trim() == "true")
                    isHidden = true;
            }

            bool hasDefaultSector = false;
            //PsxIso.Sectors defaultSector = (PsxIso.Sectors)0;
            Context context = (asmUtility.EncodingMode == ASMEncodingMode.PSP) ? Context.US_PSP : Context.US_PSX;
            Type sectorType = ISOHelper.GetSectorType(context);

            Enum defaultSector = ISOHelper.GetSector(0, context); // (Enum)Enum.ToObject(sectorType, 0);
            XmlAttribute attrDefaultFile = Utilities.GetCaseInsensitiveAttribute(node, "file");
            XmlAttribute attrDefaultSector = Utilities.GetCaseInsensitiveAttribute(node, "sector");

            if (attrDefaultFile != null)
            {
                //defaultSector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), attrDefaultFile.InnerText);
                //defaultSector = (Enum)Enum.Parse(sectorType, attrDefaultFile.InnerText);
                defaultSector = ISOHelper.GetSector(attrDefaultFile.InnerText, context);
                hasDefaultSector = true;
            }
            else if (attrDefaultSector != null)
            {
                //defaultSector = (PsxIso.Sectors)Int32.Parse(attrDefaultSector.InnerText, System.Globalization.NumberStyles.HexNumber);
                defaultSector = ISOHelper.GetSectorHex(attrDefaultSector.InnerText, context);
                hasDefaultSector = true;
            }

            XmlNodeList currentLocs = node.SelectNodes( "Location" );
            List<PatchedByteArray> patches = new List<PatchedByteArray>( currentLocs.Count );
            StringBuilder sbOuterErrorText = new StringBuilder();

            Dictionary<PatchedByteArray, string> replaceLabelsContentMap = new Dictionary<PatchedByteArray, string>();

            foreach ( XmlNode location in currentLocs )
            {
                XmlAttribute offsetAttribute = Utilities.GetCaseInsensitiveAttribute(location, "offset");
                XmlAttribute fileAttribute = Utilities.GetCaseInsensitiveAttribute(location, "file");
                XmlAttribute sectorAttribute = Utilities.GetCaseInsensitiveAttribute(location, "sector");
                XmlAttribute modeAttribute = Utilities.GetCaseInsensitiveAttribute(location, "mode");
                XmlAttribute offsetModeAttribute = Utilities.GetCaseInsensitiveAttribute(location, "offsetMode");
                XmlAttribute inputFileAttribute = Utilities.GetCaseInsensitiveAttribute(location, "inputFile");
                XmlAttribute replaceLabelsAttribute = Utilities.GetCaseInsensitiveAttribute(location, "replaceLabels");
                XmlAttribute attrLabel = Utilities.GetCaseInsensitiveAttribute(location, "label");
                XmlAttribute attrSpecific = Utilities.GetCaseInsensitiveAttribute(location, "specific");
                XmlAttribute attrMovable = Utilities.GetCaseInsensitiveAttribute(location, "movable");
                XmlAttribute attrAlign = Utilities.GetCaseInsensitiveAttribute(location, "align");
                XmlAttribute attrStatic = Utilities.GetCaseInsensitiveAttribute(location, "static");
                XmlAttribute attrBinaryFile = Utilities.GetCaseInsensitiveAttribute(location, "binaryFile");
                XmlAttribute attrWriteMask = Utilities.GetCaseInsensitiveAttribute(location, "writeMask");

                string strOffsetAttr = (offsetAttribute != null) ? offsetAttribute.InnerText : "";
                string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');
                bool ignoreOffsetMode = false;
                bool isSpecific = false;
                bool isSequentialOffset = false;

                List<SpecificLocation> specifics = FillSpecificAttributeData(attrSpecific, defaultSector);

                bool isAsmMode = false;
                bool markedAsData = false;
                if (modeAttribute != null)
                {
                    string modeAttributeText = modeAttribute.InnerText.ToLower().Trim();
                    if (modeAttributeText == "asm")
                    {
                        isAsmMode = true;
                    }
                    else if (modeAttributeText == "data")
                    {
                        markedAsData = true;
                    }
                }

                int align = 0;
                if (attrAlign != null)
                {
                    Int32.TryParse(attrAlign.InnerText, out align);

                    if (align < 0)
                        align = 0;
                }
                else if (isAsmMode)
                {
                    align = 4;
                }

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
                    ignoreOffsetMode = true;
                    isSequentialOffset = true;

                    // Advance offset to match up with alignment, if necessary
                    if (align > 0)
                    {
                        int offsetAlign = (int)(offset % align);
                        if (offsetAlign > 0)
                        {
                            offset += (align - offsetAlign);
                            isSequentialOffset = false;
                        }
                    }

                    string strOffset = offset.ToString("X");
                    strOffsets = new string[1] { strOffset };
                }

                //PsxIso.Sectors sector = (PsxIso.Sectors)0;
                Enum sector = ISOHelper.GetSector(0, context); // (Enum)Enum.ToObject(sectorType, 0);
                if (isSpecific)
                {
                    sector = specifics[0].Sector;
                }
                else if (fileAttribute != null)
                {
                    //sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), fileAttribute.InnerText );
                    //sector = (Enum)Enum.Parse( sectorType, fileAttribute.InnerText );
                    sector = ISOHelper.GetSector(fileAttribute.InnerText, context);
                }
                else if (sectorAttribute != null)
                {
                    //sector = (PsxIso.Sectors)Int32.Parse( sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber );
                    //sector = (Enum)Enum.ToObject(sectorType, Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber));
                    sector = ISOHelper.GetSectorHex(sectorAttribute.InnerText, context);
                }
                else if (hasDefaultSector)
                {
                    sector = defaultSector;
                }
                else if (patches.Count > 0)
                {
                    //sector = (PsxIso.Sectors)(patches[patches.Count - 1].Sector);
                    //sector = (Enum)Enum.ToObject(sectorType, patches[patches.Count - 1].Sector);
                    sector = patches[patches.Count - 1].SectorEnum;
                }
                else
                {
                    sbOuterErrorText.AppendLine("Error in patch XML: Invalid file/sector!");
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
                    try
                    {
                        string strMode = Enum.GetName(typeof(ASMEncodingMode), asmUtility.EncodingMode);
                        string readPath = Path.Combine("Include", inputFileAttribute.InnerText);
                        FileInfo fileInfo = new FileInfo(xmlFileName);
                        readPath = Path.Combine(fileInfo.DirectoryName, readPath);
                        using (StreamReader streamReader = new StreamReader(readPath, Encoding.UTF8))
                        {
                            content = streamReader.ReadToEnd();
                        }
                    }
                    catch (Exception)
                    {
                        string readPath = inputFileAttribute.InnerText;
                        using (StreamReader streamReader = new StreamReader(readPath, Encoding.UTF8))
                        {
                            content = streamReader.ReadToEnd();
                        }
                    }
                }

                bool isBinaryContent = false;
                byte[] binaryContent = null;
                if (attrBinaryFile != null)
                {
                    isBinaryContent = true;
                    content = "";

                    try
                    {
                        string strMode = Enum.GetName(typeof(ASMEncodingMode), asmUtility.EncodingMode);
                        string readPath = Path.Combine("Include", attrBinaryFile.InnerText);
                        FileInfo fileInfo = new FileInfo(xmlFileName);
                        readPath = Path.Combine(fileInfo.DirectoryName, readPath);
                        binaryContent = File.ReadAllBytes(readPath);
                    }
                    catch (Exception)
                    {
                        string readPath = attrBinaryFile.InnerText;
                        binaryContent = File.ReadAllBytes(readPath);
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
                    StringBuilder sbLabels = new StringBuilder();
                    foreach (PatchedByteArray currentPatchedByteArray in patches)
                    {
                        if (!string.IsNullOrEmpty(currentPatchedByteArray.Label))
                            sbLabels.Append(String.Format(".label @{0}, {1}{2}", currentPatchedByteArray.Label, currentPatchedByteArray.RamOffset, Environment.NewLine));
                    }
                    asmUtility.EncodeASM(sbLabels.ToString(), 0);
                    //content = asmUtility.ReplaceLabelsInHex(content, true, true);
                    content = asmUtility.ReplaceLabelsInHex(content, true, false);
                }

                string label = "";
                if (attrLabel != null)
                {
                    label = attrLabel.InnerText.Replace(" ", "");
                }

                bool isMoveSimple = isAsmMode;
                if (attrMovable != null)
                {
                    bool.TryParse(attrMovable.InnerText, out isMoveSimple);
                }

                bool isStatic = false;
                if (attrStatic != null)
                {
                    bool.TryParse(attrStatic.InnerText, out isStatic);
                }

                byte maskWrite = 0;
                if (attrWriteMask != null)
                {
                    byte.TryParse(attrWriteMask.InnerText, System.Globalization.NumberStyles.HexNumber, 
                        System.Globalization.CultureInfo.CurrentCulture, out maskWrite);
                }

                int ftrOffset = ISOHelper.GetFileToRamOffset(sector, context);

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
                            {
                                if (offset > (UInt32)ftrOffset)
                                    fileOffset -= (UInt32)ftrOffset;
                                else
                                {
                                    string message = String.Format("RAM offset (0x{0}) invalid for sector {1}!", ramOffset.ToString("X"), ISOHelper.GetSectorName(sector));
                                    sbOuterErrorText.AppendLine(message);
                                }
                            }
                            else
                                ramOffset += (UInt32)ftrOffset;
                        }
                        catch (Exception) { }
                    }

                    if (context == Context.US_PSX)
                        ramOffset = ramOffset | PsxIso.KSeg0Mask;

                    byte[] bytes;
                    string errorText = "";
                    if (isBinaryContent)
                    {
                        bytes = binaryContent;
                    }
                    else if (isAsmMode)
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
                            sbPrefix.Append(String.Format(".eqv %{0}, {1}{2}", ASMStringHelper.RemoveSpaces(variable.Name).Replace(",", ""),
                                PatcherLib.Utilities.Utilities.GetUnsignedByteArrayValue_LittleEndian(variable.ByteArray), Environment.NewLine));
                        }

                        encodeContent = sbPrefix.ToString() + content;

                        ASMEncoderResult result = asmUtility.EncodeASM(encodeContent, ramOffset, true);
                        bytes = result.EncodedBytes;
                        errorText = result.ErrorText;
                    }
                    else if (!replaceLabels)
                    {
                        AsmPatch.GetBytesResult result = AsmPatch.GetBytes(content, ramOffset, variables);
                        bytes = result.Bytes;
                        errorText = result.ErrorMessage;
                    }
                    else
                    {
                        bytes = new byte[0];
                    }

                    /*
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
                    */

                    //if (!string.IsNullOrEmpty(errorText))
                    //    sbOuterErrorText.Append(errorText);

                    PatchedByteArray patchedByteArray = new PatchedByteArray(sector, fileOffset, bytes);
                    patchedByteArray.IsAsm = isAsmMode;
                    patchedByteArray.MarkedAsData = markedAsData;
                    patchedByteArray.IsCheckedAsm = false; // isCheckedAsm;
                    patchedByteArray.IsSequentialOffset = isSequentialOffset;
                    patchedByteArray.IsMoveSimple = isMoveSimple;
                    //patchedByteArray.AsmText = isAsmMode ? content : "";
                    patchedByteArray.Text = content;
                    patchedByteArray.RamOffset = ramOffset;
                    patchedByteArray.ErrorText = errorText;
                    patchedByteArray.Label = label;
                    patchedByteArray.IsStatic = isStatic;
                    patchedByteArray.MaskWrite = maskWrite;

                    if (replaceLabels)
                        replaceLabelsContentMap.Add(patchedByteArray, content);
                    
                    patches.Add(patchedByteArray);

                    offsetIndex++;
                    if (offsetIndex < strOffsets.Length)
                    {
                        if (isSpecific)
                        {
                            sector = specifics[offsetIndex].Sector;
                            ftrOffset = ISOHelper.GetFileToRamOffset(sector, context);
                        }
                    }
                }
            }

            StringBuilder sbEncodePrefix = new StringBuilder();
            foreach (PatchedByteArray currentPatchedByteArray in patches)
            {
                if (!string.IsNullOrEmpty(currentPatchedByteArray.Label))
                    sbEncodePrefix.Append(String.Format(".label @{0}, {1}{2}", currentPatchedByteArray.Label, currentPatchedByteArray.RamOffset, Environment.NewLine));
            }
            foreach (VariableType variable in variables)
            {
                sbEncodePrefix.Append(String.Format(".eqv %{0}, {1}{2}", ASMStringHelper.RemoveSpaces(variable.Name).Replace(",", ""),
                    PatcherLib.Utilities.Utilities.GetUnsignedByteArrayValue_LittleEndian(variable.ByteArray), Environment.NewLine));
            }
            string strEncodePrefix = sbEncodePrefix.ToString();
            asmUtility.EncodeASM(strEncodePrefix, 0);

            foreach (PatchedByteArray patchedByteArray in patches)
            {
                string errorText = string.Empty;

                if (patchedByteArray.IsAsm)
                {
                    string encodeContent = strEncodePrefix + patchedByteArray.Text;
                    ASMEncoderResult result = asmUtility.EncodeASM(encodeContent, (uint)patchedByteArray.RamOffset);
                    patchedByteArray.SetBytes(result.EncodedBytes);
                    errorText += result.ErrorText;
                }

                if (!patchedByteArray.MarkedAsData)
                {
                    HashSet<ASMCheckCondition> checkConditions = new HashSet<ASMCheckCondition>() {
                        ASMCheckCondition.LoadDelay,
                        ASMCheckCondition.UnalignedOffset,
                        ASMCheckCondition.MultCountdown,
                        ASMCheckCondition.StackPointerOffset4,
                        ASMCheckCondition.BranchInBranchDelaySlot
                    };

                    if (asmUtility.EncodingMode == ASMEncodingMode.PSP)
                        checkConditions.Remove(ASMCheckCondition.LoadDelay);

                    ASMCheckResult checkResult = asmUtility.CheckASMFromBytes(patchedByteArray.GetBytes(), (uint)patchedByteArray.RamOffset, true, false, checkConditions);

                    if (checkResult.IsASM)
                    {
                        patchedByteArray.IsCheckedAsm = true;
                        if (!string.IsNullOrEmpty(checkResult.ErrorText))
                        {
                            errorText += checkResult.ErrorText;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(errorText))
                    sbOuterErrorText.Append(errorText);
            }

            foreach (PatchedByteArray patchedByteArray in patches)
            {
                string errorText = string.Empty;

                string replaceLabelsContent;
                if (replaceLabelsContentMap.TryGetValue(patchedByteArray, out replaceLabelsContent))
                {
                    if (!string.IsNullOrEmpty(replaceLabelsContent))
                    {
                        string newText = asmUtility.ReplaceLabelsInHex(replaceLabelsContent, true, false);
                        AsmPatch.GetBytesResult result = AsmPatch.GetBytes(newText, (uint)patchedByteArray.RamOffset, variables);
                        patchedByteArray.SetBytes(result.Bytes);
                        patchedByteArray.Text = newText;
                        errorText += result.ErrorMessage;
                    }
                }

                if (!string.IsNullOrEmpty(errorText))
                    sbOuterErrorText.Append(errorText);
            }

            currentLocs = node.SelectNodes("STRLocation");
            foreach (XmlNode location in currentLocs)
            {
                XmlAttribute fileAttribute = Utilities.GetCaseInsensitiveAttribute(location, "file");
                XmlAttribute sectorAttribute = Utilities.GetCaseInsensitiveAttribute(location, "sector");

                //PsxIso.Sectors sector = (PsxIso.Sectors)0;
                Enum sector = ISOHelper.GetSector(0, context); // (Enum)Enum.ToObject(sectorType, 0);

                if (fileAttribute != null)
                {
                    //sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), fileAttribute.InnerText );
                    //sector = (Enum)Enum.Parse(sectorType, fileAttribute.InnerText);
                    sector = ISOHelper.GetSector(fileAttribute.InnerText, context);
                }
                else if (sectorAttribute != null)
                {
                    //sector = (PsxIso.Sectors)Int32.Parse( sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber );
                    //sector = (Enum)Enum.ToObject(sectorType, Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber));
                    sector = ISOHelper.GetSectorHex(sectorAttribute.InnerText, context);
                }
                else
                {
                    throw new Exception();
                }

                string filename = Utilities.GetCaseInsensitiveAttribute(location, "input").InnerText;
                filename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( xmlFileName ), filename );

                patches.Add(new STRPatchedByteArray(sector, filename));
            }

            return new GetPatchResult(nameDesc.Key, nameDesc.Value, patches.AsReadOnly(), hideInDefault, isHidden, sbOuterErrorText.ToString());
        }

        public static IList<AsmPatch> GetPatches( XmlNode rootNode, string xmlFilename, ASMEncodingUtility asmUtility )
        {
            bool rootHideInDefault = false;
            XmlAttribute attrHideInDefault = Utilities.GetCaseInsensitiveAttribute(rootNode, "hideInDefault");
            if (attrHideInDefault != null)
            {
                rootHideInDefault = (attrHideInDefault.InnerText.ToLower().Trim() == "true");
            }

            bool rootIsHidden = false;
            XmlAttribute attrIsHidden = Utilities.GetCaseInsensitiveAttribute(rootNode, "hidden");
            if (attrIsHidden != null)
            {
                rootIsHidden = (attrIsHidden.InnerText.ToLower().Trim() == "true");
            }

            string shortXmlFilename = xmlFilename.Substring(xmlFilename.LastIndexOf("\\") + 1);

            XmlNodeList patchNodes = rootNode.SelectNodes( "Patch" );
            List<AsmPatch> result = new List<AsmPatch>( patchNodes.Count );

            Context context = (asmUtility.EncodingMode == ASMEncodingMode.PSP) ? Context.US_PSP : Context.US_PSX;
            Type sectorType = ISOHelper.GetSectorType(context);
            Enum defaultSector = ISOHelper.GetSector(0, context);

            foreach ( XmlNode node in patchNodes )
            {
                XmlAttribute ignoreNode = Utilities.GetCaseInsensitiveAttribute(node, "ignore");
                if ( ignoreNode != null && Boolean.Parse( ignoreNode.InnerText ) )
                    continue;

                bool hasDefaultSector = false;

                //PsxIso.Sectors defaultSector = (PsxIso.Sectors)0;
                //Enum defaultSector = (Enum)Enum.ToObject(sectorType, 0);
                XmlAttribute attrDefaultFile = Utilities.GetCaseInsensitiveAttribute(node, "file");
                XmlAttribute attrDefaultSector = Utilities.GetCaseInsensitiveAttribute(node, "sector");

                if (attrDefaultFile != null)
                {
                    //defaultSector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), attrDefaultFile.InnerText);
                    //defaultSector = (Enum)Enum.Parse(sectorType, attrDefaultFile.InnerText);
                    defaultSector = ISOHelper.GetSector(attrDefaultFile.InnerText, context);
                    hasDefaultSector = true;
                }
                else if (attrDefaultSector != null)
                {
                    //defaultSector = (PsxIso.Sectors)Int32.Parse(attrDefaultSector.InnerText, System.Globalization.NumberStyles.HexNumber);
                    //defaultSector = (Enum)Enum.ToObject(sectorType, Int32.Parse(attrDefaultSector.InnerText, System.Globalization.NumberStyles.HexNumber));
                    defaultSector = ISOHelper.GetSectorHex(attrDefaultSector.InnerText, context);
                    hasDefaultSector = true;
                }

                StringBuilder sbPatchErrorText = new StringBuilder();

                List<VariableType> variables = new List<VariableType>();
                List<PatchedByteArray> includePatches = new List<PatchedByteArray>();

                XmlNodeList includeNodes = node.SelectNodes("Include");
                foreach (XmlNode includeNode in includeNodes)
                {
                    XmlAttribute attrPatch = Utilities.GetCaseInsensitiveAttribute(includeNode, "patch");
                    if (attrPatch != null)
                    {
                        string patchName = attrPatch.InnerText.ToLower().Trim();
                        int foundPatchCount = 0;

                        foreach (AsmPatch currentAsmPatch in result)
                        {
                            if (currentAsmPatch.Name.ToLower().Trim().Equals(patchName))
                            {
                                foreach (VariableType variable in currentAsmPatch.Variables)
                                {
                                    variables.Add(variable.Copy());
                                }
                                for (int index = 0; index < currentAsmPatch.NonVariableCount; index++)
                                {
                                    includePatches.Add(currentAsmPatch[index].Copy());
                                }
                                foundPatchCount++;
                            }
                        }

                        if (foundPatchCount == 0)
                        {
                            sbPatchErrorText.AppendLine("Error in patch XML: Missing dependent patch \"" + attrPatch.InnerText + "\"!");
                        }
                    }
                }

                foreach ( XmlNode varNode in node.SelectNodes( "Variable" ) )
                {
                	XmlAttribute numBytesAttr = Utilities.GetCaseInsensitiveAttribute(varNode, "bytes");
                    string strNumBytes = (numBytesAttr == null) ? "1" : numBytesAttr.InnerText;
                    byte numBytes = (byte)(UInt32.Parse(strNumBytes) & 0xff);
                	
                    string varName = Utilities.GetCaseInsensitiveAttribute(varNode, "name").InnerText;

                    XmlAttribute fileAttribute = Utilities.GetCaseInsensitiveAttribute(varNode, "file");
                    XmlAttribute sectorAttribute = Utilities.GetCaseInsensitiveAttribute(varNode, "sector");
                    XmlAttribute attrSpecific = Utilities.GetCaseInsensitiveAttribute(varNode, "specific");
                    XmlAttribute attrAlign = Utilities.GetCaseInsensitiveAttribute(varNode, "align");

                    XmlAttribute offsetAttribute = Utilities.GetCaseInsensitiveAttribute(varNode, "offset");
                    string strOffsetAttr = (offsetAttribute != null) ? offsetAttribute.InnerText : "";
                    string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');
                    bool ignoreOffsetMode = false;
                    bool isSpecific = false;

                    List<SpecificLocation> specifics = FillSpecificAttributeData(attrSpecific, defaultSector);

                    int align = 0;
                    if (attrAlign != null)
                    {
                        Int32.TryParse(sectorAttribute.InnerText, out align);

                        if (align < 0)
                            align = 0;
                    }

                    XmlAttribute symbolAttribute = Utilities.GetCaseInsensitiveAttribute(varNode, "symbol");
                    bool isSymbol = (symbolAttribute != null) && PatcherLib.Utilities.Utilities.ParseBool(symbolAttribute.InnerText);

                    if (isSymbol)
                    {
                        strOffsets = new string[0];
                    }
                    else if (specifics.Count > 0)
                    {
                        isSpecific = true;
                        List<string> newStrOffsets = new List<string>(specifics.Count);
                        foreach (SpecificLocation specific in specifics)
                            newStrOffsets.Add(specific.OffsetString);
                        strOffsets = newStrOffsets.ToArray();
                    }
                    else if ((string.IsNullOrEmpty(strOffsetAttr)) && (variables.Count > 0) && (variables[variables.Count - 1].Content.Count > 0))
                    {
                        // No offset defined -- offset is (last patch offset) + (last patch size)
                        int lastIndex = variables[variables.Count - 1].Content.Count - 1;
                        PatchedByteArray lastPatchedByteArray = variables[variables.Count - 1].Content[lastIndex];
                        long offset = lastPatchedByteArray.Offset + lastPatchedByteArray.GetBytes().Length;
                        string strOffset = offset.ToString("X");
                        strOffsets = new string[1] { strOffset };
                        ignoreOffsetMode = true;

                        // Advance offset to match up with alignment, if necessary
                        if (align > 0)
                        {
                            int offsetAlign = (int)(offset % align);
                            if (offsetAlign > 0)
                                offset += (align - offsetAlign);
                        }
                    }

                    //PsxIso.Sectors sector = (PsxIso.Sectors)0;
                    Enum sector = ISOHelper.GetSector(0, context); // (Enum)Enum.ToObject(sectorType, 0);
                    if (isSpecific)
                    {
                        sector = specifics[0].Sector;
                    }
                    else if (fileAttribute != null)
                    {
                        //sector = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), fileAttribute.InnerText);
                        //sector = (Enum)Enum.Parse(sectorType, fileAttribute.InnerText);
                        sector = ISOHelper.GetSector(fileAttribute.InnerText, context);
                    }
                    else if (sectorAttribute != null)
                    {
                        //sector = (PsxIso.Sectors)Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber);
                        //sector = (Enum)Enum.ToObject(sectorType, Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber));
                        sector = ISOHelper.GetSectorHex(sectorAttribute.InnerText, context);
                    }
                    else if (hasDefaultSector)
                    {
                        sector = defaultSector;
                    }
                    else if ((variables.Count > 0) && (variables[variables.Count - 1].Content.Count > 0))
                    {
                        int lastIndex = variables[variables.Count - 1].Content.Count - 1;
                        //sector = (PsxIso.Sectors)(variables[variables.Count - 1].Content[lastIndex].Sector);
                        //sector = (Enum)Enum.ToObject(sectorType, variables[variables.Count - 1].Content[lastIndex].Sector);
                        sector = variables[variables.Count - 1].Content[lastIndex].SectorEnum;
                    }
                    else if (!isSymbol)
                    {
                        sbPatchErrorText.AppendLine("Error in patch XML: Invalid file/sector!");
                    }

                    XmlAttribute offsetModeAttribute = Utilities.GetCaseInsensitiveAttribute(varNode, "offsetMode");
                    bool isRamOffset = false;
                    if ((!ignoreOffsetMode) && (offsetModeAttribute != null))
                    {
                        if (offsetModeAttribute.InnerText.ToLower().Trim() == "ram")
                            isRamOffset = true;
                    }

                    int ftrOffset = ISOHelper.GetFileToRamOffset(sector, context);

                    XmlAttribute defaultAttr = Utilities.GetCaseInsensitiveAttribute(varNode, "default");
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
                                {
                                    if (offset > (UInt32)ftrOffset)
                                        fileOffset -= (UInt32)ftrOffset;
                                    else
                                    {
                                        string message = String.Format("Variable ({0}): RAM offset (0x{1}) invalid for sector {2}!", varName, offset.ToString("X"), ISOHelper.GetSectorName(sector));
                                        sbPatchErrorText.AppendLine(message);
                                    }
                                }
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
                                ftrOffset = ISOHelper.GetFileToRamOffset(sector, context);
                            }
                        }
                    }

                    bool isReference = false;
                    string referenceName = "";
                    string referenceOperatorSymbol = "";
                    uint referenceOperand = 0;

                    XmlAttribute attrReference = Utilities.GetCaseInsensitiveAttribute(varNode, "reference");
                    XmlAttribute attrOperator = Utilities.GetCaseInsensitiveAttribute(varNode, "operator");
                    XmlAttribute attrOperand = Utilities.GetCaseInsensitiveAttribute(varNode, "operand");
                    
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

                    List<VariableType.VariablePreset> presetValueList = new List<VariableType.VariablePreset>();
                    XmlNodeList presetNodeList = varNode.SelectNodes("Preset");

                    string presetKey = null;
                    XmlAttribute attrPreset = Utilities.GetCaseInsensitiveAttribute(varNode, "preset");
                    if (attrPreset != null)
                    {
                        presetKey = attrPreset.InnerText;
                        if (!string.IsNullOrEmpty(presetKey))
                        {
                            presetValueList = VariableType.VariablePreset.TypeMap[presetKey];
                        }
                    }
                    else if (presetNodeList != null)
                    {
                        foreach (XmlNode presetNode in presetNodeList)
                        {
                            XmlAttribute attrName = Utilities.GetCaseInsensitiveAttribute(presetNode, "name");
                            XmlAttribute attrValue = Utilities.GetCaseInsensitiveAttribute(presetNode, "value");
                            XmlAttribute attrModify = Utilities.GetCaseInsensitiveAttribute(presetNode, "modify");
                            UInt32 value = 0;

                            byte[] valueBytes = new Byte[numBytes];
                            if (attrValue != null)
                            {
                                UInt32.TryParse(attrValue.InnerText, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out value);
                                for (int i = 0; i < numBytes; i++)
                                {
                                    valueBytes[i] = (byte)((value >> (i * 8)) & 0xff);
                                }
                            }

                            bool isModifiable = false;
                            if (attrModify != null)
                            {
                                bool.TryParse(attrModify.InnerText, out isModifiable);
                            }

                            presetValueList.Add(new VariableType.VariablePreset(attrName.InnerText, value, valueBytes, isModifiable));
                        }
                    }

                    VariableType vType = new VariableType();
                    vType.NumBytes = numBytes;
                    vType.ByteArray = byteArray;
                    vType.Name = varName;
                    vType.Content = patchedByteArrayList;
                    vType.IsReference = isReference;
                    vType.Reference = new VariableReference();
                    vType.Reference.Name = referenceName;
                    vType.Reference.OperatorSymbol = referenceOperatorSymbol;
                    vType.Reference.Operand = referenceOperand;
                    vType.PresetValues = presetValueList;

                    variables.Add( vType );
                }

                GetPatchResult getPatchResult = GetPatch(node, xmlFilename, asmUtility, variables);

                List<PatchedByteArray> patches = new List<PatchedByteArray>(includePatches.Count + getPatchResult.StaticPatches.Count);
                patches.AddRange(includePatches);
                patches.AddRange(getPatchResult.StaticPatches);

                AsmPatch asmPatch = new AsmPatch(getPatchResult.Name, shortXmlFilename, getPatchResult.Description, patches, 
                    (getPatchResult.HideInDefault | rootHideInDefault), (getPatchResult.IsHidden | rootIsHidden), variables);
                
                asmPatch.ErrorText = sbPatchErrorText.ToString() + getPatchResult.ErrorText;
                //asmPatch.Update(asmUtility);

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

                Enum sector = (Enum)Enum.Parse(sectorType, Utilities.GetCaseInsensitiveAttribute(theRealNode, "file").InnerText);
                UInt32 offset = UInt32.Parse( Utilities.GetCaseInsensitiveAttribute(theRealNode, "offset").InnerText, System.Globalization.NumberStyles.HexNumber );
                UInt32 expectedLength = UInt32.Parse( Utilities.GetCaseInsensitiveAttribute(theRealNode, "expectedLength").InnerText, System.Globalization.NumberStyles.HexNumber );

                result.Add(new FileAsmPatch(name, shortXmlFilename, description, new InputFilePatch(sector, offset, expectedLength)));

            }

            return result.AsReadOnly();
        }

        public static string CreatePatchXML(IEnumerable<AsmPatch> patches, FreeSpaceMode mode)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<Patches>");

            foreach (AsmPatch patch in patches)
            {
                sb.Append(patch.CreateXML(mode));
            }

            sb.AppendLine("</Patches>");

            return sb.ToString();
        }

        private static List<SpecificLocation> FillSpecificAttributeData(XmlAttribute attrSpecific, Enum defaultSector)
        {
            string strSpecificAttr = (attrSpecific != null) ? attrSpecific.InnerText : "";
            string[] strSpecifics = ASMStringHelper.RemoveSpaces(strSpecificAttr).Split(',');
            List<SpecificLocation> specifics = new List<SpecificLocation>();

            Enum lastSector = defaultSector;
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

                    Enum specificSector;
                    if (string.IsNullOrEmpty(strSector))
                        specificSector = lastSector;
                    else
                    {
                        int sectorNum = 0;
                        bool isSectorNumeric = int.TryParse(strSector, out sectorNum);
                        //specificSector = isSectorNumeric ? (PsxIso.Sectors)sectorNum : (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), strSector);
                        Type sectorType = defaultSector.GetType();
                        specificSector = (Enum)(isSectorNumeric ? Enum.ToObject(sectorType, sectorNum) : Enum.Parse(sectorType, strSector));
                    }

                    lastSector = specificSector;
                    specifics.Add(new SpecificLocation(specificSector, strSpecificOffset));
                }
            }

            return specifics;
        }
    }
}
