using System;
using System.Runtime.CompilerServices;

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
        public RenderingPixel[] PixelBuffer { get; set; }

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

            this.PixelBuffer = new RenderingPixel[this.BufferSize];

            for(int i = 0; i < this.BufferSize - 1; i++)
            {
                this.PixelBuffer[i] = RenderingPixel.EmptyPixel;
            }
        }

        /// <summary>
        /// Convert a 1-dimensional index to a 2-dimensional index in the form 
        /// of a int array with two values.
        /// </summary>
        /// <param name="index">The index to convert/</param>
        /// <returns>A 2-dimensional index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Convert2DIndexTo1DIndex(int x, int y)
        {
            return x + (y * this.BufferWidth);
        }

        /// <summary>
        /// Apply a shader to the current rendering buffer, which may change the contents
        /// of the buffer, changing how the buffer looks when it is rendered.
        /// </summary>
        /// <param name="renderingShader">The shader to apply.</param>
        public void ApplyShader(RenderingShader renderingShader)
        {
            for(int i = 0; i < this.BufferSize - 1; i++)
            {
                int[] pixelPosition = this.Convert1DIndexTo2DIndex(i);
                this.PixelBuffer[i] = renderingShader.Execute(pixelPosition[0], pixelPosition[1], this.PixelBuffer[i]);
            }
        }

        /// <summary>
        /// Clear the current active pixel buffer with a clear character, a
        /// foreground clear color and a background clear color.
        /// </summary>
        /// <param name="clearPixel">The pixel to clear the pixel buffer with.</param>
        public void ClearPixelBuffer(RenderingPixel clearPixel)
        {
            for(int i = 0; i < this.BufferSize - 1; i++)
            {
                this.PixelBuffer[i] = clearPixel;
            }
        }

        /// <summary>
        /// Set a pixel into the current pixel buffer using a 1D index.
        /// </summary>
        /// <param name="index">The index of the pixel.</param>
        /// <param name="fillPixel">The pixel to fill the index with.</param>
        public void SetPixel(int index, RenderingPixel fillPixel)
        {
            if(index < this.BufferSize)
            {
                this.PixelBuffer[index] = fillPixel;
            }
        }

        /// <summary>
        /// Set a pixel into the current pixel buffer using a 2D index.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <param name="fillPixel">The pixel to fill the coordinates with.</param>
        public void SetPixel(int x, int y, RenderingPixel fillPixel)
        {
            if(x >= 0 && y >= 0 && x < this.BufferWidth && y < this.BufferHeight)
            {
                int pixelIndex = this.Convert2DIndexTo1DIndex(x, y);
                this.PixelBuffer[pixelIndex] = fillPixel;
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
        /// <param name="fillPixel">The pixel to fill the rectangle with.</param>
        public void SetRectangle(int x, int y, int width, int height, RenderingPixel fillPixel)
        {
            for(int xi = x; xi < x + width; xi++)
            {
                for(int yi = y; yi < y + height; yi++)
                {
                    this.SetPixel(xi, yi, fillPixel);
                }
            }
        }

        /// <summary>
        /// Write the pixel buffer to the C# command-line interface, which will
        /// produce specific output based on the contents of the pixel buffer.
        /// </summary>
        public void Render()
        {
            for(int i = 0; i < this.BufferSize - 1; i++)
            {
                int[] cursorPosition = this.Convert1DIndexTo2DIndex(i);
                Console.SetCursorPosition(cursorPosition[0], cursorPosition[1]);

                Console.ForegroundColor = this.PixelBuffer[i].ForegroundColor;
                Console.BackgroundColor = this.PixelBuffer[i].BackgroundColor;
                Console.Write(this.PixelBuffer[i].Character);
            }
        }
    }
}
