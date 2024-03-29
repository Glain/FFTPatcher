﻿using ASMEncoding;
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
            public List<AsmPatch> Patches = new List<AsmPatch>();

            public string Filename { get; set; }
            public bool LoadedCorrectly { get; set; }
            public string ErrorText { get; set; }

            public PatchFile(string filename)
            {
                this.Filename = filename;
            }
        }

        public PatchFile[] FilePatches;
        public List<AsmPatch> AllPatches = new List<AsmPatch>();
        public List<AsmPatch> AllShownPatches = new List<AsmPatch>();
        public HashSet<AsmPatch> SelectedPatches = new HashSet<AsmPatch>();
        //public List<AsmPatch> CurrentSelectedPatches = new List<AsmPatch>();
        public Color[][] BackgroundColors;

        public Dictionary<AsmPatch, int> AllOrdinalMap;
        public List<Dictionary<AsmPatch, int>> FileOrdinalMaps;

        public PatchData(string[] files, ASMEncodingUtility asmUtility)
        {
            FilePatches = new PatchFile[files.Length];
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

                TryGetPatchesResult result = PatchXmlReader.TryGetPatches(File.ReadAllText(file, Encoding.UTF8), file, asmUtility, out tryPatches);
                FilePatches[index].LoadedCorrectly = result.IsSuccess;
                FilePatches[index].ErrorText = result.ErrorText;

                if (result.IsSuccess)
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
                }

                BackgroundColors[index + 1] = fileColorList.ToArray();
            }

            BackgroundColors[0] = allColorList.ToArray();

            BuildOrdinalMaps();
        }

        public void ReloadFile(int index, ASMEncodingUtility asmUtility)
        {
            PatchFile patchFile = FilePatches[index];
            IList<AsmPatch> tryPatches;
            List<Color> fileColorList = new List<Color>();
            Color normalColor = Color.White;
            Color errorColor = Color.FromArgb(225, 125, 125);
            patchFile.Patches.Clear();

            TryGetPatchesResult result = PatchXmlReader.TryGetPatches(File.ReadAllText(patchFile.Filename, Encoding.UTF8), patchFile.Filename, asmUtility, out tryPatches);
            patchFile.LoadedCorrectly = result.IsSuccess;
            patchFile.ErrorText = result.ErrorText;

            if (result.IsSuccess)
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
            }

            BackgroundColors[index + 1] = fileColorList.ToArray();
            FileOrdinalMaps[index] = GetFileOrdinalMap(index);
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
            AllOrdinalMap = GetAllOrdinalMap();
        }

        public void RecalcBackgroundColors()
        {
            Color normalColor = Color.White;
            Color errorColor = Color.FromArgb(225, 125, 125);

            //List<Color> allColorList = new List<Color>(BackgroundColors[0].Length);
            //int allPatchIndex = 0;
            for (int fileIndex = 0; fileIndex < FilePatches.Length; fileIndex++)
            {
                //List<Color> fileColorList = new List<Color>(BackgroundColors[index + 1].Length);
                int filePatchIndex = 0;
                foreach (AsmPatch patch in FilePatches[fileIndex].Patches)
                {
                    if (!patch.IsHidden)
                    {
                        //fileColorList.Add(bgColor);
                        BackgroundColors[fileIndex + 1][filePatchIndex++] = string.IsNullOrEmpty(patch.ErrorText) ? normalColor : errorColor;
                        //if (!patch.HideInDefault)
                        //{
                            //allColorList.Add(bgColor);
                            //BackgroundColors[0][allPatchIndex++] = bgColor;
                        //}
                    }
                }

                //BackgroundColors[index + 1] = fileColorList.ToArray();
            }

            int allPatchIndex = 0;
            foreach (AsmPatch patch in AllShownPatches)
            {
                if (!(patch.IsHidden || patch.HideInDefault))
                    BackgroundColors[0][allPatchIndex++] = string.IsNullOrEmpty(patch.ErrorText) ? normalColor : errorColor;
            }

            //BackgroundColors[0] = allColorList.ToArray();
        }

        public void BuildOrdinalMaps()
        {
            AllOrdinalMap = GetAllOrdinalMap();
            FileOrdinalMaps = GetFileOrdinalMaps();
        }

        public Dictionary<AsmPatch, int> GetAllOrdinalMap()
        {
            Dictionary<AsmPatch, int> resultMap = new Dictionary<AsmPatch, int>();
            for (int index = 0; index < AllPatches.Count; index++)
            {
                resultMap.Add(AllPatches[index], index);
            }

            return resultMap;
        }

        public List<Dictionary<AsmPatch, int>> GetFileOrdinalMaps()
        {
            List<Dictionary<AsmPatch, int>> fileOrdinalMaps = new List<Dictionary<AsmPatch, int>>();
            for (int fileIndex = 0; fileIndex < FilePatches.Length; fileIndex++)
            {
                fileOrdinalMaps.Add(GetFileOrdinalMap(fileIndex));
            }

            return fileOrdinalMaps;
        }

        public Dictionary<AsmPatch, int> GetFileOrdinalMap(int fileIndex)
        {
            Dictionary<AsmPatch, int> fileOrdinalMap = new Dictionary<AsmPatch, int>();
            for (int patchIndex = 0; patchIndex < FilePatches[fileIndex].Patches.Count; patchIndex++)
            {
                fileOrdinalMap.Add(FilePatches[fileIndex].Patches[patchIndex], patchIndex);
            }

            return fileOrdinalMap;
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
