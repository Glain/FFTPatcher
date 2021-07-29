/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using FFTPatcher.Editors;
using PatcherLib;

namespace FFTPatcher
{
    static class Program
    {
		#region Private Methods (1) 

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!HandleCommandLinePatch(args))
                Application.Run(new MainForm(args));
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private static bool HandleCommandLinePatch(string[] args)
        {
            System.Collections.Generic.KeyValuePair<string, string> patchFilepaths = PatcherLib.Utilities.Utilities.GetPatchFilepaths(args, ".fftpatch");

            if ((string.IsNullOrEmpty(patchFilepaths.Key)) || (string.IsNullOrEmpty(patchFilepaths.Value)))
            {
                return false;
            }
            else
            {
                Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(patchFilepaths.Key);

                try
                {
                    FFTPatch fftPatch = new FFTPatch();
                    fftPatch.LoadPatch(patchFilepaths.Key);

                    if (fftPatch.Context == PatcherLib.Datatypes.Context.US_PSP)
                    {
                        if (!patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                        {
                            PspIso.PatchISOSimple(fftPatch, patchFilepaths.Value);
                        }
                    }
                    else
                    {
                        if (patchFilepaths.Value.ToLower().Trim().EndsWith(".psv"))
                        {
                            PsxIso.PatchPsxSavestateSimple(fftPatch, patchFilepaths.Value);
                        }
                        else
                        {
                            PsxIso.PatchPsxIsoSimple(fftPatch, patchFilepaths.Value);
                        }
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

		#endregion Private Methods 
    }
}
