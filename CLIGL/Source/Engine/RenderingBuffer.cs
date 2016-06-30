using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace CLIGL
{
    /// <summary>
    /// This class represents a CLIGL render buffer, which is responsible for 
    /// managing what will currently be drawn to the screen.
    /// </summary>
    public class RenderingBuffer
    {
        public int BufferWidth { get; set; }
        public int BufferHeight { get; set; }
        public int BufferSize { get; set; }
        public char[] TextBuffer { get; set; }
        public ConsoleColor[] ForegroundColorBuffer { get; set; }
        public ConsoleColor[] BackgroundColorBuffer { get; set; }

        /// <summary>
        /// Constructor for the RenderingBuffer class.
        /// </summary>
        /// <param name="bufferWidth">The width of the text and color buffers.</param>
        /// <param name="bufferHeight">The height of the text and color buffers.</param>
        public RenderingBuffer(int bufferWidth, int bufferHeight)
        {
            this.BufferWidth = bufferWidth;
            this.BufferHeight = bufferHeight;
            this.BufferSize = this.BufferWidth * this.BufferHeight;

            this.TextBuffer = new char[this.BufferSize];
            this.ForegroundColorBuffer = new ConsoleColor[this.BufferSize];
            this.BackgroundColorBuffer = new ConsoleColor[this.BufferSize];
        }

        /// <summary>
        /// Convert a 1-dimensional index to a 2-dimensional index in the form 
        /// of a int array with two values.
        /// </summary>
        /// <param name="index">The index to convert/</param>
        /// <returns>A 2-dimensional index.</returns>
        public int[] Convert1DIndexTo2DIndex(int index)
        {
            int x = index % this.BufferWidth;
            int y = index / this.BufferWidth;
            return new int[] { x, y };
        }

        /// <summary>
        /// Convert a 2-dimensional index to a 1-dimensional index.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A 1-dimensional index.</returns>
        public int Convert2DIndexTo1DIndex(int x, int y)
        {
            return x + (y * this.BufferWidth);
        }

        /// <summary>
        /// Clear the text buffer with a specified clear character.
        /// </summary>
        /// <param name="clearCharacter">The character to clear the text buffer with.</param>
        public void ClearTextBuffer(char clearCharacter)
        {
            for(int i = 0; i < this.BufferSize; i++)
            {
                this.TextBuffer[i] = clearCharacter;
            }
        }

        /// <summary>
        /// Clear the foreground color buffer with a specified clear color.
        /// </summary>
        /// <param name="clearColor">The color to clear the buffer with.</param>
        public void ClearForegroundColorBuffer(ConsoleColor clearColor)
        {
            for(int i = 0; i < this.BufferSize; i++)
            {
                this.ForegroundColorBuffer[i] = clearColor;
            }
        }

        /// <summary>
        /// Clear the background color buffer with a specified clear color.
        /// </summary>
        /// <param name="clearColor">The color to clear the buffer with.</param>
        public void ClearBackgroundColorBuffer(ConsoleColor clearColor)
        {
            for(int i = 0; i < this.BufferSize; i++)
            {
                this.BackgroundColorBuffer[i] = clearColor;
            }
        }

        /// <summary>
        /// Set a pixel into the current character, foreground color and background 
        /// color buffers.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <param name="character">The character representing the pixel.</param>
        /// <param name="foregroundColor">The foreground color of the pixel.</param>
        /// <param name="backgroundColor">The background color of the pixel.</param>
        public void SetPixel(int x, int y, char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if(x >= 0 && y >= 0 && x < this.BufferWidth && y < this.BufferHeight)
            {
                int pixelIndex = this.Convert2DIndexTo1DIndex(x, y);

                this.TextBuffer[pixelIndex] = character;
                this.ForegroundColorBuffer[pixelIndex] = foregroundColor;
                this.BackgroundColorBuffer[pixelIndex] = backgroundColor;
            }
        }

        /// <summary>
        /// Set a rectangle of pixels into the current chararacter, foreground color
        /// and background color buffers.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="character">The character representing the rectangle's pixels.</param>
        /// <param name="foregroundColor">The foreground color of the rectangle.</param>
        /// <param name="backgroundColor">The background color of the rectangle.</param>
        public void SetRectangle(int x, int y, int width, int height, char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            for(int xi = x; xi < x + width; xi++)
            {
                for(int yi = y; yi < y + height; yi++)
                {
                    this.SetPixel(xi, yi, character, foregroundColor, backgroundColor);
                }
            }
        }

        /// <summary>
        /// Write the text and color buffers to the C# command-line interface.
        /// </summary>
        public void Render()
        {
            for(int i = 0; i < this.BufferSize - 1; i++)
            {
                int[] cursorPosition = this.Convert1DIndexTo2DIndex(i);
                Console.SetCursorPosition(cursorPosition[0], cursorPosition[1]);

                Console.ForegroundColor = this.ForegroundColorBuffer[i];
                Console.BackgroundColor = this.BackgroundColorBuffer[i];
                Console.Write(this.TextBuffer[i]);
            }
        }
    }
}
