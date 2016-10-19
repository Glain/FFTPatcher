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

        private static void GetPatch( XmlNode node, string xmlFileName, ASMEncodingUtility asmUtility, out string name, out string description, out IList<PatchedByteArray> staticPatches )
        {
            GetPatchNameAndDescription( node, out name, out description );

            XmlNodeList currentLocs = node.SelectNodes( "Location" );
            List<PatchedByteArray> patches = new List<PatchedByteArray>( currentLocs.Count );

            foreach ( XmlNode location in currentLocs )
            {
                UInt32 offset = UInt32.Parse( location.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                XmlAttribute fileAttribute = location.Attributes["file"];
                XmlAttribute sectorAttribute = location.Attributes["sector"];
                XmlAttribute asmAttribute = location.Attributes["mode"];
                XmlAttribute offsetModeAttribute = location.Attributes["offsetMode"];
                XmlAttribute inputFileAttribute = location.Attributes["inputFile"];
                
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
                if (asmAttribute != null)
                {
                	if (asmAttribute.InnerText.ToLower().Trim() == "asm")
                	{
                		asmMode = true;
                	}
                }
                
                bool isRamOffset = false;
                if (offsetModeAttribute != null)
                {
                	if (offsetModeAttribute.InnerText.ToLower().Trim() == "ram")
                		isRamOffset = true;
                }
                
                UInt32 ramOffset = offset;
                UInt32 fileOffset = offset;

                try
                {
                    PsxIso.FileToRamOffsets ftrOffset = (PsxIso.FileToRamOffsets)Enum.Parse(typeof(PsxIso.FileToRamOffsets), "OFFSET_" + fileAttribute.InnerText);
                    if (isRamOffset)
                        fileOffset -= (UInt32)ftrOffset;
                    else
                        ramOffset += (UInt32)ftrOffset;
                }
                catch (Exception ex) { }

                string content = location.InnerText;
                if (inputFileAttribute != null)
                {
                    using (StreamReader streamReader = new StreamReader(inputFileAttribute.InnerText, Encoding.UTF8))
                    {
                        content = streamReader.ReadToEnd();
                    }
                }

                byte[] bytes;
                if (asmMode)
                {
                    bytes = asmUtility.EncodeASM(content, ramOffset).EncodedBytes;
                }
                else
                {
                	bytes = GetBytes( content );
                }
                
                patches.Add( new PatchedByteArray( sector, fileOffset, bytes ) );
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
        }

        public static IList<AsmPatch> GetPatches( XmlNode rootNode, string xmlFilename, ASMEncodingUtility asmUtility )
        {
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
                GetPatch( node, xmlFilename, asmUtility, out name, out description, out staticPatches );
                List<VariableType> variables = new List<VariableType>();
                foreach ( XmlNode varNode in node.SelectNodes( "Variable" ) )
                {
                	XmlAttribute xmlAttr = varNode.Attributes["bytes"];
                	string strBytes = (xmlAttr == null) ? "1" : xmlAttr.InnerText;
                	char bytes = (char)(UInt32.Parse(strBytes) & 0xff);
                	
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
                    
                    UInt32 varOffset = UInt32.Parse( varNode.Attributes["offset"].InnerText, System.Globalization.NumberStyles.HexNumber );
                    XmlAttribute defaultAttr = varNode.Attributes["default"];
                    
                    Byte[] byteArray = new Byte[bytes];
                    UInt32 def = 0;
                    if ( defaultAttr != null )
                    {
                        def = UInt32.Parse( defaultAttr.InnerText, System.Globalization.NumberStyles.HexNumber );
                        for (int i=0; i < bytes; i++)
                        {
                        	byteArray[i] = (Byte)((def >> (i * 8)) & 0xff);
                        }
                    }
                    
                    KeyValuePair<string, PatchedByteArray> kvp = new KeyValuePair<string, PatchedByteArray>( varName, new PatchedByteArray( varSec, varOffset, byteArray ) );
                    VariableType vType = new VariableType();
                    vType.content = kvp;
                    vType.bytes = bytes;
                    
                    variables.Add( vType );
                }
                result.Add( new AsmPatch( name, description, staticPatches, variables ) );
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
