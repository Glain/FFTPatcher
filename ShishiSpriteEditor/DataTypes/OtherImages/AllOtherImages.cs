using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using System.IO;
using System.Drawing;
using PatcherLib.Utilities;
using System.Xml;
using FFTPatcher.SpriteEditor.Properties;
using FFTPatcher.SpriteEditor.DataTypes.OtherImages;

namespace FFTPatcher.SpriteEditor
{
    public class AllOtherImages
    {
        public class AllImagesDoWorkData
        {
            public Stream ISO { get; private set; }
            public string Path { get; private set; }
            public AllImagesDoWorkData(Stream iso, string path)
            {
                ISO = iso;
                Path = path;
            }
        }

        public class AllImagesDoWorkResult
        {
            public enum Result
            {
                Success,
                Failure,
            }

            public Result DoWorkResult { get; private set; }
            public int ImagesProcessed { get; private set; }
            public AllImagesDoWorkResult(Result result, int images)
            {
                DoWorkResult = result;
                ImagesProcessed = images;
            }

        }



        private List<AbstractImageList> images;
        public List<AbstractImageList> Images
        {
            get { return images; }
        }

        public AbstractImage this[int i, int j]
        {
            get { return this.images[i][j]; }
        }

        public AbstractImageList this[int i]
        {
            get { return this.images[i]; }
        }

        public int ListCount { get { return images.Count; } }

        private static List<Type> ImageTypes
        {
            get
            {
                System.Reflection.Assembly assy = System.Reflection.Assembly.GetAssembly(typeof(AbstractImage));
                List<Type> allTypes = new List<Type>(assy.GetTypes());
                allTypes.RemoveAll(t => !t.IsSubclassOf(typeof(AbstractImage)));
                return allTypes;
            }
        }


        private static byte[] Effect_FrameDataOffsetByteSequence = new byte[4] { 0x28, 0x00, 0x00, 0x00 };

        private AllOtherImages(List<AbstractImageList> images)
        {
            this.images = images;
        }



        private AllImagesDoWorkResult LoadAllImages( Stream iso, string path, Action<int> progressReporter )
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;

            if (progress)
            {
                images.ForEach( i => total += i.Count );
            }

            HashSet<string> usedFilenameSet = new HashSet<string>();

            foreach (AbstractImageList imgList in images)
            {
                foreach (AbstractImage img in imgList)
                {
                    string name = string.Empty;
                    name = img.GetSaveFileName();

                    if (usedFilenameSet.Contains(name))
                        name = name.Replace(".", "_2.");

                    int num = 3;
                    while (usedFilenameSet.Contains(name))
                    {
                        name = name.Replace("_" + (num - 1) + ".", "_" + (num) + ".");
                        num++;
                    }

                    string filePath = Path.Combine(path, name);
                    if (File.Exists(filePath))
                    {
                        if (img.CanSelectPalette() && (img.PaletteCount > 1))
                        {
                            ISelectablePalette4bppImage img4bpp = ((ISelectablePalette4bppImage)img);
                            bool importExport8bpp = img4bpp.ImportExport8bpp;
                            int currentPalette = img4bpp.CurrentPalette;

                            img4bpp.ImportExport8bpp = true;
                            img4bpp.CurrentPalette = Math.Max(0, img4bpp.CurrentPalette - 15);

                            img.WriteImageToIso(iso, filePath);

                            img4bpp.ImportExport8bpp = importExport8bpp;
                            img4bpp.CurrentPalette = currentPalette;
                        }
                        else
                        {
                            img.WriteImageToIso(iso, filePath);
                        }

                        usedFilenameSet.Add(name);
                        imagesProcessed++;
                    }
                    if (progress)
                    {
                        progressReporter( (100 * (complete++)) / total );
                    }
                }
            }

            return new AllImagesDoWorkResult( AllImagesDoWorkResult.Result.Success, imagesProcessed );
        }

        public AllImagesDoWorkResult LoadAllImages( Stream iso, string path )
        {
            return LoadAllImages( iso, path, null );
        }

        internal void LoadAllImages( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllImagesDoWorkData data = e.Argument as AllImagesDoWorkData;
            if (data == null)
                return;
            e.Result = LoadAllImages( data.ISO, data.Path, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null );
        }

        internal void DumpAllImages( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllImagesDoWorkData data = e.Argument as AllImagesDoWorkData;
            if (data == null)
                return;
            var result = DumpAllImages( data.ISO, data.Path, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null );
            e.Result = result;
        }

