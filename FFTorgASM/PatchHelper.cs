using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Datatypes;
using PatcherLib.Iso;

namespace FFTorgASM
{
    public class PatchResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public PatchResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }
    }

    public static class PatchHelper
    {
        public static PatchResult PatchFile(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility)
        {
            string modFilepath = filename.ToLower().Trim();

            bool isISO = false;
            string[] validExtensions = { ".bin", ".iso", ".img" };
            foreach (string extension in validExtensions)
            {
                if (modFilepath.EndsWith(extension.ToLower().Trim()))
                {
                    isISO = true;
                    break;
                }
            }

            if (isISO)
            {
                return PatchISO(filename, asmPatches, asmUtility);
            }
            else if (modFilepath.EndsWith(".psv"))
            {
                return PatchPSV(filename, asmPatches, asmUtility);
            }
            else
            {
                return new PatchResult(false, "Unsupported file!");
            }
        }

        public static PatchResult PatchISO(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility)
        {
            ConflictResolveResult conflictResolveResult = ConflictHelper.ResolveConflicts(asmPatches, asmUtility);
            asmPatches = conflictResolveResult.Patches;

            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            foreach (AsmPatch asmPatch in asmPatches)
            {
                asmPatch.Update(asmUtility);
                foreach (PatchedByteArray innerPatch in asmPatch)
                {
                    patches.Add(innerPatch);
                }
            }

            using (Stream file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                PatcherLib.Iso.PsxIso.PatchPsxIso(file, patches);
            }

            StringBuilder sbResultMessage = new StringBuilder();
            sbResultMessage.AppendLine("Complete!");
            sbResultMessage.AppendLine();

            if (!string.IsNullOrEmpty(conflictResolveResult.Message))
            {
                sbResultMessage.AppendLine(conflictResolveResult.Message);
                sbResultMessage.AppendLine();
            }

            // DEBUG
            //File.WriteAllText("./output.xml", PatchXmlReader.CreatePatchXML(patches), Encoding.UTF8);

            return new PatchResult(true, sbResultMessage.ToString());
        }

        public static PatchResult PatchPSV(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility)
        {
            ConflictResolveResult conflictResolveResult = ConflictHelper.ResolveConflicts(asmPatches, asmUtility);
            asmPatches = conflictResolveResult.Patches;

            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            foreach (AsmPatch asmPatch in asmPatches)
            {
                asmPatch.Update(asmUtility);
                foreach (PatchedByteArray innerPatch in asmPatch)
                {
                    patches.Add(innerPatch);
                }
            }

            PatchPsxSaveStateResult patchResult;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                patchResult = PatcherLib.Iso.PsxIso.PatchPsxSaveState(reader, patches);
            }

            StringBuilder sbResultMessage = new StringBuilder();
            sbResultMessage.AppendLine("Complete!");
            sbResultMessage.AppendLine();

            if (!string.IsNullOrEmpty(conflictResolveResult.Message))
            {
                sbResultMessage.AppendLine(conflictResolveResult.Message);
                sbResultMessage.AppendLine();
            }

            if (patchResult.UnsupportedFiles.Count > 0)
            {
                sbResultMessage.AppendLine("Files not supported for savestate patching:");
                foreach (PsxIso.Sectors sector in patchResult.UnsupportedFiles)
                {
                    sbResultMessage.AppendFormat("\t{0}{1}", PsxIso.GetSectorName(sector), Environment.NewLine);
                }
                sbResultMessage.AppendLine();
            }
            if (patchResult.AbsentFiles.Count > 0)
            {
                sbResultMessage.AppendLine("Files not present in savestate:");
                foreach (PsxIso.Sectors sector in patchResult.AbsentFiles)
                {
                    sbResultMessage.AppendFormat("\t{0}{1}", PsxIso.GetSectorName(sector), Environment.NewLine);
                }
                sbResultMessage.AppendLine();
            }

            return new PatchResult(true, sbResultMessage.ToString());
        }
    }
}
