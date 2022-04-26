using System;
using System.IO;
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
                    if (patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                    {
                        ASMEncoding.ASMEncodingUtility asmUtility = new ASMEncoding.ASMEncodingUtility(ASMEncoding.ASMEncodingMode.PSX);
                        PatchData patchData = new PatchData(new string[1] { patchFilepaths.Key }, asmUtility);
                        PatchResult result = PatchHelper.PatchPSV(patchFilepaths.Value, patchData.AllPatches, asmUtility, false);
                        Console.WriteLine((result.IsSuccess ? "(Success) " : "(ERROR) ") + result.Message);
                    }
                    else
                    {
                        string isoPath = patchFilepaths.Value;
                        PatcherLib.Datatypes.Context context = PatcherLib.Datatypes.Context.US_PSX;

                        using (Stream iso = File.Open(isoPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                        {
                            if (iso == null)
                            {
                                throw new Exception("Could not open ISO file!");
                            }

                            context = PatcherLib.Utilities.Utilities.GetContextFromIso(iso);
                        }

                        ASMEncoding.ASMEncodingMode asmEncodingMode = ASMEncoding.ASMEncodingMode.PSX;
                        if (context == PatcherLib.Datatypes.Context.US_PSP)
                            asmEncodingMode = ASMEncoding.ASMEncodingMode.PSP;

                        ASMEncoding.ASMEncodingUtility asmUtility = new ASMEncoding.ASMEncodingUtility(asmEncodingMode);
                        PatchData patchData = new PatchData(new string[1] { patchFilepaths.Key }, asmUtility);
                        PatchResult result = PatchHelper.PatchISO(isoPath, patchData.AllPatches, asmUtility, false);
                        Console.WriteLine((result.IsSuccess ? "(Success) " : "(ERROR) ") + result.Message);
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
