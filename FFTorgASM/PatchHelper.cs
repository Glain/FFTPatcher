using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASMEncoding;
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
        public static PatchResult PatchFile(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility, bool tryResolveConflicts)
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
                return PatchISO(filename, asmPatches, asmUtility, tryResolveConflicts);
            }
            else if ((asmUtility.EncodingMode == ASMEncodingMode.PSX) && (modFilepath.EndsWith(".psv")))
            {
                return PatchPSV(filename, asmPatches, asmUtility, tryResolveConflicts);
            }
            else
            {
                return new PatchResult(false, "Unsupported file!");
            }
        }

        public static PatchResult PatchISO(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility, bool tryResolveConflicts)
        {
            string conflictResolveMessage = string.Empty;
            if (tryResolveConflicts)
            {
                ConflictResolveResult conflictResolveResult = ConflictHelper.ResolveConflicts(asmPatches, asmUtility);
                asmPatches = conflictResolveResult.Patches;
                conflictResolveMessage = conflictResolveResult.Message;
            }

            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            foreach (AsmPatch asmPatch in asmPatches)
            {
                asmPatch.Update(asmUtility);
                foreach (PatchedByteArray innerPatch in asmPatch)
                {
                    patches.Add(innerPatch);

                    if ((asmUtility.EncodingMode == ASMEncodingMode.PSP) && (innerPatch.Sector == (int)PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN))
                        patches.Add(innerPatch.GetCopyForSector(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN));
                }
            }

            using (Stream file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                if (asmUtility.EncodingMode == ASMEncoding.ASMEncodingMode.PSX)
                    PsxIso.PatchPsxIso(file, patches);
                else if (asmUtility.EncodingMode == ASMEncoding.ASMEncodingMode.PSP)
                    PspIso.PatchISO(file, patches);
            }

            StringBuilder sbResultMessage = new StringBuilder();
            sbResultMessage.AppendLine("Complete!");
            sbResultMessage.AppendLine();

            if (!string.IsNullOrEmpty(conflictResolveMessage))
            {
                sbResultMessage.AppendLine(conflictResolveMessage);
                sbResultMessage.AppendLine();
            }

            // DEBUG
            //File.WriteAllText("./output.xml", PatchXmlReader.CreatePatchXML(patches), Encoding.UTF8);

            return new PatchResult(true, sbResultMessage.ToString());
        }

        public static PatchResult PatchPSV(string filename, List<AsmPatch> asmPatches, ASMEncoding.ASMEncodingUtility asmUtility, bool tryResolveConflicts)
        {
            string conflictResolveMessage = string.Empty;
            if (tryResolveConflicts)
            {
                ConflictResolveResult conflictResolveResult = ConflictHelper.ResolveConflicts(asmPatches, asmUtility);
                asmPatches = conflictResolveResult.Patches;
                conflictResolveMessage = conflictResolveResult.Message;
            }

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

            if (!string.IsNullOrEmpty(conflictResolveMessage))
            {
                sbResultMessage.AppendLine(conflictResolveMessage);
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
