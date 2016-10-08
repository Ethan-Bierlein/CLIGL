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
        /// Render the window. This function will render the provided rendering buffer
        /// to the console window output.
        /// </summary>
        public void Render(RenderingBuffer renderingBuffer)
        {
            try
            {
                Console.SetWindowSize(this.Width, this.Height);
                Console.SetBufferSize(this.Width, this.Height);

                if(renderingBuffer.BufferWidth == this.Width && renderingBuffer.BufferHeight == this.Height)
                {
                    renderingBuffer.Render(this.ConsoleHandle);
                }
                else
                {
                    throw new Exception("The dimensions of the provided rendering buffer do not match the dimensions of the window.");
                }
            }
            catch(IOException) { }
            catch(ArgumentOutOfRangeException) { }
        }
    }
}
