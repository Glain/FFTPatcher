using System;

using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace PatcherLib
{
    public static class MyMessageBox
    {
        public static DialogResult Show( string text ) { return MessageBox.Show( text ); }
        public static DialogResult Show(
            string text,
            string caption ) { return MessageBox.Show( text, caption ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons ) { return MessageBox.Show( text, caption, buttons ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon ) { return MessageBox.Show( text, caption, buttons, icon ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool displayHelpButton ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, displayHelpButton ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            string keyword ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword ); }
        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            object param ) { return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param ); }

        public static DialogResult Show( Form owner, string text ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text ); }
        public static DialogResult Show( Form owner, string text, string caption ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton, options ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword ); }
        public static DialogResult Show( Form owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param ) { PrepToCenterMessageBoxOnForm( owner ); return MessageBox.Show( text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param ); }

        private static void PrepToCenterMessageBoxOnForm( Form form )
        {
            MessageBoxCenterHelper helper = new MessageBoxCenterHelper();
            helper.Prep( form );
        }

        private class MessageBoxCenterHelper
        {
            private int messageHook;
            private IntPtr parentFormHandle;

            public void Prep( Form form )
            {
                NativeMethods.CenterMessageCallBackDelegate callBackDelegate = new NativeMethods.CenterMessageCallBackDelegate( CenterMessageCallBack );
                GCHandle.Alloc( callBackDelegate );

                parentFormHandle = form.Handle;
                messageHook = NativeMethods.SetWindowsHookEx( 5, callBackDelegate, new IntPtr( NativeMethods.GetWindowLong( parentFormHandle, -6 ) ), NativeMethods.GetCurrentThreadId() ).ToInt32();
            }

            private int CenterMessageCallBack( int message, int wParam, int lParam )
            {
                NativeMethods.RECT formRect;
                NativeMethods.RECT messageBoxRect;
                int xPos;
                int yPos;

                if (message == 5)
                {
                    NativeMethods.GetWindowRect( parentFormHandle, out formRect );
                    NativeMethods.GetWindowRect( new IntPtr( wParam ), out messageBoxRect );

                    xPos = (int)((formRect.Left + (formRect.Right - formRect.Left) / 2) - ((messageBoxRect.Right - messageBoxRect.Left) / 2));
                    yPos = (int)((formRect.Top + (formRect.Bottom - formRect.Top) / 2) - ((messageBoxRect.Bottom - messageBoxRect.Top) / 2));

                    NativeMethods.SetWindowPos( wParam, 0, xPos, yPos, 0, 0, 0x1 | 0x4 | 0x10 );
                    NativeMethods.UnhookWindowsHookEx( messageHook );
                }

                return 0;
            }
        }

        private static class NativeMethods
        {
            internal struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            internal delegate int CenterMessageCallBackDelegate( int message, int wParam, int lParam );

            [DllImport( "user32.dll" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool UnhookWindowsHookEx( int hhk );

            [DllImport( "user32.dll", SetLastError = true )]
            internal static extern int GetWindowLong( IntPtr hWnd, int nIndex );

            [DllImport( "kernel32.dll" )]
            internal static extern int GetCurrentThreadId();

            [DllImport( "user32.dll", SetLastError = true )]
            internal static extern IntPtr SetWindowsHookEx( int hook, CenterMessageCallBackDelegate callback, IntPtr hMod, int dwThreadId );

            [DllImport( "user32.dll" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool SetWindowPos( int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags );

            [DllImport( "user32.dll" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool GetWindowRect( IntPtr hWnd, out RECT lpRect );
        }
    }
}