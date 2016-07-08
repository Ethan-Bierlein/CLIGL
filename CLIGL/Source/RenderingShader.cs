using System;

namespace CLIGL
{
    /// <summary>
    /// This class represents a CLIGL shader. A shader in CLIGL is essentially
    /// an anonymous function that is applied to every pixel in a rendering buffer,
    /// or every pixel in a rendering buffer that is inside the specified region.
    /// </summary>
    public class RenderingShader
    {
        public int RegionX { get; set; }
        public int RegionY { get; set; }
        public int RegionWidth { get; set; }
        public int RegionHeight { get; set; }
        public Func<int, int, RenderingPixel, RenderingPixel> Function { get; set; }

        /// <summary>
        /// Constructor for the RenderingShaderClass.
        /// </summary>
        /// <param name="regionX">The x coordinate of the region the shader affects.</param>
        /// <param name="regionY">The y coordinate of the region the shader affects.</param>
        /// <param name="regionWidth">The width of the region the shader affects.</param>
        /// <param name="regionHeight">The height of the region the shader affects.</param>
        /// <param name="function">The shader's function.</param>
        public RenderingShader(int regionX, int regionY, int regionWidth, int regionHeight, Func<int, int, RenderingPixel, RenderingPixel> function)
        {
            this.RegionX = regionX;
            this.RegionY = regionY;
            this.RegionWidth = regionWidth;
            this.RegionHeight = regionHeight;
            this.Function = function;
        }

        /// <summary>
        /// Execute the shader, and return an output pixel based on what the shader
        /// does to, and with the inputs.
        /// </summary>
        /// <param name="x">The x coordinate the shader is executing at.</param>
        /// <param name="y">The y coordinate the shader is executing at.</param>
        /// <param name="inputPixel">The input pixel at the x and y coordinates.</param>
        /// <returns>An output pixel, or the input pixel, if the input pixel is out of bounds.</returns>
        public RenderingPixel Execute(int x, int y, RenderingPixel inputPixel)
        {
            if(x >= this.RegionX && x <= this.RegionX + this.RegionWidth && y >= this.RegionY && y <= this.RegionY + this.RegionHeight)
            {
                return this.Function(x, y, inputPixel);
            }
            else
            {
                return inputPixel;
            }
        }
    }
}