        private AllImagesDoWorkResult DumpAllImages( Stream iso, string path, Action<int> progressReporter )
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;
            if (progress)
                images.ForEach( i => total += i.Count );

            if (!Directory.Exists( path ))
            {
                Directory.CreateDirectory( path );
            }

            HashSet<string> usedFilenameSet = new HashSet<string>();

            foreach (AbstractImageList imgList in images)
            {
                foreach (AbstractImage img in imgList)
                {
                    string name = string.Empty;
                    name = img.GetSaveFileName();
                    //if (img.Context == Context.US_PSX )
                    //{
                    //    var pos = img.Position as PatcherLib.Iso.PsxIso.KnownPosition;
                    //    name = string.Format( "{0}_{1}.png", pos.Sector, pos.StartLocation );
                    //}
                    //else if (img.Position is PatcherLib.Iso.PspIso.KnownPosition)
                    //{
                    //    var pos = img.Position as PatcherLib.Iso.PspIso.KnownPosition;
                    //    name = string.Format( "{0}_{1}.png", pos.SectorEnum, pos.StartLocation );
                    //}

                    if (!string.IsNullOrEmpty( name ))
                    {
                        if (usedFilenameSet.Contains(name))
                            name = name.Replace(".", "_2.");

                        int num = 3;
                        while (usedFilenameSet.Contains(name))
                        {
                            name = name.Replace("_" + (num - 1) + ".", "_" + (num) + ".");
                            num++;
                        }

                        //Bitmap bmp = img.GetImageFromIso( iso );
                        //bmp.Save( Path.Combine( path, name ), System.Drawing.Imaging.ImageFormat.Bmp );
                        string fullPath = Path.Combine(path, name);
                        using (System.IO.Stream s = System.IO.File.Open(fullPath, System.IO.FileMode.Create))
                        {
                            if (img.CanSelectPalette() && (img.PaletteCount > 1))
                            {
                                ISelectablePalette4bppImage img4bpp = ((ISelectablePalette4bppImage)img);
                                bool importExport8bpp = img4bpp.ImportExport8bpp;
                                int currentPalette = img4bpp.CurrentPalette;

                                img4bpp.ImportExport8bpp = true;
                                img4bpp.CurrentPalette = Math.Max(0, img4bpp.CurrentPalette - 15);
  
                                img.SaveImage(iso, s);

                                img4bpp.ImportExport8bpp = importExport8bpp;
                                img4bpp.CurrentPalette = currentPalette;
                            }
                            else
                            {
                                img.SaveImage(iso, s);
                            }
                        }

                        usedFilenameSet.Add(name);
                        imagesProcessed++;
                    }

                    if (progress)
                    {
                        progressReporter( (100 * (complete++)) / total );
                    }
                }
            }

