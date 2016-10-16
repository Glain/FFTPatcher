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

            foreach (AbstractImageList imgList in images)
            {
                foreach (AbstractImage img in imgList)
                {
                    string name = string.Empty;
                    name = img.GetSaveFileName();

                    name = Path.Combine( path, name );
                    if (File.Exists( name ))
                    {
                        if (img.CanSelectPalette() && (img.PaletteCount > 1))
                        {
                            ISelectablePalette4bppImage img4bpp = ((ISelectablePalette4bppImage)img);
                            bool importExport8bpp = img4bpp.ImportExport8bpp;
                            int currentPalette = img4bpp.CurrentPalette;

                            img4bpp.ImportExport8bpp = true;
                            img4bpp.CurrentPalette = Math.Max(0, img4bpp.CurrentPalette - 15);

                            img.WriteImageToIso(iso, name);

                            img4bpp.ImportExport8bpp = importExport8bpp;
                            img4bpp.CurrentPalette = currentPalette;
                        }
                        else
                        {
                            img.WriteImageToIso(iso, name);
                        }

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

        public void LoadAllImages( Stream iso, string path )
        {
            LoadAllImages( iso, path, null );
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
        private static List<AbstractImageList> BuildPsxImages()
        {
            return BuildImages(Resources.PSXFiles, "/PsxFiles/Section[@ignore='false' or not(@ignore)]");
        }

        private static List<AbstractImageList> BuildPspImages()
        {
            return BuildImages(Resources.PSPFiles, "/PspFiles/Section[@ignore='false' or not(@ignore)]");
        }

        private static List<AbstractImageList> BuildImages(string xmlFile, string xpath)
        {
            List<AbstractImageList> result = new List<AbstractImageList>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFile);
            foreach (XmlNode sectionNode in doc.SelectNodes(xpath))
            {
                result.Add(GetImagesFromNode(doc, sectionNode));
            }

            return result;
        }

        public static AbstractImageList GetImagesFromNode(XmlDocument doc, XmlNode node)
        {
            AbstractImageList result = new AbstractImageList();
            result.Name = (node.Attributes["Name"] == null) ? "" : node.Attributes["Name"].InnerText;
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
                    imageNode["Name"].InnerText = (inIndex < names.Count) ? names[inIndex] : imageNode["Name"].InnerText + " (" + (inIndex + 1) + ")";

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

        public static AllOtherImages GetPsx()
        {
            return new AllOtherImages(BuildPsxImages());
        }

        public static AllOtherImages GetPsp()
        {
            return new AllOtherImages(BuildPspImages());
        }

        public static AllOtherImages FromIso( Stream iso )
        {
            if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0 )
            {
                // assume psx
                return GetPsx();
            }
            else if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0 )
            {
                // assume psp
                return GetPsp();
            }
            else
            {
                throw new ArgumentException( "iso" );
            }
        }
    }
}
