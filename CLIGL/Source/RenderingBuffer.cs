﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

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
        /// Set a rectangle of pixels into the current pixel buffer.
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
        /// Set a texture into the current pixel buffer.
        /// </summary>
        /// <param name="x">The x coordinate of the texture.</param>
        /// <param name="y">The y coordinate of the texture.</param>
        /// <param name="texture">The texture to set.</param>
        public void SetTexture(int x, int y, RenderingTexture texture)
        {
            for(int xi = 0; xi < texture.Width; xi++)
            {
                for(int yi = 0; yi < texture.Height; yi++)
                {
                    this.SetPixel(x + xi, y + yi, texture.RenderingPixels[xi, yi]);
                }
            }
        }

        /// <summary>
        /// Set a string into the current pixel buffer.
        /// </summary>
        /// <param name="x">The x position of the string.</param>
        /// <param name="y">The y position of the string.</param>
        /// <param name="text">The text to set.</param>
        /// <param name="foregroundColor">The foreground color of the string.</param>
        /// <param name="backgroundColor">The background color of the string.</param>
        public void SetString(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            for(int i = 0; i < text.Length; i++)
            {
                this.SetPixel(x + i, y, new RenderingPixel(text[i], foregroundColor, backgroundColor));
            }
        }

        /// <summary>
        /// Write the pixel buffer to the C# command-line interface, which will
        /// produce specific output based on the contents of the pixel buffer.
        /// </summary>
        /// <param name="consoleHandle">The handle to the current console window.</param>
        public void Render(SafeFileHandle consoleHandle)
        {
            Interop.CharInfo[] characterBuffer = new Interop.CharInfo[this.BufferWidth * this.BufferHeight];
            Interop.SmallRect windowRectangle = new Interop.SmallRect()
            {
                Left = 0,
                Top = 0,
                Right = (short)this.BufferWidth,
                Bottom = (short)this.BufferHeight
            };

            for(int i = 0; i < characterBuffer.Length; i++)
            {
                characterBuffer[i].Attributes = (short)this.PixelBuffer[i].ForegroundColor;
                characterBuffer[i].Char.AsciiChar = this.PixelBuffer[i].Character;
            }

            Interop.WriteConsoleOutput(
                consoleHandle,
                characterBuffer,
                new Interop.Coord() { X = (short)this.BufferWidth, Y = (short)this.BufferHeight },
                new Interop.Coord() { X = 0, Y = 0 },
                ref windowRectangle
            );
        }
    }
}
