using System;
using System.Windows.Forms;

namespace FFTorgASM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!HandleCommandLinePatch(args))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private static bool HandleCommandLinePatch(string[] args)
        {
            System.Collections.Generic.KeyValuePair<string, string> patchFilepaths = PatcherLib.Utilities.Utilities.GetPatchFilepaths(args, ".xml");

            if ((string.IsNullOrEmpty(patchFilepaths.Key)) || (string.IsNullOrEmpty(patchFilepaths.Value)))
            {
                return false;
            }
            else
            {
                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                //while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(100);
                AttachConsole(ATTACH_PARENT_PROCESS);

                try
                {
                    ASMEncoding.ASMEncodingUtility asmUtility = new ASMEncoding.ASMEncodingUtility(ASMEncoding.ASMEncodingMode.PSX);
                    PatchData patchData = new PatchData(new string[1] { patchFilepaths.Key }, asmUtility);

                    if (patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                    {
                        PatcherLib.Iso.PatchPsxSaveStateResult patchResult = patchData.PatchAllSaveState(asmUtility, patchFilepaths.Value);
                        System.Text.StringBuilder sbResultMessage = new System.Text.StringBuilder();

                        bool hasUnsupportedFiles = (patchResult.UnsupportedFiles.Count > 0);
                        bool hasAbsentFiles = (patchResult.AbsentFiles.Count > 0);

                        if (hasUnsupportedFiles)
                        {
                            sbResultMessage.AppendLine("Files not supported for savestate patching:");
                            foreach (PatcherLib.Iso.PsxIso.Sectors sector in patchResult.UnsupportedFiles)
                            {
                                sbResultMessage.AppendFormat("\t{0}{1}", PatcherLib.Iso.PsxIso.GetSectorName(sector), Environment.NewLine);
                            }
                            sbResultMessage.AppendLine();
                        }
                        if (hasAbsentFiles)
                        {
                            sbResultMessage.AppendLine("Files not present in savestate:");
                            foreach (PatcherLib.Iso.PsxIso.Sectors sector in patchResult.AbsentFiles)
                            {
                                sbResultMessage.AppendFormat("\t{0}{1}", PatcherLib.Iso.PsxIso.GetSectorName(sector), Environment.NewLine);
                            }
                            sbResultMessage.AppendLine();
                        }

                        if (hasUnsupportedFiles || hasAbsentFiles)
                        {
                            Console.WriteLine(sbResultMessage.ToString());
                        }
                    }
                    else
                    {
                        patchData.PatchAllISO(asmUtility, patchFilepaths.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                return true;
            }
        }
    }
}
