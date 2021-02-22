using ASMEncoding;
using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

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
                        if (!patch.IsHidden)
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

        public void ReloadFile(int index, ASMEncodingUtility asmUtility)
        {
            PatchFile patchFile = FilePatches[index];
            IList<AsmPatch> tryPatches;
            List<Color> fileColorList = new List<Color>();
            Color normalColor = Color.White;
            Color errorColor = Color.FromArgb(225, 125, 125);
            patchFile.Patches.Clear();

            if (PatchXmlReader.TryGetPatches(File.ReadAllText(patchFile.Filename, Encoding.UTF8), patchFile.Filename, asmUtility, out tryPatches))
            {
                foreach (AsmPatch patch in tryPatches)
                {
                    if (!patch.IsHidden)
                    {
                        Color bgColor = string.IsNullOrEmpty(patch.ErrorText) ? normalColor : errorColor;
                        patchFile.Patches.Add(patch);
                        fileColorList.Add(bgColor);
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

        public void RebuildAllList()
        {
            AllPatches.Clear();
            AllShownPatches.Clear();
            List<Color> allColorList = new List<Color>();
            Color normalColor = Color.White;
            Color errorColor = Color.FromArgb(225, 125, 125);

            foreach (PatchFile patchFile in FilePatches)
            {
                foreach (AsmPatch patch in patchFile.Patches)
                {
                    Color bgColor = string.IsNullOrEmpty(patch.ErrorText) ? normalColor : errorColor;

                    if (!patch.IsHidden)
                    {
                        AllPatches.Add(patch);
                        if (!patch.HideInDefault)
                        {
                            AllShownPatches.Add(patch);
                            allColorList.Add(bgColor);
                        }
                    }
                }
            }

            BackgroundColors[0] = allColorList.ToArray();
        }

        public void PatchAllISO(ASMEncodingUtility asmUtility, string filename)
        {
            using (Stream file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                foreach (AsmPatch asmPatch in AllPatches)
                {
                    //ModifyPatch(patch);
                    asmPatch.Update(asmUtility);
                    PatcherLib.Iso.PsxIso.PatchPsxIso(file, asmPatch);
                }
            }
        }

        public PatcherLib.Iso.PatchPsxSaveStateResult PatchAllSaveState(ASMEncodingUtility asmUtility, string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                List<PatchedByteArray> patches = new List<PatchedByteArray>();
                foreach (AsmPatch asmPatch in AllPatches)
                {
                    //ModifyPatch(asmPatch);
                    asmPatch.Update(asmUtility);
                    foreach (PatchedByteArray innerPatch in asmPatch)
                    {
                        patches.Add(innerPatch);
                    }
                }

                return PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patches);
            }
        }
    }
}
