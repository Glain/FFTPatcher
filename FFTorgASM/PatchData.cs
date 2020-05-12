using ASMEncoding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTorgASM
{
    public class PatchData
    {
        public class PatchFile
        {
            public string Filename { get; set; }
            public List<AsmPatch> Patches = new List<AsmPatch>();

            public PatchFile(string filename)
            {
                this.Filename = filename;
            }
        }

        public PatchFile[] FilePatches;
        public List<AsmPatch> AllPatches = new List<AsmPatch>();
        public List<AsmPatch> AllShownPatches = new List<AsmPatch>();
        public HashSet<AsmPatch> SelectedPatches = new HashSet<AsmPatch>();
        public List<AsmPatch> CurrentSelectedPatches = new List<AsmPatch>();
        public bool[] LoadedCorrectly;
        public Color[][] BackgroundColors;

        public PatchData(string[] files, ASMEncodingUtility asmUtility)
        {
            FilePatches = new PatchFile[files.Length];
            LoadedCorrectly = new bool[files.Length];
            IList<AsmPatch> tryPatches;

            List<Color> allColorList = new List<Color>();
            Color normalColor = Color.White;
            Color errorColor = Color.FromArgb(225, 125, 125);
            BackgroundColors = new Color[files.Length + 1][];

            for (int index = 0; index < files.Length; index++)
            {
                string file = files[index];
                List<Color> fileColorList = new List<Color>();
                FilePatches[index] = new PatchFile(file);

                if (PatchXmlReader.TryGetPatches(File.ReadAllText(file, Encoding.UTF8), file, asmUtility, out tryPatches))
                {
                    foreach (AsmPatch patch in tryPatches)
                    {
                        Color bgColor = string.IsNullOrEmpty(patch.ErrorText) ? normalColor : errorColor;
                        FilePatches[index].Patches.Add(patch);
                        fileColorList.Add(bgColor);

                        AllPatches.Add(patch);
                        if (!patch.HideInDefault)
                        {
                            AllShownPatches.Add(patch);
                            allColorList.Add(bgColor);
                        }
                    }

                    LoadedCorrectly[index] = true;
                }
                else
                {
                    LoadedCorrectly[index] = false;
                }

                BackgroundColors[index + 1] = fileColorList.ToArray();
            }

            BackgroundColors[0] = allColorList.ToArray();
        }
    }
}
