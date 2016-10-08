using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CLIGL
{
    /// <summary>
    /// This class contains various external interop methods and structures 
    /// used to help speed up CLIGL rendering and provide more functionality.
    /// </summary>
    internal static class Interop
    {
        /// <summary>
        /// Create a file or open a device.
        /// </summary>
        /// <param name="fileName">The name of the file or device.</param>
        /// <param name="fileAccess">The access to the file or device.</param>
        /// <param name="fileShare">The sharing mode of the file or device.</param>
        /// <param name="securityAttributes">The security attributes of the file or device.</param>
        /// <param name="creationDisposition">The action to take on a file or device that does not exist.</param>
        /// <param name="flags">The file or device attributes.</param>
        /// <param name="template">A valid handle to a template file.</param>
        /// <returns>A handle to a file or device.</returns>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template
        );

        /// <summary>
        /// Write character and color attribute data to a specified area of a
        /// console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">A handle to the console screen buffer.</param>
        /// <param name="lpBuffer">The buffer to be written to the screen buffer.</param>
        /// <param name="dwBufferSize">The size of the buffer to be written.</param>
        /// <param name="dwBufferCoord">The upper-left coordinates of the buffer.</param>
        /// <param name="lpWriteRegion">The upper-left and lower-right coordinates of the screen buffer to write to.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion
        );

        /// <summary>
        /// Represents a C++-esque character union, containing a Unicode character
        /// and an ASCII character.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public char AsciiChar;
        }

        /// <summary>
        /// Contains information about a character to be written to the console, like
        /// the character data and color data.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        /// <summary>
        /// Represents a 2D coordinate with components of the type "short".
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;
        }

        /// <summary>
        /// Represents a 2D coordinate with components of the type "int".
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// Represents a small rectangle, with components of the type "short".
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        /// <summary>
        /// Represents a normal rectangle, with components of the type "int".
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
