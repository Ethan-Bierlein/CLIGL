using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CLIGL
{
    /// <summary>
    /// This class represents a CLIGL "window", which contains the various components
    /// required for window rendering and is responsible for managing and updating them.
    /// </summary>
    public class RenderingWindow
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SafeFileHandle ConsoleHandle { get; set; }

        /// <summary>
        /// Constructor for the Window class.
        /// </summary>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        public RenderingWindow(string title, int width, int height)
        {
            this.Title = title;
            this.Width = width;
            this.Height = height;

            Console.Title = this.Title;
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;
            Console.TreatControlCAsInput = true;

            Console.SetWindowSize(this.Width, this.Height);
            Console.SetBufferSize(this.Width, this.Height);

            this.ConsoleHandle = Interop.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if(this.ConsoleHandle.IsInvalid)
            {
                throw new Exception("Creating the console window handle failed.");
            }
        }

        /// <summary>
        /// Get the position of the cursor relative to the current window. The
        /// coordinates are returned as an array containing two short values.
        /// </summary>
        public short[] GetCursorPosition()
        {
            Interop.Coord cursorPosition;
            Interop.GetCursorPos(out cursorPosition);
            return new short[] { cursorPosition.X, cursorPosition.Y };
        }

        /// <summary>
        /// Render the window. This function will render the provided rendering buffer
        /// to the console window output.
        /// </summary>
        public void Render(RenderingBuffer renderingBuffer)
        {
            try
            {
                Console.SetWindowSize(this.Width, this.Height);
                Console.SetBufferSize(this.Width, this.Height);

                Interop.CharInfo[] characterBuffer = new Interop.CharInfo[this.Width * this.Height];
                Interop.SmallRect windowRectangle = new Interop.SmallRect() {
                    Left = 0,
                    Top = 0,
                    Right = (short)this.Width,
                    Bottom = (short)this.Height
                };

                for(int i = 0; i < characterBuffer.Length; i++)
                {
                    characterBuffer[i].Attributes = (short)renderingBuffer.PixelBuffer[i].ForegroundColor;
                    characterBuffer[i].Char.AsciiChar = renderingBuffer.PixelBuffer[i].Character;
                }

                Interop.WriteConsoleOutput(
                    this.ConsoleHandle,
                    characterBuffer,
                    new Interop.Coord() { X = (short)this.Width, Y = (short)this.Height },
                    new Interop.Coord() { X = 0, Y = 0 },
                    ref windowRectangle
                );
            }
            catch(IOException) { }
            catch(ArgumentOutOfRangeException) { }
        }
    }
}
