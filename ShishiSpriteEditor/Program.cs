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
using System.IO;

namespace FFTPatcher.SpriteEditor
{
    static class Program
    {

        #region Methods (1)


        static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (!HandleCommandLinePatch(args))
                {
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Application.ThreadException += Application_ThreadException;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    mainForm = new MainForm();
                    Application.Run(mainForm);
                    Application.ThreadException -= Application_ThreadException;
                }
            }
            catch (Exception e)
            {
                HandleException( e );
            }
        }

        static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
        {
            HandleException( e.Exception );
        }

        static void HandleException( Exception e )
        {
            if (mainForm != null)
            {
                System.Reflection.FieldInfo fi = typeof( MainForm ).GetField( "currentStream", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
                if (fi != null)
                {
                    Stream stream = fi.GetValue( mainForm ) as Stream;
                    if (stream != null)
                    {
                        stream.Flush();
                    }
                }

            }
            PatcherLib.MyMessageBox.Show( e.ToString(), "Error" );
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private static bool HandleCommandLinePatch(string[] args)
        {
            //while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(100);
            System.Collections.Generic.KeyValuePair<string, string> patchFilepaths = PatcherLib.Utilities.Utilities.GetPatchFilepathAndDirectory(args);

            bool isExpand = false;
            for (int index = 0; index < args.Length; index++)
            {
                if (args[index].ToLower().Trim().Equals("-expand"))
                {
                    isExpand = true;
                    break;
                }
            }

            if ((string.IsNullOrEmpty(patchFilepaths.Key)) || (string.IsNullOrEmpty(patchFilepaths.Value)))
            {
                return false;
            }
            else
            {
                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                //Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(patchFilepaths.Key);

                try
                {
                    string inputDirectory = patchFilepaths.Key;
                    string spritesDirectory = Path.Combine(inputDirectory, "sprites");
                    string imagesDirectory = Path.Combine(inputDirectory, "images");
                    string isoPath = patchFilepaths.Value;
                    System.Text.StringBuilder sbOutput = new System.Text.StringBuilder();

                    using (Stream iso = File.Open(isoPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    {
                        if (iso == null)
                        {
                            throw new Exception("Could not open ISO file!");
                        }

                        bool hasSpritesDirectory = Directory.Exists(spritesDirectory);
                        bool hasImagesDirectory = Directory.Exists(imagesDirectory);
                        bool hasPatchFilepath = false;
                        string patchFilepath = null;

                        //PatcherLib.Datatypes.Context context = PatcherLib.Utilities.Utilities.GetContextFromIso(iso);
                        string[] files = Directory.GetFiles(inputDirectory, "*.shishipatch");
                        if ((files != null) && (files.Length > 0))
                        {
                            hasPatchFilepath = true;
                            patchFilepath = files[0];
                        }

                        AllSprites sprites = null;
                        if (hasPatchFilepath || hasSpritesDirectory || isExpand)
                        {
                            sprites = AllSprites.FromIso(iso, isExpand);
                        }

                        if (hasPatchFilepath)
                        {
                            sprites.ApplyShishiPatchBytes(iso, File.ReadAllBytes(patchFilepath));
                            sbOutput.AppendLine("Applied patch file: " + patchFilepath);
                        }
                        if (Directory.Exists(spritesDirectory))
                        {
                            AllSprites.AllSpritesDoWorkResult result = sprites.LoadAllSprites(iso, spritesDirectory);
                            bool isSuccess = (result.DoWorkResult == AllSprites.AllSpritesDoWorkResult.Result.Success);
                            sbOutput.AppendLine(isSuccess ? (result.ImagesProcessed.ToString() + " sprites imported.") : "Failed to import sprites!");
                        }
                        if (Directory.Exists(imagesDirectory))
                        {
                            AllOtherImages images = AllOtherImages.FromIso(iso);
                            AllOtherImages.AllImagesDoWorkResult result = images.LoadAllImages(iso, imagesDirectory);
                            bool isSuccess = (result.DoWorkResult == AllOtherImages.AllImagesDoWorkResult.Result.Success);
                            sbOutput.AppendLine(isSuccess ? (result.ImagesProcessed.ToString() + " images imported.") : "Failed to import images!");
                        }
                    }

                    AttachConsole(ATTACH_PARENT_PROCESS);
                    Console.WriteLine(sbOutput.ToString());
                }
                catch (Exception ex)
                {
                    AttachConsole(ATTACH_PARENT_PROCESS);
                    Console.WriteLine("Error: " + ex.Message);
                }

                return true;
            }
        }

        #endregion Methods
    }
}