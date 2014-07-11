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
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode( UnhandledExceptionMode.CatchException );
                Application.ThreadException += Application_ThreadException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                mainForm = new MainForm();
                Application.Run( mainForm );
                Application.ThreadException -= Application_ThreadException;
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

        #endregion Methods

    }
}