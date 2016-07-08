using System;
using System.IO;
using System.Text;

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
                renderingBuffer.Render();
            }
            catch(IOException) { }
            catch(ArgumentOutOfRangeException) { }
        }
    }
}
