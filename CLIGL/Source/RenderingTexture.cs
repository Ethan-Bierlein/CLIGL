using System;
using System.Runtime.CompilerServices;

namespace CLIGL
{
    /// <summary>
    /// This class is intended to represented a rendering texture, which is a
    /// 2-dimensional array of Pixel objects.
    /// </summary>
    public class RenderingTexture
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public RenderingPixel[,] RenderingPixels { get; set; }

        /// <summary>
        /// Constructor for the RenderingTexture class.
        /// </summary>
        /// <param name="width">The width of the rendering texture.</param>
        /// <param name="height">The height of the rendering texture.</param>
        public RenderingTexture(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.RenderingPixels = new RenderingPixel[this.Width, this.Height];

            for(int x = 0; x < this.Width; x++)
            {
                for(int y = 0; y < this.Height; y++)
                {
                    this.RenderingPixels[x, y] = RenderingPixel.EmptyPixel;
                }
            }
        }

        /// <summary>
        /// Apply a shader to the current rendering texture, which may change the contents
        /// of the texture, changing how the texture looks when it is rendered.
        /// </summary>
        /// <param name="renderingShader">The shader to apply.</param>
        public void ApplyShader(RenderingShader renderingShader)
        {
            for(int x = 0; x < this.Width; x++)
            {
                for(int y = 0; y < this.Height; y++)
                {
                    this.RenderingPixels[x, y] = renderingShader.Execute(x, y, this.RenderingPixels[x, y]);
                }
            }
        }

        /// <summary>
        /// Set a pixel into the texture at the specified coordinates.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <param name="pixel">The pixel to set.</param>
        public void SetPixel(int x, int y, RenderingPixel pixel)
        {
            if(x >= 0 && x < this.Width && y >= 0 && y < this.Height)
            {
                this.RenderingPixels[x, y] = pixel;
            }
            else
            {
                throw new Exception(string.Format("The provided coordinates '({0}, {1})' are out of range.", x, y));
            }
        }
    }
}
