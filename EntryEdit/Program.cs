using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EntryEdit
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
                Application.Run(new MainForm(args));
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private static bool HandleCommandLinePatch(string[] args)
        {
            System.Collections.Generic.KeyValuePair<string, string> patchFilepaths = PatcherLib.Utilities.Utilities.GetPatchFilepaths(args, ".eepatch");

            if ((string.IsNullOrEmpty(patchFilepaths.Key)) || (string.IsNullOrEmpty(patchFilepaths.Value)))
            {
                return false;
            }
            else
            {
                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                //while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(100);

                try
                {
                    Context context = Context.US_PSX;

                    DataHelper dataHelper = new DataHelper(context);
                    EntryData patchData = PatchHelper.GetEntryDataFromPatchFile(patchFilepaths.Key, dataHelper);

                    if (patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                    {
                        PatchHelper.PatchPsxSaveState(patchData, patchFilepaths.Value, dataHelper);
                    }
                    else
                    {
                        PatchHelper.PatchISO(patchData, patchFilepaths.Value, context, dataHelper);
                    }
                }
                catch (Exception ex)
                {
                    AttachConsole(ATTACH_PARENT_PROCESS);
                    Console.WriteLine("Error: " + ex.Message);
                }

                return true;
            }
        }
    }
}
