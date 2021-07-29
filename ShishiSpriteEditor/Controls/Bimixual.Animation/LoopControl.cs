//Copyright 2009 Derek Duban
//This file is part of the Bimixual Animation Library.
//
//Bimixual Animation is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//Bimixual Animation is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with Bimixual Animation Library.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Bimixual.Animation
{
    /// <summary>
    /// Controls the main loop of the app and it is always going.
    /// Apps using this class should call SetAction() so the loop
    /// can do something useful at each iteration.
    /// </summary>
    public class LoopControl
    {
        struct MSG
        {
            IntPtr hwnd;//HWND hwnd;
            uint message;//UINT message;
            long wParam;//WPARAM wParam;
            long lParam;//LPARAM lParam;
            UInt32 time;//DWORD time;
            IntPtr pt;//POINT pt;
        };
        // DllImports are useful for .net and for mono+windows		
        //[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport( "user32", CharSet = CharSet.Auto )]
        private static extern bool PeekMessage( out MSG msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags );
        [DllImport( "user32.dll", SetLastError = true )]
        private static extern int GetMessage(
                                       ref MSG lpMsg,
                                   Int32 hwnd,
                           Int32 wMsgFilterMin,
                           Int32 wMsgFilterMax );
        [DllImport( "user32.dll", SetLastError = true )]
        private static extern bool TranslateMessage( ref MSG lpMsg );

        [DllImport( "user32.dll", SetLastError = true )]
        private static extern Int32 DispatchMessage( ref MSG lpMsg );

        private enum PeekMessageOption
        {
            PM_NOREMOVE = 0,
            PM_REMOVE
        }
        public delegate void LoopAction();      // defines the function ptr signature
        private static LoopAction loopAction = DefaultAction;   // our ptr to the function called in each frame iteration

        private static Control control;   // The form connected to this controller

        /// <summary>
        /// If assigned, then program will sleep between frames instead of looping 
        /// </summary>
        private static FpsTimer fpsTimer = null; 
        public static FpsTimer FpsTimer 
        { 
            set { fpsTimer = value; } 
        }

        public static void Stop()
        {
            Application.Idle -= Application_Idle;
        }

        public static void Start()
        {
            Application.Idle += new EventHandler( Application_Idle );
            loopAction = loopAction ?? DefaultAction;
        }

        /// <summary>
        /// Determine if there are messages in the queue for the current app
        /// </summary>
        static bool AppStillIdle
        {
            get
            {
                MSG msg;
                return !PeekMessage( out msg, IntPtr.Zero, 0, 0, 0 );
            }
        }

        /// <summary>
        /// Calls the LoopAction function every time the app goes idle.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        static void Application_Idle( object sender, EventArgs e )
        {
            // See explanation of AppStillIdle
            // https://blogs.msdn.com/tmiller/archive/2005/05/05/415008.aspx
            if ( control != null && !control.Disposing && loopAction != null )
            {
                if ( IsRunningOnMono() == false )
                {
                    while ( AppStillIdle )
                        CallLoopAction();
                }
                else
                    CallLoopAction();
            }
        }

        /// <summary>
        /// Implements a thread sleep after the action so we don't use up
        /// 100% CPU 
        /// </summary>
        static void CallLoopAction()
        {
            try
            {
                if ( fpsTimer != null && fpsTimer.Next() == false ) return;

                long startTicks = DateTime.Now.Ticks;
                loopAction();
                long diffTicks = DateTime.Now.Ticks - startTicks;
                if ( fpsTimer != null && fpsTimer.WaitCount > diffTicks )
                {
                    Thread.Sleep( (int)
                        (
                       (int)Math.Round( (double)( fpsTimer.WaitCount - diffTicks ) / 10000 ) )
                        );
                }
            }
            catch ( Exception e )
            {
                System.Console.WriteLine( e.Message );
                throw e;
            }

        }

        /// <summary>
        /// A do-nothing method that is the default thing to do if loopAction
        /// has not been set with SetAction()
        /// </summary>
        static void DefaultAction()
        {
        }

        /// <summary>
        /// Determine if this app is using the Mono runtime
        /// </summary>
        /// <returns>true if running Mono</returns>
        static public bool IsRunningOnMono()
        {
            return Type.GetType( "Mono.Runtime" ) != null;
        }

        /// <summary>
        /// Determine if we are running on Linux.
        /// </summary>
        /// <returns></returns>
        static bool IsLinux()
        {
            bool rtn;

            int p = (int)Environment.OSVersion.Platform;
            if ( ( p == 4 ) || ( p == 6 ) || ( p == 128 ) )
            {
                rtn = true;
            }
            else
            {
                rtn = false;
            }

            return rtn;
        }

        /// <summary>
        /// Implementors should call this so the main loop has something
        /// to do.
        /// </summary>
        /// <param name="p_la">LoopAction method to run at each iteration</param>
        static public void SetAndStartAction( Control control, LoopAction p_la )
        {
            SetAction( control, p_la );
            Start();
        }

        static public void SetAction( Control control, LoopAction p_la )
        {
            LoopControl.control = control;
            control.Disposed += control_Disposed;
            loopAction = p_la;
        }

        static void control_Disposed( object sender, EventArgs e )
        {
            if (control != null)
            {
                control.Disposed -= control_Disposed;
                control = null;
            }
            Stop();
        }
    }
}
