using System;
using System.Collections.Generic;
using System.Xml;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using ASMEncoding;
using System.IO;
using System.Text;

namespace FFTorgASM
{
    static class PatchXmlReader
    {
        public static readonly System.Text.RegularExpressions.Regex stripRegex = 
            new System.Text.RegularExpressions.Regex( @"\s" );

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

        private static void GetPatchNameAndDescription( XmlNode node, out string name, out string description )
        {
            name = node.Attributes["name"].InnerText;
            XmlNode descriptionNode = node.SelectSingleNode( "Description" );
            description = name;
            if ( descriptionNode != null )
            {
                description = descriptionNode.InnerText;
            }

        }

        private static void GetPatch( XmlNode node, string xmlFileName, ASMEncodingUtility asmUtility, out string name, out string description, out IList<PatchedByteArray> staticPatches,
            out List<bool> outDataSectionList, out bool hideInDefault)
        {
            GetPatchNameAndDescription( node, out name, out description );

            hideInDefault = false;
            XmlAttribute attrHideInDefault = node.Attributes["hideInDefault"];
            if (attrHideInDefault != null)
            {
                if (attrHideInDefault.InnerText.ToLower().Trim() == "true")
                    hideInDefault = true;
            }

            XmlNodeList currentLocs = node.SelectNodes( "Location" );
            List<PatchedByteArray> patches = new List<PatchedByteArray>( currentLocs.Count );
            List<bool> isDataSectionList = new List<bool>( currentLocs.Count );

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

                string strOffsetAttr = location.Attributes["offset"].InnerText;
                string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');

                PsxIso.Sectors sector =  (PsxIso.Sectors)0;
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
                if (offsetModeAttribute != null)
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
                    content = asmUtility.ReplaceLabelsInHex(content, true);
                }

                Nullable<PsxIso.FileToRamOffsets> ftrOffset = null;
                try
                {
                    ftrOffset = (PsxIso.FileToRamOffsets)Enum.Parse(typeof(PsxIso.FileToRamOffsets), "OFFSET_" + fileAttribute.InnerText);
                }
                catch (Exception)
                {
                    ftrOffset = null;
                }

                foreach (string strOffset in strOffsets)
                {
                    UInt32 offset = UInt32.Parse(strOffset, System.Globalization.NumberStyles.HexNumber);

                    UInt32 ramOffset = offset;
                    UInt32 fileOffset = offset;

                    if (ftrOffset.HasValue)
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

                    ramOffset = ramOffset | 0x80000000;     // KSEG0

                    byte[] bytes;
                    if (asmMode)
                    {
                        bytes = asmUtility.EncodeASM(content, ramOffset).EncodedBytes;
                    }
                    else
                    {
                        bytes = GetBytes(content);
                    }

                    patches.Add(new PatchedByteArray(sector, fileOffset, bytes));
                    isDataSectionList.Add(markedAsData);
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

            staticPatches = patches.AsReadOnly();
            outDataSectionList = isDataSectionList;
        }

        public static IList<AsmPatch> GetPatches( XmlNode rootNode, string xmlFilename, ASMEncodingUtility asmUtility )
        {
            bool rootHideInDefault = false;
            XmlAttribute attrHideInDefault = rootNode.Attributes["hideInDefault"];
            if (attrHideInDefault != null)
            {
                rootHideInDefault = (attrHideInDefault.InnerText.ToLower().Trim() == "true");
            }

            XmlNodeList patchNodes = rootNode.SelectNodes( "Patch" );
            List<AsmPatch> result = new List<AsmPatch>( patchNodes.Count );
            foreach ( XmlNode node in patchNodes )
            {
                XmlAttribute ignoreNode = node.Attributes["ignore"];
                if ( ignoreNode != null && Boolean.Parse( ignoreNode.InnerText ) )
                    continue;

                string name;
                string description;
                IList<PatchedByteArray> staticPatches;
                List<bool> isDataSectionList;
                bool hideInDefault;

                GetPatch(node, xmlFilename, asmUtility, out name, out description, out staticPatches, out isDataSectionList, out hideInDefault);
                List<VariableType> variables = new List<VariableType>();
                foreach ( XmlNode varNode in node.SelectNodes( "Variable" ) )
                {
                	XmlAttribute numBytesAttr = varNode.Attributes["bytes"];
                    string strNumBytes = (numBytesAttr == null) ? "1" : numBytesAttr.InnerText;
                    char numBytes = (char)(UInt32.Parse(strNumBytes) & 0xff);
                	
                    string varName = varNode.Attributes["name"].InnerText;

                    XmlAttribute fileAttribute = varNode.Attributes["file"];
                    XmlAttribute sectorAttribute = varNode.Attributes["sector"];

                    PsxIso.Sectors varSec = (PsxIso.Sectors)0;
                    if (fileAttribute != null)
                    {
                        varSec = (PsxIso.Sectors)Enum.Parse(typeof(PsxIso.Sectors), fileAttribute.InnerText);
                    }
                    else if (sectorAttribute != null)
                    {
                        varSec = (PsxIso.Sectors)Int32.Parse(sectorAttribute.InnerText, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    //PsxIso.Sectors varSec = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), varNode.Attributes["file"].InnerText );
                    
                    //UInt32 varOffset = UInt32.Parse( varNode.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                    string strOffsetAttr = varNode.Attributes["offset"].InnerText;
                    string[] strOffsets = strOffsetAttr.Replace(" ", "").Split(',');
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
                    foreach (string strOffset in strOffsets)
                    {
                        UInt32 varOffset = UInt32.Parse(strOffset, System.Globalization.NumberStyles.HexNumber);
                        patchedByteArrayList.Add(new PatchedByteArray(varSec, varOffset, byteArray));
                    }

                    VariableType vType = new VariableType();
                    vType.numBytes = numBytes;
                    vType.byteArray = byteArray;
                    vType.content = new KeyValuePair<string, List<PatchedByteArray>>(varName, patchedByteArrayList);                    
                    variables.Add( vType );
                }

                result.Add( new AsmPatch( name, description, staticPatches, isDataSectionList, (hideInDefault | rootHideInDefault), variables ) );
            }

            patchNodes = rootNode.SelectNodes( "ImportFilePatch" );
            foreach ( XmlNode node in patchNodes )
            {
                string name;
                string description;

                GetPatchNameAndDescription(node, out name, out description);

                XmlNodeList fileNodes = node.SelectNodes( "ImportFile" );
                if ( fileNodes.Count != 1 ) continue;

                XmlNode theRealNode = fileNodes[0];

                PsxIso.Sectors sector = (PsxIso.Sectors)Enum.Parse( typeof( PsxIso.Sectors ), theRealNode.Attributes["file"].InnerText );
                UInt32 offset = UInt32.Parse( theRealNode.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                UInt32 expectedLength = UInt32.Parse( theRealNode.Attributes["expectedLength"].InnerText, System.Globalization.NumberStyles.HexNumber );

                result.Add( new FileAsmPatch( name, description, new InputFilePatch( sector, offset, expectedLength ) ) );

            }

            return result.AsReadOnly();

        }

        private static byte[] GetBytes( string byteText )
        {
            string strippedText = stripRegex.Replace( byteText, string.Empty );
    
            int bytes = strippedText.Length / 2;
            byte[] result = new byte[bytes];

            for ( int i = 0; i < bytes; i++ )
            {
                result[i] = Byte.Parse( strippedText.Substring( i * 2, 2 ), System.Globalization.NumberStyles.HexNumber );
            }
            return result;
        }
    }
}
