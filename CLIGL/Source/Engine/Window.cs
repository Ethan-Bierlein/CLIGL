using System;
using System.IO;
using System.Text;
using System.Threading;

namespace CLIGL
{
    /// <summary>
    /// This class represents a CLIGL "window", which contains the various components
    /// required for window rendering and is responsible for managing and updating them.
    /// </summary>
    public class Window
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RenderingBuffer WindowRenderingBuffer { get; set; }

        /// <summary>
        /// Constructor for the Window class.
        /// </summary>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        public Window(string title, int width, int height, RenderingBuffer windowRenderingBuffer)
        {
            this.Title = title;
            this.Width = width;
            this.Height = height;
            this.WindowRenderingBuffer = windowRenderingBuffer;

            Console.Title = this.Title;
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;

            Console.SetWindowSize(this.Width, this.Height);
            Console.SetBufferSize(this.Width, this.Height);
        }

        /// <summary>
        /// Render the window. This function will render the current rendering buffer
        /// to the console window, and resize the console window to the original width
        /// and height.
        /// </summary>
        /// <param name="sleepTime">How long the thread should sleep, in milliseconds.</param>
        public void Render()
        {
            this.WindowRenderingBuffer.Render();

            try
            {
                Console.SetWindowSize(this.Width, this.Height);
                Console.SetBufferSize(this.Width, this.Height);
            }
            catch(IOException) { }
            catch(ArgumentOutOfRangeException) { }
        }
    }
}
