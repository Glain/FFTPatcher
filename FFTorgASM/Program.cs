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

                ASMEncoding.ASMEncodingUtility asmUtility = new ASMEncoding.ASMEncodingUtility(ASMEncoding.ASMEncodingMode.PSX);
                PatchData patchData = new PatchData(new string[1] { patchFilepaths.Key }, asmUtility);

                if (patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                {
                    patchData.PatchAllSaveState(asmUtility, patchFilepaths.Value);
                }
                else
                {
                    patchData.PatchAllISO(asmUtility, patchFilepaths.Value);
                }

                return true;
            }
        }
    }
}