            return new AllImagesDoWorkResult( AllImagesDoWorkResult.Result.Success, imagesProcessed );
        }

        public void DumpAllImages( Stream iso, string path )
        {
            DumpAllImages( iso, path, null );
        }

        // 0, 135
        private static List<AbstractImageList> BuildPsxImages(Stream iso)
        {
            return BuildImages(iso, Settings.PSXImages, "/PsxFiles/Section[@ignore='false' or not(@ignore)]");
            //return BuildImages(Resources.PSXFiles, "/PsxFiles/Section[@ignore='false' or not(@ignore)]");
        }

        private static List<AbstractImageList> BuildPspImages(Stream iso)
        {
            return BuildImages(iso, Settings.PSPImages, "/PspFiles/Section[@ignore='false' or not(@ignore)]"); 
            //return BuildImages(Resources.PSPFiles, "/PspFiles/Section[@ignore='false' or not(@ignore)]");
        }

        private static List<AbstractImageList> BuildImages(Stream iso, string xmlFile, string xpath)
        {
            List<AbstractImageList> result = new List<AbstractImageList>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile);
            foreach (XmlNode sectionNode in doc.SelectNodes(xpath))
            {
                if (sectionNode.Attributes["Effects"] == null)
                    result.Add(GetImagesFromNode(doc, sectionNode));
                else
                    result.Add(GetEffectImages(iso, (xmlFile == Settings.PSPImages)));
            }

            return result;
        }

        public static AbstractImageList GetImagesFromNode(XmlDocument doc, XmlNode node)
        {
            AbstractImageList result = new AbstractImageList();
            result.Name = (node.Attributes["Name"] == null) ? "" : node.Attributes["Name"].InnerText;

            bool hideEntryIndex = false;
            XmlAttribute attrHideEntryIndex = node.Attributes["HideEntryIndex"];
            if (attrHideEntryIndex != null)
            {
                string attrHideEntryIndexText = attrHideEntryIndex.InnerText;
                bool.TryParse(attrHideEntryIndexText, out hideEntryIndex);
            }
            result.HideEntryIndex = hideEntryIndex;
            
            int totalIndex = 0;
            List<string> names = new List<string>();

            foreach (XmlNode imageNode in node.SelectNodes("*"))
            {
                if (imageNode.Name == "Names")
                {
                    names.Clear();
                    foreach (XmlNode nameNode in imageNode.ChildNodes)
                    {
                        names.Add(nameNode.InnerText);
                    }
                }
                else
                {
                    List<AbstractImage> innerResult = GetImagesFromNodeInner(doc, node, imageNode, names, 0, totalIndex);
                    result.Images.AddRange(innerResult);
                    totalIndex += innerResult.Count;
                }
            }

            if ((string.IsNullOrEmpty(result.Name)) && (result.Images.Count > 0))
                result.Name = result.Images[0].Name;

            return result;
        }

        private static List<AbstractImage> GetImagesFromNodeInner(XmlDocument doc, XmlNode sectionNode, XmlNode imageNode, List<string> names,
            int depth = 0, int inIndex = 0, int inOffset = 0, int inPaletteOffset = 0)
        {
            List<AbstractImage> result = new List<AbstractImage>();

            if (imageNode.Name == "Repeat")
            {
                int number = int.Parse(imageNode.Attributes["Number"].InnerText);
                int offset = int.Parse(imageNode.Attributes["Offset"].InnerText);

                XmlAttribute paletteOffsetAttr = imageNode.Attributes["PaletteOffset"];
                int paletteOffset = (paletteOffsetAttr == null) ? 0 : int.Parse(imageNode.Attributes["PaletteOffset"].InnerText);

                XmlNode innerNode = imageNode.FirstChild;

                int totalIndex = inIndex;

                for (int index = 0; index < number; index++)
                {
                    XmlNode indexNode = innerNode.CloneNode(true);
                    List<AbstractImage> innerResult = GetImagesFromNodeInner(doc, sectionNode, indexNode, names, depth + 1, totalIndex, inOffset, inPaletteOffset);
                    result.AddRange(innerResult);

                    totalIndex += innerResult.Count;
                    inOffset += offset;
                    inPaletteOffset += paletteOffset;
                }
            }
            else
            {
                XmlNode nameNode = imageNode["Name"];
                if (nameNode == null)
                    imageNode.AppendChild(doc.CreateElement("Name"));

                if (depth > 0)
                {
                    imageNode["Name"].InnerText = (inIndex < names.Count) ? names[inIndex] : imageNode["Name"].InnerText + " (" + (inIndex) + ")";

                    XmlNode palettePositionNode = imageNode["PalettePosition"];
                    if (palettePositionNode != null)
                    {
                        imageNode["PalettePosition"]["Offset"].InnerText = (int.Parse(imageNode["PalettePosition"]["Offset"].InnerText) + inPaletteOffset).ToString();
                    }
                    
                    imageNode["Position"]["Offset"].InnerText = (int.Parse(imageNode["Position"]["Offset"].InnerText) + inOffset).ToString();
                }

                if (string.IsNullOrEmpty(imageNode["Name"].InnerText) && (inIndex < names.Count))
                {
                    imageNode["Name"].InnerText = names[inIndex];
                }

                EnsureImageNodeProperty(doc, sectionNode, imageNode, "Filename");
                EnsureImageNodeProperty(doc, sectionNode, imageNode, "Filesize");

                var types = ImageTypes;
                Type type = types.Find(t => t.Name == imageNode.Name);
                System.Reflection.MethodInfo mi = type.GetMethod("ConstructFromXml");
                AbstractImage image = (AbstractImage)mi.Invoke(null, new object[] { imageNode });
                result.Add(image);
            }
            
            return result;
        }

        public static void EnsureImageNodeProperty(XmlDocument doc, XmlNode sectionNode, XmlNode imageNode, string property)
        {
            XmlNode filenameNode = imageNode[property];
            XmlAttribute filenameAttribute = sectionNode.Attributes[property];
            if ((filenameNode == null) && (filenameAttribute != null))
            {
                XmlElement newElement = doc.CreateElement(property);
                newElement.InnerText = filenameAttribute.InnerText;
                imageNode.AppendChild(newElement);
            }
        }

        public static AbstractImageList GetEffectImages(Stream iso, bool isPsp)
        {
            AbstractImageList result = new AbstractImageList();
            result.Name = "Effects";
            result.HideEntryIndex = true;

            if (isPsp)
            {
                result.Images.AddRange(GetPSPEffectImages(iso));
            }
            else
            {
                result.Images.AddRange(GetPSXEffectImages(iso));
            }
            
            return result;
        }

        public static List<AbstractImage> GetPSXEffectImages(Stream iso)
        {
            List<AbstractImage> images = new List<AbstractImage>();

            const int effectCount = 512;
            byte[] effectFileBytes = PatcherLib.Iso.PsxIso.ReadFile(iso, PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x14E3E8, effectCount * 8);
            byte[] headerLocationBytes = PatcherLib.Iso.PsxIso.ReadFile(iso, PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x14D8D0, effectCount * 4);
            IList<string> effectNames = PatcherLib.PSXResources.Lists.AbilityEffects;

            for (int effectIndex = 0; effectIndex < effectCount; effectIndex++)
            {
                int effectByteCount = effectFileBytes.SubLength((effectIndex * 8) + 4, 4).ToIntLE();
                if (effectByteCount == 0)
                    continue;

                uint headerLocation = headerLocationBytes.SubLength((effectIndex * 4), 4).ToUInt32();
                int headerOffset = (int)(headerLocation - 0x801C2500U);

                try
                {
                    AbstractImage image = GetPSXEffectImage(iso, effectIndex, effectNames[effectIndex], headerOffset);
                    if (image != null)
                        images.Add(image);
                }
                catch (Exception) { }
            }

            return images;
        }

        public static List<AbstractImage> GetPSPEffectImages(Stream iso)
        {
            List<AbstractImage> images = new List<AbstractImage>();

            const int effectCount = 512;
            IList<string> effectNames = PatcherLib.PSPResources.Lists.AbilityEffects;
            PatcherLib.Iso.PspIso.PspIsoInfo pspIsoInfo = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso);
            //byte[] subroutineEndByteSequence = new byte[8] { 0x08, 0x00, 0xE0, 0x03, 0x00, 0x00, 0x00, 0x00 };
            //byte[] frameDataOffsetByteSequence = new byte[4] { 0x28, 0x00, 0x00, 0x00 };

            for (int effectIndex = 0; effectIndex < effectCount; effectIndex++)
            {
                try
                {
                    AbstractImage effectImage = GetPSPEffectImage(iso, effectIndex, effectNames[effectIndex], pspIsoInfo);
                    if (effectImage != null)
                        images.Add(effectImage);
                }
                catch (Exception) { }
            }

            return images;
        }

        public static AbstractImage GetPSXEffectImage(Stream iso, int effectIndex)
        {
            byte[] headerLocationBytes = PatcherLib.Iso.PsxIso.ReadFile(iso, PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x14D8D0 + (effectIndex * 8) + 4, 4);
            uint headerLocation = headerLocationBytes.ToUInt32();
            int headerOffset = (int)(headerLocation - 0x801C2500U);

            return GetPSXEffectImage(iso, effectIndex, PatcherLib.PSXResources.Lists.AbilityEffects[effectIndex], headerOffset);
        }

        public static AbstractImage GetPSXEffectImage(Stream iso, int effectIndex, string effectName, int headerOffset)
        {
            string strEffectNumber = effectIndex.ToString("000");
            string sectorName = string.Format("EFFECT_E{0}_BIN", strEffectNumber);

            PatcherLib.Iso.PsxIso.Sectors sector = (PatcherLib.Iso.PsxIso.Sectors)0;

            try
            {
                sector = (PatcherLib.Iso.PsxIso.Sectors)Enum.Parse(typeof(PatcherLib.Iso.PsxIso.Sectors), sectorName);
            }
            catch (Exception) { }

            if (sector == 0)
                return null;

            /*
            int effectByteCount = effectFileBytes.SubLength((effectIndex * 8) + 4, 4).ToIntLE();
            if (effectByteCount == 0)
                return null;

            uint headerLocation = headerLocationBytes.SubLength((effectIndex * 4), 4).ToUInt32();
            int headerOffset = (int)(headerLocation - 0x801C2500U);
            */

            int frameDataOffset = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, headerOffset, 4).ToIntLE() + headerOffset;
            int frameDataSectionCount = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, frameDataOffset, 2).ToIntLE();
            int firstFrameDataPointerOffset = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, frameDataOffset + 4 + (2 * frameDataSectionCount), 2).ToIntLE() + frameDataOffset + 4;
            int firstFrameTexturePageHeader = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, firstFrameDataPointerOffset, 2).ToIntLE();

            int colorDepthCode = (firstFrameTexturePageHeader & 0x0180) >> 7;
            if (colorDepthCode > 1)     // Invalid code
                return null;

            bool is4bpp = (colorDepthCode == 0);
            int paletteOffset = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, headerOffset + 0x24, 4).ToIntLE() + headerOffset;

            int secondSetPaletteOffset = paletteOffset + 0x200;
            int imageSizeDataOffset = paletteOffset + 0x400;
            int graphicsOffset = paletteOffset + 0x404;

            byte[] imageSizeData = PatcherLib.Iso.PsxIso.ReadFile(iso, sector, imageSizeDataOffset, 4);
            int imageSizeCombinedValue = imageSizeData.Sub(0, 2).ToIntLE();
            int rowBytes = (imageSizeData[3] != 0) ? 256 : 128;
            int height = imageSizeCombinedValue >> ((imageSizeData[3] != 0) ? 8 : 7);
            int width = is4bpp ? (rowBytes * 2) : rowBytes;
            int imageSize = rowBytes * height;
            int fileSize = graphicsOffset + imageSize;
            string name = String.Format("{0} {1}", effectIndex.ToString("X3"), effectName);
            string fileName = String.Format("E{0}.BIN", strEffectNumber);

            PatcherLib.Iso.PsxIso.KnownPosition graphicsPosition = new PatcherLib.Iso.PsxIso.KnownPosition(sector, graphicsOffset, imageSize);
            PatcherLib.Iso.PsxIso.KnownPosition palettePosition = new PatcherLib.Iso.PsxIso.KnownPosition(sector, (is4bpp ? secondSetPaletteOffset : paletteOffset), (is4bpp ? 32 : 512));

            return GetPalettedImage(is4bpp, name, width, height, fileName, fileSize, sector, graphicsPosition, palettePosition, true, effectIndex);
        }

        public static AbstractImage GetPSPEffectImage(Stream iso, int effectIndex)
        {
            return GetPSPEffectImage(iso, effectIndex, PatcherLib.PSPResources.Lists.AbilityEffects[effectIndex], PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso));
        }

        public static AbstractImage GetPSPEffectImage(Stream iso, int effectIndex, string effectName, PatcherLib.Iso.PspIso.PspIsoInfo pspIsoInfo)
        {
            string strEffectNumber = effectIndex.ToString("000");
            string sectorName = string.Format("EFFECT_E{0}_BIN", strEffectNumber);

            PatcherLib.Iso.FFTPack.Files fftPack = (PatcherLib.Iso.FFTPack.Files)3;

            try
            {
                fftPack = (PatcherLib.Iso.FFTPack.Files)Enum.Parse(typeof(PatcherLib.Iso.FFTPack.Files), sectorName);
            }
            catch (Exception) { }

            byte[] fileBytes = PatcherLib.Iso.PspIso.GetFile(iso, pspIsoInfo, fftPack);
            //byte[] fileBytes = PatcherLib.Iso.FFTPack.TryGetFileFromIso(iso, pspIsoInfo, fftPack, 0, 0x5800);
            if (fileBytes.Length == 0)
                return null;

            /*
            int headerOffset = 0;
            int lastMatchIndex = fileBytes.LastIndexOf(subroutineEndByteSequence);
            if (lastMatchIndex >= 0)
            {
                int lastMatchIndex2 = fileBytes.Sub(lastMatchIndex + 8).IndexOf(frameDataOffsetByteSequence) + lastMatchIndex + 4;
                headerOffset = (lastMatchIndex2 >= 0) ? (lastMatchIndex2 + 4) : (lastMatchIndex + 8);
            }
            */

            int headerOffset = fileBytes.IndexOf(Effect_FrameDataOffsetByteSequence);

            int frameDataOffset = fileBytes.SubLength(headerOffset, 4).ToIntLE() + headerOffset;
            int frameDataSectionCount = fileBytes.SubLength(frameDataOffset, 2).ToIntLE();
            int firstFrameDataPointerOffset = fileBytes.SubLength(frameDataOffset + 4 + (2 * frameDataSectionCount), 2).ToIntLE() + frameDataOffset + 4;
            int firstFrameTexturePageHeader = fileBytes.SubLength(firstFrameDataPointerOffset, 2).ToIntLE();

            int colorDepthCode = (firstFrameTexturePageHeader & 0x0180) >> 7;
            if (colorDepthCode > 1)     // Invalid code
                return null;

            bool is4bpp = (colorDepthCode == 0);
            int paletteOffset = fileBytes.SubLength(headerOffset + 0x24, 4).ToIntLE() + headerOffset;

            int secondSetPaletteOffset = paletteOffset + 0x200;
            int imageSizeDataOffset = paletteOffset + 0x400;
            int graphicsOffset = paletteOffset + 0x404;

            byte[] imageSizeData = fileBytes.SubLength(imageSizeDataOffset, 4).ToArray();
            //byte[] imageSizeData = PatcherLib.Iso.FFTPack.TryGetFileFromIso(iso, pspIsoInfo, fftPack, imageSizeDataOffset, 4);
            int imageSizeCombinedValue = imageSizeData.Sub(0, 2).ToIntLE();
            int rowBytes = (imageSizeData[3] != 0) ? 256 : 128;
            int height = imageSizeCombinedValue >> ((imageSizeData[3] != 0) ? 8 : 7);
            int width = is4bpp ? (rowBytes * 2) : rowBytes;
            int imageSize = rowBytes * height;
            int fileSize = graphicsOffset + imageSize;
            string name = String.Format("{0} {1}", effectIndex.ToString("X3"), effectName);
            string fileName = String.Format("E{0}.BIN", strEffectNumber);

            PatcherLib.Iso.PspIso.KnownPosition graphicsPosition = new PatcherLib.Iso.PspIso.KnownPosition(fftPack, graphicsOffset, imageSize);
            PatcherLib.Iso.PspIso.KnownPosition palettePosition = new PatcherLib.Iso.PspIso.KnownPosition(fftPack, (is4bpp ? secondSetPaletteOffset : paletteOffset), (is4bpp ? 32 : 512));

            return GetPalettedImage(is4bpp, name, width, height, fileName, fileSize, fftPack, graphicsPosition, palettePosition, true, effectIndex);
        }

        public static AbstractImage GetPalettedImage(bool is4bpp, string name, int width, int height, string fileName, int fileSize, Enum sector,
            PatcherLib.Iso.KnownPosition graphicsPosition, PatcherLib.Iso.KnownPosition palettePosition, bool isEffect = false, int effectIndex = 0)
        {
            if (is4bpp)
            {
                PalettedImage4bpp image = new PalettedImage4bpp(name, width, height, 32, Palette.ColorDepth._16bit, graphicsPosition, palettePosition);
                image.PaletteCount = 16;
                image.DefaultPalette = 0;
                image.CurrentPalette = 0;
                image.OriginalFilename = fileName;
                image.Filesize = fileSize;
                image.Sector = sector;
                image.IsEffect = isEffect;
                image.EffectIndex = effectIndex;
                return image;
            }
            else
            {
                PalettedImage8bpp image = new PalettedImage8bpp(name, width, height, 1, Palette.ColorDepth._16bit, graphicsPosition, palettePosition);
                image.PaletteCount = 0;
                image.DefaultPalette = 0;
                image.CurrentPalette = 0;
                image.OriginalFilename = fileName;
                image.Filesize = fileSize;
                image.Sector = sector;
                image.IsEffect = isEffect;
                image.EffectIndex = effectIndex;
                return image;
            }
        }

        public static AllOtherImages GetPsx(Stream iso)
        {
            return new AllOtherImages(BuildPsxImages(iso));
        }

        public static AllOtherImages GetPsp(Stream iso)
        {
            return new AllOtherImages(BuildPspImages(iso));
        }

        public static AllOtherImages FromIso( Stream iso )
        {
            if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0 )
            {
                // assume psx
                return GetPsx(iso);
            }
            else if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0 )
            {
                // assume psp
                return GetPsp(iso);
            }
            else
            {
                throw new ArgumentException( "iso" );
            }
        }
    }
}
